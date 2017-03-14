<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Kay.Site.Admin.Companies.Edit" Title="Edit company" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">

    <h2><asp:Literal ID="PageTitle" runat="server" /></h2>
    
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">

    <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="validation" />
    
<div class="row-fluid sortable">
    <div class="box span12">
	    <div class="box-header well" data-original-title>
		    <h2><i class="icon-edit"></i> Company information</h2>			
	    </div>
	    <div class="box-content">
        <div class="form-horizontal">
        <fieldset>
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Name" CssClass="control-label"><b>*</b> Name:</asp:Label>
                <asp:TextBox runat="server" ID="Name" MaxLength="64" CssClass="input-xxlarge" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Name" ErrorMessage="First name is required" Text="Required"  />
            </div>   
            
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="EmailAddress" CssClass="control-label"><b>*</b> Email Address:</asp:Label>
                <asp:TextBox runat="server" ID="EmailAddress" MaxLength="256" CssClass="input-xxlarge" />
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="EmailAddress" ErrorMessage="E-mail address is required" Text="Required" Display="Dynamic"  />                
                <span id="ValidateEmail" style="display:none;" class="Validate"></span>
            </div>
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Telephone" CssClass="control-label">Telephone:</asp:Label>
                <asp:TextBox runat="server" ID="Telephone" MaxLength="64" CssClass="input-xxlarge" />
            </div>
    
            <div class="control-group">
                <asp:Label ID="Label2" runat="server" AssociatedControlID="ContactPerson" CssClass="control-label"><b>*</b> Contact Person:</asp:Label>
                <asp:TextBox runat="server" ID="ContactPerson" MaxLength="64" CssClass="input-xxlarge" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ContactPerson" ErrorMessage="Contact Person is required" Text="Required" Display="Dynamic"  />
                <span id="Span3" style="display:none;" class="validate"></span>
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
                <asp:Label runat="server" AssociatedControlID="NumberOfAccounts" CssClass="control-label">Number Of Accounts:</asp:Label>
                <asp:TextBox runat="server" ID="NumberOfAccounts" MaxLength="128" CssClass="input-xxlarge" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="NumberOfAccounts" ErrorMessage="Number Of Accounts is required" Text="Required" Display="Dynamic"  />
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
            <asp:LinkButton ID="SaveButton" runat="server" Text="Save" OnClick="Save" CssClass="btn btn-success" />
            <asp:CheckBox runat="server" ID="SaveAndExit" Text="Save &amp; exit" CssClass="save-exit" />
        </div>
    </div>
    
    <script type="text/javascript">
        var req;
        $(document).ready(function()
        {            

            // Unique e-mail address
            $('#ctl00_Content_EmailAddress').bind('keyup', function()
            {
                // Hide validation fields
                $('#ValidateEmail,#ctl00_Content_EmailValid,#ctl00_Content_EmailRequired').hide();
            });
        });

        // Validate password
        function validatePassword(oSrc, args)
        {
            if ($('#ctl00_Content_Admin').attr('checked') && $('#ctl00_Content_Password').val() == '')
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;            
            }
        }
        
    </script>

</asp:Content>