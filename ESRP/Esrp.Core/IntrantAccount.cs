using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Esrp.Core.Systems;

namespace Esrp.Core
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
            this._GroupCode = GeneralSystemManager.GetAccountTypeByGroupCode(GetType());
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
                resultAccount._Login = account._Login;
                resultAccount._LastName = account._LastName;
                resultAccount._FirstName = account._FirstName;
                resultAccount._PatronymicName = account._PatronymicName;
                resultAccount._Phone = account._Phone;
                resultAccount._Email = account._Email;
                resultAccount._IsActive = account._IsActive;
                resultAccount._PasswordHash = account._PasswordHash;

                //resultAccount._IpAddresses = account._IpAddresses;
                var groupCodes = AccountContext.Instance().GetAccountGroup(account._Login).ToList().Select(x=>x.GroupCode);
                if ((groupCodes != null) &&( groupCodes.Any   ()))
                {
                    resultAccount._GroupCode =groupCodes.FirstOrDefault();
                    resultAccount.SetAllGroupCodes(groupCodes);
                }

                resultAccount._EditorLogin = account._EditorLogin;
                resultAccount._EditorIp = account._EditorIp;
                //resultAccount._HasFixedIp = account._HasFixedIp;
            }
            finally
            {
                AccountContext.EndLock();
            }

            account.OnLoaded();
            return resultAccount;
        }

        public static IntrantAccount GetIntrantAccount(string login)
        {
            return (IntrantAccount)GetIntrantAccount(Account.GetType(login), login);
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

        public void SetGroupCode(string newGroupCode)
        {
            this.GroupCode = newGroupCode;
        }

        public string GetGroupCode()
        {
            return GroupCode;
        }

        public IEnumerable<string> AllGroupCodes { get { return _groupCodes; } }
        private List<string> _groupCodes = new List<string>();

        public void SetAllGroupCodes(IEnumerable<string> groupCodes)
        {
            _groupCodes.Clear();
            _groupCodes.AddRange(groupCodes);
        }

        public override void Dispose()
        {
            this.EditorIp = "";
            this.EditorLogin = "";
            this.Email = "";
            this.FirstName = "";
            this.GroupCode = "";
            this.HasFixedIp = false;
            this.InternalIpAddresses = "";
            this.IsActive = false;
            this.LastName = "";
            this.Login = "";
            this.Password = "";
            this.PasswordHash = "";
            this.PatronymicName = "";
            this.Phone = "";
            base.Dispose();
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
