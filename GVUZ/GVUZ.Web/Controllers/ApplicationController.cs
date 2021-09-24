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
using System.Data.SqlClient;
using GVUZ.Model.ApplicationPriorities;
using GVUZ.Model.RecommendedLists;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.DAL.Dapper.Repository.Model;
using NLog;
using log4net;

namespace GVUZ.Web.Controllers
{

    [MenuSection("Applications")]
    [AuthorizeAdm(Roles = UserRole.EduUser), EntrantNameFilter]
    public partial class ApplicationController : BaseController
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static readonly ILog a_logger = log4net.LogManager.GetLogger("ApplicationLogger");

        public const int ShowCompetitiveGroupAddDialog = 1;
        public const int ShowMessageExistingEntrant = 2;

        public const string EntrantFullName = "Entrant.FullName";

        public int ApplicationID
        {
            get
            {
                if (CustomParameters.ContainsKey(ApplicationSessionKey))
                    return CustomParameters[ApplicationSessionKey].To(0);
                ISession session = ServiceLocator.Current.GetInstance<ISession>();
                return session.GetValue<int>(ApplicationSessionKey);
            }

            set
            {
                ISession session = ServiceLocator.Current.GetInstance<ISession>();
                session.SetValue(ApplicationSessionKey, value);
            }
        }
        public const string ApplicationSessionKey = "applicationID";
        public const string StructureItemSessionKey = "structureItemID";
        /* Новое { */

        [Authorize]
        [HttpGet]
        public ActionResult Get(int? id = 0)
        {
            string s = Request.Params["InstitutionID"];
            int InstitutionID = 0;
            if (!String.IsNullOrEmpty(s))
            {
                Int32.TryParse(s, out InstitutionID);
            }
            ApplicationWz0ViewModel app = new ApplicationWz0ViewModel() { ApplicationID = 0, InstitutionID = InstitutionID };
            try
            {
                if (id > 0)
                {
                    app = ApplicationSQL.GetApplicationWz0(id.Value);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, " GetApplicationWz0 error");
            }
            return View("ApplicationWz", app);
        }

        // GET: /Application2/id
        public ActionResult ApplicationView(int? id = 0)
        {
            ApplicationModel app = ApplicationSQL.GetApplication(id.Value);
            return PartialView("ApplicationView", app);
        }

        /* Новое { itprodavets */

        #region ApplicationV0
        // GET: /ApplicationV0/id
        public ActionResult ApplicationV0(int? id = 0)
        {
            ApplicationV0Model app = null;
            try
            {
                app = ApplicationViewSQL.GetApplicationV0(id.Value);
            }
            catch (Exception e)
            {
                logger.Error(e, "Application V0 error");
            }
            return PartialView("ApplicationV0", app);
        }

        #endregion

        #region ApplicationV1
        // GET: /ApplicationV1/id
        public ActionResult ApplicationV1(int? id = 0)
        {
            ApplicationV1Model app = ApplicationViewSQL.GetApplicationV1(id.Value);
            return PartialView("ApplicationV1", app);
        }

        #endregion

        #region ApplicationV2
        // GET: /ApplicationV2/id
        public ActionResult ApplicationV2(int? id = 0)
        {
            ApplicationV2Model app = ApplicationViewSQL.GetApplicationV2(id.Value);
            return PartialView("ApplicationV2", app);
        }

        #endregion

        #region ApplicationV3
        // GET: /ApplicationV0/id
        public ActionResult ApplicationV3(int? id = 0)
        {
            ApplicationV3Model app = ApplicationViewSQL.GetApplicationV3(id.Value);
            return PartialView("ApplicationV3", app);
        }

        #endregion

        #region ApplicationV4
        // GET: /ApplicationV0/id
        public ActionResult ApplicationV4(int? id = 0)
        {
            ApplicationV4Model app = ApplicationViewSQL.GetApplicationV4(id.Value);
            return PartialView("ApplicationV4", app);
        }

        #endregion

        #region ApplicationV5
        // GET: /ApplicationV0/id
        public ActionResult ApplicationV5(int? id = 0)
        {
            ApplicationV5Model app = ApplicationViewSQL.GetApplicationV5(id.Value);
            return PartialView("ApplicationV5", id.Value);
        }

        #endregion

        public ActionResult GetIdentityDocumentType()
        {
            try
            {
                return new AjaxResultModel { Data = ApplicationSQL.GetIdentityDocumentType() };
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return new AjaxResultModel(ex.ToString());
            }
        }

