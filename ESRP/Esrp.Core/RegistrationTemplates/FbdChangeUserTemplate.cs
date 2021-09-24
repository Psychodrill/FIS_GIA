using Esrp.Core.Users;

namespace Esrp.Core.RegistrationTemplates
{
	public class FbdChangeUserTemplate : OrganizationCommonInfoTemplate
	{
		public FbdChangeUserTemplate(OrgUser newUser, OrgUser curUser)
			: base(newUser, null, System.Configuration.ConfigurationManager.AppSettings["FbdChangeUserTemplateFileName"])
		{
			InitializeFields("FbdUser", newUser.GetFullName());
			InitializeFields("FbdUserPosition", newUser.position);
			InitializeFields("FbdUserPhone", newUser.phone);
			InitializeFields("FbdUserEmail", newUser.email);

			InitializeFields("FbdCurUser", curUser.GetFullName());
			InitializeFields("FbdCurUserPosition", curUser.position);
			InitializeFields("FbdCurUserPhone", curUser.phone);
			InitializeFields("FbdCurUserEmail", curUser.email);
		}
	}
}
