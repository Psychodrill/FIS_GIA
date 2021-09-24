using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FBS.Replicator.Replication.ERBDToFBS;

namespace FBS.Replicator.Entities.ERBD
{
    public class ERBDCertificate
    {
        public ERBDCertificate(FastDataReader reader)
        {
            Id = new CertificateId(reader);

            Wave = DataHelper.GetByte(reader, "Wave").Value;

            string licenseNumber = DataHelper.GetString(reader, "LicenseNumber");
            string typographicNumber = DataHelper.GetString(reader, "TypographicNumber");

            LicenseNumber = DataHelper.StringToBytes(licenseNumber);
            TypographicNumber = DataHelper.StringToBytes(typographicNumber);
            ParticipantFK = DataHelper.GetGuid(reader, "ParticipantFK");
            Cancelled = DataHelper.GetBool(reader, "Cancelled").Value;

            HashCode = CertificateDataHasher.GetDataHashCode(
                Wave
                , licenseNumber
                , typographicNumber
                , ParticipantFK
                , Cancelled);

            //CreateDate = DataHelper.GetDateTime(reader, "CreateDate");
            //UpdateDate = DataHelper.GetDateTime(reader, "UpdateDate");
            //ImportCreateDate = DataHelper.GetDateTime(reader, "ImportCreateDate");
            //ImportUpdateDate = DataHelper.GetDateTime(reader, "ImportUpdateDate");
        }

        public ERBDCertificate(ERBDHumanTest humanTest)
        {
            Id = new CertificateId(humanTest);

            Wave = 0;

            string licenseNumber = String.Empty;
            string typographicNumber = String.Empty;

            LicenseNumber = DataHelper.StringToBytes(licenseNumber);
            TypographicNumber = DataHelper.StringToBytes(typographicNumber);
            Cancelled = false;

            //ImportCreateDate =humanTest.ImportCreateDate;
            //ImportUpdateDate =humanTest.ImportUpdateDate;

            HashCode = CertificateDataHasher.GetDataHashCode(
                Wave
                , licenseNumber
                , typographicNumber
                , ParticipantFK
                , Cancelled);
        }

        public ERBDCertificate(ERBDCertificateMark certificateMark)
        {
            Id = new CertificateId(certificateMark);

            Wave = 0;

            string licenseNumber = String.Empty;
            string typographicNumber = String.Empty;

            LicenseNumber = DataHelper.StringToBytes(licenseNumber);
            TypographicNumber = DataHelper.StringToBytes(typographicNumber);
            Cancelled = false;

            HashCode = CertificateDataHasher.GetDataHashCode(
                Wave
                , licenseNumber
                , typographicNumber
                , ParticipantFK
                , Cancelled);
        }

        public readonly CertificateId Id;

        public readonly int HashCode;

        public void AddCertificateMark(ERBDCertificateMark certificateMark)
        {
            if (_certificateMarks == null)
            {
                _certificateMarks = new List<ERBDCertificateMark>();
            }
            _certificateMarks.Add(certificateMark);
            certificateMark.Certificate = this;
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

        public ERBDCertificateMark GetMarkById(CertificateMarkId id)
        {
            if (_certificateMarks == null)
                return null;
            if (_certificateMarks.Count < 20)
                return _certificateMarks.FirstOrDefault(x => x.Id.Equals(id));
            else
            {
                if (_certificateMarksDictionary == null)
                {
                    _certificateMarksDictionary = new Dictionary<CertificateMarkId, ERBDCertificateMark>();
                    foreach (ERBDCertificateMark certificateMark in _certificateMarks)
                    {
                        _certificateMarksDictionary.Add(certificateMark.Id, certificateMark);
                    }
                }
                if (_certificateMarksDictionary.ContainsKey(id))
                    return _certificateMarksDictionary[id];
                return null;
            }
        }
        private List<ERBDCertificateMark> _certificateMarks;
        private Dictionary<CertificateMarkId, ERBDCertificateMark> _certificateMarksDictionary;

        public ERBDParticipant Participant;

        public ERBDCancelledCertificate CancelledCertificate { get; private set; }
        public void SetCancelledCertificate(ERBDCancelledCertificate cancelledCertificate)
        {
            CancelledCertificate = cancelledCertificate;
            cancelledCertificate.Certificate = this;
        }

        public readonly byte Wave;
        public readonly byte[] LicenseNumber;
        public readonly byte[] TypographicNumber;
        public readonly Guid? ParticipantFK;
        public readonly bool Cancelled;

        //public readonly DateTime? CreateDate;
        //public readonly DateTime? UpdateDate;
        //public readonly DateTime? ImportCreateDate;
        //public readonly DateTime? ImportUpdateDate;

        public ERBDToFBSActions Action;

        public string LicenseNumberStr { get { return DataHelper.BytesToString(LicenseNumber); } }
        public string TypographicNumberStr { get { return DataHelper.BytesToString(TypographicNumber); } }
    }
}
