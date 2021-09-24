namespace Fbs.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Security;

    using Fbs.Utility.Interfaces;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Пользователь системы.
    /// </summary>
    public abstract class Account
    {
        #region Constants and Fields

        private const MachineKeyValidation PasswordFormat = MachineKeyValidation.SHA1;

        private static readonly Hashtable mAccountTypes = new Hashtable();

        private static readonly Dictionary<string, Type> mGroupTypes = new Dictionary<string, Type> {
                { "User", typeof(UserAccount) }, 
                { "Support", typeof(SupportAccount) }, 
                { "Auditor", typeof(AuditorAccount) }, 
                { "Administrator", typeof(AdministratorAccount) }, 
                { "UserDepartment", typeof(UserAccount) }, 
                { "UserRCOI", typeof(UserAccount) }, 
            };

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
            Valid
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

        #region Public Methods

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
            foreach (string testRoleCode in GetRoleCodes(login))
            {
                if (testRoleCode == roleCode)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// забанен ли пользователь (не имеет доступа к проверкам)
        /// </summary>
        /// <param name="login">логин пользователя</param>
        /// <returns>забанен или нет</returns>
        public static bool IsBanned(string login)
        {
            AccountContext.BeginLock();
            try
            {
                return Convert.ToBoolean(AccountContext.Instance().IsUserBanned(login));
            }
            finally
            {
                AccountContext.EndLock();
            }
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
                return isUniq == 1;
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The get group.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The get group.
        /// </returns>
        public static string GetGroup(string login)
        {
            GetAccountGroupResult result;
            AccountContext.BeginLock();
            try
            {
                result = AccountContext.Instance().GetAccountGroup(login).SingleOrDefault();
            }
            finally
            {
                AccountContext.EndLock();
            }

            return result != null ? result.GroupCode : null;
        }

        /// <summary>
        /// Получить коды ролей.
        /// </summary>
        /// <param name="login">
        /// Логин.
        /// </param>
        /// <returns>
        /// коды ролей для логина
        /// </returns>
        public static string[] GetRoleCodes(string login)
        {
            var cacheStorage = ServiceLocator.Current.GetInstance<IUserRolesCacheStorage>();
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
                            result = AccountContext.Instance().GetAccountGroup(login).SingleOrDefault();
                        }
                        finally
                        {
                            AccountContext.EndLock();
                        }

                        if (result == null)
                        {
                            return null;
                        }

                        Type type;
                        if (!mGroupTypes.TryGetValue(result.GroupCode, out type))
                        {
                            return typeof(Account);
                        }

                        mAccountTypes.Add(login, type);
                    }
                }
            }

            return (Type)mAccountTypes[login];
        }

        /// <summary>
        /// The update account esrp.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="patronymicName">
        /// The patronymic name.
        /// </param>
        /// <param name="organizationId">
        /// The organization id.
        /// </param>
        /// <param name="phone">
        /// The phone.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <param name="isActive">
        /// The is active.
        /// </param>
        public static void UpdateAccountEsrp(
            string login, 
            string lastName, 
            string firstName, 
            string patronymicName, 
            int? organizationId, 
            string phone, 
            string email, 
            string status, 
            bool isActive)
        {
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateAccountEsrp(
                    login, lastName, firstName, patronymicName, organizationId, phone, email, status, isActive);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The update group user esrp.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="groupIdEsrp">
        /// The group id esrp.
        /// </param>
        /// <param name="groupsEsrp">
        /// The groups esrp.
        /// </param>
        public static void UpdateGroupUserEsrp(string login, int groupIdEsrp, string groupsEsrp)
        {
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateGroupUserEsrp(login, groupIdEsrp, groupsEsrp);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// Авторизовать пользователя.
        /// </summary>
        /// <param name="login">
        /// Логин.
        /// </param>
        /// <returns>
        /// </returns>
        public static VerifyStateEnum Verify(string login)
        {
            VerifyAccountResult result;
            AccountContext.BeginLock();
            try
            {
                result = AccountContext.Instance().VerifyAccount(login, ClientIp).Single();
            }
            finally
            {
                AccountContext.EndLock();
            }

            if (result.IsLoginValid.Value && result.IsIpValid.Value)
            {
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

        #endregion

        #region Methods

        /// <summary>
        /// The get type group code.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The get type group code.
        /// </returns>
        protected static string GetTypeGroupCode(Type type)
        {
            return
                mGroupTypes.Where(groupTypeCode => groupTypeCode.Value == type).Select(
                    groupTypeCode => groupTypeCode.Key).SingleOrDefault();
        }

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
    }
}