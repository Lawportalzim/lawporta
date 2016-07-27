using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Kay.Controls
{
    public class Textarea : System.Web.UI.WebControls.TextBox
    {
        // Properties
        public String Label = "Text";

        // Init
        protected override void OnInit(EventArgs e)
        {
            // Force textarea
            this.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;

            // Base method
            base.OnInit(e);
        }

        // Pre-render
        protected override void OnPreRender(EventArgs e)
        {
            // Base method
            base.OnPreRender(e);
        }
        
        // Render
        public override void RenderControl(HtmlTextWriter writer)
        {
            // wrap
            writer.Write("<div class=\"kay-textarea\">");

            // base
            base.RenderControl(writer);

            // close
            writer.Write(String.Format("<em id=\"{1}_Feedback\">{0} characters remaining</em>", MaxLength, this.ClientID));
            writer.Write("</div>");

            // length validation
            CustomValidator req = new CustomValidator();
            req.ClientValidationFunction = "kay.controls.textarea.validate";
            req.ServerValidate += new ServerValidateEventHandler(req_ServerValidate);
            req.ControlToValidate = this.ID;
            req.Display = ValidatorDisplay.Dynamic;
            req.ErrorMessage = String.Format("{0} is too long - {1} chars max", Label, MaxLength);
            req.Text = "Too long";
            Controls.Add(req);
            req.RenderControl(writer);

            // javascript
            String js = String.Format("kay.controls.textarea.init('{0}', {1});", this.ClientID, MaxLength);
            Page.ClientScript.RegisterStartupScript(GetType(), this.ClientID, js, true);
        }

        // Server side validation
        void req_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.Text.Length <= MaxLength;
        }
    }
}
