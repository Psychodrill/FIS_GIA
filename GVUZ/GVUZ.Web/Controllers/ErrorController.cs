using System;
using System.Web.Mvc;
using GVUZ.Helper;

namespace GVUZ.Web.Controllers
{
	public class ErrorController : BaseController
	{
		public ActionResult HttpError()
		{
			Exception ex = null;

			try
			{
				ex = (Exception)HttpContext.Application[Request.UserHostAddress];
			}
			catch
			{
			}

			if (ex != null)
			{
				ViewData["Description"] = ex.Message;
			}
			else
			{
				ViewData["Description"] = "На странице произошла ошибка";
			}

			ViewData["Title"] = "Ошибка";

			return View("Error");
		}

		public ActionResult Http404()
		{
			ViewData["Title"] = "Запрошенная страница не найдена";

			return View("Error");
		}

		// (optional) Redirect to home when /Error is navigated to directly  
		public ActionResult Index()
		{
			return Redirect(Url.Generate<InstitutionController>(m => m.View(null)));
		}
	}
}