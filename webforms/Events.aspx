<%@ Page Title="Events | KUET Career Club" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="WebLab.WebForms.EventsPage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Events | KUET Career Club</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  <div class="page-header">
    <div class="page-tag">WHAT'S HAPPENING</div>
    <h1>Upcoming<br/>Events</h1>
    <div class="header-divider"></div>
    <p>Discover workshops, networking meetups, and career-focused sessions designed to prepare KUET students for real-world success.</p>
  </div>

  <section class="events-section">
    <div class="events-grid">
      <div class="event-card">
        <span class="event-badge">WORKSHOP</span>
        <h2>Resume &amp; LinkedIn Bootcamp</h2>
        <div class="event-meta">📅 May 03, 2026 &bull; 3:30 PM &bull; Central Seminar Hall</div>
        <p>Learn how to write impactful resumes and optimize your LinkedIn profile for internship and job applications.</p>
        <button class="btn-register">Register Now →</button>
      </div>

      <div class="event-card">
        <span class="event-badge">NETWORKING</span>
        <h2>Alumni Career Talk</h2>
        <div class="event-meta">📅 May 12, 2026 &bull; 5:00 PM &bull; Virtual Session</div>
        <p>Connect with KUET alumni working in top industries and gain insights on career choices and growth paths.</p>
        <button class="btn-register">Register Now →</button>
      </div>

      <div class="event-card">
        <span class="event-badge">MOCK SESSION</span>
        <h2>Interview Practice Day</h2>
        <div class="event-meta">📅 May 20, 2026 &bull; 2:00 PM &bull; Career Lab</div>
        <p>Practice technical and HR interviews with mentors and receive structured feedback to improve confidence.</p>
        <button class="btn-register">Register Now →</button>
      </div>
    </div>
  </section>

  <div class="footer-tagline">
    <h2>Stay ahead of the <em>curve.</em></h2>
    <div class="social-links">
      <a href="https://www.facebook.com/kuetcareerclub/" target="_blank"><i class="ti ti-brand-facebook"></i></a>
      <a href="https://www.linkedin.com/company/kuetcareerclub/" target="_blank"><i class="ti ti-brand-linkedin"></i></a>
    </div>
  </div>
  <div class="copyright">© 2026 KUET CAREER CLUB. ALL RIGHTS RESERVED.</div>
</asp:Content>
