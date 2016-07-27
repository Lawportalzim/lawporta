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

namespace Site
{
    public partial class Login : BasePage
    {
        // Properties
        private String Feedback = "<p>{0}</p>";

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Body.Attributes.Add("class", "login");
            Master.PageTitle.Text = "User login";
            // setup default user
            if (!Config.ApplicationLive && Request.QueryString["action"] == "setup") SetupKayDefaultAccount();
        }

        // Login
        public void LoginUser(Object s, EventArgs e)
        {
            if (KayUser.Login(this.EmailAddress.Text, this.Password.Text) == 1)
                this.Response.Redirect(this.ReturnToUrl("/"));
            else if (KayUser.Login(this.EmailAddress.Text, this.Password.Text) == 2)
            {
                this.Password.Text = "";
                this.LoginFeedback.Text = string.Format(this.Feedback, (object)"Someone is online with the same account");
            }
            else if (KayUser.Login(this.EmailAddress.Text, this.Password.Text) == 3)
            {
                this.Password.Text = "";
                this.LoginFeedback.Text = string.Format(this.Feedback, (object)"Your account expired");
            }
            else if (KayUser.Login(this.EmailAddress.Text, this.Password.Text) == 4)
            {
                this.Password.Text = "";
                this.LoginFeedback.Text = string.Format(this.Feedback, (object)"Your account is not active");
            }
            else
            {
                this.Password.Text = "";
                this.LoginFeedback.Text = string.Format(this.Feedback, (object)"Invalid login details");
            }     
        }        

        #region Site setup

        private void SetupKayDefaultAccount()
        {
            // check if the default user has already been setup
            KayUser admin = new KayUser("kuda@gmail.com");
            if (admin.Id > 0)
            {
                ErrorMessage = "Setup has already been run.";
                Response.Redirect("/");
                return;
            }

            // user
            KayUser user = new KayUser();
            user.FirstName = "Kuda";
            user.LastName = "Gore";
            user.EmailAddress = "kuda@gmail.com";
            user.Password = "kay";
            user.Groups = (int)KayUserGroups.SuperUsers + (int)KayUserGroups.Administrators + (int)KayUserGroups.Members;
            user.Telephone = "0774673610";
            user.Company.Id = 1;
            user.Address.Line1 = "6 Stoney Rd Greencroft";
            user.Address.Suburb = "Greencroft";
            user.Address.City = "Harare";
            user.Address.Country = "Zimbabwe";
            user.Save();

            // contact lists
            KayContactList list;
            list = new KayContactList();
            list.Title = "Clients";
            list.Save();        

            // login
            KayUser.Login(user.EmailAddress, user.Password);

            // done
            StatusMessage = "Setup complete!";
            Response.Redirect("/dashboard/");
        }

        #endregion
    }
}
