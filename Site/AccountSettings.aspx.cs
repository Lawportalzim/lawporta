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

namespace Site
{
    public partial class AccountSettings : BasePage
    {
        // Properties
        private KayUser user;

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get selected record
            try { user = CurrentUser; }
            catch { user = new KayUser(); }


            this.Master.MasterPageTitle = "Change password";

            // Fill form fields
            if (!Page.IsPostBack)
            {
                FirstName.Text = user.FirstName;
                LastName.Text = user.LastName;
                Company.Text = user.Company.Name;
                PasswordValidator.Enabled = user.Id == 0;
            }
        }

        // Save record
        public void Save(Object s, EventArgs e)
        {
            Feedback.Text = "";

            // Server-side validation
            if (!Page.IsValid) return;

                     
            if (!String.IsNullOrEmpty(Password.Text)) user.Password = Password.Text;            
                                 
            // Save user
            if (user.Save())
            {
                Feedback.Text = "Password Successfully changed.";                               
            }
            else
            {
                Feedback.Text = "Could not save your changes.";
            }
        }

        // Validate password
        public void ValidatePassword(object source, ServerValidateEventArgs args)
        {
            if(Password.Text != PasswordConfirm.Text)
            {
                args.IsValid = false;               
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}
