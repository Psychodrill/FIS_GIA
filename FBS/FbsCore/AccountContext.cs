using System.Linq;

namespace Fbs.Core
{
    partial class AccountContext
    {

        public AccountContext() :
            base(Properties.Settings.Default.FbsConnectionString, mappingSource)
        {
            OnCreated();
        }

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
    }
}
