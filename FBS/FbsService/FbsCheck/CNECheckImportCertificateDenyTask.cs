using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.FbsCheck
{
    public class CNECheckImportCertificateDenyTask : Task
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
                BulkFile denyBulk = BulkFile.GetAbsenteeDbSubscriptionBulkFile(
                    FbsService.FbsLoader.CNECertificateDenyImportTask.CertificateDenyBulkFileCode, DbName);
                if (denyBulk == null)
                    return;
                CheckContext.ImportCertificateDeny(denyBulk.FileName);
                denyBulk.AppendDbSubscription(DbName);
                LogInfo("Импортированы запрещения сертификатов ЕГЭ");
            }
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateDenyCheckImport";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
