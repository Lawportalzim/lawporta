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
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin.Categories
{
    public partial class Default : BasePage
    {
        // Properties
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
            // setup page
            Master.Body.Attributes.Add("class", "list");
            PageTitle.Text = String.Format("Categories / {0}", SelectedType.ToString());
            AddButton.NavigateUrl = String.Format("/admin/categories/edit.aspx?type={0}&pid={2}&{1}", (int)SelectedType, ReturnUrl, ParentCategory.Id);
            CategoryType.Visible = ParentCategory.Id == 0;
              
            if (Request.QueryString["action"] != null) ListItemCommand();
            else if (!Page.IsPostBack)
            {
                // filter
                CategoryType.DataSource = Enum.GetNames(typeof(KayCategoryType));
                CategoryType.DataBind();
                CategoryType.SelectedValue = SelectedType.ToString();

                // list
                BindList();
            }
        }
        public void SetCategoryType(Object s, EventArgs e)
        {
            Response.Redirect(Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, String.Format("type={0}", CategoryType.SelectedValue)));
        }

        // Setup list
        private void BindList()
        {
            CategoryTitle.Text = ParentCategory.Id > 0 ? String.Format("{0} - Subcategories",ParentCategory.Title) : "Categories";
            // List
            DataView data = KayCategory.List(SelectedType, ParentCategory.Id);                    
            List.DataSource = data;
            List.DataBind();

            // Empty           
            Empty.Visible = data.Count == 0;
                   
            // Info
            if (data.Count == 1) Info.Text = "There is 1 category.";
            else Info.Text = String.Format("There are {0} categories.", data.Count);
            EmptyMessage.Text = String.IsNullOrEmpty(data.RowFilter) ? "There are no categories." : "Your search did not return any results. <a href=\"default.aspx\">Clear search</a>";

            
            Back.NavigateUrl = String.Format("/admin/categories/default.aspx?pid={0}", ParentCategory.ParentId);
                
        }

        // Bind list item
        public void BindListItem(Object s, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Get item
                KayCategory item = new KayCategory(((DataRowView)e.Item.DataItem).Row);

                // Set row HTML
                ((Literal)e.Item.FindControl("Title")).Text = Utilities.HtmlSafe(item.Title);

                // Links
                ((HyperLink)e.Item.FindControl("Edit")).NavigateUrl = String.Format("edit.aspx?id={0}&pid={3}&type={2}&{1}", item.Id, ReturnUrl, (int)item.Type, ParentCategory.Id);
                ((HyperLink)e.Item.FindControl("View")).NavigateUrl = String.Format("default.aspx?pid={0}&type={2}&{1}", item.Id, ReturnUrl, (int)item.Type);
                ((HyperLink)e.Item.FindControl("Delete")).NavigateUrl = String.Format("default.aspx?id={0}&action=delete&pid={1}", item.Id, ParentCategory.Id);
                ((HyperLink)e.Item.FindControl("Delete")).ToolTip = String.Format("Delete {0}", Utilities.HtmlSafe(item.Title));
              
            }
        }

        // List item command
        private void ListItemCommand()
        {
            // Get item
            KayCategory item;
            try { item = new KayCategory(int.Parse(Request.QueryString["id"])); }
            catch { item = new KayCategory(); }

            // Select command
            switch (Request.QueryString["action"])
            {
                case "delete":
                    {
                        String result = "";
                        item.Recycled = true;
                        if ((new KayCaseCategoryDescriptionCollection(KayCaseCategoryDescription.ListByCategory(item.Id)).Count > 0) || (new KayCategory(item.Id).HasChildren()))
                        {
                            ErrorMessage = String.Format("{0} is in use and can not be deleted", item.Title);
                            Response.Clear();
                            Response.Write(result);
                            Response.Redirect(String.Format("/admin/categories/default.aspx?pid={0}", ParentCategory.Id));
                            Response.End();
                            break;
                        }
                        if ((item.Id > 0 && !item.Save()))
                        {
                            result = String.Format("Could not delete <strong>{0}</strong>.", Utilities.HtmlSafe(item.Title));
                        }
                        Response.Clear();
                        Response.Write(result);
                        Response.Redirect(String.Format("/admin/categories/default.aspx?pid={0}", ParentCategory.Id));
                        Response.End();
                        break;
                    }
            }
        }
    }
}
