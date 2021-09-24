namespace Ege.Check.App.Web.Blanks.Controllers
{
    using System.Web.Mvc;
    using Ege.Check.App.Web.Blanks.Esrp;
    using Ege.Check.App.Web.Blanks.ViewModels.Hsc;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Hsc.Logic.Configuration;
    using JetBrains.Annotations;

    public class HscController : Controller
    {
        public const string CommonLoginRouteName = "Login";
        public const string UploadBlanksRouteName = "Blanks";
        [NotNull] private readonly IHscSettings _hscSettings;

        public HscController([NotNull] IHscSettings hscSettings)
        {
            _hscSettings = hscSettings;
        }

        [Route("", Name = CommonLoginRouteName)]
        [StaffAuthenticationFilter]
        public ActionResult Login(string returnUrl = null)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToRoute(UploadBlanksRouteName);
            }
            return View();
        }

        [Route("blanks", Name = UploadBlanksRouteName)]
        [StaffAuthenticationFilter(Order = 1)]
        [EsrpAuthorize(Order = 2)]
        public ActionResult Index()
        {
            return View(new IndexPageViewModel(_hscSettings.OpenDate, User is IStaffPrincipal || _hscSettings.CsvUploadAllowedForEsrp));
        }

        [Route("downloads", Name = "Downloads")]
        [StaffAuthenticationFilter(Order = 1)]
        [EsrpAuthorize(Order = 2)]
        public ActionResult Downloads()
        {
            return View();
        }
    }
}