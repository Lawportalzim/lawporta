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

namespace Kay.Site.Admin.Companies
{
    public partial class Edit : BasePage
    {
        // Properties
        private KayCompany company;

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get selected record
            try { company = new KayCompany(int.Parse(Request.QueryString["id"])); }
            catch { company = new KayCompany(); }
            
            // Setup page
            Master.Body.Attributes.Add("class", "companies edit");
            PageTitle.Text = company.Id == 0 ? "Add a company" : String.Format("Edit {0}", Utilities.HtmlSafe(company.Name, 12));
            Back.NavigateUrl = ReturnToUrl("/companies/");

            // Fill form fields
            if (!Page.IsPostBack)
            {
                // Details
                Name.Text = company.Name;
                ContactPerson.Text = company.ContactPerson;
                EmailAddress.Text = company.EmailAddress;               

                // .extra.                
                Telephone.Text = company.Telephone;
                AddressLine1.Text = company.Address.Line1;
                AddressLine2.Text = company.Address.Line2;
                Suburb.Text = company.Address.Suburb;
                City.Text = company.Address.City;
                Country.Text = company.Address.Country;
                NumberOfAccounts.Text = company.NumberOfAccounts.ToString();

                // Save & exit
                SaveAndExit.Checked = Request.QueryString["exit"] != "false";
            }
        }

        // Save record
        public void Save(Object s, EventArgs e)
        {
            // Server-side validation
            if (!Page.IsValid) return;

            // Get group value
            

            // Set details
            company.Name = Name.Text;
            company.ContactPerson = ContactPerson.Text;
            company.EmailAddress = EmailAddress.Text;

            // .extra.
            company.Telephone = Telephone.Text;
            company.Address.Line1 = AddressLine1.Text;
            company.Address.Line2 = AddressLine2.Text;
            company.Address.Suburb = Suburb.Text;
            company.Address.City = City.Text;
            company.Address.Country = Country.Text;

            company.NumberOfAccounts = int.Parse(NumberOfAccounts.Text);

            // Check e-mail address
            if (!company.UniqueEmailAddress())
            {
                ErrorMessage = String.Format("{0} is already in use. Please select a different e-mail address.", EmailAddress.Text);
                return;
            }
            
            // Save company
            if (company.Save())
            {
                if (SaveAndExit.Checked)
                {
                    Response.Redirect(ReturnToUrl("/admin/companies/"));
                }
                else
                {
                    Response.Redirect(Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, String.Format("id={0}&exit=false", company.Id)));
                }
            }
            else
            {
                ErrorMessage = "Could not save company.";
            }
        }
        
    }
}
