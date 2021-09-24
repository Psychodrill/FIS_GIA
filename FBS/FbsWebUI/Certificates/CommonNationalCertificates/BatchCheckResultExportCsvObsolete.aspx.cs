using Fbs.Utility;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web.UI;

    using FbsServices;

    /// <summary>
    /// The batch check result export csv.
    /// </summary>
    public partial class BatchCheckResultExportCsvObsolete : Page
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
            long checkId;
            if (!long.TryParse(this.Request["id"], out checkId) || checkId <= 0)
            {
                Response.ClearContent();
                Response.SuppressContent = true;
                Response.StatusCode = 404;
                Response.End();
                return;
            }

            string login = User.IsInRole("EditAdministratorAccount") ? null : User.Identity.Name;

            var svc = new CertificateBatchCheckHistoryService(login, checkId);

            int participantsCount = svc.GetRecordsCount(); // число участников, не записей, т.е. на странице выводится динамическое число записей
            const int chunkSize = 100; // выгрузить данные о 100 участниках из totalRecords за одну итерацию

            ResponseWriter.PrepareHeaders("BatchCheckResult.csv", "application/text", Encoding.GetEncoding(1251));
            Response.BufferOutput = false;
            
            using (var writer = new StreamWriter(this.Response.OutputStream, Encoding.GetEncoding(1251)))
            {
                var exporter = new BatchCheckResultObsoleteCsvExporter();

                for (int startParticipantIndex = 1; startParticipantIndex <= participantsCount; startParticipantIndex += chunkSize)
                {
                    using (DataTable chunk = svc.GetRecordsPageObsolete(startParticipantIndex, chunkSize))
                    {
                        exporter.Export(writer, chunk);    
                    }

                    Response.Flush();
                }
            }

            Response.End();
        }

        #endregion
    }
}