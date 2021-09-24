using System;
using System.IO;
using System.Collections.Generic;
using Fbs.Core.Reports;
using Fbs.Core.Reports.Email;
using FbsReportSender.Properties;


namespace FbsReportSender
{
    class Program
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="args">Кол-во дней по которым строится отчет (считая от текущей даты)</param>
        static void Main(string[] args)
        {
            Fbs.Core.Loggers.ILogger Logger = new Fbs.Core.Loggers. ConsoleLogger();
            try
            {
                DateTime PeriodEnd = DateTime.Now;
                int DaysToSubtract;
                if ((args.Length == 0) || (!int.TryParse(args[0], out DaysToSubtract)))
                {
                    Logger.WriteMessage("Неверный формат входного параметра");
                    DaysToSubtract = 1;
                }
                DateTime PeriodBegin = PeriodEnd.AddDays(-DaysToSubtract);
                string[] ReportCodes = RetrieveTrimmedSettings(Settings.Default.ReportViewList);
                SetCorrectNames(ReportCodes, DaysToSubtract);
                List<ReportInfo> ReportInfos = GetReportInfos(ReportCodes, PeriodBegin, PeriodEnd);

                string XSLTFilePath = "Resources/CreateExcel.xsl";
                XSLTFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, XSLTFilePath);

                string MailTemplatePath = "Resources/EmailTemplate_ReportViews.xml";
                MailTemplatePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, MailTemplatePath);

                string ConnectionString = Settings.Default.DBConnection;



                Logger.WriteMessage(String.Format("Начало формирования отчетов с {0} по {1}", PeriodBegin.ToShortDateString(), PeriodEnd.ToShortDateString()));
                var message = new EMailMessageViewReports(Settings.Default.ReportName, ReportInfos, MailTemplatePath, XSLTFilePath, ConnectionString, Logger, 1200);
                Logger.WriteMessage("Начало отправки.");

                EMailSender.Send(message);
                Logger.WriteMessage("Сообщение успешно отправилось.");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }

            //Чтобы успеть глянуть сообщение об ошибке
            System.Threading.Thread.Sleep(25000);
        }

        internal static List<ReportInfo> GetReportInfos(string[] reports,DateTime periodBegin,DateTime periodEnd)
        {
            List<ReportInfo> Result = new List<ReportInfo>();
            foreach (string report in reports)
            {
                if (!report.Contains("|"))
                {
                    continue;
                }
                string ReportMethod = report.Split('|')[0].Replace(" ", "");
                string ReportName = report.Split('|')[1];
                Result.Add(new ReportInfo(ReportMethod, ReportName, periodBegin, periodEnd));
            }

            return Result;
        }

        internal static void SetCorrectNames(string[] reportCodes, int PeriodLengthInDays)
        {
            string CorrectPeriod = "";
            if (PeriodLengthInDays == 1)
            {
                CorrectPeriod = "за 24 часа";
            }
            if (PeriodLengthInDays > 1)
            {
                CorrectPeriod = "за " + PeriodLengthInDays.ToString() + " дней";
            }
            for (int i = 0; i < reportCodes.Length; i++)
            {
                reportCodes[i] = reportCodes[i].Replace("{PERIOD}", CorrectPeriod);
            }
        }

        internal static string[] RetrieveTrimmedSettings(string rawSettingsString)
        {
            string[] rawSettings = rawSettingsString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < rawSettings.Length; i++)
                rawSettings[i] = rawSettings[i].Trim();
            return rawSettings;
        }
    }
}
