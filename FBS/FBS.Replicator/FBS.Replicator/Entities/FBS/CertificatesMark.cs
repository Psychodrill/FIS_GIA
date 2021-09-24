using System;
using System.Data;
using FBS.Replicator.Replication.ERBDToFBS;

namespace FBS.Replicator.Entities.FBS
{
    public class FBSCertificateMark
    {
        public FBSCertificateMark(FastDataReader reader)
        {
            Id = new CertificateMarkId(reader);

            ParticipantFK = DataHelper.GetGuid(reader, "ParticipantFK").Value;

            byte subjectCode = DataHelper.GetByte(reader, "SubjectCode").Value;
            byte mark = DataHelper.GetByte(reader, "Mark").Value;
            bool hasAppeal = DataHelper.GetBool(reader, "HasAppeal").Value;
            Guid? printedMarkID = DataHelper.GetGuid(reader, "PrintedMarkID");
            byte testTypeID = DataHelper.GetByte(reader, "TestTypeID").Value;
            int processCondition = DataHelper.GetInt(reader, "ProcessCondition").Value;

            int? variantCode = DataHelper.GetInt(reader, "VariantCode");
            int? appealStatusID = DataHelper.GetInt(reader, "AppealStatusID");

            DateTime? examDate = DataHelper.GetDateTime(reader, "ExamDate");
            string compositionBarcode = DataHelper.GetString(reader, "CompositionBarcode");
            byte? compositionPagesCount = DataHelper.GetByte(reader, "CompositionPagesCount");
            byte? compositionStatus = DataHelper.GetByte(reader, "CompositionStatus");
            string compositionPaths = DataHelper.GetString(reader, "CompositionPaths");

            HashCode = CertificateMarkDataHasher.GetDataHashCode(
                ParticipantFK
                , subjectCode
                , mark
                , hasAppeal
                , printedMarkID
                , testTypeID
                , processCondition
                , variantCode
                , appealStatusID
                , examDate
                , compositionBarcode
                , compositionPagesCount
                , compositionStatus
                , compositionPaths
                );
        }

        public FBSParticipant Participant;
        public FBSCertificate Certificate;

        public readonly CertificateMarkId Id;

        public readonly Guid ParticipantFK;

        public readonly int HashCode;

        public ERBDToFBSActions Action;
    }
}
