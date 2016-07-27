using System;
using System.Web.UI;
using System.Text;

namespace Kay.Controls
{
    public class Date : System.Web.UI.WebControls.TextBox
    {
        // Properties
        public DateTime SelectedDate
        {
            get
            {
                try
                {
                    return DateTime.Parse(Text);
                }
                catch
                {
                    return DateTime.Today;
                }
            }
            set
            {
                Text = value.ToString("d MMMM yyyy");
            }
        }

        // Init
        protected override void OnInit(EventArgs e)
        {
            // Force properties
            this.TextMode = System.Web.UI.WebControls.TextBoxMode.SingleLine;
            this.CssClass = "datepicker";
            this.MaxLength = 256;

            // Base method
            base.OnInit(e);
        }
    }
}
