<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kay.Site.Admin.Cases.Default" Title="Cases" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">
    <h2><asp:Literal runat="server" ID="PageTitle" /></h2>
    <div id="dialog" ></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <div class="top-bar"> 
        <div class="addLink"><asp:HyperLink runat="server" id="AddButton" Text="<i class=icon-plus icon-white></i>Add Case" CssClass="btn btn-success" /></asp:HyperLink></div>        
    </div>
    <div class="row-fluid">		
	  <div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-user"></i> Cases</h2>	
                        <div class="drop" style="float: right;">
					        <asp:DropDownList ID="CategoryType" runat="server" OnSelectedIndexChanged="SetCategoryType" AutoPostBack="true" />
				        </div>					
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
            <asp:TemplateColumn HeaderText="Number"  ItemStyle-Width="50px">		    
		        <ItemTemplate>		        
		            <asp:Literal Id="Number" runat="server" />                    		            			            
		        </ItemTemplate>
		    </asp:TemplateColumn>
		
		    <asp:TemplateColumn HeaderText="Parties" ItemStyle-Width="300px">		    
		        <ItemTemplate>		        
		            <asp:Literal Id="Parties" runat="server" />
                    <asp:Literal Id="Appeal" runat="server" />			            			            
		        </ItemTemplate>
		    </asp:TemplateColumn>
		
		    <asp:TemplateColumn HeaderText="Type"  HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
		        <ItemTemplate>		        
		            <asp:Literal ID="CaseType" runat="server" />            
		        </ItemTemplate>
		    </asp:TemplateColumn>
		
		    <asp:TemplateColumn HeaderText="Court Name" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
		        <ItemTemplate>		        
		            <asp:Literal ID="CourtName" runat="server" />            
		        </ItemTemplate>
		    </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Categories" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
		        <ItemTemplate>		        
		            <asp:Literal ID="CategoriesCount" runat="server" />            
		        </ItemTemplate>
		    </asp:TemplateColumn>
		    
		    <asp:TemplateColumn HeaderText="Action" ItemStyle-Width="380px" ItemStyle-CssClass="action">	    
		        <ItemTemplate>		    
		            <asp:HyperLink Id="Edit" runat="server" Text="<i class=icon-edit icon-white></i>Edit" ToolTip="Edit" CssClass="btn btn-info" />
                    <asp:HyperLink Id="Description" runat="server" Text="<i class=icon-edit icon-white></i>Summaries" ToolTip="Description" CssClass="btn btn-info" />
                    <asp:HyperLink Id="Appealed" runat="server" Text="<i class=icon-edit icon-white></i>Appeal" ToolTip="Appeal" CssClass="btn btn-info" />		                               			                               	
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
          $('.datatable').prepend($('<thead><tr><td></td><td></td><td></td><td></td><td></td></tr></thead>').append($(this).find('tr:first'))).dataTable({
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
    

</asp:Content>