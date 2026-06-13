using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebLab.WebForms.Infrastructure;

namespace WebLab.WebForms
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetActiveLink("Default.aspx", "HomeLink");
            SetActiveLink("About.aspx", "AboutLink");
            SetActiveLink("Events.aspx", "EventsLink");
            SetActiveLink("Contact.aspx", "ContactLink");
            SetActiveLink("Join.aspx", "JoinLink");
            SetActiveLink("Members.aspx", "MembersLink");
        }

        private void SetActiveLink(string pageName, string linkId)
        {
            var currentPage = (Page.AppRelativeVirtualPath ?? string.Empty)
                .Replace("~/", string.Empty)
                .TrimStart('/');

            var link = this.FindDescendant<HyperLink>(linkId);
            if (link is null)
            {
                return;
            }

            if (string.Equals(currentPage, pageName, StringComparison.OrdinalIgnoreCase) ||
                (pageName == "Default.aspx" && string.IsNullOrWhiteSpace(currentPage)))
            {
                link.CssClass = "active";
                link.Attributes["aria-current"] = "page";
            }
            else
            {
                link.CssClass = string.Empty;
                link.Attributes.Remove("aria-current");
            }
        }
    }
}