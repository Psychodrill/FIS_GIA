using GVUZ.DAL.Dapper.Repository.Model;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Applications;
using GVUZ.Model.Entrants;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Web.Filters;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.SQLDB;
using GVUZ.Web.ViewModels;
using GVUZ.Web.ViewModels.ApplicationsList;
using GVUZ.Web.ViewModels.OrderOfAdmission;
using GVUZ.Web.ViewModels.Shared;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using NPOI.HSSF.UserModel;
using System;
//using log4net;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Applications")]
    [AuthorizeAdm(Roles = UserRole.EduUser)]
    public class InstitutionApplicationController : BaseController
    {
        //private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static readonly ILog xlsLogger = log4net.LogManager.GetLogger("InstitutionApplicationController");
        /// <summary>
        /// Начальная страница списка заявлений
        /// </summary>
        [NoCache]
        public ActionResult ApplicationList()
        {
            var result = new NewInstitutionApplicationListViewModel();
            try
            {
                result = EntrantApplicationSQL.GetInstitutionApplicationListViewModel();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ApplicationList error");
            }

            return View("InstitutionApplicationList", result);
        }

        public ActionResult NewList(int? highlightApplicationId, int? initialPage = null)
        {
            var model = new NewApplicationsListViewModel
            {
                Filter = EntrantApplicationSQL.GetNewApplicationsFilter(InstitutionID, highlightApplicationId),
                InstitutionId = InstitutionID,
                HighlightApplicationId = highlightApplicationId,
                Pager = new PagerViewModel
                {
                    CurrentPage = initialPage.GetValueOrDefault(1)
                }
            };
            return PartialView("InstitutionApplication/NewList", model);
        }

        [HttpPost]
        public ActionResult LoadApplicationNewRecords(NewApplicationsQueryViewModel model)
        {
            return Json(EntrantApplicationSQL.GetNewApplicationsRecords(InstitutionID, model));
        }

        public ActionResult UncheckedList(int? highlightApplicationId, int? initialPage)
        {
            var model = new UncheckedApplicationsListViewModel
            {
                Filter = EntrantApplicationSQL.GetUncheckedApplicationsFilter(InstitutionID, highlightApplicationId),
                HighlightApplicationId = highlightApplicationId,
                Pager = new PagerViewModel
                {
                    CurrentPage = initialPage.GetValueOrDefault(1)
                }
            };

            return PartialView("InstitutionApplication/UncheckedList", model);
        }

        [HttpPost]
        public ActionResult LoadApplicationUncheckedRecords(UncheckedApplicationsQueryViewModel model)
        {
            return Json(EntrantApplicationSQL.GetUncheckedApplicationsRecords(InstitutionID, model));
        }

        public ActionResult RevokedList(int? highlightApplicationId, int? initialPage)
        {
            var model = new RevokedApplicationsListViewModel
            {
                Filter = EntrantApplicationSQL.GetRevokedApplicationsFilter(InstitutionID, highlightApplicationId),
                HighlightApplicationId = highlightApplicationId,
                Pager = new PagerViewModel
                {
                    CurrentPage = initialPage.GetValueOrDefault(1)
                }
            };

            return PartialView("InstitutionApplication/RevokedList", model);
        }

        [HttpPost]
        public ActionResult LoadApplicationRevokedRecords(RevokedApplicationsQueryViewModel model)
        {
            return Json(EntrantApplicationSQL.GetRevokedApplicationsRecords(InstitutionID, model));
        }

        public ActionResult AcceptedList(int? highlightApplicationId, int? initialPage)
        {
            var model = new AcceptedApplicationsListViewModel
            {
                Filter = EntrantApplicationSQL.GetAcceptedApplicationsFilter(InstitutionID, highlightApplicationId),
                HighlightApplicationId = highlightApplicationId,
                Pager = new PagerViewModel
                {
                    CurrentPage = initialPage.GetValueOrDefault(1)
                }
            };

            return PartialView("InstitutionApplication/AcceptedList", model);
        }

        [HttpPost]
        public ActionResult LoadApplicationAcceptedRecords(AcceptedApplicationsQueryViewModel model)
        {
            return Json(EntrantApplicationSQL.GetAcceptedApplicationsRecords(InstitutionID, model));
        }

        public ActionResult RecommendedList()
        {
            var model = new RecommendedApplicationsListViewModel
            {
                Filter = EntrantApplicationSQL.GetRecommendedApplicationsFilter(InstitutionID)
            };

            return PartialView("InstitutionApplication/RecommendedList", model);
        }

        //[HttpPost]
        //public ActionResult LoadApplicationRecommendedRecords(RecommendedApplicationsQueryViewModel model)
        //{
        //    return Json(EntrantApplicationSQL.GetRecommendedApplicationsRecords(InstitutionID, model));
        //}

        /// <summary>
        /// Страница расширенного поиска отдельно (для ссылки под логином)
        /// </summary>
        /// <returns></returns>
        [NoCache]
        public ActionResult ExtendedApplicationSearch(string q)
        {
            var model = new SearchApplicationsListViewModel
            {
                Filter = EntrantApplicationSQL.GetSearchApplicationsFilter(InstitutionID, q)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult LoadApplicationSearchRecords(SearchApplicationsQueryViewModel model)
        {
            return Json(EntrantApplicationSQL.GetSearchApplicationsRecords(InstitutionID, model));
        }

        #region Диалоги в гридах списков заявлений
        [HttpPost]
        public ActionResult GetAcceptableApplications(int[] applicationId)
        {
            var model = EntrantApplicationSQL.GetAcceptApplicationsListViewModel(InstitutionID, applicationId);
            return Json(new { model.ApplicationRecords });
        }

        [HttpPost]
        public ActionResult AcceptApplications(AcceptApplicationsListViewModel model)
        {
            EntrantApplicationSQL.AcceptApplications(InstitutionID, model);
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult GetRevokableApplications(int[] applicationId)
        {
            var model = EntrantApplicationSQL.GetRevokeApplicationsListViewModel(InstitutionID, applicationId);
            return Json(new { model.ApplicationRecords, model.ReturnDocumentsTypes });
        }

        [HttpPost]
        public ActionResult RevokeApplications(RevokeApplicationsListViewModel model)
        {
            EntrantApplicationSQL.RevokeApplications(InstitutionID, model);
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult CancelRevokeApplications(int[] applicationId)
        {
            EntrantApplicationSQL.CancelRevoke(InstitutionID, applicationId);
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult CheckApplications(int[] applicationId)
        {
            return Json(EntrantApplicationSQL.CheckApplications(InstitutionID, applicationId));
        }

        /// <summary>
        /// Включение в список рекомендованных - загрузка данных для диалога
        /// </summary>
        [HttpPost]
        public ActionResult GetIncludeRecommendedList(int? applicationId)
        {
            return Json(EntrantApplicationSQL.GetIncludeRecommendedListViewModel(applicationId.GetValueOrDefault(), InstitutionID));
        }

        /// <summary>
        /// Включение в список рекомендованных - выполнение действия
        /// </summary>
        /// <param name="model">Модель данных для включения в список <see cref="IncludeRecommendedListSubmitViewModel"/></param>
        [HttpPost]
        public ActionResult IncludeRecommendedList(IncludeRecommendedListSubmitViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedItems == null || model.SelectedItems.Length == 0)
                {
                    return Json(new { success = false, message = "Для включения в список рекомендованных должно быть отмечено хотя бы одно условие приема" });
                }

                if (EntrantApplicationSQL.IncludeInRecommendedList(InstitutionID, model))
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Данное заявление уже включено в список рекомендованных на указанную комбинацию условий приема" });
            }

            return Json(new { success = false });
        }

        /// <summary>
        /// Исключение заявления из списка рекомендованных
        /// </summary>
        /// <param name="recListId">Id условия приема (записи в RecommendedLists)</param>
        /// <param name="applicationId">Id заявления</param>
        [HttpPost]
        public ActionResult ExcludeRecommendedList(int? recListId, int? applicationId)
        {
            EntrantApplicationSQL.ExcludeFromRecommendedList(recListId, applicationId, InstitutionID);
            return Json(new { success = true });
        }
        #endregion 

        public class FixedFileResult : ActionResult
        {
            private readonly string _filePath;
            private readonly string _contentType;
            private readonly string _fileName;

            public FixedFileResult(string path, string contentType, string fileName)
            {
                _filePath = path;
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
                    context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" +
                                HttpUtility.UrlEncode(_fileName, Encoding.UTF8).Replace("+", "%20"));
                }

                response.WriteFile(_filePath);
            }
        }

        public ActionResult GetXmlList(string xmlListName, int? tabId)
        {
            if (string.IsNullOrEmpty(xmlListName))
                return new AjaxResultModel("Ошибка: не указано имя файла");

            var fileName = "Расширенный поиск заявлений";
            if (tabId.HasValue)
            {
                if (tabId.Value == 2)
                    fileName = "Не прошедшие проверку";
                else if (tabId.Value == 3)
                    fileName = "Отозванные";
                else if (tabId.Value == 4)
                    fileName = "Принятые";
                else if (tabId.Value == 5)
                    fileName = "Включенные в приказ";
            }

            return new FixedFileResult(Path.Combine(Path.GetTempPath(), xmlListName + ".xls"), "application/msexcel",
                    string.Format("{0}_{1:yyyy-MM-dd-HH-mm}.xml", fileName, DateTime.Now));
        }

        [HttpPost]
        public ActionResult GetApplicationList(InstitutionApplicationListViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (var dbContext = new EntrantsEntities())
            {
                model.InstitutionID = InstitutionID;
                if (model.Filter != null && model.Filter.DateBegin.HasValue && model.Filter.DateEnd.HasValue && model.Filter.DateBegin.Value > model.Filter.DateEnd.Value)
                    return new AjaxResultModel().SetIsError("Filter_DateEnd", "Конечная дата должна быть меньше начальной даты");

                return new AjaxResultModel { Data = dbContext.FillInstitutionApplicationList(model, false, InstitutionID) };
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApproveApplication(InstitutionApplicationListViewModel model)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.ApproveApplication(model, InstitutionID);
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult DenyApplication(InstitutionApplicationListViewModel model)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.DenyApplication(model, InstitutionID);
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult CheckApplication(InstitutionApplicationListViewModel model)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.CheckApplication(model.ApplicationID);
            }
        }

        #region bulk operations

        //[HttpPost]
        //[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        [Authorize]
        public ActionResult ApproveAllApplications(InstitutionApplicationListViewModel model)
        {
            //Debugger.Break();

            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.ApproveAllApplications(model, InstitutionID);
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult DenyAllApplications(InstitutionApplicationListViewModel model)
        {
            //Debugger.Break();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.DenyAllApplications(model, InstitutionID);
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult CheckAllApplication(InstitutionApplicationListViewModel model)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.CheckAllApplications(model, InstitutionID);
            }
        }

        #endregion

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult PublishApplications()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.PublishApplications(InstitutionID);
            }
        }

        [Authorize, HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SaveUid(int? orderId, string uid)
        {
            if (!orderId.HasValue)
                return new AjaxResultModel(AjaxResultModel.DataError);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.SaveOrderUid(orderId.Value, InstitutionID, uid);
            }
        }

        public ActionResult IncludeInOrderList()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("InstitutionApplicationListIncludeInOrder",
                    dbContext.FillInstitutionApplicationListIncludeInOrder(
                    new InstitutionApplicationListIncludeInOrderViewModel { InstitutionID = InstitutionID }, InstitutionID));
            }
        }

        [HttpPost]
        public AjaxResultModel GetIncludeInOrderAppList(InstitutionApplicationListIncludeInOrderViewModel model)
        {
            if (model.OrderID < 1)
                return GetDataError();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.GetInstitutionApplicationListIncludeInOrder(InstitutionID, model);
            }
        }

        private AjaxResultModel GetDataError()
        {
            logger.Error(AjaxResultModel.DataError);
            return new AjaxResultModel(AjaxResultModel.DataError);
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel PublishOrder(int? orderID, int? typeID)
        {
            if (!orderID.HasValue || !typeID.HasValue)
                return GetDataError();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.PublishOrder(InstitutionID, orderID.Value, typeID.Value == 1);
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ExcludeApplicationFromOrder(int? applicationID)
        {
            if (!applicationID.HasValue)
                return new EmptyResult();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.ExcludeApplicationFromOrder(applicationID.Value);
            }
        }

        public ActionResult ExtendedApplicationList(string searchTerm)
        {
            //using(EntrantsEntities dbContext=new EntrantsEntities()) {
            //    //dbContext.CalculateApplicationRating(251);
            //    return View("ExtendedApplicationList",
            //        dbContext.FillExtendedApplicationList(new ExtendedApplicationListViewModel { InstitutionID=InstitutionID },true,InstitutionID,fastSearchTerm :searchTerm));
            //}
            return RedirectToActionPermanent("ExtendedApplicationSearch", new { q = searchTerm });
        }

        [HttpPost]
        public ActionResult GetExtendedApplicationList(ExtendedApplicationListViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                model.InstitutionID = InstitutionID;
                if (model.Filter != null && model.Filter.DateBegin.HasValue && model.Filter.DateEnd.HasValue && model.Filter.DateBegin.Value > model.Filter.DateEnd.Value)
                    return new AjaxResultModel().SetIsError("Filter_DateEnd", "Конечная дата должна быть меньше начальной даты");

                return new AjaxResultModel
                {
                    Data = dbContext.FillExtendedApplicationList(model, false, InstitutionID)
                };
            }
        }

        public ActionResult FindApplicationInList(int? applicationID)
        {
            if (applicationID == null)
                return new EmptyResult();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                int tabID;
                int pageNumber;
                int orderID;
                dbContext.FindApplicationInList(applicationID.Value, InstitutionID, out tabID, out pageNumber, out orderID);
                ViewBag.ESearchTabID = tabID;
                ViewBag.ESearchPageNumber = pageNumber;
                ViewBag.ESearchApplicationID = applicationID.Value;
                if (orderID > 0)
                    ViewBag.ESearchOrderID = orderID;
                return tabID == 5 ? IncludeInOrderList() : ApplicationList();
            }
        }

        public ActionResult RecalcAllRating()
        {
            var apps = new List<int>();
            using (var dbContext = new EntrantsEntities())
            {
                apps.AddRange(dbContext.Application.Where(
                        x => x.InstitutionID == InstitutionID && x.StatusID != ApplicationStatusType.Draft).Select(x => x.ApplicationID));
            }

            foreach (var applicationID in apps)
                ApplicationRatingCalculator.CalculateApplicationRating(applicationID);

            return new EmptyResult();
        }

        public AjaxResultModel GetOrderList(IncludeInOrderListViewModel model)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.GetOrderList(model, InstitutionID);
            }
        }

        public AjaxResultModel GetCGroupList(string campaignName)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(campaignName))
                        return new AjaxResultModel { Data = dbContext.CompetitiveGroup.Where(x => x.InstitutionID == InstitutionID).Select(x => x.Name).Distinct().ToArray() }; ;
                    Campaign camp = dbContext.Campaign.Where(x => x.Name == campaignName && x.InstitutionID == InstitutionID).Single();
                    IQueryable<CompetitiveGroup> group1 = dbContext.CompetitiveGroup.Where(x => x.CampaignID == camp.CampaignID);
                    IQueryable<string> group2 = group1.Select(x => x.Name);
                    //string[] group = group2.ToArray();
                    List<string> l = new List<string>();
                    foreach (string s in group2)
                        l.Add(s);
                    string[] group = l.ToArray();
                    return new AjaxResultModel { Data = group };
                }
                catch (System.InvalidOperationException)
                {
                    return new AjaxResultModel("Error");
                }

            }

        }

        #region Переход к заявлению в списке

        public ActionResult NavigateToList(int? applicationId)
        {
            if (EntrantApplicationSQL.ComputeNavigationParameters(applicationId, InstitutionID))
            {
                return RedirectToAction("ApplicationList");
            }

            return HttpNotFound();
        }

        #endregion

        #region Переход к редактированию заявления в зависимости от статуса

        public ActionResult NavigateToEditPage(int? applicationId, int? hashTag)
        {
            int? statusId = EntrantApplicationSQL.GetApplicationStatus(applicationId, InstitutionID);

            string hash = "#tab" + hashTag.GetValueOrDefault();

            if (statusId.HasValue)
            {
                switch (statusId.Value)
                {
                    case 1:
                        return Redirect(Url.Action("Wz", "Application", new { id = applicationId.GetValueOrDefault() }) + hash);
                    default:
                        return Redirect(Url.Action("Edit", "Application", new { id = applicationId.GetValueOrDefault() }) + hash);
                }
            }

            return HttpNotFound();
        }

        #endregion

        #region Переход - Включение Заявления в Приказ

        public ActionResult CheckApplicationsSelectOrder(int[] applicationId)
        {
            ConditionForIncudeToOrder conditions = OrderOfAdmissionSQL.GetConditionForIncudeToOrder(applicationId);
            if (conditions.ErrorId > 0)
            {
                return new AjaxResultModel { IsError = true, Data = conditions };
            }
            else
            {
                TempData["conditionForIncudeToOrder"] = conditions;
                TempData["applicationId"] = applicationId;
                return new AjaxResultModel { Data = conditions, Extra = applicationId };
            }
        }

        #endregion

        [HttpPost]
        public ActionResult DeleteApplications(int[] applicationId)
        {
            var repository = new ApplicationRepository();
            var result = repository.DeleteApplications(applicationId, InstitutionID);
            //EntrantApplicationSQL.CancelRevoke(InstitutionID, applicationId);
            return Json(new { success = true });
        }


        //-----------------------------------------------------------------------------------------------
        // export списков заявлений
        //-----------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult ExportApplications(int id, string submitModel)
        {
            try
            {
                DataTable dt = null;

                var ext = "";
                if (id == 1)
                {
                    ext = "новые";
                    dt = GetNewApplications(submitModel);
                }
                else
                if (id == 2)
                {
                    ext = "не_прошедшие_проверку";
                    dt = GetUncheckedApplications(submitModel);
                }
                else
                if (id == 3)
                {
                    ext = "отозванные";
                    dt = GetRevokedApplications(submitModel);
                }
                else
                if (id == 4)
                {
                    ext = "принятые";
                    dt = GetAcceptedApplications(submitModel);
                }
                else
                    return null;

                string fileName = string.Format("Заявления_{0}_{1}.xls", ext, DateTime.Now.ToString("yyyyMMddHHmmss"));

                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename*=UTF-8''{0}",
                    Uri.EscapeDataString(fileName)));
                Response.Charset = "UTF-8";
                WriteApplications(dt);
                Response.End();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                Exception inner = null;

                message += $"\ntackTrace: {ex.StackTrace}";

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                if (inner != null)
                {
                    message += $"\nInnerExceptionMessage:\n {ex.Message}\n";
                }

                xlsLogger.Error("XLS Export error: \n" + message);
                ViewData["JavaScriptErrorMessage"] = "Ошибка экспорта xls-файла";
                return View("JavaScriptError");
            }
            return null;
        }

        //-----------------------------------------------------------------------------------------------

        private void WriteApplications(DataTable dt)
        {
            var workbook = new HSSFWorkbook();
            var sheet1 = workbook.CreateSheet("Sheet 1");

            var row1 = sheet1.CreateRow(0);
            for (int j = 0; j < dt.Columns.Count; j++)
            {

                var cell = row1.CreateCell(j);
                var columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    var cell = row.CreateCell(j);
                    var columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            using (var stream = new MemoryStream())
            {
                workbook.Write(stream);
                Response.BinaryWrite(stream.GetBuffer());
            }

        }

        //-----------------------------------------------------------------------------------------------

        private DataTable GetNewApplications(string submitModel)
        {
            var dt = new DataTable();
            dt.Columns.Add("№ заявления", typeof(string));
            dt.Columns.Add("Статус", typeof(string));
            dt.Columns.Add("Дата последней проверки", typeof(string));
            dt.Columns.Add("Конкурс", typeof(string));
            dt.Columns.Add("ФИО", typeof(string));
            dt.Columns.Add("Документ, удостоверяющий личность", typeof(string));
            dt.Columns.Add("Дата регистрации", typeof(string));
            dt.Columns.Add("Рекомендован к зачислению", typeof(string));

            var model = JsonConvert.DeserializeObject<NewApplicationsQueryViewModel>(
                HttpUtility.UrlDecode(submitModel),
                new IsoDateTimeConverter() { DateTimeFormat = "dd.MM.yyyy" });

            // PageSizeInternal чтобы не записывалось в сессию 
            model.Pager = new PagerViewModel() { PageSizeInternal = int.MaxValue };
            var list = EntrantApplicationSQL.GetNewApplicationsRecords(InstitutionID, model);

            foreach (var item in list.Records)
                dt.Rows.Add(
                    item.ApplicationNumber, item.StatusName, item.LastCheckDate,
                    item.CompetitiveGroupNames, item.EntrantFullName, item.IdentityDocument,
                    item.RegistrationDate, item.IsInRecommendedLists ? "Да" : "Нет");

            return dt;
        }

        //-----------------------------------------------------------------------------------------------

        private DataTable GetUncheckedApplications(string submitModel)
        {
            var dt = new DataTable();
            dt.Columns.Add("№ заявления", typeof(string));
            dt.Columns.Add("Тип нарушения", typeof(string));
            dt.Columns.Add("Статус", typeof(string));
            dt.Columns.Add("Дата последней проверки", typeof(string));
            dt.Columns.Add("Конкурс", typeof(string));
            dt.Columns.Add("ФИО", typeof(string));
            dt.Columns.Add("Документ, удостоверяющий личность", typeof(string));
            dt.Columns.Add("Дата регистрации", typeof(string));
            dt.Columns.Add("Рекомендован к зачислению", typeof(string));

            var model = JsonConvert.DeserializeObject<UncheckedApplicationsQueryViewModel>(
                HttpUtility.UrlDecode(submitModel),
                new IsoDateTimeConverter() { DateTimeFormat = "dd.MM.yyyy" });

            // PageSizeInternal чтобы не записывалось в сессию 
            model.Pager = new PagerViewModel() { PageSizeInternal = int.MaxValue };
            var list = EntrantApplicationSQL.GetUncheckedApplicationsRecords(InstitutionID, model);

            foreach (var item in list.Records)
                dt.Rows.Add(
                    item.ApplicationNumber, item.ViolationErrors, item.StatusName, item.LastCheckDate,
                    item.CompetitiveGroupNames, item.EntrantFullName, item.IdentityDocument,
                    item.RegistrationDate, item.IsInRecommendedLists ? "Да" : "Нет");

            return dt;
        }

        //-----------------------------------------------------------------------------------------------

        private DataTable GetRevokedApplications(string submitModel)
        {
            var dt = new DataTable();
            dt.Columns.Add("№ заявления", typeof(string));
            dt.Columns.Add("Дата отзыва заявления", typeof(string));
            dt.Columns.Add("ФИО", typeof(string));
            dt.Columns.Add("Документ, удостоверяющий личность", typeof(string));
            dt.Columns.Add("Дата регистрации", typeof(string));
            dt.Columns.Add("Рекомендован к зачислению", typeof(string));

            var model = JsonConvert.DeserializeObject<RevokedApplicationsQueryViewModel>(
                HttpUtility.UrlDecode(submitModel),
                new IsoDateTimeConverter() { DateTimeFormat = "dd.MM.yyyy" });

            // PageSizeInternal чтобы не записывалось в сессию 
            model.Pager = new PagerViewModel() { PageSizeInternal = int.MaxValue };
            var list = EntrantApplicationSQL.GetRevokedApplicationsRecords(InstitutionID, model);

            foreach (var item in list.Records)
                dt.Rows.Add(
                    item.ApplicationNumber, item.LastDenyDate,
                    item.EntrantFullName, item.IdentityDocument,
                    item.RegistrationDate, item.IsInRecommendedLists ? "Да" : "Нет");

            return dt;
        }

        //-----------------------------------------------------------------------------------------------

        private DataTable GetAcceptedApplications(string submitModel)
        {
            var dt = new DataTable();
            dt.Columns.Add("№ заявления", typeof(string));
            dt.Columns.Add("Статус", typeof(string));
            dt.Columns.Add("Дата последней проверки", typeof(string));
            dt.Columns.Add("Конкурс", typeof(string));
            dt.Columns.Add("ФИО", typeof(string));
            dt.Columns.Add("Документ, удостоверяющий личность", typeof(string));
            dt.Columns.Add("Дата регистрации", typeof(string));
            dt.Columns.Add("Сдал документы", typeof(string));
            dt.Columns.Add("Рекомендован к зачислению", typeof(string));
            dt.Columns.Add("Рейтинг", typeof(decimal));

            var model = JsonConvert.DeserializeObject<AcceptedApplicationsQueryViewModel>(
                HttpUtility.UrlDecode(submitModel),
                new IsoDateTimeConverter() { DateTimeFormat = "dd.MM.yyyy" });

            // PageSizeInternal чтобы не записывалось в сессию 
            model.Pager = new PagerViewModel() { PageSizeInternal = int.MaxValue };

            var list = EntrantApplicationSQL.GetAcceptedApplicationsRecords(InstitutionID, model);


            foreach (var item in list.Records)
            {
                decimal res = 0;
                bool ratingExist = item.Rating != null ?
                    decimal.TryParse(item.Rating.Replace('.', ','), NumberStyles.Any, new NumberFormatInfo() { NumberDecimalSeparator = "," }, out res)
                    : false;

                decimal rating = ratingExist ? res : 0;

                dt.Rows.Add(
                    item.ApplicationNumber, item.StatusName, item.LastCheckDate,
                    item.CompetitiveGroupNames, item.EntrantFullName, item.IdentityDocument,
                    item.RegistrationDate, item.OriginalDocumentsReceived ? "Да" : "Нет",
                    item.IsInRecommendedLists ? "Да" : "Нет", rating);
            }

            return dt;
        }

        //-----------------------------------------------------------------------------------------------

    }
}