using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fbs.Web
{
    public static class CurrentUser
    {
        public static string ClietnLogin
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return HttpContext.Current.User.Identity.Name;
            }
        }
    }
}
