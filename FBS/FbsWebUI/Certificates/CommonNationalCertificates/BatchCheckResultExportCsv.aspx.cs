namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Fbs.Core.BatchCheck;
    using Fbs.Utility;

    /// <summary>
    /// The batch check result export csv.
    /// </summary>
    public partial class BatchCheckResultExportCsv : Page
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
            long batchId = 0;
            if (!long.TryParse(this.Request["id"], out batchId))
            {
                // TODO HTTP 404
                return;
            }

            string filterString;
            using (var conn = new SqlConnection(this.dsResultsList.ConnectionString))
            {
                conn.Open();
                var sqlCommand = new SqlCommand
                    {
                        Connection = conn, 
                        CommandTimeout = 600, 
                        CommandType = CommandType.Text, 
                        CommandText =
                            "SELECT [Filter] FROM [dbo].[CommonNationalExamCertificateCheckBatch] WHERE Id = dbo.GetInternalId(@batchId)"
                    };
                sqlCommand.Parameters.Add("@batchId", SqlDbType.BigInt);
                sqlCommand.Parameters["@batchId"].Value = batchId;
                filterString = sqlCommand.ExecuteScalar() as string;
            }

            BatchCheckFilter filter = filterString != null
                                          ? BatchCheckFilter.Parse(filterString)
                                          : BatchCheckFilter.EmptyFilter();
            IEnumerable<DataRow> dataSet =
                this.dsResultsList.Select(DataSourceSelectArguments.Empty).Cast<DataRowView>().Select(x => x.Row);
            ResponseWriter.PrepareHeaders("BatchCheckResult.csv", "application/text", Encoding.GetEncoding(1251));
            var writer = new StreamWriter(this.Response.OutputStream, Encoding.GetEncoding(1251));
            var checkResult = new Core.BatchCheck.BatchCheckResult(filter);
            checkResult.OutputCheckedValues = true;
            checkResult.Export(dataSet, writer);
            writer.WriteLine(
                "Комментарий: Специальным знаком «!» перед баллом выделены баллы, которые меньше минимальных значений, установленных Федеральной службой по надзору в сфере образования и науки (Рособрнадзор). С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующие годы можно ознакомиться в разделе «Документы» Подсистема ФИС Результаты ЕГЭ");
            writer.WriteLine(
                "Комментарий: Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными документами. С нормативными документами можно ознакомиться в разделе «Документы» Подсистемы ФИС Результаты ЕГЭ");
            this.Response.Flush();
            this.Response.End();
        }

        /// <summary>
        /// The ds results list_ selecting.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dsResultsList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 1200;
        }

        #endregion
    }
}