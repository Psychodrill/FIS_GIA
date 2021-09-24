using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FBS.Replicator.Replication.ERBDToFBS;

namespace FBS.Replicator.Entities.FBS
{
    public class FBSCertificate
    {
        public FBSCertificate(FastDataReader reader)
        {
            Id = new CertificateId(reader);

            ParticipantFK = DataHelper.GetGuid(reader, "ParticipantFK");

            byte wave = DataHelper.GetByte(reader, "Wave").Value;
            string licenseNumber = DataHelper.GetString(reader, "LicenseNumber");
            string typographicNumber = DataHelper.GetString(reader, "TypographicNumber");
            bool cancelled = DataHelper.GetBool(reader, "Cancelled").Value;

            HashCode = CertificateDataHasher.GetDataHashCode(
                wave
                , licenseNumber
                , typographicNumber
                , ParticipantFK
                , cancelled);

            //CreateDate = DataHelper.GetDateTime(reader, "CreateDate").Value;
            //UpdateDate = DataHelper.GetDateTime(reader, "UpdateDate");
            //ImportCreateDate = DataHelper.GetDateTime(reader, "ImportCreateDate");
            //ImportUpdateDate = DataHelper.GetDateTime(reader, "ImportUpdateDate");
        }

        public readonly CertificateId Id;
        
        //public readonly DateTime CreateDate;
        //public readonly DateTime? UpdateDate;

        public readonly Guid? ParticipantFK;

        public readonly int HashCode;

        public void AddCertificateMark(FBSCertificateMark certificateMark)
        {
            if (_certificateMarks == null)
            {
                _certificateMarks = new List<FBSCertificateMark>();
            }
            _certificateMarks.Add(certificateMark);
            certificateMark.Certificate = this;
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

        public FBSCertificateMark GetMarkById(CertificateMarkId id)
        {
            if (_certificateMarks == null)
                return null;
            if (_certificateMarks.Count < 20)
                return _certificateMarks.FirstOrDefault(x => x.Id.Equals(id));
            else
            {
                if (_certificateMarksDictionary == null)
                {
                    _certificateMarksDictionary = new Dictionary<CertificateMarkId, FBSCertificateMark>();
                    foreach (FBSCertificateMark certificateMark in _certificateMarks)
                    {
                        _certificateMarksDictionary.Add(certificateMark.Id, certificateMark);
                    }
                }
                if (_certificateMarksDictionary.ContainsKey(id))
                    return _certificateMarksDictionary[id];
                return null;
            }
        }
        private List<FBSCertificateMark> _certificateMarks;
        private Dictionary<CertificateMarkId, FBSCertificateMark> _certificateMarksDictionary;

        public FBSParticipant Participant;

        public FBSCancelledCertificate CancelledCertificate { get; private set; }
        public void SetCancelledCertificate(FBSCancelledCertificate cancelledCertificate)
        {
            CancelledCertificate = cancelledCertificate;
            cancelledCertificate.Certificate = this;
        }

        //public DateTime CreateDate { get; set; }
        //public DateTime? UpdateDate { get; set; }      
        //public DateTime? ImportCreateDate { get; set; }
        //public DateTime? ImportUpdateDate { get; set; }

        public ERBDToFBSActions Action;
    }
}
