using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin
{
    public partial class Error : BasePage
    {
        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Class
            Master.Body.Attributes.Add("class", "error");

            // Get event
            KayEventHistory Event;
            try { Event = new KayEventHistory(Double.Parse(Request.QueryString["eventId"])); }
            catch { Event = new KayEventHistory(); }
            
            // Friendly message
            if (Request.QueryString["errorCode"] == "404")
            {
                Feedback.Text = "The page you were looking for could not be found.";
            }
            else
            {
                Feedback.Text = "There are was an error processing your request.";
            }
            EventId.Text = Event.Id.ToString();

            // super user on staging site
            if (CurrentUser.SuperUser && !Config.ApplicationLive)
            {
                ErrorDisplay.Visible = true;
                ErrorData.Text = Event.Data;
            }
        }
    }
}
