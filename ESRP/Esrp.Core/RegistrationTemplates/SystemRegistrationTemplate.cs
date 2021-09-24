using System.Collections.Generic;
using Esrp.Core.Systems;
using Esrp.Core.Users;

namespace Esrp.Core.RegistrationTemplates
{
	public class SystemRegistrationTemplate : OrganizationCommonInfoTemplate
	{
		public SystemRegistrationTemplate(OrgUser user, SystemKind systemKind, string headerBlock)
			: this(user, new List<SystemKind>(new SystemKind[] {systemKind}), headerBlock)
		{
		}

		public SystemRegistrationTemplate(OrgUser user, List<SystemKind> systemKinds, string headerBlock)
			: base(user, headerBlock, System.Configuration.ConfigurationManager.AppSettings["SystemRegistrationTemplateFileName"])
		{
			InitializeFields("User", user.GetFullName());
			InitializeFields("UserPosition", user.position);
			InitializeFields("UserPhone", user.phone);
			InitializeFields("UserEmail", user.email);

			string systemName = "";
			if (systemKinds.Contains(SystemKind.Fbs))
				systemName = GeneralSystemManager.GetSystemName(2)+ ", ";
			if(systemKinds.Contains(SystemKind.Fbd))
				systemName += "«" + GeneralSystemManager.GetSystemName(3) + "»";
			systemName = systemName.TrimEnd(' ', ',');
			
			InitializeFields("SystemName", systemName);
		}
	}
}
