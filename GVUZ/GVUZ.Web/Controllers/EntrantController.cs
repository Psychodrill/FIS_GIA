using System;
using System.Reflection;
using System.Web.Mvc;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Filters;
using GVUZ.Web.Models;
using GVUZ.Web.Portlets;
using GVUZ.Web.Portlets.Entrants;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using GVUZ.Web.ViewModels.Entrants;
using log4net;

using System.Linq;
using System.Collections.Generic;

using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Model.Entrants.UniDocuments;
using GVUZ.DAL.Dapper.Repository.Model.Olympics;
using GVUZ.DAL.Dapper.Repository.Model;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Entrants")]
    public class EntrantController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Authorize]
        public ActionResult getViewDocument(int? EntrantDocumentID)
        {
            if (EntrantDocumentID == null) { EntrantDocumentID = 0; }
            if (EntrantDocumentID.Value == 0) { }
            if (EntrantDocumentID == 0)
            {
                return new AjaxResultModel("Ошибочный параметр");
            }
            try
            {
                UniDocumentViewModel udoc = new UniDocumentViewModel();
                udoc.UniD = EntrantSQL.getEntrantDocument(EntrantDocumentID, false, 0);
                udoc.ListIdentityDocumentType = ApplicationSQL.GetIdentityDocumentType();
                return PartialView("Entrant/EntrantDocumentView", udoc);
            }
            catch (Exception ex)
            {
                return new AjaxResultModel("Невозможно получить документ. " + ex.ToString());
            }
        }

        [Authorize]
        public ActionResult getEditDocument(int? EntrantDocumentID, int? DocTypeID = 0, int? EntrantID = null)
        {
            if (EntrantDocumentID == null) { EntrantDocumentID = 0; }
            if (EntrantDocumentID.Value == 0 && DocTypeID == 0) { }
            UniDocumentViewModel udoc = new UniDocumentViewModel();
            try
            {
                udoc.UniD = EntrantSQL.getEntrantDocument(EntrantDocumentID, true, DocTypeID.Value);
                if (udoc.UniD.EntrantID == 0)
                {
                    udoc.UniD.EntrantID = EntrantID.GetValueOrDefault();
                }
                udoc.MaxFileSize = GetMaxAllowedFileLength();
                udoc.ListIdentityDocumentType = ApplicationSQL.GetIdentityDocumentType();
                return PartialView("Entrant/EntrantDocumentEdit", udoc);
            }
            catch (Exception ex)
            {
                return new AjaxResultModel("Невозможно получить документ. " + ex.ToString());
            }
        }
        [Authorize]
        public ActionResult getEntrantDocument(int? EntrantDocumentID)
        {
            if (EntrantDocumentID == null) { return new AjaxResultModel("Ошибка параметра EntrantDocumentID"); }
            try
            {
                var d = EntrantSQL.getEntrantDocument(EntrantDocumentID.Value, false);
                return new AjaxResultModel { Data = d };
            }
            catch (Exception e)
            {
                return new AjaxResultModel("Внутреняя ошибка сервера. " + e.ToString());
            }
        }

        [Authorize]
        public ActionResult setEditDocument(EntrantDocumentViewModel doc)
        {
            try
            {
                //if(!ModelState.IsValid) { return new AjaxResultModel(ModelState); }

                if (String.IsNullOrEmpty(doc.DocumentNumber))
                {
                    while (true)
                    {
                        if (!String.IsNullOrEmpty(doc.Number))
                        {
                            doc.DocumentNumber = doc.Number;
                            break;
                        }
                        if (!String.IsNullOrEmpty(doc.CertificateNumber))
                        {
                            doc.DocumentNumber = doc.CertificateNumber;
                            break;
                        }
                        break;
                    }
                }
                if (String.IsNullOrEmpty(doc.DocumentOrganization))
                {
                    while (true)
                    {
                        if (doc.EntDocEdu != null)
                        {
                            doc.DocumentOrganization = doc.EntDocEdu.DocumentOU;
                        }
                        break;
                    }
                }
                if (doc.DocumentYear != null && doc.DocumentTypeID == 2)
                {
                    // ЕГЭ
                    doc.DocumentDate = new DateTime(doc.DocumentYear.Value, 1, 1);
                }
                if (!String.IsNullOrEmpty(doc.UID))
                {

                    if (EntrantSQL.checkDocumentUID(doc) > 0)
                    {
                        return new AjaxResultModel("Значение в поле Идентификатор (" + doc.UID + ") должно быть уникальным среди всех документов образовательной организации");
                    }
                }
                EntrantSQL.setEntrantDocument(doc);
                return new AjaxResultModel() { Data = new { id = doc.EntrantDocumentID } };
            }
            catch (Exception ex)
            {
                return new AjaxResultModel("Невозможно создать заявление " + ex.ToString());
            }
        }

        [Authorize]
        public ActionResult checkExistEntrantDocument(EntrantDocumentViewModel doc)
        {
            try
            {
                if (String.IsNullOrEmpty(doc.DocumentNumber))
                {
                    while (true)
                    {
                        if (!String.IsNullOrEmpty(doc.Number)) { doc.DocumentNumber = doc.Number; break; }
                        if (!String.IsNullOrEmpty(doc.CertificateNumber)) { doc.DocumentNumber = doc.CertificateNumber; break; }
                        break;
                    }
                }
                if (String.IsNullOrEmpty(doc.DocumentOrganization))
                {
                    while (true)
                    {
                        if (doc.EntDocEdu != null)
                        {
                            doc.DocumentOrganization = doc.EntDocEdu.DocumentOU;
                        }
                        break;
                    }
                }
                int id = 0;
                if (!String.IsNullOrEmpty(doc.DocumentNumber))
                {
                    id = EntrantSQL.checkExistEntrantDocument(doc);
                }
                return new AjaxResultModel() { Data = new { id = id } };
            }
            catch (Exception)
            {
                return new AjaxResultModel("Не удалось проверить уникальность нового документа!");
            }
        }

        private UserInfo GetUserInfo()
        {
            if (CustomParameters.ContainsKey("UserInfo")) { return (UserInfo)CustomParameters["UserInfo"]; }
            return UserInfo.Default;
        }

        [Authorize]
        public ActionResult getEntrantDocuments(int? EntrantID)
        {
            if (EntrantID == null) { return new AjaxResultModel("Ошибка параметра EntrantID"); }
            try
            {
                IEnumerable<EntrantDocumentModel> data = EntrantSQL.getEntrantIdentityDocuments(EntrantID.Value, false);
                return new AjaxResultModel { Data = data };
            }
            catch (Exception e)
            {
                return new AjaxResultModel("Внутреняя ошибка сервера. " + e.ToString());
            }
        }

        [Authorize]
        public ActionResult EntrantsList()
        {
            return RedirectToActionPermanent("ListEntrants");
            //using (var dbContext = new EntrantsEntities())
            //{
            //    return View(dbContext.GetEntrantsList(InstitutionID, new EntrantsListViewModel(), true));
            //}
        }

        [Authorize]
        public ActionResult EntrantInfo(int? entrantId)
        {
            if (entrantId.HasValue)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    return View(dbContext.GetEntrantInfo(entrantId.Value, InstitutionID));
                }
            }

            return new EmptyResult();
        }

        [Authorize, HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel SaveUid(int? entrantId, string uid)
        {
            if (!entrantId.HasValue)
                return GetDataError();
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.SaveEntrantUid(entrantId.Value, InstitutionID, uid);
            }
        }

        private AjaxResultModel GetDataError()
        {
            Log.Error(AjaxResultModel.DataError);
            return new AjaxResultModel(AjaxResultModel.DataError);
        }

        [Authorize]
        public AjaxResultModel GetEntrantsList(EntrantsListViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                model = dbContext.GetEntrantsList(InstitutionID, model, false);
                if (model.Filter != null && model.Filter.DateBegin.HasValue && model.Filter.DateEnd.HasValue && model.Filter.DateBegin.Value > model.Filter.DateEnd.Value)
                    return new AjaxResultModel().SetIsError("Filter_DateEnd", "Конечная дата должна быть меньше начальной даты");
                return new AjaxResultModel
                {
                    Data = model
                };
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsTabLink", MethodParams = new object[] { PortletType.PersonalRecordsDataTab, 0, "" })]
        [Authorize]
        public ActionResult EntrantViewTab0()
        {
            return Index();
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsTabLink", MethodParams = new object[] { PortletType.PersonalRecordsAddressTab, 1, "" })]
        [Authorize]
        public ActionResult EntrantViewTab1()
        {
            return Address();
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsTabLink", MethodParams = new object[] { PortletType.PersonalRecordsDocumentsTab, 2, "" })]
        [Authorize]
        public ActionResult EntrantViewTab2()
        {
            return DocumentsView();
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsTabLink", MethodParams = new object[] { PortletType.PersonalRecordsLanguageTab, 3, "" })]
        [Authorize]
        public ActionResult EntrantViewTab3()
        {
            return Languages();
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsTabLink", MethodParams = new object[] { PortletType.PersonalRecordsRequestTab, 4, "" })]
        [Authorize]
        public ActionResult EntrantViewTab4()
        {
            return ApplicationList();
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsDataLink")]
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                using (EntrantsEntities dbContext = new EntrantsEntities())
                {
                    return View("Index", dbContext.FillPersonalData(new PersonalRecordsDataViewModel { MaxFileSize = GetMaxAllowedFileLength() }, true, new EntrantKey(GetUserInfo())));
                }
            }
            catch (ArgumentException)
            {
                return Edit();
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsEditDataLink")]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult Edit()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("Edit", dbContext.FillPersonalData(new PersonalRecordsDataViewModel { MaxFileSize = GetMaxAllowedFileLength() }, false, new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        public ActionResult PersonalDataView()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/PersonalRecordsDataView", dbContext.FillPersonalData(new PersonalRecordsDataViewModel { MaxFileSize = GetMaxAllowedFileLength() }, true, new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult PersonalDataEdit()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/PersonalRecordsData", dbContext.FillPersonalData(new PersonalRecordsDataViewModel { MaxFileSize = GetMaxAllowedFileLength() }, false, new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        public ActionResult PersonalAddressView()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/PersonalRecordsAddressView", dbContext.FillPersonalAddress(new PersonalRecordsAddressViewModel(), true, new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult PersonalAddressEdit()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/PersonalRecordsAddress", dbContext.FillPersonalAddress(new PersonalRecordsAddressViewModel(), false, new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        public ActionResult PersonalDocumentsView()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/EntrantDocumentsListView", dbContext.FillDocumentList(new EntrantDocumentListViewModel(), new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult PersonalDocumentsEdit()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/EntrantDocumentsListEdit", dbContext.FillDocumentList(new EntrantDocumentListViewModel(), new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        public ActionResult PersonalLanguagesView()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/EntrantLanguageView", dbContext.FillLanguages(new EntrantLanguageViewModel { IsView = true }, new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult PersonalLanguagesEdit()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/EntrantLanguageEdit", dbContext.FillLanguages(new EntrantLanguageViewModel { IsView = false }, new EntrantKey(GetUserInfo())));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsAddressLink")]
        [Authorize]
        public ActionResult Address()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("Address", dbContext.FillPersonalAddress(new PersonalRecordsAddressViewModel(), true, new EntrantKey(GetUserInfo())));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsEditAddressLink")]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult AddressEdit()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("AddressEdit", dbContext.FillPersonalAddress(new PersonalRecordsAddressViewModel(), false, new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        public ActionResult Documents()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("DocumentList", dbContext.FillDocumentList(new EntrantDocumentListViewModel(), new EntrantKey(GetUserInfo())));
            }
        }

        [Authorize]
        public ActionResult DocumentsView()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("DocumentListView", dbContext.FillDocumentList(new EntrantDocumentListViewModel(), new EntrantKey(GetUserInfo())));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsLanguageLink")]
        [Authorize]
        public ActionResult Languages()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("Language", dbContext.FillLanguages(new EntrantLanguageViewModel { IsView = true }, new EntrantKey(GetUserInfo())));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "PersonalRecordsEditLanguageLink")]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult LanguagesEdit()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("LanguageEdit", dbContext.FillLanguages(new EntrantLanguageViewModel(), new EntrantKey(GetUserInfo())));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationListLink")]
        [Authorize]
        public ActionResult ApplicationList()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("ApplicationList", dbContext.FillApplicationList(new ApplicationListViewModel(), GetUserInfo()));
                //return View("ApplicationList", dbContext.FillApplicationList(new ApplicationListViewModel(), new UserInfo(){SNILS = "123-123-123 12"}));
            }
        }

        [GeneratorPortletLink(typeof(PortletLinkHelper), "ApplicationGoBack")]
        [Authorize]
        public ActionResult ApplicationGoBack()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return View("ApplicationList", dbContext.FillApplicationList(new ApplicationListViewModel(), GetUserInfo()));
            }
        }

        [Authorize]
        public ActionResult ApplicationListControl()
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return PartialView("Portlets/Entrants/EntrantApplicationList", dbContext.FillApplicationList(new ApplicationListViewModel(), GetUserInfo()));
            }
        }

        [PortletAjaxLink(PortletType.SavePersonalData)]
        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SavePersonalData(string model)
        {
            ValueProvider = JsonRequestWrapper.GetValueProvider(model);
            PersonalRecordsDataViewModel modelT = new PersonalRecordsDataViewModel();
            RefreshModel(modelT);
            if (modelT.ApplicationStep == ApplicationStepType.ParentData) //сведения о родителях, не вводим данные про абитуриента
            {
                foreach (string key in ModelState.Keys)
                {
                    if (!key.StartsWith("Father.") && !key.StartsWith("Mother.")) ModelState[key].Errors.Clear();
                    if (key.StartsWith("ApplicationNumber") || key.StartsWith("RegistrationDate")) ModelState[key].Errors.Clear();
                }
            }
            //если личные данные или на ПГУ (есть юзеринфо) не требуем заявления
            if (modelT.ApplicationStep == ApplicationStepType.NotApplication || GetUserInfo().SNILS != null)
            {
                foreach (string key in ModelState.Keys) //выносим ошибки от заявления
                    if (key.StartsWith("ApplicationNumber") || key.StartsWith("RegistrationDate")) ModelState[key].Errors.Clear();
            }

            if (!(modelT.ActionCommand == "back" || modelT.ActionCommand == "save")) //в случае back и save не валидируем модель на правильность
            {
                if (!ModelState.IsValid)
                {
                    //если не ввели ничего про родителя - не валидируем
                    if (modelT.Father.IsAllFieldsEmpty())
                        foreach (string key in ModelState.Keys)
                            if (key.StartsWith("Father.")) ModelState[key].Errors.Clear();
                    if (modelT.Mother.IsAllFieldsEmpty())
                        foreach (string key in ModelState.Keys)
                            if (key.StartsWith("Mother.")) ModelState[key].Errors.Clear();
                    if (!ModelState.IsValid)
                        return new AjaxResultModel(ModelState);
                }

                if (IdentityDocumentViewModel.IsSeriesRequired(modelT.DocumentTypeID) && String.IsNullOrEmpty(modelT.DocumentSeries))
                {
                    ModelState.AddModelError("DocumentSeries", "");
                    ModelState.AddModelError("DocumentNumber", "Неверная серия документа");
                    return new AjaxResultModel(ModelState);
                }
            }

            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.SavePersonalData(modelT, new EntrantKey(modelT.ApplicationID, GetUserInfo()));
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.AddDocument)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult AddDocument(int? entrantID, int? documentTypeID, int? competitiveGroupId, int? subjectId)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                if (entrantID.HasValue && documentTypeID.HasValue)
                    return dbContext.LoadEntrantDocumentByCompetitiveGroup(this, entrantID.Value, documentTypeID.Value, competitiveGroupId, subjectId);
                return new EmptyResult();
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.EditDocument)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult EditDocument(int? entrantDocumentID)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                if (entrantDocumentID.HasValue)
                    return dbContext.LoadEntrantDocument(this, entrantDocumentID.Value, false, InstitutionID);
                return new EmptyResult();
            }
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult FindSpecialityByQualification(string qualification)
        {
            object[] items = null;
            if (!string.IsNullOrWhiteSpace(qualification))
            {
                using (EntrantsEntities dbContext = new EntrantsEntities())
                {
                    items = dbContext.FindSpecialityByQualification(qualification).ToArray();
                }
            }

            return Json(items);
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.ViewDocument)]
        public ActionResult ViewDocument(int entrantDocumentID)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.LoadEntrantDocument(this, entrantDocumentID, true, InstitutionID);
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.DeleteDocument)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult DeleteDocument(int? entrantDocumentID)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                if (entrantDocumentID.HasValue)
                    return dbContext.DeleteEntrantDocument(entrantDocumentID.Value, GetUserInfo());
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Сохранение документа абитуриента
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [PortletAjaxLink(PortletType.SaveDocumentAuto)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SaveDocumentAuto(string model)
        {
            // так как MVC заранее не знает тип модели, мы её передаём строкой
            ValueProvider = JsonRequestWrapper.GetValueProvider(model);

            //получаем нужный id типа дкоумента
            int docType = ValueProvider.GetValue("DocumentTypeID").RawValue.To(0);
            if (docType == 0)
                return new AjaxResultModel("Не найден документ");
            //создаём модельнужного типа
            BaseDocumentViewModel modelT = EntrantDocumentExtensions.InstantiateDocumentByType(docType);
            // делаем правильную модель и вызываем метод сохранения с правильно десериализованной моделью
            MethodInfo methodInfo = GetType().GetMethod("RefreshModel").MakeGenericMethod(modelT.GetType());
            modelT = (BaseDocumentViewModel)methodInfo.Invoke(this, new object[] { modelT });
            return SaveDocument(modelT, GetUserInfo());
        }

        private ActionResult SaveDocument(BaseDocumentViewModel model, UserInfo userInfo)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            model.Validate(ModelState, InstitutionID);
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            string errorString = model.Validate();
            if (!String.IsNullOrEmpty(errorString))
                return new AjaxResultModel(errorString);
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.SaveEntrantDocument(model, userInfo.SNILS);
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.SavePersonalAddress)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SavePersonalAddress(string model)
        {
            ValueProvider = JsonRequestWrapper.GetValueProvider(model);
            PersonalRecordsAddressViewModel modelT = new PersonalRecordsAddressViewModel();
            RefreshModel(modelT);

            if (!ModelState.IsValid)
            {
                //если фактический адрес совпадает с пропиской - не валидируем
                //для региона валидируем либо ID либо Name
                foreach (string key in ModelState.Keys)
                {
                    if (modelT.IsOnlyRegistration)
                        if (key.StartsWith("FactAddress.")) ModelState[key].Errors.Clear();
                    if (modelT.RegistrationAddress.CountryHasRegions == 1)
                    {
                        if (key.Equals("RegistrationAddress.RegionName")) ModelState[key].Errors.Clear();
                    }
                    else
                    {
                        if (key.Equals("RegistrationAddress.RegionID")) ModelState[key].Errors.Clear();
                    }

                    if (modelT.FactAddress.CountryHasRegions == 1)
                    {
                        if (key.Equals("FactAddress.RegionName")) ModelState[key].Errors.Clear();
                    }
                    else
                    {
                        if (key.Equals("FactAddress.RegionID")) ModelState[key].Errors.Clear();
                    }
                }

                if (!ModelState.IsValid && !(modelT.ActionCommand == "back" || modelT.ActionCommand == "save"))
                    return new AjaxResultModel(ModelState);
            }

            //теперь все поля необязательные
            // GVUZ-539 Сделать необязательным населенный пункт в случае с регионом "г. Москва"
            /*if (ModelState["RegistrationAddress.RegionID"].Value.AttemptedValue != "1"
				&& String.IsNullOrEmpty(ModelState["RegistrationAddress.CityName"].Value.AttemptedValue))
			{
				ModelState.AddModelError("RegistrationAddress.CityName", "Поле \"Населенный пункт\" обязательно для заполнения");
				return new AjaxResultModel(ModelState);
			}
			if (ModelState["FactAddress.RegionID"].Value.AttemptedValue != "1"
				&& String.IsNullOrEmpty(ModelState["FactAddress.CityName"].Value.AttemptedValue))
			{
				ModelState.AddModelError("FactAddress.CityName", "Поле \"Населенный пункт\" обязательно для заполнения");
				return new AjaxResultModel(ModelState);
			}*/

            //using (EntrantsEntities dbContext = new EntrantsEntities())
            //{
            //    return dbContext.SavePersonalAddress(modelT, GetUserInfo());
            //}
            return new AjaxResultModel(ModelState);
        }

        [PortletAjaxLink(PortletType.ReceiveFile)]
        [Authorize]
        public new ActionResult ReceiveFile()
        {
            return base.ReceiveFile();
        }

        [PortletAjaxLink(PortletType.ReceiveFile)]
        [Authorize]
        public new ActionResult ReceiveFile1()
        {
            return base.ReceiveFile1();
        }


        [HttpPost]
        [PortletAjaxLink(PortletType.RegionsList)]
        public ActionResult RegionsList(int? countryID)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel { Data = dbContext.GetRegions(countryID.To(1)).ToArray() };
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.ChangeRegion)]
        public ActionResult ChangeRegion(int? regionID)
        {
            return new AjaxResultModel();
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.SaveEntrantLanguages)]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult SaveEntrantLanguages(string model)
        {
            ValueProvider = JsonRequestWrapper.GetValueProvider(model);
            EntrantLanguageViewModel modelT = RefreshModel(new EntrantLanguageViewModel());
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.SaveLanguages(modelT, GetUserInfo());
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.CitiesList)]
        public ActionResult CitiesList(int? regionId, string namePart)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return Content(String.Join(",", dbContext.GetCities(regionId.To(1), namePart).ToArray()));
            }
        }

        [HttpPost]
        [PortletAjaxLink(PortletType.CitiesList2)]
        public ActionResult CitiesList2(int? regionId, string namePart)
        {
            return CitiesList(regionId, namePart);
        }

        public ActionResult CitiesListLocal(int? regionId, string term)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return Json(dbContext.GetCities(regionId.To(1), term).ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CitiesList2Local(int? regionId, string term)
        {
            return CitiesListLocal(regionId, term);
        }

        [HttpPost]
        public ActionResult CheckDocumentOnExists(EntrantDocumentCheckOnExistsViewModel model)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                return dbContext.CheckDocumentOnExists(model);
            }
        }

        public AjaxResultModel GetCGroupList(string campaignName)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                try
                {
                    if (String.IsNullOrEmpty(campaignName))
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

        #region Список и карточка абитуриента
        [Authorize]
        public ActionResult ListEntrants()
        {
            var model = new EntrantRecordListViewModel
            {
                Filter = EntrantListSQL.GetEntrantListFilter(InstitutionID)
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult LoadEntrantListRecords(EntrantRecordListQueryViewModel query)
        {
            return Json(EntrantListSQL.GetEntrantListRecords(InstitutionID, query));
        }

        [Authorize]
        [HttpPost]
        public ActionResult EntrantDetails(int? id)
        {
            var model = EntrantListSQL.GetEntrantDetails(InstitutionID, id.GetValueOrDefault());

            if (model == null)
            {
                return HttpNotFound();
            }

            return Json(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateUID(UpdateEntrantUidViewModel updateModel)
        {
            string validationError = null;

            if (ModelState.IsValid && EntrantListSQL.UpdateEntrantUid(InstitutionID, updateModel.EntrantId.GetValueOrDefault(), updateModel.UID, out validationError))
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = validationError });
        }
        #endregion


        //----------------------------------------------------------------------------------------------------------
        // этот метод вызывается из карточки документа участника олимпиады,
        // когда пользователь щелкает комбобокс с наименованиями олимпиад
        // нужно вытащить из базы год, профили по этой олимпиаде 
        //----------------------------------------------------------------------------------------------------------

        [Authorize]
        public ActionResult GetOlympicData(int? OlympicID)
        {
            if (OlympicID == null)
                return new AjaxResultModel("Ошибка параметра OlympicID");

            try
            {
                // репозиторий для работы с олимпиадами
                var repository = new OlympicsRepository();

                // берем OlympicType, внутри коллекция профилей OlympicTypeProfile
                var olympicType = repository.GetOlympicTypeById((int)OlympicID);

                return new AjaxResultModel
                {
                    // Data = EntrantSQL.getOlympicData(OlympicID.Value)
                    Data = olympicType
                };

            }
            catch (Exception e)
            {
                return new AjaxResultModel("Внутреняя ошибка сервера. " + e.ToString());
            }
        }

        //----------------------------------------------------------------------------------------------------------

        [Authorize]
        public ActionResult GetOlympicProfileDetails(int? OlympicTypeProfileID)
        {
            if (OlympicTypeProfileID == null)
                return new AjaxResultModel("Ошибка параметра OlympicTypeProfileID");
             
            try
            {
                // репозиторий для работы с олимпиадами
                var repository = new OlympicsRepository();

                var olympicTypeProfile = repository.GetOlympicLevel(OlympicTypeProfileID.Value);
                var olympicTypeProfileSubjects = repository.GetSubjectsByOlympicTypeProfile(OlympicTypeProfileID.Value);
                return new AjaxResultModel
                {
                    Data = new
                    { 
                        OlympicTypeProfile = olympicTypeProfile,
                        OlympicTypeProfileSubjects = olympicTypeProfileSubjects 
                    }
                };

            }
            catch (Exception e)
            {
                return new AjaxResultModel("Внутреняя ошибка сервера. " + e.ToString());
            }
        }

        [Authorize]
        public ActionResult CheckBenefit(CheckBenefitModel checkBenefit)
        {
            try
            {
                int[] needsOlympicCheck = { 9, 10 };

                if (needsOlympicCheck.Contains(checkBenefit.DocumentTypeId.Value))
                {
                    var repository = new OlympicsRepository();

                    bool docUsedAsBenefit;
                    var olympicCheckResult = repository.CheckBenefitOlympic(checkBenefit.EntranceTestItemId, checkBenefit.CompetitiveGroupId,
                        checkBenefit.CheckBenefitOlympicModel.OlympicTypeProfileId, checkBenefit.CheckBenefitOlympicModel.DiplomaTypeId, checkBenefit.CheckBenefitOlympicModel.OlympicId, checkBenefit.CheckBenefitOlympicModel.ClassNumber, checkBenefit.EntrantDocumentId, out docUsedAsBenefit);

                    if ((olympicCheckResult.violationId == 0) && (docUsedAsBenefit))
                    {
                        olympicCheckResult.violationId = 21;
                    }
                    if ((olympicCheckResult.violationId > 0) || (docUsedAsBenefit))
                        return new AjaxResultModel { Data = olympicCheckResult };
                }

                return new AjaxResultModel() { Data = new { violationId = 0 } };
            }
            catch (Exception e)
            {
                return new AjaxResultModel { Data = new { errorMessage = "Внутреняя ошибка сервера. " + e.ToString() } };
            }
        }
    }
}
