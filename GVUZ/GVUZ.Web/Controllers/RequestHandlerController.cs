using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Model.Institutions;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using GVUZ.Web.ContextExtensions;
using GVUZ.DAL.Dapper.Repository.Interfaces.AllowedDirections;
using GVUZ.DAL.Dapper.Repository.Model.AllowedDirections;
using System.Linq;
using GVUZ.DAL.Dapper.Repository.Interfaces.Dictionary;
using GVUZ.DAL.Dapper.Repository.Model.Dictionary;
using GVUZ.DAL.Dapper.Repository.Interfaces.Institution;
using GVUZ.DAL.Dapper.Repository.Model.Institution;

namespace GVUZ.Web.Controllers
{
    public class RequestHandlerController : BaseController
    {
        private readonly IAllowedDirectionsRepository _allowedDirectionsRepository;
        private readonly IInstitutionRepository _institutionRepository;

        public RequestHandlerController()
        {
            _allowedDirectionsRepository = new AllowedDirectionsRepository();
            _institutionRepository = new InstitutionRepository();
        }

        public RequestHandlerController(IAllowedDirectionsRepository allowedDirectionsRepository, IInstitutionRepository institutionRepository)
        {
            _allowedDirectionsRepository = allowedDirectionsRepository;
            _institutionRepository = institutionRepository;
        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        public ActionResult ReqList()
        {
            return RedirectToActionPermanent("RequestList");
            //using (InstitutionsEntities dbContext = new InstitutionsEntities())
            //{
            //    return View("../Admission/RequestList", dbContext.InitialFillRequestListViewModel(InstitutionID));
            //}
        }

        [Authorize]
        [HttpPost]
        public AjaxResultModel MakeEditable(int? institutionid)
        {
            if (institutionid == null)
                return new AjaxResultModel("Invalid value of Institution ID.");
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.AtoU(institutionid ?? 0);
            }

        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public AjaxResultModel GetRequestList(RequestListViewModel model)
        {
            using (var dbContext = new InstitutionsEntities())
            {
                return dbContext.GetRequestList(model);
            }
        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public AjaxResultModel GetRequest(int? institutionid)
        {
            using (var dbContext = new InstitutionsEntities())
            {
                return dbContext.FillRequestList(institutionid ?? 0);
            }
        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public AjaxResultModel AddDirection(int? did, int? inid, int? adtype)
        {
            using (var dbContext = new InstitutionsEntities())
            {
                return dbContext.AcceptRequest(did ?? 0, inid ?? 0, adtype ?? 0);
            }
        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public AjaxResultModel DenyRequest(int? did,string comment, int? inid)
        {
            using (var dbContext = new InstitutionsEntities())
            {
                return dbContext.DenyRequestAdmin(did ?? 0, inid ?? 0, comment);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        #region FIS-1345 переработка администрирования зявок

        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpGet]
        public ActionResult RequestList()
        {
            var model = new InstitutionDirectionRequestListViewModel();
            return View("../Admission/InstitutionDirectionRequestList", model);
        }

        /// <summary>
        /// Данные для грида со списком заявок
        /// </summary>
        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public ActionResult LoadRequestListData(InstitutionDirectionRequestQueryViewModel query)
        {
            var items = _allowedDirectionsRepository.GetDirectionsRequestsPaged(query.Pager, query.Sort);

            var model = new InstitutionDirectionRequestListViewModel
            { 
                Pager = query.Pager,
                Records = items.Select(x => new InstitutionDirectionRequestRecordViewModel(x)).ToList(),
                SortDescending = query.Sort.SortDescending,
                SortKey = query.Sort.SortKey,
            };

            return Json(model);
        }

        /// <summary>
        /// Список направлений в заявках от ОО
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        [HttpPost]
        [Authorize(Roles = UserRole.FBDAdmin)]
        public ActionResult RequestDetails(int institutionId)
        {
            var items = _allowedDirectionsRepository.GetDirectionsRequestDetails(institutionId);
            var ins = _institutionRepository.GetInstitutionName(institutionId);

            var model = new InstitutionDirectionRequestDetailsViewModel(items)
            {
                InstitutionId = ins.Id,
                InstitutionName = ins.Name
            };
            return Json(model);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.FBDAdmin)]
        public ActionResult ViewDirectionRequestDialog(int institutionId, int requestId)
        {
            var dto = _allowedDirectionsRepository.GetDirectionRequestById(institutionId, requestId);
            var model = new InstitutionDirectionRequestViewModel(dto);
            return PartialView("../Admission/ViewDirectionRequestDialog", model);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.FBDAdmin)]
        public ActionResult ApproveDirectionRequest(InstitutionDirectionRequestViewModel model)
        {
            _allowedDirectionsRepository.ApproveDirectionRequest(model.InstitutionId, model.RequestId);
            return new JsonResult();
        }

        [HttpPost]
        [Authorize(Roles = UserRole.FBDAdmin)]
        public ActionResult DenyDirectionRequest(InstitutionDirectionRequestViewModel model)
        {
            _allowedDirectionsRepository.DenyDirectionRequest(model.InstitutionId, model.RequestId, model.Comment);
            return new JsonResult();
        }
        #endregion
    }
}
