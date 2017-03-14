<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryFilter.ascx.cs" Inherits="Kay.Site.Public.Common.UserControls.CategoryFilter" %>

<div class="list-tout">

    <asp:Repeater runat="server" ID="Categories" OnItemDataBound="BindCategory">
        <HeaderTemplate>
            <h4>Categories</h4>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
                <li>
                    <asp:HyperLink runat="server" ID="Link" />
                    <asp:Repeater runat="server" ID="SubCategories" OnItemDataBound="BindSubCategory">
                        <HeaderTemplate>                            
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <li>
                                    <asp:HyperLink runat="server" ID="Link" />
                                </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>

</div>