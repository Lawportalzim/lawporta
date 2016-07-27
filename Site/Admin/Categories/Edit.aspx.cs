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

namespace Kay.Site.Admin.Categories
{
    public partial class Edit : BasePage
    {
        // Properties
        private KayCategory category;
        public KayCategoryType SelectedType
        {
            get
            {
                KayCategoryType _type = KayCategoryType.Civil;
                try { _type = (KayCategoryType)Enum.Parse(typeof(KayCategoryType), Request.QueryString["type"].ToString(), true); }
                catch { }
                return _type;
            }
        }

        public KayCategory ParentCategory
        {
            get
            {
                KayCategory cat = new KayCategory();
                try
                {
                    cat = new KayCategory(int.Parse(Request.QueryString["pid"].ToString()));
                    return cat;
                }
                catch { return cat; }
            }
        }

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get selected record
            try { category = new KayCategory(int.Parse(Request.QueryString["id"])); }
            catch { category = new KayCategory(); }
            
            // Setup page
            Master.Body.Attributes.Add("class", "edit");
            PageTitle.Text = category.Id == 0 ? "Add a category" : String.Format("Edit {0}", Utilities.HtmlSafe(category.Title, 12));
            Back.NavigateUrl = ReturnToUrl("/admin/categories/");

            // Fill form fields
            if (!Page.IsPostBack)
            {
                // Details
                ItemTitle.Text = category.Title;
                Live.Checked = category.Live;

                // Save & exit
                SaveAndExit.Checked = Request.QueryString["exit"] != "false";
            }
        }

        // Save record
        public void Save(Object s, EventArgs e)
        {
            // Server-side validation
            if (!Page.IsValid) return;

            // Set details
            category.ParentId = ParentCategory.Id;
            category.Type = SelectedType;
            category.Title = ItemTitle.Text;
            category.Live = Live.Checked;

            // Check e-mail address
            if (!category.Unique())
            {
                ErrorMessage = String.Format("{0} is already in use. Please select a different title.", ItemTitle.Text);
                return;
            }
            
            // Save category
            if (category.Save())
            {
                if (SaveAndExit.Checked)
                {
                    Response.Redirect(ReturnToUrl("/Admin/categories/"));
                }
                else
                {
                    Response.Redirect(Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, String.Format("id={0}&exit=false", category.Id)));
                }
            }
            else
            {
                ErrorMessage = "Could not save category.";
            }
        }
    }
}
