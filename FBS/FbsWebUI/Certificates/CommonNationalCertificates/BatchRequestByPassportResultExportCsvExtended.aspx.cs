using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using Fbs.Core.BatchCheck;
using Fbs.Utility;
using System.Web.UI.WebControls;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchRequestByPassportResultExportCsvExtended : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long batchId = 0;
            if (!Int64.TryParse(Request["id"], out batchId))
            {
                // TODO HTTP 404
                return;
            }
            string filterString;
            using (SqlConnection conn = new SqlConnection(dsResultsList.ConnectionString))
            {
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = 600,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT [Filter] FROM [dbo].[CommonNationalExamCertificateRequestBatch] WHERE Id = dbo.GetInternalId(@batchId)"
                };
                sqlCommand.Parameters.Add("@batchId", SqlDbType.BigInt);
                sqlCommand.Parameters["@batchId"].Value = batchId;
                filterString = sqlCommand.ExecuteScalar() as string;
            }

            BatchCheckFilter filter = filterString != null
                ? BatchCheckFilter.Parse(filterString)
                : BatchCheckFilter.EmptyFilter();
            IEnumerable<DataRow> dataSet = dsResultsList.Select(DataSourceSelectArguments.Empty).Cast<DataRowView>().Select(x => x.Row);
            ResponseWriter.PrepareHeaders("BatchRequestResultExtended.csv", "application/text", Encoding.GetEncoding(1251));
            StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.GetEncoding(1251));
            Fbs.Core.BatchCheck.BatchCheckResult checkResult = new Fbs.Core.BatchCheck.BatchCheckResult(filter);
            checkResult.OutputCheckedValues = false;
            checkResult.Export(dataSet, writer);
            writer.WriteLine("Комментарий: Специальным знаком «!» перед баллом выделены баллы, которые меньше минимальных значений, установленных Федеральной службой по надзору в сфере образования и науки (Рособрнадзор). С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующие годы можно ознакомиться в разделе «Документы» Подсистемы ФИС Результаты ЕГЭ Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными документами. С нормативными документами можно ознакомиться в разделе «Документы» Подсистемы ФИС Результаты ЕГЭ. В данном файле приведены сведения о свидетельствах за {0} год(ы).",
                string.IsNullOrEmpty(Request["year"]) ? "все" : Request["year"]);
            Response.Flush();
            Response.End();
        }

        protected void dsResultsList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 600;
        }
    }
}
