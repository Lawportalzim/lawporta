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

namespace Kay.Site.Admin.Users
{
    public partial class Default : BasePage
    {
        private KayCompany _company;

        private KayCompany Company
        {
            get
            {
                try
                {
                    _company = new KayCompany(int.Parse(this.Request.QueryString["cp"]));
                    return _company;
                }
                catch
                {
                    _company = new KayCompany();
                    return _company;
                }
            }
        }
        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            EditLink.NavigateUrl = string.Format("edit.aspx?cp={0}", Company.Id);
            if (this.Request.QueryString["action"] != null)
            {
                this.ListItemCommand();
            }
            else
            {
                if (this.Page.IsPostBack)
                    return;
                this.BindList();
            }
        }

        // Setup list
        private void BindList()
        {
            // List
            Pager pager = new Pager();
            DataView data = KayUser.List();
            if (Company.Id > 0)
            {
                data = KayUser.List(this.Company);
                this.CompanyTitle.Text = string.Format("for {0}", Company.Name);
            }
            data.RowFilter = GetFilter();
            data.Sort = HighlightSort(List, "Users_FirstName");
            pager.RecordCount = data.Count;
            List.PageSize = 5000000;
            List.CurrentPageIndex = pager.CurrentPageIndex;
            List.DataSource = data;
            List.DataBind();

            // Empty
            List.Visible = pager.Visible = data.Count > 0;
            Empty.Visible = data.Count == 0;
                   
            // Info
            if (data.Count == 1) Info.Text = "There is 1 user.";
            else Info.Text = String.Format("There are {0} users{1}.", data.Count, pager.PageCount > 1 ? String.Format(" on {0} pages", pager.PageCount) : "");
            EmptyMessage.Text = String.IsNullOrEmpty(data.RowFilter) ? "There are no users." : "Your search did not return any results. <a href=\"default.aspx\">Clear search</a>";
                
        }
        private String GetFilter()
        {
            // Build filter
            String filter = "";
            if (!String.IsNullOrEmpty(Request.QueryString["q"]))
            {
                filter += Utilities.GetSearchFilter("Users_FirstName,Users_LastName,Users_EmailAddress", Request.QueryString["q"]) + " AND ";
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
                KayUser item = new KayUser(((DataRowView)e.Item.DataItem).Row);

                // Set row HTML
                ((Literal)e.Item.FindControl("Status")).Text = item.Online ? "<span class=\"online\"></span>" : "<span class=\"offline\"></span>";
                ((Literal)e.Item.FindControl("Name")).Text = Utilities.HtmlSafe(item.FullName);
                ((Literal)e.Item.FindControl("Company")).Text = Utilities.HtmlSafe(item.Company.Name);
                ((Literal)e.Item.FindControl("EmailAddress")).Text = Utilities.HtmlSafe(item.EmailAddress);

                // Links
                ((HyperLink)e.Item.FindControl("Edit")).NavigateUrl = String.Format("edit.aspx?id={0}&cp={1}&{2}", item.Id, Company.Id, ReturnUrl);
                ((HyperLink)e.Item.FindControl("Delete")).NavigateUrl = String.Format("default.aspx?id={0}&action=delete", item.Id);
                ((HyperLink)e.Item.FindControl("Delete")).ToolTip = String.Format("Delete {0}", Utilities.HtmlSafe(item.FullName));

                // Sorting
                e.Item.Attributes.Add("xid", item.Id.ToString());
            }
        }

        // List item command
        private void ListItemCommand()
        {
            // Get item
            KayUser item;
            try { item = new KayUser(int.Parse(Request.QueryString["id"])); }
            catch { item = new KayUser(); }

            // Select command
            switch (Request.QueryString["action"])
            {
                case "delete":
                    {
                        String result = "";
                        if (item.Id > 0 && !item.Delete())
                        {
                            result = String.Format("Could not delete <strong>{0}</strong>.", Utilities.HtmlSafe(item.FullName));
                        }
                        Response.Clear();
                        Response.Write(result);
                        Response.Redirect("/admin/users/");
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

        // Export users
        public void ExportUsers(Object s, EventArgs e)
        {
            // Titles
            StringBuilder csv = new StringBuilder();
            csv.AppendLine(
                Utilities.CsvSafe("First name") + "," +
                Utilities.CsvSafe("Last name") + "," +
                Utilities.CsvSafe("E-mail address")
            );

            // Rows
            KayUserCollection users = new KayUserCollection(KayUser.List());
            foreach (KayUser user in users)
            {
                csv.AppendLine(
                    Utilities.CsvSafe(user.FirstName) + "," +
                    Utilities.CsvSafe(user.LastName) + "," +
                    Utilities.CsvSafe(user.EmailAddress)
                );
            }

            // Download
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] csvfile = enc.GetBytes(csv.ToString());
            Utilities.DownloadBytes(csvfile, "users.csv");
        }
    }
}
