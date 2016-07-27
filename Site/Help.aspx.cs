using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Kay.BLL;
using Kay.Global;

namespace Site
{
    public partial class Help : BasePage
    {
        private KayHelp _Help;

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
            if (!Page.IsPostBack)
            {
                Master.BodyClass = "public search";
                Master.MasterPageTitle = "Help";

                // Get selected dependent record
                try { _Help = new KayHelp(1); }
                catch { _Help = new KayHelp(); }

                Content.Text = _Help.Content;
               
            }
        }

       
    }
}
