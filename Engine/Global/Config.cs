using System;
using System.Configuration;
using System.Net.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Xml;
using Kay.BLL;

namespace Kay.Global
{
	/// <summary>
	///	Global properties used throughout the application.
	/// </summary>
	public class Config
    {   
        public static Uri FriendlyUrl
        {
            get
            {
                String url = Regex.Replace(HttpContext.Current.Request.RawUrl, @"^/error.aspx\?(404|403);http[s]?://[^/]*", "", RegexOptions.IgnoreCase);
                url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + url;
                return new Uri(url.ToLower());
            }
        }
        public static String FriendlyPath
        {
            get
            {
                return FriendlyUrl.AbsolutePath.ToLower().Replace("default.aspx", "");
            }
        }

        // Web config modules
        public static KayAccessPermissions AccessPermissions
        {
            get
            {
                return (KayAccessPermissions)ConfigurationManager.GetSection("accessPermissions");
            }
        }
        
        // E-mail 
        public static Boolean LiveEmails
        {
            get
            {
                return ApplicationLive;
            }
        }
        
        // Developer e-mail
        public static String DeveloperEmail = "kudakwashe.gore@gmail.com";

        // Administrator e-mail      
        public static String AdministratorEmail = "";       

        // Database in use
        public static String DatabaseType = "MySql";

        // Domain name        
        public static String DomainName = "localhost";        

        // Application name        
        public static String ApplicationName = "LP";       

        // Application state        
        public static Boolean ApplicationLive = false;       

        // Absolute path
        private static String applicationPath;
        public static String ApplicationPath
        {
            get
            {
                if (String.IsNullOrEmpty(applicationPath))
                {
                    try
                    {
                        applicationPath = Utilities.GetAppSettingOption("ApplicationPath");
                    }
                    catch
                    {
                        applicationPath = HostingEnvironment.MapPath("/");
                    }
                }
                return applicationPath;
            }
            set
            {
                applicationPath = value;
            }
        }

        public static HttpCookie _cookie;
        public static HttpCookie OnlineCookie
        {

            get
            {
                _cookie = HttpContext.Current.Request.Cookies["online"];
                if (_cookie != null) return _cookie;
                else return new HttpCookie("online");
            }
            set
            {
                _cookie = value;                
            }
        }

        // Encryption password
        public const String Rc4Password = "Kay54";

        // POP3 MailBee
        public const String MailBeeLicenseKey = "3254";
	}
}
