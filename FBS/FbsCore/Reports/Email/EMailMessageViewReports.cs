using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;
using Fbs.Core.Reports.Excel;
using Fbs.Core.Loggers;

namespace Fbs.Core.Reports.Email
{
    public class EMailMessageViewReports : EMailMessageFromTemplate
    {
        private  ILogger Logger;
        private string ConnectionString;
        private List<ReportInfo> Reports;
        private string ReportName;
        private string XSLTPath;
        private int CommandTimeout;

        public EMailMessageViewReports(string reportName, List<ReportInfo> reports, string mailTemplatePath, string xsltPath, string connectionString, ILogger logger, int commandTimeout)
            : base(mailTemplatePath)
        {
            Logger = logger;
            ConnectionString = connectionString;
            Reports = reports;
            ReportName = reportName;
            XSLTPath = xsltPath;
            CommandTimeout = commandTimeout;
            AddParam("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm"));
            AddParam("##ReportList##", FormHtmlUnorderedList(Reports ));
            ApplyParameters();

            Stream excelStream = GetExcelStream( );
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
        private static string FormHtmlUnorderedList(List<ReportInfo> reports)
        {
            StringBuilder sb = new StringBuilder("<ul>");
            foreach (ReportInfo RInfo in reports)
            {
                sb.AppendFormat("<li>{0}</li>", RInfo.ReportName);
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        public Stream GetExcelStream()
        {
            List<DataTableWithTag> Tables = new List<DataTableWithTag>();
            foreach (ReportInfo RInfo in Reports)
            {
                DataTableWithTag DT = GetReportData(RInfo);
                if (DT  != null)
                {
                    DT.Tag = RInfo;
                    Tables.Add(DT);
                }
            }

            return ExcelCreator.GetXmlExcelStream(ReportName, Tables, XSLTPath);
        }

        public DataTableWithTag GetReportData(ReportInfo rInfo)
        {
            try
            {
                DataTableWithTag table = new DataTableWithTag();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = CommandTimeout;
                    Logger.WriteMessage(String.Format("Получаем данные из функции {0}.", rInfo.ExtractorMethodName));
                    if (!String.IsNullOrEmpty(rInfo.AdditionalArg))
                    {
                        cmd.CommandText = string.Format("SELECT * FROM {0}(@periodBegin,@periodEnd,@arg)", rInfo.ExtractorMethodName);
                        cmd.Parameters.AddWithValue("periodBegin", rInfo.PeriodBegin);
                        cmd.Parameters.AddWithValue("periodEnd", rInfo.PeriodEnd);
                        cmd.Parameters.AddWithValue("arg", rInfo.AdditionalArg);
                    }
                    else
                    {
                        cmd.CommandText = string.Format("SELECT * FROM {0}(@periodBegin,@periodEnd)", rInfo.ExtractorMethodName);
                        cmd.Parameters.AddWithValue("periodBegin", rInfo.PeriodBegin);
                        cmd.Parameters.AddWithValue("periodEnd", rInfo.PeriodEnd);
                    }
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                    Logger.WriteMessage("Данные получены.");
                }
                return table;
            }
            catch(Exception ex)
            {
                Logger.WriteError(ex);
                return null;
            }
        }

    }
}
