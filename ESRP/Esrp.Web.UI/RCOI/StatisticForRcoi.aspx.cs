using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Core.Systems;
using Esrp.Web.Administration.Reports;
using Esrp.Core.Reports;
using Esrp.Core.Reports.Email;
using Esrp.Core;

namespace Esrp.Web
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

				var message = new EMailMessageViewReports("Отчеты из " + GeneralSystemManager.GetSystemName(2), ReportInfos, MailTemplatePath, XSLTFilePath, Esrp.Core.DBSettings.ConnectionString, Logger, 600);

                Esrp.Utility.ResponseWriter.WriteStream("Report.xml", "text/xml", message.Attachments[0].ContentStream);
            }
            catch
            {
            }
        }
    }
}
