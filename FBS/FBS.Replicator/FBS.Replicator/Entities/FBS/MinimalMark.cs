using System.Data;

namespace FBS.Replicator.Entities.FBS
{
    public struct FBSMinimalMarkId
    {
        public readonly short Year;
        public readonly byte SubjectId;

        public FBSMinimalMarkId(FastDataReader reader)
        {
            Year = DataHelper.GetShort(reader, "Year").Value;
            SubjectId = DataHelper.GetByte(reader, "SubjectId").Value;
        }

        public FBSMinimalMarkId(short year, byte subjectId)
        {
            Year = year;
            SubjectId = subjectId;
        }
    }

    public class FBSMinimalMark
    {
        public FBSMinimalMark(FastDataReader reader)
        {
            Id = new FBSMinimalMarkId(reader);
            MinimalMark = DataHelper.GetByte(reader, "MinimalMark").Value;
        }

        public readonly FBSMinimalMarkId Id;

        public readonly byte MinimalMark;
    }
}
