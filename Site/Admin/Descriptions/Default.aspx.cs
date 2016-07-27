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
using Kay.Controls;

namespace Kay.Site.Admin.Descriptions
{   

    public partial class Default : BasePage
    {
        private KayCase Case;

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get selected record
            try { Case = new KayCase(int.Parse(Request.QueryString["caseid"])); }
            catch { Case = new KayCase(); }

            Master.Body.Attributes.Add("class", "list");
            if (Request.QueryString["action"] != null) ListItemCommand();
            else if (!Page.IsPostBack) BindList();

            CaseName.Text = string.Format("{0} vs {1}", Utilities.HtmlSafe(Case.Plaintiff), Utilities.HtmlSafe(Case.Defendant));

            //add dependant
            AddCategory.NavigateUrl = String.Format("edit.aspx?caseid={0}", Case.Id);
            AddCategory.Text = "Add Category";
        }

        // Setup list
        private void BindList()
        {
            // List
            Pager pager = new Pager();
            DataView data = KayCaseCategoryDescription.ListByCase(Case.Id);
            data.RowFilter = GetFilter();
            data.Sort = HighlightSort(List, "CaseCategoryDescriptions_Id Asc");
            pager.RecordCount = data.Count;
            List.PageSize = 5000000;
            List.CurrentPageIndex = pager.CurrentPageIndex;
            List.DataSource = data;
            List.DataBind();

            // Empty
            List.Visible = pager.Visible = data.Count > 0;
            Empty.Visible = data.Count == 0;
                   
            // Info
            if (data.Count == 1) Info.Text = "There is 1 category.";
            else Info.Text = String.Format("There are {0} categories{1}.", data.Count, pager.PageCount > 1 ? String.Format(" on {0} pages", pager.PageCount) : "");
            EmptyMessage.Text = String.IsNullOrEmpty(data.RowFilter) ? "There are no categories." : "Your search did not return any results. <a href=\"default.aspx\">Clear search</a>";                
        }
        private String GetFilter()
        {
            // Build filter
            String filter = "";
            if (!String.IsNullOrEmpty(Request.QueryString["q"]))
            {
                filter += Utilities.GetSearchFilter("", Request.QueryString["q"]) + " AND ";
            }

            // Clean up
            filter = Regex.Replace(filter, @" AND $", "");
            return filter;
        }

        // Bind list item
        public void BindListItem(Object s, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Get item
                KayCaseCategoryDescription item = new KayCaseCategoryDescription(((DataRowView)e.Item.DataItem).Row);

                // Set row HTML
                ((Literal)e.Item.FindControl("Parties")).Text = string.Format("{0} vs {1}", Utilities.HtmlSafe(item.Case.Plaintiff), Utilities.HtmlSafe(item.Case.Defendant)); ;
                ((Literal)e.Item.FindControl("Category")).Text = item.Category.Title;
                ((Literal)e.Item.FindControl("Summary")).Text = Utilities.Truncate(item.Description, 20);       

                // Links                
                ((HyperLink)e.Item.FindControl("Edit")).NavigateUrl = String.Format("edit.aspx?id={0}&caseid={1}&{2}", item.Id, Case.Id, ReturnUrl);                
                ((HyperLink)e.Item.FindControl("Delete")).NavigateUrl = String.Format("default.aspx?id={0}&action=delete&caseid={1}", item.Id, Case.Id);
                ((HyperLink)e.Item.FindControl("Delete")).ToolTip = String.Format("Delete {0}", Utilities.HtmlSafe(item.Category.Title));

                // Sorting
                e.Item.Attributes.Add("xid", item.Id.ToString());
            }
        }

        // List item command
        private void ListItemCommand()
        {
            // Get item
            KayCaseCategoryDescription item;
            try { item = new KayCaseCategoryDescription(int.Parse(Request.QueryString["id"])); }
            catch { item = new KayCaseCategoryDescription(); }

            // Select command
            switch (Request.QueryString["action"])
            {
                
                case "delete":
                    {
                        String result = "";
                        if (item.Id > 0 && !item.Delete())
                        {
                            result = String.Format("Could not delete <strong>{0}</strong>.", Utilities.HtmlSafe(item.Category.Title));
                        }
                        
                        Response.Write(result);
                        Response.Redirect(String.Format("/admin/descriptions/default.aspx?caseid={0}", Case.Id));
                        Response.End();
                        break;
                    }
                
            }
        }
    }
}
