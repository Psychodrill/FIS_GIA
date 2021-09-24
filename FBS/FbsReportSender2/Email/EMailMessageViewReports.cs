using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;
using FbsReportSender.Excel;
using FbsReportSender.Properties;

namespace FbsReportSender.Email
{
    public class EMailMessageViewReports : EMailMessageFromTemplate
    {
        public EmailScheduleTypeEnum Schedule { get; set; }
        private string[] reportViews;
        private string[] ReportViews
        {
            get
            {
                if(reportViews == null)
                {
                    switch (Schedule)
                    {
                        case EmailScheduleTypeEnum.Weekly:
                            reportViews = RetrieveTrimmedSettings(Settings.Default.ReportViewListWeekly);
                            break;

                        case EmailScheduleTypeEnum.Daily:
                        default:
                            reportViews = RetrieveTrimmedSettings(Settings.Default.ReportViewListDaily);
                            break;
                    }
                }
                return reportViews;
            }
        }

        private static string[] RetrieveTrimmedSettings(string rawSettingsString)
        {
            string[] rawSettings = rawSettingsString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < rawSettings.Length; i++)
                rawSettings[i] = rawSettings[i].Trim();
            return rawSettings;
        }

        public EMailMessageViewReports(EmailScheduleTypeEnum schedule)
            : base(EmailTemplateTypeEnum.ReportViews)
        {
            Schedule = schedule;
            AddParam("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm"));
            AddParam("##ReportList##", FormHtmlUnorderedList(ReportViews));
            ApplyParameters();
            string reportName = Settings.Default.ReportName;
            Stream excelStream = GetExcelStream(reportName);
            string attachmentName = string.Format("{0} от {1}.xml", reportName,
                                                  DateTime.Now.ToString("yyyy.MM.dd HH:mm"));
            Attachments.Add(new Attachment(excelStream, attachmentName, "application/x-excel"));
        }

        /// <summary>
        /// Формирование html списка по входному массиву строк.
        /// {"1","2"} --> <ul><li>1</li><li>2</li></ul>
        /// </summary>
        /// <param name="ul"></param>
        /// <returns></returns>
        private static string FormHtmlUnorderedList(string[] ul)
        {
            StringBuilder sb = new StringBuilder("<ul>");
            foreach (var li in ul)
            {
                sb.AppendFormat("<li>{0}</li>", li);
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        public Stream GetExcelStream(string name)
        {
            List<DataTable> tables=new List<DataTable>();
            foreach (string viewName in ReportViews)
            {
                if (viewName.Trim().Length>0)
                {
                    DataTable dataTable = GetViewData(viewName);
                    if (dataTable!=null)
                        tables.Add(dataTable);
                }
            }

            return ExcelCreator.GetXmlExcelStream(name, tables);
        }

        public static DataTable GetViewData(string viewName)
        {
            try
            {
                DataTable table;

                string connectionString = Settings.Default.DBConnection;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = connection.ConnectionTimeout;
                    Program.LogMessage(string.Format("Получаем данные из отчета {0}.", viewName));
                    cmd.CommandText = string.Format("select * from {0}",viewName);
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        table = MainFunctions.CreateTableFromSqlDataReader(reader);
                        table.TableName = viewName;
                    }
                    Program.LogMessage("Данные получены.");
                }
                return table;
            }
            catch(Exception exp)
            {
                Program.LogError(exp);
                return null;
            }
        }

    }
}
