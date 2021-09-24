namespace Esrp.Web.Administration.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.CatalogElements;
    using Esrp.Core.Loggers;
    using Esrp.Core.Reports;
    using Esrp.Core.Reports.Email;
    using Esrp.Core.Systems;
    using Esrp.Utility;

    /// <summary>
    /// The request reports.
    /// </summary>
    public partial class RequestReports : Page
    {
        #region Methods

        /// <summary>
        /// The bt send_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void BtSend_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            var Logger = new MemoryLogger();
            this.DSelectReports.Visible = false;

            try
            {
                DateTime PeriodEnd = DateTime.Now;
                int DaysToSubtract = int.Parse(this.RBPeriods.SelectedValue);
                DateTime PeriodBegin = PeriodEnd.AddDays(-DaysToSubtract);

                var ReportInfos = new List<ReportInfo>();
                foreach (ListItem LI in this.ChReports.Items)
                {
                    if (LI.Selected)
                    {
                        ReportInfos.Add(
                            new ReportInfo(
                                LI.Value, 
                                LI.Text.Trim() + " " + this.RBPeriods.SelectedItem.Text, 
                                PeriodBegin, 
                                PeriodEnd));
                    }
                }

                foreach (ListItem LI in this.ChReportsWithoutPeriod.Items)
                {
                    if (LI.Selected)
                    {
                        ReportInfos.Add(new ReportInfo(LI.Value, LI.Text.Trim(), PeriodBegin, PeriodEnd));
                    }
                }

                foreach (ListItem LI in this.ChReportsByRegion.Items)
                {
                    if (LI.Selected)
                    {
                        ReportInfos.Add(
                            new ReportInfo(
                                LI.Value, LI.Text.Trim(), PeriodBegin, PeriodEnd, this.DDLRegions.SelectedValue));
                    }
                }

                string XSLTFilePath = "~\\Administration\\Reports\\Resources\\CreateExcel.xsl";
                XSLTFilePath = this.Server.MapPath(XSLTFilePath);

                string MailTemplatePath = "~\\Administration\\Reports\\Resources\\EmailTemplate_ReportViews.xml";
                MailTemplatePath = this.Server.MapPath(MailTemplatePath);

                Logger.WriteMessage(
                    string.Format(
                        "Начало формирования отчетов с {0} по {1}", 
                        PeriodBegin.ToShortDateString(), 
                        PeriodEnd.ToShortDateString()));
                var message = new EMailMessageViewReports(
                    "Отчеты из " + GeneralSystemManager.GetSystemName(2), 
                    ReportInfos, 
                    MailTemplatePath, 
                    XSLTFilePath, 
                    DBSettings.ConnectionString, 
                    Logger, 
                    600);
                Logger.WriteMessage("Начало отправки.");

                message.To.Add(this.TBEMail.Text);

                EmailSender.SendMessage(message);

                Logger.WriteMessage("Сообщение успешно отправилось.");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }

            this.LResult.Text = Logger.GetLog();
        }

        /// <summary>
        /// The lb orgs info_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void LBOrgsInfo_Click(object sender, EventArgs e)
        {
            var Logger = new MemoryLogger();
            try
            {
                DateTime PeriodEnd = DateTime.Now;
                int DaysToSubtract = int.Parse(this.RBPeriods.SelectedValue);
                DateTime PeriodBegin = PeriodEnd.AddDays(-DaysToSubtract);

                var ReportInfos = new List<ReportInfo>();
                ReportInfos.Add(
                    new ReportInfo(
                        ((LinkButton)sender).CommandArgument, ((LinkButton)sender).Text, PeriodBegin, PeriodEnd));

                string XSLTFilePath = "~\\Administration\\Reports\\Resources\\CreateExcel.xsl";
                XSLTFilePath = this.Server.MapPath(XSLTFilePath);

                string MailTemplatePath = "~\\Administration\\Reports\\Resources\\EmailTemplate_ReportViews.xml";
                MailTemplatePath = this.Server.MapPath(MailTemplatePath);

                var message = new EMailMessageViewReports(
                    "Отчеты из АИС " + GeneralSystemManager.GetSystemName(2), 
                    ReportInfos, 
                    MailTemplatePath, 
                    XSLTFilePath, 
                    DBSettings.ConnectionString, 
                    Logger, 
                    600);

                ResponseWriter.WriteStream("Report.xml", "text/xml", message.Attachments[0].ContentStream);
            }
            catch
            {
            }
        }

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
            if (!this.User.IsInRole("EditAdministratorAccount"))
            {
                this.ChReportsWithoutPeriod.Items.RemoveAt(1);
                this.ChReportsWithoutPeriod.Items.RemoveAt(0);
            }

            if (!this.Page.IsPostBack)
            {
                DataTable Regions = RegionDataAcessor.GetAllInEtalon(null);
                this.DDLRegions.DataSource = Regions;
                this.DDLRegions.DataBind();
                this.DDLRegions.SelectedIndex = 0;

                UserAccount CurrentAccount = UserAccount.GetUserAccount(Account.ClientLogin);
                this.TBEMail.Text = CurrentAccount.Email;
            }
        }

        /// <summary>
        /// The v req reports_ server validate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected void VReqReports_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (this.ChReports.SelectedItem != null) || (this.ChReportsWithoutPeriod.SelectedItem != null)
                            || (this.ChReportsByRegion.SelectedItem != null);
        }

        #endregion
    }

    /// <summary>
    /// The memory logger.
    /// </summary>
    internal class MemoryLogger : ILogger
    {
        #region Constants and Fields

        private readonly StringBuilder Log_ = new StringBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get log.
        /// </summary>
        /// <returns>
        /// The get log.
        /// </returns>
        public string GetLog()
        {
            return this.Log_.ToString();
        }

        /// <summary>
        /// The write error.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public void WriteError(Exception ex)
        {
            this.Log_.Append("Произошла ошибка:" + ex.Message + "<br />");
            this.Log_.Append("Тип:" + ex.GetType() + "<br />");
            this.Log_.Append("Источник:" + ex.Source + "<br />");
            this.Log_.Append("Стек:" + ex.StackTrace + "<br />");

            if (ex.InnerException != null)
            {
                this.Log_.Append("Произошла ошибка (внутренняя ошибка):" + ex.InnerException.Message + "<br />");
                this.Log_.Append("Тип (внутренняя ошибка):" + ex.InnerException.GetType() + "<br />");
                this.Log_.Append("Источник (внутренняя ошибка):" + ex.InnerException.Source + "<br />");
            }
        }

        /// <summary>
        /// The write message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void WriteMessage(string message)
        {
            this.Log_.Append(message + "<br />");
        }

        #endregion
    }
}