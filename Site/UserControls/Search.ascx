<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="Kay.Site.Public.Common.UserControls.Search" %>

<asp:Panel ID="SearchForm" runat="server" DefaultButton="SearchButton" CssClass="search">
    <label>Search the portal</label>
    <asp:TextBox ID="Keywords" runat="server" ValidationGroup="Search" CssClass="input" />
    <asp:Button ID="SearchButton" Text="Search" runat="server" OnClick="Find" ValidationGroup="Search" CssClass="button" />

</asp:Panel>