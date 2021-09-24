namespace Ege.Check.App.Web.Common.Auth
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
    using Ege.Check.Common.Extensions;
    using JetBrains.Annotations;

    public class CookieHelper
    {
        private static readonly DateTime LongLongAgo = new DateTime(1993, 10, 4);

        public static void Remove([NotNull] HttpResponseMessage result, [NotNull] string name)
        {
            var removedCookie = new CookieHeaderValue(name, "")
                {
                    Expires = LongLongAgo,
                    Path = "/",
                };
            result.Headers.AddCookies(removedCookie.ToEnumerable());
        }

        public static void Remove(HttpContext context, [NotNull] string name)
        {
            if (context == null)
            {
                return;
            }
            context.Response.Cookies.Add(new HttpCookie(name)
                {
                    Expires = LongLongAgo,
                });
        }

        public static void Remove(HttpContextBase context, [NotNull] string name)
        {
            if (context == null)
            {
                return;
            }
            context.Response.Cookies.Add(new HttpCookie(name)
            {
                Expires = LongLongAgo,
            });
        }
    }
}
