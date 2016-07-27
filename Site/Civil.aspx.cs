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
using Kay.BLL;
using Kay.Global;

namespace Site
{
    public partial class Civil : BasePage
    {
        private static int CatId
        {
            get
            {
                if (HttpContext.Current.Request.RequestContext.RouteData != null &&
                    HttpContext.Current.Request.RequestContext.RouteData.Values["id"] != null &&
                    HttpContext.Current.Request.RequestContext.RouteData.Values["category"] != null)
                {
                    return int.Parse(HttpContext.Current.Request.RequestContext.RouteData.Values["id"].ToString());
                }
                else
                    return 0;
            }
        }

        private static int Id
        {
            get
            {
                if (HttpContext.Current.Request.RequestContext.RouteData != null &&
                    HttpContext.Current.Request.RequestContext.RouteData.Values["id"] != null &&
                    HttpContext.Current.Request.RequestContext.RouteData.Values["plantiff"] != null &&
                    HttpContext.Current.Request.RequestContext.RouteData.Values["defendant"] != null)
                {
                    return int.Parse(HttpContext.Current.Request.RequestContext.RouteData.Values["id"].ToString());
                }
                else
                    return 0;
            }
        }

        private static string Ruler
        {
            get
            {
                if (HttpContext.Current.Request.RequestContext.RouteData != null && HttpContext.Current.Request.RequestContext.RouteData.Values["ruler"] != null)
                {
                    return RemoveDash(HttpContext.Current.Request.RequestContext.RouteData.Values["ruler"].ToString());
                }
                else
                    return string.Empty;
            }
        }

        private HttpCookie _cookie;
        private HttpCookie TypeCookie
        {

            get
            {               
                _cookie = HttpContext.Current.Request.Cookies["type"];
                if(_cookie != null) return _cookie;
                else return new HttpCookie("type");               
            }
        }

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //clear case type cookie            
            if (TypeCookie.Value != String.Empty)
            {
                TypeCookie.Value = String.Empty;
                HttpContext.Current.Response.Cookies.Add(TypeCookie);
            }

            // Add case type cookie            
            TypeCookie.Value = "civil";
            TypeCookie.Expires = DateTime.Now.AddDays(1);
            TypeCookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(TypeCookie);

            // Setup page
            if (!Page.IsPostBack)
            {                

                if (Id > 0)
                {
                    Case = new KayCase(Id);
                    SetupDetail();
                }
                else if (CatId > 0)
                {
                    Category = new KayCategory(CatId);
                    SetupListByCategory();
                }
                else
                {
                    SetupCasesList();
                }
            }
           
        }

        // List
        private String _title = String.Empty;
        private String _articleUrl = String.Empty;
        private void SetupCasesList()
        {
            // Toggle controls
            Detail.Visible = false;
            List.Visible = true;
            ListPager.Visible = true;
            CategoryCasePager.Visible = false;
            EmptyList.Visible = false;
            Back.Visible = MainMenu.Visible = false;

            Master.MasterPageTitle = "Civil Cases";
            // Bind cases
            DataView Data = KayCase.LiveList();

            // Default
            _title = CurrentPage.Title;

            // Filter
            String Filter = "Cases_CaseType = 2";

            if (Ruler != string.Empty)
            {
                Filter += String.Format(" And Cases_RulerUrl = '{0}'", Utilities.GenerateUrlPath(Ruler));
                Master.MasterPageTitle = String.Format("Civil Cases by {0}", Ruler);
                Back.Visible = MainMenu.Visible = true;
            }

            Data.RowFilter = Filter;

            // Bind paged data
            ListPager.Data = Data;
            List.DataSource = ListPager.PagedData;
            List.DataBind();
            List.Visible = (ListPager.RecordCount > 0);
            EmptyList.Visible = (ListPager.RecordCount == 0);
            ListPager.Visible = (ListPager.PageCount > 1);            

            // Set title
            Title = _title;
            
        }

