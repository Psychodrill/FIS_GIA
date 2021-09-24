using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GVUZ.Helper.MVC;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Web.Filters;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels.CompositionResults;
using GVUZ.Web.ViewModels.OrderOfAdmission;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GVUZ.Data.Model;
using System.Linq;
using System.Web.Script.Serialization;
using NLog;

namespace GVUZ.Web.Controllers
{
    [MenuSection(MenuSections.OrdersOfAdmission)]
    [AuthorizeAdm(Roles = UserRole.EduUser)]
    public partial class OrderOfAdmissionController : BaseController
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [NoCache]
        public ActionResult OrdersOfAdmissionList()
        {
            var model = new OrderOfAdmissionListViewModel
                {
                    Filter = OrderOfAdmissionSQL.GetOrderOfAdmissionFilter(InstitutionID, OrderOfAdmissionType.OrderOfAdmission)
                };

            return View("OrderOfAdmissionList", model);
        }

        [NoCache]
        public ActionResult OrdersOfAdmissionRefuseList()
        {
            var model = new OrderOfAdmissionListViewModel
            {
                Filter = OrderOfAdmissionSQL.GetOrderOfAdmissionFilter(InstitutionID, OrderOfAdmissionType.OrderOfAdmissionRefuse)
            };

            return View("OrderOfAdmissionList", model);
        }

        [HttpPost]
        public ActionResult LoadOrderOfAdmissionRecords(OrderOfAdmissionQueryViewModel model)
        {
            return Json(OrderOfAdmissionSQL.GetOrderOfAdmissionRecords(InstitutionID, model));
        }

        [NoCache]
        public ActionResult EditOrder(int? id)
        {
            var model = OrderOfAdmissionSQL.GetOrderOfAdmissionEditModel(InstitutionID, id);

            if ((model == null) || (model.CampaignStatusID == 2))
                return RedirectToAction("ViewOrder", new { id = id });

            model.ApplicationList = new ApplicationOrderListViewModel
                {
                    Filter = OrderOfAdmissionSQL.GetApplicationOrderFilter(InstitutionID),
                    OrderId = model.OrderId,
                    ShowOrderOfAdmissionInfo = model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmissionRefuse,
                    AllowEditDisagreedInfo = (model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmission)
                                               && (model.OrderStatus == OrderOfAdmissionStatus.Published),
                    AllowRefuseAdmission = model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmission
                };

            return View(model);
        }

        [HttpPost]
        [ActionName("EditOrderSubmit")]
        public ActionResult EditOrder(OrderOfAdmissionEditViewModel model)
        {
            OrderOfAdmissionSQL.UpdateOrder(InstitutionID, model, ModelState);

            if (ModelState.IsValid)
            {
                if (model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmission)
                    return RedirectToAction("OrdersOfAdmissionList");
                else if (model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmissionRefuse)
                    return RedirectToAction("OrdersOfAdmissionRefuseList");
            }

            model.ApplicationList.Filter = OrderOfAdmissionSQL.GetApplicationOrderFilter(InstitutionID);

            return View("EditOrder", model);
        }

