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
    public class FbsMembershipProvider : MembershipProvider
    {

        #region Private properties

        private string mConnectionString;
        private string mApplicationName;
        private bool mEnablePasswordReset;
        private bool mEnablePasswordRetrieval;
        private bool mRequiresQuestionAndAnswer;
        private bool mRequiresUniqueEmail;
        private int mMaxInvalidPasswordAttempts;
        private int mPasswordAttemptWindow;
        private int mMinRequiredPasswordLength;
        private int mMinRequiredNonAlphanumericCharacters;
        private bool mWriteExceptionsToEventLog;
        private string mPasswordStrengthRegularExpression;
        private MembershipPasswordFormat mPasswordFormat;

        #endregion

        #region Public properties

        public override string ApplicationName
        {
            get { return mApplicationName; }
            set { mApplicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return mEnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return mEnablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return mMaxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return mMinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return mMinRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return mPasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return mPasswordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return mPasswordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return mRequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return mRequiresUniqueEmail; }
        }

        #endregion

        #region Public methods

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "FbsMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Fbs Membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            mApplicationName = GetConfigValue(config["applicationName"],
                    System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            mMaxInvalidPasswordAttempts =
                Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            mPasswordAttemptWindow =
                Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            mMinRequiredNonAlphanumericCharacters =
                Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            mMinRequiredPasswordLength =
                Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            mPasswordStrengthRegularExpression =
                Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            mEnablePasswordReset =
                Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            mEnablePasswordRetrieval =
                Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            mRequiresQuestionAndAnswer =
                Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            mRequiresUniqueEmail =
                Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            mWriteExceptionsToEventLog =
                Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            string format = config["passwordFormat"];
            if (format == null)
                format = "Hashed";

            switch (format)
            {
                case "Hashed":
                    mPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    mPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    mPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Initialize MySqlConnection.
            ConnectionStringSettings ConnectionStringSettings =
              ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
                throw new ProviderException("Connection string cannot be blank.");

            mConnectionString = ConnectionStringSettings.ConnectionString;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            Account.ChangePassword(username, newPassword);
            return true;
        }

        public override bool ValidateUser(string username, string password)
        {
            Account.VerifyStateEnum verifyState;
            verifyState = Account.Verify(username);

            if (verifyState == Account.VerifyStateEnum.Valid)
                return true;

            return false;
        }

        #endregion

        #region Private methods

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        #endregion

        #region Not implemented

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException("ChangePasswordQuestionAndAnswer");
        }

        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey,
            out MembershipCreateStatus status)
        {
            throw new NotImplementedException("CreateUser");
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException("DeleteUser");
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex,
            int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("FindUsersByEmail");
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch,
            int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("FindUsersByName");
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException("GetAllUsers");
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException("GetNumberOfUsersOnline");
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException("GetPassword");
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException("GetUser");
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException("GetUser");
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException("GetUserNameByEmail");
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException("ResetPassword");
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException("UnlockUser");
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException("UpdateUser");
        }

        #endregion

    }
}
