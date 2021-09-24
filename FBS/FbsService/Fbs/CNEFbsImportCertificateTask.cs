using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.Fbs
{
    public class CNEFbsImportCertificateTask : Task
    {
        class Search : TaskStatus
        {
            public const string DbName = "fbs_db";

            protected internal override string GetStatusCode()
            {
                return "search";
            }

            protected internal override void Execute()
            {
                BulkFile certificateBulk = BulkFile.GetAbsenteeDbSubscriptionBulkFile(
                    FbsService.FbsLoader.CNECertificateImportTask.CertificateBulkFileCode, DbName);
                if (certificateBulk == null)
                    return;
                BulkFile certificateSubjectBulk = BulkFile.GetAbsenteeDbSubscriptionBulkFile(
                        FbsService.FbsLoader.CNECertificateImportTask.CertificateSubjectBulkFileCode, DbName);
                if (certificateSubjectBulk == null)
                    return;
                FbsContext.ImportCertificates(certificateBulk.FileName, certificateSubjectBulk.FileName);
                certificateBulk.AppendDbSubscription(DbName);
                certificateSubjectBulk.AppendDbSubscription(DbName);
                LogInfo("Импортированы сертификаты ЕГЭ");
            }
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateFbsImport";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
