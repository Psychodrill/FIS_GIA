using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.Fbs
{
    public class EntrantCheckBatchMonitorTask : Task
    {
        class Search : TaskStatus
        {
            protected internal override string GetStatusCode()
            {
                return "search";
            }

            protected internal override void Execute()
            {
                foreach (EntrantCheckBatch batch in EntrantCheckBatch.GetProcessingBatches())
                {
                    EntrantCheckBatchTask task = new EntrantCheckBatchTask();
                    task.BatchId = batch.Id;
                    task.Update();
                    batch.Executing = true;
                    batch.Update();
                }
            }
        }

        protected override string GetTaskCode()
        {
            return "EntrantCheckBatchMonitor";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
