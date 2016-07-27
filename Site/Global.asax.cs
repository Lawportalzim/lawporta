using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;

namespace Site
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Response.Redirect("/error/");
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }


        //route table
        void RegisterRoutes(RouteCollection routes)
        {
            //global
            routes.MapPageRoute("account", "accounts", "~/AccountSettings.aspx", true);

            //Civil
            routes.MapPageRoute("civil", "cases/civil", "~/Civil.aspx", true);
            routes.MapPageRoute("civilcategory", "cases/civil/{id}/{category}", "~/Civil.aspx", true);
            routes.MapPageRoute("civiljudge", "cases/civil/{ruler}", "~/Civil.aspx", true);
            routes.MapPageRoute("civildetail", "cases/civil/{id}/{plantiff}/{defendant}", "~/Civil.aspx", true); 

            //Criminal
            routes.MapPageRoute("criminal", "cases/criminal", "~/Criminal.aspx", true);
            routes.MapPageRoute("criminalcategory", "cases/criminal/{id}/{category}", "~/Criminal.aspx", true);
            routes.MapPageRoute("criminaljudge", "cases/criminal/{ruler}", "~/Criminal.aspx", true);
            routes.MapPageRoute("criminaldetail", "cases/criminal/{id}/{plantiff}/{defendant}", "~/Criminal.aspx", true);   

            //help
            routes.MapPageRoute("help", "help", "~/Help.aspx", true);

            //error
            routes.MapPageRoute("error", "error", "~/Error.aspx", true);
        }

    }
}