        // GET: /Application/
        [Authorize]
        [HttpGet]
        public ActionResult DefaultPage(int? id = 0)
        {
            string s = Request.Params["InstitutionID"];
            int InstitutionID = 0;
            if (!String.IsNullOrEmpty(s))
            {
                Int32.TryParse(s, out InstitutionID);
            }
            ApplicationWzModel app = new ApplicationWzModel() { ApplicationID = 0, WizardStepID = 0, InstitutionID = InstitutionID };
            try
            {
                if (id > 0)
                {
                    app = ApplicationSQL.GetApplicationWz(id.Value);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            base.ViewData[EntrantFullName] = app.EntrantFullName;
            return View("ApplicationWz", app);
        }

        // GET: /Application/
        public ActionResult Wz(int? id = 0)
        {
            logger.Trace("In ActionResult Wz");
            int InstitutionID = InstitutionHelper.GetInstitutionID();
            string s = Request.Params["InstitutionID"];
            //if(!String.IsNullOrEmpty(s)) {	Int32.TryParse(s,out InstitutionID);	}
            ApplicationWzModel app = new ApplicationWzModel() { ApplicationID = 0, WizardStepID = 0, InstitutionID = InstitutionID };
            try
            {
                if (id > 0)
                {
                    app = ApplicationSQL.GetApplicationWz(id.Value);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            if (app == null)
                return RedirectToAction("ApplicationList", "InstitutionApplication");
            else
            {
                base.ViewData[EntrantFullName] = app.EntrantFullName;
                return View("ApplicationWz", app);
            }
        }

        #region ApplicationEdit
        public ActionResult Edit(int? id = 0)
        {
            ApplicationEditModel app = null;
            if (ApplicationSQL.GetApplicationStatusId(id.Value) == 8)
            {
                return RedirectToAction("ApplicationList", "InstitutionApplication");
            }
            try
            {
                app = ApplicationSQL.GetApplicationEdit(id.Value);
            }
#warning Empty catch
            catch (Exception)
            {
            }
            if (app != null)
            {
                base.ViewData[EntrantFullName] = app.EntrantFullName;
                return View("ApplicationEdit", app);
            }
            else
                return RedirectToAction("ApplicationList", "InstitutionApplication");
        }
        #endregion

        [Authorize]
        [HttpPost]
        public ActionResult SetWzStep(int? ApplicationID, int? Step)
        {
            logger.Trace($"In SetWzStep, Step:{0}", Step);
            if (ApplicationID == null || Step == null)
            {
                return new AjaxResultModel("Ошибка параметров!");
            }
            try
            {
                ApplicationSQL.SetWzStep(ApplicationID.Value, Step.Value);
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return new AjaxResultModel(ex.ToString());
            }
        }

        #region Wz1
        // GET: /Application/
        public ActionResult Wz1(int? id = 0)
        {

            if (!id.HasValue) { return new EmptyResult(); }
            // 		    ClearApplicationCache(applicationID.Value);

            var wz1 = new ApplicationWz1ViewModel();
            try
            {
                wz1 = ApplicationSQL.GetApplicationWz1(id.Value);
                wz1.MaxFileSize = GetMaxAllowedFileLength();

                // для редактирования ещё и справочники
                wz1.GenderList = new[] {
                new { ID = 0, Name = "" },
                new { ID = GenderType.Male, Name = GenderType.GetName(GenderType.Male) },
                new { ID = GenderType.Female, Name = GenderType.GetName(GenderType.Female) } };

                wz1.NationalityList = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.CountryType).Select(x => new { ID = x.Key, Name = x.Value });
                wz1.IdentityDocumentList = ApplicationSQL.GetIdentityDocumentType().Select(x => new { ID = x.IdentityDocumentTypeId, Name = x.IdentityDocumentTypeName }).ToArray();
                wz1.ForceAddData = true;
                wz1.ReleaseCountryList = wz1.NationalityList;
                //wz1.SelectedCitizenships = ApplicationSQL.GetSelectedCitizenships(id).Select(x => new { ID = x.Key, Name = x.Value }).ToList();



                wz1.SelectedCitizenships = ApplicationSQL.GetSelectedCitizenships(id);
            }
            catch (Exception ex)
            {
                a_logger.ErrorFormat("Wz1 -> {0}", ex.Message);
                logger.Error(ex, "Wz1 error");
            }
           
            return PartialView("ApplicationWz1", wz1);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetWz1(int? id = 0)
        {
            var appWz1 = new ApplicationWz1ViewModel();
            try
            {
                appWz1 = ApplicationSQL.GetApplicationWz1(id.Value);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "GetWz1 error");
            }           
            return new AjaxResultModel { Data = appWz1 };
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdWz1(ApplicationWz1Model app)
        {
            a_logger.DebugFormat("Обновление визарда 1 (UpdWz1) для ОО {0}....", app.InstitutionID);
            try
            {
                string[] errorStrings = new string[2];
                if (app.BirthDate.Year > DateTime.Now.Year)
                {
                    errorStrings[0] = "BirthDateError";
                    //return new AjaxResultModel { Extra = "BirthDateError" };
                }

                if (app.BirthDate.Year == DateTime.Now.Year)
                {
                    if (app.BirthDate.Month > DateTime.Now.Month)
                    {
                        errorStrings[0] = "BirthDateError";
                        //return new AjaxResultModel {Extra = "BirthDateError"};
                    }
                    if (app.BirthDate.Month == DateTime.Now.Month)
                    {
                        if (app.BirthDate.Day > DateTime.Now.Day)
                        {
                            errorStrings[0] = "BirthDateError";
                            //return new AjaxResultModel {Extra = "BirthDateError"};
                        }
                    }
                }

                if (app.BirthDate.Year <= DateTime.Now.Year - 100)
                {
                    errorStrings[0] = "BirthDateError";
                    //return new AjaxResultModel { Extra = "BirthDateError" };
                }

                if ((app.DocumentDate.HasValue ? app.DocumentDate.Value.Year : 0) > DateTime.Now.Year)
                {
                    errorStrings[1] = "dDateError";
                }
                if ((app.DocumentDate.HasValue ? app.DocumentDate.Value.Year : 0) == DateTime.Now.Year)
                {
                    if ((app.DocumentDate.HasValue ? app.DocumentDate.Value.Month : 0) > DateTime.Now.Month)
                    {
                        errorStrings[1] = "dDateError";
                        //return new AjaxResultModel {Extra = "dDateError"};
                    }
                    if ((app.DocumentDate.HasValue ? app.DocumentDate.Value.Month : 0) == DateTime.Now.Month)
                    {
                        if ((app.DocumentDate.HasValue ? app.DocumentDate.Value.Day : 0) > DateTime.Now.Day)
                        {
                            errorStrings[1] = "dDateError";
                            //return new AjaxResultModel {Extra = "dDateError"};
                        }
                    }
                }
                if ((app.DocumentDate.HasValue ? app.DocumentDate.Value.Year : 0) <= DateTime.Now.Year - 100)
                {
                    errorStrings[1] = "dDateError";
                    //return new AjaxResultModel { Extra = "dDateError" };
                }
                if (errorStrings.Any())
                {
                    if (errorStrings[0] == null & errorStrings[1] == null)
                    {
                        errorStrings = null;
                    }
                    if ((errorStrings != null))
                    {
                        return new AjaxResultModel { Extra = errorStrings };
                    }
                }
                a_logger.DebugFormat("Обновление заявления для ОО {0} №{1} абитуриент {2}....", app.InstitutionID, app.ApplicationID, app.EntrantID);
                ApplicationSQL.UpdApplicationWz1(app, a_logger);
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                a_logger.DebugFormat("Ошибка обновления заявления: {0}....", ex.Message);
                logger.Error(ex);
                return new AjaxResultModel(ex.ToString());
            }
        }
        #endregion

        // Wz3 перенесен в ApplicationControllerWz3.cs




        public ActionResult GetUidList(int institutionId)
        {
            return new AjaxResultModel { Data = ApplicationSQL.GetUidList(institutionId) };
        }

        #region Wz4
        public ActionResult Wz4(int? id)
        {
            if (!id.HasValue) { return new EmptyResult(); }
            ApplicationWz4ViewModel wz4 = ApplicationSQL.GetApplicationWz4(id.Value);
            return PartialView("ApplicationWz4", wz4);
        }
        #endregion

        #region Wz5

        public ActionResult Wz5(int? id)
        {
            if (!id.HasValue)
                return new EmptyResult();

            ApplicationWz5ViewModel.Wz5SendingViewModel wz5 = ApplicationSQL.GetApplicationWz5(id.Value, InstitutionID);

            // заполнение комбо с приемными компаниями
            if (wz5.InstitutionID > 0)
                wz5.Campaigns = SQL.GetCampaigns(wz5.InstitutionID);

            // заполнение комбо с полом
            wz5.Genders = SQL.GetGenders();

            return PartialView("ApplicationWz5", wz5);
        }

        #endregion

        #region Update wz5

        public ActionResult ChecksPriorities(ApplicationPrioritiesViewModel data, bool? checkUnique, bool? checkZeroes)
        {
            var valMsgs = new Dictionary<string, string>();
            if (!CheckPriorities(data, checkUnique, checkZeroes, ref valMsgs))
            {
                AddModelErrors(valMsgs);
                var ajaxResultModel = new AjaxResultModel(ModelState);
                return Json(ajaxResultModel);
            }
            return new AjaxResultModel() { Data = null };
        }

        [HttpPost]
        public ActionResult UpdateWz5(ApplicationPrioritiesViewModel data)
        {
            try
            {
                ApplicationSQL.UpdateApplicationCompetitiveGroupItems(data.ApplicationId, data.ApplicationPriorities, true);
                return new AjaxResultModel() { IsError = false };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel() { IsError = true, Data = ex.Message };
            }
        }

        #endregion

        [HttpPost]
        public AjaxResultModel GetAvailiableAndExistingWz5(int? applicationId)
        {
            var data = new ApplicationPrioritiesViewModel();
            data.ApplicationId = applicationId.HasValue ? applicationId.Value : -1;

            data.ApplicationPriorities = SQL.GetApplicationPriorities(applicationId.Value);

            return new AjaxResultModel() { Data = data };
        }

        [Authorize]
        [HttpPost]
        public ActionResult Del(ApplicationModel Req)
        {
            try
            {
                int ru = ApplicationSQL.DelApplication(Req.ApplicationID.Value);
                return new AjaxResultModel { Data = new { RowUpdated = ru, Error = "" } };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel { Data = new { RowUpdated = 0, Error = ex.ToString() } };
            }
        }

        [PortletAjaxLink(PortletType.ReceiveFile)]
        [Authorize]
        public new ActionResult ReceiveFile1()
        {
            return base.ReceiveFile1();
        }

        public ActionResult GetDocumentForAppDocList(int ApplicationID, int EntrantDocumentID)
        {
            if (ApplicationID == 0 || EntrantDocumentID == 0)
            {
                return new AjaxResultModel("Ошибочные параметры");
            }
            try
            {
                var doc = ApplicationSQL.getDocumentForAppDocList(ApplicationID, EntrantDocumentID);
                return new AjaxResultModel { Data = doc };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }

        public ActionResult AttachDocument(int ApplicationID, int EntrantDocumentID)
        {
            if (ApplicationID == 0 || EntrantDocumentID == 0)
            {
                return new AjaxResultModel("Ошибочные параметры");
            }
            try
            {
                var doc = ApplicationSQL.AttachDocument(ApplicationID, EntrantDocumentID);
                return new AjaxResultModel { Data = doc };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }

        public ActionResult DetachDocument(int ApplicationID, int EntrantDocumentID)
        {
            if (ApplicationID == 0 || EntrantDocumentID == 0)
            {
                return new AjaxResultModel("Ошибочные параметры");
            }
            try
            {
                var doc = ApplicationSQL.DetachDocument(ApplicationID, EntrantDocumentID);
                return new AjaxResultModel { Data = doc };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }

        public ActionResult DeleteDocument(int ApplicationID, int EntrantDocumentID)
        {
            if (ApplicationID == 0 || EntrantDocumentID == 0)
            {
                return new AjaxResultModel("Ошибочные параметры");
            }
            try
            {
                var doc = ApplicationSQL.DeleteDocument(ApplicationID, EntrantDocumentID);
                return new AjaxResultModel { Data = doc };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }


        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel SetDocumentOriginalReceived(int? applicationID, int? entrantDocumentID, bool? received, DateTime? receivedDate)
        {
            if (applicationID == null || received == null || entrantDocumentID == null) { return new AjaxResultModel(AjaxResultModel.DataError); }

            try
            {
                var doc = ApplicationSQL.SetDocumentReceived(applicationID.Value, entrantDocumentID.Value, received.Value, receivedDate);
                ClearApplicationCache(applicationID.Value);
                return new AjaxResultModel { Data = doc };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel(ex.ToString());
            }
        }


        /* Новое } */

        private UserInfo GetUserInfo()
        {
            if (CustomParameters.ContainsKey("UserInfo"))
                return (UserInfo)CustomParameters["UserInfo"];
            return UserInfo.Default;
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { PortletType.ApplicationStepPersonalData })]
        [Authorize]
        public ActionResult ApplicationPersonalData(int? applicationID)
        {
            UpdateApplicationIDFromRequest();
            if (!applicationID.HasValue)
                applicationID = ApplicationID;
            using (var dbContext = new EntrantsEntities())
            {
                var model = new PersonalRecordsDataViewModel
                {
                    MaxFileSize = GetMaxAllowedFileLength(),
                    ApplicationStep = ApplicationStepType.PersonalData
                };
                PersonalRecordsDataViewModel viewModel = dbContext.FillPersonalData(model, false, new EntrantKey(applicationID.Value, GetUserInfo()));
                return View("ApplicationPersonalData", viewModel);
            }
        }

        [Authorize]
        public ActionResult ApplicationPersonalDataByApp(int? applicationID, string tabID)
        {
            if (!applicationID.HasValue)
                return new EmptyResult();

            ClearApplicationCache(applicationID.Value);

            ApplicationID = applicationID.Value;
            Session["ApplicationReturnTab"] = tabID;
            return ApplicationPersonalData(applicationID);
        }

        private void UpdateApplicationIDFromRequest()
        {
            int appID = Request.QueryString["applicationID"].To(0);
            if (appID > 0) ApplicationID = appID;
            ViewData["ApplicationID"] = ApplicationID;
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                if (InstitutionID > 0)
                {
                    int count = dbContext.Application.Count(x => x.InstitutionID == InstitutionID && x.ApplicationID == ApplicationID);
                    if (count == 0) //защита от редактирования чужих заявлений
                        ApplicationID = 0;
                }
            }
        }

        [Authorize]
        public ActionResult ApplicationPersonalDataStep()
        {
            UpdateApplicationIDFromRequest();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                PersonalRecordsDataViewModel model = new PersonalRecordsDataViewModel
                {
                    MaxFileSize = GetMaxAllowedFileLength(),
                    ApplicationStep = ApplicationStepType.PersonalData
                };
                return PartialView("Portlets/Applications/ApplicationPersonalData",
                    dbContext.FillPersonalData(model, false, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [PortletAjaxLink(PortletType.ApplicationGetNavigationUrl)]
        [Authorize]
        public ActionResult GetApplicationNavigationUrl(int id)
        {
            return JavaScript(PortletLinkHelper.ApplicationLink(id));
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { 0, PortletType.ApplicationStepParents })]
        [Authorize]
        public ActionResult ApplicationParentData()
        {
            UpdateApplicationIDFromRequest();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                PersonalRecordsDataViewModel model = new PersonalRecordsDataViewModel
                {
                    MaxFileSize = GetMaxAllowedFileLength(),
                    ApplicationStep = ApplicationStepType.ParentData
                };
                model = dbContext.FillPersonalData(model, false, new EntrantKey(ApplicationID, GetUserInfo()));
                model.ForceAddData = false;
                return View("ApplicationParentData", model);
            }
        }

        [Authorize]
        public ActionResult ApplicationParentDataStep()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                PersonalRecordsDataViewModel model = new PersonalRecordsDataViewModel
                {
                    MaxFileSize = GetMaxAllowedFileLength(),
                    ApplicationStep = ApplicationStepType.ParentData
                };
                model = dbContext.FillPersonalData(model, false, new EntrantKey(ApplicationID, GetUserInfo()));
                model.ForceAddData = false;
                return PartialView("Portlets/Applications/ApplicationParentData", model);
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { 0, PortletType.ApplicationStepAddress })]
        [Authorize]
        public ActionResult ApplicationAddress()
        {
            UpdateApplicationIDFromRequest();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                PersonalRecordsAddressViewModel model = new PersonalRecordsAddressViewModel
                {
                    ApplicationStep = ApplicationStepType.Address,
                    ApplicationID = ApplicationID
                };
                return View("ApplicationAddress", dbContext.FillPersonalAddress(model, false, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [Authorize]
        public ActionResult EducationDocumentCopyReplacement(int? applicationID)
        {
            if (applicationID.HasValue)
                using (var dbContext = new EntrantsEntities())
                {
                    return View("PrintTemplates/EducationDocumentCopyReplacement",
                                dbContext.GetPrintTemplateInfoModel(new EntrantKey(applicationID.Value, GetUserInfo())));
                }

            return HttpNotFound();
        }

        [Authorize]
        public ActionResult DocumentsIssuanceReceipt(int? applicationID)
        {
            if (applicationID.HasValue)
                using (var dbContext = new EntrantsEntities())
                {
                    return View("PrintTemplates/DocumentsIssuanceReceipt", dbContext.GetPrintTemplateInfoModel(new EntrantKey(applicationID.Value, GetUserInfo())));
                }

            return HttpNotFound();
        }

        [Authorize]
        public ActionResult DocumentsAdmissionReceipt(int? applicationID)
        {
            if (applicationID.HasValue)
                using (var dbContext = new EntrantsEntities())
                {
                    return View("PrintTemplates/DocumentsAdmissionReceipt",
                        dbContext.GetPrintTemplateInfoModel(new EntrantKey(applicationID.Value, GetUserInfo())));
                }

            return HttpNotFound();
        }

        [Authorize]
        public ActionResult ApplicationDocumentsList(int? applicationID)
        {
            if (applicationID.HasValue)
                using (var dbContext = new EntrantsEntities())
                {
                    return View("PrintTemplates/ApplicationDocumentsList",
                        dbContext.GetPrintTemplateInfoModel(new EntrantKey(applicationID.Value, GetUserInfo())));
                }

            return HttpNotFound();
        }

        [Authorize]
        public ActionResult ExaminationResultReference(int? applicationID)
        {
            if (applicationID.HasValue)
                using (var dbContext = new EntrantsEntities())
                {
                    return View("PrintTemplates/ExaminationResultReference",
                        dbContext.GetPrintExaminationReferenceViewModel(new EntrantKey(applicationID.Value, GetUserInfo())));
                }

            return HttpNotFound();
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationAddressStep()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                PersonalRecordsAddressViewModel model = new PersonalRecordsAddressViewModel
                {
                    ApplicationStep = ApplicationStepType.Address
                };
                return PartialView("Portlets/Applications/ApplicationAddress",
                    dbContext.FillPersonalAddress(model, false, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { 0, PortletType.ApplicationStepLanguage })]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationLanguages()
        {
            UpdateApplicationIDFromRequest();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                EntrantLanguageViewModel model = new EntrantLanguageViewModel { ApplicationStep = ApplicationStepType.Languages, IsView = false, ApplicationID = ApplicationID };
                return View("ApplicationLanguages", dbContext.FillLanguages(model, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationLanguagesStep()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                EntrantLanguageViewModel model = new EntrantLanguageViewModel { ApplicationStep = ApplicationStepType.Languages, IsView = false };
                return PartialView("Portlets/Applications/ApplicationLanguages", dbContext.FillLanguages(model, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { 0, PortletType.ApplicationStepDocuments })]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationDocuments()
        {
            UpdateApplicationIDFromRequest();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                ApplicationEntrantDocumentsViewModel model = new ApplicationEntrantDocumentsViewModel
                {
                    ApplicationStep = ApplicationStepType.Documents
                };
                return View("ApplicationDocuments",
                    dbContext.FillApplicationDocumentList(model, false, ApplicationID));
            }
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationDocumentsStep()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                ApplicationEntrantDocumentsViewModel model = new ApplicationEntrantDocumentsViewModel
                {
                    ApplicationStep = ApplicationStepType.Documents
                };
                return PartialView("Portlets/Applications/ApplicationDocuments",
                    dbContext.FillApplicationDocumentList(model, false, ApplicationID));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { 0, PortletType.ApplicationStepIndividualAchivements })]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationIndividualAchivements()
        {
            UpdateApplicationIDFromRequest();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                var app = dbContext.Application.FirstOrDefault(x => x.ApplicationID == ApplicationID);
                var entrantID = app.EntrantID;

                var model = dbContext.GetApplicationIndividualAchivements(ApplicationID, entrantID);
                return View("ApplicationIndividualAchivements", model);
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { 0, PortletType.ApplicationStepAdditional })]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationAdditionalInfo()
        {
            // TODO: implement when approved (Eventually)
            return View("ApplicationAdditionalInfo");
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationAdditionalInfoStep()
        {
            // TODO: implement when approved (Eventually)
            return PartialView("Portlets/Applications/ApplicationAdditionalInfo");
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { 0, PortletType.ApplicationStepTests })]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationEntranceTest()
        {
            UpdateApplicationIDFromRequest();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("ApplicationEntranceTest", dbContext.FillApplicationEntranceTestC(new ApplicationEntranceTestViewModelC(), false, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationEntranceTestStep()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Applications/ApplicationEntranceTestC", dbContext.FillApplicationEntranceTestC(new ApplicationEntranceTestViewModelC(), false, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.SaveApplicationDocuments)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SaveApplicationDocuments(string model)
        {
            ValueProvider = JsonRequestWrapper.GetValueProvider(model);
            ApplicationEntrantDocumentsViewModel modelT = new ApplicationEntrantDocumentsViewModel();
            RefreshModel(modelT);

            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                if (ApplicationID == 0 && modelT.ApplicationID != 0) ApplicationID = modelT.ApplicationID;
                return dbContext.SaveApplicationDocumentList(modelT, GetUserInfo(), ApplicationID, InstitutionID);
            }
        }

        /// <summary>
        /// Сохранение подачи/проверки сведений
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [PortletAjaxLink(PortletType.SaveApplicationCheck)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SaveApplicationCheck(string model)
        {
            //берём модель нужного типа
            ValueProvider = JsonRequestWrapper.GetValueProvider(model);
            ApplicationSendingViewModel modelT = new ApplicationSendingViewModel();
            RefreshModel(modelT);

            using (ApplicationPrioritiesEntities context = new ApplicationPrioritiesEntities())
            {
                modelT.Priorities = context.FillExistingPriorities(modelT.ApplicationID);
            }

            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                if (ApplicationID == 0 && modelT.ApplicationID != 0) ApplicationID = modelT.ApplicationID;

                //показываем ошибки только если не уходим назад. 
                bool isDraft = dbContext.Application.Any(x => x.ApplicationID == ApplicationID && x.StatusID == ApplicationStatusType.Draft);
                var isBackOrSave = modelT.ActionCommand == "back" || (modelT.ActionCommand == "save" && isDraft);

                if (!ModelState.IsValid && !isBackOrSave)
                    return new AjaxResultModel(ModelState);


                return dbContext.SaveApplicationsChecks(modelT, GetUserInfo(), ApplicationID, isBackOrSave);
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.SendEntrantApplication)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SendEntrantApplication(string model)
        {
            ValueProvider = JsonRequestWrapper.GetValueProvider(model);
            var modelT = new ApplicationSendingViewModel();
            RefreshModel(modelT);

            if (ApplicationID == 0 && modelT.ApplicationID != 0) ApplicationID = modelT.ApplicationID;
            return SendEntrantApplication(ApplicationID);
        }

        /// <summary>
        /// Подаём заявление (копируем данные по абитуриентам)
        /// </summary>
        public static ActionResult SendEntrantApplication(int applicationID)
        {
            using (var dbContext = new EntrantsEntities())
            {
                var application = dbContext.ApplicationWhere(applicationID).First();
                if (application == null)
                    throw new ArgumentException("Application is required");

                //if (application.StatusID == ApplicationStatusType.Draft)
                //    dbContext.SendApplication(applicationID);
            }

            ApplicationRatingCalculator.CalculateApplicationRating(applicationID);
            return new AjaxResultModel("");
        }

        private const string FirstHigherEducationLabel = "Высшее профессиональное образование получаю впервые";
        private const string FamiliarWithLicenseAndRules = "С лицензией на право осуществления образовательной деятельности, свидетельством о государственной аккредитации и приложениями к нему, Правилами приема и условиями обучения в данном образовательном учреждении высшего профессионального образования, правилами подачи апелляций ознакомлен";

        private void FillApplicationSendingOUTypeFields(ViewResultBase applicationSending, ApplicationSendingViewModel model)
        {
            applicationSending.ViewData["FirstHigherEducationLabel"] = model.IsVUZ ? FirstHigherEducationLabel : FirstHigherEducationLabel.Replace("Высшее", "Среднее");
            applicationSending.ViewData["FamiliarWithLicenseAndRules"] = model.IsVUZ ? FamiliarWithLicenseAndRules : FamiliarWithLicenseAndRules.Replace("высшего", "среднего");
            applicationSending.ViewData["OULevelGenetive"] = model.IsVUZ ? "высшего" : "среднего";
            applicationSending.ViewData["Institution"] = model.IsVUZ ? "ВУЗ" : "CCУЗ";
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink", MethodParams = new object[] { 0, PortletType.ApplicationStepSending })]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationSending()
        {
            UpdateApplicationIDFromRequest();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                ApplicationSendingViewModel model = new ApplicationSendingViewModel { ApplicationStep = ApplicationStepType.Sending };

                ViewResult applicationSending = View("ApplicationSending", dbContext.FillApplicationSending(model, new PersonalRecordsDataViewModel(), GetUserInfo(), ApplicationID));
                FillApplicationSendingOUTypeFields(applicationSending, model);
                return applicationSending;
            }
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult ApplicationSendingData()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                ApplicationSendingViewModel model = new ApplicationSendingViewModel { ApplicationStep = ApplicationStepType.Sending };
                PartialViewResult applicationSendingData = PartialView("Portlets/Applications/ApplicationSendingPage",
                    dbContext.FillApplicationSending(model, new PersonalRecordsDataViewModel { MaxFileSize = GetMaxAllowedFileLength() },
                    GetUserInfo(), ApplicationID));
                FillApplicationSendingOUTypeFields(applicationSendingData, model);
                return applicationSendingData;
            }
        }

        [PortletAjaxLink(PortletType.GetApplicationSendingParentsTab)]
        [Authorize]
        public ActionResult ApplicationSendingParentsTab()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                PersonalRecordsDataViewModel model = new PersonalRecordsDataViewModel
                {
                    MaxFileSize = GetMaxAllowedFileLength(),
                    ApplicationStep = ApplicationStepType.ParentData
                };
                return PartialView("Portlets/Entrants/PersonalRecordsDataView",
                    dbContext.FillPersonalData(model, true, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [PortletAjaxLink(PortletType.GetApplicationSendingAddressTab)]
        [Authorize]
        public ActionResult ApplicationSendingAddressTab()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                PersonalRecordsAddressViewModel model = new PersonalRecordsAddressViewModel
                {
                    ApplicationStep = ApplicationStepType.Address
                };
                return PartialView("Portlets/Entrants/PersonalRecordsAddressView",
                    dbContext.FillPersonalAddress(model, true, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [PortletAjaxLink(PortletType.GetApplicationSendingDocumentsTab)]
        [Authorize]
        public ActionResult ApplicationSendingDocumentsTab()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                ApplicationEntrantDocumentsViewModel model = new ApplicationEntrantDocumentsViewModel
                {
                    ApplicationStep = ApplicationStepType.Documents
                };
                return PartialView("Portlets/Applications/ApplicationDocumentsView",
                    dbContext.FillApplicationDocumentList(model, true, ApplicationID));
            }
        }

        [PortletAjaxLink(PortletType.GetApplicationSendingTestsTab)]
        [Authorize]
        public ActionResult ApplicationSendingTestsTab()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Applications/ApplicationEntranceTestCView", dbContext.FillApplicationEntranceTestC(new ApplicationEntranceTestViewModelC(), true, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [PortletAjaxLink(PortletType.GetApplicationSendingLanguagesTab)]
        [Authorize]
        public ActionResult ApplicationSendingLanguagesTab()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                EntrantLanguageViewModel model = new EntrantLanguageViewModel
                {
                    ApplicationStep = ApplicationStepType.Languages,
                    IsView = true
                };
                return PartialView("Portlets/Entrants/EntrantLanguageView",
                    dbContext.FillLanguages(model, new EntrantKey(ApplicationID, GetUserInfo())));
            }
        }

        [PortletAjaxLink(PortletType.SaveCurrentApplicationID)]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SaveSelectedApplicationID(int? appID)
        {
            if (!appID.HasValue)
                return new EmptyResult();
            ISession session = ServiceLocator.Current.GetInstance<ISession>();
            session.SetValue(ApplicationSessionKey, appID);
            return Json(new { Url = PortletLinkHelper.ApplicationViewLink(appID.Value) });
        }

        [PortletAjaxLink(PortletType.SaveStructureItemID)]
        [Authorize]
        public ActionResult SaveStructureItemID(string structureItemID)
        {
            ISession session = ServiceLocator.Current.GetInstance<ISession>();
            session.SetValue(StructureItemSessionKey, structureItemID);
            return Json("");
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationLink",
            MethodParams = new object[] { 0, PortletType.ApplicationAdd })]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult AddApplication()
        {
            // страница для добавления заявления. Идентификатор структурного элемента хранится в сессии. Имя ключа в сессии - см. переменную StructureItemSessionKey
            ISession session = ServiceLocator.Current.GetInstance<ISession>();
            int structureID = session.GetValue<int>(StructureItemSessionKey);
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                int applicationID = dbContext.CreateApplication(GetUserInfo(), structureID);
                session.SetValue(ApplicationSessionKey, applicationID);
                //return Content("<b>StructureItem ID: {0}</b>".FormatWith(structureID));
                return ApplicationPersonalDataStep();
            }
        }

        //		[GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewLink",
        //			MethodParams = new object[] { "" })]
        [Authorize]
        private ActionResult ApplicationView(int tabNumber = 0)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                ControllerWrapperExecutor cwe = new ControllerWrapperExecutor();
                cwe.CustomParameters["UserInfo"] = GetUserInfo();
                cwe.CustomParameters[ApplicationSessionKey] = ApplicationID;

                ApplicationViewModel model = new ApplicationViewModel();
                if (tabNumber == 0)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewCommonTab());
                else if (tabNumber == 1)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewPersonalTab());
                else if (tabNumber == 2)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewAddressTab());
                else if (tabNumber == 3)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewDocumentsTab());
                else if (tabNumber == 4)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewTestsTab());
                else if (tabNumber == 5)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewLanguagesTab());
                else if (tabNumber == 6)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewPrintTemplatesTab());

                return View("ApplicationView", dbContext.FillApplicationView(model, GetUserInfo(), ApplicationID));
            }
        }

