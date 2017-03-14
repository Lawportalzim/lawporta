<%@ Page Title="Account Settings" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"   CodeBehind="AccountSettings.aspx.cs" Inherits="Site.AccountSettings" %>
<%@ MasterType TypeName="Site.SiteMaster" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

     <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="validation" />
        
     
     <p class="feedback"><asp:Literal ID="Feedback" runat="server" /></p>    

	        
		  	
	<div class="form">

        <div class="control-group">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="FirstName" CssClass="control-label">First name:</asp:Label>
            <asp:TextBox runat="server" ID="FirstName" MaxLength="64" CssClass="textEntry" Enabled="false" />            
        </div>
    
        <div class="control-group">
            <asp:Label ID="Label3" runat="server" AssociatedControlID="LastName" CssClass="control-label">Last name:</asp:Label>
            <asp:TextBox runat="server" ID="LastName" MaxLength="64" CssClass="textEntry" Enabled="false"  />            
        </div>
    
        <div class="control-group">
            <asp:Label ID="Label4" runat="server" AssociatedControlID="Company" CssClass="control-label">Company:</asp:Label>
            <asp:TextBox runat="server" ID="Company" MaxLength="64" CssClass="textEntry" Enabled="false"  />
        </div>
    
        <div id="password-row" class="control-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label">New Password:</asp:Label>
            <asp:TextBox runat="server" ID="Password" TextMode="Password" MaxLength="32" CssClass="textEntry" />
            <asp:CustomValidator ID="PasswordValidator" runat="server" ErrorMessage="Password does not match confirmation password!" Text="Password does not match confirmation password!" Display="Dynamic" OnServerValidate="ValidatePassword" />
            
        </div>

        <div id="Div1" class="control-group">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="PasswordConfirm" CssClass="control-label">Confirm Password:</asp:Label>
            <asp:TextBox runat="server" ID="PasswordConfirm" TextMode="Password" MaxLength="32" CssClass="textEntry" /> 
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Password does not match confirmation password!" Text="Confirmation password does not match password!" Display="Dynamic" OnServerValidate="ValidatePassword" />           
        </div>


        
        <div>
            <asp:LinkButton ID="SaveButton" runat="server" Text="Save" OnClick="Save" CssClass="button" />
        </div>        
  </div>
  <asp:HyperLink runat="server" ID="MainMenu" Text="Main menu" CssClass="back mainmenu register" NavigateUrl="/cases/civil" /> 
</asp:Content>
