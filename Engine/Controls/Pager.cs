using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kay.Global;

namespace Kay.Controls
{
    public class Pager : System.Web.UI.WebControls.WebControl
    {
        // Current page index
        public int currentPageIndex = -1;
        public int CurrentPageIndex
        {
            get
            {
                if (currentPageIndex == -1)
                {
                    try { currentPageIndex = int.Parse(HttpContext.Current.Request.QueryString["pg"]); }
                    catch { currentPageIndex = 0; }
                }
                return currentPageIndex < 0 ? 0 : currentPageIndex > PageCount - 1 ? PageCount - 1 : currentPageIndex;
            }
            set { currentPageIndex = value; }
        }

        // Record count
        private int recordCount = 0;
        public int RecordCount
        {
            get { return recordCount; }
            set
            {
                if (value >= 0) recordCount = value;
                pageCount = int.Parse((Math.Ceiling(double.Parse(recordCount.ToString()) / double.Parse(PageSize.ToString()))).ToString());
            }
        }

        // Page size
        private int pageSize = -1;
        public int PageSize
        {
            get
            {
                if (pageSize == -1)
                {
                    try { pageSize = int.Parse(HttpContext.Current.Request.QueryString["sz"]); }
                    catch { pageSize = 50; }
                }
                return Math.Max(pageSize, 1);
            }
            set { if (value > 0) pageSize = value; }
        }

        // Page count
        private int pageCount = 1;
        public int PageCount
        {
            get { return pageCount < 1 ? 1 : pageCount; }
        }

        // Show page jump
        private bool showPageJump = true;
        public bool ShowPageJump
        {
            get { return showPageJump; }
            set { showPageJump = value; }
        }

        // Show page shortcuts
        private bool showPageShortcuts = false;
        public bool ShowPageShortcuts
        {
            get { return showPageShortcuts; }
            set { showPageShortcuts = value; if (value) ShowPageJump = false; }
        }

        // Data
        private DataView _data;
        public DataView Data
        {
            set
            {
                _data = value;

                // set record count
                RecordCount = _data.Count;

                // set paged data
                PagedData = new PagedDataSource();
                PagedData.AllowPaging = true;
                PagedData.PageSize = PageSize;
                PagedData.CurrentPageIndex = CurrentPageIndex;
                PagedData.DataSource = _data;
            }
            get
            {
                return _data;
            }
        }
        public PagedDataSource PagedData;

        // Render HTML
        protected override void Render(HtmlTextWriter writer)
        {
            // Base method
            base.AddAttributesToRender(writer);

            // Build links
            string PreviousUrl = CurrentPageIndex > 0 ? Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, "pg=" + (CurrentPageIndex - 1).ToString()) : "";
            string NextUrl = CurrentPageIndex < PageCount - 1 ? Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, "pg=" + (CurrentPageIndex + 1).ToString()) : "";

            // Build page options
            string PageOptions = "";
            for (int i = 0; i < PageCount; i++)
            {
                string Selection = i == CurrentPageIndex ? "selected" : "";
                PageOptions += "<a href=\"" + Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, "pg=") + i.ToString() + "\"  class=" + Selection + ">" + (i + 1).ToString() + "</a>";
            }
            string PageOptionsJavascript = "location.href='" + Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, "pg=") + "'+this.value;";


            // Build HTML
            // **********

            // Start DIV
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "paging");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            // Previous anchor
            if (PreviousUrl == "") writer.WriteLine("<a class=\"previous disabled\" title=\"No previous pages\">prev</a>");
            else writer.WriteLine("<a href=\"" + PreviousUrl + "\" class=\"previous\" title=\"Previous page\">prev</a>");

            // Page jump
            if (ShowPageJump)
            {               
                writer.WriteLine(PageOptions);               
            }

            // Page shortcuts
            if (showPageShortcuts)
            {
                // Setup more links
                var _startIndex = CurrentPageIndex - 2;
                if (_startIndex < 0) _startIndex = 0;
                if (PageCount > 5 && PageCount - _startIndex < 5) _startIndex = PageCount - 5;

                writer.WriteLine("<div class=\"more\">");

                if (_startIndex > 0)
                {
                    writer.WriteLine(String.Format("<a href=\"{0}\"{2}>{1}</a>", Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, "pg=0"), 1, ""));
                }
                for (var i = _startIndex; i < PageCount && i < _startIndex + 5; i++)
                {
                    string selected = CurrentPageIndex == i ? " class=\"selected\"" : "";
                    writer.WriteLine(String.Format("<a href=\"{0}\"{2}>{1}</a>", Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, "pg=" + i), i + 1, selected));
                }

                writer.WriteLine("</div>");
            }

            // Next anchor
            if (NextUrl == "") writer.WriteLine("<a class=\"next disabled\" title=\"No next pages\">next</a>");
            else writer.WriteLine("<a href=\"" + NextUrl + "\" class=\"next\" title=\"Next page\">next</a>");

            // Close tag         
            writer.RenderEndTag();
        }
    }
}
