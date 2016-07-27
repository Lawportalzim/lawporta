using System;
using System.Web.UI;

namespace Kay.Controls
{
    public class LinkButton : System.Web.UI.WebControls.LinkButton
    {
        protected override void OnLoad(System.EventArgs e)
        {
            // Function script
            Page.ClientScript.RegisterClientScriptBlock(
                GetType(), 
                "LinkButtonClickScript",
                "function _albc(id) { var b = document.getElementById(id); if (b && typeof(b.click) == 'undefined') { b.click = function() { var result = true; if (b.onclick) result = b.onclick();  if (typeof(result) == 'undefined' || result) { eval(b.href); } } } };", 
                true);

            // Button script
            String script = String.Format("_albc('{0}');", ClientID);
            Page.ClientScript.RegisterStartupScript(GetType(), ClientID + "Click", script, true);

            // Base
            base.OnLoad(e);
        }
    }
}
