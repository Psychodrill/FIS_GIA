namespace Ege.Check.Dal.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>
        ///     Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     Регион
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        ///     Логин
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        ///     Хэш пароля
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Активна ли учётная запись
        /// </summary>
        public int Enable { get; set; }
    }
}