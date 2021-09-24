using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FbsService
{
    public class BulkServerImportTask : Task 
    {
        class Search : TaskStatus
        {
            protected internal override string GetStatusCode()
            {
                return "search";
            }

            private string GetLocalFileName(string fileName)
            {
                return Path.Combine(TaskService.BulkFileDirectory, Path.GetFileName(fileName));
            }

            protected internal override void Execute()
            {
                foreach (BulkFile bulk in BulkFile.GetDeprecatedBulkFiles())
                    if (File.Exists(bulk.FileName))
                        File.Delete(bulk.FileName);
                foreach (BulkFile bulk in BulkFile.GetAbsenteeBulkFiles())
                {
                    File.Copy(bulk.FileName, GetLocalFileName(bulk.FileName), true);
                    bulk.FileName = GetLocalFileName(bulk.FileName);
                    bulk.Store();
                }
            }
        }

        protected override string GetTaskCode()
        {
            return "BulkServerImport";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }

    }
}
