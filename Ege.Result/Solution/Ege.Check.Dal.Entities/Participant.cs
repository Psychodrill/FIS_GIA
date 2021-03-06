namespace Ege.Check.Dal.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_Participants")]
    public class Participant : IHasIntId
    {
        public Participant()
        {
            ParticipantExamHideSettings = new HashSet<ParticipantExamHideSetting>();
            ParticipantExams = new HashSet<ParticipantExam>();
        }

        /// <summary>
        /// </summary>
        public Guid ParticipantRbdId { get; set; }

        /// <summary>
        /// </summary>
        public string ParticipantCode { get; set; }

        /// <summary>
        /// </summary>
        public string ParticipantHash { get; set; }

        /// <summary>
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// </summary>
        public int RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }

        [InverseProperty("Participant")]
        public ICollection<ParticipantExamHideSetting> ParticipantExamHideSettings { get; set; }

        [InverseProperty("Participant")]
        public ICollection<ParticipantExam> ParticipantExams { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}