namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Fbs.Core.CNEChecks;
    using Fbs.Core.UICheckLog;
    using Fbs.Core.Organizations;

    /// <summary>
    /// The check result.
    /// </summary>
    public partial class CheckResult : Page
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
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <returns>
        /// The form link to new certificate.
        /// </returns>
        public string FormLinkToNewCertificate(string comment, string newCertificate, string lastName)
        {
            if (string.IsNullOrEmpty(newCertificate))
            {
                return comment;
            }

            return
                string.Format(
                    "<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={1}&LastName={2}\">{0}</a>", 
                    comment, 
                    newCertificate, 
                    lastName);
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

            if (!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj))
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
            if ((!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj)) || string.IsNullOrEmpty(checkValue))
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
                var dv = new DataView();
                var dt = new DataTable();
                dv = (DataView)this.dsSearch.Select(DataSourceSelectArguments.Empty);
                dt = dv.ToTable();

                this.phUniqueChecks.Visible = dt.Rows.Count > 0;
                if (dt.Rows.Count > 0)
                {
                    this.historyCertificate.LastName = dt.Rows[0]["LastName"].ToString();
                    this.historyCertificate.FirstName = dt.Rows[0]["FirstName"].ToString();
                    this.historyCertificate.PatronymicName = dt.Rows[0]["PatronymicName"].ToString();
                    this.historyCertificate.PassportSeria = dt.Rows[0]["PassportSeria"].ToString();
                    this.historyCertificate.PassportNumber = dt.Rows[0]["PassportNumber"].ToString();
                    this.historyCertificate.CurrentCertificateNumber = dt.Rows[0]["Number"].ToString();

                    var certificates = dt.ToCertificatesCollection(true);
                    if (certificates.Count > 0)
                        this.SaveCertificateInfo(certificates[0]);
                }
                else
                {
                    this.historyCertificate.Visible = false;
                }
            }
        }

        /// <summary>
        /// The page_ pre render.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The ds search_ selecting.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dsSearch_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 90;
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
                bool bIsExist = Convert.ToBoolean(((HiddenField)e.Item.FindControl("hfIsExist")).Value);
                bool bIsDeny = Convert.ToBoolean(((HiddenField)e.Item.FindControl("hfIsDeny")).Value);
                bool bLastNameIsCorrect =
                    Convert.ToBoolean(((HiddenField)e.Item.FindControl("hfLastNameIsCorrect")).Value);
                bool bFirstNameIsCorrect =
                    Convert.ToBoolean(((HiddenField)e.Item.FindControl("hfFirstNameIsCorrect")).Value);
                bool bPatronymicNameIsCorrect =
                    Convert.ToBoolean(((HiddenField)e.Item.FindControl("hfPatronymicNameIsCorrect")).Value);

                e.Item.FindControl("pDeny").Visible = bIsDeny;

                if (bIsExist)
                {
                    if (bLastNameIsCorrect && bFirstNameIsCorrect && bPatronymicNameIsCorrect)
                    {
                        this.pActions.Visible = true;
                        this.hlCompetitionCertificates.NavigateUrl =
                            string.Format(
                                this.hlCompetitionCertificates.NavigateUrl,
                                HttpUtility.UrlEncode(((HiddenField)e.Item.FindControl("hfLastName")).Value),
                                HttpUtility.UrlEncode(((HiddenField)e.Item.FindControl("hfFirstName")).Value),
                                HttpUtility.UrlEncode(((HiddenField)e.Item.FindControl("hfPatronymicName")).Value),
                                ((HiddenField)e.Item.FindControl("hfRegion")).Value);

                        // Обновим событие проверки в соответствии с найденным свидетельством
                        if (!string.IsNullOrEmpty(this.Request.QueryString["Ev"]))
                        {
                            var CNEId = ((HiddenField)e.Item.FindControl("hfCNEId")).Value;
                            CheckLogDataAccessor.UpdateCheckEvent(this.Request.QueryString["Ev"], CNEId);
                            CheckLogEntry entry = CheckLogDataAccessor.Get(Int32.Parse(this.Request.QueryString["Ev"]));
                            //в случае перехода н асправку дабы избежать чита с querystring сохряним в сессию ид организации
                            Organization orgByLogin = OrganizationDataAccessor.GetByLogin(entry.Login);
                            if (orgByLogin != null)
                            {
                                this.Session["OrgId"] = orgByLogin.Id;
                            }
                        }
                    }
                    else
                    {
                        this.historyCertificate.Visible = false;
                        e.Item.FindControl("pCertificate").Visible = false;
                        //this.dgSubjects.Visible = false;
                        this.pNoticeMarks.Visible = false;
                        this.nrtSubjects.Visible = false;

                        e.Item.FindControl("pDeny").Visible = false;
                        e.Item.FindControl("pNotExist").Visible = true;
                        this.phUniqueChecks.Visible = false;
                    }
                }
                else
                {
                    this.phUniqueChecks.Visible = false;
                    e.Item.FindControl("pNotExist").Visible = true;
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
            values.Add("GivenName",this.Request.QueryString["PatronymicName"]);
            values.Add("FirstName",this.Request.QueryString["FirstName"]);
            values.Add("LastName", this.Request.QueryString["LastName"]);
            this.Session["NoteInfo"] = values;
            return String.Format("PrintNotFoundNote.aspx");
            //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&number={1}&check=byNumberName&GivenName={2}&FirstName={3}&LastName={4}", HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["number"], this.Request.QueryString["PatronymicName"], this.Request.QueryString["FirstName"], this.Request.QueryString["LastName"]);
        }
        #endregion
    }
}