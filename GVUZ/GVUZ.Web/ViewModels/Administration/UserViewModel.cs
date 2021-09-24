using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using FogSoft.Web.Mvc;
using GVUZ.Model.Administration;
using System.Linq;
using GVUZ.Web.Security;
using GVUZ.Web.Helpers;
using GVUZ.Model.Institutions;

namespace GVUZ.Web.ViewModels.Administration
{
	public class UserListViewModel
	{		
		public UserViewModel DisplayModel;
		public UserViewModel[] Users;
		public int SortID = 1;
	}

    public class baseData
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }


	public class UserViewModel
	{		
		[ScriptIgnore]
		public static string Active = "Действующий";
		[ScriptIgnore]
		public static string Blocked = "Заблокирован";

		public Guid UserID { get; set; }

		[DisplayName("Логин")]		
		public string Login { get; set; }
		[DisplayName("Ф.И.О.")]
		[LocalRequired]
		public string FullName { get; set; }
		[DisplayName("Должность")]
		public string Position { get; set; }
		[DisplayName("Логин")]
		[LocalRequired]
		public string EmailLogin { get; set; }
		[DisplayName("Телефон")]
		public string Phone { get; set; }
		[DisplayName("Комментарий")]
		public string Comment { get; set; }
		[DisplayName("Пароль")]
		[LocalRequired]
		public string Password { get; set; }
		[DisplayName("Статус")]
		public string StatusID { get; set; }
		[DisplayName("Статус")]
		public string Status { get; set; }

		[DisplayName("Роли")] 
		public List<string> RolesDict { get; set; }

		[DisplayName("Имеющиеся роли")]
		public string[] ExistingRoles { get; set; }

		[DisplayName("Назначенные роли")]
		public string[] AssignedRoles { get; set; }

		[DisplayName("Только чтение")]
		public bool IsReadOnly { get; set; }


        // Роли пользователя ОО
        public int Subroles { get; set; }

        // Разрешенный филиал для пользователя ОО
        public int? FilialID { get; set; }


        public string MainRole
        {
            get
            {
                string strCore = string.IsNullOrEmpty(Login) ? EmailLogin : Login;
                if (string.IsNullOrEmpty(strCore))
                    return string.Empty;

                string[] roles = Roles.GetRolesForUser(strCore);
                if ((roles.Length == 1) && (roles[0] == "fbd_^user"))
                    return "User";

                for (int i = 0; i < roles.Length; i++)
                {
                    if (roles[i] == "fbd_^administrator") 
                        return "Администратор системы";
                    if (roles[i] == "fbd_^authorizedstaff")
                        return "Администратор ОО";
                    if (roles[i] == "fbd_^ronuser")
                        return "Пользователь РОН";
                }
                return "Не определены";
            }
        }

        public string roles
        {
            get
            {
                string strCore = string.IsNullOrEmpty(Login) ? EmailLogin : Login;
                if (string.IsNullOrEmpty(strCore))
                    return string.Empty;


                string[] roles = Roles.GetRolesForUser(strCore);
                string res = string.Empty;                
                


                for (int i = 0; i < roles.Length; i++)
                {
                    if (roles[i] == "fbd_^user")
                      res+="Пользователь ОО";
                    if (roles[i] == "fbd_^administrator")
                      res+="Администратор системы";
                    if (roles[i] == "fbd_^authorizedstaff")
                      res+="Администратор ОО";
                    if (roles[i] == "fbd_^ronuser")
                      res+="Пользователь РОН";

                    if (i != roles.Length - 1)
                        res += ", ";
                }

                return res;
            }
        }


		
		[ScriptIgnore]
		public EntityCollection<aspnet_Roles> Aspnet_Roles { get; set; }

		public string GetUserRoles()
		{
			if(UserOffice.GetOffice() == Office.Edu)
				return String.Join("; ", Aspnet_Roles.Where(
					r => r.RoleName.StartsWith(UserOffice.Edu)).Select(x => UserRole.GetRoleUIName(x.RoleName)).ToArray());
			if(UserOffice.GetOffice() == Office.Ron)
				return String.Join("; ", Aspnet_Roles.Where(
					r => r.RoleName.StartsWith(UserOffice.Ron)).Select(x => UserRole.GetRoleUIName(x.RoleName)).ToArray());            
			return "";
		}

		public static SelectListItem[] GetStatusesList()
		{
			return new[] { 
				new SelectListItem { Value = "Active", Text = Active }, 
				new SelectListItem { Value = "Blocked", Text = Blocked } };
		}

		public void SetStatus(bool isLockedOut)
		{			StatusID = isLockedOut ? "Blocked" : "Active";
			Status = isLockedOut ? Blocked : Active;
		}

		public bool GetStatusIsBlocked()
		{
			return StatusID == "Blocked";
		}

        public int? GetUserFilial ()
        {
            string strLogin = string.IsNullOrEmpty(Login) ? EmailLogin : Login;
            if (string.IsNullOrEmpty(strLogin))
                return null;

            using (AdministrationEntities dbContext = new AdministrationEntities())
			{
  				GVUZ.Model.Administration.UserPolicy user = dbContext.UserPolicy.Where(x => x.UserName == strLogin).FirstOrDefault();
                if (user != null)
                    return user.FilialID;
             }
            return null;
		}

        public baseData[] GetFilials()
        {
            
            if (InstitutionHelper.GetInstitutionID() != InstitutionHelper.MainInstitutionID)
                return null; // сами сидим на филиале, здесь не может быть филиалов

            var filials = new baseData[0];
            int institutionID = InstitutionHelper.MainInstitutionID;

            using (InstitutionsEntities context = new InstitutionsEntities())
            {
                int? esrpOrgID = context.Institution.Where(x => x.InstitutionID == institutionID).Select(y => y.EsrpOrgID).FirstOrDefault();
                if ((esrpOrgID != null) && (esrpOrgID != 0))
                {
                    filials = context.Institution.Where(x => x.MainEsrpOrgId == esrpOrgID).Select(x => new baseData
                    { ID = x.InstitutionID, Name = x.BriefName } ).ToArray();
                }
            }

            return filials;

        }


	}
}