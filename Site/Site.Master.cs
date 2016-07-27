using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kay.BLL;
using Kay.Global;

namespace Site
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private BasePage basepage
        {
            get
            {
                return (BasePage)this.Page;
            }
        }

        private KayUser CurrentUser
        {
            get
            {
                return basepage.CurrentUser;
            }
        }

        public string MasterPageTitle
        {
            get
            {
                PageTitle.Text = "Civil Cases";
                return PageTitle.Text;
            }
            set
            {
                PageTitle.Text = value;
            }
        }

        public string BodyClass
        {
            get
            {
                Body.Attributes["class"] = "public";
                return Body.Attributes["class"];
            }
            set
            {
                Body.Attributes["class"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Id > 0)
            {
                HeadLoginName.Text = CurrentUser.FirstName;
                HeadLoginStatus.Visible = true;
            }
            else
            {
                HeadLoginStatus.Visible = false;
            }
            if (Request.Url.ToString().Contains("/cases/civil"))
            {
                CivilLink.Attributes["class"] = "selected";
            }
            if (Request.Url.ToString().Contains("/cases/criminal"))
            {
                CriminalLink.Attributes["class"] = "selected";
            }
        }
    }
}
