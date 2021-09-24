using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using FogSoft.Helpers;
using GVUZ.Model.Helpers;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace GVUZ.Model.Cache
{
    public class WebCacheManager 
    {
        private static readonly ICacheManager userRolesCache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>("UserRoleCache");

        public static bool IsUserInRole(string[] roles)
        {
            lock (userRolesCache)
            {
                try
                {
                    var userId = UserHelper.GetCurrentUserID();
                    /* Если такого человека не нашли - ясно что ему ничего нельзя */
                    if (string.IsNullOrEmpty(userId)) return false;

                    /* Если нет у нас в кеше этого парня - бегаем в цикле проверяем,
                     * иначе не тратим на это ресурсы */
                    if (!userRolesCache.Contains(userId))
                    {
                        var allroles = Roles.GetRolesForUser(UserHelper.GetAuthenticatedUserName());
                        userRolesCache.Add(userId, allroles.ToDictionary(role => role, role => true));
                    }

                    var userRoles = (Dictionary<String, bool>)userRolesCache.GetData(userId);
                    if (roles.Any(role => userRoles.ContainsKey(role) && userRoles[role]))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log.ErrorFormat("Ошибка в IsUserInRole: {0}", ex.Message);
                }
                return false;
            }
        }

        public static void ClearUserRights(string userId)
        {
            lock (userRolesCache)
            {
                try
                {
                    if (!string.IsNullOrEmpty(userId) && userRolesCache.Contains(userId))
                        userRolesCache.Remove(userId);
                }
                catch (Exception ex)
                {
                    LogHelper.Log.ErrorFormat("Ошибка в ClearUserRights: {0}", ex.Message);
                }
            }
        }
    }
}