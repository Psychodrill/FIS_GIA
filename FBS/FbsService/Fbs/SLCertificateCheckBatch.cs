using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.Fbs
{
    partial class SLCertificateCheckBatch
    {
        static public SLCertificateCheckBatch[] GetProcessingBatches()
        {
            FbsContext.BeginLock();
            try
            {
                return FbsContext.Instance().
                        SearchProcessingSchoolLeavingCertificateCheckBatch().ToArray();
            }
            finally
            {
                FbsContext.EndLock();
            }
        }

        static public SLCertificateCheckBatch GetBatch(long id)
        {
            FbsContext.BeginLock();
            try
            {
                return FbsContext.Instance().GetSchoolLeavingCertificateCheckBatch(id).SingleOrDefault();
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
                FbsContext.Instance().InternalUpdateSLCertificateCheckBatchCheckBatch(this);
            }
            finally
            {
                FbsContext.EndLock();
            }
        }
    }

    internal partial class FbsContext
    {
        public void InternalUpdateSLCertificateCheckBatchCheckBatch(SLCertificateCheckBatch batch)
        {
            this.UpdateSLCertificateCheckBatch(batch);
        }
    }
}
