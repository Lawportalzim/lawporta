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
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Public.Common.UserControls
{
    public partial class Login : BaseUserControl
    {
        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (CurrentUser.Id > 0) LoginForm.Visible = false;
            }
        }

        // Login
        public void LoginUser(Object s, EventArgs e)
        {
          /*  if (KayUser.Login(EmailAddress.Text, Password.Text))
            {
                Response.Redirect(ReturnToUrl("/account/dashboard"));
            }
            else
            {
                Password.Text = "";
                ErrorMessage = "Invalid login details.";
            }*/
        }
    }
}