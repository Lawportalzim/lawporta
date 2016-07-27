using System;
using System.Web.UI;
using System.Text;

namespace Kay.Controls
{
    public class Editor : System.Web.UI.WebControls.TextBox
    {
        // Properties
        public String Toolbar = "Simple";
        public String UploadPath = String.Empty;
        public String CallbackFunction = "null";
        public String BodyClass = String.Empty;

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
            // Global script
            Page.ClientScript.RegisterClientScriptInclude("CKEditor", "/compress.axd?s=/shared/ckeditor/ckeditor.js");
            Page.ClientScript.RegisterClientScriptInclude("CKEditorJquery", "/compress.axd?s=/shared/ckeditor/adapters/jquery.js");

            // Build options
            StringBuilder Options = new StringBuilder();
            Options.Append("{");
            if (!String.IsNullOrEmpty(UploadPath))
            {
                Options.Append(String.Format("filebrowserUploadUrl: '/shared/ckeditor/files/upload.aspx?path={0}', ", UploadPath));
                Options.Append(String.Format("filebrowserImageUploadUrl: '/shared/ckeditor/files/upload.aspx?path={0}', ", UploadPath));
            }
            Options.Append(String.Format("toolbar: '{0}'", Toolbar));
            if (!String.IsNullOrEmpty(BodyClass))
            {
                Options.Append(String.Format("bodyClass: 'editor {0}'", BodyClass));
            }
            Options.Append("}");

            // Instance script
            String Key = String.Format("{0}_Init", this.ClientID);
            String Script = String.Format("kay.editors.push($('#{0}').ckeditor({2}, {1}));", this.ClientID, CallbackFunction, Options.ToString());
            Page.ClientScript.RegisterStartupScript(GetType(), Key, Script, true);

            // Add class
            CssClass += " ignore";

            // Base method
            base.OnPreRender(e);
        }

        // Begin tag
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            // Start div
            writer.Write("<div class=\"kay-editor\">");

            // Base method
            base.RenderBeginTag(writer);
        }

        // End tag
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            // Base method
            base.RenderEndTag(writer);

            // End div
            writer.Write("</div>");
        }
    }
}
