using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FBS.Replicator.Replication.FBSToGVUZ;

namespace FBS.Replicator.Entities.FBS
{
    public class FBSPerson
    {
        public FBSPerson(FastDataReader reader)
        {
            Id = new ParticipantId(reader);
            PersonId = DataHelper.GetInt(reader, "PersonId");
            PersonLinkDate = DataHelper.GetDateTime(reader, "PersonLinkDate");

            string surname = DataHelper.NormalizeString(DataHelper.GetString(reader, "Surname"), true);
            string name = DataHelper.NormalizeString(DataHelper.GetString(reader, "Name"), true);
            string secondName = DataHelper.NormalizeString(DataHelper.GetString(reader, "SecondName"), true);
            NormSurname = DataHelper.StringToBytes(surname);
            NormName = DataHelper.StringToBytes(name);
            NormSecondName = DataHelper.StringToBytes(secondName);

            NameHashCode = PersonNameHasher.GetDataHashCode(surname, name, secondName);

            BirthDay = DataHelper.GetDateTime(reader, "BirthDay").Value;
            Sex = DataHelper.GetBool(reader, "Sex").Value;

            CreateDate = DataHelper.GetDateTime(reader, "CreateDate").Value;
            UpdateDate = DataHelper.GetDateTime(reader, "UpdateDate").Value;
        }

        public readonly ParticipantId Id;

        public int? PersonId { get; protected set; }
        public DateTime? PersonLinkDate { get; protected set; }

        public void SetPersonId(int? value)
        {
            PersonId = value;
            if (value.HasValue)
            {
                PersonLinkDate = DateTime.Now;                
            }
            else
            {
                PersonLinkDate = null;
            }

            foreach (FBSPerson sibling in Siblings)
            {
                sibling.PersonId = this.PersonId;
                sibling.PersonLinkDate = this.PersonLinkDate;
                if (sibling.Action == FBSToGVUZActions.Undefined)
                {
                    sibling.Action = FBSToGVUZActions.Link;
                }
            }
        }

        public readonly byte[] NormSurname;
        public readonly byte[] NormName;
        public readonly byte[] NormSecondName;

        public string NormSurnameStr { get { return DataHelper.BytesToString(NormSurname); } }
        public string NormNameStr { get { return DataHelper.BytesToString(NormName); } }
        public string NormSecondNameStr { get { return DataHelper.BytesToString(NormSecondName); } }

        public readonly int NameHashCode;

        public readonly DateTime BirthDay;
        public readonly bool Sex;

        public readonly DateTime CreateDate;
        public readonly DateTime UpdateDate;

        public FBSIdentityDocument Document;

        public IEnumerable<FBSPerson> Siblings { get { return _siblings; } }
        public void AddSibling(FBSPerson sibling)
        {
            if (!_siblings.Contains(sibling))
            {
                _siblings.Add(sibling);
            }
            if (!sibling._siblings.Contains(this))
            {
                sibling._siblings.Add(this);
            }
        }
        private List<FBSPerson> _siblings = new List<FBSPerson>();

        public FBSToGVUZActions Action;
    }
}
