using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            KayUser.Logout();            
            Response.Redirect("/admin/dashboard/");
        }
    }
}