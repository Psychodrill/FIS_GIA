using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using Fbs.Core;

namespace Fbs.Web
{
    public static class UserAccountExtentions
    {
        // Список названий статусов пользователей.

        //private static string[] UserStatusName = new string[] { "Неизвестен", "Шаг 2 из 3 «Формирование и загрузка заявки на регистрацию»", 
        //        "На доработке", "На согласовании", "Действующий", "Отключенный" };
        private static string[] UserStatusName = new string[] { "Неизвестен", 
                "Шаг 2 из 3 «Формирование и загрузка заявки на регистрацию»", 
                "Шаг 3 из 3: «Подтверждение регистрационных данных пользователя»", 
                "Шаг 3 из 3: «Подтверждение регистрационных данных пользователя»", "Действующий", "Отключенный" };

        private static string[] UserNewStatusName = new string[] { "Неизвестен", 
                "На регистрации", "На доработке", "На проверке", "Действующий", "Отключенный" };

        private static string[] ViewUserStatusDescription = new string[] { string.Empty,
                string.Format(@"Для продолжения регистрации вам необходимо перейти на <a href='{0}'", Config.UrlEsrp+@"/Profile/DocumentUpload.aspx") +
                    "title=\"Загрузить документ регистрации\">страницу формирования и загрузки заявки на регистрацию</a>.", 
                "Ваша заявка была отправлена администратором на доработку.<br/>Для продолжения процесса регистрации, " 
                    +"пожалуйста, исправьте следующую причину, указанную администратором: <br/><div style='COLOR: darkred; width:370px; height:120px; overflow:auto;'><i>{AdminComment}</i></div>", 
                "Ваши данные получены и находятся на рассмотрении. " +"Результаты проверки будут направлены Вам по электронной почте.", 
                "Вы успешно зарегистрированы в системе.", 
                "Ваша учетная запись отключена администратором по следующей причине: <br/><B><div style='COLOR: darkred; width:370px; height:120px; overflow:auto;'><i>{AdminComment}</i></div></B>" };

        // Список описаний статусов пользователей для редактирования.
        private static string[] EditUserStatusDescription = new string[] { string.Empty,
                "Вы проходите процесс регистрации.<br/>Проверьте правильность заполнения полей анкеты и нажмите \"Сохранить\"", 
                "Ваша заявка была отправлена администратором на доработку.<br/>Для продолжения процесса регистрации, " +
                    "пожалуйста, исправьте следующую причину, указанную администратором: <br/><div style='COLOR: darkred; width:370px;height:120px; overflow:auto;'><i>{AdminComment}</i></div>", 
                "Ваша заявка находится на рассмотрении администратора. " +"В случае ошибки, с вами свяжутся сотрудники горячей линии.", 
                "Вы успешно зарегистрированы в системе.", 
                "Ваша учетная запись отключена администратором по следующей причине: <br/><div style='COLOR: darkred; width:370px; height:120px; overflow:auto;'><i>{AdminComment}</i></div>" };

        private static string[] AdministartionStatusDescription = new string[] { string.Empty,
            string.Empty,
            "Причина: <i>{AdminComment}</i>",
            string.Empty,
            string.Empty,
            "Причина: <i>{AdminComment}</i>",
            string.Empty };

        /// <summary>
        /// Получение названия статуса.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string GetStatusName(this UserAccount account)
        {
            return GetUserAccountStatusName(account.Status);
        }

        public static string GetNewStatusName(this UserAccount account)
        {
            return GetUserAccountNewStatusName(account.Status);
        }

        public static string GetNewStatusName(string sysStatus)
        {
            try
            {

                sysStatus = sysStatus.Substring(0, 1).ToUpper() + sysStatus.Substring(1).ToLower();
                UserAccount.UserAccountStatusEnum enStatus =
                    (UserAccount.UserAccountStatusEnum)
                    Enum.Parse(typeof(UserAccount.UserAccountStatusEnum), sysStatus);
                return GetUserAccountNewStatusName(enStatus);
            }
            catch
            {
                return sysStatus;
            }
        }

