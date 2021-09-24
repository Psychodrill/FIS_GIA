using System;
using System.Configuration;
using System.Configuration.Provider;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using Esrp.Core;
using Esrp.Utility;

namespace Esrp.Web.Providers
{
    public class EsrpRoleProvider : RoleProvider
    {
        private ConnectionStringSettings mConnectionStringSettings;
        private string mConnectionString;
        private string mApplicationName;

        public override string ApplicationName
        {
            get { return mApplicationName; }
            set { mApplicationName = value; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "Fbs Role Provider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Fbs Role Provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
            {
                mApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                mApplicationName = config["applicationName"];
            }

            // Initialize .
            mConnectionStringSettings = ConfigurationManager.
              ConnectionStrings[config["connectionStringName"]];

            if (mConnectionStringSettings == null || mConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            mConnectionString = mConnectionStringSettings.ConnectionString;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return Account.CheckRole(username, roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ProviderException("Не задано имя пользователя");

            return Account.GetRoleCodes(username);
        }

        #region "Not Implemented"

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException("AddUsersToRoles");
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException("CreateRole");
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException("DeleteRole");
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException("FindUsersInRole");
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException("GetAllRoles");
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException("GetUsersInRole");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException("RemoveUsersFromRoles");
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException("RoleExists");
        }

        #endregion

    }
}
