namespace Ege.Check.Dal.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_BlankInfo")]
    public class BlankInfo : IHasIntId
    {
        /// <summary>
        /// </summary>
        public int ParticipantExamId { get; set; }

        [ForeignKey("ParticipantExamId")]
        public ParticipantExam ParticipantExam { get; set; }

        /// <summary>
        /// </summary>
        public int BlankType { get; set; }

        /// <summary>
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// </summary>
        public int PrimaryMark { get; set; }

        /// <summary>
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}