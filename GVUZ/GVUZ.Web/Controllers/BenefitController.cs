using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Applications;
using GVUZ.Model.Benefits;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using System.Linq;

namespace GVUZ.Web.Controllers
{
	[AuthorizeAdm(Roles = UserRole.EduUser)]
	[MenuSection("Institution")]
	public class BenefitController : BaseController
	{
		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult AddBenefitItem(int? entranceTestItemID, int? competitiveGroupID)
		{
			if (!entranceTestItemID.HasValue || !competitiveGroupID.HasValue)
				return new AjaxResultModel(AjaxResultModel.DataError);
			using (BenefitsEntities dbContext = new BenefitsEntities())
			{
				return PartialView("Benefit/AddBenefitItem", dbContext.LoadBenefitItem(new AddBenefitViewModelC
																					   {
																						EntranceTestItemID = entranceTestItemID.Value,
																						CompetitiveGroupID = competitiveGroupID.Value
																					   }));
			}
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult EditBenefitItem(int? benefitItemID)
		{
			if (benefitItemID.HasValue)
			{
				using (BenefitsEntities dbContext = new BenefitsEntities())
				{
					return PartialView("Benefit/AddBenefitItem", dbContext.LoadBenefitItem(new AddBenefitViewModelC { BenefitItemID = benefitItemID.Value }));
				}
			}

			return new EmptyResult();
		}

		public ActionResult BenefitList(int? entranceTestItemID, int? competitiveGroupID)
		{
			if (!entranceTestItemID.HasValue || !competitiveGroupID.HasValue)
				return new AjaxResultModel(AjaxResultModel.DataError);
			using (BenefitsEntities dbContext = new BenefitsEntities())
			{
				var model = dbContext.FillBenefitItems(new BenefitViewModelC(entranceTestItemID.Value, competitiveGroupID.Value));
				model.CanEdit = model.CanEdit & !UserRole.IsCurrentUserReadonly();
				return PartialView("Benefit/BenefitItemList", model);
			}
		}

        [HttpPost]
        public ActionResult GetOlympicData(AddBenefitViewModelC model)
        {
            using (var dbContext = new BenefitsEntities())
            {
                return dbContext.GetOlympicData(model.SubjectID, model.OlympicYearID);
            }
	    }

	    [HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult SaveBenefitItem(AddBenefitViewModelC model)
		{
			using (BenefitsEntities dbContext = new BenefitsEntities())
			{
				return Json(dbContext.SaveBenefitItem(model, InstitutionID));
			}
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult DeleteBenefitItem(int? benefitItemID)
		{
			using (BenefitsEntities dbContext = new BenefitsEntities())
			{
				return Json(dbContext.DeleteBenefitItemC(benefitItemID));
			}
		}

		public ActionResult OlympicDetailsView(int? olympicID)
		{
			if (!olympicID.HasValue)
				return new AjaxResultModel(AjaxResultModel.DataError);
			using (BenefitsEntities dbContext = new BenefitsEntities())
			{
				OlympicDetailsViewModel model = new OlympicDetailsViewModel();
				model.OlympicID = olympicID.Value;
				model.FillData(dbContext);
				return PartialView("Benefit/OlympicView", model);
			}
		}
	}
}
