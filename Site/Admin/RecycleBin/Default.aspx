<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kay.Site.Admin.RecycleBin.Default" Title="Recycle Bin" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">
    <h2>Cases</h2>
    <div id="dialog" ></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <div class="top-bar">
        
    </div>
    <!--/Cases-->

    <div class="row-fluid sortable">		
	  <div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-user"></i>Recycled Cases</h2>						
					</div>
		<div class="box-content">
            <asp:Panel ID="Empty" runat="server" CssClass="empty">
                <p><asp:Literal Id="EmptyMessage" runat="server" /></p>
            </asp:Panel>
    
    <asp:DataGrid runat="server" ID="CaseList" AutoGenerateColumns="false" GridLines="None"
        CssClass="table table-striped table-bordered bootstrap-datatable datatable" HeaderStyle-CssClass="header" AlternatingItemStyle-CssClass="alt" 
        AllowPaging="true" PagerStyle-Visible="false" AllowSorting="true" OnSortCommand="SortList"
        OnItemDataBound="BindCaseListItem">
       
				
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
                    <asp:HyperLink Id="Restore" runat="server" Text="<i class=icon-edit icon-white></i>Restore" ToolTip="Restore" CssClass="btn btn-info" />		                               			                               	
		            <asp:HyperLink Id="Delete" runat="server" Text="<i class=icon-trash icon-white></i> Delete" ToolTip="Delete" CssClass="btn btn-danger showDialog" />		        
		        </ItemTemplate> 
		    </asp:TemplateColumn>
		    
		</Columns>            
        
    </asp:DataGrid>
        </div>
      </div><!--/span-->
  </div>	


    <!--/categories-->
    <div class="row-fluid">		
	  <div class="box span12">
			<div class="box-header well" data-original-title>
                <h2><i class="icon-user"></i>Recycled Categories</h2>
			</div>                    
		<div class="box-content">

    <asp:DataGrid runat="server" ID="CategoryList" AutoGenerateColumns="false" GridLines="None"
        CssClass="table table-striped table-bordered bootstrap-datatable" HeaderStyle-CssClass="header" AlternatingItemStyle-CssClass="alt" 
        AllowPaging="true" PagerStyle-Visible="false" AllowSorting="true" OnSortCommand="SortList"
        OnItemDataBound="BindCategoryListItem">
				
		<Columns>		
		
		    <asp:TemplateColumn HeaderText="Name" ItemStyle-Width="480px">		    
		        <ItemTemplate>		        
		            <asp:Literal Id="Title" runat="server" />
		        </ItemTemplate>
		    </asp:TemplateColumn>
		    
		    <asp:TemplateColumn  HeaderText="Action"  ItemStyle-Width="150px" ItemStyle-CssClass="action nodrag">	    
		        <ItemTemplate>		    		            	                               
                    <asp:HyperLink Id="Restore" runat="server" Text="<i class=icon-minus-sign icon-white></i>Restore" ToolTip="Restore" CssClass="btn btn-info"/>
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
    

</asp:Content>