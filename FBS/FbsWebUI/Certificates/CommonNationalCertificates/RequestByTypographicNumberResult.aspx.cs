namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Fbs.Core.UICheckLog;
    using System.Collections.Generic;

    /// <summary>
    /// The request by typographic number result.
    /// </summary>
    public partial class RequestByTypographicNumberResult : Page
    {
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
            if (this.Page.IsPostBack)
            {
                return;
            }

            var dv = new DataView();
            var dt = new DataTable();
            dv = (DataView)this.dsSearch.Select(DataSourceSelectArguments.Empty);
            dt = dv.ToTable();

            this.phUniqueChecks.Visible = dt.Rows.Count > 0;
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

                // Если свидетельство не найдено или закрыто, скрыть контролы
                // вывода результатов и показать соответствующий контрол отображения.
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
                        }
                    }
                    else
                    {
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
                    e.Item.FindControl("pCertificate").Visible = false;
                    this.phUniqueChecks.Visible = false;
                    e.Item.FindControl("pNotExist").Visible = true;
                }
            }
        }
        protected string GenerateNotFoundPrintLink()
        {
            Dictionary<String, string> values = new Dictionary<string, string>();
            //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&number={1}&check=byNumber", HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["number"]);
            values.Add("TypographicNumber", this.Request.QueryString["TypographicNumber"]);
            values.Add("GivenName", this.Request.QueryString["PatronymicName"]);
            values.Add("FirstName", this.Request.QueryString["FirstName"]);
            values.Add("LastName", this.Request.QueryString["LastName"]);
            this.Session["NoteInfo"] = values;
            return String.Format("PrintNotFoundNote.aspx");
            //return String.Format("PrintNotFoundNote.aspx?check=byTypographicName&GivenName={0}&FirstName={1}&LastName={2}&tNumber={3}", this.Request.QueryString["PatronymicName"], this.Request.QueryString["FirstName"], this.Request.QueryString["LastName"], this.Request.QueryString["TypographicNumber"]);
        }
        #endregion
    }
}