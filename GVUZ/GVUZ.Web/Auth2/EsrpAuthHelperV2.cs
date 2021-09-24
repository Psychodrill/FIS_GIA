using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web.Security;
using FogSoft.Helpers;
using GVUZ.Model.Administration;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.Web.Auth;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Security;
using log4net;
using Institution = GVUZ.Model.Institutions.Institution;
using UserPolicy = GVUZ.Model.Administration.UserPolicy;
using System.Runtime.Remoting.Contexts;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using ISession = FogSoft.Helpers.ISession;
using System.Web.Caching;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.ServiceModel.Export;

namespace GVUZ.Web.Auth
{
    /// <summary>
    /// Авторизация пользователя в ЕСРП. Вторая версия.
    /// Теперь она единственная и окончательная.
    /// </summary>
    public class EsrpAuthHelperV2
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Генерация случайного пароля
        /// </summary>
        private static string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Обновление прав пользователя
        /// </summary>
        public UpdateUserDetailsResult UpdateUserRights(string userLogin)
        {
            int institutionID = 0;
            return UpdateUserRights(userLogin, ref institutionID);
        }

        /// <summary>
        /// Обновление прав пользователя
        /// </summary>
        public UpdateUserDetailsResult UpdateUserRights(string userLogin, ref int institutionID)
        {
            if (ESRPAuthDisabled(userLogin))
                return UpdateUserDetailsResult.Updated;

            int[] filials;
            int instID;
            int? filID;

            try
            {
                LogHelper.Log.DebugFormat("UpdateUserRights-1: Enter");

                string esrpPath = ConfigurationManager.AppSettings["ESRPAuth.Path"];
                if (!esrpPath.EndsWith("/")) esrpPath += "/";


                Log.Debug(String.Format("esrpPath={0}", esrpPath));
                LogHelper.Log.DebugFormat("UpdateUserRights-2: esrpPath: {0}", esrpPath);


                GetEsrpUserData ca = new GetEsrpUserData(esrpPath + "auth/GetData.asmx");
                LogHelper.Log.DebugFormat("UpdateUserRights-3: GetEsrpUserData: {0}", ca != null);

                // получаем данные пользователя из базы
                UserDbData userDbData = GetUserDbData(userLogin);
                Log.Debug(String.Format("userDbData.IsExists={0}", userDbData.IsExists));
                LogHelper.Log.DebugFormat("UpdateUserRights-4: userDbData: {0}", userDbData.IsExists);


                Log.Debug(String.Format("GetActualizationData - Begin(userLogin={0}, UserUpdateDate={1}, OrgUpdateDate={2})", userLogin, userDbData.UserUpdateDate, userDbData.OrgUpdateDate));  
                // получаем данные актуализации из ЕСРП
                var actualizationData = ca.GetActualizationData(userLogin, userDbData.UserUpdateDate, userDbData.OrgUpdateDate,
                    userDbData.ChildrenOrgs,
                    userDbData.FounderOrgUpdateDate,
                    userDbData.MainOrgUpdateDate);
                Log.Debug("GetActualizationData - End"); 

                LogHelper.Log.DebugFormat("UpdateUserRights-5: GetActualizationData: {0}, {1}, {2}, {3}, {4}, {5}, actualizationData: {6}, {7}, {8}, {9}, {10}",    
                        userLogin, 
                        userDbData.UserUpdateDate, 
                        userDbData.OrgUpdateDate,
                        userDbData.ChildrenOrgs,
                        userDbData.FounderOrgUpdateDate,
                        userDbData.MainOrgUpdateDate,
                        actualizationData.ShouldRenewUser,
                        actualizationData.ShouldRenewFounder,
                        actualizationData.ShouldRenewMainOrg,
                        actualizationData.ShouldRenewOrganizations,
                        actualizationData.ShouldRenewOrganization
                    );

                UserDetails userDetails = null;

                //на всякий случай
                if (!userDbData.IsExists)
                {
                    actualizationData.ShouldRenewUser = true;
                    LogHelper.Log.DebugFormat("UpdateUserRights-6: ShouldRenewUser = true");
                }

                //* проверка на наличие subrole у пользователя ОО
                if (userDbData.IsExists && (Roles.GetRolesForUser(userLogin).Length == 1) && (Roles.GetRolesForUser(userLogin)[0] == "fbd_^user") && (userDbData.Subrole == 0))
                {
                    actualizationData.ShouldRenewUser = true;
                    LogHelper.Log.DebugFormat("UpdateUserRights-7: ShouldRenewUser = true");
                }

                // загружаем пользовательские данные из ЕСРП
                // в ЛЮБОМ случае заполняем ...["Subrole"] - для всех, кроме пользователей ОО это 0
                int subrole = 0;
                if (actualizationData.ShouldRenewUser)
                {
                    userDetails = ca.GetUserDetails(userLogin, 3);
                    LogHelper.Log.DebugFormat("UpdateUserRights-8: userDetails: {0}", 
                            userDetails.Email,
                            userDetails.FirstName, 
                            userDetails.Groups.ToString(),
                            userDetails.LastName,
                            userDetails.Login,
                            userDetails.OrganizationID,
                            userDetails.PatronymicName,
                            userDetails.Phone,
                            userDetails.Status,
                            userDetails.UserID
                        );

                    // здесь читаем напрямую из БД
                    using (AdministrationEntities dbContext = new AdministrationEntities())
                    {
                        subrole =
                    dbContext.UserPolicy
                    .Where(x => x.UserName == userLogin && x.InstitutionID != null)
                    .Select(x => x.Subrole)
                    .FirstOrDefault();
                    }
                }
                else
                    subrole = userDbData.Subrole;

                Log.Debug(String.Format("subrole={0}", subrole));

                // ВСЕГДА устанавливаем значение subrole
                if (HttpRuntime.Cache.Get("Subrole") == null)
                    HttpRuntime.Cache.Insert("Subrole", subrole);
                else
                    HttpRuntime.Cache["Subrole"] = subrole;

                Log.Debug(String.Format("userDbData.FilialID={0}", userDbData.FilialID.GetValueOrDefault()));
                // Устаенавливаем значение филиала, к которому относятся роли пользователя ОО, если нужно
                if ((userDbData.IsExists) && (userDbData.FilialID != null))
                    HttpRuntime.Cache.Insert("FilialID", userDbData.FilialID);
                else
                    HttpRuntime.Cache.Insert("FilialID", 0);

                if (userDetails != null && userDetails.OrganizationID.To(0) > 0 && !actualizationData.ShouldRenewOrganization)
                {
                    int? userInstitutionID = GetInstitutionID(userDetails.OrganizationID ?? 0);
                    if (userInstitutionID.To(0) == 0)
                    {
                        actualizationData.ShouldRenewOrganization = true;
                    }
                }
                if (userDetails != null)
                {
                    Log.Debug(String.Format("userDetails.OrganizationID={0}", userDbData.EsrpOrgId));
                }
                else
                {
                    Log.Debug("userDetails=null");
                }
                //пока уберу, из-за этого апдейта падает авторизация
                //if (actualizationData.ShouldRenewOrganization || actualizationData.ShouldRenewFounder || actualizationData.ShouldRenewMainOrg || actualizationData.ShouldRenewOrganizations)
                //{
                //    Log.Debug("UpdateUserOrganization - Begin");  
                //    UpdateUserOrganization(ca, userDbData.EsrpOrgId, actualizationData);
                //    Log.Debug("UpdateUserOrganization - End"); 
                //}

                //запись в таблицу UserPolicy обновленной инфы + установка значения subrole по умолчанию
                if (actualizationData.ShouldRenewUser)
                {
                    Log.Debug("UpdateUserDetails - Begin");  
                    UpdateUserDetails(userLogin, userDetails);
                    Log.Debug("UpdateUserDetails - End"); 
                }

                //Всегда обновляем всех пользователей ОО + филиалов при входе любого пользователя головного ОО
                if (userDbData.FilialID == null)
                {
                    Log.Debug("UpdateAllOrgUsers - Begin");  
                    UpdateAllOrgUsers(userLogin);
                    Log.Debug("UpdateAllOrgUsers - End"); 
                }

                // получаем ID ОО текущего пользователя
                using (AdministrationEntities dbContext = new AdministrationEntities())
                {
                    instID = dbContext.UserPolicy.Where(x => x.UserName == userLogin && x.InstitutionID != null)
                        .Select(x => x.InstitutionID)
                        .FirstOrDefault() ?? 0;
                }


                // проверяем значение institutionID, если отлично от 0 
                if (instID == 0)
                {
                    institutionID = 0;
                }
                else
                {
                    if ((institutionID > 0) && (instID != institutionID))
                    {
                        int tempID = institutionID;
                        filials = InstitutionExporter.GetInstitutionFilials(instID);
                        filID = filials.Where(x => x == tempID).FirstOrDefault();
                        if ((filID == null) || (filID == 0))
                        {
                            Log.Debug("return");
                            return UpdateUserDetailsResult.IncorrectInstution;
                        }
                    }
                    else
                        institutionID = instID;  // если нужно, устанавливаем institutionID из БД по пользователю
                }

                var result= (institutionID > 0 )? UpdateUserDetailsResult.Updated : UpdateUserDetailsResult.NoInstution;
                Log.Debug(String.Format("result={0}", result)); 
                if (result == UpdateUserDetailsResult.Updated)
                {
                    SqlCachedRoleProvider.DropCacheForUser(userLogin);
                }
                Log.Debug("return"); 
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("User Creation Failed ", ex);
                return UpdateUserDetailsResult.Failed;
            }
        }

