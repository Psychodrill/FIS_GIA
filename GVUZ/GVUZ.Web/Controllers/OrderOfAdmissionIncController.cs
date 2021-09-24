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
using FogSoft.Helpers;

namespace GVUZ.Web.Controllers {

    public partial class OrderOfAdmissionController : BaseController {

        public ActionResult ApplicationSelectOrder() {
            try {
                ConditionForIncudeToOrder cfiToOrder = null;
                var model = new ApplicationSelectOrderViewModel {
                    //Filter = "";
                };

                cfiToOrder = (ConditionForIncudeToOrder)TempData["conditionForIncudeToOrder"];
                int[] appsArrayId = (int[])TempData["applicationId"];
                if(appsArrayId != null) {
                    if(appsArrayId.Any()) {
                        if(appsArrayId.Length == 1) {
                            model.ApplicationId = appsArrayId[0];
                        }
                        model.ApplicationsId = appsArrayId;

                        TempData["conditionForIncudeToOrder"] = cfiToOrder;
                        TempData["applicationId"] = model.ApplicationsId;

                        return View("ApplicationSelectOrder", model);
                    }
                }
                return Redirect(Url.Action("ApplicationList", "InstitutionApplication") + "#tab3");
            } catch(SqlException ) {
                throw;
            } catch(Exception ) {
                throw;
            }
        }

        [HttpPost]
        public ActionResult LoadApplicationSelectOrderRecords(ApplicationSelectOrderQueryViewModel model,
            int[] applicationsId)
        {
            ConditionForIncudeToOrder conditions = TempData["conditionForIncudeToOrder"] as ConditionForIncudeToOrder;
            if (conditions == null)
            {
                conditions = OrderOfAdmissionSQL.GetConditionForIncudeToOrder(applicationsId);
            }

            return Json(OrderOfAdmissionForAppSQL.GetOrderOfAdmissionRecords(InstitutionID, applicationsId, OrderOfAdmissionType.OrderOfAdmission, model, conditions));
        }

        public ActionResult IncludeToOrder(int? ApplicationID, int? OrderID) {
            if(!ApplicationID.HasValue || !OrderID.HasValue) {
                return new EmptyResult();
            }
            if(ApplicationSQL.GetApplicationStatusId(ApplicationID.Value) == 8)
            {
                return RedirectToAction("ApplicationList", "InstitutionApplication");
            }
            try {
                var model = OrderOfAdmissionSQL.GetIncludeToOrder(ApplicationID.Value, OrderID.Value);
                return View("IncludeToOrder", model);
            } catch(Exception ex) {
                throw ex;
            }
        }

        public ActionResult GetConditionForIncudeToOrder(int[] ApplicationsId)
        {
            try
            {
                var conditions = OrderOfAdmissionSQL.GetConditionForIncudeToOrder(ApplicationsId);
                if (conditions.ErrorId > 0)
                    return new AjaxResultModel { IsError = true, Data = conditions };
                else
                    return new AjaxResultModel { Data = conditions };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel("Невозможно получить данные  " + ex.ToString());
            }
        }

        public ActionResult MultiIncludeToOrder(int[] applicationId, int? orderIdField) {
            if (applicationId == null || !orderIdField.HasValue) {
                LogHelper.Log.Debug(string.Format("MultiIncludeToOrder - applicationId == null || !orderIdField.HasValue"));
                return new EmptyResult();
            }
            try {
                LogHelper.Log.Debug(string.Format("MultiIncludeToOrder applicationId.Count={0}, orderIdField.Value={1}", applicationId.Count(), orderIdField.Value));
                var model = OrderOfAdmissionSQL.GetMultiIncludeToOrder(applicationId, orderIdField.Value);
                return View("MultiIncludeToOrder", model);
            } catch {// (Exception ex) {
                throw; // ex;
                //return new AjaxResultModel("Невозможно получить данные  " + ex.ToString());
            }
        }

        public ActionResult FuncCheckIncludeAppToOrder(int? ApplicationID, int? OrderID, int? AppCGItemID,
            int? IdLevelBudget,
            int? BenefitID)
        {
            if(!ApplicationID.HasValue || !OrderID.HasValue || !AppCGItemID.HasValue) {
                return new AjaxResultModel("Ошибка в параметрах запроса");
            }
            try {
                if(BenefitID == 0) {
                    BenefitID = null;
                }
                var checkerrors = OrderOfAdmissionSQL.FuncCheckIncludeAppToOrder(ApplicationID, OrderID, AppCGItemID,
                    IdLevelBudget, BenefitID);

                bool check = false;
                foreach(var i in checkerrors) {
                    if((i.ID == 6) || (i.ID == 10)) {
                        check = true;
                    } else {
                        check = false;
                        break;
                    }
                }

                if(checkerrors.Count > 0) {
                    return new AjaxResultModel() { IsError = true, Data = checkerrors, Extra = check };
                } else {
                    return new AjaxResultModel() { IsError = false, Data = { } };
                }
            } catch(Exception ex) {
                return new AjaxResultModel("Невозможно получить данные  " + ex.ToString());
            }
        }

        public ActionResult IncludeAppToOrder(int? ApplicationID, int? OrderID, int? AppCGItemID, int? IdLevelBudget, int? BenefitID) {
            if (!ApplicationID.HasValue || !OrderID.HasValue || !AppCGItemID.HasValue)
            {
                return new AjaxResultModel("Ошибка в параметрах запроса");
            }
            try {
                if(BenefitID == 0) {	BenefitID = null;}
                var checkerror = OrderOfAdmissionSQL.IncludeAppToOrder(ApplicationID, OrderID, AppCGItemID, IdLevelBudget, BenefitID);

	            if (checkerror.Where(x => (x.ID != 6 && x.ID != 10)).FirstOrDefault() != null){
		            return new AjaxResultModel() {IsError = true, Data = checkerror};
	            }else{
						return new AjaxResultModel() { IsError=false, Data=checkerror };
	            }
            } catch(Exception ex) {
                return new AjaxResultModel("Невозможно получить данные  " + ex.ToString());
            }
        }
    }
}