namespace Fbs.Web
{
    using System;
    using System.Collections;

    /// <summary>
    /// The account extentions.
    /// </summary>
    public static class AccountExtentions
    {
        #region Public Methods

        /// <summary>
        /// Получить описание ошибок аутентификации.
        /// </summary>
        /// <param name="IsPasswordValid">
        /// The Is Password Valid.
        /// </param>
        /// <param name="IsIpValid">
        /// The Is Ip Valid.
        /// </param>
        /// <returns>
        /// The get authentication errors description.
        /// </returns>
        public static string GetAuthenticationErrorsDescription(object IsPasswordValid, object IsIpValid)
        {
            if (!Convert.ToBoolean(IsPasswordValid))
            {
                return "неверный пароль";
            }

            if (!Convert.ToBoolean(IsIpValid))
            {
                return "неверный IP-адрес";
            }

            return string.Empty;
        }

        /// <summary>
        /// Получить изменения аккаунта.
        /// </summary>
        /// <param name="IsEdit">
        /// The Is Edit.
        /// </param>
        /// <param name="IsPasswordChange">
        /// The Is Password Change.
        /// </param>
        /// <param name="IsStatusChange">
        /// The Is Status Change.
        /// </param>
        /// <returns>
        /// The get changes description.
        /// </returns>
        public static string GetChangesDescription(object IsEdit, object IsPasswordChange, object IsStatusChange)
        {
            var result = new ArrayList();
            if (Convert.ToBoolean(IsEdit))
            {
                result.Add("редактирование");
            }

            if (Convert.ToBoolean(IsPasswordChange))
            {
                result.Add("изменение пароля");
            }

            if (Convert.ToBoolean(IsStatusChange))
            {
                result.Add("изменение состояния");
            }

            return string.Join(", ", (string[])result.ToArray(typeof(string)));
        }

        /// <summary>
        /// Формирование полного имени пользователя.
        /// </summary>
        /// <param name="LastName">
        /// Фамилия
        /// </param>
        /// <param name="FirstName">
        /// Имя
        /// </param>
        /// <param name="PatronymicName">
        /// The Patronymic Name.
        /// </param>
        /// <param name="FirstName">
        /// Отчество
        /// </param>
        /// <returns>
        /// The get full name.
        /// </returns>
        public static string GetFullName(object LastName, object FirstName, object PatronymicName)
        {
            // Соберу все в одну строку и уберу лишние пробелы
            return
                string.Format(
                    "{0} {1} {2}", 
                    Convert.ToString(LastName), 
                    Convert.ToString(FirstName), 
                    Convert.ToString(PatronymicName)).Trim();
        }

        /// <summary>
        /// Формирование полного имени пользователя с хинтом в виде HTML-кода.
        /// </summary>
        /// <param name="LastName">
        /// Фамилия
        /// </param>
        /// <param name="FirstName">
        /// Имя
        /// </param>
        /// <param name="PatronymicName">
        /// The Patronymic Name.
        /// </param>
        /// <param name="Login">
        /// Логин
        /// </param>
        /// <param name="Ip">
        /// IP-адрес
        /// </param>
        /// <param name="IsVpnEditorIp">
        /// The Is Vpn Editor Ip.
        /// </param>
        /// <param name="FirstName">
        /// Отчество
        /// </param>
        /// <returns>
        /// The get full name with hint.
        /// </returns>
        public static string GetFullNameWithHint(
            object LastName, object FirstName, object PatronymicName, object Login, object Ip, object IsVpnEditorIp)
        {
            string name;
            string title;
            string login = Convert.ToString(Login);
            string ip = string.Format(
                "{0}{1}", Convert.ToBoolean(IsVpnEditorIp) ? "VPN " : string.Empty, Convert.ToString(Ip));

            if (string.IsNullOrEmpty(login))
            {
                name = ip;
                title = string.Empty;
            }
            else
            {
                name = GetFullName(LastName, FirstName, PatronymicName);
                title = string.Format(" title='{0} ({1}, {2})'", name, login, ip);
            }

            return string.Format("<div{0}>{1}</div>", title, name);
        }

        /// <summary>
        /// The get ip full name.
        /// </summary>
        /// <param name="ip">
        /// The ip.
        /// </param>
        /// <param name="isVpnIp">
        /// The is vpn ip.
        /// </param>
        /// <returns>
        /// The get ip full name.
        /// </returns>
        public static string GetIpFullName(object ip, object isVpnIp)
        {
            if (Convert.ToBoolean(isVpnIp))
            {
                return string.Format("VPN {0}", ip);
            }

            return Convert.ToString(ip);
        }

        #endregion
    }
}