        /// <summary>
        /// Обновление статусов всех пользователей ОО и филиалов при входе любого пользователя головного ОО
        /// </summary>
        /// <param name="userLogin"></param>
        private void UpdateAllOrgUsers(string userLogin)
        {
            using (var dbContext = new AdministrationEntities())
            {
                string esrpPath = ConfigurationManager.AppSettings["ESRPAuth.Path"];
                if (!esrpPath.EndsWith("/")) esrpPath += "/";
                GetEsrpUserData ca = new GetEsrpUserData(esrpPath + "auth/GetData.asmx");

                var userDetails = ca.GetUserDetails(userLogin, 3);

                // Получим организацию пользователя
                var EduOrg = dbContext.Institution.FirstOrDefault(x => x.EsrpOrgID == userDetails.OrganizationID);
                if (EduOrg == null) return;

                // Получим все подчинённые, включим и головную
                var EduOrgs = dbContext.Institution.Where(x => x.MainEsrpOrgId == EduOrg.EsrpOrgID).ToList();
                EduOrgs.Add(EduOrg);

                // Постороим список пользователей всех организаций
                List<UserPolicy> users = new List<UserPolicy>();

                foreach (var org in EduOrgs)
                    users.AddRange(dbContext.UserPolicy.Where(x => x.InstitutionID == org.InstitutionID));

                foreach (var policy in users)
                {
                    try
                    {
                        var userDet = ca.GetUserDetails(policy.UserName, 3);
                        policy.IsDeactivated = (userDet.Status != "activated");
                    }
                    catch (Exception)
                    {
                        policy.IsDeactivated = true;
                    }
                }

                dbContext.SaveChanges();
            }
            return;
        }

