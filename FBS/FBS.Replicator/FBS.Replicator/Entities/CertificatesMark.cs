using System;
using System.Data;
using FBS.Replicator.Entities.ERBD;

namespace FBS.Replicator.Entities
{
    public struct CertificateMarkId
    {
        public CertificateMarkId(ERBDHumanTest humanTest)
        {
            CertificateMarkID = humanTest.HumanTestID;
            UseYear = humanTest.UseYear;
            REGION = humanTest.RegionCode;
            CertificateFK = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
        }

        public CertificateMarkId(FastDataReader reader)
        {
            CertificateMarkID = DataHelper.GetGuid(reader, "CertificateMarkID").Value;
            UseYear = DataHelper.GetShort(reader, "UseYear").Value;
            REGION = DataHelper.GetByte(reader, "REGION").Value;
            CertificateFK = DataHelper.GetGuid(reader, "CertificateFK").Value;
        }

        public readonly Guid CertificateMarkID;
        public readonly short UseYear;
        public readonly byte REGION;
        public readonly Guid CertificateFK;

        public override bool Equals(object obj)
        {
            if (!(obj is CertificateMarkId))
                return false;

            CertificateMarkId typedObj = (CertificateMarkId)obj;

            return CertificateMarkID.Equals(typedObj.CertificateMarkID)
                && UseYear.Equals(typedObj.UseYear)
                && REGION.Equals(typedObj.REGION)
                && CertificateFK.Equals(typedObj.CertificateFK);
        }

        public override int GetHashCode()
        {
            return (CertificateMarkID.ToString() + UseYear.ToString() + REGION.ToString() + CertificateFK.ToString()).GetHashCode();
        }
    }

    public static class CertificateMarkDataHasher
    {
        public static int GetDataHashCode(Guid participantFK, byte subjectCode, byte mark, bool hasAppeal, Guid? printedMarkID, byte testTypeID, int processCondition, int? variantCode, int? appealStatusId, DateTime? examDate, string compositionBarcode, byte? compositionPagesCount, byte? compositionStatus, string compositionPaths)
        {
            return String.Concat(
                   participantFK
                   , subjectCode
                   , mark
                   , hasAppeal
                   , printedMarkID
                   , testTypeID
                   , processCondition
                   , variantCode
                   , appealStatusId
                   , examDate
                   , compositionBarcode
                   , compositionPagesCount
                   , compositionStatus
                   , compositionPaths
                   ).GetHashCode();
        }
    }
}
