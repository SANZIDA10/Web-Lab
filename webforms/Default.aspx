<%@ Page Title="Home | KUET Career Club" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebLab.WebForms.HomePage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">KUET Career Club</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@tabler/icons-webfont@latest/tabler-icons.min.css" />
    <style>
      :root[data-theme="dark"] {
        --bg: #0d0d0d; --bg2: #111; --card: #161616;
        --text: #fff; --text-muted: rgba(255,255,255,0.55);
        --text-faint: rgba(255,255,255,0.4);
        --border: rgba(255,255,255,0.08);
        --border-nav: rgba(255,255,255,0.12);
        --nav-bg: rgba(255,255,255,0.07);
        --marquee-border: rgba(255,255,255,0.06);
        --glow: rgba(26,122,110,0.2);
        --copyright: rgba(255,255,255,0.2);
      }
      :root[data-theme="light"] {
        --bg: #f5f5f5; --bg2: #ebebeb; --card: #fff;
        --text: #0a0a0a; --text-muted: rgba(0,0,0,0.6);
        --text-faint: rgba(0,0,0,0.45);
        --border: rgba(0,0,0,0.08);
        --border-nav: rgba(0,0,0,0.12);
        --nav-bg: rgba(0,0,0,0.06);
        --marquee-border: rgba(0,0,0,0.08);
        --glow: rgba(26,122,110,0.12);
        --copyright: rgba(0,0,0,0.3);
      }
      * { margin: 0; padding: 0; box-sizing: border-box; }
      body { font-family: 'Segoe UI', sans-serif; background: var(--bg); color: var(--text); transition: background 0.3s, color 0.3s; }
      .hero { display: flex; align-items: center; justify-content: space-between; padding: 3rem 2.5rem 4rem; min-height: 440px; position: relative; overflow: hidden; }
      .hero-glow { position: absolute; right: 220px; top: 50%; transform: translateY(-50%); width: 380px; height: 380px; background: radial-gradient(circle, var(--glow) 0%, transparent 70%); pointer-events: none; }
      .hero-left { flex: 1; z-index: 2; }
      .hero-title { font-size: 72px; font-weight: 700; line-height: 1; letter-spacing: -0.02em; color: var(--text); margin-bottom: 1rem; }
      .hero-divider { width: 80px; height: 2px; background: #1a7a6e; margin-bottom: 1.5rem; }
      .hero-sub { font-size: 15px; color: var(--text-muted); max-width: 420px; line-height: 1.7; margin-bottom: 2rem; }
      .hero-btns { display: flex; gap: 12px; }
      .btn-ghost { background: transparent; border: 1px solid var(--border-nav); color: var(--text); padding: 10px 24px; border-radius: 30px; font-size: 13px; cursor: pointer; letter-spacing: 0.05em; text-decoration: none; transition: all 0.2s; }
      .btn-teal { background: #1a7a6e; border: none; color: #fff; padding: 10px 24px; border-radius: 30px; font-size: 13px; cursor: pointer; letter-spacing: 0.05em; font-weight: 700; text-decoration: none; }
      .logo-display { width: 220px; height: 220px; border-radius: 50%; overflow: hidden; border: 2px solid rgba(26,122,110,0.6); box-shadow: 0 0 50px rgba(26,122,110,0.25); }
      .logo-display img { width: 100%; height: 100%; object-fit: cover; display: block; }
      .marquee-wrap { background: var(--bg2); border-top: 0.5px solid var(--marquee-border); border-bottom: 0.5px solid var(--marquee-border); padding: 14px 0; overflow: hidden; white-space: nowrap; }
      .marquee-track { display: inline-block; animation: marquee 22s linear infinite; }
      .marquee-item { display: inline-block; font-size: 18px; font-weight: 700; letter-spacing: 0.12em; color: #1a7a6e; margin: 0 2rem; }
      .marquee-dot { display: inline-block; width: 6px; height: 6px; background: var(--text-faint); border-radius: 50%; vertical-align: middle; margin: 0 0.5rem; }
      @keyframes marquee { 0% { transform: translateX(0); } 100% { transform: translateX(-50%); } }
      .featured { padding: 3rem 2.5rem 2rem; }
      .featured-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.25rem; }
      .featured-header h2 { font-size: 20px; font-weight: 700; color: var(--text); }
      .event-banner { background: var(--card); border: 0.5px solid var(--border); border-radius: 12px; }
      .event-banner-inner { padding: 1.5rem 2rem; display: flex; align-items: center; justify-content: space-between; }
      .event-tag { font-size: 11px; color: #1a7a6e; border: 0.5px solid rgba(26,122,110,0.5); padding: 3px 10px; border-radius: 4px; letter-spacing: 0.06em; display: inline-block; margin-bottom: 8px; }
      .event-banner h3 { font-size: 18px; font-weight: 600; color: var(--text); }
      .event-date { font-size: 13px; color: var(--text-faint); margin-top: 4px; }
      .stats { display: grid; grid-template-columns: repeat(3, 1fr); gap: 14px; padding: 2rem 2.5rem 3rem; }
      .stat-card { background: var(--card); border: 0.5px solid var(--border); border-radius: 12px; padding: 2rem 1.5rem; text-align: center; }
      .stat-num { font-size: 48px; font-weight: 700; color: #1a7a6e; line-height: 1; }
      .stat-label { font-size: 11px; letter-spacing: 0.1em; color: var(--text-faint); margin-top: 10px; }
      .footer-tagline { padding: 2rem 2.5rem 1.5rem; border-top: 0.5px solid var(--border); display: flex; align-items: center; justify-content: space-between; }
      .footer-tagline h2 { font-size: 28px; font-weight: 700; color: var(--text); }
      .footer-tagline h2 em { font-style: italic; color: #1a7a6e; }
      .social-links { display: flex; gap: 16px; }
      .social-links a { color: var(--text-faint); text-decoration: none; font-size: 22px; transition: color 0.2s; }
      .copyright { text-align: right; padding: 0 2.5rem 1.5rem; font-size: 12px; color: var(--copyright); letter-spacing: 0.04em; }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="hero">
      <div class="hero-glow"></div>
      <div class="hero-left">
        <h1 class="hero-title">KUET<br/>CAREER<br/>CLUB</h1>
        <div class="hero-divider"></div>
        <p class="hero-sub">Build skills, connect with mentors, and discover opportunities that shape your future career.</p>
        <div class="hero-btns">
          <a href="About.aspx" class="btn-ghost">DISCOVER MORE</a>
          <a href="Events.aspx" class="btn-teal">VIEW EVENTS</a>
        </div>
      </div>
      <div class="hero-right">
        <div class="logo-display">
          <img src="kcc-logo.png" alt="KUET Career Club Logo" />
        </div>
      </div>
    </section>

    <div class="marquee-wrap">
      <div class="marquee-track">
        <span class="marquee-item">SKILL DEVELOPMENT</span><span class="marquee-dot"></span>
        <span class="marquee-item">CAREER NETWORKING</span><span class="marquee-dot"></span>
        <span class="marquee-item">REAL OPPORTUNITIES</span><span class="marquee-dot"></span>
        <span class="marquee-item">LEADERSHIP</span><span class="marquee-dot"></span>
        <span class="marquee-item">INTERVIEW PREP</span><span class="marquee-dot"></span>
        <span class="marquee-item">ALUMNI CONNECT</span><span class="marquee-dot"></span>
      </div>
    </div>

    <section class="featured">
      <div class="featured-header">
        <h2>Featured Event</h2>
        <a href="Events.aspx" class="see-all">ALL EVENTS &rarr;</a>
      </div>
      <div class="event-banner">
        <div class="event-banner-inner">
          <div>
            <div class="event-tag">WORKSHOP</div>
            <h3>Resume &amp; LinkedIn Bootcamp</h3>
            <div class="event-date">May 03, 2026 &bull; 3:30 PM &bull; Central Seminar Hall</div>
          </div>
          <a href="Events.aspx" class="event-arrow">&#8594;</a>
        </div>
      </div>
    </section>

    <div class="stats">
      <div class="stat-card"><div class="stat-num">200+</div><div class="stat-label">ACTIVE MEMBERS</div></div>
      <div class="stat-card"><div class="stat-num">30+</div><div class="stat-label">EVENTS HELD</div></div>
      <div class="stat-card"><div class="stat-num">50+</div><div class="stat-label">ALUMNI NETWORK</div></div>
    </div>

    <div class="footer-tagline">
      <h2>Stay ahead of the <em>curve.</em></h2>
      <div class="social-links">
        <a href="https://www.facebook.com/kuetcareerclub/" target="_blank"><i class="ti ti-brand-facebook"></i></a>
        <a href="https://www.linkedin.com/company/kuetcareerclub/" target="_blank"><i class="ti ti-brand-linkedin"></i></a>
      </div>
    </div>
    <div class="copyright">© 2026 KUET CAREER CLUB. ALL RIGHTS RESERVED.</div>

    <script>
      (function(){
        var btn = document.getElementById('themeToggle');
        if(btn){
          btn.addEventListener('click', function(){
            var html = document.documentElement;
            if (html.getAttribute('data-theme') === 'dark'){
              html.setAttribute('data-theme','light');
              btn.textContent = '☀️';
            } else {
              html.setAttribute('data-theme','dark');
              btn.textContent = '🌙';
            }
          });
        }
      })();
    </script>
</asp:Content>
