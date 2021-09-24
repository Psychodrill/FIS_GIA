using System;
using System.Web.Security;
using FogSoft.Helpers;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Web.Auth
{
    public class SqlCachedRoleProvider : SqlRoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            var key = string.Format("RolesForUser_{0}", username);
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var result = cache.Get<string[]>(key, null);
            if (result == null)
            {
                result = base.GetRolesForUser(username);
                cache.Insert(key, result);
            }
            return result;
        }

        public static void DropCacheForUser(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");

            var key = string.Format("RolesForUser_{0}", userName);
            var cache = ServiceLocator.Current.GetInstance<ICache>();

            cache.Remove(key);
        }
    }
}