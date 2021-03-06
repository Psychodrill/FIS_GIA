namespace Ege.Check.Dal.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_ParticipantExams")]
    public class ParticipantExam : IHasIntId
    {
        public ParticipantExam()
        {
            Answers = new HashSet<Answer>();
            Appeals = new HashSet<Appeal>();
            BlankInfo = new HashSet<BlankInfo>();
        }

        /// <summary>
        /// </summary>
        public int ParticipantId { get; set; }

        [ForeignKey("ParticipantId")]
        public Participant Participant { get; set; }

        /// <summary>
        /// </summary>
        public int ExamGlobalId { get; set; }

        [ForeignKey("ExamGlobalId")]
        public Exam ExamGlobal { get; set; }

        /// <summary>
        /// </summary>
        public int PrimaryMark { get; set; }

        /// <summary>
        /// </summary>
        public int TestMark { get; set; }

        /// <summary>
        /// </summary>
        public int Mark5 { get; set; }

        /// <summary>
        /// </summary>
        public int ProcessCondition { get; set; }

        /// <summary>
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// </summary>
        public bool IsDeleted { get; set; }

        [InverseProperty("ParticipantExam")]
        public ICollection<Answer> Answers { get; set; }

        [InverseProperty("ParticipantExa")]
        public ICollection<Appeal> Appeals { get; set; }

        [InverseProperty("ParticipantExam")]
        public ICollection<BlankInfo> BlankInfo { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}