<%@ Page Language="C#"  MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kay.Site.Admin.Users.Default" Title="Users" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">

    <h2>Users <asp:Literal Id="CompanyTitle" runat="server" /></h2>
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="top-bar"> 
        <div class="addLink"><asp:HyperLink runat="server" ID="EditLink" CssClass="btn btn-success" Text="<i class='icon-plus icon-white'></i>Add User" /></div>
    </div>
    <div class="row-fluid sortable">		
	  <div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-user"></i> Users</h2>						
					</div>
		<div class="box-content">
            <asp:Panel ID="Empty" runat="server" CssClass="empty">
                <p><asp:Literal Id="EmptyMessage" runat="server" /></p>
            </asp:Panel>

    <asp:DataGrid runat="server" ID="List" AutoGenerateColumns="false" GridLines="None"
        CssClass="table table-striped table-bordered bootstrap-datatable datatable" HeaderStyle-CssClass="header" AlternatingItemStyle-CssClass="alt" 
        AllowPaging="true" PagerStyle-Visible="false" AllowSorting="true" OnSortCommand="SortList"
        OnItemDataBound="BindListItem">
				
		<Columns>		
		
		    <asp:TemplateColumn HeaderText="Name" >		    
		        <ItemTemplate>	
                    <asp:Literal Id="Status" runat="server" />	        
		            <asp:Literal Id="Name" runat="server" />		            
		        </ItemTemplate>
		    </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Company" ItemStyle-Width="200px">		    
		        <ItemTemplate>		        
		            <asp:Literal Id="Company" runat="server" />		            
		        </ItemTemplate>
		    </asp:TemplateColumn>
		
		    <asp:TemplateColumn HeaderText="Username" ItemStyle-Width="300px">		    
		        <ItemTemplate>		        
		            <asp:Literal ID="EmailAddress" runat="server" />            
		        </ItemTemplate>
		    </asp:TemplateColumn>
		    
		    <asp:TemplateColumn HeaderText="&nbsp;" ItemStyle-Width="200px" ItemStyle-CssClass="action">	    
		        <ItemTemplate>		    
		            <asp:HyperLink Id="Edit" runat="server" Text="<i class=icon-edit icon-white></i>Edit" ToolTip="Edit" CssClass="btn btn-info" />
		            <asp:HyperLink Id="Delete" runat="server" Text="<i class=icon-trash icon-white></i> Delete" ToolTip="Delete" CssClass="btn btn-danger" />		        
		        </ItemTemplate>
		    </asp:TemplateColumn>
		    
		</Columns>            
        
    </asp:DataGrid>

       </div>
     </div><!--/span-->
  </div>	

</asp:Content>
<asp:Content ContentPlaceHolderID="StatusBar" runat="server">

    <p class="info"><asp:Literal ID="Info" runat="server" /></p> 
    <script>
         $('document').ready(function () {
             //datatable
             $('.datatable').prepend($('<thead><tr><td></td><td></td><td></td></tr></thead>').append($(this).find('tr:first'))).dataTable({
                 "sDom": "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span12'i><'span12 center'p>>",
                 "sPaginationType": "bootstrap",
                 "oLanguage": {
                     "sLengthMenu": "_MENU_ records per page"
                 }
             });
         });

      

  </script>
</asp:Content>