using System;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using Kay.BLL;
using System.Web.UI.WebControls;

namespace Kay.Global
{
	/// <summary>
	///	Base class for all pages
	/// </summary>
	public class BasePage : Page
    {
        // Page init
        protected override void OnInit(EventArgs e)
        {
            // Check domain name
            if (Config.ApplicationLive && Request.ServerVariables["SERVER_NAME"] != Config.DomainName)
            {
                String redir = "http://" + Config.DomainName + Config.FriendlyUrl.PathAndQuery;
                Redirect(redir, 301);
            }

            // Check access permissions
            foreach (KayAccessPermissionsPath Permission in Config.AccessPermissions.Paths)
            {
                if (Regex.IsMatch(Config.FriendlyPath, Permission.Path, RegexOptions.IgnoreCase))
                {
                    if (Permission.Groups == 0)
                    {
                        // allowed
                        break;
                    }
                    else if ((CurrentUser.Groups & Permission.Groups) == 0 && !Regex.IsMatch(Config.FriendlyPath, "^" + Permission.LoginUrl))
                    {
                        // Not allowed
                        Response.Redirect(Utilities.AddQueryStringVariable(Permission.LoginUrl, ReturnUrl), true);
                    }
                    else
                    {
                        // Do not continue processing permissions
                        break;
                    }
                }
            }
            // Call the base method
            base.OnInit(e);
        }

        #region CMS methods

        // Sort
        public void SortList(Object s, DataGridSortCommandEventArgs e)
        {
            String url = Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, String.Format("sort={0}", e.SortExpression.ToLower()));
            Response.Redirect(url);
        }
        public String HighlightSort(DataGrid List, String DefaultSort)
        {
            // Get sort string
            String SortExpression = Request.QueryString["sort"];
            SortExpression = String.IsNullOrEmpty(SortExpression) ? DefaultSort : SortExpression; 

            // Add class to header cell
            foreach (DataGridColumn Col in List.Columns)
            {
                if (!String.IsNullOrEmpty(Col.SortExpression) && Regex.IsMatch(SortExpression, Col.SortExpression, RegexOptions.IgnoreCase))
                {
                    String align = Col.HeaderStyle.HorizontalAlign == HorizontalAlign.NotSet ? "left" : Col.HeaderStyle.HorizontalAlign.ToString().ToLower();
                    if (Regex.IsMatch(SortExpression, "desc", RegexOptions.IgnoreCase))
                    {
                        Col.HeaderStyle.CssClass = String.Format("asc-{0}", align);
                    }
                    else
                    {
                        Col.HeaderStyle.CssClass = String.Format("desc-{0}", align);
                        Col.SortExpression += "+desc";
                    }
                }
            }

            // Done
            return SortExpression;
        }

        #endregion

        private KayPage currentPage = null;
        public KayPage CurrentPage
        {
            get
            {
                if (currentPage == null)
                {
                    try
                    {
                        String _urlPath = Request.QueryString["urlpath"].ToLower() == "/default.aspx" ? "/" : Request.QueryString["urlpath"];
                        currentPage = new KayPage(_urlPath, KayDevice.Public);
                    }
                    catch
                    {
                        currentPage = new KayPage();
                    }
                    finally
                    {
                        if (currentPage.Id == 0)
                        {
                            currentPage = new KayPage();
                            currentPage.Live = false;
                        }
                    }
                }
                return currentPage;
            }
            set
            {
                currentPage = value;
            }
        }

        // Authenticated user
        private KayUser currentUser;
        public KayUser CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    try
                    {
                        // Get user from Session first, otherwise get from Auth cookie
                        if (Session["CurrentUser"] != null)
                            currentUser = (KayUser)Session["CurrentUser"];
                        else
                        {
                            int UserId = int.Parse(Utilities.DecryptText(Request.Cookies["Kay_auth"].Value));
                            currentUser = new KayUser(int.Parse(Utilities.DecryptText(Request.Cookies["Kay_auth"].Value)));
                            Session.Add("CurrentUser", currentUser);
                        }
                    }
                    catch
                    {
                        currentUser = new KayUser();
                        Session.Add("CurrentUser", currentUser);
                    }
                }
                return currentUser;
            }
            set
            {
                currentUser = value;
                Session["CurrentUser"] = CurrentUser;
            }
        }
       
        // Status message
        public String StatusMessage
        {
            get
            {
                if (Session["StatusMessage"] != null)
                {
                    return Session["StatusMessage"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                Session.Add("StatusMessage", value);
            }
        }
        public String ErrorMessage
        {
            get
            {
                if (Session["ErrorMessage"] != null)
                {
                    return Session["ErrorMessage"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                Session.Add("ErrorMessage", value);
            }
        }

        // Selected node in sitemap (use the URL)
        private String selectedNodeUrl = String.Empty;
        public String SelectedNodeUrl
        {
            get
            {
                if (String.IsNullOrEmpty(selectedNodeUrl))
                {
                    String path = String.Empty;
                    int offset = Config.FriendlyUrl.AbsolutePath.EndsWith("/") ? 0 : 1;
                    for (int i = 0; i < Config.FriendlyUrl.Segments.Length - offset; i++)
                    {
                        path += Config.FriendlyUrl.Segments[i];
                    }
                    return path;
                }
                return selectedNodeUrl;
            }
            set
            {
                selectedNodeUrl = value;
            }
        }

        // Get redirection URL
        public String ReturnUrl
        {
            get 
            {
                return "r=" + HttpUtility.UrlEncode(Config.FriendlyUrl.PathAndQuery); 
            }
        }

        // Set redirection URL
        public String ReturnToUrl()
        {
            return ReturnToUrl(HttpContext.Current.Request.ApplicationPath.ToString().ToLower());
        }
        public String ReturnToUrl(String DefaultUrl)
        {
            if (HttpContext.Current.Request.QueryString["r"] != null)
            {
                return HttpContext.Current.Request.QueryString["r"].ToString().ToLower();
            }
            else
            {
                return DefaultUrl.ToLower();
            }
        }        

        // Redirect
        public void PageNotFound()
        {
            Redirect("/error/?errorcode=404", 404);
        }
        public void Redirect(String Url, int StatusCode)
        {
            Response.StatusCode = StatusCode;
            Response.Redirect(Url, true);
        }

        // Unique IP address
        public String VisitorIpAddress
        {
            get
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;

                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
	}
}
