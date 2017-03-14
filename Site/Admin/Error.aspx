<%@ Page Language="C#" MasterPageFile="~/admin/Common/MasterPages/Default.Master"  AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Kay.Site.Admin.Error" Title="Error" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleBar" runat="server">
    <h2>Error</h2>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">

    <div class="padder">
    
        <p><strong><asp:Literal ID="Feedback" runat="server" /></strong><br />
        The error has been logged and the site developers have been notified.</p>

        <p><em>Event ID: <asp:Literal ID="EventId" runat="server" /></em></p>
        
        <asp:Panel Id="ErrorDisplay" runat="server" Visible="false" CssClass="error-message">
            <pre><asp:Literal ID="ErrorData" runat="server" /></pre>
        </asp:Panel>
    
    </div>

</asp:Content>
