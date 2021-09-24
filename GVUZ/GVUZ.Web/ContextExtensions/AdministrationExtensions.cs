using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using GVUZ.Model.Administration;
using GVUZ.Web.Auth;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels.Administration;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с пользователями. Практически не используются, т.к. авторизация сейчас через ЕСРП
	/// </summary>
	public static class AdministrationExtensions
	{
		/// <summary>
		/// Список пользователей
		/// </summary>
		public static UserListViewModel GetUsersList(this AdministrationEntities context, int institutionID,
			string userOffice, int sortID)
		{
			UserListViewModel viewModel = new UserListViewModel { SortID = sortID };

			IQueryable<aspnet_Users> queryable = context.aspnet_Users.Include(u => u.UserPolicy);

			if (userOffice == UserOffice.Fbd)
			{
				queryable = queryable.Where(u => u.UserPolicy.InstitutionID == institutionID &&
				                     u.aspnet_Roles.Any(r => r.LoweredRoleName.StartsWith(UserOffice.Fbd.ToLower()) ||
				                                             r.LoweredRoleName.StartsWith(UserOffice.Edu.ToLower())));
			}
			else if (userOffice == UserOffice.Ron)
			{
				queryable = queryable.Where(u => u.UserPolicy.InstitutionID == institutionID &&
														 u.aspnet_Roles.Any(r => r.LoweredRoleName.StartsWith(UserOffice.Ron.ToLower())));
			}
			else
			{
				throw new ArgumentException("Указан некорректный префикс системы для фильтрации пользователей", "userOffice");
			}

            queryable = queryable.Where(up => (up.UserPolicy.IsDeactivated == null || !up.UserPolicy.IsDeactivated.Value));

			IQueryable<UserViewModel> uvmQueryable = queryable.Select(u => new UserViewModel
					{
						UserID = u.UserId,
						Login = u.UserName,
						FullName = u.UserPolicy.FullName,
						Position = u.UserPolicy.Position,
						Aspnet_Roles = u.aspnet_Roles
					});

			if (sortID == 1) uvmQueryable = uvmQueryable.OrderBy(x => x.Login);
			if (sortID == -1) uvmQueryable = uvmQueryable.OrderByDescending(x => x.Login);
			if (sortID == 2) uvmQueryable = uvmQueryable.OrderBy(x => x.FullName);
			if (sortID == -2) uvmQueryable = uvmQueryable.OrderByDescending(x => x.FullName);

			viewModel.Users = uvmQueryable.ToArray();
			return viewModel;
		}

		/// <summary>
		/// Список существующих в системе ролей
		/// </summary>
		public static string[] GetExistingRolesStrings(this AdministrationEntities context, bool withPrefix = false)
		{
			return GetRoles(context.aspnet_Roles.ToList());
		}

		/// <summary>
		/// Получить название ролей из базы
		/// </summary>
		public static string[] GetRoles(List<aspnet_Roles> rolesCollection, bool withPrefix = false)
		{
			Office office = UserOffice.GetOffice();
			string[] roles = new string[0];

			if (office == Office.Ron)
			{
				roles = rolesCollection
					.Where(x => x.RoleName.StartsWith(UserOffice.Ron))
					.Select(x => x.RoleName).ToArray();
			}

			if (office == Office.Edu)
			{
				roles = rolesCollection
					.Where(x => x.RoleName.StartsWith(UserOffice.Edu))
					.Select(x => x.RoleName).ToArray();
			}

			if (!withPrefix)
				return roles.Select(UserRole.GetRoleUIName).ToArray();

			return roles;
		}

		/// <summary>
		/// Список существующих ролей
		/// </summary>
		public static List<UserRole> GetExistingRoles(this AdministrationEntities context)
		{
			List<UserRole> resultList = new List<UserRole>();

			foreach (var roleName in GetExistingRolesStrings(context, withPrefix: true))
				resultList.Add(new UserRole(roleName));

			return resultList;
		}

		/// <summary>
        /// Сохранение пользователя. По факту сейчас меняются только UserPolicy.Subrole & UserPolicy.FilialID
		/// </summary>
		/// <returns>null - в случае успеха, иначе текст ошибки</returns>
		public static string SaveUser(this AdministrationEntities context, UserViewModel model, int institutionID)
		{
			// полное сохранение пользователя
			bool isAdd = model.UserID == Guid.Empty;

			if (isAdd)
			{
				MembershipUser user = Membership.CreateUser(model.EmailLogin, model.Password, model.EmailLogin);				
				if (user.ProviderUserKey != null) model.UserID = (Guid)user.ProviderUserKey;
				else
					throw new Exception("Can't get user id for just generated user.");
			}

			UserPolicy userPolicy = isAdd ? new UserPolicy() :
				context.UserPolicy.Include(x => x.aspnet_Users.aspnet_Membership).First(x => x.UserID == model.UserID);

			userPolicy.Comment = model.Comment;
			userPolicy.FullName = model.FullName;
			userPolicy.InstitutionID = institutionID;
			userPolicy.IsMainAdmin = false;
			userPolicy.PhoneNumber = model.Phone;
			userPolicy.Position = model.Position;
			userPolicy.UserID = model.UserID;
			userPolicy.UserName = model.EmailLogin;
            userPolicy.Subrole = model.Subroles;
            userPolicy.FilialID = model.FilialID;

            // От поля 'IsReadOnly' нужно, видимо, избавляться...

			if (!isAdd)
				userPolicy.aspnet_Users.aspnet_Membership.IsLockedOut = model.GetStatusIsBlocked();
            //bool isReadonlyChanged = model.IsReadOnly != userPolicy.IsReadOnly;
            //userPolicy.IsReadOnly = model.IsReadOnly;
            //if (isReadonlyChanged && !isAdd)
            //{
            //    if (userPolicy.IsReadOnly)
            //    {
            //        Roles.AddUserToRoles(userPolicy.UserName, new[] { UserRole.FBDReadonly });
            //        string res = EsrpAuthHelperFactory.GetAuthHelper().UpdateUserStatus(userPolicy.UserName, "readonly");
            //        if (res != null)
            //            return res;
            //    }
            //    else
            //    {
            //        Roles.RemoveUserFromRole(userPolicy.UserName, UserRole.FBDReadonly);
            //        string res = EsrpAuthHelperFactory.GetAuthHelper().UpdateUserStatus(userPolicy.UserName, "activated");
            //        if (res != null)
            //            return res;
            //    }
            //}

			if (isAdd)
				context.UserPolicy.AddObject(userPolicy);

			SaveUserRoles(model);

			context.SaveChanges();
			return null;
		}

		/// <summary>
		/// Сохранение ролей пользователя
		/// </summary>
		public static void SaveUserRoles(UserViewModel model)
		{
			// если пользователю меняют роли, то не вышибать ему существующие у него роли
			string[] currentRoles = UserRole.GetUserRolesForCurrentOffice(model.EmailLogin);

			if (model.ExistingRoles != null)
				foreach (string role in model.ExistingRoles.Intersect(currentRoles))
					Roles.RemoveUserFromRole(model.EmailLogin, UserRole.ApplyPrefix(role));

			if (model.AssignedRoles != null)
			{
				string[] newRoles = model.AssignedRoles.Except(currentRoles).ToArray();
				if (newRoles.Length > 0)
					// нельзя давать в membership пустой массив с ролями
					Roles.AddUserToRoles(model.EmailLogin, UserRole.ApplyPrefix(newRoles));
			}
		}

		/// <summary>
		/// Удаление пользователя
		/// </summary>
		public static void DeleteUser(this AdministrationEntities context, Guid userID)
		{
			/*aspnet_Users aspnetUsers = context.aspnet_Users.Where(x => x.UserId == userID).First();
			string userName = aspnetUsers.UserName;*/

			UserPolicy userPolicy = context.UserPolicy.First(x => x.UserID == userID);			
			string userName = userPolicy.UserName;

			context.UserPolicy.DeleteObject(userPolicy);
			context.SaveChanges();
			Membership.DeleteUser(userName);
		}

		/// <summary>
		/// Загрузить из базы view-модель пользователя
		/// </summary>
		public static UserViewModel FillUserModel(this AdministrationEntities context, Guid userID)
		{
			UserViewModel viewModel = new UserViewModel();
			viewModel.UserID = userID;
			UserPolicy userPolicy = context.UserPolicy
				.Include(x => x.aspnet_Users.aspnet_Membership)
				.Include(x => x.aspnet_Users.aspnet_Roles)
				.First(x => x.UserID == userID);			

			viewModel.FullName = userPolicy.FullName;
			viewModel.Position = userPolicy.Position;
			viewModel.EmailLogin = userPolicy.UserName;
			viewModel.Phone = userPolicy.PhoneNumber;
			viewModel.Comment = userPolicy.Comment;			
			viewModel.SetStatus(userPolicy.aspnet_Users.aspnet_Membership.IsLockedOut);
			viewModel.IsReadOnly = userPolicy.IsReadOnly;
            viewModel.Subroles = userPolicy.Subrole;
            viewModel.FilialID = userPolicy.FilialID;

			// заполнить роли пользователя
			viewModel.AssignedRoles = GetRoles(userPolicy.aspnet_Users.aspnet_Roles.ToList());
			viewModel.ExistingRoles = GetExistingRolesStrings(context).Except(viewModel.AssignedRoles).ToArray();			

			return viewModel;
		}
	}
}