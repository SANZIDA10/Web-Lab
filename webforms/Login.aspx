<%@ Page Title="Login | KUET Career Club" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebLab.WebForms.LoginPage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Login | KUET Career Club</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .login-panel { max-width: 420px; margin: 40px auto; padding: 24px; background: var(--card); border-radius: 12px; border:1px solid rgba(255,255,255,0.06); }
    .login-panel h2 { margin-bottom: 12px; }
    .form-field { margin-bottom: 12px; }
    .btn-submit { width: 100%; }
  </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  <div class="login-panel">
    <h2>Sign in</h2>
    <asp:Label ID="StatusLabel" runat="server" CssClass="form-feedback" Visible="false" />
    <asp:Panel runat="server">
      <div class="form-field">
        <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="text-input" Placeholder="Username" />
      </div>
      <div class="form-field">
        <asp:TextBox ID="PasswordTextBox" runat="server" CssClass="text-input" TextMode="Password" Placeholder="Password" />
      </div>
      <div class="form-field">
        <asp:Button ID="LoginButton" runat="server" CssClass="btn-submit" Text="Sign in" OnClick="LoginButton_Click" />
      </div>
    </asp:Panel>
  </div>
</asp:Content>