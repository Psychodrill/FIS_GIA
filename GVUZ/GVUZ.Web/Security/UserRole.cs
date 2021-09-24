using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Linq;
using GVUZ.Model.Helpers;
using GVUZ.Web.Helpers;
using GVUZ.Model.Cache;

namespace GVUZ.Web.Security
{
	[Flags]
	public enum Role
	{
		EduAdm = 1,
		EduUsr = 2,
		RonAdm = 4,
		RonNsi = 8,		
		RonInst = 16,
		RonCamp = 32,
		RonFbdUser = 64,
        EduAutoOrders = 128
	}

	public enum Office
	{
		None = 0,
		Edu = 1,
		Ron = 2,
		Fbd = 3
	}


	public class UserOffice
	{
		public const string Ron = "РОН^";
		public const string Edu = "_ОУ^";
		public const string Fbd = "fbd_^";

		public static Office GetOffice()
		{
			string[] rolesForUser = Roles.GetRolesForUser();
			if (rolesForUser.Length == 0) return Office.None;
			if (rolesForUser[0].StartsWith(Edu)) return Office.Edu;
			if (rolesForUser[0].StartsWith(Ron)) return Office.Ron;
			if (rolesForUser[0].StartsWith(Fbd)) return Office.Fbd;
			return Office.None;
		}

		public static string GetOfficePrefix()
		{
			switch (GetOffice())
			{
				case Office.Edu: return Edu;
				case Office.Ron: return Ron;
				case Office.Fbd: return Fbd;
				default: return null;
			}
		}
	}

	public class UserRole
	{
		private readonly Role _roles;

		public const Role AdminUsrEduRole = Role.EduAdm | Role.EduUsr;

		public const string FBDAdmin = UserOffice.Fbd + "administrator";
		public const string FBDReadonly = UserOffice.Fbd + "readonly";
		public const string EduAdminOnly = UserOffice.Edu + "Администратор";
		public const string EduAdmin = FBDAdmin + "," + EduAdminOnly + "," + FbdAuthorizedStaff;
		public const string FbdUser = UserOffice.Fbd + "user";
		public const string FbdAuthorizedStaff = UserOffice.Fbd + "authorizedstaff";
        public const string FbdAuthorizedStaffOlympic = UserOffice.Fbd + "authorizedstaffolympic";
		public const string FbdRonUser = UserOffice.Fbd + "ronuser";
		public const string EduUserOnly = UserOffice.Edu + "Пользователь";

        public const string EduAutoOrders = UserOffice.Edu + "Автоприказы";

        public const string EduUser = EduUserOnly + "," /*+ EduAdmin + ","*/ + FbdUser + "," + FbdAuthorizedStaff;

		public const string RonAdmin = UserOffice.Ron + "Администратор";
		public const string RonNsi = UserOffice.Ron + "Оператор НСИ";
		public const string RonInst = UserOffice.Ron + "Оператор ОО";
		public const string RonCamp = UserOffice.Ron + "Оператор ПК";
		public const string EduUserAndRonInst = EduUser + "," + RonInst;

		public UserRole(Role roles)
		{
			_roles = roles;
		}

		public UserRole(string roleName)
		{
			switch (roleName)
			{
				case EduAdmin: _roles = Role.EduAdm; break;
				case EduUser: _roles = Role.EduUsr; break;
				case RonAdmin: _roles = Role.RonAdm; break;
				case RonCamp: _roles = Role.RonCamp; break;
				case RonInst: _roles = Role.RonInst; break;
				case RonNsi: _roles = Role.RonNsi; break;
				case FbdRonUser: _roles = Role.RonFbdUser; break;
                case EduAutoOrders: _roles = Role.EduAutoOrders; break;
			}
		}

		public string GetStr()
		{
			List<string> list = new List<string>();
			if ((_roles & Role.EduAdm) > 0) list.Add(EduAdmin);
			if ((_roles & Role.EduUsr) > 0) list.Add(EduUser);
			if ((_roles & Role.RonAdm) > 0) list.Add(RonAdmin);
			if ((_roles & Role.RonNsi) > 0) list.Add(RonNsi);
			if ((_roles & Role.RonInst) > 0) list.Add(RonInst);
			if ((_roles & Role.RonCamp) > 0) list.Add(RonCamp);
			if ((_roles & Role.RonFbdUser) > 0) list.Add(FbdRonUser);
            if ((_roles & Role.EduAutoOrders) > 0) list.Add(EduAutoOrders);

            string rolesStr = String.Join(",", list);
			// могут быть дубликаты ролей, убираем их
			return String.Join(",", rolesStr.Split(',').Distinct());
		}

		public string[] GetRoleNames()
		{
			return GetStr().Split(',');
		}

		public string StringValue()
		{
			return StringValue(_roles);
		}

		public bool IsUserInRole()
		{
			return GetRoleNames().Any(roleName => Roles.IsUserInRole(UserHelper.GetAuthenticatedUserName(), roleName));
		}

		public static string[] GetUserRolesForCurrentOffice(string userName)
		{
			string[] rolesForUser = Roles.GetRolesForUser(userName)
				.Where(x => x.StartsWith(UserOffice.GetOfficePrefix())).ToArray();
			return CutPrefix(rolesForUser);
		}

		public static implicit operator UserRole(string roleName)
		{
			return new UserRole(roleName);
		}

		public static string StringValue(Role role)
		{
			return new UserRole(role).GetStr();
		}

		public static string GetRoleUIName(string roleName)
		{
			return roleName.Substring(4);
		}

		public static string ApplyPrefix(string role)
		{
			return UserOffice.GetOfficePrefix() + role;
		}

		public static string[] ApplyPrefix(string[] roles)
		{
			return roles.Select(ApplyPrefix).ToArray();
		}

		public static string[] CutPrefix(string[] roles)
		{
			return roles.Select(GetRoleUIName).ToArray();
		}

		public static bool CurrentUserInRole(string rolesList)
		{
			string[] roles = rolesList.Split(',');
			if (WebCacheManager.IsUserInRole(roles))
				return true;
			return IsAdminEmulatesInstitution(HttpContext.Current.User, roles);
		}

		private static readonly string[] _fbdUserRoles = EduUser.Split(',');

		public static bool IsAdminEmulatesInstitution(IPrincipal user, string[] roles)
		{
			if ((user.IsInRole(FBDAdmin) || user.IsInRole(FbdRonUser)) && InstitutionHelper.GetInstitutionID(false) > 0)
			{
				if (roles.Intersect(_fbdUserRoles).Any())
					return true;
			}

			return false;
		}

		public static bool IsCurrentUserReadonly()
		{
			return CurrentUserInRole(FBDReadonly) || CurrentUserInRole(FbdRonUser);
		}
	}
}
