<%@ Page Title="Members | KCC Portfolio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="WebLab.WebForms.Members" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    /* Members grid: square tiles with images, scrollable container */
    .members-wrapper { max-height: 720px; overflow: auto; padding: 1rem 0 2rem; }
    .members-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(240px, 1fr)); gap: 22px; }
    .member-card { background: var(--card); border: 1px solid rgba(255,255,255,0.04); border-radius: 12px; overflow: hidden; display: flex; flex-direction: column; transition: transform 0.18s ease, box-shadow 0.18s ease; }
    .member-card:hover { transform: translateY(-8px); box-shadow: 0 18px 40px rgba(0,0,0,0.45); }
    .member-figure { width: 100%; aspect-ratio: 1 / 1; background: #222; display: block; }
    .member-figure img { width: 100%; height: 100%; object-fit: cover; display: block; }
    .member-info { padding: 14px 16px; display: flex; flex-direction: column; gap: 6px; }
    .member-name { font-size: 16px; font-weight: 800; color: var(--text); }
    .member-role { font-size: 12px; font-weight: 700; color: #e6c700; text-transform: uppercase; letter-spacing: 0.06em; }
    .member-meta { font-size: 12px; color: var(--text-muted); }
    .member-bio { margin-top: 8px; color: var(--text-faint); font-size: 13px; line-height: 1.5; }
    @media (max-width: 980px) { .members-grid { grid-template-columns: repeat(2, 1fr); } }
    @media (max-width: 640px) { .members-grid { grid-template-columns: 1fr; } .members-wrapper { max-height: none; } }
  </style>
</asp:Content>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Members | KCC Portfolio</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <section class="page-hero">
        <p class="kicker">PEOPLE BEHIND THE WORK</p>
        <h1>Meet the KCC Members</h1>
        <p class="subtitle">The students who plan, organize, and deliver KCC's career development programs.</p>
    </section>

    <section class="members-wrapper">
      <div class="members-grid">
        <asp:Repeater ID="MembersRepeater" runat="server">
          <ItemTemplate>
            <article class="member-card">
              <figure class="member-figure">
                <img src='<%# Eval("ImageUrl") ?? "~/webforms/kcc-logo.png" %>' alt='<%# Eval("Name") %>' />
              </figure>
              <div class="member-info">
                <div>
                  <div class="member-name"><%# Eval("Name") %></div>
                  <div class="member-role"><%# Eval("Role") %></div>
                </div>
                <div class="member-meta"><%# Eval("Department") %> &bull; <%# Eval("Year") %></div>
                <p class="member-bio"><%# Eval("Bio") %></p>
              </div>
            </article>
          </ItemTemplate>
        </asp:Repeater>
      </div>

      <asp:Panel ID="EmptyStatePanel" runat="server" CssClass="empty-state" Visible="false">
        <h3>No members to display yet.</h3>
      </asp:Panel>
    </section>

</asp:Content>