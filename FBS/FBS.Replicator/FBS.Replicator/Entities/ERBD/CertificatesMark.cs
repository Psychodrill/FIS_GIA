using System;
using System.Collections.Generic;
using System.Data;
using FBS.Replicator.Entities.FBS;
using FBS.Replicator.Replication.ERBDToFBS;

namespace FBS.Replicator.Entities.ERBD
{
    public class ERBDCertificateMark
    {
        public ERBDCertificateMark(FastDataReader reader, Dictionary<short, FBSExpireDate> expireDates, Dictionary<FBSMinimalMarkId, FBSMinimalMark> minimalMarks)
        {
            Id = new CertificateMarkId(reader);

            ParticipantFK = DataHelper.GetGuid(reader, "ParticipantFK").Value;
            SubjectCode = DataHelper.GetByte(reader, "SubjectCode").Value;
            Mark = DataHelper.GetByte(reader, "Mark").Value;
            HasAppeal = DataHelper.GetBool(reader, "HasAppeal").Value;
            PrintedMarkID = DataHelper.GetGuid(reader, "PrintedMarkID");
            TestTypeID = DataHelper.GetByte(reader, "TestTypeID").Value;
            ProcessCondition = DataHelper.GetShort(reader, "ProcessCondition").Value;

            if ((expireDates != null) && (minimalMarks != null))
            {
                Mark = RecalculateMark(minimalMarks);
                ProcessCondition = RecalculateProcessCondition(expireDates, minimalMarks);
            }

            HashCode = CertificateMarkDataHasher.GetDataHashCode(
              ParticipantFK
              , SubjectCode
              , Mark
              , HasAppeal
              , PrintedMarkID
              , TestTypeID
              , ProcessCondition
              , VariantCode
              , AppealStatusID
              , ExamDate
              , null
              , null
              , null
              , null
              );
        }

        public ERBDCertificateMark(ERBDHumanTest humanTest, Dictionary<short, FBSExpireDate> expireDates, Dictionary<FBSMinimalMarkId, FBSMinimalMark> minimalMarks)
        {
            Id = new CertificateMarkId(humanTest);
            ParticipantFK = humanTest.ParticipantFK;
            SubjectCode = humanTest.SubjectCode;
            Mark = humanTest.Mark;
            HasAppeal = humanTest.HasAppeal;
            TestTypeID = humanTest.TestTypeID;
            ProcessCondition = humanTest.ProcessCondition;

            VariantCode = humanTest.VariantCode;
            AppealStatusID = humanTest.AppealStatusID;

            Mark = RecalculateMark(minimalMarks);
            ProcessCondition = RecalculateProcessCondition(expireDates, minimalMarks);

            ExamDate = humanTest.ExamDate;
            CompositionBarcode = humanTest.CompositionBarcode;
            CompositionStatus = humanTest.CompositionStatus;
            CompositionProjectBatchID = humanTest.CompositionProjectBatchID;
            CompositionProjectName = humanTest.CompositionProjectName;

            HashCode = CertificateMarkDataHasher.GetDataHashCode(
              ParticipantFK
              , SubjectCode
              , Mark
              , HasAppeal
              , PrintedMarkID
              , TestTypeID
              , ProcessCondition
              , VariantCode
              , AppealStatusID
              , ExamDate
              , CompositionBarcodeStr
              , null
              , CompositionStatus
              , null
              );
        }

        public void SetCompositionsData(byte pagesCount, string paths)
        {
            CompositionPagesCount = pagesCount;
            CompositionPaths = DataHelper.StringToBytes(paths);

            HashCode = CertificateMarkDataHasher.GetDataHashCode(
             ParticipantFK
             , SubjectCode
             , Mark
             , HasAppeal
             , PrintedMarkID
             , TestTypeID
             , ProcessCondition
             , VariantCode
             , AppealStatusID
             , ExamDate
             , CompositionBarcodeStr
             , CompositionPagesCount
             , CompositionStatus
             , CompositionPathsStr
             );
        }

        private byte RecalculateMark(Dictionary<FBSMinimalMarkId, FBSMinimalMark> minimalMarks)
        {
            if (ProcessCondition == 6)
            {
                FBSMinimalMarkId minimalMarkId = new FBSMinimalMarkId(Id.UseYear, SubjectCode);
                if (minimalMarks.ContainsKey(minimalMarkId))
                {
                    FBSMinimalMark minimalMark = minimalMarks[minimalMarkId];
                    if (Mark < minimalMark.MinimalMark)
                        return 0;
                }
                return Mark;
            }
            else
                return 0;
        }

        private short RecalculateProcessCondition(Dictionary<short, FBSExpireDate> expireDates, Dictionary<FBSMinimalMarkId, FBSMinimalMark> minimalMarks)
        {
            if (expireDates.ContainsKey(Id.UseYear))
            {
                FBSExpireDate expireDate = expireDates[Id.UseYear];
                if (expireDate.ExpireDate.Year < DateTime.Now.Year)
                    return 11;
            }

            if (ProcessCondition == 6)
            {
                FBSMinimalMarkId minimalMarkId = new FBSMinimalMarkId(Id.UseYear, SubjectCode);
                if (minimalMarks.ContainsKey(minimalMarkId))
                {
                    FBSMinimalMark minimalMark = minimalMarks[minimalMarkId];
                    if (Mark < minimalMark.MinimalMark)
                        return 66;
                }
            }

            return ProcessCondition;
        }

        public ERBDCertificate Certificate;
        public ERBDParticipant Participant;

        public readonly CertificateMarkId Id;

        public int HashCode { get; private set; }

        public readonly Guid ParticipantFK;
        public readonly byte SubjectCode;
        public readonly byte Mark;
        public readonly bool HasAppeal;
        public readonly Guid? PrintedMarkID;
        public readonly byte TestTypeID;
        public readonly short ProcessCondition;

        public readonly int? VariantCode;
        public readonly int? AppealStatusID;

        public readonly DateTime? ExamDate;
        public readonly byte[] CompositionBarcode;
        public readonly byte? CompositionStatus;
        public readonly byte[] CompositionProjectName;
        public readonly int? CompositionProjectBatchID;

        public byte? CompositionPagesCount { get; private set; }
        public byte[] CompositionPaths { get; private set; }

        public string CompositionBarcodeStr { get { return DataHelper.BytesToString(CompositionBarcode); } }
        public string CompositionPathsStr { get { return DataHelper.BytesToString(CompositionPaths); } }
        public string CompositionProjectNameStr { get { return DataHelper.BytesToString(CompositionProjectName); } }

        public ERBDToFBSActions Action;
    }
}
