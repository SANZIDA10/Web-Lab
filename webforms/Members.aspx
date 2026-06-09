<%@ Page Title="Members | KUET Career Club" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="WebLab.WebForms.MembersPage" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Members | KUET Career Club</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="members-container">
        <div class="dashboard-header">
            <div>
                <p class="hero-kicker">Members Dashboard</p>
                <h1>Joined Members</h1>
            </div>
            <div class="dashboard-actions">
                <asp:TextBox ID="SearchTextBox" runat="server" CssClass="search-input" placeholder="Search members" />
                <asp:Button ID="SearchButton" runat="server" CssClass="search-button" Text="Search" OnClick="SearchButton_Click" CausesValidation="false" />
            </div>
        </div>

        <div class="stats-grid">
            <article class="stat-card">
                <h3>Total Members</h3>
                <div class="stat-value"><asp:Literal ID="TotalMembersLiteral" runat="server" /></div>
            </article>
            <article class="stat-card">
                <h3>Joined This Month</h3>
                <div class="stat-value"><asp:Literal ID="JoinedThisMonthLiteral" runat="server" /></div>
            </article>
            <article class="stat-card">
                <h3>Search Results</h3>
                <div class="stat-value"><asp:Literal ID="SearchResultsLiteral" runat="server" /></div>
            </article>
        </div>

        <section class="program-stats">
            <h3>Program Distribution</h3>
            <div class="program-grid">
                <asp:Repeater ID="ProgramRepeater" runat="server">
                    <ItemTemplate>
                        <div class="program-item">
                            <div class="program-name"><%# Eval("Key") %></div>
                            <div class="program-count"><%# Eval("Value") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </section>

        <section class="members-list">
            <asp:Repeater ID="MembersRepeater" runat="server">
                <ItemTemplate>
                    <article class="member-card">
                        <div class="member-header">
                            <div class="member-info">
                                <h3><%# Eval("Name") %></h3>
                                <a class="member-email" href='mailto:<%# Eval("Email") %>'><%# Eval("Email") %></a>
                                <div class="member-meta">Joined <%# Eval("SubmittedAt", "{0:MMMM dd, yyyy}") %></div>
                            </div>
                            <span class="event-badge"><%# Eval("Type") %></span>
                        </div>

                        <div class="member-details">
                            <div class="detail-item">
                                <div class="detail-label">Study Program</div>
                                <div class="detail-value"><%# Eval("StudyProgram") %></div>
                            </div>
                            <div class="detail-item">
                                <div class="detail-label">Interests</div>
                                <div class="detail-value"><%# Eval("Interests") %></div>
                            </div>
                        </div>

                        <div class="message-section">
                            <p><%# Eval("Message") %></p>
                        </div>
                    </article>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Panel ID="EmptyStatePanel" runat="server" CssClass="empty-state" Visible="false">
                <h3>No members found.</h3>
                <p>Try a different search term or invite new members to join.</p>
            </asp:Panel>
        </section>
    </div>
</asp:Content>