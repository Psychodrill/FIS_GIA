using System.Web.Security;
using FogSoft.Helpers;

namespace GVUZ.Web.Models.Account
{
	public enum InstitutionCreationError
	{
		Unknown,
		DuplicatedINN,
		DuplicatedOGRN,
	}

	public class AccountValidation
	{
		public string Key { get; private set; }
		public string ErrorMessage { get; private set; }

		private AccountValidation(string key, string message)
		{
			Key = key;
			ErrorMessage = message;
		}

		public static AccountValidation Create(InstitutionCreationError error)
		{
			return new AccountValidation(GetFieldName(error), error.GetResourceString());
		}

		private static string GetFieldName(InstitutionCreationError error)
		{
			switch (error)
			{
				case InstitutionCreationError.DuplicatedINN:
					return "INN";
				case InstitutionCreationError.DuplicatedOGRN:
					return "OGRN";
				default:
					return "";
			}
		}

		public static AccountValidation Create(MembershipCreateStatus createStatus)
		{
			string message = createStatus.GetResourceString(Messages.AccountValidation_UnknownError);
			
			return new AccountValidation(GetFieldName(createStatus), message);
		}

		private static string GetFieldName(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.InvalidUserName:
				case MembershipCreateStatus.InvalidEmail:
				case MembershipCreateStatus.DuplicateUserName:
				case MembershipCreateStatus.DuplicateEmail:
					return "Email";
				case MembershipCreateStatus.InvalidPassword:
					return "Password";
				default:
					return "";
			}
		}
	}
}