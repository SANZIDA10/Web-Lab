<%@ Page Title="Join | KUET Career Club" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Join.aspx.cs" Inherits="WebLab.WebForms.JoinPage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Join | KUET Career Club</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="join-container">
        <div class="join-header">
            <h1>Join Our Community</h1>
            <p>Become a member of KUET Career Club and unlock access to exclusive opportunities, networking events, and professional development resources.</p>
        </div>

        <div class="benefits-preview">
            <div class="benefit-item">
                <div class="benefit-icon">🎓</div>
                <h3>Skill Development</h3>
                <p>Workshops on CV writing, interviews, and leadership</p>
            </div>
            <div class="benefit-item">
                <div class="benefit-icon">🤝</div>
                <h3>Networking</h3>
                <p>Connect with alumni and industry professionals</p>
            </div>
            <div class="benefit-item">
                <div class="benefit-icon">💼</div>
                <h3>Opportunities</h3>
                <p>Access internships and collaborative projects</p>
            </div>
        </div>

        <div class="info-box">
            <strong>What happens next?</strong>
            After you submit this form, our team will review your application within 24-48 hours and send you a confirmation email with membership details.
        </div>

        <div class="form-wrapper">
            <asp:ValidationSummary ID="JoinSummary" runat="server" CssClass="validation-summary" ValidationGroup="JoinGroup" />
            <asp:Panel ID="JoinStatusPanel" runat="server" CssClass="form-feedback success" Visible="false">
                <asp:Literal ID="JoinStatusText" runat="server" />
            </asp:Panel>

            <asp:Panel ID="JoinFormPanel" runat="server" CssClass="membership-form">
                <div class="form-section">
                    <h3>Personal Information</h3>
                    <div class="form-grid two-columns">
                        <div class="form-field">
                            <label for="FullNameTextBox">Full Name</label>
                            <asp:TextBox ID="FullNameTextBox" runat="server" CssClass="text-input" />
                            <asp:RequiredFieldValidator ID="FullNameRequiredValidator" runat="server" ControlToValidate="FullNameTextBox" ErrorMessage="Please enter your full name." Display="Dynamic" CssClass="field-validation-error" ValidationGroup="JoinGroup" />
                            <asp:CustomValidator ID="FullNameLengthValidator" runat="server" ControlToValidate="FullNameTextBox" ErrorMessage="Please enter at least 2 characters." Display="Dynamic" CssClass="field-validation-error" OnServerValidate="FullNameLengthValidator_ServerValidate" ValidationGroup="JoinGroup" />
                        </div>

                        <div class="form-field">
                            <label for="EmailTextBox">Email Address</label>
                            <asp:TextBox ID="EmailTextBox" runat="server" CssClass="text-input" TextMode="Email" />
                            <asp:RequiredFieldValidator ID="EmailRequiredValidator" runat="server" ControlToValidate="EmailTextBox" ErrorMessage="Please enter your email address." Display="Dynamic" CssClass="field-validation-error" ValidationGroup="JoinGroup" />
                            <asp:RegularExpressionValidator ID="EmailRegexValidator" runat="server" ControlToValidate="EmailTextBox" ErrorMessage="Please enter a valid email address." Display="Dynamic" CssClass="field-validation-error" ValidationExpression="^[^\s@]+@[^\s@]+\.[^\s@]+$" ValidationGroup="JoinGroup" />
                        </div>
                    </div>
                </div>

                <div class="form-section">
                    <h3>Academic Information</h3>
                    <asp:RadioButtonList ID="StudyProgramList" runat="server" CssClass="program-list" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Value="CSE">CSE</asp:ListItem>
                        <asp:ListItem Value="EEE">EEE</asp:ListItem>
                        <asp:ListItem Value="ME">ME</asp:ListItem>
                        <asp:ListItem Value="CE">CE</asp:ListItem>
                        <asp:ListItem Value="BTE">BTE</asp:ListItem>
                        <asp:ListItem Value="Other">Other</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="StudyProgramRequiredValidator" runat="server" ControlToValidate="StudyProgramList" ErrorMessage="Please select your study program." Display="Dynamic" CssClass="field-validation-error" ValidationGroup="JoinGroup" />
                    <div class="help-text">Select your department or program.</div>
                </div>

                <div class="form-section">
                    <h3>Your Interests</h3>
                    <asp:CheckBoxList ID="InterestsList" runat="server" CssClass="checkbox-group" RepeatLayout="Flow">
                        <asp:ListItem Value="Workshops">Skill Development Workshops</asp:ListItem>
                        <asp:ListItem Value="Mentoring">Mentoring & Guidance</asp:ListItem>
                        <asp:ListItem Value="Internships">Internship Opportunities</asp:ListItem>
                        <asp:ListItem Value="Networking">Networking Events</asp:ListItem>
                        <asp:ListItem Value="Projects">Collaborative Projects</asp:ListItem>
                    </asp:CheckBoxList>
                    <asp:CustomValidator ID="InterestsValidator" runat="server" ErrorMessage="Please select at least one interest." Display="Dynamic" CssClass="field-validation-error" OnServerValidate="InterestsValidator_ServerValidate" ValidationGroup="JoinGroup" />
                </div>

                <div class="form-section">
                    <h3>Tell Us About Yourself</h3>
                    <div class="form-field">
                        <label for="MessageTextBox">Why do you want to join KUET Career Club?</label>
                        <asp:TextBox ID="MessageTextBox" runat="server" CssClass="text-area" TextMode="MultiLine" Rows="6" />
                        <asp:RequiredFieldValidator ID="MessageRequiredValidator" runat="server" ControlToValidate="MessageTextBox" ErrorMessage="Please share your motivation." Display="Dynamic" CssClass="field-validation-error" ValidationGroup="JoinGroup" />
                        <asp:CustomValidator ID="MessageLengthValidator" runat="server" ControlToValidate="MessageTextBox" ErrorMessage="Your message should be at least 10 characters long." Display="Dynamic" CssClass="field-validation-error" OnServerValidate="MessageLengthValidator_ServerValidate" ValidationGroup="JoinGroup" />
                    </div>
                </div>

                <div class="form-actions">
                    <button type="reset" class="btn-cancel">Clear Form</button>
                    <asp:Button ID="SubmitButton" runat="server" CssClass="btn-submit" Text="Submit Application" OnClick="SubmitButton_Click" ValidationGroup="JoinGroup" />
                </div>
            </asp:Panel>
        </div>

        <div class="join-footer-note">
            <p>Have questions? <a href="Contact.aspx">Contact us</a></p>
        </div>
    </div>
</asp:Content>