using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace Esrp.Core
{
    partial class AccountContext
    {
        static private ThreadInstanceManager<AccountContext> mInstanceManager =
                new ThreadInstanceManager<AccountContext>(CreateInstance);

        static private AccountContext CreateInstance()
        {
            AccountContext instance = new AccountContext();
            instance.ObjectTrackingEnabled = false;
            instance.CommandTimeout = 0;
            return instance;
        }

        static internal AccountContext Instance()
        {
            return mInstanceManager.Instance();
        }

        static internal void BeginLock()
        {
            mInstanceManager.BeginLock();
        }

        static internal void EndLock()
        {
            mInstanceManager.EndLock();
        }

        static public string GetAccountLoginByKey(string key, string ip)
        {
            BeginLock();
            try
            {
                CheckAccountKeyResult result = Instance().CheckAccountKey(key, ip).SingleOrDefault();
                if ((bool)result.IsValid)
                    return result.Login;
                else
                    return null;
            }
            finally
            {
                EndLock();
            }
        }

        /*
                        [Function(Name = "dbo.CheckUserAccountEmail")]
                        internal int CheckUserAccountEmail(
                                [Parameter(DbType = "NVarChar(255)")] string login,
                                [Parameter(DbType = "NVarChar(255)")] string email,
                                [Parameter(DbType = "Bit")] ref int isUniq)
                        {
                            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), login, email, isUniq);
                            isUniq = ((int)(result.GetParameterValue(2)));
                            return ((int)(result.ReturnValue));
                        }
        */
    }

    partial class UserAccount
    {
        public string Position { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_orgTypeName", DbType = "NVarChar(255)")]
        public string OrgTypeName { get { return _orgTypeName; } set { _orgTypeName = value; } }
        private string _orgTypeName;

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_townName", DbType = "NVarChar(255)")]
        public string TownName { get { return _townName; } set { _townName = value; } }
        private string _townName;

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_organizationIsPrivate", DbType = "Bit")]
        public bool? OrganizationIsPrivate { get { return _organizationIsPrivate; } set { _organizationIsPrivate = value; } }
        private bool? _organizationIsPrivate;
    }
}
