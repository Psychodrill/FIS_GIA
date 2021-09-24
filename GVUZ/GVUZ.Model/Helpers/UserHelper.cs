using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using FogSoft.Helpers;
using GVUZ.Model.Institutions;
using log4net;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Model.Helpers
{
	/// <summary>
	/// Возвращает информацию о текущем пользователе, не предназначен для использования в портлетах.</summary>
	public static class UserHelper
	{
		// TODO: попробовать отрефакторить
		//private const string UserFullNameSessionKey = "User.FullName";
        //private const string UserIDSessionKey = "User.UserID";

		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// Возвращает непустое значение, если текущий пользователь аутентифицирован.</summary>
		public static string GetAuthenticatedUserName()
		{
			if (HttpContext.Current != null && HttpContext.Current.User != null &&
					HttpContext.Current.User.Identity.IsAuthenticated)
				return HttpContext.Current.User.Identity.Name;
			return string.Empty;
		}

		/// <summary>
		/// Возвращает непустое значение, если текущий пользователь аутентифицирован и у него есть UserPolicy.</summary>
		public static string GetCurrentFullName()
		{
            //var session = ServiceLocator.Current.GetInstance<ISession>();
            //string value = session.GetValue(UserFullNameSessionKey, "");

            ////LogHelper.Log.DebugFormat("Работает пользователь: {0} ({1})", HttpContext.Current.User.Identity.Name, value);
            //if (!string.IsNullOrEmpty(value)) return value;
			
            //return LoadCurrentUserFullName(session);

            string userName = UserHelper.GetAuthenticatedUserName();
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var fullName = cache.Get("CurrentUserFullName_" + userName, "");
		    if (!string.IsNullOrEmpty(fullName))
		        return fullName;

		    return LoadCurrentUserFullName();
		}

        public static string GetCurrentUserID()
        {
            //var session = ServiceLocator.Current.GetInstance<ISession>();
            //var value = session.GetValue(UserIDSessionKey, "");

            ////LogHelper.Log.DebugFormat("Работает пользователь: {0} ({1})", HttpContext.Current.User.Identity.Name, value);
            //if (!string.IsNullOrEmpty(value)) return value;

            //return LoadCurrentUserID(session);

            string userName = UserHelper.GetAuthenticatedUserName();
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var id = cache.Get("CurrentUserId_" + userName, "");
            if (!string.IsNullOrEmpty(id))
                return id;

            return LoadCurrentUserID();
        }

		/// <summary>
		/// Возвращает имя пользователя или пустую строку, которую можно использовать в ActionLink (чтобы работало корректно)
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentFullNameNonEmpty()
		{
			string fullName = GetCurrentFullName();
			return String.IsNullOrEmpty(fullName) ? " " : fullName;
		}

		/// <summary>
		/// Загружает ФИО для текущего пользователя.</summary>
		private static string LoadCurrentUserFullName()
		{
			string userName = GetAuthenticatedUserName();
			if (string.IsNullOrEmpty(userName))
			{
				Log.Error("Current user does not specified.");
				return string.Empty;
			}

            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var fullName = cache.Get("CurrentUserFullName_" + userName, "");
            if (!string.IsNullOrEmpty(fullName))
                return fullName;

			UserPolicy policy = LoadUserPolicy(userName);
			if (policy == null)
			{
				Log.Error("UserPolicy not found.");
				return string.Empty;
			}

            cache.Insert("CurrentUserFullName_" + userName, policy.FullName, Int32.MaxValue);
			return policy.FullName ?? "";
		}

        private static string LoadCurrentUserID()
        {
            string userName = GetAuthenticatedUserName();
            if (string.IsNullOrEmpty(userName))
            {
                Log.Error("Current user does not specified.");
                return "";
            }

            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var id = cache.Get("CurrentUserId_" + userName, "");
            if (!string.IsNullOrEmpty(id))
                return id;

            UserPolicy policy = LoadUserPolicy(userName);
            if (policy == null)
            {
                Log.Error("UserPolicy not found.");
                return "";
            }

            cache.Insert("CurrentUserId_" + userName, policy.UserID.ToString(), Int32.MaxValue);
            return policy.UserID.ToString();
        }

		public static void SetCurrentUserFullName(string fullName)
		{
            //if (session == null)
            //    session = ServiceLocator.Current.GetInstance<ISession>();
            //session.SetValue(UserFullNameSessionKey, fullName);

            string userName = UserHelper.GetAuthenticatedUserName();
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var cached = cache.Get("CurrentUserFullName_" + userName, "");
            if (!string.IsNullOrEmpty(cached))
                cache.Remove("CurrentUserFullName_" + userName);

            cache.Insert("CurrentUserFullName_" + userName, fullName, Int32.MaxValue);
		}

        public static void SetCurrentUserID(string userid)
        {
            //if (session == null)
            //    session = ServiceLocator.Current.GetInstance<ISession>();
            //session.SetValue(UserIDSessionKey, userid);

            string userName = GetAuthenticatedUserName();
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var id = cache.Get("CurrentUserId_" + userName, "");
            if (userid != id)
                cache.Remove("CurrentUserId_" + userName);

            cache.Insert("CurrentUserId_" + userName, userid, Int32.MaxValue);
        }

		public static bool IsAuthenticatedUserInRole(string roleName)
		{
			return HttpContext.Current.User.IsInRole(roleName);
		}

        /// <summary>
		/// Возвращает <see cref="UserPolicy"/>, если нет - null.</summary>
        static Dictionary<string, UserPolicy> _userPolicyCache = new Dictionary<string, UserPolicy>();
        static Dictionary<string, bool> _canLogonCache = new Dictionary<string, bool>(); 

#warning переписать по человечески! сделано на скорую руку!!!
        public static UserPolicy LoadUserPolicy(string userName)
        {
            try
            {
                lock (_userPolicyCache)
                {
                    if (!_userPolicyCache.ContainsKey(userName))
                    {
                        using (var context = new InstitutionsEntities())
                        {
                            var policy = context.UserPolicy.FirstOrDefault(x => x.UserName == userName);
                            if (policy != null)
                                _userPolicyCache.Add(userName, policy);
                            else return null;
                        }
                    }
                    return _userPolicyCache[userName];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.ErrorFormat(ex.Message, ex);
                using (var context = new InstitutionsEntities())
                {
                    return context.UserPolicy.FirstOrDefault(x => x.UserName == userName);
                }
            }
        }

		public static bool CanLogon(string userName)
		{
		    try
		    {
                lock (_canLogonCache)
                {
		            if (!_canLogonCache.ContainsKey(userName))
		            {
		                using (var context = new InstitutionsEntities())
		                {
		                    var user = context.UserPolicy.FirstOrDefault(x => x.UserName == userName);
                            if (user != null && user.UserID != Guid.Empty)
                                if (!_canLogonCache.ContainsKey(userName))
                                    _canLogonCache.Add(userName, true);
                                else
                                    _canLogonCache[userName] = true;
                            else return false;
		                }
		            }

                    return _canLogonCache[userName];
		        }
		    }
            catch (Exception ex)
            {
                LogHelper.Log.ErrorFormat(ex.Message, ex);
                using (var context = new InstitutionsEntities())
                {
                    var user = context.UserPolicy.FirstOrDefault(x => x.UserName == userName);
                    return (user != null && user.UserID != Guid.Empty);
                }
            }
		}

        //Этот класс вызывает ошибку сериализации (даже если реализовать ISerializable)
        //при использовании веб-сервера VS
        //Нужно использовать IIS (или IIS Express)
		private class GVUZIdentity : IIdentity
		{
			private readonly string _name;
			private readonly bool _isCorrectUser = false;
			public string Name
			{
				get { return _name; }
			}

			public string AuthenticationType
			{
				get { return "Forms"; }
			}

			public bool IsAuthenticated
			{
				get { return _isCorrectUser; }
			}

			public GVUZIdentity(FormsIdentity identity)
			{
				_name = identity.Name;
				_isCorrectUser = CanLogon(_name);
			}
		}

		private class GVUZPrinicipal : IPrincipal
		{
			private readonly GenericPrincipal _baseUser;
			private readonly IIdentity _gvuzIdentity;
			public GVUZPrinicipal(GenericPrincipal principal)
			{
				_baseUser = principal;
				FormsIdentity forms = _baseUser.Identity as FormsIdentity;
                if (forms != null)
                    _gvuzIdentity = new GVUZIdentity(forms);
                else
                {
					_gvuzIdentity = _baseUser.Identity;
				}
			}

			public bool IsInRole(string role)
			{
				return _baseUser.IsInRole(role);
			}

			public IIdentity Identity
			{
				get { return _gvuzIdentity; }
			}
		}

		public static IPrincipal ReplaceIdentity(IPrincipal genericPrincipal)
		{
			GenericPrincipal principal = genericPrincipal as GenericPrincipal;
			if (principal != null)
				return new GVUZPrinicipal(principal);
			return genericPrincipal;
		}
	}
}