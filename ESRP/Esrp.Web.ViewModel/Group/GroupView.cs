namespace Esrp.Web.ViewModel.Group
{
    /// <summary>
    /// The group view.
    /// </summary>
    public class GroupView
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Обозначение группы
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// По умолчанию
        /// </summary>
        public bool Default { get; set; }
    }
}