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
using System.Globalization;
using System.Xml;
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin.Users
{
    public partial class Edit : BasePage
    {
        // Properties
        private KayUser user;
        private KayCompany _company;
        IFormatProvider format = new CultureInfo("en-GB");


        private KayCompany Company
        {
            get
            {
                try
                {
                    _company = new KayCompany(int.Parse(Request.QueryString["cp"]));
                    return _company;
                }
                catch(Exception ex)
                {
                    _company = new KayCompany();
                    return _company;
                }
            }
        }

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get selected record
            try { user = new KayUser(int.Parse(Request.QueryString["id"])); }
            catch { user = new KayUser(); }
            
            // Setup page
            Master.Body.Attributes.Add("class", "users edit");
            PageTitle.Text = user.Id == 0 ? "Add a user" : String.Format("Edit {0}", Utilities.HtmlSafe(user.FullName, 12));
            Back.NavigateUrl = Company.Id > 0 ? ReturnToUrl(string.Format("/admin/users/default.aspx?cp={0}", Company.Id)) : ReturnToUrl(string.Format("/admin/companies/", Company.Id));

            // Fill form fields
            if (!Page.IsPostBack)
            {
                // Details
                FirstName.Text = user.FirstName;
                LastName.Text = user.LastName;
                EmailAddress.Text = user.EmailAddress;                
                PasswordValidator.Enabled = (user.Id == 0);
                Admin.Checked = user.Administrator;
                SuperUser.Checked = user.SuperUser;

                FirstName.Text = user.FirstName;
                LastName.Text = user.LastName;
                EmailAddress.Text = user.EmailAddress;
                PasswordValidator.Enabled = user.Id == 0;
                Admin.Checked = user.Administrator;
                SuperUser.Checked = user.SuperUser;
                Active.Checked = user.Active;
                Telephone.Text = user.Telephone;
                AddressLine1.Text = user.Address.Line1;
                AddressLine2.Text = user.Address.Line2;
                Suburb.Text = user.Address.Suburb;
                City.Text = user.Address.City;
                Country.Text = user.Address.Country;
                ExpiryDates.Text = user.ExpiryDate.ToString("dd/MM/yyyy", format);
                StartDate.Text = user.StartDate.ToString("dd/MM/yyyy", format);
                SaveAndExit.Checked = Request.QueryString["exit"] != "false";
            }
        }

        // Save record
        public void Save(Object s, EventArgs e)
        {
            // Server-side validation
            if (!Page.IsValid) return;

            // Get group value
            int groups = (int)KayUserGroups.Members;
            if (Admin.Checked) groups += (int)KayUserGroups.Administrators;
            if (SuperUser.Checked) groups += (int)KayUserGroups.SuperUsers;

            // Set details
            user.FirstName = FirstName.Text;
            user.LastName = LastName.Text;
            user.EmailAddress = EmailAddress.Text;
            if (!String.IsNullOrEmpty(Password.Text)) user.Password = Password.Text;
            user.Groups = groups;
            // .extra.
            user.Company = Company;
            user.Telephone = Telephone.Text;
            user.Address.Line1 = AddressLine1.Text;
            user.Address.Line2 = AddressLine2.Text;
            user.Address.Suburb = Suburb.Text;
            user.Address.City = City.Text;
            user.Address.Country = Country.Text;
            user.ExpiryDate = DateTime.Parse(ExpiryDates.Text,format);
            user.StartDate = DateTime.Parse(StartDate.Text, format);
            user.Active = Active.Checked;

            // Check e-mail address
            if (!user.UniqueEmailAddress())
            {
                ErrorMessage = String.Format("{0} is already in use. Please select a different e-mail address.", EmailAddress.Text);
                return;
            }
            
            // Save user
            if (user.Save())
            {
                if (this.SaveAndExit.Checked)
                {
                    if (Company.Id > 0)
                        Response.Redirect(ReturnToUrl(string.Format("/admin/users/default.aspx?cp={0}", Company.Id)));
                    else
                        Response.Redirect(ReturnToUrl(string.Format("/admin/companies")));
                }
                else
                    this.Response.Redirect(Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, string.Format("id={0}&exit=false", user.Id)));
            }
            else
            {
                ErrorMessage = "Could not save user.";
            }
        }

        // Validate password
        public void ValidatePassword(object source, ServerValidateEventArgs args)
        {
            if (Admin.Checked && Password.Text == String.Empty)
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