        public static string GetUserAccountStatusName(UserAccount.UserAccountStatusEnum status)
        {
            if ((int)status > UserStatusName.Length)
                return UserStatusName[0];
            return UserStatusName[(int)status];
        }

        public static string GetUserAccountNewStatusName(UserAccount.UserAccountStatusEnum status)
        {
            if ((int)status > UserStatusName.Length)
                return UserStatusName[0];
            return UserNewStatusName[(int)status];
        }

        public static string GetUserAccountStatusName(object statusCode)
        {
            return GetUserAccountStatusName(UserAccount.ConvertStatusCode(Convert.ToString(statusCode)));
        }

        public static string GetUserAccountNewStatusName(object statusCode)
        {
            return GetUserAccountNewStatusName(UserAccount.ConvertStatusCode(Convert.ToString(statusCode)));
        }

        public static string GetNewViewStatusDescription(this UserAccount account)
        {

            return GetUserAccountNewStatusName(account.Status);
        }

        public static string GetViewStatusDescription(this UserAccount account)
        {
            if ((int)account.Status > ViewUserStatusDescription.Length)
                return ViewUserStatusDescription[0];

            return ViewUserStatusDescription[(int)account.Status].
                Replace("{AdminComment}", account.AdminComment);
        }


        public static string GetEditStatusDescription(this UserAccount account)
        {
            if ((int)account.Status > EditUserStatusDescription.Length)
                return EditUserStatusDescription[0];

            return EditUserStatusDescription[(int)account.Status].
                Replace("{AdminComment}", account.AdminComment);
        }


        public static string GetEditStatusDescription(Fbs.Core.Users.OrgUser user)
        {
            if ((int)user.status > EditUserStatusDescription.Length)
                return EditUserStatusDescription[0];

            return EditUserStatusDescription[(int)user.status].
                Replace("{AdminComment}", user.AdminComment);
        }

        public static string GetFullName(this UserAccount account)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(account.LastName))
                result.Append(account.LastName);
            if (!string.IsNullOrEmpty(account.FirstName))
            {
                if (result.Length > 0)
                    result.Append(" ");
                result.Append(account.FirstName);
            }
            if (!string.IsNullOrEmpty(account.PatronymicName))
            {
                if (result.Length > 0)
                    result.Append(" ");
                result.Append(account.PatronymicName);
            }
            return result.ToString();
        }

        public static void SetFullName(this UserAccount account, string fullName)
        {
            account.LastName = fullName;
            account.FirstName = string.Empty;
            account.PatronymicName = string.Empty;
        }
        /*
        public static string GetIpAddressesAsEdit(this UserAccount account)
        {
            if (account.IpAddresses == null)
                return null;
            return string.Join("\r\n", account.IpAddresses);
        }

        public static void SetIpAddressesAsEdit(this UserAccount account, string ipAddresses)
        {
            account.IpAddresses = ipAddresses.Split("\r\n,; ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetIpAddressesAsView(this UserAccount account)
        {
            if (account.IpAddresses == null)
                return null;
            return string.Join("<br />", account.IpAddresses);
        }*/

        public static string GetAdministartionStatusDescription(this UserAccount account)
        {
            if ((int)account.Status > AdministartionStatusDescription.Length)
                return AdministartionStatusDescription[0];
            return AdministartionStatusDescription[(int)account.Status].
                Replace("{AdminComment}", account.AdminComment);
        }

        public static string GetAdministartionStatusDescription(Fbs.Core.Users.OrgUser user)
        {

            if ((int)user.status > AdministartionStatusDescription.Length)
                return AdministartionStatusDescription[0];
            return AdministartionStatusDescription[(int)user.status].
                Replace("{AdminComment}", user.AdminComment);
        }
    }
}
