using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using WebLab.WebForms.Data;
using WebLab.WebForms.Infrastructure;
using WebLab.WebForms.Models;

namespace WebLab.WebForms
{
    public partial class MembersPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var query = Request.QueryString["q"] ?? string.Empty;
                SearchTextBox.Text = query;
                BindMembers(query);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            var query = SearchTextBox.Text.Trim();
            Response.Redirect(string.IsNullOrWhiteSpace(query) ? "Members.aspx" : $"Members.aspx?q={Server.UrlEncode(query)}", false);
        }

        private void BindMembers(string query)
        {
            IReadOnlyList<Submission> members = string.IsNullOrWhiteSpace(query)
                ? SubmissionRepository.GetMembers()
                : SubmissionRepository.SearchMembers(query);

            var statistics = SubmissionRepository.GetMemberStatistics(members);

            TotalMembersLiteral.Text = statistics.TotalMembers.ToString();
            JoinedThisMonthLiteral.Text = statistics.JoinedThisMonth.ToString();
            SearchResultsLiteral.Text = members.Count.ToString();

            MembersRepeater.DataSource = members;
            MembersRepeater.DataBind();

            ProgramRepeater.DataSource = statistics.ProgramDistribution.OrderBy(entry => entry.Key);
            ProgramRepeater.DataBind();

            if (members.Count == 0)
            {
                EmptyStatePanel.Visible = true;
            }
        }
    }
}