        [Authorize]
        public ActionResult ApplicationViewPopup(int tabNumber, int applicationID, bool contentOnly)
        {
            using (var dbContext = new EntrantsEntities())
            {
                var cwe = new ControllerWrapperExecutor();
                cwe.CustomParameters["UserInfo"] = GetUserInfo();
                cwe.CustomParameters[ApplicationSessionKey] = applicationID;
                cwe.IsInsidePortlet = false;

                var model = new ApplicationViewModel { ApplicationID = applicationID };

                var key = "ApplicationViewPopup_" + applicationID;
                var cache = ServiceLocator.Current.GetInstance<ICache>();
                var app = cache.Get<Application>(key, null);
                if (app == null)
                {
                    app = dbContext.GetApplication(applicationID);
                    cache.Insert(key, app, 120);
                }

                model.CanView = true;
                model.CanEdit = ApplicationStatusType.IsEditable(app.StatusID);

                if (tabNumber == 0)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewCommonTab());
                else if (tabNumber == 1)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewPersonalTab());
                else if (tabNumber == 2)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewAddressTab());
                else if (tabNumber == 3)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewDocumentsTab());
                else if (tabNumber == 4)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewTestsTab());
                else if (tabNumber == 5)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewIndividualAchivementsTab());
                else if (tabNumber == 6)
                    model.Content = cwe.DoViewControllerAction<ApplicationController>(x => x.ApplicationViewPrintTemplatesTab());

                if (app.StatusID == ApplicationStatusType.Draft)
                {
                    model.CanView = false;
                    model.DenyMessage = "Доступ запрещен: заявление еще не отправлено";
                }

                if (contentOnly)
                {
                    //dbContext.FillApplicationView(model, InstitutionID, applicationID);
                    if (model.CanView)
                        return Content(model.Content);
                    return Content(model.DenyMessage);
                }

                //return PartialView("Application/ApplicationViewPopup", dbContext.FillApplicationView(model, InstitutionID, applicationID));
                return PartialView("Application/ApplicationViewPopup", model);
            }
        }

        public Application GetCachedPopUpApplication(int applicationId)
        {
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            return cache.Get<Application>("ApplicationViewPopup_" + applicationId, null);
        }

        public static void ClearApplicationCache(int applicationId)
        {
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            cache.RemoveAllWithPrefix(
                "ApplicationViewPopup_" + applicationId,
                "ApplicationViewCommonTab_" + applicationId,
                "ApplicationViewPersonalTab_" + applicationId,
                "ApplicationViewAddressTab_" + applicationId,
                "ApplicationViewDocumentsTab_" + applicationId,
                "ApplicationViewTestsTab_" + applicationId,
                "ApplicationViewLanguagesTab_" + applicationId,
                "ApplicationViewIndividualAchivementsTab_" + applicationId
            );

            //var application = EntrantCacheManager.GetFirst<Application>(x => x.ApplicationID == applicationId);
            //if (application != null)
            //{
            //    EntrantCacheManager.Remove<GVUZ.Model.Entrants.Entrant>(application.EntrantID);
            //    EntrantCacheManager.Remove<Application>(applicationId);
            //}
        }

        [Authorize]
        public ActionResult ApplicationViewCommonTab()
        {
            PartialViewResult applicationCommonInfo;

            var key = "ApplicationViewCommonTab_" + ApplicationID;
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var cachedModel = cache.Get<ApplicationCommonInfoViewModel>(key, null);
            if (cachedModel != null)
            {
                applicationCommonInfo = PartialView("Portlets/Applications/ApplicationCommonInfo", cachedModel);
            }
            else
            {
                using (var dbContext = new EntrantsEntities())
                {
                    cachedModel = new ApplicationCommonInfoViewModel();
                    applicationCommonInfo = PartialView("Portlets/Applications/ApplicationCommonInfo", dbContext.FillApplicationCommonInfo(cachedModel, GetUserInfo(), ApplicationID));
                    cache.Insert(key, cachedModel, 120);
                }
            }

            applicationCommonInfo.ViewData["Institution"] = cachedModel.IsVUZ ? "ВУЗ" : "CCУЗ";
            return applicationCommonInfo;
        }

        [Authorize]
        public ActionResult ApplicationViewPersonalTab()
        {
            PartialViewResult applicationCommonInfo;

            var key = "ApplicationViewPersonalTab_" + ApplicationID;
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var cachedModel = cache.Get<PersonalRecordsDataViewModel>(key, null);
            if (cachedModel != null)
            {
                applicationCommonInfo = PartialView("Portlets/Entrants/PersonalRecordsDataView", cachedModel);
            }
            else
            {
                using (var dbContext = new EntrantsEntities())
                {
                    cachedModel = new PersonalRecordsDataViewModel { MaxFileSize = GetMaxAllowedFileLength(), ApplicationStep = ApplicationStepType.Sending };
                    applicationCommonInfo = PartialView("Portlets/Entrants/PersonalRecordsDataView",
                        dbContext.FillPersonalData(cachedModel, true, new EntrantKey(ApplicationID, GetUserInfo())));
                    cache.Insert(key, cachedModel, 120);
                }
            }

            return applicationCommonInfo;
        }

        [Authorize]
        public ActionResult ApplicationViewAddressTab()
        {
            PartialViewResult applicationCommonInfo;

            var key = "ApplicationViewAddressTab_" + ApplicationID;
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var cachedModel = cache.Get<PersonalRecordsAddressViewModel>(key, null);
            if (cachedModel != null)
            {
                applicationCommonInfo = PartialView("Portlets/Entrants/PersonalRecordsAddressView", cachedModel);
            }
            else
            {
                using (var dbContext = new EntrantsEntities())
                {
                    cachedModel = new PersonalRecordsAddressViewModel { ApplicationStep = ApplicationStepType.Sending };
                    applicationCommonInfo = PartialView("Portlets/Entrants/PersonalRecordsAddressView",
                        dbContext.FillPersonalAddress(cachedModel, true, new EntrantKey(ApplicationID, GetUserInfo())));
                    cache.Insert(key, cachedModel, 120);
                }
            }

            return applicationCommonInfo;
        }

        [Authorize]
        public ActionResult ApplicationViewDocumentsTab()
        {
            PartialViewResult applicationCommonInfo;

            var key = "ApplicationViewDocumentsTab_" + ApplicationID;
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var cachedModel = cache.Get<ApplicationEntrantDocumentsViewModel>(key, null);
            if (cachedModel != null)
            {
                applicationCommonInfo = PartialView("Portlets/Applications/ApplicationDocumentsView", cachedModel);
            }
            else
            {
                using (var dbContext = new EntrantsEntities())
                {
                    cachedModel = new ApplicationEntrantDocumentsViewModel();
                    applicationCommonInfo = PartialView("Portlets/Applications/ApplicationDocumentsView",
                        dbContext.FillApplicationDocumentList(cachedModel, true, ApplicationID));
                    cache.Insert(key, cachedModel, 120);
                }
            }

            return applicationCommonInfo;
        }

        [Authorize]
        public ActionResult ApplicationViewTestsTab()
        {
            PartialViewResult applicationCommonInfo;

            var key = "ApplicationViewTestsTab_" + ApplicationID;
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var cachedModel = cache.Get<ApplicationEntranceTestViewModelC>(key, null);
            if (cachedModel != null)
            {
                applicationCommonInfo = PartialView("Portlets/Applications/ApplicationEntranceTestCView", cachedModel);
            }
            else
            {
                using (var dbContext = new EntrantsEntities())
                {
                    cachedModel = new ApplicationEntranceTestViewModelC();
                    applicationCommonInfo = PartialView("Portlets/Applications/ApplicationEntranceTestCView",
                        dbContext.FillApplicationEntranceTestC(cachedModel, true, new EntrantKey(ApplicationID, GetUserInfo())));
                    cache.Insert(key, cachedModel, 120);
                }
            }

            return applicationCommonInfo;
        }

        [Authorize]
        public ActionResult ApplicationViewLanguagesTab()
        {
            PartialViewResult applicationCommonInfo;

            var key = "ApplicationViewLanguagesTab_" + ApplicationID;
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var cachedModel = cache.Get<EntrantLanguageViewModel>(key, null);
            if (cachedModel != null)
            {
                applicationCommonInfo = PartialView("Portlets/Entrants/EntrantLanguageView", cachedModel);
            }
            else
            {
                using (EntrantsEntities dbContext = new EntrantsEntities())
                {
                    cachedModel = new EntrantLanguageViewModel { IsView = true, ApplicationStep = ApplicationStepType.Sending };
                    applicationCommonInfo = PartialView("Portlets/Entrants/EntrantLanguageView",
                        dbContext.FillLanguages(cachedModel, new EntrantKey(ApplicationID, GetUserInfo())));
                    cache.Insert(key, cachedModel, 120);
                }
            }

            return applicationCommonInfo;
        }

        [Authorize]
        public ActionResult ApplicationViewIndividualAchivementsTab()
        {
            PartialViewResult applicationCommonInfo;

            var key = "ApplicationViewIndividualAchivementsTab_" + ApplicationID;
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var cachedModel = cache.Get<IndividualAchivementsViewModel>(key, null);
            if (cachedModel != null)
            {
                applicationCommonInfo = PartialView("Portlets/Applications/ApplicationIndividualAchivementsView", cachedModel);
            }
            else
            {
                using (var dbContext = new EntrantsEntities())
                {
                    var app = GetCachedPopUpApplication(ApplicationID);
                    int entrantId = app != null ? app.EntrantID : dbContext.GetApplication(ApplicationID).EntrantID;

                    cachedModel = dbContext.GetApplicationIndividualAchivements(ApplicationID, entrantId);
                    applicationCommonInfo = PartialView("Portlets/Applications/ApplicationIndividualAchivementsView", cachedModel);
                    cache.Insert(key, cachedModel, 120);
                }
            }

            return applicationCommonInfo;
        }

        [Authorize]
        public ActionResult ApplicationViewPrintTemplatesTab()
        {
            return PartialView("Portlets/Entrants/EntrantPrintTemplatesView", ApplicationID);
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewLink", MethodParams = new object[] { })]
        [Authorize]
        public ActionResult ApplicationViewTab0App(int applicationID)
        {
            return ApplicationView(0);
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewTabLink", MethodParams = new object[] { PortletType.ApplicationViewCommonTab, 0, "" })]
        [Authorize]
        public ActionResult ApplicationViewTab0()
        {
            return ApplicationView(0);
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewTabLink", MethodParams = new object[] { PortletType.ApplicationViewPersonalTab, 1, "" })]
        [Authorize]
        public ActionResult ApplicationViewTab1()
        {
            return ApplicationView(1);
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewTabLink", MethodParams = new object[] { PortletType.ApplicationViewAddressTab, 2, "" })]
        [Authorize]
        public ActionResult ApplicationViewTab2()
        {
            return ApplicationView(2);
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewTabLink", MethodParams = new object[] { PortletType.ApplicationViewDocumentsTab, 3, "" })]
        [Authorize]
        public ActionResult ApplicationViewTab3()
        {
            return ApplicationView(3);
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewTabLink", MethodParams = new object[] { PortletType.ApplicationViewTestsTab, 4, "" })]
        [Authorize]
        public ActionResult ApplicationViewTab4()
        {
            return ApplicationView(4);
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewTabLink", MethodParams = new object[] { PortletType.ApplicationViewLanguageTab, 5, "" })]
        [Authorize]
        public ActionResult ApplicationViewTab5()
        {
            return ApplicationView(5);
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationViewTabLink", MethodParams = new object[] { PortletType.ApplicationViewLanguageTab, 6, "" })]
        [Authorize]
        public ActionResult ApplicationViewTab6()
        {
            return ApplicationView(6);
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult IncludeInOrderPage(int? applicationID)
        {
            if (!applicationID.HasValue)
                return new EmptyResult();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                ApplicationIncludeInOrderViewModel model = new ApplicationIncludeInOrderViewModel { ApplicationID = applicationID.Value };

                return PartialView("Application/ApplicationIncludeInOrder", dbContext.FillIncludeInOrderViewModel(model));
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult IncludeInOrderFromRecListPage(int? applicationID, int? recListId)
        {
            if (!applicationID.HasValue)
                return new EmptyResult();
            if (!recListId.HasValue)
                return new EmptyResult();

            GVUZ.Model.RecommendedLists.RecomendedLists recListElement;

            using (RecommendedListsEntities recListContext = new RecommendedListsEntities())
            {
                recListElement = recListContext.GetListElement(recListId.Value);

                using (EntrantsEntities dbContext = new EntrantsEntities())
                {
                    ApplicationIncludeInOrderViewModel model = new ApplicationIncludeInOrderViewModel { ApplicationID = applicationID.Value };
                    if (recListElement != null)
                    {
                        model.EducationFormID = recListElement.EduFormID;
                        model.DirectionID = recListElement.CompetitiveGroup.CompetitiveGroupItem.First(x => x.DirectionID == recListElement.DirectionID).CompetitiveGroupItemID;
                    }
                    return PartialView("Application/ApplicationIncludeInOrder", dbContext.FillIncludeInOrderViewModel(model));
                }
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel IncludeApplicationInOrder(ApplicationIncludeInOrderViewModel model)
        {
            ApplicationPrioritiesViewModel priorities = new ApplicationPrioritiesViewModel();
            using (ApplicationPrioritiesEntities context = new ApplicationPrioritiesEntities())
            {
                priorities = context.FillExistingPriorities(model.ApplicationID);
            }

            using (var dbContext = new EntrantsEntities())
            {
                ApplicationIncludeInOrderViewModel dataFromDb = new ApplicationIncludeInOrderViewModel { ApplicationID = model.ApplicationID };
                dataFromDb = dbContext.FillIncludeInOrderViewModel(dataFromDb);

                if (dataFromDb.NoOriginalDocuments && !priorities.ApplicationPriorities.Any(x =>
                     x.EducationSourceId == GVUZ.Model.Institutions.EDSourceConst.Paid &&
                     x.CompetitiveGroupItemId == model.DirectionID &&
                     x.EducationSourceId == model.EducationSourceID &&
                     x.Priority.HasValue))
                {
                    return new AjaxResultModel() { Data = "В приказ на бюджетные места можно включить только заявления, для которых предоставлены оригиналы документов об образовании" };
                }

                ApplicationRatingCalculator.CalculateApplicationRating(model.ApplicationID);
                return dbContext.IncludeApplicationInOrder(model);
            }
        }

        #region массовые операции


        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult IncludeAllApplicationsInOrderPage(string str)
        {
            string[] ids = str.Split('#');
            ApplicationIncludeInOrderViewModel model = new ApplicationIncludeInOrderViewModel();
            List<int> nlist = new List<int>();
            foreach (string sid in ids)
            {
                nlist.Add(Convert.ToInt32(sid));
            }

            model.applicationIds = nlist.ToArray();
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                model = dbContext.FillCommonModel4AllApps(model);
                return PartialView("Application/ApplicationIncludeInOrderList", model);
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel IncludeApplicationsInOrder(ApplicationIncludeInOrderViewModel model)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.IncludeAllApplicationsInOrder(model, InstitutionID);
            }
        }


        #endregion


        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel CheckApplication(int applicationID)
        {
            string strRes = string.Empty;
            using (var dbContext = new EntrantsEntities())
            {
                var model = new ApplicationIncludeInOrderViewModel { ApplicationID = applicationID };
                model = dbContext.FillIncludeInOrderViewModel(model);

                strRes = model.EducationFormID.ToString() + "#" + model.EducationSourceID.ToString();
            }
            return new AjaxResultModel(strRes);
        }


        //[HttpPost]
        //[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        //public AjaxResultModel SetOriginalDocumentsReceived(int? applicationID, bool? documentsReceived)
        //{
        //    if (applicationID == null || documentsReceived == null)
        //        return new AjaxResultModel(AjaxResultModel.DataError);
        //    using (EntrantsEntities dbContext = new EntrantsEntities())
        //    {
        //        return dbContext.SetDocumentsReceived(applicationID.Value, InstitutionID, documentsReceived.Value);
        //    }
        //}

        //[HttpPost]
        //[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        //  public AjaxResultModel SetDocumentOriginalReceived(int InstitutionID, int? applicationID,int? entrantDocumentID,bool? received,DateTime? receivedDate) {
        //   if(applicationID==null||received==null||entrantDocumentID==null) { return new AjaxResultModel(AjaxResultModel.DataError); }

        //   //ApplicationSQL.SetDocumentReceived(InstitutionID,applicationID.Value,entrantDocumentID.Value,(bool)received,receivedDate??DateTime.Today);

        //   using (var dbContext = new EntrantsEntities()){
        //      var results = dbContext.SetDocumentReceived(InstitutionID, applicationID.Value, entrantDocumentID.Value, (bool)received, receivedDate ?? DateTime.Today);
        //      ClearApplicationCache(applicationID.Value);
        //      return results;
        //   }
        //}


        [HttpPost]
        public ActionResult GetAllowedDocumentsForEntranceTest(int? applicationID, int? entranceTestItemID, int? docSourceID, int? groupID)
        {
            if (!entranceTestItemID.HasValue || !applicationID.HasValue || !docSourceID.HasValue)
                return new AjaxResultModel(AjaxResultModel.DataError);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.GetAllowedDocumentDocumentsForEntranceTest(applicationID.Value, entranceTestItemID.Value, docSourceID.Value, groupID ?? 0);
            }
        }

        [HttpPost]
        public ActionResult GetAbilityToEnterManualValue(int? applicationID, int? entranceTestItemID)
        {
            if (!entranceTestItemID.HasValue || !applicationID.HasValue)
                return new AjaxResultModel(AjaxResultModel.DataError);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.GetAbilityToEnterManualValue(applicationID.Value, entranceTestItemID.Value);
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel SaveApplicationEntranceTestSelectedData(ApplicationEntranceTestViewModelC.AttachedDocumentData model)
        {
            return SaveEntranceTestSelectedData(model);
        }

        [HttpPost]
        public AjaxResultModel SaveEgeForOlympics(int? documentId, int? egeResult)
        {
            if (!documentId.HasValue)
                return new AjaxResultModel("Документ не определён");

            using (var dbContext = new EntrantsEntities())
            {
                var doc = dbContext.ApplicationEntranceTestDocument.FirstOrDefault(x => x.ID == documentId);

                if (doc == null)
                    return new AjaxResultModel("Документ не определён");

                if (doc.BenefitID != 3)
                    return new AjaxResultModel("Льгота не соответствует льготе \"Приравнивание к лицам, набравшим максимальное количество баллов по ЕГЭ\"");

                if (!egeResult.HasValue || egeResult.Value == 0)
                    return new AjaxResultModel("Балл ЕГЭ должен быть задан");

                doc.EgeResultValue = egeResult.Value;
                dbContext.SaveChanges();

#warning Не разобрался что к чему - но при созранении документа выдается тип документа - OlympicTotalDocumentViewModel в классе EntrantController строка 441 - EntrantDocumentExtensions.InstantiateDocumentByType(docType)
                BaseDocumentViewModel baseDoc = doc.EntrantDocumentID.HasValue ? dbContext.LoadEntrantDocument(doc.EntrantDocumentID.Value) : null;
                var olympDoc = baseDoc as OlympicDocumentViewModel;

                if (olympDoc != null && !dbContext.CheckEGEScoreForSubject(doc.EntranceTestItemID.Value, doc.ApplicationID, olympDoc.OlympicID))
                {
                    var docInfo = new
                    {
                        BenefitId = doc.BenefitID,
                        BenefitErrorMessage = "",
                        ID = doc.ID,
                        CompetitiveGroupID = doc.CompetitiveGroupID,
                        DocumentDescription = dbContext.GetDocDescription(doc.EntrantDocumentID.Value, 0, 0, 0)
                    };

                    var errorItems = new { doc.EntranceTestItemID.Value };

                    return new AjaxResultModel()
                    {
                        Data = errorItems,
                        Extra = docInfo
                    };
                }

                return new AjaxResultModel();
            }
        }

        /// <summary>
        /// Сохраняем РВИ
        /// </summary>
        public static AjaxResultModel SaveEntranceTestSelectedData(ApplicationEntranceTestViewModelC.AttachedDocumentData model)
        {
            if (model.EntranceTestItemID == 0)
            {
                return SaveEntranceTestSelectedDataGlobal(model);
            }

            Application app;
            using (var dbContext = new EntrantsEntities())
            {
                int? olympicsId = null;

                app = dbContext.GetApplication(model.ApplicationID);
                ApplicationEntranceTestDocument doc = dbContext.ApplicationEntranceTestDocument
                    .FirstOrDefault(x => x.ApplicationID == model.ApplicationID && x.EntranceTestItemID == model.EntranceTestItemID);
                var testData = dbContext.EntranceTestItemC.Where(x => x.EntranceTestItemID == model.EntranceTestItemID)
                    .Select(x => new { x.EntranceTestTypeID, x.SubjectID, x.CompetitiveGroupID }).Single();
                if (doc == null)
                {
                    doc = new ApplicationEntranceTestDocument();
                    dbContext.ApplicationEntranceTestDocument.AddObject(doc);
                }

                doc.SourceID = model.SourceID;
                doc.ResultValue = model.ResultValue;
                if (model.EntrantDocumentID == 0 && model.SourceID == 3)
                    return new AjaxResultModel(AjaxResultModel.DataError);
                if (model.EntrantDocumentID > 0)
                {
#warning https://redmine.armd.ru/issues/18408k
                    //string docDescription = GetDocDescription(dbContext, model.EntrantDocumentID, testData.SubjectID ?? 0, model.EntranceTestItemID, testData.CompetitiveGroupID);
                    //if (docDescription == null)
                    //    return new AjaxResultModel("Данный документ не подходит как основание для оценки для выбранной дисцпилины");

                    BaseDocumentViewModel baseDoc = dbContext.LoadEntrantDocument(model.EntrantDocumentID);
                    baseDoc.FillData(dbContext, true, null, null);
                    var egeDoc = baseDoc as EGEDocumentViewModel;
                    if (egeDoc != null)
                    {
                        //проставляем правильный иностранный язык
                        var subjectValue = egeDoc.Subjects.Where(x => x.SubjectID == testData.SubjectID).Select(x => (decimal?)x.Value).FirstOrDefault();
                        if (subjectValue == null && DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).Any(x => x.Key == testData.SubjectID && x.Value == LanguageSubjects.ForeignLanguage))
                        {
                            subjectValue = egeDoc.Subjects.Where(x => x.SubjectName.StartsWith(LanguageSubjects.ForeignLanguagePrefix))
                                .OrderByDescending(x => x.Value).Select(x => (decimal?)x.Value).FirstOrDefault();
                        }

                        doc.ResultValue = subjectValue.To(0.0m);
                    }

                    var giaDoc = baseDoc as GiaDocumentViewModel;
                    if (giaDoc != null)
                    {
                        var subjectValue = giaDoc.Subjects.Where(x => x.SubjectID == testData.SubjectID).Select(x => (decimal?)x.Value).FirstOrDefault();
                        if (subjectValue == null && dbContext.Subject.Any(x => x.SubjectID == testData.SubjectID && x.Name == LanguageSubjects.ForeignLanguage))
                        {
                            subjectValue = giaDoc.Subjects.Where(x => x.SubjectName.StartsWith(LanguageSubjects.ForeignLanguagePrefix))
                                .OrderByDescending(x => x.Value).Select(x => (decimal?)x.Value).FirstOrDefault();
                        }

                        doc.ResultValue = subjectValue.To(0.0m);
                    }

                    OlympicDocumentViewModel olDoc = baseDoc as OlympicDocumentViewModel;
                    OlympicTotalDocumentViewModel totDoc = baseDoc as OlympicTotalDocumentViewModel;
                    //если олимпиада, то проставляем тип льготы
                    if (olDoc != null || totDoc != null)
                    {
                        doc.ResultValue = 100;
                        if (testData.EntranceTestTypeID == EntranceTestType.MainType)
                            doc.BenefitID = 3; //Макс. баллы ЕГЭ
                        else
                            doc.BenefitID = 2; //Без доп. вступительных испытаний
                        doc.EgeResultValue = model.ResultValue;
                        olympicsId = olDoc == null ? (int?)null : olDoc.OlympicID;
                    }

                    doc.EntrantDocumentID = model.EntrantDocumentID;
                }

                doc.EntranceTestItemID = model.EntranceTestItemID;
                doc.ApplicationID = model.ApplicationID;
                doc.SubjectID = testData.SubjectID;
                if (model.SourceID == EntranceTestSource.OUTestSourceId)
                {
                    doc.InstitutionDocumentDate = model.InstitutionDocumentDate;
                    doc.InstitutionDocumentNumber = model.InstitutionDocumentNumber;
                    doc.InstitutionDocumentTypeID = model.InstitutionDocumentTypeID;
                    var ability = dbContext.GetAbilityToEnterManualValueInternal(model.ApplicationID, model.EntranceTestItemID);
                    doc.HasEge = ability.DocExisting != null && ability.DocExisting.Length > 0;
                }
                else
                {
                    doc.InstitutionDocumentDate = null;
                    doc.InstitutionDocumentNumber = null;
                    doc.InstitutionDocumentTypeID = null;
                }

                dbContext.SaveChanges();
                dbContext.UpdateAppDocumentReceived(model.ApplicationID);

                // Проверим баллы ЕГЭ на соответствие условиям применения льготы

                if (olympicsId.HasValue && !dbContext.CheckEGEScoreForSubject(model.EntranceTestItemID, model.ApplicationID, olympicsId.Value))
                {
                    // Олимпиада есть, но баллы не соответствуют.

                    var docInfo = new
                    {
                        BenefitId = doc.BenefitID,
                        BenefitErrorMessage = "",
                        ID = doc.ID,
                        CompetitiveGroupID = doc.CompetitiveGroupID,
                        DocumentDescription = dbContext.GetDocDescription(model.EntrantDocumentID, 0, 0, 0)
                    };

                    var errorItems = new { model.EntranceTestItemID };

                    return new AjaxResultModel()
                    {
                        Data = errorItems,
                        Extra = docInfo
                    };
                }
            }

            if (app.StatusID != ApplicationStatusType.Draft)
                ApplicationRatingCalculator.CalculateApplicationRating(model.ApplicationID);
            return new AjaxResultModel();
        }

        /// <summary>
        /// Сохраняем льготу
        /// </summary>
        private static AjaxResultModel SaveEntranceTestSelectedDataGlobal(ApplicationEntranceTestViewModelC.AttachedDocumentData model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                if (dbContext.ApplicationEntranceTestDocument
                    .Any(x => x.ApplicationID == model.ApplicationID
                        && x.EntranceTestItemID == null
                        && x.CompetitiveGroupID != model.CompetitiveGroupID
                        && x.BenefitID == model.BenefitID))
                    return new AjaxResultModel("Абитуриент уже использовал данную льготу в данном заявления для другого конкурса");

                // Закоментарено в ходе решения задачи № 26159 - добавление нескольких льгот в одну КГ
                // Всегда создаём новый документ, вне зависимости от того, что уже есть.

                ApplicationEntranceTestDocument doc = null;/* = dbContext.ApplicationEntranceTestDocument
                    .FirstOrDefault(x => x.ApplicationID == model.ApplicationID && x.EntranceTestItemID == null && x.CompetitiveGroupID == model.CompetitiveGroupID);*/
                if (doc == null)
                {
                    doc = new ApplicationEntranceTestDocument();
                    dbContext.ApplicationEntranceTestDocument.AddObject(doc);
                }

                doc.SourceID = null;
                doc.ResultValue = null;
                if (model.EntrantDocumentID == 0)
                    return new AjaxResultModel(AjaxResultModel.DataError);
                Application app = dbContext.GetApplication(model.ApplicationID);

#warning https://redmine.armd.ru/issues/18408
                //string docDescription = GetDocDescription(dbContext, model.EntrantDocumentID, 0, model.EntranceTestItemID, model.CompetitiveGroupID); 
                //if (docDescription == null)
                //    return new AjaxResultModel("Данный документ не подходит как основание для оценки для выбранной дисцпилины");

                doc.EntrantDocumentID = model.EntrantDocumentID;
                doc.EntranceTestItemID = null;
                doc.ApplicationID = model.ApplicationID;
                doc.SubjectID = null;
                doc.BenefitID = (short?)model.BenefitID;
                doc.CompetitiveGroupID = model.CompetitiveGroupID;
                dbContext.SaveChanges();
                if (app.StatusID != ApplicationStatusType.Draft)
                    ApplicationRatingCalculator.CalculateApplicationRating(model.ApplicationID);

                // Проверим, есть ли в олимпиаде баллы
                // Если есть, то проверим соответствие введённых баллов ЕГЭ минимально необходимым баллам

                List<int> errorTestItems = null;
                Dictionary<int, string> errorMessages = new Dictionary<int, string>();

                if (model.BenefitID == 1) // Поступление вне конкурса
                    errorTestItems = dbContext.CheckEGEScoresForCommonOlympics(
                        model.CompetitiveGroupID,
                        model.ApplicationID,
                        model.EntrantDocumentID,
                        out errorMessages);

                var docInfo = new
                {
                    BenefitId = doc.BenefitID,
                    BenefitErrorMessage = "",
                    ID = doc.ID,
                    CompetitiveGroupID = doc.CompetitiveGroupID,
                    DocumentDescription = dbContext.GetDocDescription(model.EntrantDocumentID, 0, 0, doc.CompetitiveGroupID.Value)
                };

                var errors = errorTestItems == null ? null :
                    errorTestItems.Select(x => new
                    {
                        ItemId = x,
                        Message = errorMessages[x]
                    }).ToArray();

                return new AjaxResultModel()
                {
                    Data = errors,
                    Extra = (errorTestItems == null || errorTestItems.Count == 0) ? null : docInfo
                };
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel DeleteApplicationEntranceTestSelectedData(int? docID)
        {
            if (!docID.HasValue)
                return new AjaxResultModel(AjaxResultModel.DataError);

            return DeleteEntranceTestSelectedData(docID.Value);
        }

        /// <summary>
        /// удаляем одно рви
        /// </summary>
        public AjaxResultModel DeleteEntranceTestSelectedData(int docID)
        {
            int appID = 0;
            var statusID = ApplicationStatusType.Draft;
            using (var dbContext = new EntrantsEntities())
            {
                ApplicationEntranceTestDocument doc = dbContext.ApplicationEntranceTestDocument
                    .Include("Application")
                    .FirstOrDefault(x => x.ID == docID);
                if (doc != null)
                {
                    statusID = doc.Application.StatusID;
                    appID = doc.ApplicationID;
                    dbContext.ApplicationEntranceTestDocument.DeleteObject(doc);
                    dbContext.SaveChanges();
                    dbContext.UpdateAppDocumentReceived(appID);
                }
                else return new AjaxResultModel();
            }

            if (statusID != ApplicationStatusType.Draft)
                ApplicationRatingCalculator.CalculateApplicationRating(appID);

            return new AjaxResultModel();
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel CheckApplicationEGEResults(int? applicationID, int? etID)
        {
            if (!applicationID.HasValue)
                return new AjaxResultModel(AjaxResultModel.DataError);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.CheckEgeResults(applicationID.Value, etID != null, etID);
            }
        }

        [HttpPost]
        public AjaxResultModel CheckEGEResults(AppResultsModel model)
        {
            model.userLogin = User.Identity.Name;
            a_logger.ErrorFormat("Проверка результатов ЕГЭ {0} для заявления {1} ({2}) методом '{3}' -> {4} : {5}",
                                 model.userLogin, model.ApplicationID, model.ApplicationNumber, model.method, model.doc, model.regNum);
            try
            {
                return new AjaxResultModel
                {
                    Data = ApplicationSQL.GetCheckEGEResults(model, a_logger),
                    Extra = ApplicationSQL.GetResultValueSubject(model.ApplicationID, model.EtiId)
                };
            }
            catch (SqlException ex)
            {
                a_logger.ErrorFormat("Ошибка SQL: {0}", ex.Message);
                logger.Error(ex, "Превышено время ожидания (SQL)");
                return new AjaxResultModel("Превышено время ожидания");
            }
            catch (Exception ex)
            {
                a_logger.ErrorFormat("Ошибка ({0}): {1}", ex.GetType().Name, ex.Message);
                logger.Error(ex, "Превышено время ожидания (другое)"); ;
                return new AjaxResultModel("Превышено время ожидания"); //ex.ToString()
            }
        }

        [HttpPost]
        public AjaxResultModel CheckOlympicResults(AppResultsModel model)
        {
            try
            {
                return new AjaxResultModel { Data = ApplicationSQL.GetCheckOlympicResults(model) };
            }
            catch (SqlException)
            {
                return new AjaxResultModel("Превышено время ожидания");
            }
            catch (Exception)
            {
                return new AjaxResultModel("Превышено время ожидания");
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel GetApplicationEGEResults(int? applicationID, string certificateNumber)
        {
            if (!applicationID.HasValue)
                return new AjaxResultModel(AjaxResultModel.DataError);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.GetEgeResults(applicationID.Value, certificateNumber);
            }
        }

        [HttpPost]
        public ActionResult SavePriorities(ApplicationPrioritiesViewModel data, bool? checkUnique, bool? checkZeroes)
        {
            // Сделаем все необходимые проверки
            var valMsgs = new Dictionary<string, string>();
            if (!CheckPriorities(data, checkUnique, checkZeroes, ref valMsgs))
            {
                AddModelErrors(valMsgs);
                var ajaxResultModel = new AjaxResultModel(ModelState);
                return Json(ajaxResultModel);
            }

            // Сохраним данные в БД

            using (var context = new ApplicationPrioritiesEntities())
            {
                context.SavePriorities(data);
            }

            // Вернём пустой результат, что скажет о том, что всё хорошо
            // Собственно данные возьмём при отображении
            return new AjaxResultModel() { Data = null };
        }

        [HttpPost]
        public AjaxResultModel GetAvailiableAndExistingPriorities(int? applicationId, int[] competitiveGroupIds, string[] directionKeys)
        {
            var data = new ApplicationPrioritiesViewModel();
            data.ApplicationId = applicationId.HasValue ? applicationId.Value : -1;

            if (competitiveGroupIds == null || directionKeys == null)
                return new AjaxResultModel() { Data = null };

            ApplicationPrioritiesViewModel existingPriorities = null;
            using (var context = new ApplicationPrioritiesEntities())
            {
                existingPriorities = context.FillExistingPriorities(applicationId.Value);
            }

            using (var dbContext = new EntrantsEntities())
            {
                var res =
                    dbContext.CompetitiveGroupItem.Where(
                        x => competitiveGroupIds.Contains(x.CompetitiveGroupID) && x.CompetitiveGroup.InstitutionID == InstitutionID)
                        .Select(x => new
                        {
                            x.CompetitiveGroupItemID,
                            x.CompetitiveGroupID,
                            x.CompetitiveGroup.EducationLevelID,
                            x.CompetitiveGroup.DirectionID,
                            x.NumberBudgetO,
                            x.NumberBudgetOZ,
                            x.NumberBudgetZ,
                            x.NumberPaidO,
                            x.NumberPaidOZ,
                            x.NumberPaidZ,
                            x.NumberQuotaO,
                            x.NumberQuotaOZ,
                            x.NumberQuotaZ,
                            GroupName = x.CompetitiveGroup.Name,
                            DirectionName = x.CompetitiveGroup.Direction.Name,
                            NumberTargetO = x.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetO) ?? 0,
                            NumberTargetOZ = x.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetOZ) ?? 0,
                            NumberTargetZ = x.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetZ) ?? 0,
                            TargetOrganizationsO = x.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.NumberTargetO > 0).Select(y => y.CompetitiveGroupTarget),
                            TargetOrganizationsOZ = x.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.NumberTargetOZ > 0).Select(y => y.CompetitiveGroupTarget),
                            TargetOrganizationsZ = x.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.NumberTargetZ > 0).Select(y => y.CompetitiveGroupTarget),
                        }).ToArray()
                                     .Where(x => directionKeys.Contains(x.EducationLevelID + "@" + x.DirectionID + "@" + x.CompetitiveGroupItemID))
                                     .ToArray();

                ApplicationPriorityViewModel ex = null;
                foreach (var element in res)
                {
                    /* ------------------------------------- Бюждетные места --------------------------- */
                    if (element.NumberBudgetO > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            14,
                            11,
                            element.GroupName,
                            element.DirectionName);
                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 11 &&
                                x.EducationSourceId == 14
                            );
                        priority.Id = (ex == null ? -1 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }

                    if (element.NumberBudgetOZ > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            14,
                            12,
                            element.GroupName,
                            element.DirectionName);
                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 12 &&
                                x.EducationSourceId == 14
                            );
                        priority.Id = (ex == null ? -2 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }

                    if (element.NumberBudgetZ > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            14,
                            10,
                            element.GroupName,
                            element.DirectionName);
                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 10 &&
                                x.EducationSourceId == 14
                            );
                        priority.Id = (ex == null ? -3 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }
                    /* ------------------------------------------------------------------------------------- */

                    /* ------------------------------------- Места по квотам --------------------------- */
                    if (element.NumberQuotaO > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            20,
                            11,
                            element.GroupName,
                            element.DirectionName);

                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 11 &&
                                x.EducationSourceId == 20
                            );
                        priority.Id = (ex == null ? -11 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }

                    if (element.NumberQuotaOZ > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            20,
                            12,
                            element.GroupName,
                            element.DirectionName);
                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 12 &&
                                x.EducationSourceId == 20
                            );
                        priority.Id = (ex == null ? -11 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }

                    if (element.NumberQuotaZ > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            20,
                            10,
                            element.GroupName,
                            element.DirectionName);
                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 10 &&
                                x.EducationSourceId == 20
                            );
                        priority.Id = (ex == null ? -11 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }
                    /* ------------------------------------------------------------------------------------- */

                    /* ------------------------------------- Платные места --------------------------- */
                    if (element.NumberPaidO > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            15,
                            11,
                            element.GroupName,
                            element.DirectionName);
                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 11 &&
                                x.EducationSourceId == 15
                            );
                        priority.Id = (ex == null ? -21 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }

                    if (element.NumberPaidOZ > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            15,
                            12,
                            element.GroupName,
                            element.DirectionName);
                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 12 &&
                                x.EducationSourceId == 15
                            );
                        priority.Id = (ex == null ? -22 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }

                    if (element.NumberPaidZ > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority
                            (
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            15,
                            10,
                            element.GroupName,
                            element.DirectionName);
                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 10 &&
                                x.EducationSourceId == 15
                            );
                        priority.Id = (ex == null ? -23 : ex.Id);
                        if (ex != null) priority.Priority = ex.Priority;
                        data.ApplicationPriorities.Add(priority);
                    }
                    /* ------------------------------------------------------------------------------------- */

                    /* ---------------------------- Целевой приём ------------------------------------------ */
                    if (element.NumberTargetO > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority(
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            16,
                            11,
                            element.GroupName,
                            element.DirectionName);

                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 11 &&
                                x.EducationSourceId == 16
                            );

                        priority.Id = (ex == null ? -31 : ex.Id);
                        if (ex != null)
                        {
                            priority.Priority = ex.Priority;
                            priority.CompetitiveGroupTargetId = ex.CompetitiveGroupTargetId;
                        }

                        priority.TargetOrganizations = res.SelectMany(x => x.TargetOrganizationsO).Select(x => new { ID = x.CompetitiveGroupTargetID, Name = x.Name })
                        .Distinct().OrderBy(y => y.Name).ToArray();

                        var organization = res.SelectMany(x => x.TargetOrganizationsO)
                            .Select(x => new { ID = x.CompetitiveGroupTargetID, Name = x.Name }).Distinct().OrderBy(y => y.Name)
                            .ToArray().FirstOrDefault(x => x.ID == priority.CompetitiveGroupTargetId);
                        if (organization != null)
                            priority.TargetOrganizationName = organization.Name;

                        data.ApplicationPriorities.Add(priority);
                    }

                    if (element.NumberTargetOZ > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority(
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            16,
                            12,
                            element.GroupName,
                            element.DirectionName);

                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 12 &&
                                x.EducationSourceId == 16
                            );

                        priority.Id = (ex == null ? -32 : ex.Id);

                        if (ex != null)
                        {
                            priority.Priority = ex.Priority;
                            priority.CompetitiveGroupTargetId = ex.CompetitiveGroupTargetId;
                        }

                        priority.TargetOrganizations = res.SelectMany(x => x.TargetOrganizationsOZ).Select(x => new { ID = x.CompetitiveGroupTargetID, Name = x.Name })
                        .Distinct().OrderBy(y => y.Name).ToArray();
                        var firstOrDefault = res.SelectMany(x => x.TargetOrganizationsOZ).Select(x => new { ID = x.CompetitiveGroupTargetID, Name = x.Name }).Distinct().OrderBy(y => y.Name).ToArray().FirstOrDefault(x => x.ID == priority.CompetitiveGroupTargetId);
                        if (firstOrDefault != null)
                            priority.TargetOrganizationName = firstOrDefault.Name;
                        data.ApplicationPriorities.Add(priority);
                    }

                    if (element.NumberTargetZ > 0)
                    {
                        ApplicationPriorityViewModel priority = CreatePriority(
                            element.CompetitiveGroupID,
                            element.CompetitiveGroupItemID,
                            16,
                            10,
                            element.GroupName,
                            element.DirectionName);

                        ex = existingPriorities.ApplicationPriorities.FirstOrDefault
                            (
                                x => x.CompetitiveGroupId == element.CompetitiveGroupID &&
                                x.CompetitiveGroupItemId == element.CompetitiveGroupItemID &&
                                x.EducationFormId == 10 &&
                                x.EducationSourceId == 16
                            );

                        priority.Id = (ex == null ? -33 : ex.Id);

                        if (ex != null)
                        {
                            priority.Priority = ex.Priority;
                            priority.CompetitiveGroupTargetId = ex.CompetitiveGroupTargetId;
                        }

                        priority.TargetOrganizations = res.SelectMany(x => x.TargetOrganizationsZ).Select(x => new { ID = x.CompetitiveGroupTargetID, Name = x.Name })
                        .Distinct().OrderBy(y => y.Name).ToArray();

                        var firstOrDefault = res.SelectMany(x => x.TargetOrganizationsZ).Select(x =>
                            new { ID = x.CompetitiveGroupTargetID, Name = x.Name }).Distinct().OrderBy(y => y.Name).ToArray()
                            .FirstOrDefault(x => x.ID == priority.CompetitiveGroupTargetId);
                        if (firstOrDefault != null)
                            priority.TargetOrganizationName = firstOrDefault.Name;

                        data.ApplicationPriorities.Add(priority);
                    }
                    /* ------------------------------------------------------------------------------------- */
                }
                return new AjaxResultModel() { Data = data };
            }
        }

        [HttpPost]
        public AjaxResultModel GetFormsForDirection(int? applicationId, int? directionId)
        {
            using (var dbContext = new ApplicationPrioritiesEntities())
            {
                if (directionId.HasValue)
                    return new AjaxResultModel { Data = dbContext.GetEduFormsForDirectionInApplication(applicationId.Value, directionId.Value) };
                else return new AjaxResultModel { Data = null };
            }
        }

        [HttpPost]
        public AjaxResultModel GetSourcesForDirection(int? applicationId, int? directionId, int? eduFormId)
        {
            using (var dbContext = new ApplicationPrioritiesEntities())
            {
                if (directionId.HasValue && eduFormId.HasValue)
                    return new AjaxResultModel { Data = dbContext.GetSourcesForApplication(applicationId.Value, directionId.Value, eduFormId.Value) };
                else return new AjaxResultModel { Data = null };
            }
        }


        #region IndividualAchievements
        [HttpPost]

        //public AjaxResultModel SaveIndividualAchievement(IndividualAchivementsViewModel.IndividualAchivementViewModel model)

        public AjaxResultModel SaveIndividualAchievement(ApplicationWz4ViewModel.IndividualAchivementViewModel model)
        {
            List<string> UidList = ApplicationSQL.GetUidList(model.InstitutionID);

            for (int i = 0; i < UidList.Count; i++)
            {
                if (model.UID == UidList[i])
                {
                    return new AjaxResultModel { Data = null, Extra = "Значение в поле Идентификатор должно быть уникальным среди всех индивидуальных достижений, определенных для образовательной организации" };
                    //return new AjaxResultModel("Такой UID уже используется!");
                }
            }
            if (model == null)
                return new AjaxResultModel("Ошибка передачи данных");

            //if(!model.IADocumentID.HasValue)
            //      return new AjaxResultModel("Превышен допустимый максимальный балл за индивидуальное достижение");

            if (!ModelState.IsValid && model.IAName.Length > 100)
                return new AjaxResultModel("Наименование индивидуального достижения имеет слишком большую длину");
            ApplicationWz4ViewModel.IndividualAchivementViewModel ach = null;
            try
            {
                ach = ApplicationSQL.SaveIndividualAchievement(model);
                if (model.ApplicationID.HasValue)
                {
                    ApplicationSQL.UpdateIndividualAchivementsMark(model.ApplicationID.Value);
                }
            }
            catch (Exception e)
            {
                return new AjaxResultModel("Ошибка сохранения." + e.ToString());
            }
            return new AjaxResultModel() { Data = ach };
        }


        [HttpPost]
        public AjaxResultModel DeleteIndividualAchievement(int? achievementID, int? entrantDocumentID)
        {
            if (!achievementID.HasValue || achievementID == 0)
                return new AjaxResultModel("Ошибка удаления индивидуального достижения - не найдено достижения для удаления");

            int applicationId = ApplicationSQL.GetApplicationIdByAchivementId(achievementID.Value);
            var result = ApplicationSQL.DeleteIndividualAchievement(achievementID.Value, entrantDocumentID.HasValue ? entrantDocumentID.Value : 0);
            ApplicationSQL.UpdateIndividualAchivementsMark(applicationId);
            return result;
        }
        #endregion

        private ApplicationPriorityViewModel CreatePriority(int GroupId, int GroupItemId, int EduSource, int EduForm, string GroupName, string DirectionName)
        {
            using (var dbContext = new EntrantsEntities())
            {
                var admissionItemType = dbContext.AdmissionItemType.FirstOrDefault(x => x.ItemTypeID == EduForm);
                var educationSource = dbContext.AdmissionItemType.FirstOrDefault(x => x.ItemTypeID == EduSource);
                return new ApplicationPriorityViewModel()
                {
                    CompetitiveGroupId = GroupId,
                    CompetitiveGroupItemId = GroupItemId,
                    CompetitiveGroupItemName = DirectionName,
                    CompetitiveGroupName = GroupName,
                    EducationFormId = EduForm,
                    EducationSourceId = EduSource,
                    EducationFormName = admissionItemType != null ? admissionItemType.Name : "",
                    EducationSourceName = educationSource != null ? educationSource.Name : ""
                };
            }
        }

        private bool CheckPriorities(ApplicationPrioritiesViewModel priorities, bool? checkUnique, bool? checkZeroez, ref Dictionary<string, string> validationMessages)
        {
            #region Проверка на то, что для каждого направления выбрана хотя бы одна комбинация источника финансирования и формы обучения
            var directionIds = priorities.ApplicationPriorities
                .Select(x => new { x.CompetitiveGroupItemId })
                .Distinct()
                .ToArray();

            bool error = false;

            foreach (var directionId in directionIds)
            {
                var selectedItems = priorities.ApplicationPriorities
                    .Where(x => x.CompetitiveGroupItemId == directionId.CompetitiveGroupItemId && x.Priority.HasValue)
                    .Count();

                if (selectedItems == 0)
                {
                    validationMessages[directionId.CompetitiveGroupItemId.ToString()] = "Для специальности на выбрано ни одной комбинации формы обучения и источника финансирования";
                    error = true;
                }
            }

            if (error)
            {
                validationMessages["Fake"] = "Fake";
                return false;
            }

            #endregion

            #region Проверка на то, что все приоритеты различны
            if (checkUnique.HasValue && checkUnique.Value)
            {
                int totalValues = priorities.ApplicationPriorities.Count(x => x.Priority.HasValue && x.Priority.Value != 0);
                int distinctValues = priorities.ApplicationPriorities
                    .Select(x => x.Priority)
                    .Distinct()
                    .Count(x => x.HasValue && x.Value != 0);

                if (totalValues > distinctValues)
                {
                    validationMessages["NonUniquePriorities"] = "Для нескольких условий приема указаны одинаковые приоритеты. Вы уверены, что хотите продолжить?";
                    return false;
                }
            }
            #endregion

            #region Проверка на то, что все приоритеты - нули
            if (checkZeroez.HasValue && checkZeroez.Value)
            {
                var zeroCount = priorities.ApplicationPriorities.Count(x => x.Priority.HasValue && x.Priority.Value == 0);

                if (zeroCount > 0 && zeroCount != priorities.ApplicationPriorities.Count)
                {
                    validationMessages["zeroMessage"] = "Нули могут быть указаны только в случае приема без учета приоритетов. Вы уверены, что хотите продолжить?";
                    return false;
                }
            }
            #endregion

            #region Также проверим, что для целевого приёма задана организация целевого приёма
            int targets = priorities.ApplicationPriorities.Count(x => x.EducationSourceId == 16 && x.CompetitiveGroupTargetId == null);
            if (targets > 0)
            {
                validationMessages["targetMessage"] = "Для целевого приёма не указана организация целевого приёма";
                return false;
            }
            #endregion

            return true;
        }


        // для фильтра "Уровень образования" на форме заявления
        // перечень значений в выпадающем списке "Уровень образования" должен зависеть от типа выбранной ПК
        // (см.таблицу соответствия EduLevelsToCampaignTypes)
        [Authorize]
        [HttpPost]
        public ActionResult GetAdmissionItemTypeByCampaign(int? campaignId)
        {
            if (!campaignId.HasValue)
                return new AjaxResultModel();
            return new AjaxResultModel { Data = new ApplicationRepository().GetAdmissionItemTypeByCampaign(campaignId.Value) };
        }

        // для заявления - шаг 1
        [Authorize]
        [HttpPost]
        public ActionResult GetCampaignById(int? campaignId)
        {
            if (!campaignId.HasValue)
                return new AjaxResultModel();
            return new AjaxResultModel { Data = new ApplicationRepository().GetCampaignById(campaignId.Value) };
        }

        // для заявления - шаг 1
        [Authorize]
        [HttpPost]
        public ActionResult CheckEntranceTestItemC(int? competitiveGroupId)
        {
            if (!competitiveGroupId.HasValue)
                return new AjaxResultModel();
            return new AjaxResultModel { Data = new ApplicationRepository().CheckEntranceTestItemC(competitiveGroupId.Value) };
        }
    }
}
