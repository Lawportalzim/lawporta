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
using System.Xml;
using Kay.Global;
using Kay.BLL;

namespace Kay.Site.Admin.Cases
{
    public partial class Edit : BasePage
    {
        // Properties
        private KayCase Case;
        private KayCase ParentCase = new KayCase();
        private Boolean Appeal = false;

        public KayCaseType SelectedType
            {
              get
              {
                KayCaseType kayCaseType = KayCaseType.Civil;
                try
                {
                  kayCaseType = (KayCaseType) Enum.Parse(typeof (KayCaseType), Request.QueryString["type"].ToString(), true);
                }
                catch
                {
                }
                return kayCaseType;
              }
            }


        // Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get selected record
            try { Case = new KayCase(int.Parse(Request.QueryString["id"]));}
            catch { Case = new KayCase(); }

            

            //appealed?
            if (Request.QueryString["appeal"] != null && Request.QueryString["id"] != null)
            {
                Appeal = Boolean.Parse(Request.QueryString["appeal"]);
                ParentCase = Case;
                if (ParentCase.Appealed)
                {
                    Case = new KayCase(ParentCase.Id, true);
                }
                else
                Case = new KayCase();
            }
            
            // Setup page
            Master.Body.Attributes.Add("class", "edit");
            PageTitle.Text = (Case.Id == 0) ? "Add a Case" : string.Format("{0} vs {1}", Utilities.HtmlSafe(Case.Plaintiff), Utilities.HtmlSafe(Case.Defendant));
            Back.NavigateUrl = ReturnToUrl("/admin/cases/");

            // Fill form fields
            if (!Page.IsPostBack)
            {
                // Details
                Plaintiff.Text = Appeal ? ParentCase.Defendant : Case.Plaintiff;
                Defendant.Text = Appeal ? ParentCase.Plaintiff : Case.Defendant;
                Number.Text = Case.CaseNumber;
                Ruler.Text = Case.Ruler;
                Dates.Text = Case.Date;
                CourtName.Text = Case.CourtName;
                Notes.Text = Case.Notes;
                FullCase.Text = Case.FullCase;
                Case.CaseType = SelectedType;
                
                // Save & exit
                SaveAndExit.Checked = Request.QueryString["exit"] != "false";
            }
        }

        // Save record
        public void Save(Object s, EventArgs e)
        {
            // Server-side validation
            if (!Page.IsValid) return;            

            // Details
            if(Appeal)Case.ParentId = ParentCase.Id;
            Case.CaseNumber = Number.Text;
            Case.Plaintiff = Plaintiff.Text;
            Case.Defendant = Defendant.Text;
            Case.Ruler = Ruler.Text;
            Case.Date = Dates.Text;
            Case.CourtName = CourtName.Text;
            Case.Notes = Notes.Text;
            Case.FullCase = FullCase.Text;
            Case.CaseType = SelectedType;

            if (!Case.UniqueCaseNumber()) { ErrorMessage = "Case number is already in use!"; return; }
           
            if (Case.Save())
            {                
                if (ParentCase.Id > 0)
                {
                    ParentCase.Appealed = true;
                    ParentCase.Save();
                }

                // redirect
                if (SaveAndExit.Checked)
                {
                    Response.Redirect(ReturnToUrl(Request.QueryString["r"].ToString()));
                }
                else
                {
                    Response.Redirect(Utilities.AddQueryStringVariable(Config.FriendlyUrl.PathAndQuery, String.Format("id={0}&exit=false", Case.Id)));
                }
                
            }
            else
            {
                // error
                ErrorMessage = "Could not save Case.";
            }
        }

        // Save record
        public void CheckCaseNumber(Object s, EventArgs e)
        {
            Case.CaseNumber = Number.Text;
            if (!Case.UniqueCaseNumber()) { ErrorMessage = "Case number is already in use!"; return; }
        }
    }
}
