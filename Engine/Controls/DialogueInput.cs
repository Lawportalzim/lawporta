using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Kay.Global;
using Kay.BLL;

namespace Kay.Controls
{
    public class DialogueInput : TextBox
    {
        // Properties
        public XmlNode XmlNode
        {
            get
            {
                return Utilities.StringToXmlNode(Text);
            }
            set
            {
                Text = value.OuterXml;
            }
        }
        public Boolean Required = false;
        public String RequiredErrorMessage = String.Empty;
        public String SavePath = "/data/files/";
        public String DialoguePath = "dialogue.aspx";
        public int DialogueWidth = 700;
        public int DialogueHeight = 520;

        // Init
        protected override void OnInit(EventArgs e)
        {
            // base
            base.OnInit(e);
        }

        // Render
        public override void RenderControl(HtmlTextWriter writer)
        {
            String _title = XmlNode.Attributes["title"].FirstChild.Value;

            // wrap
            writer.Write("<div class=\"dialogue-input\">");
            writer.Write(String.Format("<div>{0}</div>", _title));

            // base (input control)
            CssClass = "ignore";
            base.RenderControl(writer);
            
            // close
            writer.Write("</div>");

            // required field validator
            if (Required)
            {
                CustomValidator req = new CustomValidator();
                req.ClientValidationFunction = "kay.controls.dialogueinput.validate";
                req.ServerValidate += new ServerValidateEventHandler(req_ServerValidate);
                req.ControlToValidate = this.ID;
                req.Display = ValidatorDisplay.Dynamic;
                req.ErrorMessage = RequiredErrorMessage;
                req.Text = "Required";
                Controls.Add(req);
                req.RenderControl(writer);
            }

            // javascript
            String js = String.Format("kay.controls.dialogueinput.init('{0}', '{1}', {2}, {3}, '{4}');", this.ClientID, DialoguePath, DialogueWidth, DialogueHeight, SavePath);
            Page.ClientScript.RegisterStartupScript(GetType(), this.ClientID, js, true);
        }

        // Server side validation
        void req_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = XmlNode.Attributes["title"].Value != String.Empty;
        }
    }
}
