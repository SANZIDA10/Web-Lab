<%@ Page Title="Events | KUET Career Club" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="WebLab.WebForms.EventsPage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Events | KUET Career Club</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    /* Page-scoped styles to match visual design: large left-aligned header and teal accents */
    .page-header {
      padding: 6rem 2.5rem 3.5rem;
      position: relative;
      overflow: visible;
      max-width: 920px;
      text-align: left;
    }
    .page-header::before {
      content: '';
      position: absolute;
      left: -80px; top: -80px;
      width: 420px; height: 420px;
      background: radial-gradient(circle, rgba(26,122,110,0.12) 0%, transparent 70%);
      pointer-events: none;
    }
    .page-tag { font-size: 12px; color: var(--text-accent); letter-spacing: 0.12em; font-weight: 700; margin-bottom: 12px; }
    .page-header h1 { font-size: 72px; font-weight: 800; line-height: 0.95; color: var(--text); margin-bottom: 1rem; letter-spacing: -1px; }
    .header-divider { width: 60px; height: 2px; background: var(--text-accent); margin: 1.5rem 0; }
    .page-header p { font-size: 16px; color: var(--text-muted); max-width: 640px; line-height: 1.7; }

    .events-section { padding: 2rem 2.5rem 4rem; }
    .events-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 24px; margin-top: 2.5rem; }
    .event-card {
      background: var(--card);
      border: 1px solid rgba(255,255,255,0.04);
      border-radius: 12px;
      padding: 24px;
      display: flex;
      flex-direction: column;
      gap: 12px;
      min-height: 320px;
      transition: transform 0.18s ease, border-color 0.2s;
    }
    .event-card:hover { transform: translateY(-6px); border-color: rgba(26,122,110,0.18); }
    .event-badge { display: inline-block; padding: 6px 10px; border-radius: 6px; border: 1px solid rgba(26,122,110,0.18); color: var(--text-accent); background: transparent; font-weight:700; font-size: 0.78rem; }
    .event-card h2 { font-size: 20px; font-weight: 800; color: var(--text); margin-top: 6px; }
    .event-meta { color: var(--text-faint); font-size: 0.95rem; margin-top: 6px; }
    .event-card p { color: var(--text-muted); line-height: 1.7; }
    .btn-register { margin-top: auto; align-self: start; padding: 10px 22px; border-radius: 24px; background: transparent; border: 1px solid rgba(26,122,110,0.12); color: var(--text); cursor: pointer; }

    @media (max-width: 980px) {
      .events-grid { grid-template-columns: repeat(2, 1fr); }
      .page-header h1 { font-size: 48px; }
    }
    @media (max-width: 640px) {
      .events-grid { grid-template-columns: 1fr; }
      .page-header { padding-top: 4rem; }
      .page-header h1 { font-size: 36px; }
    }
  </style>
</asp:Content>
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
