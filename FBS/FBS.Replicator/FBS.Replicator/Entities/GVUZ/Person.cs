using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FBS.Replicator.Replication.FBSToGVUZ;

namespace FBS.Replicator.Entities.GVUZ
{
    public class GVUZPerson
    {
        public GVUZPerson(FastDataReader reader)
        {
            Id = DataHelper.GetInt(reader, "PersonId").Value;

            string surname = DataHelper.NormalizeString(DataHelper.GetString(reader, "NormSurname"), true);
            string name = DataHelper.NormalizeString(DataHelper.GetString(reader, "NormName"), true);
            string secondName = DataHelper.NormalizeString(DataHelper.GetString(reader, "NormSecondName"), true);
            NormSurname = DataHelper.StringToBytes(surname);
            NormName = DataHelper.StringToBytes(name);
            NormSecondName = DataHelper.StringToBytes(secondName);

            NameHashCode = PersonNameHasher.GetDataHashCode(surname, name, secondName);

            BirthDay = DataHelper.GetDateTime(reader, "BirthDay");
            Sex = DataHelper.GetBool(reader, "Sex");

            CreateDate = DataHelper.GetDateTime(reader, "CreateDate").Value;
            UpdateDate = DataHelper.GetDateTime(reader, "UpdateDate").Value;
        }

        public readonly int Id;

        public readonly byte[] NormSurname;
        public readonly byte[] NormName;
        public readonly byte[] NormSecondName;

        public string NormSurnameStr { get { return DataHelper.BytesToString(NormSurname); } }
        public string NormNameStr { get { return DataHelper.BytesToString(NormName); } }
        public string NormSecondNameStr { get { return DataHelper.BytesToString(NormSecondName); } }

        public readonly int NameHashCode;

        public readonly DateTime? BirthDay;
        public readonly bool? Sex;

        public readonly DateTime CreateDate;
        public readonly DateTime UpdateDate; 

        public void AddDocument(GVUZIdentityDocument document)
        {
            if (_documents == null)
            {
                _documents = new List<GVUZIdentityDocument>();
            }
            _documents.Add(document);
            document.Person = this;
        }
        public IEnumerable<GVUZIdentityDocument> Documents
        {
            get
            {
                if (_documents == null)
                    return new List<GVUZIdentityDocument>();
                return _documents;
            }
        }
        private List<GVUZIdentityDocument> _documents;
    }
}
