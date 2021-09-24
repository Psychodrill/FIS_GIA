namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Web.UI.WebControls;

    using Fbs.Core.UICheckLog;
    using Fbs.Web.ViewBase;
    using System.Web;
    using System.Collections.Generic;

    /// <summary>
    /// The request by marks result.
    /// </summary>
    public partial class RequestByMarksResult : CertificateCheckResultBase
    {
        // Номер колонки "Документ".
        #region Constants and Fields

        private static int DocumentColumnNumber = 4;

        #endregion

        #region Methods

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
        /// The dg search_ item created.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dgSearch_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            // if (!String.IsNullOrEmpty(Request.QueryString["Ev"]))
            // {
            // string CNEId = Convert.ToInt32(((HiddenField)e.Item.FindControl("hfCNEId")).Value);
            // CheckLogDataAccessor.UpdateCheckEvent(Request.QueryString["Ev"], CNEId);
            // }
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
            values.Add("SubjectMarks", this.Request.QueryString["SubjectMarks"]);
            values.Add("GivenName", this.Request.QueryString["PatronymicName"]);
            values.Add("FirstName", this.Request.QueryString["FirstName"]);
            values.Add("LastName", this.Request.QueryString["LastName"]);
            this.Session["NoteInfo"] = values;
            return String.Format("PrintNotFoundNote.aspx");
            //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&check=byMarkName&GivenName={1}&FirstName={2}&LastName={3}", HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["PatronymicName"], this.Request.QueryString["FirstName"], this.Request.QueryString["LastName"]);
        }

        protected void dsSearch_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 600;
        }

        #endregion
    }
}