namespace Ege.Check.Dal.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_AppealsSettings")]
    public class AppealsSetting : IHasIntId
    {
        /// <summary>
        /// </summary>
        public int RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }

        /// <summary>
        /// </summary>
        public int ExamGlobalId { get; set; }

        [ForeignKey("ExamGlobalId")]
        public Exam ExamGlobal { get; set; }

        /// <summary>
        /// </summary>
        public bool ShowResult { get; set; }

        /// <summary>
        /// </summary>
        public bool ApplyAppeals { get; set; }

        /// <summary>
        /// </summary>
        public DateTime? AppealDateFrom { get; set; }

        /// <summary>
        /// </summary>
        public DateTime? AppealDateTo { get; set; }

        /// <summary>
        /// </summary>
        public bool ShowBlanks { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}