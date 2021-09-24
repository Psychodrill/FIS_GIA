using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.FbsCheck
{
    partial class CNERequestBatch
    {
        static public CNERequestBatch[] GetProcessingBatches()
        {
            CheckContext.BeginLock();
            try
            {
                return CheckContext.Instance().
                        SearchProcessingCommonNationalExamCertificateRequestBatch().ToArray();
            }
            finally
            {
                CheckContext.EndLock();
            }
        }

        static public CNERequestBatch GetBatch(long id)
        {
            CheckContext.BeginLock();
            try
            {
                return CheckContext.Instance().
                        GetCommonNationalExamCertificateRequestBatch(id).SingleOrDefault();
            }
            finally
            {
                CheckContext.EndLock();
            }
        }

        public void Update()
        {
            CheckContext.BeginLock();
            try
            {
                CheckContext.Instance().InternalUpdateCNERequestBatch(this);
            }
            finally
            {
                CheckContext.EndLock();
            }
        }
    }

    internal partial class CheckContext
    {
        public void InternalUpdateCNERequestBatch(CNERequestBatch batch)
        {
            this.UpdateCNERequestBatch(batch);
        }
    }
}
