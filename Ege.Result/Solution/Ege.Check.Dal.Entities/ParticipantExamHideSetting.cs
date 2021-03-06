namespace Ege.Check.Dal.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_ParticipantExamHideSettings")]
    public class ParticipantExamHideSetting : IHasIntId
    {
        /// <summary>
        /// </summary>
        public int ParticipantId { get; set; }

        [ForeignKey("ParticipantId")]
        public Participant Participant { get; set; }

        /// <summary>
        /// </summary>
        public int ExamGlobalId { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}