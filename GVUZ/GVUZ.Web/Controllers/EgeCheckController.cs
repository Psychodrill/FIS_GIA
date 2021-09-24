using System.Web.Mvc;
using GVUZ.Helper.MVC;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;

namespace GVUZ.Web.Controllers
{
	[AuthorizeAdm(Roles = UserRole.EduUser), MenuSection(MenuSections.EgeCheck)]
	public class EgeCheckController : BaseController
	{
		public ActionResult Index()
		{
            return View("Index");
		}
	}
}
