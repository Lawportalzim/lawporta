<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Kay.Site.Admin.Descriptions.Edit" Title="Edit Category" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">

    <h2><asp:Literal ID="PageTitle" runat="server" /></h2>
    
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">

    <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="validation" />
<div class="row-fluid sortable">
<div class="box span12">
	<div class="box-header well" data-original-title>
		<h2><i class="icon-edit"></i> Category Information</h2>			
	</div>
	<div class="box-content">
    <div class="form-horizontal">
        <fieldset>    
    
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Category" CssClass="control-label"><b>*</b> Category:</asp:Label>
                <asp:DropDownList runat="server" ID="Category" CssClass="input-xxlarge" AutoPostBack="true"  OnSelectedIndexChanged="SetParentCategory" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Category" ErrorMessage="Category is required" Text="Required" Display="Dynamic"  />
                <span id="Span3" style="display:none;" class="validate"></span>
            </div>  
            
            <asp:Panel ID="SubCatPanel" runat="server" CssClass="control-group">
                <asp:Label runat="server" AssociatedControlID="SubCategory" CssClass="control-label">Sub Category:</asp:Label>
                <asp:DropDownList runat="server" ID="SubCategory" CssClass="input-xxlarge" />                
            </asp:Panel>   

            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="Description" CssClass="control-label"><b>*</b> Description:</asp:Label>                    
                <asp:TextBox TextMode="MultiLine" runat="server" ID="Description" CssClass="cleditor" Rows="3" />                    
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Description" ErrorMessage="Description is required" Text="Required" Display="Dynamic"  />
                <span id="Span4" style="display:none;" class="validate"></span>                    
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
        $(document).ready(function () {

        });
    </script>

</asp:Content>