using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FBS.Replicator.Replication.FBSToGVUZ;

namespace FBS.Replicator.Entities.FBS
{
    public class FBSIdentityDocument
    {
        public FBSIdentityDocument(FBSPerson person, FastDataReader reader, IEnumerable<FBSGVUZDocumentTypesMapping> documentTypesMappings)
        {
            byte documentTypeCode = DataHelper.GetByte(reader, "DocumentTypeCode").Value;

            RVIDocumentTypeCode = GetRVIDocumentTypeCode(documentTypeCode, documentTypesMappings);

            string documentSeries = DataHelper.GetString(reader, "DocumentSeries");
            string documentNumber = DataHelper.GetString(reader, "DocumentNumber");
            DocumentSeries = DataHelper.StringToBytes(documentSeries);
            DocumentNumber = DataHelper.StringToBytes(documentNumber);

            this.Person = person;
            person.Document = this;
        }

        public readonly byte RVIDocumentTypeCode;
        public readonly byte[] DocumentSeries;
        public readonly byte[] DocumentNumber;

        public string DocumentSeriesStr { get { return DataHelper.BytesToString(DocumentSeries); } }
        public string DocumentNumberStr { get { return DataHelper.BytesToString(DocumentNumber); } }

        public readonly FBSPerson Person;

        private byte GetRVIDocumentTypeCode(byte documentTypeCode, IEnumerable<FBSGVUZDocumentTypesMapping> documentTypesMappings)
        {
            FBSGVUZDocumentTypesMapping mapping = documentTypesMappings.FirstOrDefault(x => x.ParticipantDocumentTypeCode == documentTypeCode);
            if (mapping == null)
                return 0;
            return mapping.RVIDocumentTypeCode;
        }
    }
}
