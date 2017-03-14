<%@ Page Language="C#"  MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Site.Error" Title="Error" %>
<%@ MasterType TypeName="Site.SiteMaster" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    
   
        <div class="alert alert-info">
		      <p>Oops, what did you just do? an error occurred during your request!</p>
		</div>  
         <a class="back mainmenu" style="border: none; width: 200; text-align: center; margin: 0 auto; display: block;" href="/cases/civil" />Main menu</a>
   
            
</asp:Content>