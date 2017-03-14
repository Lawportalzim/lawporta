using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Kay.BLL;
using Kay.Global;

namespace Site
{
    public partial class Search : BasePage
    {
        string SearchString = string.Empty;

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //get cookie value
            HttpCookie cookie = HttpContext.Current.Request.Cookies["type"];

            //main menu
            if(cookie != null)MainMenu.NavigateUrl = string.Format("/cases/{0}", cookie.Value);
            
            if (!Page.IsPostBack)
            {
                Master.BodyClass = "public search";
                Master.MasterPageTitle = "Search Results";
                String query = Request.QueryString["q"];
                SearchString = query;
                query = query.Replace("'", "''");

                if (!String.IsNullOrEmpty(query))
                {
                   
                    
                    // Find in modules
                    DataView CasesData = KayCase.Find(query);
                    DataView DescriptionsData = KayCaseCategoryDescription.Find(query);
                    DataView CategoriesData = KayCategory.Find(query);
                    
                    if (cookie.Value == "civil")
                    {
                        CasesData = KayCase.Find(query, KayCaseType.Civil);
                        DescriptionsData = KayCaseCategoryDescription.Find(query, KayCaseType.Civil);
                        CategoriesData = KayCategory.Find(query, KayCategoryType.Civil);
                    }
                    else if (cookie.Value == "criminal")
                    {
                        CasesData = KayCase.Find(query, KayCaseType.Criminal);
                        DescriptionsData = KayCaseCategoryDescription.Find(query, KayCaseType.Criminal);
                        CategoriesData = KayCategory.Find(query, KayCategoryType.Criminal);
                    }

                    // Summary                    
                    if (CasesData.Count == 0)
                    {                        
                        CasesPager.Visible = CasesList.Visible = false;
                    }
                    if (DescriptionsData.Count == 0)
                    {
                        DescriptionsPager.Visible = DescriptionsList.Visible = false;
                    }
                    if (CategoriesData.Count == 0)
                    {
                        CategoriesPager.Visible = CategoriesList.Visible = false;
                    }
                    if (CasesData.Count == 0 && DescriptionsData.Count == 0 && CategoriesData.Count == 0)
                    {
                        SearchSummary.Text = String.Format("Your search for <strong>{0}</strong> did not return any results.", Utilities.HtmlSafe(SearchString));
                    }
                    else
                    {
                        SearchSummary.Text = String.Format("Your search for <strong>{0}</strong> returned <strong>{1}</strong> result{2}.",
                            Utilities.HtmlSafe(query),
                            CasesData.Count + DescriptionsData.Count + CasesData.Count,
                            CasesData.Count + DescriptionsData.Count + CasesData.Count == 1 ? "" : "s");
                    }

                    // Bind paged data
                    CasesPager.Data = CasesData;
                    CasesList.DataSource = CasesPager.PagedData;
                    CasesList.DataBind();
                    CasesList.Visible = (CasesPager.RecordCount > 0);
                    CasesPager.Visible = (CasesPager.PageCount > 1);

                    //descriptions
                    DescriptionsPager.Data = DescriptionsData;
                    DescriptionsList.DataSource = DescriptionsPager.PagedData;
                    DescriptionsList.DataBind();
                    DescriptionsList.Visible = (DescriptionsPager.RecordCount > 0);
                    DescriptionsPager.Visible = (DescriptionsPager.PageCount > 1);

                    //Categories
                    CategoriesPager.Data = CategoriesData;
                    CategoriesList.DataSource = CategoriesPager.PagedData;
                    CategoriesList.DataBind();
                    CategoriesList.Visible = (CategoriesPager.RecordCount > 0);
                    CategoriesPager.Visible = (CategoriesPager.PageCount > 1);
                }
                else
                {
                    SearchSummary.Text = "You did not enter a search query.";
                    CasesPager.Visible = CasesList.Visible =
                    CategoriesPager.Visible = CategoriesList.Visible =
                    DescriptionsPager.Visible = DescriptionsList.Visible = false;
                }
            }
        }

        //Cases
        public void BindCasesResult(Object s, RepeaterItemEventArgs e)
        {
            // Row
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Header)
            {
                ((Literal)e.Item.FindControl("SearchType")).Text = "Cases that match your search query";
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KayCase _item = new KayCase(((DataRowView)e.Item.DataItem).Row);
                string trancatedNotes = Utilities.TruncateByKeyWords(Utilities.RemoveNums(_item.Notes), SearchString,1,1,true,"",false,"...",true,"");

                ((HyperLink)e.Item.FindControl("Title")).Text = (String.Format("<big>{0}</big> : {1} vs {2}", Utilities.HighlightKeyWords(_item.CaseNumber, SearchString, "pink", false), Utilities.HighlightKeyWords(_item.Plaintiff, SearchString, "pink", false), Utilities.HighlightKeyWords(_item.Defendant, SearchString, "yellow", false)));
                ((HyperLink)e.Item.FindControl("Title")).NavigateUrl = _item.UrlPath;
                //((Literal)e.Item.FindControl("Description")).Text = Utilities.Truncate(Utilities.RemoveNums(_item.Notes), 15);
                ((Literal)e.Item.FindControl("Description")).Text = Utilities.HighlightKeyWords(Utilities.RemoveNums(trancatedNotes), SearchString, "yellow", false);
            }
        }

        //Descriptions
        public void BindDescriptionsResult(Object s, RepeaterItemEventArgs e)
        {
            // Row
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Header)
            {
                ((Literal)e.Item.FindControl("SearchType")).Text = "Summaries that match your search query";
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KayCaseCategoryDescription _item = new KayCaseCategoryDescription(((DataRowView)e.Item.DataItem).Row);

                string trancatedNotes = Utilities.TruncateByKeyWords(Utilities.RemoveNums(_item.Description), SearchString, 1, 1, true, "", false, "...", true, "");

                //((HyperLink)e.Item.FindControl("Title")).Text = (String.Format("<big>{0}</big> : {1} vs {2}", _item.Case.CaseNumber, _item.Case.Plaintiff, _item.Case.Defendant));
                ((HyperLink)e.Item.FindControl("Title")).Text = (String.Format("<big>{0}</big> : {1} vs {2}", Utilities.HighlightKeyWords(_item.Case.CaseNumber, SearchString, "pink", false), Utilities.HighlightKeyWords(_item.Case.Plaintiff, SearchString, "pink", false), Utilities.HighlightKeyWords(_item.Case.Defendant, SearchString, "yellow", false)));
                ((HyperLink)e.Item.FindControl("Title")).NavigateUrl = _item.Case.UrlPath;
                ((Literal)e.Item.FindControl("Description")).Text = Utilities.HighlightKeyWords(Utilities.RemoveNums(trancatedNotes), SearchString, "yellow", false);
            }
        }

        //Categories
        public void BindCategoriesResult(Object s, RepeaterItemEventArgs e)
        {
            // Row
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Header)
            {
                ((Literal)e.Item.FindControl("SearchType")).Text = "Categories that match your search query";
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KayCategory _item = new KayCategory(((DataRowView)e.Item.DataItem).Row);

                ((HyperLink)e.Item.FindControl("Title")).Text = Utilities.HighlightKeyWords(_item.Title, SearchString,"yellow", false);
                ((HyperLink)e.Item.FindControl("Title")).NavigateUrl = "cases" + _item.UrlPath; 
            }
        }
    }
}
