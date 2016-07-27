using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin.Dashboard
{
    public partial class Default : BasePage
    {
        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // custom class
            Master.Body.Attributes.Add("class", "dashboard");
            SetupOverview();
        }

        private void SetupOverview()
        {
            List.DataSource = KayUser.List();
            List.DataBind();
        }

        //bind users
        public void BindUsers(object s, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            KayUser kayUser = new KayUser(((DataRowView)e.Item.DataItem).Row);
            TimeSpan timeSpan = kayUser.ExpiryDate.Subtract(DateTime.Now);
            int days = timeSpan.Days;
            if (days <= 5 & days > 3)
            {
                ((Literal)e.Item.FindControl("Name")).Text = string.Format("<li> <strong>Name: </strong>{0}<br />", (object)kayUser.FullName);
                ((Literal)e.Item.FindControl("Username")).Text = string.Format("<strong>Username: </strong>{0}<br />", (object)kayUser.EmailAddress);
                ((Literal)e.Item.FindControl("ExpiryDate")).Text = string.Format("<strong>Expiry Date: </strong>{0}<br>", (object)kayUser.ExpiryDate.ToString("dd/MM/yyyy"));
                Literal literal = (Literal)e.Item.FindControl("DaysLeft");
                string format = "<strong>Days Left:</strong> <span class=\"label label-info\">{0}</span><br /> ";
                timeSpan = kayUser.ExpiryDate.Subtract(DateTime.Now);
                string str1 = timeSpan.Days.ToString();
                string str2 = string.Format(format, (object)str1);
                literal.Text = str2;
                ((Literal)e.Item.FindControl("Company")).Text = string.Format("<strong>Company: </strong>{0}</li>", (object)kayUser.Company.Name);
            }
            else if (days <= 3 & days > 0)
            {
                ((Literal)e.Item.FindControl("Name")).Text = string.Format("<li> <strong>Name: </strong>{0}<br />", (object)kayUser.FullName);
                ((Literal)e.Item.FindControl("Username")).Text = string.Format("<strong>Username: </strong>{0}<br />", (object)kayUser.EmailAddress);
                ((Literal)e.Item.FindControl("ExpiryDate")).Text = string.Format("<strong>Expiry Date: </strong>{0}<br>", (object)kayUser.ExpiryDate.ToString("dd/MM/yyyy"));
                Literal literal = (Literal)e.Item.FindControl("DaysLeft");
                string format = "<strong>Days Left: </strong> <span class=\"label label-warning\">{0}</span><br /> ";
                timeSpan = kayUser.ExpiryDate.Subtract(DateTime.Now);
                string str1 = timeSpan.Days.ToString();
                string str2 = string.Format(format, (object)str1);
                literal.Text = str2;
                ((Literal)e.Item.FindControl("Company")).Text = string.Format("<strong>Company: </strong>{0}</li>", (object)kayUser.Company.Name);
            }
            else if (days <= 0)
            {
                ((Literal)e.Item.FindControl("Name")).Text = string.Format("<li> <strong>Name: </strong>{0}<br />", (object)kayUser.FullName);
                ((Literal)e.Item.FindControl("Username")).Text = string.Format("<strong>Username: </strong>{0}<br />", (object)kayUser.EmailAddress);
                ((Literal)e.Item.FindControl("ExpiryDate")).Text = string.Format("<strong>Expiry Date: </strong>{0}<br>", (object)kayUser.ExpiryDate.ToString("dd/MM/yyyy"));
                Literal literal = (Literal)e.Item.FindControl("DaysLeft");
                string format = "<strong>Days Left: </strong> <span class=\"label label-important\">{0}</span><br /> ";
                timeSpan = kayUser.ExpiryDate.Subtract(DateTime.Now);
                string str1 = timeSpan.Days.ToString();
                string str2 = string.Format(format, (object)str1);
                literal.Text = str2;
                ((Literal)e.Item.FindControl("Company")).Text = string.Format("<strong>Company: </strong>{0}</li>", (object)kayUser.Company.Name);
            }
        }


    }
}
