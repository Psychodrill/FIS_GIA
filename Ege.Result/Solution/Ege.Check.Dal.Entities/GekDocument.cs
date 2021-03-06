namespace Ege.Check.Dal.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_GekDocuments")]
    public class GekDocument : IHasCreateDate, IHasIntId
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
        public string Number { get; set; }

        /// <summary>
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}