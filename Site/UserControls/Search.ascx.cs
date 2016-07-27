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
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Public.Common.UserControls
{
    public partial class Search : BaseUserControl
    {
        // Search url
        private String _searchUrl = "/search.aspx";
        public String SearchUrl
        {
            get { return _searchUrl; }
            set { _searchUrl = value; }
        }

        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // Do search
        public void Find(Object s, EventArgs e)
        {
            // Strip illegal characters
            //String query = Regex.Replace(Keywords.Text, @"[^a-zA-Z0-9\s]", "");

            // Create search url
            SearchUrl = Utilities.AddQueryStringVariable(SearchUrl, "q=" + Keywords.Text);

            // Redirect
            Response.Redirect(SearchUrl);
        }
    }
}