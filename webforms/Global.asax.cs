using System;
using System.Web;
using WebLab.WebForms.Data;

namespace WebLab.WebForms
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            SubmissionRepository.Initialize();
        }
    }
}