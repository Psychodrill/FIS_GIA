using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Hosting;
using System.Web.Mvc;
using GVUZ.Helper.MVC;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels.OrderOfAdmission;
using log4net;
using GVUZ.Helper;
using GVUZ.Data.Model;

namespace GVUZ.Web.Controllers
{
    public partial class OrderOfAdmissionController : BaseController
    {
        [HttpPost]
        public ActionResult ApplicationAdmissionRefuse(int orderOfAdmissionId, int[] applicationItemIds)
        {
            TempData["ApplicationItemToRefuseIds"] = applicationItemIds;
            TempData["OrderOfAdmissionId"] = orderOfAdmissionId;
            return RedirectToAction("ApplicationSelectRefuseOrder");
        }

        public ActionResult ApplicationSelectRefuseOrder()
        {
            int[] applicationItemIds = TempData["ApplicationItemToRefuseIds"] as int[];
            if ((applicationItemIds == null) || (!applicationItemIds.Any()))
                return RedirectToAction("OrdersOfAdmissionList");

            var model = new ApplicationSelectOrderOfAdmissionRefuseViewModel();
            model.OrderOfAdmissionId = TempData["OrderOfAdmissionId"] as int?;
            model.ApplicationItemIds = applicationItemIds;
            return View("ApplicationSelectRefuseOrder", model);
        }

        [HttpPost]
        public ActionResult LoadApplicationSelectRefuseOrderRecords(ApplicationSelectOrderQueryViewModel model )
        { 
            var listModel = OrderOfAdmissionSQL.GetOrderOfAdmissionRefuseRecords(InstitutionID, model);
            return Json(listModel);
        }

        [HttpPost]
        public ActionResult IncludeToOrderOfAdmissionRefuse(int[] applicationItemIds, int? orderOfAdmissionRefuseId)
        {
            if (applicationItemIds == null || !orderOfAdmissionRefuseId.HasValue)
                return new AjaxResultModel { IsError = true };

            try
            {
                OrderOfAdmissionSQL.IncludeApplicationsToOrderOfAdmissionRefuse(applicationItemIds, orderOfAdmissionRefuseId.Value);
                return new AjaxResultModel { IsError = false };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel { IsError = true, Data = ex.Message };
            }
        } 
    }
}