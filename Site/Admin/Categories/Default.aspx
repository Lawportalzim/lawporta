<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kay.Site.Admin.Categories.Default" Title="Categories" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">

    <h2><asp:Literal runat="server" ID="PageTitle" /></h2>
    <div id="dialog" ></div>
   
    
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <div class="top-bar"> 
        <div class="addLink"><asp:HyperLink runat="server" id="AddButton" Text="<i class=icon-plus icon-white></i>Add Category" CssClass="btn btn-success" /></asp:HyperLink></div>        
    </div>
    <div class="row-fluid">		
	  <div class="box span12">
			<div class="box-header well" data-original-title>
            <h2><i class="icon-user"></i> <asp:Literal Id="CategoryTitle" runat="server" /></h2>
                <div class="drop" style="float: right">
					<asp:DropDownList ID="CategoryType" runat="server" OnSelectedIndexChanged="SetCategoryType" AutoPostBack="true" />
				</div>
			</div>                    
		<div class="box-content">
    <asp:Panel ID="Empty" runat="server" CssClass="empty">
        <p><asp:Literal Id="EmptyMessage" runat="server" /></p>
    </asp:Panel>

    <asp:DataGrid runat="server" ID="List" AutoGenerateColumns="false" GridLines="None"
        CssClass="table table-striped table-bordered bootstrap-datatable datatable" HeaderStyle-CssClass="header" AlternatingItemStyle-CssClass="alt" 
        AllowPaging="false" PagerStyle-Visible="false" AllowSorting="true" OnSortCommand="SortList"
        OnItemDataBound="BindListItem">
				
		<Columns>		
		
		    <asp:TemplateColumn  HeaderText="Name" ItemStyle-Width="480px">		    
		        <ItemTemplate>		        
		            <asp:Literal Id="Title" runat="server" />
		        </ItemTemplate>
		    </asp:TemplateColumn>
		    
		    <asp:TemplateColumn   HeaderText="Action"  ItemStyle-Width="150px" ItemStyle-CssClass="action nodrag">	    
		        <ItemTemplate>		    
		            <asp:HyperLink Id="Edit" runat="server" Text="<i class=icon-edit icon-white></i>Edit" ToolTip="Edit" CssClass="btn btn-info"/>		                               
                    <asp:HyperLink Id="View" runat="server" Text="<i class=icon-minus-sign icon-white></i>View" ToolTip="View" CssClass="btn btn-info"/>
		            <asp:HyperLink Id="Delete" runat="server" Text="<i class=icon-trash icon-white></i> Delete" ToolTip="Delete" CssClass="btn btn-danger showDialog" />		                
		        </ItemTemplate>
		    </asp:TemplateColumn>
		    
		</Columns>            
        
    </asp:DataGrid>
    </div>
   </div><!--/span-->
  </div>
  <script>
      $('document').ready(function () {
          //datatable
          $('.datatable').prepend($('<thead><tr><td></td><td></td></tr></thead>').append($(this).find('tr:first'))).dataTable({
              "sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span12'i><'span12 center'p>>",
              "sPaginationType": "bootstrap",
              "aaSorting": [],
              "oLanguage": {
                  "sLengthMenu": "_MENU_ records per page"
              }
          });
      });      

  </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="StatusBar" runat="server">

    <p class="info"><asp:Literal ID="Info" runat="server" /></p> 
    
     <div class="form-actions">
        <div class="back"><asp:HyperLink ID="Back" runat="server" Text="<i class=icon-arrow-left icon-white></i>Back" CssClass="btn btn-inverse" /></div>            
    </div>   
    
</asp:Content>