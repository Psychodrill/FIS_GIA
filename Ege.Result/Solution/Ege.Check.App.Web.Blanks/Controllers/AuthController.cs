namespace Ege.Check.App.Web.Blanks.Controllers
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Ege.Check.App.Web.Blanks.Esrp;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.Dal.Store.Factory;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Dal.Store.Repositories;
    using JetBrains.Annotations;

    public class AuthController : Controller
    {
        [NotNull] private readonly IConnectionFactory<SqlConnection> _connectionFactory;
        [NotNull] private readonly IEsrpUrlCreator _esrpUrlCreator;
        [NotNull] private readonly IUserRepository _userRepository;

        private const string RedirectFromEsrpRouteName = "ReturnFromEsrp";
        private const string EsrpLoginRouteName = "EsrpLogin";

        [NotNull]
        public new UrlHelper Url
        {
            get
            {
                var result = base.Url;
                if (result == null)
                {
                    throw new NullReferenceException("Controller.Url is null");
                }
                return result;
            }
        }

        public AuthController(
            [NotNull]IEsrpUrlCreator esrpUrlCreator, 
            [NotNull]IUserRepository userRepository,
            [NotNull]IConnectionFactory<SqlConnection> connectionFactory)
        {
            _esrpUrlCreator = esrpUrlCreator;
            _userRepository = userRepository;
            _connectionFactory = connectionFactory;
        }

        [Route("auth/login", Name = EsrpLoginRouteName)]
        public RedirectResult Login(string returnUrl = null)
        {
            if (Request == null || Request.Url == null)
            {
                throw new NullReferenceException("Request is null");
            }
            if (User != null && User.Identity.IsAuthenticated)
            {
                return Redirect(Url.RouteUrl(HscController.UploadBlanksRouteName));
            }
            var returnFromEsrpUrl = Url.RouteUrl(RedirectFromEsrpRouteName, new {returnUrl = HttpUtility.UrlEncode(returnUrl)}, Request.Url.Scheme);
            var url = _esrpUrlCreator.Login(returnFromEsrpUrl);
            return Redirect(url);
        }

        [Route("auth/logout", Name = "Logout")]
        public RedirectResult Logout()
        {
            if (Request != null && Request.Cookies != null && Request.Url != null && Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                FormsAuthentication.SignOut();
                var returnUrl = Url.RouteUrl(RedirectFromEsrpRouteName, new {}, Request.Url.Scheme);
                var url = _esrpUrlCreator.Logout(returnUrl);
                return Redirect(url);
            }
            CookieHelper.Remove(System.Web.HttpContext.Current, Cookies.StaffCookieName);
            return Redirect(Url.RouteUrl(HscController.CommonLoginRouteName));
        }

        [Route("ReturnFromEsrp", Name = RedirectFromEsrpRouteName)]
        public async Task<RedirectResult> RedirectFromEsrp(Guid esrpres, string l, int st, string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(l))
            {
                User user;
                using (var connection = await _connectionFactory.CreateHscAsync())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                    }
                    user = await _userRepository.MergeAsync(connection, l, esrpres);
                }
                if (user == null)
                {
                    throw new InvalidOperationException("IUserRepository::MergeAsync returned null");
                }
                FormsAuthentication.SetAuthCookie(user.Login, true);
                returnUrl = returnUrl != null
                    ? HttpUtility.UrlDecode(returnUrl)
                    : Url.RouteUrl(HscController.UploadBlanksRouteName);
                return Redirect(returnUrl);
            }
            return Redirect(Url.RouteUrl(EsrpLoginRouteName));
        }
    }
}
