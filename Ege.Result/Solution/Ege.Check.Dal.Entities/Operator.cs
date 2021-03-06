namespace Ege.Check.Dal.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_Operators")]
    public class Operator : IHasIntId
    {
        public Operator()
        {
            AppealsHistory = new HashSet<AppealsHistory>();
        }

        /// <summary>
        /// </summary>
        public string OperatorPosition { get; set; }

        /// <summary>
        /// </summary>
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        /// <summary>
        /// </summary>
        public int? RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }

        /// <summary>
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// </summary>
        public bool IsDeleted { get; set; }

        [InverseProperty("Operator")]
        public ICollection<AppealsHistory> AppealsHistory { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}