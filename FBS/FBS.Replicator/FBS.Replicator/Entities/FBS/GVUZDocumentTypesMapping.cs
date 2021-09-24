using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FBS.Replicator.Entities.FBS
{
    public class FBSGVUZDocumentTypesMapping
    {
        public FBSGVUZDocumentTypesMapping(FastDataReader reader)
        {
            RVIDocumentTypeCode = DataHelper.GetByte(reader, "RVIDocumentTypeCode").Value;
            ParticipantDocumentTypeCode = DataHelper.GetByte(reader, "ParticipantDocumentTypeCode").Value;
        }

        public readonly byte RVIDocumentTypeCode;
        public readonly byte ParticipantDocumentTypeCode;
    }
}
