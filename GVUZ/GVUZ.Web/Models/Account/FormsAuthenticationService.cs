using System;
using System.Web.Security;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.Models.Account
{
	public interface IFormsAuthenticationService
	{
		void SignIn(string userName, bool createPersistentCookie, int institutionID = 0, string userFullName = null);
		void SignOut();
	}

	public class FormsAuthenticationService : IFormsAuthenticationService
	{
		public void SignIn(string userName, bool createPersistentCookie, int institutionID = 0, string userFullName = null)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("Значение должно быть задано.", "userName");

			if (institutionID == 0)
			{
				UserPolicy userPolicy = UserHelper.LoadUserPolicy(userName);
				if (userPolicy == null)
					throw new InvalidOperationException(string.Format("Не найдено ОО для '{0}'.", userName));
				institutionID = userPolicy.InstitutionID ?? 0;
				userFullName = userPolicy.FullName;
			}
            InstitutionHelper.MainInstitutionID = institutionID;
			InstitutionHelper.SetInstitutionID(institutionID);
            
			UserHelper.SetCurrentUserFullName(userFullName);
			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}
	}
}