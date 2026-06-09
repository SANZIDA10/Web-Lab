using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebLab.WebForms.Data;
using WebLab.WebForms.Infrastructure;
using WebLab.WebForms.Models;

namespace WebLab.WebForms
{
    public partial class JoinPage : Page
    {
        protected void FullNameLengthValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value) && args.Value.Trim().Length >= 2;
        }

        protected void MessageLengthValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value) && args.Value.Trim().Length >= 10;
        }

        protected void InterestsValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var interestsList = this.RequireDescendant<CheckBoxList>("InterestsList");
            args.IsValid = interestsList.Items.Cast<ListItem>().Any(item => item.Selected);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Page.Validate("JoinGroup");
            if (!Page.IsValid)
            {
                return;
            }

            var fullNameTextBox = this.RequireDescendant<TextBox>("FullNameTextBox");
            var emailTextBox = this.RequireDescendant<TextBox>("EmailTextBox");
            var studyProgramList = this.RequireDescendant<RadioButtonList>("StudyProgramList");
            var interestsList = this.RequireDescendant<CheckBoxList>("InterestsList");
            var messageTextBox = this.RequireDescendant<TextBox>("MessageTextBox");
            var statusPanel = this.RequireDescendant<Panel>("JoinStatusPanel");
            var statusText = this.RequireDescendant<Literal>("JoinStatusText");

            var selectedInterests = interestsList.Items
                .Cast<ListItem>()
                .Where(item => item.Selected)
                .Select(item => item.Value)
                .ToArray();

            SubmissionRepository.Save(new Submission
            {
                Type = "join",
                Name = fullNameTextBox.Text.Trim(),
                Email = emailTextBox.Text.Trim(),
                StudyProgram = studyProgramList.SelectedValue.Trim(),
                Interests = string.Join(", ", selectedInterests),
                Message = messageTextBox.Text.Trim()
            });

            statusText.Text = "Your membership request has been submitted successfully. Check your email for next steps.";
            statusPanel.Visible = true;

            fullNameTextBox.Text = string.Empty;
            emailTextBox.Text = string.Empty;
            messageTextBox.Text = string.Empty;
            studyProgramList.ClearSelection();
            foreach (ListItem item in interestsList.Items)
            {
                item.Selected = false;
            }

            Page.SetFocus(fullNameTextBox);
        }
    }
}