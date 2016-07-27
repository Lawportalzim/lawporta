using System;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Kay.Controls
{
    public class Form : System.Web.UI.HtmlControls.HtmlForm
    {
        new public string Action;

        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            // Name
            if (this.Name == "") throw new ApplicationException("Name attribute missing from form");
            writer.WriteAttribute("name", this.Name);
            base.Attributes.Remove("name");

            // Method
            if (this.Method == "") throw new ApplicationException("Method attribute missing from form");
            writer.WriteAttribute("method", this.Method);
            base.Attributes.Remove("method");

            // Write
            this.Attributes.Render(writer);

            // Action (add filename)            
            this.Action = Global.Config.FriendlyUrl.PathAndQuery;            
            String path = Global.Config.FriendlyUrl.LocalPath;            
            
            // Add default.aspx (if necessary)            
            // This is required because postbacks to a non-existent folder cause an error redirect            
            // which is then examined by global.asax to determine if a url rewrite is required.            
            if (this.Action.Contains("?") && !Regex.IsMatch(path, @".aspx?"))            
            {                
                String url = this.Action.Substring(0, this.Action.IndexOf('?'));                
                String qs = this.Action.Substring(this.Action.IndexOf('?'));                
                this.Action = url + "default.aspx" + qs;            
            }
            else if (this.Action.EndsWith("/"))
            {
                this.Action = this.Action + "default.aspx";
            }
            
            // Replace action attribute
            writer.WriteAttribute("action", this.Action.ToLower());
            base.Attributes.Remove("action");

            // ID
            if (base.ID != null) writer.WriteAttribute("id", base.ClientID);
        }
    }
}
