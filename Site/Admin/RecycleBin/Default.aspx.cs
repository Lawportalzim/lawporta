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

namespace Kay.Site.Admin.RecycleBin
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
            // cases List
            Pager pager = new Pager();
            DataView caseData = KayCase.RecycledList();
            CaseList.DataSource = caseData;
            CaseList.DataBind();

            //categories
            DataView categoryData = KayCategory.RecycledList();
            CategoryList.DataSource = categoryData;
            CategoryList.DataBind();
        }
       
        // Bind list item
        public void BindCaseListItem(Object s, DataGridItemEventArgs e)
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

                ((HyperLink)e.Item.FindControl("Restore")).NavigateUrl = String.Format("default.aspx?id={0}&action=restore&type=case", item.Id);
                ((HyperLink)e.Item.FindControl("Delete")).NavigateUrl = String.Format("default.aspx?id={0}&action=delete&type=case", item.Id);
                ((HyperLink)e.Item.FindControl("Delete")).ToolTip = String.Format("Delete {0}", string.Format("{0} vs {1}", Utilities.HtmlSafe(item.Plaintiff), Utilities.HtmlSafe(item.Defendant)));

                // Sorting
                e.Item.Attributes.Add("xid", item.Id.ToString());
            }
        }

        //Categories
        public void BindCategoryListItem(Object s, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Get item
                KayCategory item = new KayCategory(((DataRowView)e.Item.DataItem).Row);

                // Set row HTML
                ((Literal)e.Item.FindControl("Title")).Text = Utilities.HtmlSafe(item.Title);

                // Links                
                ((HyperLink)e.Item.FindControl("Restore")).NavigateUrl = String.Format("default.aspx?id={0}&action=restore&type=category", item.Id);
                ((HyperLink)e.Item.FindControl("Delete")).NavigateUrl = String.Format("default.aspx?id={0}&action=delete&type=category", item.Id);
                ((HyperLink)e.Item.FindControl("Delete")).ToolTip = String.Format("Delete {0}", Utilities.HtmlSafe(item.Title));

                // Sorting
                e.Item.Attributes.Add("xid", item.Id.ToString());
            }
        }

        // List item command
        private void ListItemCommand()
        {
            //cases
            if(Request.QueryString["type"] != null && Request.QueryString["type"] == "case")
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
                            if ((new KayCaseCategoryDescriptionCollection(KayCaseCategoryDescription.ListByCase(item.Id)).Count > 0))
                            {
                                ErrorMessage = String.Format("Case for {0} vs {1} is in use and can not be deleted", item.Plaintiff, item.Defendant);

                                Response.Clear();
                                Response.Write(result);
                                Response.Redirect("/admin/recyclebin/");
                                Response.End();
                                break;
                            }
                            if (item.Id > 0 && !item.Delete())
                            {
                                result = String.Format("Could not delete <strong>{0}</strong>.", string.Format("{0} vs {1}", Utilities.HtmlSafe(item.Plaintiff), Utilities.HtmlSafe(item.Defendant)));
                            }

                            Response.Clear();
                            Response.Write(result);
                            Response.Redirect("/admin/recyclebin/");
                            Response.End();
                            break;
                        }

                    case "restore":
                        {
                            String result = "";
                            item.Recycled = false;
                            
                            if (item.Id > 0 && !item.Save())
                            {
                                result = String.Format("Could not restore <strong>{0}</strong>.", string.Format("{0} vs {1}", Utilities.HtmlSafe(item.Plaintiff), Utilities.HtmlSafe(item.Defendant)));
                            }

                            Response.Clear();
                            Response.Write(result);
                            Response.Redirect("/admin/recyclebin/");
                            Response.End();
                            break;
                        }
                }  
            }

            //categories
            if (Request.QueryString["type"] != null && Request.QueryString["type"] == "category")
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
                            if ((new KayCaseCategoryDescriptionCollection(KayCaseCategoryDescription.ListByCategory(item.Id)).Count > 0) || (new KayCategory(item.Id).HasChildren()))
                            {
                                ErrorMessage = String.Format("{0} is in use and can not be deleted", item.Title);
                                Response.Clear();
                                Response.Write(result);
                                Response.Redirect(String.Format("/admin/recyclebin/"));
                                Response.End();
                                break;
                            }
                            if ((item.Id > 0 && !item.Delete()))
                            {
                                result = String.Format("Could not delete <strong>{0}</strong>.", Utilities.HtmlSafe(item.Title));
                            }
                            Response.Clear();
                            Response.Write(result);
                            Response.Redirect(String.Format("/admin/recyclebin/"));
                            Response.End();
                            break;
                        }
                    case "restore":
                        {
                            String result = "";
                            item.Recycled = false;
                            if ((item.Id > 0 && !item.Save()))
                            {
                                result = String.Format("Could not restore <strong>{0}</strong>.", Utilities.HtmlSafe(item.Title));
                            }
                            Response.Clear();
                            Response.Write(result);
                            Response.Redirect(String.Format("/admin/recyclebin/"));
                            Response.End();
                            break;
                        }
                }
            }
        }
    }
}
