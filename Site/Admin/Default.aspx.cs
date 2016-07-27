using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin
{
    public partial class Default : BasePage
    {
        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("/admin/dashboard/");
        }
    }
}
