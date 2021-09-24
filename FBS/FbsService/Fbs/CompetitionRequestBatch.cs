using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.Fbs
{
    partial class CompetitionRequestBatch
    {
        static public CompetitionRequestBatch[] GetProcessingBatches()
        {
            FbsContext.BeginLock();
            try
            {
                return FbsContext.Instance().
                        SearchProcessingCompetitionCertificateRequestBatch().ToArray();
            }
            finally
            {
                FbsContext.EndLock();
            }
        }

        static public CompetitionRequestBatch GetBatch(long id)
        {
            FbsContext.BeginLock();
            try
            {
                return FbsContext.Instance().GetCompetitionCertificateRequestBatch(id).SingleOrDefault();
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
                FbsContext.Instance().InternalUpdateCompetitionRequestBatch(this);
            }
            finally
            {
                FbsContext.EndLock();
            }
        }
    }

    internal partial class FbsContext
    {
        public void InternalUpdateCompetitionRequestBatch(CompetitionRequestBatch batch)
        {
            this.UpdateCompetitionRequestBatch(batch);
        }
    }
}
