namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Fbs.Core.CNEChecks;
    using Fbs.Core.UICheckLog;
    using Fbs.Core.WebServiceCheck;

    using FbsChecksClient;

    /// <summary>
    /// The check result for opened fbs.
    /// </summary>
    public partial class CheckResultForOpenedFbs : BasePage
    {
        #region Public Properties

        /// <summary>
        /// Настройка web.config 
        /// </summary>
        public bool EnableOpenedFbs
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]);
            }
        }

        /// <summary>
        /// Gets or sets NoteId.
        /// </summary>
        public Guid NoteId { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The form link to new certificate.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <param name="newCertificate">
        /// The new certificate.
        /// </param>
        /// <returns>
        /// The form link to new certificate.
        /// </returns>
        public string FormLinkToNewCertificate(string comment, string newCertificate)
        {
            if (string.IsNullOrEmpty(newCertificate))
            {
                return comment;
            }

            string hash = CheckUtil.GetCheckHash(this.User.Identity.Name, newCertificate);

            return
                string.Format(
                    "<a href=\"/Certificates/CommonNationalCertificates/CheckResultForOpenedFbs.aspx?number={1}&check={2}\">{0}</a>",
                    comment,
                    newCertificate,
                    hash);
        }

        /// <summary>
        /// The highlight value.
        /// </summary>
        /// <param name="valueObj">
        /// The value obj.
        /// </param>
        /// <returns>
        /// The highlight value.
        /// </returns>
        public string HighlightValue(object valueObj)
        {
            string value = Convert.ToString(valueObj);
            if (value.StartsWith("!") || value.StartsWith("Истек"))
            {
                return string.Format("<font color=\"red\">{0}</font>", value);
            }

            return value;
        }

        /// <summary>
        /// The highlight value.
        /// </summary>
        /// <param name="valueObj">
        /// The value obj.
        /// </param>
        /// <param name="isCorrectObj">
        /// The is correct obj.
        /// </param>
        /// <returns>
        /// The highlight value.
        /// </returns>
        public string HighlightValue(object valueObj, object isCorrectObj)
        {
            string value = Convert.ToString(valueObj);
            if (value == "ИСТОРИЯ")
            {
                this.SRusHist.Visible = true;
            }

            if (!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(Convert.ToInt32(isCorrectObj)))
            {
                return value;
            }

            return string.Format("<font color=\"red\">{0}</font>", value);
        }

        /// <summary>
        /// The highlight values.
        /// </summary>
        /// <param name="valueObj">
        /// The value obj.
        /// </param>
        /// <param name="checkValueObj">
        /// The check value obj.
        /// </param>
        /// <param name="isCorrectObj">
        /// The is correct obj.
        /// </param>
        /// <returns>
        /// The highlight values.
        /// </returns>
        public string HighlightValues(object valueObj, object checkValueObj, object isCorrectObj)
        {
            string value = Convert.ToString(valueObj);
            string checkValue = Convert.ToString(checkValueObj);
            if ((!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(Convert.ToInt32(isCorrectObj)))
                || string.IsNullOrEmpty(checkValue))
            {
                return value;
            }

            if (Convert.IsDBNull(valueObj) && Convert.IsDBNull(checkValueObj))
            {
                return string.Empty;
            }

            checkValue = string.IsNullOrEmpty(checkValue) ? "не&nbsp;задано" : checkValue;
            value = string.IsNullOrEmpty(value) ? "не&nbsp;найдено" : value;

            return
                string.Format(
                    "<span title=\"Ошибка: заявленое {0} (в базе {1})\"><span style=\"color:Red\">{0}</span> ({1})</span>",
                    checkValue,
                    value);
        }

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
            if (!this.Page.IsPostBack)
            {
                var dt = new DataTable();

                var checkClient = new WSCheckClient();

                // возьмем оценки из результата пакетной проверки, если св-во уже проверялось (batchId)
                // либо возьммем их из запроса (SubjectMarks)
                // либо просто загрузим сертификат из открытой ФБС (параметр check), если нам нужно перейти например 
                // на свидетельство, в которое было аннулировано текущее (ссылка про анулированое свидетльство)
                if ((string.IsNullOrEmpty(this.Request.QueryString["check"]) && this.GetParamLong("batchId") <= 0
                     && string.IsNullOrEmpty(this.Request.QueryString["SubjectMarks"]))
                    || (string.IsNullOrEmpty(this.Request.QueryString["number"]) && string.IsNullOrEmpty(this.Request.QueryString["participantid"])))
                {
                    throw new Exception(string.Format("неверный запрос {0}", this.Request.Url.AbsoluteUri));
                }

                string marks = string.Empty;
                bool shouldCheck = true;
                if (this.GetParamLong("batchId") > 0)
                {
                    marks = CheckDataAccessor.GetMarksFromBatchCheck(
                        this.Request.QueryString["number"], this.GetParamLong("batchId"), this.User.Identity.Name);
                }
                else if (!string.IsNullOrEmpty(this.Request.QueryString["SubjectMarks"]))
                {
                    marks = this.Request.QueryString["SubjectMarks"];
                }

                if (string.IsNullOrEmpty(this.Request.QueryString["check"])
                    && CheckUtil.VerifyCheckHash(this.User.Identity.Name, this.Request.QueryString["number"], this.Request.QueryString["check"]))
                {
                    shouldCheck = false;
                }

                XmlElement xml = null;
                if (checkClient.CheckCommonNationalExamCertificateByNumberForXml(
                    string.IsNullOrEmpty(this.Request.QueryString["number"]) ? "" : this.Request.QueryString["number"],
                    marks,
                    this.Request.QueryString["participantid"],
                    HttpContext.Current.User.Identity.Name,
                    HttpContext.Current.Request.UserHostAddress,
                    shouldCheck,
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
                        this.rpSearch.DataSource = dt;
                        //this.dgSubjects.DataSource = dt;
                        this.rpSearch.DataBind();
                       // this.dgSubjects.DataBind();
                        this.phUniqueChecks.Visible = dt.Rows.Count > 0;
                        if (dt.Rows.Count > 0)
                        {
                            this.historyCertificate.PassportSeria = dt.Rows[0]["PassportSeria"].ToString();
                            this.historyCertificate.PassportNumber = dt.Rows[0]["PassportNumber"].ToString();
                            this.historyCertificate.CurrentCertificateNumber = dt.Rows[0]["Number"].ToString();

                            var certificates = dt.ToCertificatesCollection(false);
                            if (certificates.Count > 0)
                                this.SaveCertificateInfo(certificates[0]);
                        }
                        else
                        {
                            this.historyCertificate.Visible = false;
                        }
                    }
                    else
                    {
                        this.historyCertificate.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// The rp search_ item data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void rpSearch_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Показываю только один элемент в репитере, остальные скрываю.
            e.Item.Visible = this.rpSearch.Items.Count < 1;
            if (e.Item.Visible)
            {
                bool bIsExist = Convert.ToBoolean(Convert.ToInt32(((HiddenField)e.Item.FindControl("hfIsExist")).Value));
                bool bIsDeny = Convert.ToBoolean(Convert.ToInt32(((HiddenField)e.Item.FindControl("hfIsDeny")).Value));

                e.Item.FindControl("pDeny").Visible = bIsDeny;

                if (bIsExist)
                {
                    this.pActions.Visible = true;
                }
                else
                {
                    this.phUniqueChecks.Visible = false;
                }
            }
        }

        private void SaveCertificateInfo(CNEInfo item)
        {
            Guid key = Guid.NewGuid();
            this.NoteId = key;

            var val = new KeyValuePair<Guid, CNEInfo>(this.NoteId, item);
            this.Session["CertificateInfo"] = val;
        }

        protected string GenerateNotFoundPrintLink()
        {
            Dictionary<String, string> values = new Dictionary<string, string>();
            //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&number={1}&check=byNumber", HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["number"]);
            values.Add("SubjectMarks", this.Request.QueryString["SubjectMarks"]);
            values.Add("CertNumber", this.Request.QueryString["number"]);
            this.Session["NoteInfo"] = values;
            return String.Format("PrintNotFoundNote.aspx");
        }
        #endregion
    }
}