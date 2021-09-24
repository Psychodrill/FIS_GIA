using System.Security.Principal;
using System.Web.Mvc;
using GVUZ.Web.Security;

namespace GVUZ.Web.Helpers
{
	/// <summary>
	/// Атрибут авторизации для администратора, эмулирующего сотрудника ОО
	/// </summary>
	public class AuthorizeAdmAttribute : AuthorizeAttribute
	{
		private string[] _splittedRoles;

		protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
		{
			var isAuthorized = base.AuthorizeCore(httpContext);
#if DEBUG
            #warning ВНИМАНИЕ! В отладке авторизация всегда успешна!
            isAuthorized = true;
#endif
            if (isAuthorized)
				return true;
			var user = httpContext.User;
			if (user.Identity.IsAuthenticated)
			{
				//добавляем к пользователю роли
				if (_splittedRoles == null)
				{
					_splittedRoles = Roles.Split(',');
				}

				if (IsAdminEmulatesInstitution(user, _splittedRoles)) return true;
			}

			return false;
		}

		public static bool IsAdminEmulatesInstitution(IPrincipal user, string[] roles)
		{
			return UserRole.IsAdminEmulatesInstitution(user, roles);
		}
	}
}
