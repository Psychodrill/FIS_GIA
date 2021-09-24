using System;
using System.Collections.Generic;
using System.Data;
using FBS.Replicator.Replication.ERBDToFBS;

namespace FBS.Replicator.Entities.ERBD
{
    public class ERBDParticipant
    {
        public ERBDParticipant(FastDataReader reader)
        {
            Id = new ParticipantId(reader);

            string participantCode = DataHelper.GetString(reader, "ParticipantCode");
            string surname = DataHelper.GetString(reader, "Surname");
            string name = DataHelper.GetString(reader, "Name");
            string secondName = DataHelper.GetString(reader, "SecondName");
            string documentSeries = DataHelper.GetString(reader, "DocumentSeries");
            string documentNumber = DataHelper.GetString(reader, "DocumentNumber");

            ParticipantCode = DataHelper.StringToBytes(participantCode);
            Surname = DataHelper.StringToBytes(surname);
            Name = DataHelper.StringToBytes(name);
            SecondName = DataHelper.StringToBytes(secondName);
            DocumentSeries = DataHelper.StringToBytes(documentSeries);
            DocumentNumber = DataHelper.StringToBytes(documentNumber);
            DocumentTypeCode = DataHelper.GetByte(reader, "DocumentTypeCode").Value;
            Sex = DataHelper.GetBool(reader, "Sex").Value;
            BirthDay = DataHelper.GetDateTime(reader, "BirthDay").Value;
            FinishRegion = DataHelper.GetByte(reader, "FinishRegion");
            ParticipantCategoryFK = DataHelper.GetByte(reader, "ParticipantCategoryFK").Value;
            TestTypeID = DataHelper.GetByte(reader, "TestTypeID").Value;

            HashCode = ParticipantDataHasher.GetDataHashCode(
                participantCode
                , surname
                , name
                , secondName
                , documentSeries
                , documentNumber
                , DocumentTypeCode
                , Sex
                , BirthDay
                , FinishRegion
                , ParticipantCategoryFK
                , TestTypeID);

            //CreateDate = DataHelper.GetDateTime(reader, "CreateDate").Value;
            //UpdateDate = DataHelper.GetDateTime(reader, "UpdateDate").Value;
            //ImportCreateDate = DataHelper.GetDateTime(reader, "ImportCreateDate");
            //ImportUpdateDate = DataHelper.GetDateTime(reader, "ImportUpdateDate");

            SurnameTrimmed = DataHelper.StringToBytes(DataHelper.NormalizeString(surname,false));
            NameTrimmed = DataHelper.StringToBytes(DataHelper.NormalizeString(name, false));
            SecondNameTrimmed = DataHelper.StringToBytes(DataHelper.NormalizeString(secondName, false));
        }

        public readonly ParticipantId Id;

        public readonly int HashCode;

        public void AddCertificate(ERBDCertificate certificate)
        {
            if (_certificates == null)
            {
                _certificates = new List<ERBDCertificate>();
            }
            _certificates.Add(certificate);
            certificate.Participant = this;
        }
        public IEnumerable<ERBDCertificate> Certificates
        {
            get
            {
                if (_certificates == null)
                    return new List<ERBDCertificate>();
                return _certificates;
            }
        }
        private List<ERBDCertificate> _certificates;

        public void AddCertificateMark(ERBDCertificateMark certificateMark)
        {
            if (_certificateMarks == null)
            {
                _certificateMarks = new List<ERBDCertificateMark>();
            }
            _certificateMarks.Add(certificateMark);
            certificateMark.Participant = this;
        }
        public IEnumerable<ERBDCertificateMark> CertificateMarks
        {
            get
            {
                if (_certificateMarks == null)
                    return new List<ERBDCertificateMark>();
                return _certificateMarks;
            }
        }
        private List<ERBDCertificateMark> _certificateMarks;

        public readonly byte[] ParticipantCode;
        public readonly byte[] Surname;
        public readonly byte[] Name;
        public readonly byte[] SecondName;
        public readonly byte[] DocumentSeries;
        public readonly byte[] DocumentNumber;
        public readonly byte DocumentTypeCode;
        public readonly bool Sex;
        public readonly DateTime BirthDay;
        public readonly byte? FinishRegion;
        public readonly byte ParticipantCategoryFK;
        public readonly byte TestTypeID;

        //public readonly DateTime CreateDate;
        //public readonly DateTime UpdateDate;
        //public readonly DateTime? ImportCreateDate;
        //public readonly DateTime? ImportUpdateDate;

        public readonly byte[] SurnameTrimmed;
        public readonly byte[] NameTrimmed;
        public readonly byte[] SecondNameTrimmed;

        public string ParticipantCodeStr { get { return DataHelper.BytesToString(ParticipantCode); } }
        public string SurnameStr { get { return DataHelper.BytesToString(Surname); } }
        public string NameStr { get { return DataHelper.BytesToString(Name); } }
        public string SecondNameStr { get { return DataHelper.BytesToString(SecondName); } }
        public string DocumentSeriesStr { get { return DataHelper.BytesToString(DocumentSeries); } }
        public string DocumentNumberStr { get { return DataHelper.BytesToString(DocumentNumber); } }

        public string SurnameTrimmedStr { get { return DataHelper.BytesToString(SurnameTrimmed); } }
        public string NameTrimmedStr { get { return DataHelper.BytesToString(NameTrimmed); } }
        public string SecondNameTrimmedStr { get { return DataHelper.BytesToString(SecondNameTrimmed); } }

        public ERBDToFBSActions Action;
    }
}
