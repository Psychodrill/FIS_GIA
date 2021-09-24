using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.Fbs
{
    partial class EntrantCheckBatch
    {
        static public EntrantCheckBatch[] GetProcessingBatches()
        {
            FbsContext.BeginLock();
            try
            {
                return FbsContext.Instance().
                        SearchProcessingEntrantCheckBatch().ToArray();
            }
            finally
            {
                FbsContext.EndLock();
            }
        }

        static public EntrantCheckBatch GetBatch(long id)
        {
            FbsContext.BeginLock();
            try
            {
                return FbsContext.Instance().GetEntrantCheckBatch(id).SingleOrDefault();
            }
            finally
            {
                FbsContext.EndLock();
            }
        }

        public void Update()
        {
            FbsContext.BeginLock();
            try
            {
                FbsContext.Instance().InternalUpdateEntrantCheckBatch(this);
            }
            finally
            {
                FbsContext.EndLock();
            }
        }
    }

    internal partial class FbsContext
    {
        public void InternalUpdateEntrantCheckBatch(EntrantCheckBatch batch)
        {
            this.UpdateEntrantCheckBatch(batch);
        }
    }
}
