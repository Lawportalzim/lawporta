using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Kay.BLL;
using Kay.Global;

namespace Kay.Site.Public.Common.UserControls
{
    public partial class CategoryFilter : BaseUserControl
    {
        // Properties
        private KayCategoryType _categoryType;
        public KayCategoryType CategoryType
        {
            set { _categoryType = value; }
            get { return _categoryType; }
        }
        private String _urlTemplate;
        public String UrlTemplate
        {
            set { _urlTemplate = value; }
            get { return _urlTemplate; }
        }

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Enum.IsDefined(typeof(KayCategoryType), CategoryType))
            {
                throw new Exception("You must define the category type for the category filter.");
            }
            if (String.IsNullOrEmpty(UrlTemplate))
            {
                throw new Exception("You must define the url template for the category filter.");
            }

            SetupFilter();
        }

        // Build
        private void SetupFilter()
        {
            Categories.DataSource = new KayCategoryCollection( KayCategory.LiveList(CategoryType, 0) );
            Categories.DataBind();
            if (Categories.Items.Count == 0) this.Visible = false;
        }
        public void BindCategory(Object s, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KayCategory category = (KayCategory)e.Item.DataItem;
                HyperLink link = (HyperLink)e.Item.FindControl("Link");
                link.Text = Utilities.HtmlSafe(category.Title);
                link.NavigateUrl = String.Format("/cases" + category.UrlPath);
                if (link.NavigateUrl == Config.FriendlyPath)
                {
                    link.CssClass += " selected ";
                }

                if (category.HasChildren())
                {
                    Repeater SubCategories = (Repeater)e.Item.FindControl("SubCategories");
                    SubCategories.DataSource = new KayCategoryCollection(KayCategory.LiveList(CategoryType, category.Id));
                    SubCategories.DataBind();
                }
            }
        }

        public void BindSubCategory(Object s, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KayCategory category = (KayCategory)e.Item.DataItem;
                HyperLink link = (HyperLink)e.Item.FindControl("Link");
                link.Text = Utilities.HtmlSafe(category.Title);
                link.NavigateUrl = String.Format("/cases" + category.UrlPath);
                if (link.NavigateUrl == Config.FriendlyPath)
                {
                    link.CssClass += " selected ";
                }
            }
        }
    }
}