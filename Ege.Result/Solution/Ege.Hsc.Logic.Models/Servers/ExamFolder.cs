namespace Ege.Hsc.Logic.Models.Servers
{
    using System;

    public struct ExamFolder : IEquatable<ExamFolder>
    {
        public ExamFolder(DateTime examDate, int subjectCode) : this()
        {
            ExamDate = examDate;
            SubjectCode = subjectCode;
        }

        public DateTime ExamDate { get; private set; }
        public int SubjectCode { get; private set; }

        public bool Equals(ExamFolder other)
        {
            return ExamDate.Equals(other.ExamDate) && SubjectCode == other.SubjectCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj.GetType() == GetType() && Equals((ExamFolder) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ExamDate.GetHashCode() * 397) ^ SubjectCode;
            }
        }

        public static bool operator ==(ExamFolder one, ExamFolder? other)
        {
            return other != null && one.Equals(other);
        }

        public static bool operator !=(ExamFolder one, ExamFolder? other)
        {
            return !(one == other);
        }
    }
}