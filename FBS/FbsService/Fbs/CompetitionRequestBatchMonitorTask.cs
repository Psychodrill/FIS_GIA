using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.Fbs
{
    public class CompetitionRequestBatchMonitorTask: Task
    {
        class Search : TaskStatus
        {
            protected internal override string GetStatusCode()
            {
                return "search";
            }

            protected internal override void Execute()
            {
                foreach (CompetitionRequestBatch batch in CompetitionRequestBatch.GetProcessingBatches())
                {
                    CompetitionRequestBatchTask task = new CompetitionRequestBatchTask();
                    task.BatchId = batch.Id;
                    task.Update();
                    batch.Executing = true;
                    batch.Update();
                }
            }
        }

        protected override string GetTaskCode()
        {
            return "CompetitionCertificateRequestBatchMonitor";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
