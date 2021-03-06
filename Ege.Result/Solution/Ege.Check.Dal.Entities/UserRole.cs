namespace Ege.Check.Dal.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("UserRoles")]
    public class UserRole : IHasIntId
    {
        /// <summary>
        /// </summary>
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        /// <summary>
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}