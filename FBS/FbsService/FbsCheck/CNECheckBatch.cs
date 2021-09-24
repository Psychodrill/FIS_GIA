using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.FbsCheck
{
    public partial class CNECheckBatch
    {

        static public CNECheckBatch[] GetProcessingBatches()
        {
            CheckContext.BeginLock();
            try
            {
                return CheckContext.Instance().
                        SearchProcessingCommonNationalExamCertificateCheckBatch().ToArray();
            }
            finally
            {
                CheckContext.EndLock();
            }
        }

        static public CNECheckBatch GetBatch(long id)
        {
            CheckContext.BeginLock();
            try
            {
                return CheckContext.Instance().
                        GetCommonNationalExamCertificateCheckBatch(id).SingleOrDefault();
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
                CheckContext.Instance().InternalUpdateCNECheckBatch(this);
            }
            finally
            {
                CheckContext.EndLock();
            }
        }
    }


    internal partial class CheckContext
    {
        public void InternalUpdateCNECheckBatch(CNECheckBatch batch)
        {
            this.UpdateCNECheckBatch(batch);
        }
    }
}
