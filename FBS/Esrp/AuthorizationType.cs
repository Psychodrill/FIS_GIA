namespace Esrp
{
    /// <summary>
    /// Необходимый тип авторизации
    /// </summary>
    public enum AuthorizationType
    {
        /// <summary>
        /// Проверка аутентификации. Система имеет открытую часть
        /// </summary>
        WithoutLogon = 0,

        /// <summary>
        /// Проверка аутентификации. Требуется ввести логин и пароль
        /// </summary>
        Logon = 1,

        /// <summary>
        /// Выход из ЕСРП, с последующим вводом логина и пароля
        /// </summary>
        Relogin = 2,

        /// <summary>
        /// Выход из ЕСРП. Переход в открытую часть системы
        /// </summary>
        Logout = 3
    }
}