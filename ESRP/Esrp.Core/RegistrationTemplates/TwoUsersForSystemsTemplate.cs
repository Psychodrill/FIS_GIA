using Esrp.Core.Users;

namespace Esrp.Core.RegistrationTemplates
{
	public class TwoUsersForSystemsTemplate : OrganizationCommonInfoTemplate
	{
		public TwoUsersForSystemsTemplate(OrgUser fbsUser, OrgUser fbdUser, string headerBlock)
			: base(fbsUser, headerBlock, System.Configuration.ConfigurationManager.AppSettings["TwoUsersForSystemsTemplateFileName"])
		{
			InitializeFields("FbsUser", fbsUser.GetFullName());
			InitializeFields("FbsUserPosition", fbsUser.position);
			InitializeFields("FbsUserPhone", fbsUser.phone);
			InitializeFields("FbsUserEmail", fbsUser.email);

			InitializeFields("FbdUser", fbdUser.GetFullName());
			InitializeFields("FbdUserPosition", fbdUser.position);
			InitializeFields("FbdUserPhone", fbdUser.phone);
			InitializeFields("FbdUserEmail", fbdUser.email);
		}
	}
}
