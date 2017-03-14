<%@ Page Language="C#" MasterPageFile="~/Site.master" EnableViewState="false" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="Site.Help" Title="Help" %>
<%@ MasterType TypeName="Site.SiteMaster" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <div style="margin-top: 20px;"><asp:Literal runat="server" ID="Content" /></div>
    
    <asp:HyperLink runat="server" ID="Back" Text="Back" CssClass="back" NavigateUrl="javascript:history.go(-1)" />
    <asp:HyperLink runat="server" ID="MainMenu" Text="Main menu" CssClass="back mainmenu" NavigateUrl="/cases/civil" />
</asp:Content>
