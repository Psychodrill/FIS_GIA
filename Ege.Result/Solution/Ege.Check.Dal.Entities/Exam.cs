namespace Ege.Check.Dal.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// </summary>
    [Table("dat_Exams")]
    public class Exam
    {
        public Exam()
        {
            AppealsSettings = new HashSet<AppealsSetting>();
            GekDocuments = new HashSet<GekDocument>();
            ParticipantExams = new HashSet<ParticipantExam>();
        }

        /// <summary>
        /// </summary>
        public int ExamGlobalId { get; set; }

        /// <summary>
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// </summary>
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// </summary>
        public int WaveCode { get; set; }

        /// <summary>
        /// </summary>
        public string WaveName { get; set; }

        /// <summary>
        /// </summary>
        public int SubjectCode { get; set; }

        [ForeignKey("SubjectCode")]
        public Subject SubjectObject { get; set; }

        [InverseProperty("ExamGlobal")]
        public ICollection<AppealsSetting> AppealsSettings { get; set; }

        [InverseProperty("ExamGlobal")]
        public ICollection<GekDocument> GekDocuments { get; set; }

        [InverseProperty("ExamGlobal")]
        public ICollection<ParticipantExam> ParticipantExams { get; set; }
    }
}