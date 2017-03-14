<%@ Page Language="C#"  MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Kay.Site.Admin.Login" Title="Login" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content runat="server" ContentPlaceHolderID="Content">
    
    <asp:Panel id="LoginForm" runat="server" DefaultButton="LoginButton" CssClass="well span5 center login-box">
        <div class="alert alert-info">
						Please login with your Username and Password.
		</div>  
        <div class="form-horizontal"> 
            <fieldset>
                <asp:Literal ID="LoginFeedback" runat="server" />
        
               
                
        
                <div class="input-prepend" title="Username" data-rel="tooltip">
					<span class="add-on"><i class="icon-user"></i></span>
                    <asp:TextBox ID="EmailAddress" runat="server" MaxLength="256" TabIndex="1" CssClass="input-large span10" />
				</div>
				<div class="clearfix"></div>

                <div class="input-prepend" title="Password" data-rel="tooltip">
					<span class="add-on"><i class="icon-lock"></i></span>                    
                    <asp:TextBox ID="Password" CssClass="input-large span10" Text="Password" TextMode="Password" runat="server" MaxLength="16" TabIndex="2" />
				</div>
				<div class="clearfix"></div>


                <p class="center span5">				    
                    <asp:Button Id="LoginButton" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="LoginUser" />
				</p>       
                
            </fieldset>
        </div> 
    </asp:Panel>  
 
            
</asp:Content>