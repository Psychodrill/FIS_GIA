using Esrp.Core.Users;

namespace Esrp.Core.RegistrationTemplates
{
	public class FbsUserFbdChangeUserTemplate : OrganizationCommonInfoTemplate
	{
		public FbsUserFbdChangeUserTemplate(OrgUser fbsUser, OrgUser newUser, OrgUser curUser)
			: base(newUser, null, System.Configuration.ConfigurationManager.AppSettings["FbsUserFbdChangeUserTemplateFileName"])
		{
			InitializeFields("FbsUser", fbsUser.GetFullName());
			InitializeFields("FbsUserPosition", fbsUser.position);
			InitializeFields("FbsUserPhone", fbsUser.phone);
			InitializeFields("FbsUserEmail", fbsUser.email);


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