        public void BindCase(Object s, RepeaterItemEventArgs e)
        {
            // Row
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KayCase _case = new KayCase(((DataRowView)e.Item.DataItem).Row);

                if (_case.ParentId > 0)
                {
                    ((HyperLink)e.Item.FindControl("ApealStatus")).Text = "<span>[SC]/APP</span><br />";
                    ((HyperLink)e.Item.FindControl("ApealStatus")).NavigateUrl = new KayCase(_case.ParentId).UrlPath;
                }

                if (_case.Appealed)
                {
                    ((HyperLink)e.Item.FindControl("ApealStatus")).Text = "<span class='ap'>Appealed</span><br />";
                    ((HyperLink)e.Item.FindControl("ApealStatus")).NavigateUrl = _case.Appealed ? new KayCase(_case.Id, true).UrlPath : "";
                }

                ((HyperLink)e.Item.FindControl("Parties")).Text = (String.Format("<big>{0}</big> : {1} vs {2}", _case.CaseNumber, _case.Plaintiff, _case.Defendant));
                ((HyperLink)e.Item.FindControl("Parties")).NavigateUrl = _case.UrlPath;
                ((HyperLink)e.Item.FindControl("Ruler")).Text = _case.Ruler;
                ((HyperLink)e.Item.FindControl("Ruler")).NavigateUrl = String.Format("/cases/civil{0}",Utilities.GenerateUrlPath(_case.Ruler));
                ((Literal)e.Item.FindControl("Dates")).Text = _case.Date;
                ((Literal)e.Item.FindControl("Summary")).Text = Utilities.Truncate(Utilities.RemoveNums(_case.Notes), 20);
            }
        }
        // Category
        private KayCategory _category;
        private KayCategory Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new KayCategory(CatId);
                }
                return _category;
            }
            set
            {
                _category = value;
            }
        }

        private void SetupListByCategory()
        {
            // Toggle controls
            Detail.Visible = false;
            List.Visible = false;
            CategoryCase.Visible = true;
            CategoryCasePager.Visible = true;
            ListPager.Visible = false;
            EmptyList.Visible = false;

            // Bind cases
            DataView Data = KayCaseCategoryDescription.ListByCategory(Category.Id);

            // Default
            _title = CurrentPage.Title;

            // Filter
            String Filter = "";

            Data.RowFilter = Filter;

            // Bind paged data
            CategoryCasePager.Data = Data;
            CategoryCase.DataSource = CategoryCasePager.PagedData;
            CategoryCase.DataBind();
            CategoryCase.Visible = (CategoryCasePager.RecordCount > 0);
            EmptyList.Visible = ((CategoryCasePager.RecordCount == 0) && !Category.HasChildren());
            CategoryCasePager.Visible = (CategoryCasePager.PageCount > 1);
            ParentCatText.Text = Category.HasChildren() ? String.Format("<p>There are <strong>({0})</strong> sub categories under <strong>{1}</strong>. <br> Use the navigation on your right to browse them</p>", new KayCategoryCollection(KayCategory.List(Category.Id)).Count, Category.Title) : "";

            // Set title
            Title = _title;
            Master.MasterPageTitle = Category.Title;
        }

        public void BindCategoryCase(Object s, RepeaterItemEventArgs e)
        {            
            // Row
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {                
                KayCaseCategoryDescription _item = new KayCaseCategoryDescription(((DataRowView)e.Item.DataItem).Row);

                if (_item.Case.ParentId > 0)
                {
                    ((HyperLink)e.Item.FindControl("ApealStatus")).Text = "<span>[SC]/APP</span><br />";
                    ((HyperLink)e.Item.FindControl("ApealStatus")).NavigateUrl = new KayCase(_item.Case.ParentId).UrlPath;
                }

                if (_item.Case.Appealed)
                {
                    ((HyperLink)e.Item.FindControl("ApealStatus")).Text = "<span class='ap'>Appealed</span><br />";
                    ((HyperLink)e.Item.FindControl("ApealStatus")).NavigateUrl = _item.Case.Appealed ? new KayCase(_item.Case.Id, true).UrlPath : "";
                }

                ((HyperLink)e.Item.FindControl("Parties")).Text = (String.Format("<big>{0}</big> : {1} vs {2}", _item.Case.CaseNumber, _item.Case.Plaintiff, _item.Case.Defendant));
                ((HyperLink)e.Item.FindControl("Parties")).NavigateUrl = _item.Case.UrlPath;
                ((HyperLink)e.Item.FindControl("Ruler")).Text = _item.Case.Ruler ;
                ((HyperLink)e.Item.FindControl("Ruler")).NavigateUrl = String.Format("/cases/civil{0}", Utilities.GenerateUrlPath(_item.Case.Ruler));
                ((Literal)e.Item.FindControl("Summary")).Text = Utilities.Truncate(Utilities.RemoveNums(_item.Description), 50);
                ((Literal)e.Item.FindControl("FullSummary")).Text = Utilities.RemoveNums(_item.Description);
            }
            
        }
        // Detail
        private KayCase _case;
        private KayCase Case
        {
            get
            {
                if (_case == null)
                {
                    _case = new KayCase(Id);
                }
                return _case;
            }
            set
            {
                _case = value;
            }
        }
        private void SetupDetail()
        {
            // Toggle controls
            Detail.Visible = true;
            List.Visible = false;
            ListPager.Visible = false;
            CategoryCasePager.Visible = false;
            EmptyList.Visible = false;
            CategoryCasePager.Visible = false;
            CaseDetailList.Visible = true;


            // Bind cases
            DataView Data = KayCaseCategoryDescription.ListByCase(Case.Id);

            // Default
            _title = CurrentPage.Title;

            // Bind paged data           
            CaseDetailList.DataSource = Data;
            CaseDetailList.DataBind();

            // Set title
            Title = _title;
            if (Case.ParentId > 0)
            {
                ApealStatus.Text = "<span>[SC]/APP</span>";
                ApealStatus.NavigateUrl = new KayCase(Case.ParentId).UrlPath;
            }

            if (Case.Appealed)
            {
                ApealStatus.Text = "<span class='ap'>Appealed</span>";
                ApealStatus.NavigateUrl = Case.Appealed ? new KayCase(Case.Id, true).UrlPath : "";
            }
            Notes.Text = Case.Notes;
            Master.MasterPageTitle = String.Format("{0} - {1} vs {2}", Case.CaseNumber, Case.Plaintiff, Case.Defendant);
            FullCase.Text = Case.FullCase;
        }
        public void BindCaseDetail(Object s, RepeaterItemEventArgs e)
        {
            // Row
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KayCaseCategoryDescription _item = new KayCaseCategoryDescription(((DataRowView)e.Item.DataItem).Row);

                ((HyperLink)e.Item.FindControl("Category")).Text = _item.Category.Title;
                ((HyperLink)e.Item.FindControl("Category")).NavigateUrl = "cases/" +  _item.Category.UrlPath;
                //((Literal)e.Item.FindControl("Dates")).Text = _item.Case.Date;
                ((Literal)e.Item.FindControl("Summary")).Text = _item.Description;

                _articleUrl = _case.UrlPath;
            }
        }
        
        public static string RemoveDash(string Text)
        {
            Text = Regex.Replace(Text, @"-", " ");
            return Text;
        }
    }
}
