using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fbs.Core;
using Fbs.Core.CatalogElements;
using Fbs.Core.Reports;
using Fbs.Core.Reports.Email;
using Fbs.Web.Helpers;

namespace Fbs.Web.Administration.Reports
{
    public partial class RequestReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.IsInRole("EditAdministratorAccount"))
            {
                LBCheckedCNEsTVF.Visible = false;
                LBCheckedCNEsAggregatedTVF.Visible = false;
                ChReportsWithoutPeriod.Items.RemoveAt(1);
                ChReportsWithoutPeriod.Items.RemoveAt(0);
            }
            if (!Page.IsPostBack)
            {
                DataTable Regions = RegionDataAcessor.GetAllInEtalon(null);
                DDLRegions.DataSource = Regions;
                DDLRegions.DataBind();
                DDLRegions.SelectedIndex = 0;

                UserAccount CurrentAccount = UserAccount.GetUserAccount(Account.ClientLogin);
                TBEMail.Text = CurrentAccount.Email;
            }
        }

        protected void BtSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            MemoryLogger Logger = new MemoryLogger();
            DSelectReports.Visible = false;

            try
            {
                DateTime PeriodEnd = DateTime.Now;
                int DaysToSubtract = int.Parse(RBPeriods.SelectedValue);
                DateTime PeriodBegin = PeriodEnd.AddDays(-DaysToSubtract);

                List<ReportInfo> ReportInfos = new List<ReportInfo>();
                foreach (ListItem LI in ChReports.Items)
                {
                    if (LI.Selected)
                    {
                        ReportInfos.Add(new ReportInfo(LI.Value, LI.Text.Trim() + " " + RBPeriods.SelectedItem.Text, PeriodBegin, PeriodEnd));
                    }
                }
                foreach (ListItem LI in ChReportsWithoutPeriod.Items)
                {
                    if (LI.Selected)
                    {
                        ReportInfos.Add(new ReportInfo(LI.Value, LI.Text.Trim(), PeriodBegin, PeriodEnd));
                    }
                }
                foreach (ListItem LI in ChReportsByRegion.Items)
                {
                    if (LI.Selected)
                    {
                        ReportInfos.Add(new ReportInfo(LI.Value, LI.Text.Trim(), PeriodBegin, PeriodEnd, DDLRegions.SelectedValue));
                    }
                }


                string XSLTFilePath = "~\\Administration\\Reports\\Resources\\CreateExcel.xsl";
                XSLTFilePath = Server.MapPath(XSLTFilePath);

                string MailTemplatePath = "~\\Administration\\Reports\\Resources\\EmailTemplate_ReportViews.xml";
                MailTemplatePath = Server.MapPath(MailTemplatePath);

                Logger.WriteMessage(String.Format("Начало формирования отчетов с {0} по {1}", PeriodBegin.ToShortDateString(), PeriodEnd.ToShortDateString()));
                var message = new EMailMessageViewReports("Отчеты из Подсистемы ФИС Результаты ЕГЭ", ReportInfos, MailTemplatePath, XSLTFilePath, Fbs.Core.DBSettings.ConnectionString, Logger, 600);
                Logger.WriteMessage("Начало отправки.");

                message.To.Add(TBEMail.Text);

                Fbs.Utility.EmailSender.SendMessage(message);

                Logger.WriteMessage("Сообщение успешно отправилось.");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            LResult.Text = Logger.GetLog();
        }

        protected void VReqReports_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ((ChReports.SelectedItem != null) || (ChReportsWithoutPeriod.SelectedItem != null) || (ChReportsByRegion.SelectedItem != null));
        }

        protected void LBOrgsInfo_Click(object sender, EventArgs e)
        {
            MemoryLogger Logger = new MemoryLogger();
            try
            {
                DateTime PeriodEnd = DateTime.Now;
                int DaysToSubtract = int.Parse(RBPeriods.SelectedValue);
                DateTime PeriodBegin = PeriodEnd.AddDays(-DaysToSubtract);

                List<ReportInfo> ReportInfos = new List<ReportInfo>();
                ReportInfos.Add(new ReportInfo(((LinkButton)sender).CommandArgument, ((LinkButton)sender).Text, PeriodBegin, PeriodEnd));

                string XSLTFilePath = "~\\Administration\\Reports\\Resources\\CreateExcel.xsl";
                XSLTFilePath = Server.MapPath(XSLTFilePath);

                string MailTemplatePath = "~\\Administration\\Reports\\Resources\\EmailTemplate_ReportViews.xml";
                MailTemplatePath = Server.MapPath(MailTemplatePath);

                var message = new EMailMessageViewReports("Отчеты из АИС ФБС", ReportInfos, MailTemplatePath, XSLTFilePath, Fbs.Core.DBSettings.ConnectionString, Logger, 600);

                Fbs.Utility.ResponseWriter.WriteStream("Report.xml", "text/xml", message.Attachments[0].ContentStream);
            }
            catch
            {
            }
        }
    }
}
