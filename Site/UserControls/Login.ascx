<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="Kay.Site.Public.Common.UserControls.Login" %>

<div class="quick-login">
    
    <%-- Login form --%>
    <asp:Panel runat="server" ID="LoginForm" DefaultButton="LoginButton">
        
        <div class="form">

            <div class="row">
                <asp:Label ID="Label1" runat="server" AssociatedControlID="EmailAddress" Text="E-mail:" />
                <asp:TextBox ID="EmailAddress" runat="server" MaxLength="256" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="EmailAddress" ErrorMessage="E-mail address is required" Display="Dynamic" ValidationGroup="Login"  />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="EmailAddress" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="E-mail address is invalid" runat="server" Display="Dynamic" ValidationGroup="Login" />
            </div>

            <div class="row">
                <asp:Label ID="Label2" runat="server" AssociatedControlID="Password" Text="Password:" />
                <asp:TextBox ID="Password" runat="server" TextMode="Password" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" ErrorMessage="Password is required" Text="Required" Display="Dynamic" ValidationGroup="Login"  />
            </div>

        </div>

        <p class="button">
            <asp:Button ID="LoginButton" runat="server" OnClick="LoginUser" Text="Login" ValidationGroup="Login" />
        </p>

    </asp:Panel>

</div>