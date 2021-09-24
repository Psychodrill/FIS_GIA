using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Courses;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Portlets;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Controllers
{
	[AuthorizeAdm(Roles = UserRole.EduUser)]
	[MenuSection("Institution")]
	public class PreparatoryCourseController : BaseController
	{
		//[GeneratorPortletLink(typeof(PortletLinkHelper), "InstitutionTabLink", MethodParams = new object[] { PortletType.InstitutionCourcesTab, 4 })]
		//public ActionResult Index()
		//{
		//	using (CoursesEntities dbContext = new CoursesEntities())
		//	{
		//		return View(dbContext.FillPreparatoryCourses(new PreparatoryCourseViewModel(InstitutionID)));
		//	}
		//}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult AddPreparatoryCourse()
		{
			using (CoursesEntities dbContext = new CoursesEntities())
			{
				return PartialView("PreparatoryCourse/AddPreparatoryCourse", dbContext.InitPreparatoryCourse(new AddPreparatoryCourseViewModel(InstitutionID)));
			}
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult EditPreparatoryCourse(int? courseID)
		{
			if (courseID.HasValue)
			{
				using (CoursesEntities dbContext = new CoursesEntities())
				{
					return PartialView("PreparatoryCourse/AddPreparatoryCourse", dbContext.LoadPreparatoryCourse(new AddPreparatoryCourseViewModel(InstitutionID, courseID.Value)));
				}
			}

			return new EmptyResult();
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult CreatePreparatoryCourse(AddPreparatoryCourseViewModel model)
		{
			if (!ModelState.IsValid)
				return new AjaxResultModel(ModelState);
			using (CoursesEntities dbContext = new CoursesEntities())
			{
				return dbContext.CreatePreparatoryCourse(model);
			}
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult DeletePreparatoryCourse(int? preparatoryCourseID)
		{
			using (CoursesEntities dbContext = new CoursesEntities())
			{
				return dbContext.DeletePreparatoryCourse(preparatoryCourseID);
			}
		}
	}
}