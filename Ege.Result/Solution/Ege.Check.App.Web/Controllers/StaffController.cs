namespace Ege.Check.App.Web.Controllers
{
    using System.Web.Mvc;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.Logic.Models.Staff;

    public class StaffController : Controller
    {
        public const string SettingsRoute = "Settings";
        public const string ServersRoute = "ServerAvailability";
        public const string ActivationRoute = "Activation";
        public const string LoginRoute = "StaffLogin";
        public const string CancelledRoute = "Cancelled";
        public const string ProfileRoute = "Profile";
        public const string DocumentsRoute = "Documents";
        public const string TaskSettingsRoute = "TaskSettings";
        public const string UsersRoute = "Users";

        private string GetDefaultRoute(Role role)
        {
            switch (role)
            {
                case Role.Fct:
                case Role.Rcoi:
                    return SettingsRoute;
                case Role.FctOperator:
                    return ServersRoute;
                // ReSharper disable RedundantCaseLabel
                case Role.HscOperator:
                case Role.None:
                // ReSharper restore RedundantCaseLabel
                default:
                    CookieHelper.Remove(HttpContext, Cookies.StaffCookieName);
                    return LoginRoute;
            }
        }

        [Route("staff")]
        [Route("rcoi", Name = LoginRoute)]
        [StaffAuthenticationFilter]
        public ActionResult Login()
        {
            var user = User as IStaffPrincipal;
            if (user != null)
            {
                return user.User.IsEnabled
                           ? RedirectToRoute(GetDefaultRoute(user.User.Role))
                           : RedirectToRoute(ActivationRoute);
            }
            return View();
        }

        [Route("rcoi/activation", Name = ActivationRoute)]
        [StaffAuthenticationFilter]
        [Authorize]
        public ActionResult Activation()
        {
            return View();
        }

        [Route("staff/settings")]
        [Route("rcoi/settings", Name = SettingsRoute)]
        [StaffAuthenticationFilter]
        [Authorize(Roles = "fct,rcoi")]
        public ActionResult Settings()
        {
            return View();
        }

        [Route("rcoi/cancelled", Name = CancelledRoute)]
        [StaffAuthenticationFilter]
        [Authorize(Roles = "fct,rcoi")]
        public ActionResult Cancelled()
        {
            return View();
        }

        [Route("rcoi/profile", Name = ProfileRoute)]
        [StaffAuthenticationFilter]
        [Authorize(Roles = "fct,rcoi")]
        public ActionResult StaffProfile()
        {
            return View();
        }

        [Route("rcoi/documents", Name = DocumentsRoute)]
        [StaffAuthenticationFilter]
        [Authorize(Roles = "fct,rcoi")]
        public ActionResult Documents()
        {
            return View();
        }

        [Route("rcoi/tasksettings", Name = TaskSettingsRoute)]
        [StaffAuthenticationFilter]
        [Authorize(Roles = "fct")]
        public ActionResult TaskSettings()
        {
            return View();
        }

        [Route("rcoi/users", Name = UsersRoute)]
        [StaffAuthenticationFilter]
        [Authorize(Roles = "fct")]
        public ActionResult Users()
        {
            return View();
        }

        [Route("rcoi/serversaccessibility", Name = ServersRoute)]
        [StaffAuthenticationFilter]
        [Authorize(Roles = "fct,fctoperator")]
        public ActionResult ServersAccessibility()
        {
            return View();
        }
    }
}
