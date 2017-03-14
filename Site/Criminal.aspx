<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"   CodeBehind="Criminal.aspx.cs" Inherits="Site.Criminal" %>
<%@ MasterType TypeName="Site.SiteMaster" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
     <%-- Empty --%>
    <asp:Literal ID="EmptyList" runat="server">
        <p>There are no cases.</p>
    </asp:Literal>

    <asp:Literal ID="ParentCatText" runat="server" />

    <%-- List --%>
    <asp:Repeater ID="List" runat="server" OnItemDataBound="BindCase">
        <HeaderTemplate>
        
            <div id="list">
            
        </HeaderTemplate>
        <ItemTemplate>
        
                <asp:Panel runat="server" ID="Row">
                    <h3><asp:Hyperlink ID="ApealStatus" runat="server" /> <asp:HyperLink ID="Parties" runat="server" CssClass="parties" /> <br /> <small class="by">Ruled By: <asp:Hyperlink ID="Ruler" runat="server" /></small></h3>                                        
                    <div>
                        <p><strong><asp:Literal ID="Dates" runat="server" /></strong>
                         <asp:Literal ID="Summary" runat="server" /></p> 
                    </div>
                </asp:Panel>
            
        </ItemTemplate>
        <FooterTemplate>

            </div>
            
        </FooterTemplate>
    </asp:Repeater>
    <kay:Pager runat="server" ID="ListPager" /> 

    <%-- Categories --%>
     <asp:Repeater ID="CategoryCase" runat="server" OnItemDataBound="BindCategoryCase">
        <HeaderTemplate>
        
            <div id="list">
            
        </HeaderTemplate>
        <ItemTemplate>
        
                 <asp:Panel runat="server" ID="Row">
                    <h3><asp:Hyperlink ID="ApealStatus" runat="server" /> <asp:HyperLink ID="Parties" runat="server" CssClass="parties" /> <br /><small class="by">Ruled By: <asp:Hyperlink ID="Ruler" runat="server" /></small></h3>                                        
                    <div class="synoWrapper">
                        <p><strong><asp:Literal ID="Dates" runat="server" /></strong>
                         <div class="synopsis"><asp:Literal ID="Summary" runat="server" /></div>
                         <div class="fullText" style="display: none;"><asp:Literal ID="FullSummary" runat="server" /></div>
                         <a class="moreText">More</a>
                         </p> 
                    </div>
                </asp:Panel>
            
        </ItemTemplate>
        <FooterTemplate>

            </div>
            
        </FooterTemplate>
    </asp:Repeater>
    <kay:Pager runat="server" ID="CategoryCasePager" />


    
     <%-- Detail --%>
    <asp:Panel ID="Detail" runat="server">       
        <div class="details">
            <ul class="tabs"><li class="bycat selected">View Judgment By Categories</li><li class="byfull">View Full Judgment</li></ul>
            <div id="case" class="byCategory">
               <p class="notes"><asp:Hyperlink ID="ApealStatus" runat="server" /><asp:Literal ID="Notes" runat="server" /></p>
            <%-- Categories --%>
            <asp:Repeater ID="CaseDetailList" runat="server" OnItemDataBound="BindCaseDetail">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
        
                        <div class="cases">
                            <h3><asp:HyperLink ID="Category" runat="server" /></h3>                                        
                            <div>  
                                <small></small><asp:Literal ID="Dates" runat="server" />
                                <asp:Literal ID="Summary" runat="server" />
                            </div>
                        </div>
            
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
             </asp:Repeater>
         </div>  
         <div class="fullcase">    
            <asp:Literal runat="server" ID="FullCase" /> 
         </div> 
     </div>
    </asp:Panel>
    <asp:HyperLink runat="server" ID="Back" Text="Back" CssClass="back" NavigateUrl="javascript:history.go(-1)" />
    <asp:HyperLink runat="server" ID="MainMenu" Text="Main menu" CssClass="back mainmenu" NavigateUrl="/cases/criminal" />
</asp:Content>
<asp:Content ID="Aside" runat="server" ContentPlaceHolderID="Aside">
    <%-- Category filter --%>
    <kay:CategoryFilter ID="Categories" runat="server" CategoryType="Criminal" UrlTemplate="/cases/{0}/" />
</asp:Content>