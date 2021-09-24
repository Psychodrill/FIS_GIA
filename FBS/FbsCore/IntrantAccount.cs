using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace Fbs.Core
{
    /// <summary>
    /// Внутренний пользователь системы.
    /// </summary>
    partial class IntrantAccount : Account
    {

       /* private string[] mIpAddresses;

        public string[] IpAddresses
        {
            get
            {
                if (mIpAddresses == null)
                    mIpAddresses = InternalIpAddresses.Split(",".ToCharArray());
                return mIpAddresses;
            }
            set
            {
                mIpAddresses = value;
                InternalIpAddresses = string.Join(",", value);
            }
        }*/

        partial void OnCreated()
        {
            // означиваю поля при создании объекта
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
            this._GroupCode = GetTypeGroupCode(this.GetType());
            this._IsActive = true;
        }

        partial void OnLoaded()
        {
            // переозначиваю поля после загрузки данных в объект, т.к. в этом случае значения,  
            // которые присваивались в OnCreated(), сбрасываются
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
        }

        protected static IntrantAccount GetIntrantAccount(Type intrantAccountType, string login)
        {
            IntrantAccount resultAccount = (IntrantAccount)Activator.CreateInstance(
                    intrantAccountType);
            IntrantAccount account;
            AccountContext.BeginLock();
            try
            {
                account = AccountContext.Instance().GetAccount(login).ToList().SingleOrDefault<IntrantAccount>();
            }
            finally
            {
                AccountContext.EndLock();
            }
            resultAccount._Login = account._Login;
            resultAccount._LastName = account._LastName;
            resultAccount._FirstName = account._FirstName;
            resultAccount._PatronymicName = account._PatronymicName;
            resultAccount._Phone = account._Phone;
            resultAccount._Email = account._Email;
            resultAccount._IsActive = account._IsActive;
            resultAccount._PasswordHash = account._PasswordHash;
            //resultAccount._IpAddresses = account._IpAddresses;
            resultAccount._GroupCode = account._GroupCode;
            resultAccount._EditorLogin = account._EditorLogin;
            resultAccount._EditorIp = account._EditorIp;
            //resultAccount._HasFixedIp = account._HasFixedIp;
            resultAccount._UpdateDate = account._UpdateDate;

            account.OnLoaded();
            return resultAccount;
        }

        public static IntrantAccount GetIntrantAccount(string login)
        {
            return (IntrantAccount)GetIntrantAccount(Account.GetType(login), login);
        }

        public static IntrantAccount GetAccount(string login)
        {
            IntrantAccount account;
            AccountContext.BeginLock();
            try
            {
                account = AccountContext.Instance().GetAccount(login).ToList().SingleOrDefault<IntrantAccount>();
            }
            finally
            {
                AccountContext.EndLock();
            }
            return account;
        }

        public void Update()
        {
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateIntrant(this);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        protected override void SetPassword(string passwordHash, string password)
        {
            this.PasswordHash = passwordHash;
        }

        public static bool CheckNewLogin(string login)
        {
            AccountContext.BeginLock();
            try
            {
                return AccountContext.Instance().CheckIntrantAccountLogin(login);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        public static string GetRemindAccountLogin(string email)
        {
            AccountContext.BeginLock();
            try
            {
                return AccountContext.Instance().GetRemindAccount(email, ClientLogin, ClientIp).Single().Login;
            }
            finally
            {
                AccountContext.EndLock();
            }
        }
    }

    public partial class AccountContext
    {
        internal void UpdateIntrant(IntrantAccount intrantAccount)
        {
            UpdateIntrantAccount(intrantAccount);
        }

        internal bool CheckIntrantAccountLogin(string login)
        {
            return (bool)CheckNewLogin(login).Single<CheckNewLoginResult>().IsExists;
        }
    }

}
