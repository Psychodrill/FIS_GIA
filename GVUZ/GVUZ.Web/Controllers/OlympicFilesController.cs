using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.FileStorage;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels.OlympicFiles;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Administration")]
    [Authorize(Roles = UserRole.FBDAdmin + "," + UserRole.FbdRonUser + "," + UserRole.FbdAuthorizedStaff)]
    public class OlympicFilesController : BaseController
    {
        private readonly IFileStorage _fileStorage;

        public OlympicFilesController()
        {
            _fileStorage = ServiceLocator.Current.GetInstance<IFileStorage>();
        }


        [HttpGet]
        public ActionResult Index()
        {
            if (!ConfigHelper.ShowOlympicFilesUpload())
            {
                return HttpNotFound();
            }

            var model = new OlympicFilesListViewModel
                {
                    Records = _fileStorage.GetAll(InstitutionID, true).Select(f => new OlympicFileRecordViewModel
                        {
                            Comments = f.Comments,
                            FileName = f.FileName,
                            Id = f.FileId,
                            UploadDate = f.UploadDate
                        }).ToList()
                };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddDialog()
        {
            if (!ConfigHelper.ShowOlympicFilesUpload())
            {
                return HttpNotFound();
            }

            return PartialView("AddDialog", new OlympicFileUploadViewModel());
        }

        [HttpPost]
        public ActionResult Submit(OlympicFileUploadViewModel model)
        {
            if (ModelState.IsValidField("UploadFile"))
            {
                try
                {
                    _fileStorage.Add(InstitutionID, model.UploadFile.InputStream, model.UploadFile.FileName, model.Comments);
						  return new FileJsonResult(new { });
                } catch {
						 return new FileJsonResult("Ошибка сохранения файла");
                }
            }
				return new FileJsonResult("Не указан файл");
        }

        [HttpGet]
        public ActionResult Download(string id)
        {
            if (!ConfigHelper.ShowOlympicFilesUpload())
            {
                return HttpNotFound();
            }

            var file = _fileStorage.Get(InstitutionID, id, false);

            string fileName = file.FileName;
            const string contentType = "application/octet-stream";

            fileName = fileName.Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\n", string.Empty).Replace(" ", "_").Replace("\"", string.Empty);
            Response.Clear();
            if (Request.Browser.Browser == "IE" || Request.Browser.Browser == "InternetExplorer")
            {
                string attachment = string.Format("attachment; filename=\"{0}\"", Server.UrlPathEncode(fileName));
                Response.AddHeader("Content-Disposition", attachment);
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            }

            Response.ContentType = contentType;
            Response.Charset = "utf-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.Headers.Add("Content-length", file.FileSize.ToString(CultureInfo.InvariantCulture));
            
            _fileStorage.WriteContentTo(InstitutionID, file.FileId, Response.OutputStream);
            Response.OutputStream.Flush();
            Response.End();

            return null;
        }
    }
}