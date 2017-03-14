<%@ Page Language="C#" MasterPageFile="~/Admin/Common/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Kay.Site.Admin.Cases.Edit" Title="Edit Case" %>
<%@ MasterType TypeName="Kay.Site.Admin.Common.MasterPages.Default" %>

<asp:Content ContentPlaceHolderID="TitleBar" runat="server">

    <h2><asp:Literal ID="PageTitle" runat="server" /></h2>
    
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">

    <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="validation" />  

<div class="row-fluid sortable">
	<div class="box span12">
		<div class="box-header well" data-original-title>
			<h2><i class="icon-edit"></i> Case Information</h2>			
		</div>
		<div class="box-content">
     <div class="form-horizontal">
            <fieldset>
                <div class="control-group">
                    <asp:Label runat="server" AssociatedControlID="Number" CssClass="control-label"><b>*</b> Number:</asp:Label>
                    <asp:TextBox runat="server" ID="Number" MaxLength="256" CssClass="input-xxlarge" AutoPostBack="true" OnTextChanged="CheckCaseNumber" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Number" ErrorMessage="Number is required" Text="Required" Display="Dynamic"  />
                    <span id="Span6" style="display:none;" class="validate"></span>
                </div>

                <div class="control-group">
                    <asp:Label runat="server" AssociatedControlID="Plaintiff" CssClass="control-label"><b>*</b> Plaintiff:</asp:Label>
                    <asp:TextBox runat="server" ID="Plaintiff" MaxLength="256" CssClass="input-xxlarge" />
                    <asp:RequiredFieldValidator ID="ItemTitleRequired" runat="server" ControlToValidate="Plaintiff" ErrorMessage="Plaintiff is required" Text="Required" Display="Dynamic"  />
                    <span id="ValidateItemTitle" style="display:none;" class="validate"></span>
                </div> 
    
                <div class="control-group">
                    <asp:Label runat="server" AssociatedControlID="Defendant" CssClass="control-label"><b>*</b> Defendant:</asp:Label>
                    <asp:TextBox runat="server" ID="Defendant" MaxLength="256" CssClass="input-xxlarge" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Defendant" ErrorMessage="Defendant is required" Text="Required" Display="Dynamic"  />
                    <span id="Span1" style="display:none;" class="validate"></span>
                </div> 
    
                <div class="control-group">
                    <asp:Label runat="server" AssociatedControlID="Dates" CssClass="control-label"><b>*</b> Dates:</asp:Label>
                    <asp:TextBox runat="server" ID="Dates" MaxLength="256" CssClass="input-xxlarge" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Dates" ErrorMessage="Dates are required" Text="Required" Display="Dynamic"  />
                    <span id="Span2" style="display:none;" class="validate"></span>
                </div> 
                
                <div class="control-group">
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="Ruler" CssClass="control-label"><b>*</b> Judge Name:</asp:Label>
                    <asp:TextBox runat="server" ID="Ruler" MaxLength="256" CssClass="input-xxlarge" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Ruler" ErrorMessage="Megistrate/Judge Name is required" Text="Required" Display="Dynamic"  />
                    <span id="Span5" style="display:none;" class="validate"></span>
                </div>

                <div class="control-group">
                    <asp:Label runat="server" AssociatedControlID="CourtName" CssClass="control-label"><b>*</b> Court Name:</asp:Label>
                    <asp:TextBox runat="server" ID="CourtName" MaxLength="256" CssClass="input-xxlarge" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="CourtName" ErrorMessage="Court Name is required" Text="Required" Display="Dynamic"  />
                    <span id="Span3" style="display:none;" class="validate"></span>
                </div> 

                <div class="control-group">
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="Notes" CssClass="control-label"><b>*</b> Notes:</asp:Label>                    
                    <asp:TextBox TextMode="MultiLine" runat="server" ID="Notes" CssClass="cleditor" Rows="3" />                    
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Notes" ErrorMessage="Notes are required" Text="Required" Display="Dynamic"  />
                    <span id="Span4" style="display:none;" class="validate"></span>                    
                </div>

                <div class="control-group">
                    <asp:Label runat="server" AssociatedControlID="FullCase" CssClass="control-label"><b>*</b> Full Case:</asp:Label>                    
                    <asp:TextBox TextMode="MultiLine" runat="server" ID="FullCase" CssClass="cleditor" Rows="3" />                    
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="FullCase" ErrorMessage="Notes are required" Text="Required" Display="Dynamic"  />
                    <span id="Span7" style="display:none;" class="validate"></span>                    
                </div>

                <div class="control-group" style="display: none;">
                    <label class="control-label">Civil Case</label>
                    <div class="controls check">
                        <asp:Label runat="server" AssociatedControlID="CaseType" CssClass="checkbox inline">
                            <asp:CheckBox runat="server" ID="CaseType" Text="Case Civil" CssClass="checkbox" />
                        </asp:Label>
                    </div>
                </div>
        </fieldset>
      </div>
   </div>
  </div>
 </div>
    
</asp:Content>
<asp:Content ContentPlaceHolderID="StatusBar" runat="server">
    <div class="form-actions">
        <div class="back"><asp:HyperLink ID="Back" runat="server" Text="<i class=icon-arrow-left icon-white></i>Back" CssClass="btn btn-inverse" /></div>    
        <div class="savebtn">
            <asp:LinkButton ID="SaveButton" runat="server" Text="Save" OnClick="Save" CssClass="btn btn-success" />
            <asp:CheckBox runat="server" ID="SaveAndExit" Text="Save &amp; exit" CssClass="save-exit" />
        </div>
    </div>
    <script type="text/javascript">
        var req;
        $(document).ready(function() {
        });
    </script>

</asp:Content>