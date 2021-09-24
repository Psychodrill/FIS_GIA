namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Fbs.Core.CNEChecks;
    using Fbs.Core.UICheckLog;
    using Fbs.Utility;

    using FbsChecksClient;

    using UserCredentials = FbsChecksClient.WSChecksReference.UserCredentials;

    /// <summary>
    /// The check result for opened fbs.
    /// </summary>
    public partial class CheckResultByPassportForOpenedFbs : Page
    {
        #region Public Properties

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

                // нужно ли проверять оценки
                bool hashIsValid = CheckUtil.VerifyCheckHash(
                    this.User.Identity.Name, this.Request.QueryString["number"], this.Request.QueryString["check"]);

                if (!hashIsValid)
                {
                    LogManager.Warning(
                        string.Format(
                            "Пользователь {0} произвел попытку получить доступ к свидетельству с номером {1} минуя проверки", 
                            this.User.Identity.Name, 
                            this.Request.QueryString["number"]));
                    this.Response.Redirect("~/InvalidAccessToCertificate.aspx");
                }

                XmlElement xml = null;

                checkClient.CheckCommonNationalExamCertificateByNumberForXml(
                    string.IsNullOrEmpty(this.Request.QueryString["number"]) ? null : this.Request.QueryString["number"], 
                    this.Request.QueryString["SubjectMarks"], 
                    this.Request.QueryString["participantid"],
                    HttpContext.Current.User.Identity.Name, 
                    HttpContext.Current.Request.UserHostAddress, 
                    false, 
                    ref xml);

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
                        //this.dgSubjects.DataBind();
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

                        // Обновим событие проверки в соответствии с найденным свидетельством
                        if (!string.IsNullOrEmpty(this.Request.QueryString["Ev"]))
                        {
                            string CNEId = ((HiddenField)e.Item.FindControl("hfCNEId")).Value;
                            CheckLogDataAccessor.UpdateCheckEvent(this.Request.QueryString["Ev"], CNEId);
                        }
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

        #endregion
    }
}