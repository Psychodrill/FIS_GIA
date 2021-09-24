using System.Reflection;
using FogSoft.Helpers;
using GVUZ.Web.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using log4net;
using System;
using System.Linq;

namespace GVUZ.Web.Helpers
{
	/// <summary>
	/// Методы для работы с текущим ОО пользователя
	/// </summary>
	public static class InstitutionHelper
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static int MainInstitutionID
        {
            get             
            {
                string userName = UserHelper.GetAuthenticatedUserName();
                var cache = ServiceLocator.Current.GetInstance<ICache>();
                return cache.Get("MainInstitutionId_" + userName, 0);
            }
            set 
            {
                string userName = UserHelper.GetAuthenticatedUserName();
                var cache = ServiceLocator.Current.GetInstance<ICache>();
                int cachedId = cache.Get("MainInstitutionId_" + userName, -1);
                if (cachedId != -1)
                    cache.Remove("MainInstitutionId_" + userName);

                cache.Insert("MainInstitutionId_" + userName, value, Int32.MaxValue);
                FilterStateManager.Current.RemoveAll();
            }
        }

		/// <summary>
		/// Устанавливает InstitutionID для текущей сессии.</summary>
        public static void SetInstitutionID(int id)
		{
            //ISession session = ServiceLocator.Current.GetInstance<ISession>();
            //session.SetValue("InstitutionID", value);

            if (id > 0)
            {
                string userName = UserHelper.GetAuthenticatedUserName();
                var cache = ServiceLocator.Current.GetInstance<ICache>();
                int cachedId = cache.Get("InstitutionId_" + userName, 0);
                if (cachedId != id)
                {
                    cache.Remove("InstitutionId_" + userName);
                    cache.Insert("InstitutionId_" + userName, id, Int32.MaxValue);
                    // сбрасываем состояния значения фильтров для нового InstitutionId
                    FilterStateManager.Current.RemoveAll();
                }
            }
            else
            {
                // сбрасываем состояния значения фильтров для нового InstitutionId
                FilterStateManager.Current.RemoveAll();
            }
		}

		/// <summary>
		/// Возвращает InstitutionID для текущей сессии.</summary>
		public static int GetInstitutionID(bool insidePortlet = false)
		{
            //ISession session = ServiceLocator.Current.GetInstance<ISession>();
            //int id = session.GetValue("InstitutionID", 0).To(0);
            //if (id != 0)
            //    return id;
            //if (insidePortlet)
            //    return 0;

			string userName = UserHelper.GetAuthenticatedUserName();
		    if (string.IsNullOrEmpty(userName))
		    {
                Log.Error("Current user does not specified.");
		        return 0;
		    }

		    var cache = ServiceLocator.Current.GetInstance<ICache>();
            int id = cache.Get("InstitutionId_" + userName, -1);
            if (id != -1) 
                return id;
            
            if (insidePortlet) 
                return 0;

			UserPolicy policy = UserHelper.LoadUserPolicy(userName);
			if (policy == null)
			{
				Log.Error("UserPolicy not found.");
				return 0;
			}

            id = policy.InstitutionID ?? 0;
            MainInstitutionID = id;

            cache.Insert("InstitutionId_" + userName, id, Int32.MaxValue);
            return id;
		}
        
        public static string  InstitutionName 
        {
            get
            {
                int institutionId = GetInstitutionID(false);
                if (institutionId == 0)
                    return string.Empty;

                var cache = ServiceLocator.Current.GetInstance<ICache>();
                string name = cache.Get("InstitutionName_" + institutionId, "");
                if (!string.IsNullOrEmpty(name))
                    return name;

                using (var context = new InstitutionsEntities())
                {
                    name = context.Institution.Where(x => x.InstitutionID == institutionId).Select(x => x.BriefName).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(name))
                    cache.Insert("InstitutionName_" + institutionId, name, Int32.MaxValue);

                return name ?? "";
            }
        }
	}
}