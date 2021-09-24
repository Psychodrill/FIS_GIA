namespace Esrp.Web.ViewModel.Users
{
    using Esrp.Core;

    /// <summary>
    /// Преставление о пользователе
    /// </summary>
    public class UserViewForRequest
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string FIO { get; set; }

        /// <summary>
        /// Системы
        /// </summary>
        public string SystemNames { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public UserAccount.UserAccountStatusEnum Status { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Ссылка на скан заявки на регистрацию
        /// </summary>
        public string RegDocument { get; set; }
    }
}