        private class UserDbData
        {
            public bool IsExists { get; set; }
            public DateTime UserUpdateDate { get; set; }
            public DateTime OrgUpdateDate { get; set; }
            public int EsrpOrgId { get; set; }
            public int MainEsrpOrgId { get; set; }
            public int FounderEsrpOrgId { get; set; }
            public DateTime MainOrgUpdateDate { get; set; }
            public DateTime FounderOrgUpdateDate { get; set; }
            public int ChildrenOrgs { get; set; }
            public int Subrole { get; set; }
            public int? FilialID { get; set; }
        }

        /// <summary>
        /// Получение текущего статуса пользователя из базы
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        private static UserDbData GetUserDbData(string userLogin)
        {
            UserDbData dbData = new UserDbData();
            using (AdministrationEntities dbContext = new AdministrationEntities())
            {
                var userUpdData = dbContext.UserPolicy.Where(x => x.UserName == userLogin)
                    .Select(x => new
                    {
                        UDateUpdated = x.DateUpdated,
                        IDateUpdated = x.Institution.DateUpdated,
                        EsrpOrgId = x.Institution.EsrpOrgID,
                        x.Institution.MainEsrpOrgId,
                        x.Institution.FounderEsrpOrgId,
                        Subrole = x.Subrole,
                        FilialID = x.FilialID
                    }).FirstOrDefault();
                if (userUpdData != null)
                {
                    dbData.UserUpdateDate = userUpdData.UDateUpdated ?? DateTime.MinValue;
                    dbData.OrgUpdateDate = userUpdData.IDateUpdated ?? DateTime.MinValue;
                    dbData.EsrpOrgId = userUpdData.EsrpOrgId ?? 0;
                    dbData.MainEsrpOrgId = userUpdData.MainEsrpOrgId ?? 0;
                    dbData.FounderEsrpOrgId = userUpdData.FounderEsrpOrgId ?? 0;
                    dbData.Subrole = userUpdData.Subrole;
                    dbData.FilialID = userUpdData.FilialID;
                    dbData.IsExists = true;
                    //считаем головные организации, основателей и филиалы
                    if (dbData.MainEsrpOrgId > 0)
                        dbData.MainOrgUpdateDate = dbContext.Institution
                            .Where(x => x.EsrpOrgID == dbData.MainEsrpOrgId)
                            .Select(x => x.DateUpdated).FirstOrDefault() ?? DateTime.MinValue;
                    if (dbData.FounderEsrpOrgId > 0)
                        dbData.FounderOrgUpdateDate = dbContext.Institution
                            .Where(x => x.EsrpOrgID == dbData.FounderEsrpOrgId)
                            .Select(x => x.DateUpdated).FirstOrDefault() ?? DateTime.MinValue;
                    //if(dbData.MainEsrpOrgId > 0)
                    dbData.ChildrenOrgs = dbContext.Institution.Where(x => x.MainEsrpOrgId == dbData.EsrpOrgId && x.EsrpOrgID != dbData.EsrpOrgId).Count();
                    //var chorg = dbContext.Institution.Where(x => x.MainEsrpOrgId == dbData.MainEsrpOrgId).Count();
                }
                else
                {
                    dbData.IsExists = false;
                }
            }

            return dbData;
        }

