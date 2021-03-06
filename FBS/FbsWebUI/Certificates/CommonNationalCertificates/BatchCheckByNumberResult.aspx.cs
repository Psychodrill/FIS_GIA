namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Fbs.Core.CNEChecks;
    using Fbs.Core.WebServiceCheck;

    using FbsChecksClient;
    using Fbs.Web.Helpers;
    using FbsServices;
    using UserCredentials = FbsChecksClient.WSChecksReference.UserCredentials;
    using System.Web;
    using System.Collections.Generic;
    using System.Web.UI;

    /// <summary>
    /// The batch check result.
    /// </summary>
    public partial class BatchCheckByNumberResult : BasePage
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether HasResults.
        /// </summary>
        public bool HasResults
        {
            get
            {
                int count = this.dgResultsList.Items.Count;
                if (count == 0)
                {
                    long id = Convert.ToInt64(this.Request.QueryString["Id"]);
                    if (id > 0)
                    {
                        var checkClient = new WSCheckClient();
                        XmlElement xml = null;
                        if (checkClient.SearchCommonNationalExamCertificateCheckByOuterId(this.User.Identity.Name, id, ref xml) == (int)WebServiceReplyCodes.UserIsBanned)
                        {
                            this.Response.Redirect("CheckBanPage.aspx");
                        }

                        var cnecService = new CNECService();
                        cnecService.AddCheckBatchResult(xml, id);
                        if (!string.IsNullOrEmpty(xml.InnerXml))
                        {
                            this.Response.Redirect(this.Request.RawUrl);
                        }
                    }
                }

                return count > 0;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// id пакета
        /// </summary>
        protected long BatchId
        {
            get
            {
                return this.GetParamLong("id");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get check link.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="dataItem">
        /// The data item.
        /// </param>
        /// <param name="forNewCert">
        /// The for new cert.
        /// </param>
        /// <returns>
        /// The get check link.
        /// </returns>
        protected string GetCheckLink(string number, string lastName, DataRowView dataItem, bool forNewCert)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]))
            {
                string sourceParam = "batchId";
                string sourceValue = this.BatchId.ToString();
                if (forNewCert)
                {
                    sourceValue = CheckUtil.GetCheckHash(this.User.Identity.Name, number);
                    sourceParam = "check";
                }

                return
                    string.Format(
                        "<a href=\"/Certificates/CommonNationalCertificates/CheckResultForOpenedFbs.aspx?number={0}&{2}={1}\">{0}</a>", 
                        number, 
                        sourceValue, 
                        sourceParam);
            }

            return
                string.Format(
                    "<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}&LastName={1}\">{0}</a>", 
                    number, 
                    lastName);
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
        /// <summary>
        /// The show certificate number.
        /// </summary>
        /// <param name="dataItem">
        /// The data item.
        /// </param>
        /// <returns>
        /// The show certificate number.
        /// </returns>
        protected string ShowCertificateNumber(object dataItem)
        {
            var dataRow = (DataRowView)dataItem;
            string number = dataRow["CertificateNumber"].ToString();
            bool isDeny = Convert.ToBoolean(dataRow["IsDeny"]);
            bool isExist = Convert.ToBoolean(dataRow["IsExist"]);
            string newNumber = dataRow["DenyNewCertificateNumber"].ToString();
            string lastName = string.IsNullOrEmpty(dataRow["LastName"].ToString())
                                  ? dataRow["CheckLastName"].ToString()
                                  : dataRow["LastName"].ToString();
            string denyComment = dataRow["DenyComment"].ToString();

            string numberString = this.GetCheckLink(number, lastName, dataRow, false);
            if (!isExist)
            {
                return string.Format(
                        "<span title='Свидетельство №{0} не найдено' style=\"color:Red\">не&nbsp;найдено</span><br/>{1}",
                        number,
                        number);
                //string notFoundLink = String.Format("PrintNotFoundNote.aspx?check=byNumber&number={0}&SubjectMarks={1}", dataRow["CertificateNumber"], HttpUtility.UrlEncode(dataRow.ExtractSubjectsWithMarksString()));
                //return
                //    string.Format(
                //        "<a title='Свидетельство №{0} не найдено' href='{2}' style=\"color:Red\">не&nbsp;найдено</a><br/>{1}", 
                //        number, 
                //        number,notFoundLink);
            }

            string newNumberString = this.GetCheckLink(newNumber, lastName, dataRow, true);
            if (isDeny)
            {
                return
                        string.Format(
                        "<span title='Свидетельство №{0} аннулировано по следующей причине:\n{1}' style=\"color:Red\">аннулировано<br/>{3}{2}</span>",
                        number,
                        denyComment,
                        string.IsNullOrEmpty(newNumber) ? string.Empty : string.Format("<br/>актуальное<br/>{0}", newNumberString),
                        numberString);
            }

            return  numberString ;
        }

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

        #endregion
    }
}