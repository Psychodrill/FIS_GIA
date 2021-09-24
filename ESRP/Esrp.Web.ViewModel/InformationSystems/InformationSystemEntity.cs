namespace Esrp.Web.ViewModel.InformationSystems
{
    /// <summary>
    /// Объект Информационной системы
    /// </summary>
    public class InformationSystemEntity
    {
        private bool availableRegistrationEnable = true;

        private bool visibleHrefAddGroup = true;

        #region Public Properties

        /// <summary>
        /// Доступно для регистрации
        /// </summary>
        public bool AvailableRegistration { get; set; }

        /// <summary>
        /// Доступно для регистрации (Enabled)
        /// </summary>
        public bool AvailableRegistrationEnable
        {
            get
            {
                return this.availableRegistrationEnable;
            }

            set
            {
                this.availableRegistrationEnable = value;
            }
        }

        /// <summary>
        /// Обозначение системы
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Количество групп
        /// </summary>
        public int NumberGroups { get; set; }

        /// <summary>
        /// Короткое наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Идентификатор ИС
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// Есть или нет группа по умолчанию
        /// </summary>
        public bool IsExistDefautlGroup { get; set; }

        /// <summary>
        /// Ссылка для добавления групп
        /// </summary>
        public bool VisibleHrefAddGroup
        {
            get
            {
                return this.visibleHrefAddGroup;
            }

            set
            {
                this.visibleHrefAddGroup = value;
            }
        }

        #endregion
    }
}