        /// <summary>
        /// Обновление информации о пользователе (в таблице UserPolicy, по данным ЕСРП)
        /// </summary>
        private static void UpdateUserDetails(string userLogin, UserDetails userDetails)
        {
            using (AdministrationEntities dbContext = new AdministrationEntities())
            {
                UserPolicy user = dbContext.UserPolicy.Where(x => x.UserName == userLogin).FirstOrDefault();
                if (user == null)
                {
                    user = new UserPolicy();
                    user.UserName = userDetails.Login;
                    MembershipUserCollection membershipUsers = Membership.FindUsersByName(userDetails.Login);
                    MembershipUser membershipUser;
                    // создаём aspnet пользователя со случайным паролем
                    if (membershipUsers.Count == 0)
                        membershipUser = Membership.CreateUser(userDetails.Login, GenerateRandomPassword(), userDetails.Login);
                    else
                        membershipUser = membershipUsers[userDetails.Login];

                    //в базе гуид
                    user.UserID = (Guid)membershipUser.ProviderUserKey;
                    dbContext.UserPolicy.AddObject(user);
                }

                string oldFullName = user.FullName;
                user.FullName = userDetails.LastName + " " + userDetails.FirstName + " " + userDetails.PatronymicName;
                user.PhoneNumber = userDetails.Phone;
                user.IsReadOnly = userDetails.Status == "readonly";

                // проставляем роли для пользователя
                string[] rolesForUser = Roles.GetRolesForUser(user.UserName);
                if (rolesForUser.Length > 0)
                    Roles.RemoveUserFromRoles(user.UserName, rolesForUser);
                if (userDetails.Groups.Length > 0)
                    Roles.AddUserToRoles(user.UserName, userDetails.Groups.Select(x => x.Code).ToArray());
                //if (user.IsReadOnly)
                //    Roles.AddUserToRoles(user.UserName, new[] { UserRole.FBDReadonly });
                user.IsMainAdmin = userDetails.Groups.Select(x => x.Code).Contains(UserRole.FBDAdmin);
                user.InstitutionID = GetInstitutionID(userDetails.OrganizationID ?? 0);
                if (user.InstitutionID == 0) //тут мы сохранить не сможем, так что пишем в логи и откатываемся
                {
                    Log.ErrorFormat("User: {0} with OrgID: {1} has missing institution.", userDetails.Login, userDetails.OrganizationID);
                    throw new ArgumentException("Invalid user institution data. " + user.UserName);
                }

                // если нет пока ролей для пользователя ОО - устанавливаем значение по умолчанию, записываем его в БД и в контекст
                int subrole = 0;
                // здесь значение Subrole д.б. уже установлено
                if (HttpRuntime.Cache.Get("Subrole") != null)
                    subrole = (int)HttpRuntime.Cache.Get("Subrole");
                if ((userDetails.Groups.Length == 1) && (userDetails.Groups[0].Code == "fbd_^user") && (subrole == 0))
                {
                    user.Subrole = (int)FBDUserSubroles.ApplicationsDirection;
                    if (HttpRuntime.Cache.Get("Subrole") == null)
                        HttpRuntime.Cache.Insert("Subrole", (int)FBDUserSubroles.ApplicationsDirection);
                    else
                        HttpRuntime.Cache["Subrole"] = (int)FBDUserSubroles.ApplicationsDirection;
                }

                if ((userDetails.Groups.Length == 1) && (userDetails.Groups[0].Code == "fbd_^authorizedstaff") && (subrole == 0))
                {
                    user.Subrole = 0;
                    if (HttpRuntime.Cache.Get("Subrole") == null)
                        HttpRuntime.Cache.Insert("Subrole", 0);
                    else
                        HttpRuntime.Cache["Subrole"] = 0;
                }

                user.DateUpdated = DateTime.Now.AddMinutes(5); //на случай возможной рассинхронизации времени
                dbContext.SaveChanges();
                dbContext.AddToLog(oldFullName, user.FullName, user.InstitutionID, user.UserName);
            }
        }

