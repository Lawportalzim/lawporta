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
using Kay.Controls;

namespace Kay.Site.Admin.Companies
{
    public partial class Default : BasePage
    {
        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Body.Attributes.Add("class", "list");
            if (Request.QueryString["action"] != null) ListItemCommand();
            else if (!Page.IsPostBack) BindList();
        }

        // Setup list
        private void BindList()
        {
            // List
            Pager pager = new Pager();
            DataView data = KayCompany.List();
            data.RowFilter = GetFilter();
            data.Sort = HighlightSort(List, "Companies_Name");
            pager.RecordCount = data.Count;
            List.PageSize = 5000000;
            List.CurrentPageIndex = pager.CurrentPageIndex;
            List.DataSource = data;
            List.DataBind();

            // Empty
            List.Visible = pager.Visible = data.Count > 0;
            Empty.Visible = data.Count == 0;
                   
            // Info
            if (data.Count == 1) Info.Text = "There is 1 company.";
            else Info.Text = String.Format("There are {0} companies{1}.", data.Count, pager.PageCount > 1 ? String.Format(" on {0} pages", pager.PageCount) : "");
            EmptyMessage.Text = String.IsNullOrEmpty(data.RowFilter) ? "There are no companies." : "Your search did not return any results. <a href=\"default.aspx\">Clear search</a>";
                
        }
        private String GetFilter()
        {
            // Build filter
            String filter = "";
            if (!String.IsNullOrEmpty(Request.QueryString["q"]))
            {
                filter += Utilities.GetSearchFilter("Companies_FirstName,Companies_LastName,Companies_EmailAddress", Request.QueryString["q"]) + " AND ";
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
                KayCompany item = new KayCompany(((DataRowView)e.Item.DataItem).Row);

                // Set row HTML
                ((Literal)e.Item.FindControl("Name")).Text = Utilities.HtmlSafe(item.Name);
                ((Literal)e.Item.FindControl("NumberOfAccounts")).Text = Utilities.HtmlSafe(item.NumberOfAccounts.ToString());
                ((Literal)e.Item.FindControl("Telephone")).Text = Utilities.HtmlSafe(item.Telephone);
                ((Literal)e.Item.FindControl("EmailAddress")).Text = Utilities.HtmlSafe(item.EmailAddress);

                // Links
                ((HyperLink)e.Item.FindControl("Edit")).NavigateUrl = String.Format("edit.aspx?id={0}&{1}", item.Id, ReturnUrl);
                ((HyperLink)e.Item.FindControl("Users")).NavigateUrl = string.Format("/admin/users/default.aspx?cp={0}&{1}", item.Id, ReturnUrl);
                ((HyperLink)e.Item.FindControl("Delete")).NavigateUrl = String.Format("default.aspx?id={0}&action=delete", item.Id);
                ((HyperLink)e.Item.FindControl("Delete")).ToolTip = String.Format("Delete {0}", Utilities.HtmlSafe(item.Name));

                // Sorting
                e.Item.Attributes.Add("xid", item.Id.ToString());
            }
        }

        // List item command
        private void ListItemCommand()
        {
            // Get item
            KayCompany item;
            try { item = new KayCompany(int.Parse(Request.QueryString["id"])); }
            catch { item = new KayCompany(); }

            // Select command
            switch (Request.QueryString["action"])
            {
                case "delete":
                    {
                        String result = "";
                        if (item.Id > 0 && !item.Delete())
                        {
                            result = String.Format("Could not delete <strong>{0}</strong>.", Utilities.HtmlSafe(item.Name));
                        }
                        Response.Clear();
                        Response.Write(result);
                        Response.Redirect("/admin/companies/");
                        Response.End();
                        break;
                    }
                case "validate-email":
                    {
                        String result = "";
                        item.EmailAddress = Request.Form["email"];
                        if (!item.UniqueEmailAddress()) result = "Taken";
                        Response.Clear();
                        Response.Write(result);
                        Response.End();
                        break;
                    }
            }
        }

       
    }
}
