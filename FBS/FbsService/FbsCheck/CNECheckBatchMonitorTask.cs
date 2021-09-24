using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.FbsCheck
{
    [Serializable]
    public class CNECheckBatchMonitorTask : Task
    {
        class Search : TaskStatus
        {
            protected internal override string GetStatusCode()
            {
                return "search";
            }

            protected internal override void Execute()
            {
                foreach (CNECheckBatch batch in CNECheckBatch.GetProcessingBatches())
                {
                    CNECheckBatchTask task = new CNECheckBatchTask();
                    task.BatchId = batch.Id;
                    task.Update();
                    batch.Executing = true;
                    batch.Update(); 
                }
            }
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateBatchCheck";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