        private static int? GetInstitutionID(int esrpOrgID)
        {
            if (esrpOrgID == 0)
                return null;
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.Institution.Where(x => x.EsrpOrgID == esrpOrgID).Select(x => x.InstitutionID).FirstOrDefault();
            }
        }

        /// <summary>
        /// Обновление организации пользователя
        /// </summary>
        private static void UpdateUserOrganization(GetEsrpUserData ca, int userOrgId, ActualizationDataExtended actData)
        {
            // в зависимости от требуемых параметров ставим нужные флажки
            int updateFlags = 0;
            if (actData.ShouldRenewOrganization) updateFlags |= 1;
            if (actData.ShouldRenewOrganizations) updateFlags |= 2;
            if (actData.ShouldRenewFounder) updateFlags |= 4;
            if (actData.ShouldRenewMainOrg) updateFlags |= 8;
            OrganizationData[] organizationDatas = ca.GetOrganizationData(userOrgId, updateFlags);
            //no more doubles and nulls
            organizationDatas = organizationDatas.Where(x => x != null).GroupBy(x => x.ID).Select(x => x.First()).ToArray();
            var parsedInsts = new List<int>();
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                foreach (OrganizationData orgDetails in organizationDatas)
                {
                    OrganizationData details = orgDetails;
                    if (details == null) continue;
                    Institution inst = dbContext.Institution.Where(x => x.EsrpOrgID == details.ID).FirstOrDefault();
                    if (inst == null)
                    {
                        inst = new Institution();
                        dbContext.Institution.AddObject(inst);
                        inst.EsrpOrgID = details.ID;
                        InstitutionItem ii = new InstitutionItem();
                        ii.Institution = inst;
                        ii.ItemTypeID = 0;
                        ii.BriefName = "Нет";
                        ii.Name = "Нет";
                        InstitutionStructure ins = new InstitutionStructure();
                        ins.InstitutionItem = ii;
                    }
                    else
                    {
                        parsedInsts.Add(inst.InstitutionID);
                    }
                    //inst.Address = details.FactAddress;
                    inst.BriefName = details.ShortName;
                    inst.FullName = details.FullName;
                    //inst.Fax = details.Fax;
                    inst.FormOfLawID = details.KindId;
                    inst.INN = details.INN;
                    inst.InstitutionTypeID = (short)(details.TypeId == 2 ? 2 : 1); //другие типы не должны быть
                    inst.OGRN = details.OGRN;
                    inst.OwnerDepartment = details.OwnerDepartment;
                    //inst.Phone = details.Phone;
                    //inst.Site = details.Site;
                    int regionID = dbContext.RegionType.Where(x => x.EsrpCode == details.Region.Code).Select(x => x.RegionId).FirstOrDefault();
                    inst.RegionID = regionID > 0 ? regionID : (int?)null;
                    // Исправлено (было наоборот MainEsrpOrgId <> FounderEsrpOrgId)
                    inst.MainEsrpOrgId = details.MainID == details.ID ? 0 : details.MainID;
                    inst.FounderEsrpOrgId = details.DepartmentID == details.ID ? 0 : details.DepartmentID;
                    inst.DateUpdated = DateTime.Now.AddMinutes(5);

                    int newStatus = ca.GetOrgStatus(orgDetails.ID);
                    if (newStatus != -1) inst.StatusId = newStatus;
                }

                dbContext.SaveChanges();
                parsedInsts.ForEach(dbContext.AddChangesToHistory);
            }
        }

        /// <summary>
        /// Обновляем статус пользователя в ЕСРП (можем менять ридонли)
        /// </summary>
        public string UpdateUserStatus(string userLogin, string newStatus)
        {
            string esrpPath = ConfigurationManager.AppSettings["ESRPAuth.Path"];
            if (!esrpPath.EndsWith("/")) esrpPath += "/";
            GetEsrpUserData ca = new GetEsrpUserData(esrpPath + "auth/GetData.asmx");
            try
            {
                return ca.UpdateUserStatus(userLogin, newStatus);
            }
            catch (Exception ex)
            {
                Log.Error("Update user status error", ex);
            }

            return "Ошибка изменения статуса";
        }

        /// <summary>
        /// Ссылка для обновления в ЕСРП, чтобы оно не забывало пользователя
        /// </summary>
        public static string GetRenewLink()
        {
            if (ESRPAuthDisabled(""))
                return String.Empty;

            string esrpPath = ConfigurationManager.AppSettings["ESRPAuth.Path"];
            if (!esrpPath.EndsWith("/")) esrpPath += "/";
            return esrpPath + "auth/renew.ashx";
        }

        /// <summary>
        /// Признак отключения аутентификации через ЕСРП
        /// (ИСПОЛЬЗОВАТЬ ТОЛЬКО ДЛЯ ОТЛАДКИ)
        /// </summary>
        public static bool ESRPAuthDisabled(string login)
        {
            var isDisabled = (ConfigurationManager.AppSettings["ESRPAuth.Disabled"] == "true")
                    || (ConfigurationManager.AppSettings["ESRPAuth.Disabled"] == "True");

            var usersHardcoded = ConfigurationManager.AppSettings["ESRPAuth.UsersHardcoded"];
            var isUserHardcoded = false;
            if (!string.IsNullOrWhiteSpace(usersHardcoded) && !string.IsNullOrWhiteSpace(login))
            {
                var users = usersHardcoded.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                isUserHardcoded = users.Any(t => t.ToLower() == login.ToLower());
            }

            return isDisabled || isUserHardcoded;

            //get
            //{
            //    return (ConfigurationManager.AppSettings["ESRPAuth.Disabled"] == "true")
            //        || (ConfigurationManager.AppSettings["ESRPAuth.Disabled"] == "True");
            //}
        }
    }
}