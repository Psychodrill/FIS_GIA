namespace Ege.Check.Dal.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_FctUsers")]
    public class FctUser : IHasIntId
    {
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

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}