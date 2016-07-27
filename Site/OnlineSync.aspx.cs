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
    public partial class OnlineSync : BasePage
    {
        private string _userIdCookieValue;
        private string _online;

        private string UserIdCookieValue
        {
            get
            {
                _userIdCookieValue = Request.QueryString["userid"];
                return _userIdCookieValue;
            }
        }
        private string Online
        {
            get
            {
                _online = this.Request.QueryString["online"];
                return _online;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            KayUser kayUser;
            Exception exception;
            try
            {
                kayUser = new KayUser(int.Parse(Utilities.DecryptText(UserIdCookieValue)));
            }
            catch (Exception ex)
            {
                exception = ex;
                kayUser = new KayUser();
            }
            bool flag;
            try
            {
                flag = bool.Parse(Online);
            }
            catch (Exception ex)
            {
                exception = ex;
                flag = false;
            }
            kayUser.LastSeen = DateTime.Now;
            if (kayUser.Id > 0 & !flag)
            {
                kayUser.Online = false;
                kayUser.Save();
                Response.Write("{\"response\": OK}");
            }
            else if (kayUser.Id > 0 & flag)
            {
                kayUser.Online = true;
                kayUser.Save();
                Response.Write("{\"response\": OK}");
            }
            else
                Response.Write("{\"response\": ERROR}");
        }
    }
}
