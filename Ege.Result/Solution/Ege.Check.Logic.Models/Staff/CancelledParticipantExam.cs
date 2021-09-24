namespace Ege.Check.Logic.Models.Staff
{
    using System;
    using JetBrains.Annotations;

    public class CancelledParticipantExam : IEquatable<CancelledParticipantExam>
    {
        /// <summary>
        ///     Код регистрации участника
        /// </summary>
        [NotNull]
        public string Code { get; set; }

        /// <summary>
        /// Код региона
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        public int ExamGlobalId { get; set; }

        public bool Equals(CancelledParticipantExam other)
        {

            return other != null && Code.Equals(other.Code, StringComparison.Ordinal) && RegionId.Equals(other.RegionId) &&
                   ExamGlobalId.Equals(other.ExamGlobalId);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return Code.GetHashCode() ^ (RegionId << 8) ^ (ExamGlobalId << 16);
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as CancelledParticipantExam;
            return Equals(other);
        }
    }
}