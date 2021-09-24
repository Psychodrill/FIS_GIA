namespace Esrp.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Security;

    using Esrp.Core.Systems;
    using Esrp.Utility.Interfaces;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Пользователь системы.
    /// </summary>
    public abstract class Account:IDisposable
    {
        #region Constants and Fields

        private const MachineKeyValidation PasswordFormat = MachineKeyValidation.SHA1;

        private static readonly Hashtable mAccountTypes = new Hashtable();

        #endregion

        #region Enums

        /// <summary>
        /// The verify state enum.
        /// </summary>
        public enum VerifyStateEnum
        {
            /// <summary>
            /// The none.
            /// </summary>
            None, 

            /// <summary>
            /// The invalid.
            /// </summary>
            Invalid, 

            /// <summary>
            /// The invalid login.
            /// </summary>
            InvalidLogin, 

            /// <summary>
            /// The invalid ip.
            /// </summary>
            InvalidIp, 

            /// <summary>
            /// The valid.
            /// </summary>
            Valid, 

            /// <summary>
            /// The on registration.
            /// </summary>
            OnRegistration, 

            /// <summary>
            /// The on consideration.
            /// </summary>
            OnConsideration
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets ClientIp.
        /// </summary>
        public static string ClientIp
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        /// <summary>
        /// Gets ClientLogin.
        /// </summary>
        public static string ClientLogin
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.User.Identity.Name;
            }
        }

        /// <summary>
        /// Sets Password.
        /// </summary>
        public string Password
        {
            set
            {
                this.SetPassword(HashPassword(value), value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Изменить пароль.
        /// </summary>
        /// <param name="login">
        /// Логин.
        /// </param>
        /// <param name="password">
        /// Пароль.
        /// </param>
        public static void ChangePassword(string login, string password)
        {
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateAccountPassword(
                    login, HashPassword(password), ClientLogin, ClientIp, password);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// Проверка последнего адресса пользователя, под которым он авторизовался
        /// </summary>
        /// <param name="login">
        /// Логин.
        /// </param>
        /// <returns>
        /// The check last account ip.
        /// </returns>
        public static bool CheckLastAccountIp(string login)
        {
            AccountContext.BeginLock();
            try
            {
                return Convert.ToBoolean(
                    AccountContext.Instance().CheckLastAccountIp(login, ClientIp).Single().IsLastIp);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The check new user account email.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The check new user account email.
        /// </returns>
        public static bool CheckNewUserAccountEmail(string email)
        {
            AccountContext.BeginLock();
            try
            {
                return Convert.ToBoolean(AccountContext.Instance().CheckNewUserAccountEmail(email).Single().IsValid);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// Проверить роль.
        /// </summary>
        /// <param name="login">
        /// Логин.
        /// </param>
        /// <param name="roleCode">
        /// Код роли.
        /// </param>
        /// <returns>
        /// The check role.
        /// </returns>
        public static bool CheckRole(string login, string roleCode)
        {
            return GetRoleCodes(login).Any(testRoleCode => testRoleCode == roleCode);
        }

        /// <summary>
        /// The check user account email.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The check user account email.
        /// </returns>
        public static bool CheckUserAccountEmail(string login, string email)
        {
            AccountContext.BeginLock();
            try
            {
                int? isUniq = 0;
                Convert.ToBoolean(AccountContext.Instance().CheckUserAccountEmail(login, email, ref isUniq));
                if (isUniq != null)
                {
                    return isUniq.Value == 1;
                }

                return false;
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// Получить коды ролей.
        /// </summary>
        /// <param name="login">
        /// Логин.
        /// </param>
        /// <returns>
        /// </returns>
        public static string[] GetRoleCodes(string login)
        {
            var cacheStorage = ServiceLocator.Current.GetInstance<IUserDataCacheStorage>();
            var cachedRoles = cacheStorage.Retrieve<string[]>(login);
            if (cachedRoles != null)
            {
                return cachedRoles;
            }

            GetAccountRoleResult[] results;
            AccountContext.BeginLock();
            try
            {
                results = AccountContext.Instance().GetAccountRole(login).ToArray();
            }
            finally
            {
                AccountContext.EndLock();
            }

            var roles = new List<string>();
            foreach (GetAccountRoleResult result in results)
            {
                roles.Add(result.RoleCode);
            }

            string[] roleCodes = roles.ToArray();
            cacheStorage.Store(login, roleCodes);
            return roleCodes;
        }

        /// <summary>
        /// The get type.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// </returns>
        public static Type GetType(string login)
        {
            if (!mAccountTypes.ContainsKey(login))
            {
                lock (mAccountTypes)
                {
                    if (!mAccountTypes.ContainsKey(login))
                    {
                        GetAccountGroupResult result;
                        AccountContext.BeginLock();
                        try
                        {
                            result = AccountContext.Instance().GetAccountGroup(login).FirstOrDefault();
                        }
                        finally
                        {
                            AccountContext.EndLock();
                        }

                        if (result == null)
                        {
                            return null;
                        }

                        mAccountTypes.Add(
                            login, GeneralSystemManager.GetAccountType(result.GroupCode, (SystemKind)result.SystemID));
                    }
                }
            }

            return (Type)mAccountTypes[login];
        }

        /// <summary>
        /// проверка что пользователь принадлежит главной организации
        /// </summary>
        /// <param name="login">
        /// логин
        /// </param>
        /// <returns>
        /// проверка
        /// </returns>
        public static bool IsUserFromMainOrg(string login)
        {
            var key = login + "_IsUserFromMainOrg"; 
            var cacheStorage = ServiceLocator.Current.GetInstance<IUserDataCacheStorage>();
            var result = cacheStorage.Retrieve<bool?>(key);
            if (result != null)
            {
                return result.Value;
            }

            AccountContext.BeginLock();
            try
            {
                result = AccountContext.Instance().IsUserFromMainOrg(login);
            }
            finally
            {
                AccountContext.EndLock();
            }

            if (result == null)
            {
                throw new Exception("ошибка: результат не может быть null ");
            }

            cacheStorage.Store(key, result);
            return result.Value;
        }

        /// <summary>
        /// The verify.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// </returns>
        public static VerifyStateEnum Verify(string login, string password)
        {
            UserAccount.UserAccountStatusEnum status;
            return Verify(login, password, out status);
        }

        /// <summary>
        /// Авторизовать пользователя.
        /// </summary>
        /// <param name="login">
        /// Логин.
        /// </param>
        /// <param name="password">
        /// Пароль.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// </returns>
        public static VerifyStateEnum Verify(
            string login, string password, out UserAccount.UserAccountStatusEnum status)
        {
            status = UserAccount.UserAccountStatusEnum.None;
            VerifyAccountResult result;
            AccountContext.BeginLock();
            try
            {
                result =
                    AccountContext.Instance().VerifyAccount(login, ClientIp, HashPassword(password), password).Single();
            }
            finally
            {
                AccountContext.EndLock();
            }

            if (result.IsLoginValid.Value && result.IsIpValid.Value)
            {
                status = UserAccount.ConvertStatusCode(result.UserStatus);
                return VerifyStateEnum.Valid;
            }
            else if (!result.IsLoginValid.Value)
            {
                return VerifyStateEnum.InvalidLogin;
            }
            else if (!result.IsIpValid.Value)
            {
                return VerifyStateEnum.InvalidIp;
            }
            else
            {
                return VerifyStateEnum.Invalid;
            }
        }

        // Копылов Андрей - надо получить браузер пользователя (27.11.2017)
        public static void SaveUserAgent (string login, string userAgent) {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(userAgent))
            {
                return;
            }

            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().SaveUserAgent (
                    login, userAgent);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// The set password.
        /// </summary>
        /// <param name="passwordHash">
        /// The password hash.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        protected abstract void SetPassword(string passwordHash, string password);

        /// <summary>
        /// Хэшировать пароль.
        /// </summary>
        /// <param name="password">
        /// Пароль.
        /// </param>
        /// <returns>
        /// The hash password.
        /// </returns>
        private static string HashPassword(string password)
        {
            if (HttpContext.Current == null)
            {
                return password;
            }

            string result = FormsAuthentication.HashPasswordForStoringInConfigFile(password, PasswordFormat.ToString());
            return result;
        }

        #endregion

        public virtual void Dispose()
        {
            this.Password = "";
        }
    }
}