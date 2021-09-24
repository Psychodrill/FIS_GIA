using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Esrp.Web
{
    public static class AccountExtentions
    {
        /// <summary>
        /// Формирование полного имени пользователя.
        /// </summary>
        /// <param name="LastName">Фамилия</param>
        /// <param name="FirstName">Имя</param>
        /// <param name="FirstName">Отчество</param>
        public static string GetFullName(object LastName, object FirstName, object PatronymicName)
        {
            // Соберу все в одну строку и уберу лишние пробелы
            return String.Format("{0} {1} {2}", Convert.ToString(LastName), 
                Convert.ToString(FirstName), Convert.ToString(PatronymicName)).Trim();
        }

        /// <summary>
        /// Формирование полного имени пользователя с хинтом в виде HTML-кода.
        /// </summary>
        /// <param name="LastName">Фамилия</param>
        /// <param name="FirstName">Имя</param>
        /// <param name="FirstName">Отчество</param>
        /// <param name="Login">Логин</param>
        /// <param name="Ip">IP-адрес</param>
        public static string GetFullNameWithHint(object LastName, object FirstName,
            object PatronymicName, object Login, object Ip, object IsVpnEditorIp)
        {
            string name;
            string title;
            string login = Convert.ToString(Login);
            string ip = String.Format("{0}{1}", Convert.ToBoolean(IsVpnEditorIp) ? "VPN " : 
                String.Empty, Convert.ToString(Ip));

            if (String.IsNullOrEmpty(login))
            {
                name = ip;
                title = String.Empty;
            }
            else
            {
                name = GetFullName(LastName, FirstName, PatronymicName);
                title = String.Format(" title='{0} ({1}, {2})'", name, login, ip);
            }
            return String.Format("<div{0}>{1}</div>", title, name);
        }

        /// <summary>
        /// Получить изменения аккаунта.
        /// </summary>
        public static string GetChangesDescription(object IsEdit, object IsPasswordChange,
            object IsStatusChange)
        {
            ArrayList result = new ArrayList();
            if (Convert.ToBoolean(IsEdit))
                result.Add("редактирование");
            if (Convert.ToBoolean(IsPasswordChange))
                result.Add("изменение пароля");
            if (Convert.ToBoolean(IsStatusChange))
                result.Add("изменение состояния");
            if (!Convert.ToBoolean(IsEdit) && !Convert.ToBoolean(IsPasswordChange) && !Convert.ToBoolean(IsStatusChange))
                result.Add("просмотр профиля");
            
            return string.Join(", ", (string[])result.ToArray(typeof(string)));
        }

        /// <summary>
        /// Получить описание ошибок аутентификации.
        /// </summary>
        public static string GetAuthenticationErrorsDescription(object IsPasswordValid, object IsIpValid)
        {
            if (!Convert.ToBoolean(IsPasswordValid))
                return "неверный пароль";
            if (!Convert.ToBoolean(IsIpValid))
                return "неверный IP-адрес";
            return string.Empty;
        }

        public static string GetIpFullName(object ip, object isVpnIp)
        {
            if (Convert.ToBoolean(isVpnIp))
                return string.Format("VPN {0}", ip);
            return Convert.ToString(ip);
        }
    }
}
