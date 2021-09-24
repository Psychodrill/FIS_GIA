// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserAccountExtentions.cs" company="">
//   
// </copyright>
// <summary>
//   The user account extentions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Esrp.Web
{
    using System;
    using System.Text;

    using Esrp.Core;
    using Esrp.Core.Common;
    using Esrp.Core.Users;
    using Esrp.Utility;

    /// <summary>
    /// The user account extentions.
    /// </summary>
    public static class UserAccountExtentions
    {
        // Список названий статусов пользователей.

        // private static string[] UserStatusName = new string[] { "Неизвестен", "Шаг 2 из 3 «Формирование и загрузка заявки на регистрацию»", 
        // "На доработке", "На согласовании", "Действующий", "Отключенный" };
        #region Constants and Fields

        /// <summary>
        /// The administartion status description.
        /// </summary>
        private static readonly string[] AdministartionStatusDescription = new[]
            {
                string.Empty, string.Empty, string.Empty, "Причина: <i>{AdminComment}</i>", string.Empty, 
                "Причина: <i>{AdminComment}</i>", string.Empty
            };

        /// <summary>
        /// The edit user status description.
        /// </summary>
        private static readonly string[] EditUserStatusDescription = new[]
            {
                string.Empty, 
                "Вы проходите процесс регистрации.<br/>Проверьте правильность заполнения полей анкеты и нажмите \"Сохранить\""
                , 
                "Ваша заявка находится на рассмотрении администратора. "
                + "В случае ошибки, с вами свяжутся сотрудники горячей линии.", 
                "Ваша заявка была отправлена администратором на доработку.<br/>Для продолжения процесса регистрации, "
                +
                "пожалуйста, исправьте следующую причину, указанную администратором: <br/><div style='COLOR: darkred; width:370px;height:120px; overflow:auto;'><i>{AdminComment}</i></div>"
                , string.Empty, "Вы успешно зарегистрированы в системе.", 
                "Ваша учетная запись отключена администратором по следующей причине: <br/><div style='COLOR: darkred; width:370px; height:120px; overflow:auto;'><i>{AdminComment}</i></div>"
            };

        /// <summary>
        /// The user new status name.
        /// </summary>
        private static readonly string[] UserNewStatusName = new[]
            {
                "Неизвестен", "На регистрации", "На согласовании", "На доработке", "Только для чтения", "Действующий", 
                "Отключенный"
            };

        /// <summary>
        /// The user status name.
        /// </summary>
        private static readonly string[] UserStatusName = new[]
            {
                "Неизвестен", "Шаг 2 из 3 «Формирование и загрузка заявки на регистрацию»", 
                "Шаг 3 из 3: «Подтверждение регистрационных данных пользователя»", 
                "Шаг 3 из 3: «Подтверждение регистрационных данных пользователя»", 
                "Шаг 3 из 3: «Подтверждение регистрационных данных пользователя»", "Действующий", "Отключенный"
            };

        // Список описаний статусов пользователей для просмотра.
        // private static string[] ViewUserStatusDescription = new string[] { string.Empty,
        // string.Format("Вы проходите процесс регистрации.<br/>Для продолжения регистрации вам необходимо распечатать " +
        // "бланк документа регистрации, заверить его подписями и печатями, загрузить в систему отсканированную копию " +
        // "подтвержденного документа. Проверьте правильность заполнения анкеты и перейдите по ссылке <a href=\"{0}\" " +
        // "title=\"Загрузить документ регистрации\">Шаг 2 регистрации: Загрузка документа регистрации.</a>.", 
        // "/Profile/DocumentUpload.aspx"), 
        // "Ваша заявка была отправлена администратором на доработку.<br/>Для продолжения процесса регистрации, " +
        // "пожалуйста, исправьте следующую причину, указанную администратором: <br/><div style='COLOR: darkred; width:600px; overflow:auto;'><i>{AdminComment}</i></div>", 
        // "Ваша заявка находится на рассмотрении администратора. " +
        // "В случае ошибки, с вами свяжутся сотрудники горячей линии.", 
        // "Вы успешно зарегистрированы в системе.", 
        // "Ваша учетная запись отключена администратором по следующей причине: <br/><B><div style='COLOR: darkred; width:600px; overflow:auto;'><i>{AdminComment}</i></div></B>" };

        /// <summary>
        /// The view user status description.
        /// </summary>
        private static readonly string[] ViewUserStatusDescription = new[]
            {
                string.Empty, 
                "Для продолжения регистрации вам необходимо перейти на <a href=\"/Profile/DocumentUpload.aspx\" "
                + "title=\"Загрузить документ регистрации\">страницу формирования и загрузки заявки на регистрацию</a>.", 
                "Ваши данные получены и находятся на рассмотрении. "
                + "Результаты проверки будут направлены Вам по электронной почте.", 
                "Ваша заявка была отправлена администратором на доработку.<br/>Для продолжения процесса регистрации, "
                +
                "пожалуйста, исправьте следующую причину, указанную администратором: <br/><div style='COLOR: darkred; width:370px; height:120px; overflow:auto;'><i>{AdminComment}</i></div>"
                , string.Empty, "Вы успешно зарегистрированы в системе.", 
                "Ваша учетная запись отключена администратором по следующей причине: <br/><B><div style='COLOR: darkred; width:370px; height:120px; overflow:auto;'><i>{AdminComment}</i></div></B>"
            };

        #endregion

        #region Public Methods

        /// <summary>
        /// Активация пользователя с отправкой письма при успешной активации.
        /// В случае ошибок будет возвращено сообщение.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The activate user.
        /// </returns>
        public static string ActivateUser(OrgUser user)
        {
            // Подтвержу регистрацию
            var Acc = new UserAccount();
            Acc.Login = user.login;
            try
            {
                Acc.ConfirmRegistration();
            }
            catch (DbException exc)
            {
                return exc.Message;
            }

            // Подготовлю email сообщение 
            var template = new EmailTemplate(EmailTemplateTypeEnum.Activation);
            EmailMessage message = template.ToEmailMessage();
            message.To = user.email;
            message.Params = Utility.CollectEmailMetaVariables(user);

            // Отправлю уведомление
            TaskManager.SendEmail(message);

            return null;
        }

        /// <summary>
        /// The get administartion status description.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// The get administartion status description.
        /// </returns>
        public static string GetAdministartionStatusDescription(this UserAccount account)
        {
            if ((int)account.Status > AdministartionStatusDescription.Length)
            {
                return AdministartionStatusDescription[0];
            }

            return AdministartionStatusDescription[(int)account.Status].Replace("{AdminComment}", account.AdminComment);
        }

        /// <summary>
        /// The get administartion status description.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The get administartion status description.
        /// </returns>
        public static string GetAdministartionStatusDescription(OrgUser user)
        {
            if ((int)user.status > AdministartionStatusDescription.Length)
            {
                return AdministartionStatusDescription[0];
            }

            return AdministartionStatusDescription[(int)user.status].Replace("{AdminComment}", user.AdminComment);
        }

        /// <summary>
        /// The get edit status description.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// The get edit status description.
        /// </returns>
        public static string GetEditStatusDescription(this UserAccount account)
        {
            if ((int)account.Status > EditUserStatusDescription.Length)
            {
                return EditUserStatusDescription[0];
            }

            return EditUserStatusDescription[(int)account.Status].Replace("{AdminComment}", account.AdminComment);
        }

        /// <summary>
        /// The get edit status description.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The get edit status description.
        /// </returns>
        public static string GetEditStatusDescription(OrgUser user)
        {
            if ((int)user.status > EditUserStatusDescription.Length)
            {
                return EditUserStatusDescription[0];
            }

            return EditUserStatusDescription[(int)user.status].Replace("{AdminComment}", user.AdminComment);
        }

        /// <summary>
        /// The get full name.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// The get full name.
        /// </returns>
        public static string GetFullName(this UserAccount account)
        {
            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(account.LastName))
            {
                result.Append(account.LastName);
            }

            if (!string.IsNullOrEmpty(account.FirstName))
            {
                if (result.Length > 0)
                {
                    result.Append(" ");
                }

                result.Append(account.FirstName);
            }

            if (!string.IsNullOrEmpty(account.PatronymicName))
            {
                if (result.Length > 0)
                {
                    result.Append(" ");
                }

                result.Append(account.PatronymicName);
            }

            return result.ToString();
        }

        // Список описаний статусов пользователей для редактирования.

        /// <summary>
        /// The get new status name.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// The get new status name.
        /// </returns>
        public static string GetNewStatusName(this UserAccount account)
        {
            return GetUserAccountNewStatusName(account.Status);
        }

        /// <summary>
        /// The get new status name.
        /// </summary>
        /// <param name="sysStatus">
        /// The sys status.
        /// </param>
        /// <returns>
        /// The get new status name.
        /// </returns>
        public static string GetNewStatusName(string sysStatus)
        {
            try
            {
                sysStatus = sysStatus.Substring(0, 1).ToUpper() + sysStatus.Substring(1).ToLower();
                var enStatus =
                    (UserAccount.UserAccountStatusEnum)Enum.Parse(typeof(UserAccount.UserAccountStatusEnum), sysStatus);
                return GetUserAccountNewStatusName(enStatus);
            }
            catch
            {
                return sysStatus;
            }
        }

        /// <summary>
        /// The get new view status description.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// The get new view status description.
        /// </returns>
        public static string GetNewViewStatusDescription(this UserAccount account)
        {
            return GetUserAccountNewStatusName(account.Status);
        }

        /// <summary>
        /// Получение названия статуса.
        /// </summary>
        /// <param name="account">
        /// </param>
        /// <returns>
        /// The get status name.
        /// </returns>
        public static string GetStatusName(this UserAccount account)
        {
            return GetUserAccountStatusName(account.Status);
        }

        /// <summary>
        /// The get user account new status name.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The get user account new status name.
        /// </returns>
        public static string GetUserAccountNewStatusName(UserAccount.UserAccountStatusEnum status)
        {
            if ((int)status > UserStatusName.Length)
            {
                return UserStatusName[0];
            }

            return UserNewStatusName[(int)status];
        }

        /// <summary>
        /// The get user account new status name.
        /// </summary>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <returns>
        /// The get user account new status name.
        /// </returns>
        public static string GetUserAccountNewStatusName(object statusCode)
        {
            return GetUserAccountNewStatusName(UserAccount.ConvertStatusCode(Convert.ToString(statusCode)));
        }

        /// <summary>
        /// The get user account status name.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The get user account status name.
        /// </returns>
        public static string GetUserAccountStatusName(UserAccount.UserAccountStatusEnum status)
        {
            if ((int)status > UserStatusName.Length)
            {
                return UserStatusName[0];
            }

            return UserStatusName[(int)status];
        }

        /// <summary>
        /// The get user account status name.
        /// </summary>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <returns>
        /// The get user account status name.
        /// </returns>
        public static string GetUserAccountStatusName(object statusCode)
        {
            return GetUserAccountStatusName(UserAccount.ConvertStatusCode(Convert.ToString(statusCode)));
        }

        /// <summary>
        /// The get view status description.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// The get view status description.
        /// </returns>
        public static string GetViewStatusDescription(this UserAccount account)
        {
            if ((int)account.Status > ViewUserStatusDescription.Length)
            {
                return ViewUserStatusDescription[0];
            }

            return ViewUserStatusDescription[(int)account.Status].Replace("{AdminComment}", account.AdminComment);
        }

        /// <summary>
        /// The send mail.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="emailType">
        /// The email type.
        /// </param>
        public static void SendMail(OrgUserBrief user, EmailTemplateTypeEnum emailType)
        {
            // Подготовлю email сообщение 
            var template = new EmailTemplate(emailType);
            EmailMessage message = template.ToEmailMessage();
            message.To = user.Email;
            message.Params = Utility.CollectEmailMetaVariables(user);

            // Отправлю уведомление
            TaskManager.SendEmail(message);
        }

        /// <summary>
        /// The set full name.
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <param name="fullName">
        /// The full name.
        /// </param>
        public static void SetFullName(this UserAccount account, string fullName)
        {
            account.LastName = fullName;
            account.FirstName = string.Empty;
            account.PatronymicName = string.Empty;
        }

        #endregion
    }
}