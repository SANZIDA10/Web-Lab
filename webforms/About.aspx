<%@ Page Title="About | KUET Career Club" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebLab.WebForms.AboutPage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">About | KUET Career Club</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="page-header">
        <h1>About KUET Career Club</h1>
        <p>We are a student-led platform helping KUET students prepare for meaningful careers with confidence and direction.</p>
    </section>

    <section class="about-grid">
        <article class="about-card">
            <h2>Our Mission</h2>
            <p>To bridge the gap between academic learning and professional success by creating practical, inclusive, and growth-focused experiences.</p>
        </article>
        <article class="about-card">
            <h2>Our Vision</h2>
            <p>To develop a vibrant career ecosystem where every KUET student can explore potential, sharpen skills, and lead with impact.</p>
        </article>
        <article class="about-card">
            <h2>What We Do</h2>
            <ul>
                <li>Career guidance workshops</li>
                <li>Resume and interview preparation</li>
                <li>Industry and alumni networking</li>
                <li>Leadership and team projects</li>
            </ul>
        </article>
    </section>
</asp:Content>