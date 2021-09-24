using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels.CompositionResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GVUZ.DAL.Dapper.Repository.Model;
using System.Configuration;
using ICSharpCode.SharpZipLib.Zip;
using FogSoft.Helpers;
using System.Threading;
using GVUZ.CompositionExportModel;
using GVUZ.Helper.MVC;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Administration")]
    [Authorize(Roles = UserRole.FBDAdmin + "," + UserRole.FbdRonUser + "," + UserRole.FbdAuthorizedStaff)]
    public class CompositionResultsExportController : BaseController
    {
        static readonly int GetFileTryCount = 10;
        static readonly int GetFileTryDelay = 2;
        static readonly int GetFileTotalTryCount = 100;

        string CompositionOldDrive = ConfigurationManager.AppSettings["CompositionOldDrive"];
        string CompositionNewDrive = ConfigurationManager.AppSettings["CompositionNewDrive"];

        

        static CompositionResultsExportController()
        {
            GetFileTryCount = AppSettings.Get("Composition.GetFileTryCount", 10);
            GetFileTryDelay = AppSettings.Get("Composition.GetFileTryDelay", 2);
            GetFileTotalTryCount = AppSettings.Get("Composition.GetFileTotalTryCount", 100);

        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            var model = new CompositionResultsListViewModel
                {
                    Filter = CompositionResultSQL.GetCompositionResultsFilter(InstitutionID)
                };

            return View(model);
        }

        [HttpPost]
        public ActionResult LoadCompositionResultsRecords(CompositionResultsQueryViewModel model)
        {
            return Json(CompositionResultSQL.GetCompositionResultRecords(InstitutionID, model));
        }

        [HttpPost]
        public ActionResult ExportCsv(string submitModel)
        {
            try
            {
                var isoDateFormat = new IsoDateTimeConverter();
                isoDateFormat.DateTimeFormat = "dd.MM.yyyy";

                var model = JsonConvert.DeserializeObject<CompositionResultsQueryViewModel>(HttpUtility.UrlDecode(submitModel), isoDateFormat);

                var list = CompositionResultSQL.GetCompositionResultRecords(InstitutionID, model, true);

                string fileName = string.Format("Результаты_сочинений_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));
                const string contentType = "text/csv";

                using (var download = new ResponseFileWriter(Server, Request, Response, fileName, contentType))
                {
                    using (var writer = new StreamWriter(download.Output))
                    {
                        CompositionResultSQL.WriteCsv(writer, list.Records);
                    }
                }
            }
            catch (Exception)
            {
                ViewData["JavaScriptErrorMessage"] = "Ошибка экспорта CSV-файла";
                return View("JavaScriptError");
            }

            return null;
        }

        [HttpPost]
        public ActionResult ExportHtml(string submitModel)
        {
            try
            {
                var isoDateFormat = new IsoDateTimeConverter();
                isoDateFormat.DateTimeFormat = "dd.MM.yyyy";

                var model = JsonConvert.DeserializeObject<CompositionResultsQueryViewModel>(HttpUtility.UrlDecode(submitModel), isoDateFormat);

                var list = CompositionResultSQL.GetCompositionResultRecords(InstitutionID, model, true);

                string fileName = string.Format("Результаты_сочинений_{0}.html", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string htmlTitle = string.Format("Результаты сочинений {0}", DateTime.Now.ToString("yyyyMMddHHmmss"));

                const string contentType = "text/html";

                using (var download = new ResponseFileWriter(Server, Request, Response, fileName, contentType))
                {
                    using (var writer = new StreamWriter(download.Output, Encoding.UTF8))
                    {
                        CompositionResultSQL.WriteHtml(Server, htmlTitle, writer, list.Records);
                    }
                }
            }
            catch
            {
                ViewData["JavaScriptErrorMessage"] = "Ошибка экспорта HTML-файла";
                return View("JavaScriptError");
            }

            return null;
        }

        [HttpPost]
        public ActionResult UpdateCompositionResults()
        {
            try
            {
                CompositionResultSQL.UpdateCompositionResults(InstitutionID);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }



        [HttpPost]
        public ActionResult ExportCompositions(string submitModel)
        {
            ICompositionExportService service;
            try
            {
                service = new CompositionExportServiceClient();

                string user = ConfigurationManager.AppSettings["CompositionUser"];
                string password = ConfigurationManager.AppSettings["CompositionPassword"];

                string fileName = string.Format("Результаты_сочинений_{0}.zip", DateTime.Now.ToString("yyyyMMddHHmmss"));
                const string contentType = "zip";

                var aids = HttpUtility.UrlDecode(submitModel).Replace("[", "(").Replace("]", ")").Replace(";", ",");
                var repository = new ApplicationRepository();
                var list = repository.GetCompositionPaths(aids);

                var result = service.GetCompositions(list);

                if (result.HasData)
                {
                    repository.UpdateCompositionDates(aids);

                    var download = new ResponseFileWriter(Server, Request, Response, fileName, contentType);
                    download.Output.Write(result.File, 0, result.File.Length);
                }
                else
                {
                    throw new Exception("Не удалось получить доступ к бланкам сочинений или бланки сочинений не найдены");
                }

                //bool hasData = false;
                //using (var mem = new MemoryStream())
                //{
                //    using (var zip = new ZipOutputStream(mem))
                //    {
                //        zip.IsStreamOwner = false;
                //        zip.SetLevel(3);

                //        var totalTryNumber = 1;
                //        foreach (var item in list)
                //        {
                //            if (!string.IsNullOrEmpty(item.CompositionPaths))
                //            {
                //                var i = 1;
                //                foreach (var path in item.CompositionPaths.Split(';'))
                //                {
                //                    var trimmedPath = path.Trim();
                //                    if (CompositionNewDrive != null && CompositionNewDrive != "")
                //                    {
                //                        trimmedPath = trimmedPath.Replace(CompositionOldDrive, CompositionNewDrive);
                //                    }
                //                    if (trimmedPath != "")
                //                    {
                //                        var name = string.Format("{0}_{1}_{2}_{3}_{4}_{5}.png",
                //                            item.LastName, item.FirstName, item.MiddleName, item.DocumentSeries, item.DocumentNumber, i++);

                //                        byte[] file = null;
                //                        int tryNumber = 1;

                //                        while (tryNumber <= GetFileTryCount)
                //                        {
                //                            try
                //                            {
                //                                file = new NetworkIO("", "").ReadFile(trimmedPath);
                //                                LogHelper.Log.Debug(string.Format("Успешный факт анонимного получения бланка сочинения: {0}, entrantID={1}", trimmedPath, item.EntrantID));
                //                                break;
                //                            }
                //                            catch (IOException ioEx)
                //                            {
                //                                LogHelper.Log.Error(string.Format("Ошибка анонимного получения бланка сочинения: {0}, entrantID={1}, попытка: {2}, общая попытка: {3}", trimmedPath, item.EntrantID, tryNumber, totalTryNumber), ioEx);
                //                                try
                //                                {
                //                                    file = new NetworkIO(user, password).ReadFile(trimmedPath);
                //                                    LogHelper.Log.Debug(string.Format("Успешный факт анонимного получения бланка сочинения: {0}, entrantID={1}", trimmedPath, item.EntrantID));
                //                                    break;
                //                                }
                //                                catch (Exception ex)
                //                                {
                //                                    LogHelper.Log.Error(string.Format("Ошибка авторизованного получения бланка сочинения: {0}, entrantID={1}, попытка: {2}, общая попытка: {3}", trimmedPath, item.EntrantID, tryNumber, totalTryNumber), ex);
                //                                    try
                //                                    {
                //                                        file = new NetworkIO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).ReadFile(trimmedPath);
                //                                    }
                //                                    catch(Exception iEx)
                //                                    {
                //                                        LogHelper.Log.Error(string.Format("Ошибка криво-авторизованного получения бланка сочинения: {0}, entrantID={1}, попытка: {2}, общая попытка: {3}", trimmedPath, item.EntrantID, tryNumber, totalTryNumber), iEx);
                //                                    }

                //                                }
                //                            }
                //                            finally
                //                            {
                //                                LogHelper.Log.Error(string.Format("Ошибка получения бланка сочинения: {0}, entrantID={1}, попытка: {2}, общая попытка: {3}", trimmedPath, item.EntrantID, tryNumber, totalTryNumber));
                //                                tryNumber++;
                //                                totalTryNumber++;
                //                                if (!(tryNumber >= GetFileTryCount || totalTryNumber >= GetFileTotalTryCount))
                //                                    Thread.Sleep(1000 * GetFileTryDelay);
                //                            }
                //                        }

                //                        if ((file != null) && (file.Length > 0))
                //                        {
                //                            var newEntry = new ZipEntry(name);
                //                            newEntry.DateTime = DateTime.Now;

                //                            zip.PutNextEntry(newEntry);
                //                            zip.Write(file, 0, file.Length);
                //                            zip.CloseEntry();

                //                            hasData = true;
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        if (hasData)
                //        {                            
                //            repository.UpdateCompositionDates(aids);
                //        }
                //        zip.Close();
                //        mem.Position = 0;
                //    }

                //if (hasData)
                //{
                //    var download = new ResponseFileWriter(Server, Request, Response, fileName, contentType);
                //    download.Output.Write(mem.ToArray(), 0, (int)mem.Length);
                //}
                //}
                //if (!hasData)
                //    throw new Exception("Не удалось получить доступ к бланкам сочинений или бланки сочинений не найдены");
            }
            catch (Exception ex)
            {
                ViewData["JavaScriptErrorMessage"] = String.Format("Ошибка экспорта архива бланков сочинений: {0}", ex.Message);
                return View("JavaScriptError");
            }

            return null;
        }

    }
}