namespace Fbs.Core.WebServiceCheck
{
    /// <summary>
    /// коды ответов веб сервиса проверок
    /// </summary>
    public enum WebServiceReplyCodes
    {
        /// <summary>
        /// все норм
        /// </summary>
        Ok = 0,

        /// <summary>
        /// пользователь забанен
        /// </summary>
        UserIsBanned = 1
    }
}