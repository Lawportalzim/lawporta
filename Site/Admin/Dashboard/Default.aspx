<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Kay.Site.Admin.Dashboard.Default" Title="Dashboard" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">
    <h2>Dashboard</h2>
</asp:Content>
<asp:Content ID="ContentBar" ContentPlaceHolderID="Content" runat="server">
    <div class="row-fluid sortable ui-sortable">
				<div class="box span4">
					<div data-original-title="" class="box-header well">
						<h2><i class="icon-user"></i> User Activity</h2>
						<div class="box-icon">
							<a class="btn btn-minimize btn-round" href="#"><i class="icon-chevron-up"></i></a>
							<a class="btn btn-close btn-round" href="#"><i class="icon-remove"></i></a>
						</div>
					</div>
					<div class="box-content">
						<div class="box-content">
                        <asp:Repeater ID="List" runat="server" OnItemDataBound="BindUsers">
                            <HeaderTemplate>
        
                                <ul class="dashboard-list">
            
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="Name" />
                                <asp:Literal runat="server" ID="Username" />
								<asp:Literal runat="server" ID="ExpiryDate" />
								<asp:Literal runat="server" ID="DaysLeft" />
                                <asp:Literal runat="server" ID="Company" />  
                            </ItemTemplate>
                            <FooterTemplate>

                                </ul>
            
                            </FooterTemplate>
                        </asp:Repeater>
							
						</div>
					</div>
				</div><!--/span-->
			</div>
    
</asp:Content>