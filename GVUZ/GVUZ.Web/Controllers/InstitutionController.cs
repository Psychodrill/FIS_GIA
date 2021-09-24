using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Portlets;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using GVUZ.DAL.Dapper.Repository.Interfaces.Institution;
using GVUZ.DAL.Dapper.Repository.Model.Institution;
using GVUZ.Web.ViewModels.InstitutionInfo;

namespace GVUZ.Web.Controllers
{
	/// <summary>
	/// Контроллер редактирования общей информации ОО.
	/// У контроллера только одна View с именем "Edit". На ней происходит как отображение, так и редактирование.
	/// </summary>
	[HandleError, MenuSection("Institution")]
	// redirect page. don't specify roles	
	[Authorize]
	public class InstitutionController : BaseController
	{
		private enum PageMode
		{
			View = 1,
			Edit = 2
		}

        private readonly IInstitutionRepository _institutionRepository;

        public InstitutionController()
        {
            _institutionRepository = new InstitutionRepository();
        }

        public InstitutionController(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

		[AuthorizeAdm(Roles = UserRole.EduUser)]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult Edit()
		{
            return RedirectToActionPermanent("EditNew");
			//return View("Edit", CreateInstituteCommonInfoModel(PageMode.Edit));
		}

		[HttpPost]
		[AuthorizeAdm(Roles = UserRole.EduUser)]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult Edit(InstituteCommonInfoViewModel viewModel)
		{
			if (viewModel.HasHostel && viewModel.HostelCapacity == null && ModelState["HostelCapacity"].Errors.Count == 0)
				ModelState["HostelCapacity"].Errors.Add(@"Поле ""Количество мест"" обязательно для заполнения.");
			if (!ModelState.IsValid)
				return Json(new AjaxResultModel(ModelState));
			if (viewModel.HostelFecDocumentDeleted)
				DeleteHostelFile();

			return Json(viewModel.SaveToDb());
		}

		/// <summary>
		/// Контроллер просмотра использует View для редактирования, указывая во ViewMode флаг просмотра.
		/// </summary>
		//[GeneratorPortletLink(typeof(PortletLinkHelper), "InstitutionTabLink", UseControllersArgs = false, MethodParams = new object[] { PortletType.InstitutionInfoTab, 0 })]
		//[AuthorizeAdm(Roles = UserRole.EduUser)]
		//public ActionResult View(InstituteCommonInfoViewModel viewModel)
		//{
		//    if (viewModel == null || viewModel.InstitutionID == 0)
		//        return View("Index", CreateInstituteCommonInfoModel(PageMode.View));

		//    viewModel.IsEdit = false;
		//    return View("Index", viewModel);
		//}
		
		[AuthorizeAdm(Roles = UserRole.EduUser)]
		public ActionResult View(int? historyID)
		{
            return RedirectToActionPermanent("ViewNew", new { historyID });
			//return View("Index", CreateInstituteCommonInfoModel(PageMode.View, historyID));
		}
	
		[Authorize(Roles = UserRole.RonInst)]
		public ActionResult ViewRonInst(int? institutionID)
		{
			int instID = institutionID.HasValue ? institutionID.Value : InstitutionHelper.GetInstitutionID(false);
			using (var context = new InstitutionsEntities())
			{
				return View("Index", context.LoadInstitutionCommonInfoVM(instID, false));
			}			
		}

		[AuthorizeAdm(Roles = UserRole.FBDAdmin + "," + UserRole.FbdRonUser)]
		public ActionResult ViewAdmPopup(int? institutionID)
		{
			return PartialView("Common/CommonInfoControl", CreateInstituteCommonInfoModel(PageMode.View, null, institutionID));
		}

		private InstituteCommonInfoViewModel CreateInstituteCommonInfoModel(PageMode mode, int? historyID = null, int? institutionID = null)
		{
			var instID = institutionID ?? InstitutionID;
			using (var context = new InstitutionsEntities())
			{
				InstituteCommonInfoViewModel model;
				if ((historyID ?? 0) > 0)
				{
					InstitutionHistory institution = context.LoadInstitutionHistory(instID, historyID ?? 0);
					model = new InstituteCommonInfoViewModel(institution, mode == PageMode.Edit);
				}
				else
				{
					Institution institution = context.LoadInstitution(instID);
					model = new InstituteCommonInfoViewModel(institution, mode == PageMode.Edit);
				}

				if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
				{
					var ch = context.GetHistoriesForInstitution(instID)
						.Select(x => new { ID = x.HistoryID, Name = x.DateChanged.ToString("dd MMMM yyyy, HH:mm:ss") }).ToList();
					//это и так текущая информация
					if (ch.Count > 0)
						ch[0] = new { ID = 0, Name = "Текущая информация" };
					model.ChangeHistories = ch.ToArray();
				}

				return model;
			}
		}
		
		[Authorize]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public ActionResult DeleteHostelFile()
		{
			using (var context = new InstitutionsEntities())
			{
				var institution = context.Institution
					.Include(i => i.HostelAttachment)
					.Include(i => i.InstitutionAccreditation)
					.Include(i => i.InstitutionAccreditation.Select(ac => ac.Attachment))
					.Include(i => i.InstitutionLicense)
					.Include(i => i.InstitutionLicense.Select(il => il.Attachment))
					.First(x => x.InstitutionID == InstitutionID);
				try
				{
					institution.DeleteFile(context, Institution.FileTypes.HostelCondition);
					context.SaveChanges();
					context.AddChangesToHistory(InstitutionID);
				}
				catch (Exception exc)
				{
					LogHelper.Log.Debug("Can't delete file with type {0} for specified institute {1}".FormatWith(
						Institution.FileTypes.HostelCondition, InstitutionID), exc);
					throw;
				}
			}

			return Json(new AjaxResultModel());
		}

        [HttpGet]
        [AuthorizeAdm(Roles = UserRole.EduUser)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult EditNew()
        {
            var dto = _institutionRepository.GetInstitutionInfoDto(InstitutionID);
            var model = new InstitutionInfoViewModel(dto);
            return View(model);
        }

        [HttpGet]
        [AuthorizeAdm(Roles = UserRole.EduUser)]
        public ActionResult ViewNew()
        {
            var dto = _institutionRepository.GetInstitutionInfoDto(InstitutionID);
            var model = new InstitutionInfoViewModel(dto);
            ViewData["isReadOnly"] = true;
            return View(model);
        }

        [HttpPost]
        [AuthorizeAdm(Roles = UserRole.EduUser)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult EditNew(InstitutionInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = model.GetDto();
                _institutionRepository.UpdateInstitutionInfo(InstitutionID, dto);
                return RedirectToAction("View");
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizeAdm(Roles = UserRole.EduUser)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult AddInstitutionDocument(InstitutionInfoUploadDocumentViewModel model)
        {
            if (ModelState.IsValid)
            {
                _institutionRepository.AddInstitutionDocument(InstitutionID, model.GetDto(), model.Year);
                return Json(new AjaxResultModel());
            }
            else {
                //return Json(new AjaxResultModel(ModelState));
                return new AjaxResultModel(ModelState).SetMessageFromErrors();
            }
        }

        [HttpPost]
        public ActionResult DocumentsList()
        {
            var model = _institutionRepository.GetInstitutionDocumentsList(InstitutionID).Select(x => new InstitutionInfoYearDocumentViewModel(x));
            return PartialView("EditInstitutionDocumentsList", model);
        }

        [HttpPost]
        [AuthorizeAdm(Roles = UserRole.EduUser)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult DeleteInstitutionDocument(int attachmentId)
        {
            _institutionRepository.DeleteInstitutionDocument(InstitutionID, attachmentId);
            return DocumentsList();
        }
    }
}