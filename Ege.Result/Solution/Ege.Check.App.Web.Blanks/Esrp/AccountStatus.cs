namespace Esrp
{
    /// <summary>
    ///     Статус пользователя
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        ///     Не авторизован
        /// </summary>
        NotAuthorized = 0,

        /// <summary>
        ///     Авторизован
        /// </summary>
        Authorized = 1,

        /// <summary>
        ///     Авторизован, но не имеет прав доступа к целевой системе
        /// </summary>
        AuthorizedNoAccess = 2
    }
}