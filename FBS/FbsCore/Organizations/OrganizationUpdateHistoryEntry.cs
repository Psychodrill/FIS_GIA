namespace Fbs.Core.Organizations
{
    /// <summary>
    /// запись об изменении организации
    /// </summary>
    public class OrganizationUpdateHistoryEntry
    {
        /// <summary>
        /// id записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// сведения об организации до изменения
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Имя пользователя, который вносил изменения
        /// </summary>
        public string EditorName { get; set; }

        /// <summary>
        /// текстовое описание изменений
        /// </summary>
        public string UpdateDescription { get; set; }
    }
}