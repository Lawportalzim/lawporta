using System;
using System.Web.UI;
using Kay.BLL;

namespace Kay.Global
{
    /// <summary>
    ///	Base class for all user controls
    /// </summary>
    public class BaseUserControl : UserControl
    {
        // Authenticated user
        public KayUser CurrentUser
        {
            get
            {
                if (this.Page is BasePage) return ((BasePage)(this.Page)).CurrentUser;
                else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
            }
        }

        

        // Selected node in sitemap (use the URL);
        public String SelectedNodeUrl
        {
            get
            {
                if (this.Page is BasePage) return ((BasePage)(this.Page)).SelectedNodeUrl;
                else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
            }
        }

        // Status message
        public String StatusMessage
        {
            get
            {
                if (this.Page is BasePage) return ((BasePage)(this.Page)).StatusMessage;
                else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
            }
            set
            {
                if (this.Page is BasePage) ((BasePage)(this.Page)).StatusMessage = value;
                else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
            }
        }
        public String ErrorMessage
        {
            get
            {
                if (this.Page is BasePage) return ((BasePage)(this.Page)).ErrorMessage;
                else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
            }
            set
            {
                if (this.Page is BasePage) ((BasePage)(this.Page)).ErrorMessage = value;
                else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
            }
        }

        // Get redirection URL
        public String ReturnUrl
        {
            get
            {
                if (this.Page is BasePage) return ((BasePage)(this.Page)).ReturnUrl;
                else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
            }
        }

        // Set redirection URL
        public String ReturnToUrl()
        {
            if (this.Page is BasePage) return ((BasePage)(this.Page)).ReturnToUrl();
            else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
        }
        public String ReturnToUrl(string DefaultUrl)
        {
            if (this.Page is BasePage) return ((BasePage)(this.Page)).ReturnToUrl(DefaultUrl);
            else throw new ApplicationException("Consuming page does not contain abstract class BasePage");
        }
        
    }
}
