using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.ServiceModel.Import;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Administration")]
    [Authorize(Roles = UserRole.FBDAdmin + "," + UserRole.FbdRonUser + "," + UserRole.FbdAuthorizedStaff)]
	public class ImportController : BaseController
	{
		public ActionResult ImportPackageList()
		{
			using (var dbContext = new ImportEntities())
			{
                if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
				    return View("ImportList", dbContext.InitialFillImportListViewModell());
                else
                    return View("ImportList", dbContext.InitialFillImportListViewModell(InstitutionID));
			}
		}

		[HttpPost]
		public ActionResult GetImportList(ImportListViewModel model)
		{
			using (var dbContext = new ImportEntities())
			{
                if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
				    return dbContext.GetImportListModel(model);
                else
                    return dbContext.GetImportListModel(model, true, InstitutionID);
			}
		}

		[HttpPost]
		public ActionResult GenerateXmlListForExtended(ImportListViewModel model)
		{
			if (!ModelState.IsValid)
				return new AjaxResultModel(ModelState);

			using (var dbContext = new ImportEntities())
			{
				dbContext.GetImportListModel(model, false);

				var exporter = new ImportPackageListXmlExporter();
				exporter.AddFilterRow(model);
				exporter.AddHeader();

				foreach (var package in model.ImportPackages)
				{
					exporter.AddRow(new ImportPackageListXmlExporter.RowData
					{
						ID = package.ID,
						InctitutionName = package.InstitutionName,
						DateSend = package.DateSent,
						DateProcessing = package.DateProcessing,
						Type = package.Type,
						Content = package.Content,
						Status = package.Status
					});
				}

				string fileNamePart = exporter.SaveToTempFile();
				return new AjaxResultModel { Data = fileNamePart };
			}
		}

		[Authorize]
		public ActionResult ImportInfo(int? packageID)
		{
			if (packageID.HasValue)
			{
				using (var dbContext = new ImportEntities())
				{
					return View(dbContext.GetImportInfo(packageID.Value, InstitutionID));
				}
			}

			return new EmptyResult();
		}

        [HttpPost]
        public ActionResult GetImportInfo(int? packageID, int? SortID)
        {
            using (var dbContext = new ImportEntities())
            {
                return dbContext.GetImportInfoList(packageID.Value, SortID.Value, InstitutionID);
            }
        }

        public ActionResult GetXmlList(string xmlListName, int? tabId)
		{
			if (string.IsNullOrEmpty(xmlListName))
				return new AjaxResultModel("Ошибка: не указано имя файла");

			return new FixedFileResult(Path.Combine(Path.GetTempPath(), xmlListName + ".xls"), "application/msexcel",
					string.Format("Поиск запросов_{0:yyyy-MM-dd-HH-mm}.xml", DateTime.Now));
		}

		private class FixedFileResult : ActionResult
		{
			private readonly string _filePath;
			private readonly byte[] _fileContent;
			private readonly string _contentType;
			private readonly string _fileName;

			public FixedFileResult(string path, string contentType, string fileName)
			{
				_filePath = path;
				_contentType = contentType;
				_fileName = fileName;
			}

			public FixedFileResult(byte[] fileContent, string contentType, string fileName)
			{
				_fileContent = fileContent;
				_contentType = contentType;
				_fileName = fileName;
			}

			public override void ExecuteResult(ControllerContext context)
			{
				if (context == null)
					throw new ArgumentNullException("context");
				HttpResponseBase response = context.HttpContext.Response;
				response.ContentType = _contentType;
				if (!string.IsNullOrEmpty(_fileName))
				{
					context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(_fileName, Encoding.UTF8).Replace("+", "%20"));
				}

				if (_filePath != null)
					response.WriteFile(_filePath);
				else
				{
					response.BinaryWrite(_fileContent);
				}
			}
		}

		public ActionResult GetRawImportPackage(int? packageID)
		{
			if (packageID.HasValue)
			{
				using (var dbContext = new ImportEntities())
				{
				    string data = dbContext.GetPackageXml(packageID.Value);

                    if (!string.IsNullOrEmpty(data))
                    {
                        return new FixedFileResult(
                            Encoding.UTF8.GetBytes(data),
                            "text/xml", "packagedata_" + packageID.Value.ToString() + ".xml");    
                    }
				}
			}

			return new EmptyResult();
		}
	}
}