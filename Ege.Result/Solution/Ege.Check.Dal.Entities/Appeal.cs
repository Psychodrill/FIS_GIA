namespace Ege.Check.Dal.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_Appeals")]
    public class Appeal : IHasIntId
    {
        public Appeal()
        {
            AppealsHistory = new HashSet<AppealsHistory>();
        }

        /// <summary>
        /// </summary>
        public int ParticipantExams { get; set; }

        [ForeignKey("ParticipantExams")]
        public ParticipantExam ParticipantExam { get; set; }

        /// <summary>
        /// </summary>
        public bool AppealType { get; set; }

        /// <summary>
        /// </summary>
        public int? Station { get; set; }

        /// <summary>
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// </summary>
        public int? ReviewType { get; set; }

        /// <summary>
        /// </summary>
        public string Agent { get; set; }

        /// <summary>
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// </summary>
        public int? AgentType { get; set; }

        [InverseProperty("Appeal")]
        public ICollection<AppealsHistory> AppealsHistory { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}