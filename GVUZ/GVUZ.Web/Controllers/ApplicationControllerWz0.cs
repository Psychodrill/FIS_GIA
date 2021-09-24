using System;
using System.Linq;
using System.Web.Mvc;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Helper.MVC;
using GVUZ.Model;
using GVUZ.Model.Applications;
using GVUZ.Model.Cache;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Filters;
using GVUZ.Web.Helpers;
using GVUZ.Web.Models;
using GVUZ.Web.Portlets;
using GVUZ.Web.Portlets.Applications;
using GVUZ.Web.Portlets.Entrants;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Application = GVUZ.Model.Entrants.Application;
using System.Collections.Generic;
using FogSoft.Web.Mvc;
using GVUZ.Model.ApplicationPriorities;
using GVUZ.Model.RecommendedLists;
using GVUZ.Web.ContextExtensionsSQL;

namespace GVUZ.Web.Controllers
{

    public partial class ApplicationController : BaseController
    {
        #region Wz0
        public ActionResult Wz0(int? id = 0)
        {
            string s = Request.Params["InstitutionID"];
            int InstitutionID = 0;
            if (!String.IsNullOrEmpty(s)) { Int32.TryParse(s, out InstitutionID); }
            ApplicationWz0ViewModel app = new ApplicationWz0ViewModel() { ApplicationID = 0, InstitutionID = InstitutionID };
            try
            {
                if (id > 0)
                {
                    app = ApplicationSQL.GetApplicationWz0(id.Value);
                    InstitutionID = app.InstitutionID.Value;
                }
                if (InstitutionID > 0)
                {
                    using (EntrantsEntities dbContext = new EntrantsEntities())
                    {
                        app.Campaigns = SQL.GetCampaigns(InstitutionID);
                    }
                }
                app.ListIdentityDocumentType = ApplicationSQL.GetIdentityDocumentType();
                app.IdentityDocumentList = ApplicationSQL.GetIdentityDocumentType().Select(x => new { ID = x.IdentityDocumentTypeId, Name = x.IdentityDocumentTypeName }).ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
            return PartialView("ApplicationWz0", app);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetWz0(int? id = 0)
        {
            var appWz0 = ApplicationSQL.GetApplicationWz0(id.Value);
            return new AjaxResultModel { Data = appWz0 };
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewWz0(ApplicationWz0Model app)
        {
            try
            {
                if (app.ApplicationNumber.Length > 50)
                {
                    return new AjaxResultModel("Максимальное количество символов 50 для Номера заявления ОО");
                }

                if (app.RegistrationDate.Year > DateTime.Now.Year)
                {
                    return new AjaxResultModel { Extra = "DateError" };
                }

                if (app.RegistrationDate.Year == DateTime.Now.Year)
                {
                    if (app.RegistrationDate.Month > DateTime.Now.Month)
                    {
                        return new AjaxResultModel { Extra = "DateError" };
                    }
                    if (app.RegistrationDate.Month == DateTime.Now.Month)
                    {
                        if (app.RegistrationDate.Day > DateTime.Now.Day)
                        {
                            return new AjaxResultModel { Extra = "DateError" };
                        }
                    }
                }

                if (app.RegistrationDate.Year <= DateTime.Now.Year - 100)
                {
                    return new AjaxResultModel { Extra = "DateError" };
                }

                if (!ModelState.IsValid)
                {
                    return new AjaxResultModel(ModelState);
                }

                var valMsgs = new Dictionary<string, string>();
                app.InstitutionID = app.InstitutionID ?? 0;

                int id = ApplicationSQL.NewApplicationWz0(app);

                if (id == -1)
                { // Такой номер уже существует
                    return new AjaxResultModel("Данный номер заявления '" + app.ApplicationNumber + "' уже используется!");
                }
                return new AjaxResultModel() { Data = new { id = id, EntrantIsNew = app.EntrantIsNew } };
            }
            catch (Exception ex)
            {
                logger.Error(ex, "NewWz0 error");
                //return new AjaxResultModel { Data=new { id=0, Error=ex.ToString() } };
                return new AjaxResultModel("Невозможно создать заявление " + ex.ToString());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdWz0(ApplicationWz0Model app)
        {
            try
            {
                int ru = ApplicationSQL.UpdApplicationWz0(app);
                return new AjaxResultModel { Data = new { RowUpdated = ru, Error = "" } };
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UpdWz0 error");
                return new AjaxResultModel("Невозможно обновить заявление " + ex.ToString());
            }
        }

        #endregion
    }
}