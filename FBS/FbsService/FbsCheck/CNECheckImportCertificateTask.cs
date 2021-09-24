using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FbsService.FbsCheck
{
    public class CNECheckImportCertificateTask : Task
    {
        class Search : TaskStatus
        {
            public const string DbName = "fbs_check_db";

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
                CheckContext.ImportCertificates(certificateBulk.FileName, certificateSubjectBulk.FileName);
                certificateBulk.AppendDbSubscription(DbName);
                certificateSubjectBulk.AppendDbSubscription(DbName);
                LogInfo("Импортированы сертификаты ЕГЭ");
            }
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateCheckImport";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
