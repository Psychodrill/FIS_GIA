namespace Ege.Check.Dal.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_AppealsHistory")]
    public class AppealsHistory : IHasIntId
    {
        /// <summary>
        /// </summary>
        public int AppealId { get; set; }

        [ForeignKey("AppealId")]
        public Appeal Appeal { get; set; }

        /// <summary>
        /// </summary>
        public int AppealStatus { get; set; }

        /// <summary>
        /// </summary>
        public int? OperatorId { get; set; }

        [ForeignKey("OperatorId")]
        public Operator Operator { get; set; }

        /// <summary>
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// </summary>
        public bool SignDocument { get; set; }

        /// <summary>
        /// </summary>
        public int? AgentType { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}