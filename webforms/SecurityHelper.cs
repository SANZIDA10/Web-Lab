using System;
using System.Web;
using System.Web.UI;

namespace WebLab.WebForms
{
    public static class SecurityHelper
    {
        // Prevent browser caching of protected pages
        public static void PreventBrowserCache(Page page)
        {
            if (page == null) return;
            var response = page.Response;
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        }

        // Require login and redirect to Login.aspx when not authenticated
        public static void RequireLogin(Page page)
        {
            PreventBrowserCache(page);
            if (page.Session["LoggedInUser"] == null)
            {
                page.Response.Redirect(page.ResolveUrl("~/Login.aspx?msg=login_required"), true);
            }
        }
    }
}