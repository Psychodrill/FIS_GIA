using System;
using System.Data;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;

namespace FBS.Replicator.Entities
{
    public struct ParticipantId
    {
        public ParticipantId(FastDataReader reader)
        {
            ParticipantID = DataHelper.GetGuid(reader, "ParticipantID").Value;
            UseYear = DataHelper.GetShort(reader, "UseYear").Value;
            REGION = DataHelper.GetByte(reader, "REGION").Value;
        }

        public ParticipantId(FBSCertificate certificate)
        {
            ParticipantID = certificate.ParticipantFK.Value;
            UseYear = certificate.Id.UseYear;
            REGION = certificate.Id.REGION;
        }

        public ParticipantId(ERBDCertificate certificate)
        {
            ParticipantID = certificate.ParticipantFK.Value;
            UseYear = certificate.Id.UseYear;
            REGION = certificate.Id.REGION;
        }

        public ParticipantId(FBSCertificateMark certificateMark)
        {
            ParticipantID = certificateMark.ParticipantFK;
            UseYear = certificateMark.Id.UseYear;
            REGION = certificateMark.Id.REGION;
        }

        public ParticipantId(ERBDCertificateMark certificateMark)
        {
            ParticipantID = certificateMark.ParticipantFK;
            UseYear = certificateMark.Id.UseYear;
            REGION = certificateMark.Id.REGION;
        }

        public readonly Guid ParticipantID;
        public readonly short UseYear;
        public readonly byte REGION;

        public override bool Equals(object obj)
        {
            if (!(obj is ParticipantId))
                return false;

            ParticipantId typedObj = (ParticipantId)obj;

            return ParticipantID.Equals(typedObj.ParticipantID)
                && UseYear.Equals(typedObj.UseYear)
                && REGION.Equals(typedObj.REGION);
        }

        public override int GetHashCode()
        {
            return (ParticipantID.ToString() + UseYear.ToString() + REGION.ToString()).GetHashCode();
        }
    }

    public static class ParticipantDataHasher
    {
        public static int GetDataHashCode(string participantCode, string surname, string name, string secondName, string documentSeries, string documentNumber, byte documentTypeCode, bool sex, DateTime birthDay, byte? finishRegion, byte participantCategoryFK, byte testTypeID)
        {
            return String.Concat(
                participantCode
                , surname
                , name
                , secondName
                , documentSeries
                , documentNumber
                , documentTypeCode
                , sex
                , birthDay.Year
                , birthDay.Month
                , birthDay.Day
                , finishRegion
                , participantCategoryFK
                , testTypeID).GetHashCode();
        }
    }
}
