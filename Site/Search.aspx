<%@ Page Language="C#" MasterPageFile="~/Site.master" EnableViewState="false" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Site.Search" Title="Search results" %>
<%@ MasterType TypeName="Site.SiteMaster" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <p><asp:Literal runat="server" ID="SearchSummary" /></p>

    <%--Cases --%>
    <asp:Repeater ID="CasesList" runat="server" OnItemDataBound="BindCasesResult">
        <HeaderTemplate>
            
            <div class="search-results">
            <h2><asp:Literal ID="SearchType" runat="server" /></h2>
            <div id="list">
        </HeaderTemplate>
        <ItemTemplate>        
                
                <div>            
                    <h3><asp:HyperLink ID="Title" runat="server" /></h3>                    
                    <p>
                        <asp:Literal ID="Description" runat="server" />
                    </p>           
                </div>
                       
        </ItemTemplate>
        <FooterTemplate>
            </div>
            </div>
        </FooterTemplate>
    </asp:Repeater>

    <Kay:Pager runat="server" ID="CasesPager" />

    <%--Desciptions --%>
    <asp:Repeater ID="DescriptionsList" runat="server" OnItemDataBound="BindDescriptionsResult">
        <HeaderTemplate>
            
            <div class="search-results">
            <h2><asp:Literal ID="SearchType" runat="server" /></h2>
            <div id="list">
        </HeaderTemplate>
        <ItemTemplate>        
                
                <div>            
                    <h3><asp:HyperLink ID="Title" runat="server" /></h3>                    
                    <p>
                        <asp:Literal ID="Description" runat="server" />
                    </p>           
                </div>
                      
        </ItemTemplate>
        <FooterTemplate>
            </div>
            </div>
            
        </FooterTemplate>
    </asp:Repeater>

    <Kay:Pager runat="server" ID="DescriptionsPager" />

    <%--Desciptions --%>
    <asp:Repeater ID="CategoriesList" runat="server" OnItemDataBound="BindCategoriesResult">
        <HeaderTemplate>
            
            <div class="search-results">
            <h2><asp:Literal ID="SearchType" runat="server" /></h2>
            <div id="articles">
        </HeaderTemplate>
        <ItemTemplate>        
                
                <div>            
                    <h3><asp:HyperLink ID="Title" runat="server" /></h3>
                </div>
                       
        </ItemTemplate>
        <FooterTemplate>
            </div>
            </div>
            
        </FooterTemplate>
    </asp:Repeater>

    <Kay:Pager runat="server" ID="CategoriesPager" />
    <asp:HyperLink runat="server" ID="Back" Text="Back" CssClass="back" NavigateUrl="javascript:history.go(-1)" />
    <asp:HyperLink runat="server" ID="MainMenu" Text="Main menu" CssClass="back mainmenu" NavigateUrl="/cases/civil" />
</asp:Content>
