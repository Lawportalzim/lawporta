<%@ Page Language="C#"  MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kay.Site.Admin.Companies.Default" Title="Companies" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">

    <h2>Companies</h2>
    <div id="dialog" ></div
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="top-bar"> 
        <div class="addLink"><a href="edit.aspx" class="btn btn-success"><i class="icon-plus icon-white"></i>Add Company</a></div>
    </div>
    <div class="row-fluid sortable">		
	  <div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-company"></i> Companies</h2>						
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
		
		    <asp:TemplateColumn HeaderText="Name" ItemStyle-Width="300px">		    
		        <ItemTemplate>		        
		            <asp:Literal Id="Name" runat="server" />		            
		        </ItemTemplate>
		    </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Accounts" ItemStyle-Width="100px">		    
		        <ItemTemplate>		        
		            <asp:Literal Id="NumberOfAccounts" runat="server" />		            
		        </ItemTemplate>
		    </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Telephone" ItemStyle-Width="200px">		    
		        <ItemTemplate>		        
		            <asp:Literal Id="Telephone" runat="server" />		            
		        </ItemTemplate>
		    </asp:TemplateColumn>
		
		    <asp:TemplateColumn HeaderText="EmailAddress" ItemStyle-Width="300px">		    
		        <ItemTemplate>		        
		            <asp:Literal ID="EmailAddress" runat="server" />            
		        </ItemTemplate>
		    </asp:TemplateColumn>
		    
		    <asp:TemplateColumn HeaderText="&nbsp;" ItemStyle-Width="300px" ItemStyle-CssClass="action nodrag">	    
		        <ItemTemplate>		    
		            <asp:HyperLink Id="Edit" runat="server" Text="<i class=icon-edit icon-white></i>Edit" ToolTip="Edit" CssClass="btn btn-info" />
                    <asp:HyperLink Id="Users" runat="server" Text="<i class=icon-edit icon-white></i>Users" ToolTip="Users" CssClass="btn btn-info" />
		            <asp:HyperLink Id="Delete" runat="server" Text="<i class=icon-trash icon-white></i> Delete" ToolTip="Delete" CssClass="btn btn-danger showDialog" />		        
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