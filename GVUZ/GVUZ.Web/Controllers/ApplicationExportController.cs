using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using GVUZ.AppExport.Services;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Web.Controllers
{
    [Authorize(Roles = UserRole.FBDAdmin + "," + UserRole.FbdRonUser + "," + UserRole.FbdAuthorizedStaff)]
    public class ApplicationExportController : BaseController
    {
        private readonly IApplicationExportRequestRepository _requestRepository;

        public ApplicationExportController()
        {
            _requestRepository = ServiceLocator.Current.GetInstance<IApplicationExportRequestRepository>();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            var model = new ApplicationExportViewModel();
            if (InstitutionID == 0)
            {
                model.IsDenied = true;
            }
            else
            {
                PopulateViewModel(model);
                model.SelectedYear = DateTime.Now.Year;    
            }

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Submit(ApplicationExportViewModel model)
        {
            if (InstitutionID == 0)
            {
                throw new ApplicationException("Пользователь должен являться представителем ВУЗа");
            }

            if (ModelState.IsValid)
            {
                if (_requestRepository.HasPending(InstitutionID, model.SelectedYear.Value))
                {
                    ModelState.AddModelError(string.Empty,
                                             string.Format(
                                                 "Обработка запроса на выгрузку данных за {0} год уже выполняется",
                                                 model.SelectedYear.Value));

                    PopulateViewModel(model);
                }
                else
                {
                    _requestRepository.AddNew(InstitutionID, model.SelectedYear.Value);
                    return RedirectToAction("Index");        
                }
            }

            return View("Index", model);
            
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download()
        {
            if (InstitutionID == 0)
            {
                throw new ApplicationException("Пользователь должен являться представителем ВУЗа");
            }

            var lastRequest = _requestRepository.FindByInstitution(InstitutionID).FirstOrDefault();

            if (lastRequest != null && lastRequest.RequestStatus == ApplicationExportRequestStatus.Complete)
            {
                var file = GetOutputFilePath(lastRequest.RequestDate, lastRequest.RequestId);

                if (file.Exists)
                {
                    Response.Headers.Add("Content-disposition", string.Format("attachment; filename=export_{0}.xml", lastRequest.RequestDate.ToString("yyyyMMdd_HHmmss")));
                    Response.Headers.Add("Content-length", file.Length.ToString());
                    return new FileStreamResult(file.Open(FileMode.Open, FileAccess.Read), "text/xml");
                }
            }

            return new HttpNotFoundResult();
        }

        private void PopulateViewModel(ApplicationExportViewModel model)
        {
            var lastRequest = _requestRepository.FindByInstitution(InstitutionID).FirstOrDefault();

            if (lastRequest != null)
            {
                model.IsExportInProgress = lastRequest.RequestStatus == ApplicationExportRequestStatus.New ||
                                           lastRequest.RequestStatus == ApplicationExportRequestStatus.Enqueued ||
                                           lastRequest.RequestStatus == ApplicationExportRequestStatus.Processing;
                model.IsExportComplete = lastRequest.RequestStatus == ApplicationExportRequestStatus.Complete;
                model.IsExportFailed = lastRequest.RequestStatus == ApplicationExportRequestStatus.Error;

                if (model.IsExportComplete || model.IsExportFailed)
                {
                    model.ExportedFileDate = lastRequest.RequestDate;
                }
            }
        }

        private string GetExportFileStorageRoot()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.AppExportFileStorage))
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FisAppExport");
            }

            return Properties.Settings.Default.AppExportFileStorage;
        }

        private FileInfo GetOutputFilePath(DateTime date, Guid requestId)
        {
            string fileName = string.Format("{0}.xml", requestId.ToString("N"));
            string fileFolderName = Path.Combine(GetExportFileStorageRoot(), date.ToString("yyyyMMdd"));
            return new FileInfo(Path.Combine(fileFolderName, fileName));
        }

    }
}