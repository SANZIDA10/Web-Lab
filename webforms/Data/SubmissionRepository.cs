using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using WebLab.WebForms.Models;

namespace WebLab.WebForms.Data
{
    public static class SubmissionRepository
    {
        private static readonly object InitializationLock = new object();
        private static bool _initialized;

        private static string DatabasePath
        {
            get
            {
                var mappedPath = HostingEnvironment.MapPath("~/App_Data/weblab.db");
                if (string.IsNullOrWhiteSpace(mappedPath))
                {
                    throw new InvalidOperationException("The App_Data folder could not be resolved.");
                }

                return mappedPath;
            }
        }

        private static string ConnectionString => $"Data Source={DatabasePath};Version=3;Foreign Keys=True;";

        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            lock (InitializationLock)
            {
                if (_initialized)
                {
                    return;
                }

                var directory = Path.GetDirectoryName(DatabasePath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
CREATE TABLE IF NOT EXISTS Submissions (
    Id TEXT NOT NULL PRIMARY KEY,
    Type TEXT NOT NULL,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL,
    StudyProgram TEXT NULL,
    Interests TEXT NULL,
    Message TEXT NOT NULL,
    SubmittedAt TEXT NOT NULL
);";
                        command.ExecuteNonQuery();
                    }
                }

                _initialized = true;
            }
        }

        public static Submission Save(Submission submission)
        {
            Initialize();

            submission.Id = submission.Id == Guid.Empty ? Guid.NewGuid() : submission.Id;
            submission.Type = submission.Type.Trim().ToLowerInvariant();
            submission.SubmittedAt = DateTimeOffset.UtcNow;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
INSERT INTO Submissions (Id, Type, Name, Email, StudyProgram, Interests, Message, SubmittedAt)
VALUES (@Id, @Type, @Name, @Email, @StudyProgram, @Interests, @Message, @SubmittedAt);";

                    command.Parameters.AddWithValue("@Id", submission.Id.ToString());
                    command.Parameters.AddWithValue("@Type", submission.Type);
                    command.Parameters.AddWithValue("@Name", submission.Name);
                    command.Parameters.AddWithValue("@Email", submission.Email);
                    command.Parameters.AddWithValue("@StudyProgram", (object?)submission.StudyProgram ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Interests", (object?)submission.Interests ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Message", submission.Message);
                    command.Parameters.AddWithValue("@SubmittedAt", submission.SubmittedAt.ToString("o", CultureInfo.InvariantCulture));

                    command.ExecuteNonQuery();
                }
            }

            return submission;
        }

        public static IReadOnlyList<Submission> GetSubmissions()
        {
            return LoadSubmissions(@"SELECT * FROM Submissions ORDER BY datetime(SubmittedAt) DESC, rowid DESC;");
        }

        public static IReadOnlyList<Submission> GetMembers()
        {
            return LoadSubmissions(@"SELECT * FROM Submissions WHERE Type = 'join' ORDER BY datetime(SubmittedAt) DESC, rowid DESC;");
        }

        public static IReadOnlyList<Submission> SearchMembers(string term)
        {
            var trimmedTerm = term.Trim().ToLowerInvariant();
            var likeTerm = $"%{trimmedTerm}%";

            return LoadSubmissions(@"SELECT * FROM Submissions
WHERE Type = 'join'
  AND (
        LOWER(Name) LIKE @Term
        OR LOWER(Email) LIKE @Term
        OR LOWER(COALESCE(StudyProgram, '')) LIKE @Term
      )
ORDER BY datetime(SubmittedAt) DESC, rowid DESC;", new SQLiteParameter("@Term", likeTerm));
        }

        public static MemberStatistics GetMemberStatistics(IReadOnlyCollection<Submission> members)
        {
            var programDistribution = members
                .Where(member => !string.IsNullOrWhiteSpace(member.StudyProgram))
                .GroupBy(member => member.StudyProgram!.Trim())
                .ToDictionary(group => group.Key, group => group.Count());

            var currentMonth = DateTimeOffset.UtcNow.Month;
            var currentYear = DateTimeOffset.UtcNow.Year;

            return new MemberStatistics
            {
                TotalMembers = members.Count,
                JoinedThisMonth = members.Count(member => member.SubmittedAt.Month == currentMonth && member.SubmittedAt.Year == currentYear),
                ProgramDistribution = programDistribution
            };
        }

        private static IReadOnlyList<Submission> LoadSubmissions(string sql, params SQLiteParameter[] parameters)
        {
            Initialize();

            var submissions = new List<Submission>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            submissions.Add(ReadSubmission(reader));
                        }
                    }
                }
            }

            return submissions;
        }

        private static Submission ReadSubmission(SQLiteDataReader reader)
        {
            return new Submission
            {
                Id = Guid.Parse(reader.GetString(0)),
                Type = reader.GetString(1),
                Name = reader.GetString(2),
                Email = reader.GetString(3),
                StudyProgram = reader.IsDBNull(4) ? null : reader.GetString(4),
                Interests = reader.IsDBNull(5) ? null : reader.GetString(5),
                Message = reader.GetString(6),
                SubmittedAt = DateTimeOffset.Parse(reader.GetString(7), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
            };
        }
    }
}