        public ActionResult CreateOrderOfAdmission(int[] applicationId)
        {
            TempData["applicationId"] = applicationId;
            OrderOfAdmissionCreateViewModel model = null;
            try
            {
                model = OrderOfAdmissionSQL.GetOrderOfAdmissionCreateModel(InstitutionID, OrderOfAdmissionType.OrderOfAdmission, applicationId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View("CreateOrder", model);
        }

        public ActionResult CreateOrderOfAdmissionRefuse(int[] applicationId, int? orderOfAdmissionId)
        {
            TempData["ApplicationItemToRefuseIds"] = applicationId;
            TempData["OrderOfAdmissionId"] = orderOfAdmissionId;
            OrderOfAdmissionCreateViewModel model = null;
            try
            {
                model = OrderOfAdmissionSQL.GetOrderOfAdmissionCreateModel(InstitutionID, OrderOfAdmissionType.OrderOfAdmissionRefuse, applicationId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View("CreateOrder", model);
        }

        [HttpPost]
        [ActionName("CreateOrderSubmit")]
        public ActionResult CreateOrder(OrderOfAdmissionCreateViewModel model)
        {
            int result = OrderOfAdmissionSQL.CreateOrder(InstitutionID, model, ModelState);

            if (result > 0)
            {
                if (model.FromApplication)
                {
                    if (model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmissionRefuse)
                        return RedirectToAction("ApplicationSelectRefuseOrder");
                    else
                    {
                        TempData["applicationId"] = model.ApplicationId;
                        return RedirectToAction("ApplicationSelectOrder");
                    }
                }

                return RedirectToAction("EditOrder", new { id = result });
            }

            model.Reposted = true;
            return View("CreateOrder", model);
        }

        [HttpPost]
        public ActionResult GetOrderParameters(OrderOfAdmissionParametersViewModel model)
        {
            return Json(OrderOfAdmissionSQL.GetOrderOfAdmissionParameters(InstitutionID, model));
        }

        [HttpPost]
        public ActionResult RemoveOrder(int orderId)
        {
            string error;
            if (OrderOfAdmissionSQL.RemoveOrder(InstitutionID, orderId, out error))
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, errorMessage = error });
        }

        [HttpPost]
        public ActionResult LoadApplicationOrderRecords(int? id)
        {
            var data = Json(OrderOfAdmissionSQL.GetApplicationOrderRecords(InstitutionID, id.GetValueOrDefault()));

            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(data),
                ContentType = "application/json"
            };
        }

        [HttpPost]
        public ActionResult SaveApplicationDisagreeData(ApplicationDisagreeData model)
        {
            try
            {
                OrderOfAdmissionForAppSQL.SaveApplicationDisagreeData(model.ApplicationItemId, model.IsDisagreed, model.IsDisagreedDate);
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = "Произошла ошибка при сохранении данных" });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ExportApplicationsCsv(int? id, string submitModel)
        {
            try
            {
                var isoDateFormat = new IsoDateTimeConverter();
                isoDateFormat.DateTimeFormat = "dd.MM.yyyy";

                var model = JsonConvert.DeserializeObject<ApplicationOrderQueryViewModel>(HttpUtility.UrlDecode(submitModel), isoDateFormat);

                var list = OrderOfAdmissionSQL.GetApplicationOrderRecords(InstitutionID, id.GetValueOrDefault());

                string fileName = string.Format("Заявления_в_приказе_{0}_{1}.csv", list.OrderId, DateTime.Now.ToString("yyyyMMddHHmmss"));
                const string contentType = "text/csv";

                using (var download = new ResponseFileWriter(Server, Request, Response, fileName, contentType))
                {
                    using (var writer = new StreamWriter(download.Output, Encoding.UTF8))
                    {
                        OrderOfAdmissionSQL.WriteApplicationsCsv(writer, list.Records);
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
        public ActionResult ExcludeApplicationsFromOrder(int? orderId, int[] applicationItemIds)
        {
            if((applicationItemIds==null)||(!applicationItemIds.Any()))
            {
                return Json(new
                    {
                        success = true,
                    });
            }
            var result = OrderOfAdmissionSQL.ExcludeApplicationsFromOrder(InstitutionID, orderId.GetValueOrDefault(), applicationItemIds ?? new int[0]);

            return Json(new
                {
                    success = true,
                    data = new
                    {
                        reloadOrder = result.StatusChanged,
                        reloadList = result.AffectedRows > 0,
                        orderId = result.OrderId
                    }
                });
        }

        [HttpPost]
        public ActionResult ValidatePublication(int? orderId, string regNumber, DateTime? regDate, string orderName, string orderUID)
        {
            List<string> errors = OrderOfAdmissionSQL.ValidatePublishAvailable(InstitutionID, orderId.GetValueOrDefault(), regNumber, regDate, orderName, orderUID);

            return Json(new
                {
                    success = errors.Count == 0,
                    errorMessage = string.Join("<br />", errors),
                    confirmationMessage = "Вы уверены, что хотите опубликовать приказ?"
                });
        }

        [HttpPost]
        public ActionResult CommitPublication(int? orderId, string regNumber, DateTime? regDate, string orderName, string orderUID)
        {
            List<string> errors = OrderOfAdmissionSQL.PublishOrder(InstitutionID, orderId.GetValueOrDefault(), regNumber, regDate, orderName, orderUID);

            return Json(new
                {
                    success = errors.Count == 0,
                    errorMessage = string.Join("<br />", errors)
                });
        }

        [HttpGet]
        public ActionResult ViewOrder(int? id)
        {
            if (!id.HasValue)
                return new HttpNotFoundResult();

            OrderOfAdmissionViewModel model = OrderOfAdmissionSQL.GetOrderOfAdmissionViewModel(id.Value);
            if (model == null)
                return new HttpNotFoundResult();

            model.ApplicationList = new ApplicationOrderListViewModel
            {
                Filter = OrderOfAdmissionSQL.GetApplicationOrderFilter(InstitutionID),
                OrderId = model.OrderId,
                IsReadOnly = true,
                ShowOrderOfAdmissionInfo = model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmissionRefuse,
                AllowEditDisagreedInfo = (model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmission)
                                           && (model.OrderStatus == OrderOfAdmissionStatus.Published),
                AllowRefuseAdmission = model.OrderTypeId == OrderOfAdmissionType.OrderOfAdmission
            };
            return View(model);
        } 
    }
}
