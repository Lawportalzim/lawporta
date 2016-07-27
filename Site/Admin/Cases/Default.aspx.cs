using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin.Cases
{
    public partial class Default : BasePage
    {
        private bool isSearch = false;
        public KayCaseType SelectedType
        {
            get
            {
                KayCaseType kayCaseType = KayCaseType.Civil;
                try
                {
                    kayCaseType = (KayCaseType)Enum.Parse(typeof(KayCaseType), this.Request.QueryString["type"].ToString(), true);
                }
                catch
                {
                }
                return kayCaseType;
            }
        }

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Body.Attributes.Add("class", "list");
            PageTitle.Text = string.Format("Cases under {0} law", SelectedType.ToString());
            AddButton.NavigateUrl = string.Format("edit.aspx?type={0}&{1}", SelectedType, ReturnUrl);
            if (Request.QueryString["action"] != null) ListItemCommand();
            
            else if (!Page.IsPostBack)
            {
                CategoryType.DataSource = Enum.GetNames(typeof(KayCaseType));
                CategoryType.DataBind();
                CategoryType.SelectedValue = SelectedType.ToString();
                BindList();
            }
        }

        public void SetCategoryType(object s, EventArgs e)
        {
            Response.Redirect(Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, string.Format("type={0}", CategoryType.SelectedValue)));
        }


        public void SearchCase(object s, EventArgs e)
        {
            isSearch = true;
            BindList();
        }

        // Setup list
        private void BindList()
        {
            // List
            ListPager.Visible = true;
            DataView data;
            if (isSearch)
            {
                data = KayCase.Find(SearchText.Text, SelectedType);
            }
            else
            {
                data = KayCase.List((int)SelectedType);
            }
            
            data.RowFilter = GetFilter();
            data.Sort = HighlightSort(List, "Cases_Plaintiff Asc");

            List.PageSize = 50;
            ListPager.Data = data;
            List.DataSource = ListPager.PagedData;
            List.DataBind();
            List.Visible = (ListPager.RecordCount > 0);
            ListPager.Visible = (ListPager.PageCount > 1);

            // Info
            if (data.Count == 1) Info.Text = "There is 1 Case.";
            else Info.Text = String.Format("There are {0} Cases{1}.", data.Count, ListPager.PageCount > 1 ? String.Format(" on {0} pages", ListPager.PageCount) : "");
            //EmptyMessage.Text = String.IsNullOrEmpty(data.RowFilter) ? "There are no Cases." : "Your search did not return any results. <a href=\"default.aspx\">Clear search</a>";                
        }
        private String GetFilter()
        {
            // Build filter
            String filter = "";
            if (!String.IsNullOrEmpty(Request.QueryString["q"]))
            {
                filter += Utilities.GetSearchFilter("Cases_Plaintiff,Cases_Defendant", Request.QueryString["q"]) + " AND ";
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
                KayCase item = new KayCase(((DataRowView)e.Item.DataItem).Row);

                Literal Appeal = ((Literal)e.Item.FindControl("Appeal"));
                if(item.Appealed)
                {
                    Appeal.Text = " <span class='label label-important'>Appealed</span>";
                }
                if(item.ParentId > 0)
                {
                    Appeal.Text = " <span class='label label-success'>Appeal</span>";
                }

                // Set row HTML
                ((Literal)e.Item.FindControl("Number")).Text = item.CaseNumber.ToString();
                ((Literal)e.Item.FindControl("Parties")).Text = string.Format("{0} vs {1}", Utilities.HtmlSafe(item.Plaintiff), Utilities.HtmlSafe(item.Defendant));                                
                ((Literal)e.Item.FindControl("CaseType")).Text = item.CaseType.ToString();
                ((Literal)e.Item.FindControl("CourtName")).Text = item.CourtName;
                ((Literal)e.Item.FindControl("CategoriesCount")).Text = item.CaseCategoryDescriptions.Count.ToString();

                // Links                
                ((HyperLink)e.Item.FindControl("Edit")).NavigateUrl = String.Format("edit.aspx?id={0}&type={2}&{1}", item.Id, ReturnUrl,item.CaseType.ToString());
                ((HyperLink)e.Item.FindControl("Description")).NavigateUrl = String.Format("/admin/descriptions/default.aspx?caseid={0}&{1}", item.Id, ReturnUrl);
                ((HyperLink)e.Item.FindControl("Appealed")).NavigateUrl = String.Format("edit.aspx?id={0}&appeal=true&&type={2}&{1}", item.Id, ReturnUrl, item.CaseType.ToString());
                ((HyperLink)e.Item.FindControl("Delete")).NavigateUrl = String.Format("default.aspx?id={0}&action=delete", item.Id);
                ((HyperLink)e.Item.FindControl("Delete")).ToolTip = String.Format("Delete {0}", string.Format("{0} vs {1}", Utilities.HtmlSafe(item.Plaintiff), Utilities.HtmlSafe(item.Defendant)));

                // Sorting
                e.Item.Attributes.Add("xid", item.Id.ToString());
            }
        }

        // List item command
        private void ListItemCommand()
        {
            // Get item
            KayCase item;
            try { item = new KayCase(int.Parse(Request.QueryString["id"])); }
            catch { item = new KayCase(); }

            // Select command
            switch (Request.QueryString["action"])
            {
                
                case "delete":
                    {
                        String result = "";
                        item.Recycled = true;
                        if ((new KayCaseCategoryDescriptionCollection(KayCaseCategoryDescription.ListByCase(item.Id)).Count > 0))
                        {
                            ErrorMessage = String.Format("Case for {0} vs {1} is in use and can not be deleted", item.Plaintiff, item.Defendant);

                            Response.Clear();
                            Response.Write(result);
                            Response.Redirect("/admin/cases/");
                            Response.End();
                            break;
                        }
                        if (item.Id > 0 && !item.Save())
                        {
                            result = String.Format("Could not delete <strong>{0}</strong>.", string.Format("{0} vs {1}", Utilities.HtmlSafe(item.Plaintiff), Utilities.HtmlSafe(item.Defendant)));
                        }
                        
                        Response.Clear();
                        Response.Write(result);
                        Response.Redirect("/admin/cases/");
                        Response.End();
                        break;
                    }
                
            }
        }
    }
}
