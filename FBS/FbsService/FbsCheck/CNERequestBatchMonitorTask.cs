using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.FbsCheck
{
    public class CNERequestBatchMonitorTask : Task
    {
        class Search : TaskStatus
        {
            protected internal override string GetStatusCode()
            {
                return "search";
            }

            protected internal override void Execute()
            {
                foreach (CNERequestBatch batch in CNERequestBatch.GetProcessingBatches())
                {
                    CNERequestBatchTask task = new CNERequestBatchTask();
                    task.BatchId = batch.Id;
                    task.Update();
                    batch.Executing = true;
                    batch.Update();
                }
            }
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateBatchRequest";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
