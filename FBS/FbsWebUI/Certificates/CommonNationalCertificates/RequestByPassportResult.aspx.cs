namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Web.UI.WebControls;

    using Fbs.Core.UICheckLog;
    using Fbs.Web.ViewBase;
    using System.Web;
    using System.Collections.Generic;

    /// <summary>
    /// The request by passport result.
    /// </summary>
    public partial class RequestByPassportResult : CertificateCheckResultBase
    {
        // Номер колонки "Документ".
        #region Constants and Fields

        private static int DocumentColumnNumber = 4;

        #endregion

        #region Methods

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // this.PageTitle = AccountExtentions.GetFullName(Request.QueryString["LastName"], 
            // Request.QueryString["FirstName"], Request.QueryString["PatronymicName"]);
        }

        /// <summary>
        /// The dg search_ init.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dgSearch_Init(object sender, EventArgs e)
        {
            // Покажу колонку "Документ" если пользователь имеет роль ViewCommonNationalCertificateDocument.
            this.dgSearch.Columns[DocumentColumnNumber].Visible =
                this.User.IsInRole("ViewCommonNationalCertificateDocument");
        }

        /// <summary>
        /// The dg search_ pre render.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dgSearch_PreRender(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["Ev"]))
            {
                string CNEId = string.Empty;
                foreach (DataGridItem Item in this.dgSearch.Items)
                {
                    var HF = (HiddenField)Item.FindControl("hfCNEId");
                    if (HF != null)
                    {
                        CNEId = HF.Value;
                        break; // Берем только 1й результат
                    }
                }

                CheckLogDataAccessor.UpdateCheckEvent(this.Request.QueryString["Ev"], CNEId);
            }

            this.phUniqueChecks.Visible = this.dgSearch.Items.Count > 0;
        }

        protected string GenerateNotFoundPrintLink()
        {
            Dictionary<String, string> values = new Dictionary<string, string>();
            //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&number={1}&check=byNumber", HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["number"]);
            values.Add("Year", this.Request.QueryString["Year"]);
            values.Add("GivenName", this.Request.QueryString["PatronymicName"]);
            values.Add("FirstName", this.Request.QueryString["FirstName"]);
            values.Add("LastName", this.Request.QueryString["LastName"]);
            values.Add("PassportNumber", this.Request.QueryString["Number"]);
            values.Add("Series", this.Request.QueryString["Series"]);
            this.Session["NoteInfo"] = values;
            return String.Format("PrintNotFoundNote.aspx");
            //return String.Format("PrintNotFoundNote.aspx?Year={0}&check=byPassportName&GivenName={1}&FirstName={2}&LastName={3}&Series={4}&Number={5}", HttpUtility.UrlEncode(this.Request.QueryString["Year"]), this.Request.QueryString["PatronymicName"], this.Request.QueryString["FirstName"], this.Request.QueryString["LastName"], this.Request.QueryString["Series"], this.Request["Number"]);
        }
        #endregion
    }
}