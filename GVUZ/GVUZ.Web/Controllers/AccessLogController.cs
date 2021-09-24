using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;
using GVUZ.Model.Helpers;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Controllers
{
	[Authorize(Roles = UserRole.FBDAdmin)]
    public class AccessLogController : Controller
    {
		public ActionResult AccessLogList()
		{
			using (var dbContext = new EntrantsEntities())
			{
				return View("AccessLogList", dbContext.InitialFillAccessListViewModel());
			}
		}

		[HttpPost]
		public ActionResult GetAccessLogList(AccessListViewModel model)
		{
			using (var dbContext = new EntrantsEntities())
			{
				return dbContext.GetAccessListModel(model);
			}
		}
    }
}
