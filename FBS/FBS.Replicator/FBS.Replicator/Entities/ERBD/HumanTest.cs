using System;

namespace FBS.Replicator.Entities.ERBD
{
    public class ERBDHumanTest
    {
        public ERBDHumanTest(FastDataReader reader)
        {
            HumanTestID = DataHelper.GetGuid(reader, "HumanTestID").Value;
            ParticipantFK = DataHelper.GetGuid(reader, "ParticipantFK").Value;
            RegionCode = DataHelper.GetByte(reader, "RegionCode").Value;
            TestTypeID = DataHelper.GetByte(reader, "TestTypeID").Value;
            SubjectCode = DataHelper.GetByte(reader, "SubjectCode").Value;
            ProcessCondition = DataHelper.GetShort(reader, "ProcessCondition").Value;

            UseYear = DataHelper.GetShort(reader, "UseYear").Value;
            HasAppeal = DataHelper.GetBool(reader, "HasAppeal").Value;

            ImportCreateDate = DataHelper.GetDateTime(reader, "ImportCreateDate");
            ImportUpdateDate = DataHelper.GetDateTime(reader, "ImportUpdateDate");

            VariantCode = DataHelper.GetInt(reader, "VariantCode");
            AppealStatusID = DataHelper.GetInt(reader, "AppealStatusID");

            string compositionBarcode = DataHelper.GetString(reader, "Barcode");
            CompositionBarcode = DataHelper.StringToBytes(compositionBarcode);
            string compositionProjectName = DataHelper.GetString(reader, "ProjectName");
            CompositionProjectName = DataHelper.StringToBytes(compositionProjectName);

            CompositionProjectBatchID = DataHelper.GetInt(reader, "ProjectBatchID");

            ExamDate = DataHelper.GetDateTime(reader, "ExamDate");

            Mark = DataHelper.GetByte(reader, "Mark").Value;
            CompositionStatus = DataHelper.GetByte(reader, "Status");
        }

        public readonly Guid HumanTestID;
        public readonly Guid ParticipantFK;
        public readonly byte RegionCode;
        public readonly byte TestTypeID;
        public readonly byte SubjectCode;
        public readonly DateTime? ExamDate;
        //public readonly int StationCode;
        //public readonly string AuditoriumCode;
        //public readonly int VariantCode;
        public readonly short ProcessCondition;
        public readonly byte Mark;
        public readonly short UseYear;
        public readonly bool HasAppeal;
        public readonly int? VariantCode;
        public readonly int? AppealStatusID;

        public readonly byte[] CompositionBarcode;
        public readonly byte? CompositionStatus;
        public readonly byte[] CompositionProjectName;
        public readonly int? CompositionProjectBatchID;

        public readonly DateTime? ImportCreateDate;
        public readonly DateTime? ImportUpdateDate;
    }
}
