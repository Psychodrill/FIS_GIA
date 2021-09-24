namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Collections.Generic;
    using System;
    using System.Data;
    using Fbs.Web.Helpers;
    /// <summary>
    /// The batch check result.
    /// </summary>
    public partial class BatchCheckResult : BasePage
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether HasResults.
        /// </summary>
        public bool HasResults
        {
            get
            {
                return this.dgResultsList.Items.Count > 0;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The ds results list_ selecting.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dsResultsList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 1200;
        }

        protected override void OnInit(EventArgs e)
        {
            this.dgResultsList.ItemCommand += new DataGridCommandEventHandler(dgResultsList_ItemCommand);
            this.dgResultsList.ItemDataBound += new DataGridItemEventHandler(dgResultsList_ItemDataBound);
            base.OnInit(e);
        }

        private int? _commandIndex;
        void dgResultsList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (this._commandIndex.HasValue && this._commandIndex == e.Item.ItemIndex)
            {
                 var dataRow = e.Item.DataItem as DataRowView;
                Dictionary<String, string> values = new Dictionary<string, string>();
                //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&number={1}&check=byNumber", HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["number"]);
                values.Add("CertNumber", dataRow["CertificateNumber"].ToString());
                values.Add("SubjectMarks", dataRow.ExtractSubjectsWithMarksString());
                values.Add("FirstName", dataRow["CheckFirstName"].ToString());
                values.Add("LastName", dataRow["CheckLastName"].ToString());
                values.Add("GivenName", dataRow["CheckPatronymicName"].ToString());
                this.Session["NoteInfo"] = values;
                this.Response.Redirect("PrintNotFoundNote.aspx", true);
            }
        }

        void dgResultsList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "PrintNotFound")
            {
                this._commandIndex = e.Item.ItemIndex;
                this.dgResultsList.DataBind();
            }

        }
        #endregion
    }
}