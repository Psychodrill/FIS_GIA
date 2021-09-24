using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Institutions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using GVUZ.Web.ContextExtensions;

namespace GVUZ.Web.Controllers
{
	[Authorize(Roles = UserRole.FBDAdmin + "," + UserRole.FbdRonUser)]
	[MenuSection("Administration")]
    public class InstitutionAdminController : BaseController
    {
        public ActionResult List()
        {
			using (InstitutionsEntities dbContext = new InstitutionsEntities())
			{
				return View("../Institution/InstitutionList", dbContext.InitialFillInstitutionListViewModel(InstitutionID));
			}
        }

		[Authorize]
		[HttpPost]
		public AjaxResultModel GetInstitutionList(InstitutionListViewModel model)
		{
			using (var dbContext = new InstitutionsEntities())
			{
				return dbContext.GetInstitutionList(model);
			}
		}

		public ActionResult SwitchToInstitution(int? institutionID)
		{
			if (institutionID.HasValue)
			{
                InstitutionHelper.MainInstitutionID = institutionID.Value;
				InstitutionHelper.SetInstitutionID(institutionID.Value);
				return RedirectToAction("ApplicationList", "InstitutionApplication");
			}

			return RedirectToAction("List");
		}

		public ActionResult SwitchToInstitutionRedirect(int? institutionID, string url)
		{
			if (institutionID.HasValue)
			{
                InstitutionHelper.MainInstitutionID = institutionID.Value;
				InstitutionHelper.SetInstitutionID(institutionID.Value);
				return Redirect(url);
			}

			return RedirectToAction("List");
		}
    }
}
