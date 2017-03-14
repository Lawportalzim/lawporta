<%@ Page Language="C#"  MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Site.Login" Title="Login" %>
<%@ MasterType TypeName="Site.SiteMaster" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:Panel id="LoginForm" runat="server" DefaultButton="LoginButton" CssClass="well span5 login-box">
        <div class="alert alert-info">
						Please login with your Username and Password.
		</div>  
        <div class="form-horizontal"> 
            <fieldset>
                <asp:Literal ID="LoginFeedback" runat="server" />    
               
                
        
                <div class="input-prepend" title="Username" data-rel="tooltip">
					<label>Username: </label> 
                    <asp:TextBox ID="EmailAddress" runat="server" MaxLength="256" TabIndex="1" CssClass="input-large span10" />
				</div>
				<div class="clearfix"></div>

                <div class="input-prepend" title="Password" data-rel="tooltip">
					<label>Password: </label>                    
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