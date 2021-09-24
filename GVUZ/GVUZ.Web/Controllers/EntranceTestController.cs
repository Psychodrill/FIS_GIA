using System;
using System.Linq;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Entrants;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Controllers
{
	[AuthorizeAdm(Roles = UserRole.EduUser)]
	[MenuSection("Institution")]
	public class EntranceTestController : BaseController
	{
		public ActionResult TreeStructureView(int? structureItemID)
		{
			/*using (var context = new InstitutionsEntities())
			{
				// если запрашивается идентификатор 0, то возвращаем рутовый элемент
				if (structureItemID == 0)
				{
					var rootStructure = context.AdmissionStructurePublished
						.Include(s => s.AdmissionStructurePublished1)
						.Include(s => s.AdmissionItemPublished)
						.Include(s => s.AdmissionItemPublished.Institution)
						.Where(x => x.AdmissionItemPublished.InstitutionID == InstitutionID).First();

					return Json(new TreeItemViewModel
					{
						ItemID = rootStructure.AdmissionStructureID,
						Name = rootStructure.AdmissionItemPublished.Institution.FullName,
						IsLeaf = rootStructure.AdmissionStructurePublished1.Count == 0,
						CanAdd = false
					});
				}

				var instItems = context.AdmissionStructurePublished.Where(
					// open tree structure
					x => structureItemID > 0 && x.ParentID == structureItemID)
					.OrderBy(x => x.AdmissionItemPublished.Name)
					.Select(x => new
					{
						ItemID = x.AdmissionStructureID,
						x.AdmissionItemPublished.AdmissionItemType.ItemTypeID,
						PlaceCount = x.AdmissionItemPublished.EntranceTestItemPublished.Count,
						x.AdmissionItemPublished.Name,
						IsLeaf = x.AdmissionStructurePublished1.Count == 0,
						x.AdmissionItemPublished.AdmissionItemType.ItemLevel,
					}).ToList().Select
					(x =>
					 new TreeItemViewModel
					 {
						 ItemID = x.ItemID,
						 Name =
							 AddAdmissionItemViewModel.CorrectItemFullName(
								 x.ItemTypeID, x.Name, x.PlaceCount == 0 ? (int?) null : x.PlaceCount),
						 IsLeaf = x.IsLeaf,
						 CanAdd = false,
						 CanDelete = false,
						 CanEdit =
							 (x.ItemLevel != AdmissionItemTypeConstants.Course || x.Name.ToUpper() != EntranceTestType.DeniedCourseName.ToUpper()) &&
							 x.ItemLevel != AdmissionItemTypeConstants.PlacesType,
					 });

				return Json(instItems.ToArray());
			}*/
			return new EmptyResult();
		}
		
		public ActionResult AddEntranceTest(int? groupID)
		{
			if (groupID.HasValue)
			{
				using (EntrantsEntities dbContext = new EntrantsEntities())
				{
					EntranceTestViewModelC model = dbContext.CreateEntranceTestDataByCompetitiveGroup(new EntranceTestViewModelC(groupID.Value));
					model.CanEdit = model.CanEdit & !UserRole.IsCurrentUserReadonly();
					return PartialView("EntranceTest/AddEntranceTest", model);
				}
			}

			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult GetEntranceTestCount(int? groupID)
		{
			if (groupID.HasValue)
			{
				using (EntrantsEntities dbContext = new EntrantsEntities())
				{
					int count = dbContext.GetEntranceTestCount(groupID.Value);
					return new AjaxResultModel { Data = count };
				}
			}

			return new AjaxResultModel(AjaxResultModel.DataError) { Data = 0 };
		}

		[HttpPost]
		[Obsolete]
		public ActionResult ViewEntranceTest(int? structureItemID)
		{
			/*if (structureItemID.HasValue)
			{
				using (InstitutionsEntities dbContext = new InstitutionsEntities())
				{
					return PartialView("Portlets/Institutions/EntranceTestView",
									   dbContext.ViewEntranceTestDataByStructure(
										new EntranceTestViewModel(structureItemID.Value)));
				}
			}*/
			return new EmptyResult();
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult CreateEntranceTestItem(EntranceTestViewModelC model)
		{
			if (!ModelState.IsValid)
				return new AjaxResultModel(ModelState).SetMessageFromErrors();
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				return dbContext.SaveEntranceTestItem(model);
			}
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult DeleteEntranceTestItem(int? testItemID)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				if (testItemID.HasValue)
					return Json(dbContext.DeleteEntranceTestItem(testItemID.Value));
				return Json(new AjaxResultModel("Не найден элемент испытания"));
			}
		}

		[HttpPost]
		[Obsolete]
		public ActionResult CopyEntranceTestItemFromParent(int? structureItemID)
		{
			/*if (structureItemID.HasValue)
			{
				using (InstitutionsEntities dbContext = new InstitutionsEntities())
				{
					return Json(dbContext.CopyEntranceTestStructure(structureItemID.Value));
				}
			}*/
			return new EmptyResult();
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult PublishStructure()
		{
			using (InstitutionsEntities dbContext = new InstitutionsEntities())
			{
				dbContext.PublishAdmissionStructure(InstitutionID);
				using (var context = new InstitutionsEntities())
				{
					DateTime? publishDate = context.Institution.Where(x => x.InstitutionID == InstitutionID).Select(x => x.AdmissionStructurePublishDate).FirstOrDefault();
					string resDate = publishDate == null ? null : publishDate.Value.ToString("dd.MM.yyyy HH:mm");
					return Json(new AjaxResultModel { Data = resDate });
				}
			}
		}
	}
}