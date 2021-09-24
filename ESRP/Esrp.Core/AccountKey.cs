using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.Core
{
    /// <summary>
    /// Ключи доступа пользователя.
    /// </summary>
    public partial class AccountKey
    {
        /// <summary>
        /// Создание ключа доступа для заданного пользователя.
        /// </summary>
        /// <param name="login"> Логин </param>
        /// <param name="dateFrom"> Действителен с</param>
        /// <param name="dateTo"> Действителен по </param>
        /// <param name="isActive"> Ключ активный </param>
        /// <returns> Код созданного ключа </returns>
        public static string CreateAccountKey(string login, DateTime? dateFrom, DateTime? dateTo, bool isActive)
        {
            string key = Guid.NewGuid().ToString("N");
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateAccountKey(login, key, dateFrom, dateTo, isActive,
                    Account.ClientLogin, Account.ClientIp);
                return key;
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// Создание ключа доступа для текущего пользователя.
        /// </summary>
        /// <param name="dateFrom"> Действителен с </param>
        /// <param name="dateTo"> Действителен по </param>
        /// <param name="isActive"> Ключ активный </param>
        /// <returns> Код созданного ключа </returns>
        public static string CreateAccountKey(DateTime? dateFrom, DateTime? dateTo, bool isActive)
        {
            return CreateAccountKey(Account.ClientLogin, dateFrom, dateTo, isActive);
        }

        /// <summary>
        /// Получение ключа доступа для заданного пользователя.
        /// </summary>
        /// <param name="login"> Логин </param>
        /// <param name="key"> Код ключа </param>
        /// <returns> Ключи доступа пользователя </returns>
        public static AccountKey GetAccountKey(string login, string key)
        {
            AccountContext.BeginLock();
            try
            {
                return AccountContext.Instance().GetAccountKey(login, key).Single();
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// Получение ключа доступа для текущего пользователя.
        /// </summary>
        /// <param name="login"> Логин </param>
        /// <param name="key"> Код ключа </param>
        /// <returns> Ключи доступа пользователя </returns>
        public static AccountKey GetAccountKey(string key)
        {
            return GetAccountKey(Account.ClientLogin, key);
        }

        /// <summary>
        /// Сохранение ключа доступа
        /// </summary>
        public void Update()
        {
            this.EditorLogin = Account.ClientLogin;
            this.EditorIp = Account.ClientIp;

            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().InternalUpdateAccountKey(this);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }
    }

    partial class AccountContext
    {
        internal void InternalUpdateAccountKey(AccountKey accountKey)
        {
            UpdateAccountKey(accountKey);
        }
    }
}
