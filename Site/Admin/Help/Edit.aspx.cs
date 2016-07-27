using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin.Help
{
    public partial class Edit : BasePage
    {
        // Properties
        private KayHelp Help;
        
        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            // Get selected dependent record
            try { Help = new KayHelp(int.Parse(Request.QueryString["id"])); }
            catch { Help = new KayHelp(); }
            
            // Setup page
            Master.Body.Attributes.Add("class", "edit");
            PageTitle.Text = "Help";
            // Fill form fields
            if (!Page.IsPostBack)
            {
                Description.Text = Help.Content;
            }
        }

        

        // Save record
        public void Save(Object s, EventArgs e)
        {
            // Server-side validation
            if (!Page.IsValid) return;


            Help.Content = Description.Text;


            if (Help.Save())
            {
                  // redirect                
                 Response.Redirect(ReturnToUrl(String.Format("edit.aspx?id={0}", Help.Id)));
                
            }
            else
            {
                // error
                ErrorMessage = "Could not save help content.";
            }
        }
    }
}
