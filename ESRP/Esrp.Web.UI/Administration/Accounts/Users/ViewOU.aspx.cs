using System;
using Esrp.Core;
using Esrp.Core.Organizations;
using Esrp.Core.Systems;
using Esrp.Core.Loggers;
using System.Configuration;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class ViewOU : BasePage
    {
		private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
		private const string LoginQueryKey = "login";
		AdministratorAccount mCurrentUser;

		public AdministratorAccount CurrentUser
		{
			get
			{
				if (mCurrentUser == null)
					mCurrentUser = AdministratorAccount.GetAdministratorAccountForce(Login);

				if (mCurrentUser == null)
					throw new NullReferenceException(String.Format(ErrorUserNotFound, Login));

				return mCurrentUser;
			}
		}

		public string Login
		{
			get
			{
				if (string.IsNullOrEmpty(Request.QueryString[LoginQueryKey]))
					return string.Empty;

				return Request.QueryString[LoginQueryKey];
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// Установлю заголовок страницы)
			this.PageTitle = string.Format("Регистрационные данные “{0}”", CurrentUser.Login);

			Organization org = OrganizationDataAccessor.GetByLogin(User.Identity.Name);
			string res1 = "";
			string res2 = "";
			if (GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbdManager.UserGroupCode))
				res1 = GeneralSystemManager.GetSystemName(3);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["LogUserView"]))
            {
                AccountEventLogger.LogAccountViewEvent(Login);
            }
			if (org != null)
			{
				bool hasAccess = false;
				if (org.OrgType.Id == 1)
					hasAccess = GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbsManager.VuzGroupCode);
				if (org.OrgType.Id == 2)
					hasAccess = GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbsManager.SsuzGroupCode);
				if (org.OrgType.Id == 1)
					hasAccess = GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbsManager.VuzGroupCode);
				if (org.OrgType.Id == 2)
					hasAccess = GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbsManager.SsuzGroupCode);
				if (org.OrgType.Id == 3)
					hasAccess = GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbsManager.InfoProcessingGroupCode);
				if (org.OrgType.Id == 4)
					hasAccess = GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbsManager.DirectionGroupCode);
				if (org.OrgType.Id == 6)
					hasAccess = GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbsManager.FounderGroupCode);
				if (org.OrgType.Id == 5)
					hasAccess = GeneralSystemManager.HasAccessToGroup(CurrentUser.Login, FbsManager.OtherGroupCode);
				if (hasAccess)
					res2 = GeneralSystemManager.GetSystemName(2);
			}
			lbSystemAccess.Text = res1 + (!String.IsNullOrEmpty(res1) ? "<br/>" : "") + res2;
		}
	}
}
