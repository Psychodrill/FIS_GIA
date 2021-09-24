using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Fbs.Core;
using Fbs.Core.Reports;
using Fbs.Core.Reports.Email;
using Fbs.Web.Helpers;

namespace Fbs.Web
{
    public partial class StatisticForRcoi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LBReportOrgsInfoByRegionTVF_Click(object sender, EventArgs e)
        {
            MemoryLogger Logger = new MemoryLogger();
            try
            {
                DateTime PeriodEnd = DateTime.Now;
                int DaysToSubtract = 24;//int.Parse(RBPeriods.SelectedValue);
                DateTime PeriodBegin = PeriodEnd.AddDays(-DaysToSubtract);

                UserAccount CurrentAccount = UserAccount.GetUserAccount(Account.ClientLogin);

                List<ReportInfo> ReportInfos = new List<ReportInfo>();
                ReportInfos.Add(new ReportInfo(((LinkButton)sender).CommandArgument, ((LinkButton)sender).Text, PeriodBegin, PeriodEnd, CurrentAccount.OrganizationRegionId.ToString()));

                string XSLTFilePath = "~\\Administration\\Reports\\Resources\\CreateExcel.xsl";
                XSLTFilePath = Server.MapPath(XSLTFilePath);

                string MailTemplatePath = "~\\Administration\\Reports\\Resources\\EmailTemplate_ReportViews.xml";
                MailTemplatePath = Server.MapPath(MailTemplatePath);

                var message = new EMailMessageViewReports("Отчеты из Подсистемы ФИС Результаты ЕГЭ", ReportInfos, MailTemplatePath, XSLTFilePath, Fbs.Core.DBSettings.ConnectionString, Logger, 600);

                Fbs.Utility.ResponseWriter.WriteStream("Report.xml", "text/xml", message.Attachments[0].ContentStream);
            }
            catch
            {
            }
        }
    }
}
