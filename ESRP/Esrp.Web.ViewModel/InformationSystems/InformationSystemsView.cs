namespace Esrp.Web.ViewModel.InformationSystems
{
    /// <summary>
    /// Представление для ИС
    /// </summary>
    public class InformationSystemsView
    {
        /// <summary>
        /// Идентификатор ИС
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Короткое наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Количество групп
        /// </summary>
        public int NumberGroups { get; set; }

        /// <summary>
        /// Доступно для регистрации
        /// </summary>
        public string AvailableRegistration { get; set; }

        /// <summary>
        /// Обозначение системы
        /// </summary>
        public string Code { get; set; }
    }
}
