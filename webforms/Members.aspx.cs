using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebLab.WebForms
{
    public partial class Members : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var members = GetMembers();
            MembersRepeater.DataSource = members;
            MembersRepeater.DataBind();
            EmptyStatePanel.Visible = (members.Count == 0);
        }

        private List<dynamic> GetMembers()
        {
            var list = new List<dynamic>();
            string connStr = ConfigurationManager.ConnectionStrings["KCCDB"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                // Check if ImageUrl column exists so we can include it when present
                bool hasImageUrl = false;
                using (var colCheck = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Members' AND COLUMN_NAME = 'ImageUrl'", conn))
                {
                    try { hasImageUrl = (int)colCheck.ExecuteScalar() > 0; } catch { hasImageUrl = false; }
                }

                var sql = hasImageUrl
                    ? "SELECT Id, Name, Role, Department, Year, Bio, ImageUrl FROM Members ORDER BY Name"
                    : "SELECT Id, Name, Role, Department, Year, Bio, '' AS ImageUrl FROM Members ORDER BY Name";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Role = reader.GetString(2),
                            Department = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            Year = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            Bio = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            ImageUrl = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                        });
                    }
                }
            }
            return list;
        }
    }
}