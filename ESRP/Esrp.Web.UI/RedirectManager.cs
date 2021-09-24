using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using Esrp.Core;
using Esrp.Core.Systems;

namespace Esrp.Web
{
    public class RedirectManager
    {
        private const string AdministratorAccountRedirectUrl = "/Administration/Accounts/Users/List.aspx";
        private const string AuditorAccountRedirectUrl = "/Administration/Accounts/Users/List.aspx";
        private const string SupportAccountRedirectUrl = "/Administration/Accounts/Users/List.aspx";
        public const string EditUserAccountRedirectUrl = "/Profile/View.aspx";
        private const string CertificateUserAccountRedirectUrl = "/Certificates/CommonNationalCertificates/Check.aspx";
        //private const string CertificateUserAccountRedirectUrl = "/Profile/View.aspx";
        public const string DefaultRedirectUrl = "/Profile/View.aspx";
        private const string HomeRedirectUrl = "/Profile/View.aspx"; //""
    	public const string DocumentUploadForRegistrationUrl = "/Profile/DocumentUpload.aspx";
    	public const string AuthorizedStaffRedirectUrl = "/Administration/Accounts/Users/ListOU.aspx";

        // Скрытый конструктор
        private RedirectManager() { }

        /// <summary>
        /// Получение странцы по умолчанию в зависимости от типа пользователя
        /// </summary>
        /// <param name="userName">логин</param>
        /// <returns>url страницы</returns>
        public static string GetCurrentAuthUrl(string accountLogin)
        {

            // Получить тип пользователя
            Type accountType = Account.GetType(accountLogin);
            // Определить страницу перехода
			if (accountType == typeof(AdministratorAccount) && GeneralSystemManager.IsOpenSystem())
				return AdministratorAccountRedirectUrl;
			else if (accountType == typeof(AuditorAccount))
				return EditUserAccountRedirectUrl;
			else if (accountType == typeof(SupportAccount))
				return EditUserAccountRedirectUrl;
			else if (accountType == typeof(UserAccount) &&
					UserAccount.GetUserAccount(accountLogin).Status == UserAccount.UserAccountStatusEnum.Revision)
				return EditUserAccountRedirectUrl;
            else if (accountType == typeof(UserAccount) && GeneralSystemManager.HasAccessToGroup(accountLogin, EsrpManager.AuthorizedStaffGroupCode) && GeneralSystemManager.IsOpenSystem())
				return AuthorizedStaffRedirectUrl;
						/* не нужно GVUZ-783
            else if (accountType == typeof(UserAccount) &&
                    Account.CheckRole(accountLogin, "CheckCommonNationalCertificate"))
                  return CertificateUserAccountRedirectUrl;
                //return HomeRedirectUrl; */
            //else if(accountType == typeof(UserRegistration))
                return DefaultRedirectUrl;
        }
    }
}
