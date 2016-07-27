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

namespace Kay.Site.Admin.Descriptions
{
    public partial class Edit : BasePage
    {
        // Properties
        private KayCaseCategoryDescription CaseCategoryDescription;
        private KayCase Case;

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
            // Get selected client record
            try { Case = new KayCase(int.Parse(Request.QueryString["caseid"])); }
            catch { Case = new KayCase(); }

            // Get selected dependent record
            try { CaseCategoryDescription = new KayCaseCategoryDescription(int.Parse(Request.QueryString["id"])); }
            catch { CaseCategoryDescription = new KayCaseCategoryDescription(); }
            
            // Setup page
            Master.Body.Attributes.Add("class", "edit");
            PageTitle.Text = (CaseCategoryDescription.Id == 0) ? String.Format("Add a category for {0} vs {1}", Utilities.HtmlSafe(Case.Plaintiff), Utilities.HtmlSafe(Case.Defendant)) : String.Format("Edit {0}", Utilities.HtmlSafe(CaseCategoryDescription.Category.Title, 12));
            Back.NavigateUrl = ReturnToUrl(String.Format("default.aspx?caseid={0}", Case.Id));

            // Fill form fields
            if (!Page.IsPostBack)
            {
                // Categories
                if (Case.CaseType == KayCaseType.Civil){Category.DataSource = KayCategory.List(KayCategoryType.Civil, 0);}
                else { Category.DataSource = KayCategory.List(KayCategoryType.Criminal, 0); }

                Category.DataTextField = "Title";
                Category.DataValueField = "Id";
                Category.DataBind();
                Category.Items.Insert(0, new ListItem("Select...", ""));

                //SubCatPanel.Visible = false;
                // Details
                if (CaseCategoryDescription.Category.ParentId == 0)
                {
                    Category.SelectedValue = CaseCategoryDescription.Category.Id.ToString();
                    if (CaseCategoryDescription.Category.HasChildren())
                    {
                        SubCategory.DataSource = KayCategory.List(CaseCategoryDescription.Category.Id);

                        SubCatPanel.Visible = true;
                        SubCategory.DataTextField = "Title";
                        SubCategory.DataValueField = "Id";
                        SubCategory.DataBind();
                        SubCategory.Items.Insert(0, new ListItem("Select...", "-1"));
                    }
                    else { SubCatPanel.Visible = false; }
                }
                //Sub category selected
                else
                {
                    Category.SelectedValue = CaseCategoryDescription.Category.ParentId.ToString();
                    SubCategory.DataSource = KayCategory.List(CaseCategoryDescription.Category.ParentId);

                    SubCatPanel.Visible = true;
                    SubCategory.DataTextField = "Title";
                    SubCategory.DataValueField = "Id";
                    SubCategory.DataBind();
                    SubCategory.Items.Insert(0, new ListItem("Select...", "-1"));
                    SubCategory.SelectedValue = CaseCategoryDescription.Category.Id.ToString();
                }

                Description.Text = CaseCategoryDescription.Description;

                // Save & exit
                SaveAndExit.Checked = Request.QueryString["exit"] != "false";
            }
        }

        public void SetParentCategory(Object s, EventArgs e)
        {            
            KayCategory _cat = new KayCategory(int.Parse(Category.SelectedValue));

            if (_cat.HasChildren())
            {
                SubCategory.DataSource = KayCategory.List(_cat.Id);

                SubCatPanel.Visible = true;
                SubCategory.DataTextField = "Title";
                SubCategory.DataValueField = "Id";
                SubCategory.DataBind();
                SubCategory.Items.Insert(0, new ListItem("Select...", "-1"));
            }
            else { SubCatPanel.Visible = false; }
        }

        // Save record
        public void Save(Object s, EventArgs e)
        {
            // Server-side validation
            if (!Page.IsValid) return;

            string te = SubCategory.SelectedValue;

            if (SubCatPanel.Visible && int.Parse(SubCategory.SelectedValue) > 0)
            {
                int.TryParse(SubCategory.SelectedValue, out CaseCategoryDescription.Category.Id);
            }
            else
            {
                int.TryParse(Category.SelectedValue, out CaseCategoryDescription.Category.Id);
            }

            CaseCategoryDescription.Case = Case;            
            CaseCategoryDescription.Description = Description.Text;
            
           
            if (CaseCategoryDescription.Save())
            {
                               // redirect
                if (SaveAndExit.Checked)
                {
                    Response.Redirect(ReturnToUrl(String.Format("default.aspx?caseid={0}", Case.Id)));
                }
                else
                {
                    Response.Redirect(Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, String.Format("id={0}&exit=false", CaseCategoryDescription.Id)));
                }
            }
            else
            {
                // error
                ErrorMessage = "Could not save dependant.";
            }
        }
    }
}
