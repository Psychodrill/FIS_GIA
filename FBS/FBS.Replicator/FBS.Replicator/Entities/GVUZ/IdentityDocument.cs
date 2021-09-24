using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FBS.Replicator.Entities.GVUZ
{
    public class GVUZIdentityDocument
    {
        public GVUZIdentityDocument(FastDataReader reader)
        {
            Id = DataHelper.GetInt(reader, "PersonIdentDocID").Value;
            PersonId = DataHelper.GetInt(reader, "PersonId").Value;

            DocumentTypeCode  = DataHelper.GetByte(reader, "DocumentTypeCode").Value;

            string documentSeries = DataHelper.GetString(reader, "DocumentSeries");
            string documentNumber = DataHelper.GetString(reader, "DocumentNumber");
            DocumentSeries = DataHelper.StringToBytes(documentSeries);
            DocumentNumber = DataHelper.StringToBytes(documentNumber);
        }

        public readonly int Id;
        public readonly int PersonId;

        public readonly byte DocumentTypeCode;
        public readonly byte[] DocumentSeries;
        public readonly byte[] DocumentNumber;

        public string DocumentSeriesStr { get { return DataHelper.BytesToString(DocumentSeries); } }
        public string DocumentNumberStr { get { return DataHelper.BytesToString(DocumentNumber); } }

        public GVUZPerson Person;
    }
}
