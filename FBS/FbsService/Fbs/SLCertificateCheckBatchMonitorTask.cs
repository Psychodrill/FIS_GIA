using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.Fbs
{
    public class SLCertificateCheckBatchMonitorTask : Task
    {
        class Search : TaskStatus
        {
            protected internal override string GetStatusCode()
            {
                return "search";
            }

            protected internal override void Execute()
            {
                foreach (SLCertificateCheckBatch batch in SLCertificateCheckBatch.GetProcessingBatches())
                {
                    SLCertificateCheckBatchTask task = new SLCertificateCheckBatchTask();
                    task.BatchId = batch.Id;
                    task.Update();
                    batch.Executing = true;
                    batch.Update();
                }
            }
        }

        protected override string GetTaskCode()
        {
            return "SchoolLeavingCertificateCheckBatchMonitor";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
