using System;
using System.Collections.Generic;
using System.Data;
using FBS.Replicator.Replication.ERBDToFBS;

namespace FBS.Replicator.Entities.FBS
{
    public class FBSParticipant
    {
        public FBSParticipant(FastDataReader reader)
        {
            Id = new ParticipantId(reader);

            //CreateDate = DataHelper.GetDateTime(reader, "CreateDate").Value;
            //UpdateDate = DataHelper.GetDateTime(reader, "UpdateDate").Value;

            string participantCode = DataHelper.GetString(reader, "ParticipantCode");
            string surname = DataHelper.GetString(reader, "Surname");
            string name = DataHelper.GetString(reader, "Name");
            string secondName = DataHelper.GetString(reader, "SecondName");
            string documentSeries = DataHelper.GetString(reader, "DocumentSeries");
            string documentNumber = DataHelper.GetString(reader, "DocumentNumber");
            byte documentTypeCode = DataHelper.GetByte(reader, "DocumentTypeCode").Value;
            bool sex = DataHelper.GetBool(reader, "Sex").Value;
            DateTime birthDay = DataHelper.GetDateTime(reader, "BirthDay").Value;
            byte? finishRegion = DataHelper.GetByte(reader, "FinishRegion");
            byte participantCategoryFK = DataHelper.GetByte(reader, "ParticipantCategoryFK").Value;
            byte testTypeID = DataHelper.GetByte(reader, "TestTypeID").Value;

            HashCode = ParticipantDataHasher.GetDataHashCode(
                participantCode
                , surname
                , name
                , secondName
                , documentSeries
                , documentNumber
                , documentTypeCode
                , sex
                , birthDay
                , finishRegion
                , participantCategoryFK
                , testTypeID);
        }

        public readonly ParticipantId Id;

        //public readonly DateTime CreateDate;
        //public readonly DateTime UpdateDate;

        public readonly int HashCode;

        public void AddCertificate(FBSCertificate certificate)
        {
            if (_certificates == null)
            {
                _certificates = new List<FBSCertificate>();
            }
            _certificates.Add(certificate);
            certificate.Participant = this;
        }
        public IEnumerable<FBSCertificate> Certificates
        {
            get
            {
                if (_certificates == null)
                    return new List<FBSCertificate>();
                return _certificates;
            }
        }
        private List<FBSCertificate> _certificates;

        public void AddCertificateMark(FBSCertificateMark certificateMark)
        {
            if (_certificateMarks == null)
            {
                _certificateMarks = new List<FBSCertificateMark>();
            }
            _certificateMarks.Add(certificateMark);
            certificateMark.Participant = this;
        }
        public IEnumerable<FBSCertificateMark> CertificateMarks
        {
            get
            {
                if (_certificateMarks == null)
                    return new List<FBSCertificateMark>();
                return _certificateMarks;
            }
        }
        private List<FBSCertificateMark> _certificateMarks; 

        public ERBDToFBSActions Action;
    }
}
