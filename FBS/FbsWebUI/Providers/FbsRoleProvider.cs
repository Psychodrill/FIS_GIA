using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using Fbs.Core;
using Fbs.Utility;
using System.Web.ApplicationServices;

namespace Fbs.Web.Providers
{
    public class FbsRoleProvider : System.Web.Security.RoleProvider
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

            var x = Account.GetRoleCodes(username);
            return x;
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


        //---------------------------------------------------------
        //public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void UpdateUser(MembershipUser user)
        //{
        //    throw new NotImplementedException();
        //}

        //public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        //{
        //    throw new NotImplementedException();
        //}

        //public override bool EnablePasswordRetrieval
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override int MinRequiredNonAlphanumericCharacters
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override bool ChangePassword(string username, string oldPassword, string newPassword)
        //{
        //    throw new NotImplementedException();
        //}

        //public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        //{
        //    throw new NotImplementedException();
        //}

        //public override bool RequiresUniqueEmail
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override MembershipPasswordFormat PasswordFormat
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        //{
        //    throw new NotImplementedException();
        //}

        //public override bool RequiresQuestionAndAnswer
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override bool ValidateUser(string username, string password)
        //{
        //    throw new NotImplementedException();
        //}

        //public override int MaxInvalidPasswordAttempts
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override bool UnlockUser(string userName)
        //{
        //    throw new NotImplementedException();
        //}

        //public override MembershipUser GetUser(string username, bool userIsOnline)
        //{
        //    throw new NotImplementedException();
        //}

        //public override string PasswordStrengthRegularExpression
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override string ResetPassword(string username, string answer)
        //{
        //    throw new NotImplementedException();
        //}

        //public override string GetUserNameByEmail(string email)
        //{
        //    throw new NotImplementedException();
        //}

        //public override int PasswordAttemptWindow
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override int GetNumberOfUsersOnline()
        //{
        //    throw new NotImplementedException();
        //}

        //public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        //{
        //    throw new NotImplementedException();
        //}

        //public override string GetPassword(string username, string answer)
        //{
        //    throw new NotImplementedException();
        //}

        //public override int MinRequiredPasswordLength
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override bool EnablePasswordReset
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        //{
        //    throw new NotImplementedException();
        //}

        //public override bool DeleteUser(string username, bool deleteAllRelatedData)
        //{
        //    throw new NotImplementedException();
        //}


        #endregion

    }
}
