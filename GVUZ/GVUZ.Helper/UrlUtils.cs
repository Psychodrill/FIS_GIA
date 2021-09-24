using System;
using System.Web;
using System.Web.Security;
using FogSoft.Helpers;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Helper
{
    [Flags]
    public enum FBDUserSubroles
    {
        ReadOnly = 1,
        OrderDirection = 2,
        ApplicationsDirection = 4,
        CompetitiveGroupsDirection = 8,
        CampaignDataDirection = 16,
        CampaignDirection = 32,
        InstitutionDataDirection = 64
    }


    public static class UrlUtils
    {
        public static string ShemaAndHost
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    Uri currentUrl = HttpContext.Current.Request.Url;
                    return string.Format("{0}://{1}", currentUrl.Scheme, currentUrl.Authority);
                }

                return "http://localhost";
            }
        }

        public static string AppPath
        {
            get { return HttpRuntime.AppDomainAppVirtualPath; }
        }

        public static string PageLink(string relPath)
        {
            return AppPath + relPath;
        }

        public static bool IsReadOnly(FBDUserSubroles subrole4Check)
        {
            if (HttpRuntime.Cache.Get("Subrole") == null)
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.RewritePath("~/Account/AuthRedirect");

                if (Roles.IsUserInRole("fbd_^user"))
                    return true;

                return false;
            }

            var filialID = (int?) HttpRuntime.Cache.Get("FilialID");
            if ((filialID != null) && (filialID != 0))
            {
                var session = ServiceLocator.Current.GetInstance<ISession>();
                int instID = session.GetValue("InstitutionID", 0).To(0);
                if (filialID != instID)
                    return true;
            }


            var subrole = (int) HttpRuntime.Cache.Get("Subrole");

            if (subrole == 0)
                return false;

            if (subrole == 1) // read-only role - always single
                return true;

            if (((int) subrole4Check != 0) && ((subrole & (int) subrole4Check) > 0))
                return false;

            return true;
        }


/*
		public static string GetFullWebApplicationUrl(string relativeUrl)
		{
			return Schr + AppPath + ;
		}
*/
    }
}