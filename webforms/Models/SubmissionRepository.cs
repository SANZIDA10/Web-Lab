using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebLab.WebForms.Models
{
    public static class SubmissionRepository
    {
        // In-memory fallback store
        private static readonly List<Submission> _store = new List<Submission>();

        private static string GetConnectionString()
        {
            try
            {
                var cs = ConfigurationManager.ConnectionStrings["KCCDB"];
                return cs?.ConnectionString;
            }
            catch
            {
                return null;
            }
        }

        private static void EnsureTableExists(string connStr)
        {
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
IF OBJECT_ID('dbo.Submissions','U') IS NULL
BEGIN
    CREATE TABLE dbo.Submissions(
        Id uniqueidentifier NOT NULL PRIMARY KEY,
        [Type] nvarchar(50) NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [Email] nvarchar(200) NOT NULL,
        [StudyProgram] nvarchar(100) NULL,
        [Interests] nvarchar(MAX) NULL,
        [Message] nvarchar(MAX) NULL,
        [SubmittedAt] datetimeoffset NOT NULL
    );
END"
                ;
                cmd.ExecuteNonQuery();
            }
        }

        public static void Save(Submission submission)
        {
            var connStr = GetConnectionString();
            if (string.IsNullOrWhiteSpace(connStr))
            {
                // fallback to in-memory store
                _store.Add(submission);
                return;
            }

            try
            {
                EnsureTableExists(connStr);

                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"INSERT INTO dbo.Submissions
                        (Id, [Type], [Name], [Email], [StudyProgram], [Interests], [Message], [SubmittedAt])
                        VALUES
                        (@Id, @Type, @Name, @Email, @StudyProgram, @Interests, @Message, @SubmittedAt)";

                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = submission.Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.NVarChar, 50) { Value = (object)submission.Type ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = (object)submission.Name ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 200) { Value = (object)submission.Email ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@StudyProgram", SqlDbType.NVarChar, 100) { Value = (object)submission.StudyProgram ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Interests", SqlDbType.NVarChar, -1) { Value = (object)submission.Interests ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, -1) { Value = (object)submission.Message ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@SubmittedAt", SqlDbType.DateTimeOffset) { Value = submission.SubmittedAt });

                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // If database write fails, keep an in-memory copy so data isn't lost completely
                _store.Add(submission);
            }
        }

        public static IReadOnlyList<Submission> GetAll()
        {
            var connStr = GetConnectionString();
            if (string.IsNullOrWhiteSpace(connStr))
            {
                return _store.AsReadOnly();
            }

            try
            {
                EnsureTableExists(connStr);
                var list = new List<Submission>();
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT Id, [Type], [Name], [Email], [StudyProgram], [Interests], [Message], [SubmittedAt] FROM dbo.Submissions ORDER BY SubmittedAt DESC";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Submission
                            {
                                Id = reader.GetGuid(0),
                                Type = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                StudyProgram = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Interests = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Message = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                SubmittedAt = reader.IsDBNull(7) ? DateTimeOffset.UtcNow : (DateTimeOffset)reader.GetValue(7)
                            });
                        }
                    }
                }

                return list.AsReadOnly();
            }
            catch
            {
                return _store.AsReadOnly();
            }
        }

        public static Submission GetById(Guid id)
        {
            var connStr = GetConnectionString();
            if (string.IsNullOrWhiteSpace(connStr))
            {
                return _store.Find(s => s.Id == id);
            }

            try
            {
                EnsureTableExists(connStr);
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT Id, [Type], [Name], [Email], [StudyProgram], [Interests], [Message], [SubmittedAt] FROM dbo.Submissions WHERE Id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id });
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Submission
                            {
                                Id = reader.GetGuid(0),
                                Type = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                StudyProgram = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Interests = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Message = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                SubmittedAt = reader.IsDBNull(7) ? DateTimeOffset.UtcNow : (DateTimeOffset)reader.GetValue(7)
                            };
                        }
                    }
                }
            }
            catch
            {
                // ignore and fallback to in-memory
            }

            return _store.Find(s => s.Id == id);
        }
    }
}
