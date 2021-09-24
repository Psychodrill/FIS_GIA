namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Fbs.Core.CNEChecks;
    using Fbs.Core.UICheckLog;
    using Fbs.Core.WebServiceCheck;
    using Fbs.Web.ViewBase;

    using FbsChecksClient;
    using UserCredentials = FbsChecksClient.WSChecksReference.UserCredentials;
    using System.Collections.Generic;

    /// <summary>
    /// The request by passport result for opened fbs.
    /// </summary>
    public partial class RequestByPassportResultForOpenedFbs : CertificateCheckResultBase
    {
        // Номер колонки "Документ".
        #region Constants and Fields

        private static int DocumentColumnNumber = 3;

        #endregion

        #region Methods

        /// <summary>
        /// The get certificate link with hash.
        /// </summary>
        /// <param name="dataItem">
        /// The data item.
        /// </param>
        /// <returns>
        /// The get certificate link with hash.
        /// </returns>
        protected string GetCertificateLinkWithHash(object dataItem)
        {
            // Открытая версия приложения
            var itemCasted = dataItem as DataRowView;
            if (itemCasted == null)
            {
                return string.Empty;
            }

            object isDenyRaw = itemCasted["IsDeny"];
            bool isDeny = !(isDenyRaw is DBNull || Convert.ToBoolean(Convert.ToInt32(isDenyRaw)) == false);

            var certNumber = string.IsNullOrEmpty(itemCasted["CertificateNumber"].ToString()) ? "" : (string)itemCasted["CertificateNumber"];
            //var certNumber = (string)itemCasted["CertificateNumber"];
            string hash = CheckUtil.GetCheckHash(this.User.Identity.Name, certNumber);
            string participantId = (string)itemCasted["ParticipantsID"];

            return Convert.ToBoolean(Convert.ToInt32(itemCasted["IsExist"]))
                       ? string.Format(
                           "<span{2}><nobr>{0}</nobr> {1}</span>", 
                           string.Format(
                               "<a href=\"/Certificates/CommonNationalCertificates/CheckResultByPassportForOpenedFbs.aspx?number={0}&participantid={4}&check={1}&SubjectMarks={2}\">{3}</a>", 
                               certNumber, 
                               hash, 
                               this.Request.QueryString["SubjectMarks"], 
                               string.IsNullOrEmpty(certNumber) ? "Нет свидетельства" : certNumber,
                               participantId),
                           isDeny ? "<span style=\"color:Red\">(аннулировано)</span>" : string.Empty, 
                           isDeny
                               ? string.Format(
                                   " title='Свидетельство №{0} аннулировано по следующей причине: {1}'", 
                                   certNumber, 
                                   Convert.ToString(itemCasted["DenyComment"]))
                               : string.Empty)
                       : "<span style=\"color:Red\" title='Свидетельство не найдено'>Не&nbsp;найдено</span>";
        }

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.Page.IsPostBack)
            {
                var dt = new DataTable();

                var checkClient = new WSCheckClient();
                XmlElement xml = null;

                string shouldWriteLogsParam = (this.Request.Params["ShouldWriteLogs"] ?? string.Empty).ToLower();

                if (checkClient.CheckCommonNationalExamCertificateByPassportForXml(
                    this.Request.QueryString["Series"],
                    this.Request.QueryString["Number"],
                    this.Request.QueryString["SubjectMarks"],
                    HttpContext.Current.User.Identity.Name,
                    HttpContext.Current.Request.UserHostAddress,
                    shouldWriteLogsParam != "false",
                    ref xml) == (int)WebServiceReplyCodes.UserIsBanned)
                {
                    this.Response.Redirect("CheckBanPage.aspx");
                }

                if (xml != null)
                {
                    var dataSet = new DataSet();
                    dataSet.ReadXml(
                        new XmlTextReader(new StringReader(string.Format("<root>{0}</root>", xml.InnerXml))));
                    if (dataSet.Tables.Count > 0)
                    {
                        dt = dataSet.Tables[0];
                        this.dgSearch.DataSource = dt;
                        this.dgSearch.DataBind();
                    }
                }
            }
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
            // this.dgSearch.Columns[DocumentColumnNumber].Visible = this.User.IsInRole("ViewCommonNationalCertificateDocument");
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
            this.phUniqueChecks.Visible = this.dgSearch.Items.Count > 0;
        }

        #endregion

        protected string GenerateNotFoundPrintLink()
        {
            Dictionary<String, string> values = new Dictionary<string, string>();
            //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&number={1}&check=byNumber", HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["number"]);
            values.Add("SubjectMarks", this.Request.QueryString["SubjectMarks"]);
            values.Add("PassportNumber", this.Request.QueryString["Number"]);
            values.Add("Series", this.Request.QueryString["Series"]);
            this.Session["NoteInfo"] = values;
            return String.Format("PrintNotFoundNote.aspx");
            //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&Series={1}&Number={2}&check=byPassport",HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["Series"], this.Request.QueryString["Number"]);
        }
    }
}