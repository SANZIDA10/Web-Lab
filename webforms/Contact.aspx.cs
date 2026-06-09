using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebLab.WebForms.Data;
using WebLab.WebForms.Infrastructure;
using WebLab.WebForms.Models;

namespace WebLab.WebForms
{
    public partial class ContactPage : Page
    {
        protected void NameLengthValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value) && args.Value.Trim().Length >= 2;
        }

        protected void MessageLengthValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value) && args.Value.Trim().Length >= 10;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Page.Validate("ContactGroup");
            if (!Page.IsValid)
            {
                return;
            }

            var nameTextBox = this.RequireDescendant<TextBox>("NameTextBox");
            var emailTextBox = this.RequireDescendant<TextBox>("EmailTextBox");
            var messageTextBox = this.RequireDescendant<TextBox>("MessageTextBox");
            var statusPanel = this.RequireDescendant<Panel>("ContactStatusPanel");
            var statusText = this.RequireDescendant<Literal>("ContactStatusText");

            SubmissionRepository.Save(new Submission
            {
                Type = "contact",
                Name = nameTextBox.Text.Trim(),
                Email = emailTextBox.Text.Trim(),
                Message = messageTextBox.Text.Trim()
            });

            statusText.Text = "Thanks! Your message has been received.";
            statusPanel.Visible = true;

            nameTextBox.Text = string.Empty;
            emailTextBox.Text = string.Empty;
            messageTextBox.Text = string.Empty;
            Page.SetFocus(nameTextBox);
        }
    }
}