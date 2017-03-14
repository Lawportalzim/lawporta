<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Kay.Site.Admin.Users.Edit" Title="Edit user" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">

    <h2><asp:Literal ID="PageTitle" runat="server" /></h2>
    
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">

    <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="validation" />
    
<div class="row-fluid sortable">
    <div class="box span12">
	    <div class="box-header well" data-original-title>
		    <h2><i class="icon-edit"></i> User information</h2>			
	    </div>
	    <div class="box-content">
        <div class="form-horizontal">
        <fieldset>
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="FirstName" CssClass="control-label"><b>*</b> First name:</asp:Label>
                <asp:TextBox runat="server" ID="FirstName" MaxLength="64" CssClass="input-xxlarge" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName" ErrorMessage="First name is required" Text="Required"  />
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="LastName" CssClass="control-label"><b>*</b> Last name:</asp:Label>
                <asp:TextBox runat="server" ID="LastName" MaxLength="64" CssClass="input-xxlarge" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="LastName" ErrorMessage="Last name is required" Text="Required" />
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="EmailAddress" CssClass="control-label"><b>*</b> Username:</asp:Label>
                <asp:TextBox runat="server" ID="EmailAddress" MaxLength="256" CssClass="input-xxlarge" />
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="EmailAddress" ErrorMessage="Username name is required" Text="Required" Display="Dynamic"  />                                
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Telephone" CssClass="control-label">Telephone:</asp:Label>
                <asp:TextBox runat="server" ID="Telephone" MaxLength="64" CssClass="input-xxlarge" />
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="AddressLine1" CssClass="control-label">Address Line 1:</asp:Label>
                <asp:TextBox runat="server" ID="AddressLine1" MaxLength="128"  CssClass="input-xxlarge"  />
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="AddressLine2" CssClass="control-label">Address Line 2:</asp:Label>
                <asp:TextBox runat="server" ID="AddressLine2" MaxLength="128" CssClass="input-xxlarge" />
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Suburb" CssClass="control-label">Suburb:</asp:Label>
                <asp:TextBox runat="server" ID="Suburb" MaxLength="128"  CssClass="input-xxlarge"  />
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="City" CssClass="control-label">City:</asp:Label>
                <asp:TextBox runat="server" ID="City" MaxLength="128" CssClass="input-xxlarge" />
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Country" CssClass="control-label">Country:</asp:Label>
                <asp:TextBox runat="server" ID="Country" MaxLength="128" CssClass="input-xxlarge" />
            </div> 
            
            <div class="control-group">
                <asp:Label  runat="server" AssociatedControlID="StartDate" CssClass="control-label">Start Date:</asp:Label>
                <asp:TextBox runat="server" ID="StartDate" MaxLength="128" CssClass="input-xxlarge localDatePicker" />
            </div>         

            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="ExpiryDates" CssClass="control-label">Expiry Date:</asp:Label>
                <asp:TextBox runat="server" ID="ExpiryDates" MaxLength="128" CssClass="input-xxlarge localDatePicker" />
            </div> 
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Admin" CssClass="control-label">Administrator:</asp:Label>
                <div class="controls check">
                    <asp:Label runat="server" AssociatedControlID="Admin" CssClass="checkbox inline">
                        <asp:CheckBox runat="server" ID="Admin" Text="Allow access to users" CssClass="checkbox" />
                    </asp:Label>
                </div>
            </div>
    
            <div class="control-group">
                <asp:Label ID="Label1" runat="server" AssociatedControlID="SuperUser" CssClass="control-label">Super User:</asp:Label>
                <div class="controls check">
                    <asp:Label runat="server" AssociatedControlID="SuperUser" CssClass="checkbox inline">
                        <asp:CheckBox runat="server" ID="SuperUser" Text="Show advanced features" CssClass="checkbox" />
                    </asp:Label>
                </div>
            </div>

            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Active" CssClass="control-label">Active:</asp:Label>
                <div class="controls check">
                    <asp:Label runat="server" AssociatedControlID="Active" CssClass="checkbox inline">
                        <asp:CheckBox runat="server" ID="Active" Text="User active" CssClass="checkbox" />
                    </asp:Label>
                </div>
            </div>
    
            <div id="password-row" class="control-group">
                <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label"><b>*</b> Password:</asp:Label>
                <asp:TextBox runat="server" ID="Password" TextMode="Password" MaxLength="32" CssClass="input-xxlarge" />
                <asp:CustomValidator ID="PasswordValidator" runat="server" ErrorMessage="Password is required" Text="Required" Display="Dynamic" ClientValidationFunction="validatePassword" OnServerValidate="ValidatePassword" />
                <asp:Literal runat="server" ID="PasswordPrompt"><br /><em style="margin-left: 285px;">Leave field blank if you do NOT want to change the password.</em></asp:Literal>
            </div>
        </fieldset>
    </div>
   </div>
  </div>
 </div>   

</asp:Content>
<asp:Content ContentPlaceHolderID="StatusBar" runat="server">

     <div class="form-actions">
        <div class="back"><asp:HyperLink ID="Back" runat="server" Text="<i class=icon-arrow-left icon-white></i>Back" CssClass="btn btn-inverse" /></div>    
        <div class="savebtn">
            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="Save" CssClass="btn btn-success" />
            <asp:CheckBox runat="server" ID="SaveAndExit" Text="Save &amp; exit" CssClass="save-exit" />
        </div>
    </div>
    
  

</asp:Content>