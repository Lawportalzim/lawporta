<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Kay.Site.Admin.Categories.Edit" Title="Edit category" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">

    <h2><asp:Literal ID="PageTitle" runat="server" /></h2>
    
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">

<asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="validation" />
<div class="row-fluid sortable">
<div class="box span12">
	<div class="box-header well" data-original-title>
		<h2><i class="icon-edit"></i> Details</h2>			
	</div>
	<div class="box-content">
    <div class="form-horizontal">
        <fieldset>    
        
    <div class="control-group">
        <asp:Label runat="server" AssociatedControlID="ItemTitle"  CssClass="control-label"><b>*</b> Title:</asp:Label>
        <asp:TextBox runat="server" ID="ItemTitle" MaxLength="128"  CssClass="input-xxlarge" />
        <asp:RequiredFieldValidator ID="ItemTitleRequired" runat="server" ControlToValidate="ItemTitle" ErrorMessage="Title is required" Text="Required" Display="Dynamic"  />
        <span id="ValidateItemTitle" style="display:none;" class="validate"></span>
    </div>
    
    <div class="control-group">
        <label class="control-label">Live</label>
        <div class="controls check">
            <asp:Label runat="server" AssociatedControlID="Live" CssClass="checkbox inline">
                <asp:CheckBox runat="server" ID="Live" Text="Category is live" CssClass="checkbox" />
            </asp:Label>
        </div>
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
            // Unique
            $('#ctl00_Content_ItemTitle').bind('keyup', function()
            {
                // Hide validation fields
                $('#ValidateItemTitle,#ctl00_Content_ItemTitleRequired').hide();

                // Get details
                var id = kay.utils.getQsVar('id');
                var title = $("#ctl00_Content_ItemTitle").val();
                var type = kay.utils.getQsVar('type');

                // Validate correct format
                if (title != '')
                {
                    if (req) req.abort();
                    req = $.post('default.aspx?action=validate&id=' + id, { title: title, type: type },
                        function(data)
                        {
                            $('#ValidateItemTitle').html(data).show();
                            if (data == "") $('#ValidateItemTitle').hide();
                        });
                }
            });
        });
        
    </script>

</asp:Content>