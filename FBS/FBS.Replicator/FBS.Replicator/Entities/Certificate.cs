using System;
using System.Data;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;

namespace FBS.Replicator.Entities
{
    public struct CertificateId
    {
        public CertificateId(ERBDHumanTest humanTest)
        {
            CertificateID = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
            UseYear = humanTest.UseYear;
            REGION = humanTest.RegionCode;
        }

        public CertificateId(FastDataReader reader)
        {
            CertificateID = DataHelper.GetGuid(reader, "CertificateID").Value;
            UseYear = DataHelper.GetShort(reader, "UseYear").Value;
            REGION = DataHelper.GetByte(reader, "REGION").Value;
        }

        public CertificateId(FBSCertificateMark certificateMark)
        {
            CertificateID = certificateMark.Id.CertificateFK;
            UseYear = certificateMark.Id.UseYear;
            REGION = certificateMark.Id.REGION;
        }

        public CertificateId(ERBDCertificateMark certificateMark)
        {
            CertificateID = certificateMark.Id.CertificateFK;
            UseYear = certificateMark.Id.UseYear;
            REGION = certificateMark.Id.REGION;
        }

        public readonly Guid CertificateID;
        public readonly short UseYear;
        public readonly byte REGION;

        public override bool Equals(object obj)
        {
            if (!(obj is CertificateId))
                return false;

            CertificateId typedObj = (CertificateId)obj;

            return CertificateID.Equals(typedObj.CertificateID)
                && UseYear.Equals(typedObj.UseYear)
                && REGION.Equals(typedObj.REGION);
        }

        public override int GetHashCode()
        {
            return (CertificateID.ToString() + UseYear.ToString() + REGION.ToString()).GetHashCode();
        }
    }

    public static class CertificateDataHasher
    {
        public static int GetDataHashCode(byte wave, string licenseNumber, string typographicNumber, Guid? participantFK, bool cancelled)
        {
            return String.Concat(
                wave
                , licenseNumber
                , typographicNumber
                , participantFK
                , cancelled).GetHashCode();
        }
    }
}
