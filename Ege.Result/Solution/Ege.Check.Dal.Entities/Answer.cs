namespace Ege.Check.Dal.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_Answers")]
    public class Answer : IHasIntId
    {
        /// <summary>
        /// </summary>
        public int RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }

        /// <summary>
        /// </summary>
        public int ParticipantExamId { get; set; }

        [ForeignKey("ParticipantExamId")]
        public ParticipantExam ParticipantExam { get; set; }

        /// <summary>
        /// </summary>
        public int TaskTypeCode { get; set; }

        /// <summary>
        /// </summary>
        public int TaskNumber { get; set; }

        /// <summary>
        /// </summary>
        public string AnswerValue { get; set; }

        /// <summary>
        /// </summary>
        public int Mark { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}