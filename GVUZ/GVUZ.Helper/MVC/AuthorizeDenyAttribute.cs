using System.Web;
using System.Web.Mvc;

namespace GVUZ.Helper.MVC
{
    public class AuthorizeDenyAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return !base.AuthorizeCore(httpContext);
        }
    }
}