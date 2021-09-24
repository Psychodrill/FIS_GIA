using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GVUZ.Helper;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;

namespace GVUZ.Model.Entrants.ContextExtensions
{
    public static class EntrantDocumentExtensions
    {
        public static EntrantDocumentListViewModel FillDocumentList(this EntrantsEntities dbContext,
                                                                    EntrantDocumentListViewModel model, EntrantKey key)
        {
            int entrantID = key.GetEntrantID(dbContext, false);
            DocumentShortInfoViewModel[] q = dbContext.EntrantDocument
                                                      .Include(x => x.DocumentType)
                                                      .Include(x => x.Attachment)
                                                      .Where(x => x.EntrantID == entrantID)
                                                      .OrderBy(x => x.DocumentTypeID)
                                                      .Select(x => new DocumentShortInfoViewModel
                                                          {
                                                              DocumentAttachmentID =
                                                                  x.AttachmentID == null
                                                                      ? Guid.Empty
                                                                      : new Guid(x.Attachment.FileID == null ? Guid.Empty.ToString() : x.Attachment.FileID.ToString()),
                                                              DocumentAttachmentName =
                                                                  x.AttachmentID == null ? null : x.Attachment.Name,
                                                              DocDate = x.DocumentDate,
                                                              DocNumber = x.DocumentNumber,
                                                              DocTypeID = x.DocumentTypeID,
                                                              DocSpecificData = x.DocumentSpecificData,
                                                              DocumentTypeName = x.DocumentType.Name,
                                                              DocumentOrganization = x.DocumentOrganization,
                                                              DocSeries = x.DocumentSeries,
                                                              EntrantDocumentID = x.EntrantDocumentID,
                                                              CanBeModified =
                                                                  !x.ApplicationEntranceTestDocument.Any() &&
                                                                  /*x.Entrant_IdentityDocument.Count() == 0 &&*/
                                                                  !x.ApplicationEntrantDocument.Any()
                                                          })
                                                      .ToArray();
            foreach (DocumentShortInfoViewModel doc in q)
                doc.FillData();
            model.EntrantID = entrantID;
            model.Documents = q;
            model.DocumentTypes = 
                    DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DocumentType)                
                                           .Select(x => new EntrantDocumentListViewModel.DocumentType
                                               {
                                                   TypeID = x.Key,
                                                   Name = x.Value
                                               }).OrderBy(x => x.Name).ToArray();
            return model;
        }

        public static BaseDocumentViewModel InstantiateDocumentByType(int documentTypeID)
        {
            var model = Activator.CreateInstance(GetDocumentViewModelType(documentTypeID)) as BaseDocumentViewModel;
            if (model != null)
                model.DocumentTypeID = documentTypeID;

            return model;
        }

        public static Type GetDocumentViewModelType(int documentTypeID)
        {
            switch (documentTypeID)
            {
                case 1:
                    return typeof (IdentityDocumentViewModel);
                case 2:
                    return typeof (EGEDocumentViewModel);
                case 3:
                case 16:
                    return typeof (SchoolCertificateDocumentViewModel);
                case 4:
                case 5:
                case 7:
                case 8:
                case 25:
                case 26:
                    return typeof (DiplomaDocumentViewModel);
                case 6:
                    return typeof (BasicDiplomaDocumentViewModel);
                case 9:
                    return typeof (OlympicDocumentViewModel);
                case 10:
                    return typeof (OlympicTotalDocumentViewModel);
                case 11:
                    return typeof (DisabilityDocumentViewModel);
                case 12:
                case 13:
                    return typeof (PsychoDocumentViewModel);
                case 14:
                    return typeof (MilitaryCardDocumentViewModel);
                case 15:
                    return typeof (CustomDocumentViewModel);
                case 17:
                    return typeof (GiaDocumentViewModel);
                case 18:
                    return typeof (StudentDocumentViewModel);
                case 19:
                    return typeof (EduCustomDocumentViewModel);
                default:
                    return typeof (CustomDocumentViewModel);
            }
        }

        public static ActionResult LoadEntrantDocumentByCompetitiveGroup(this EntrantsEntities dbContext, Controller controller,
            int entrantID, int documentTypeID, int? competitiveGroupId, int? subjectId)
        {
            BaseDocumentViewModel model = InstantiateDocumentByType(documentTypeID);
            model.EntrantID = entrantID;

            return FillEntrantDocumentViewData(dbContext, model, controller, false, competitiveGroupId, subjectId);
        }

        public static void ConvertDatesToLocal(object mainObject)
        {
            if (mainObject != null)
            {
                foreach (
                    PropertyInfo propertyInfo in
                        mainObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (propertyInfo.PropertyType == typeof (DateTime))
                    {
                        var dt = (DateTime) propertyInfo.GetValue(mainObject, null);
                        dt = dt.ToLocalTime();
                        propertyInfo.SetValue(mainObject, dt, null);
                    }

                    if (propertyInfo.PropertyType == typeof (DateTime?))
                    {
                        var dt = (DateTime?) propertyInfo.GetValue(mainObject, null);
                        if (dt.HasValue)
                            dt = dt.Value.ToLocalTime();
                        propertyInfo.SetValue(mainObject, dt, null);
                    }
                }
            }
        }

        public static IEnumerable<BaseDocumentViewModel> LoadApplicationDocuments(this EntrantsEntities dbContext,
                                                                                  int applicationID)
        {
            var resultList = new List<BaseDocumentViewModel>();

            List<EntrantDocument> docs = dbContext.ApplicationEntrantDocument
                                                  .Where(x => x.ApplicationID == applicationID)
                                                  .Select(x => x.EntrantDocument).Include(x => x.Attachment).ToList();

            foreach (EntrantDocument doc in docs)
            {
                var model = (BaseDocumentViewModel)
                            new JavaScriptSerializer().Deserialize(doc.DocumentSpecificData,
                                                                   InstantiateDocumentByType(doc.DocumentTypeID)
                                                                       .GetType());
                ConvertDatesToLocal(model);
                model.EntrantID = doc.EntrantID ?? default(int);
                model.EntrantDocumentID = doc.EntrantDocumentID;
                if (doc.Attachment != null)
                {
                    model.DocumentAttachmentID = doc.Attachment.FileID ?? default(Guid);
                    model.DocumentAttachmentName = doc.Attachment.Name;
                }

                model.DocumentTypeName =
                    dbContext.DocumentType.Where(x => x.DocumentID == model.DocumentTypeID).Select(x => x.Name).First();

                resultList.Add(model);
            }

            return resultList;
        }

        public static BaseDocumentViewModel LoadEntrantDocument(this EntrantsEntities dbContext, int entrantDocumentID,
                                                                bool withAttachment = true)
        {
            IQueryable<EntrantDocument> q =
                dbContext.EntrantDocument.Where(x => x.EntrantDocumentID == entrantDocumentID);
            if (withAttachment)
                q = q.Include(x => x.Attachment);
            EntrantDocument doc = q.FirstOrDefault();
            return GetEntrantDocumentViewModel(doc);
        }

        public static BaseDocumentViewModel GetEntrantDocumentViewModel(EntrantDocument doc, bool withAttachment = true)
        {
            if (doc == null)
                return null;
            var model = (BaseDocumentViewModel) new JavaScriptSerializer().Deserialize(doc.DocumentSpecificData,
                                                                                       InstantiateDocumentByType(
                                                                                           doc.DocumentTypeID).GetType());
            ConvertDatesToLocal(model);
            model.EntrantID = doc.EntrantID ?? default(int);
            model.EntrantDocumentID = doc.EntrantDocumentID;
            if (withAttachment && doc.Attachment != null)
            {
                model.DocumentAttachmentID = doc.Attachment.FileID ?? default(Guid);
                model.DocumentAttachmentName = doc.Attachment.Name;
            }

            model.DocumentTypeName = DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.DocumentType,
                                                             model.DocumentTypeID);
            return model;
        }

        public static ActionResult LoadEntrantDocument(this EntrantsEntities dbContext, Controller controller,
                int entrantDocumentID, bool isView, int institutionID)
        {
            EntrantDocument doc = dbContext.EntrantDocument
                                           .Where(
                                               x =>
                                               x.EntrantDocumentID == entrantDocumentID /*&&
                                               x.Entrant.InstitutionID == institutionID
                 && x.Entrant.SNILS == userInfo.SNILS*/)
                                           .Include(x => x.Attachment)
                                           .Include(x => x.Entrant).FirstOrDefault();
            if (doc == null)
                return new EmptyResult();
            var model = (BaseDocumentViewModel)
                        new JavaScriptSerializer().Deserialize(doc.DocumentSpecificData,
                                                               InstantiateDocumentByType(
                                                                   doc.DocumentTypeID).GetType());
            ConvertDatesToLocal(model);
            model.EntrantID = doc.EntrantID ?? default(int);
            model.EntrantDocumentID = entrantDocumentID;
            if (doc.Attachment != null)
            {
                model.DocumentAttachmentID = doc.Attachment.FileID ?? default(Guid);
                model.DocumentAttachmentName = doc.Attachment.Name;
            }

            if (model.DocumentTypeID == 1) //identity
                dbContext.AddDocumentAccessToLog(doc.Entrant, "ViewIdentityDocument", model.EntrantID, institutionID);
            return FillEntrantDocumentViewData(dbContext, model, controller, isView, null, null);
        }

        private static ActionResult FillEntrantDocumentViewData(EntrantsEntities dbContext, BaseDocumentViewModel model,
             Controller controller, bool isView, int? competitiveGroupId, int? subjectId)
        {
            model.DocumentTypeName =   DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DocumentType)
                                           .Select(x => new EntrantDocumentListViewModel.DocumentType
                                           {
                                               TypeID = x.Key,
                                               Name = x.Value
                                           }).Where(x => x.TypeID == model.DocumentTypeID).Select(x => x.Name).First();
            model.FillData(dbContext, isView, competitiveGroupId, subjectId);
            controller.ViewData.Model = model;

            return new PartialViewResult
                {
                    ViewName =
                        "Portlets/Entrants/Documents/" + model.GetType().Name.Replace("ViewModel", "") +
                        (isView ? "View" : "Edit"),
                    ViewData = controller.ViewData,
                    TempData = controller.TempData
                };
        }

        public static string GetSubjectName(this EntrantsEntities dbContext, ApplicationEntranceTestDocument document)
        {
            if (dbContext == null) throw new ArgumentNullException("dbContext");
            if (document == null) throw new ArgumentNullException("document");

            if (document.Subject != null && !string.IsNullOrEmpty(document.Subject.Name))
                return document.Subject.Name;
            return DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).First(x => x.Key == document.SubjectID).Value;
        }

        public static AjaxResultModel SaveEntrantDocument(this EntrantsEntities dbContext, BaseDocumentViewModel model,
                                                          string snils)
        {
            model.PrepareForSave(dbContext);
            //IDocumentFields fields = model as IDocumentFields;
            EntrantDocument doc;
            bool isEdit = model.EntrantDocumentID > 0;
            if (isEdit)
            {
                doc = dbContext.EntrantDocument
                               .Where(
                                   x =>
                                   x.EntrantDocumentID == model.EntrantDocumentID &&
                                   (x.Entrant.SNILS == snils || snils == null))
                               .Include(x => x.Attachment)
                               .Include(x => x.Entrant).FirstOrDefault();
                if (doc == null)
                    return new AjaxResultModel("Не найден документ");
            }
            else doc = new EntrantDocument();

            if (isEdit && model.DocumentTypeID != doc.DocumentTypeID)
                return new AjaxResultModel("Не найден документ");

            if (isEdit && doc.Attachment != null && doc.Attachment.FileID != model.DocumentAttachmentID)
                dbContext.Attachment.DeleteObject(doc.Attachment);

            if (model.DocumentAttachmentID != Guid.Empty)
                doc.Attachment = dbContext.Attachment.First(x => x.FileID == model.DocumentAttachmentID);
            doc.DocumentDate = model.DocumentDate;
            doc.DocumentNumber = model.DocumentNumber;
            doc.DocumentOrganization = model.DocumentOrganization;
            doc.DocumentSeries = model.DocumentSeries;
            doc.DocumentType = dbContext.DocumentType.First(x => x.DocumentID == model.DocumentTypeID);
            doc.Entrant =
                dbContext.Entrant.First(x => x.EntrantID == model.EntrantID && (x.SNILS == snils || snils == null));
            doc.UID = model.UID;
            doc.DocumentSpecificData = new JavaScriptSerializer().Serialize(model);
            if (!isEdit)
                dbContext.EntrantDocument.AddObject(doc);

            if (!isEdit)
            {
                if (model is IdentityDocumentViewModel)
                {
                    doc.Entrant.EntrantDocument_Identity = doc;
                    //можем редактировать поданные заявления и как результат изменить документ для абитуриента
                    if (model.ApplicationID > 0)
                    {
                        Entrant appEntrant =
                            dbContext.Application.Where(x => x.EntrantID == model.EntrantID)
                                     .Select(x => x.Entrant)
                                     .FirstOrDefault();
                        if (appEntrant != null)
                            appEntrant.EntrantDocument_Identity = doc;
                    }
                }
            }

            dbContext.SaveChanges();
            model.EntrantDocumentID = doc.EntrantDocumentID;
            model.SaveToAdditionalTable(dbContext);
            if (doc.Attachment != null)
                model.DocumentAttachmentName = doc.Attachment.Name;
            model.DocumentTypeName = doc.DocumentType.Name;

            if (model is IdentityDocumentViewModel)
                dbContext.AddDocumentAccessToLog(doc.Entrant, doc.Entrant, "SaveIdentityDocument", model.EntrantID,
                                                 doc.Entrant != null ? doc.Entrant.InstitutionID : null);
            return new AjaxResultModel
                {
                    Data = new DocumentShortInfoViewModel(model)
                        {
                            CanBeModified = true,
                            ShowWarnBeforeModifying =
                                dbContext.ApplicationEntrantDocument.Any(y => y.ApplicationID != model.ApplicationID),
                            CanNotSetReceived = model.DocumentTypeID == 1, //ДУЛ
                            CanBeDetached = (!(model is IdentityDocumentViewModel))
                            //,CanBeDeleted = (!(model is IdentityDocumentViewModel))
                    }
            };
        }

        public static EntrantDocument CreateEntrantDocument(this EntrantsEntities dbContext, BaseDocumentViewModel model, Entrant entrant)
        {
            EntrantDocument doc = new EntrantDocument();

            doc.DocumentNumber = model.DocumentNumber;
            doc.DocumentSeries = model.DocumentSeries;
            doc.DocumentTypeID = model.DocumentTypeID;
            doc.Entrant = entrant;
            
            doc.DocumentSpecificData = new JavaScriptSerializer().Serialize(model);

            dbContext.EntrantDocument.AddObject(doc);

            dbContext.SaveChanges();
            model.EntrantDocumentID = doc.EntrantDocumentID;
            model.SaveToAdditionalTable(dbContext);

            if (model is IdentityDocumentViewModel)
                dbContext.AddDocumentAccessToLog(doc.Entrant, doc.Entrant, "SaveIdentityDocument", model.EntrantID, doc.Entrant != null ? doc.Entrant.InstitutionID : null);
            return doc;
        }

        public static ActionResult DeleteEntrantDocument(this EntrantsEntities dbContext, int entrantDocumentID, UserInfo userInfo){
            EntrantDocument doc = dbContext.EntrantDocument
                                           .Where( x => x.EntrantDocumentID == entrantDocumentID && (x.Entrant.SNILS == userInfo.SNILS || userInfo.SNILS == null))
                                           .Include(x => x.Attachment)
                                           .Include(x => x.Entrant).FirstOrDefault();
            if (doc == null)
                return new AjaxResultModel("Не найден документ");
            try
            {
                BaseDocumentViewModel model = null;
                if (doc.DocumentTypeID == 1) //identity
                {
                    model = LoadEntrantDocument(dbContext, entrantDocumentID);
                }

                if (doc.Attachment != null)
                    dbContext.Attachment.DeleteObject(doc.Attachment);
                dbContext.EntrantDocument.DeleteObject(doc);
                dbContext.SaveChanges();
                if (doc.Entrant != null)
                    dbContext.AddDocumentAccessToLog(doc.Entrant, null, "DeleteIdentityDocument", doc.EntrantID ?? default(int),
                                                     doc.Entrant.InstitutionID);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException as SqlException;
                if (inner != null)
                    return new AjaxResultModel("Невозможно удалить документ, т.к. он уже используется");
                throw;
            }

            return new AjaxResultModel();
        }

        public static ActionResult CheckDocumentOnExists(this EntrantsEntities dbContext, EntrantDocumentCheckOnExistsViewModel model)
        {
            var existingDocument = dbContext.EntrantDocument.Where(x => x.EntrantID == model.EntrantID
                    && x.DocumentTypeID == model.DocumentTypeID
						  && (x.DocumentSeries == model.DocumentSeries || (x.DocumentSeries == null && model.DocumentSeries == null))
                    && (x.DocumentNumber == model.DocumentNumber || (x.DocumentNumber == null && model.DocumentNumber == null))
                    && x.EntrantDocumentID != model.EntrantDocumentID)
                .Select(x => new { x.EntrantDocumentID,
                    CanBeModified = !x.ApplicationEntranceTestDocument.Any(y => y.ApplicationID == model.ApplicationID)
                                   &&x.ApplicationEntrantDocument.All(y => y.ApplicationID == model.ApplicationID)
                }).FirstOrDefault();
            if (existingDocument != null){
                return new AjaxResultModel{ Data = new {IsFound = existingDocument.EntrantDocumentID, existingDocument.CanBeModified} };
            }
            return new AjaxResultModel {Data = new {IsFound = 0}};
        }


        public static string[] CheckGlobalBenefitsEGEMinValues(this EntrantsEntities dbContext, int appID)
        {
            List<string> errors = new List<string>();

            var app = dbContext.Application.FirstOrDefault(x => x.ApplicationID == appID);

            // Список всех документов по общим льготам
            var applicationTestDocuments = app.ApplicationEntranceTestDocument.Where(x => x.BenefitID == 1).ToList();

            foreach (var doc in applicationTestDocuments)
            {
                if (!doc.EntrantDocumentID.HasValue)
                    continue;

                //Получим документ на льготу
                BaseDocumentViewModel baseDoc = dbContext.LoadEntrantDocument(doc.EntrantDocumentID.Value);
                baseDoc.FillData(dbContext, true, null, null);

                OlympicDocumentViewModel olympDoc = baseDoc as OlympicDocumentViewModel;

                if (olympDoc == null)
                    continue;

                var groupBenefits = dbContext.BenefitItemC.Where(x => x.CompetitiveGroupID == doc.CompetitiveGroupID && !x.EntranceTestItemID.HasValue).ToList();

                BenefitItemSubject[] subjects = null;

                if (groupBenefits.Count == 0)
                    continue;

                var olympicsBenefitItemIds = dbContext.BenefitItemCOlympicType.Where(x => x.OlympicTypeID == olympDoc.OlympicID).Select(x => x.BenefitItemID);

                var AllEgeResults = dbContext.ApplicationEntranceTestDocument.Where(x => x.ApplicationID == app.ApplicationID)
                    .Select(x => new
                    {
                        egeResult = x.ResultValue,
                        SubjectID = x.SubjectID,
                        ItemId = x.EntranceTestItemID,
                        SubjectName = x.Subject.Name
                    }).ToList();

                //Выберем все ВИ для данной КГ.
                var items = dbContext.EntranceTestItemC.Where(x => x.CompetitiveGroupID == doc.CompetitiveGroupID).Select(x => x.EntranceTestItemID).ToArray();

                // Отсечём те результаты ЕГЭ, что не относятся к данной КГ
                var EgeResults = AllEgeResults.Where(x => x.ItemId.HasValue && items.Contains(x.ItemId.Value)).ToList();

                if (olympicsBenefitItemIds.Count() != 0) // Что-то есть, можно брать данные
                {
                    var benefitItemToUse = groupBenefits.FirstOrDefault(x => olympicsBenefitItemIds.Contains(x.BenefitItemID));
                    if (benefitItemToUse != null)
                    {
                        // Нашли и будем его использовать для получения баллов
                        subjects = dbContext.BenefitItemSubject.Where(x => x.BenefitItemId == benefitItemToUse.BenefitItemID).ToArray();
                    }
                    else
                    {
                        // Для конкретной олимпиады данных нет - ищем для всех
                        var commonBenefit = groupBenefits.FirstOrDefault(x => x.IsForAllOlympic);
                        if (commonBenefit != null) // Есть льгота для всех олимпиад
                        {
                            subjects = dbContext.BenefitItemSubject.Where(x => x.BenefitItemId == commonBenefit.BenefitItemID).ToArray();
                        }
                    }
                }
                else
                {
                    // Для конкретной олимпиады данных нет - ищем для всех
                    var commonBenefit = groupBenefits.FirstOrDefault(x => x.IsForAllOlympic);
                    if (commonBenefit != null) // Есть льгота для всех олимпиад
                    {
                        subjects = dbContext.BenefitItemSubject.Where(x => x.BenefitItemId == commonBenefit.BenefitItemID).ToArray();
                    }
                }

                if (subjects != null && subjects.Count() > 0) // Баллы, которые надо проверить, найдены
                {
                    if (EgeResults.Count > 0 && EgeResults.Any(x => x.egeResult.HasValue))
                    {
                        List<int> errorSubjects = new List<int>();
                        foreach (var res in EgeResults)
                        {
                            if (!res.egeResult.HasValue) continue;

                            int? minValue = subjects.Where(x => x.SubjectId == res.SubjectID).Select(x => x.EgeMinValue).FirstOrDefault();
                            if (minValue.HasValue && res.egeResult.Value < (decimal)minValue)
                            {
                                errors.Add(string.Format("Для использования льготы минимальный балл по предмету {0} не может быть меньше {1} баллов (есть {2} баллов)", res.SubjectName, minValue.Value, res.egeResult.Value));
                            }
                        }
                    }
                }
            }

            return errors.ToArray();
        }

        public static string[] CheckSubjectEGEMinValues(this EntrantsEntities dbContext, int appID)
        {
            var app = dbContext.Application.FirstOrDefault(x => x.ApplicationID == appID);

            List<string> errors = new List<string>();

            var applicationTestDocuments = app.ApplicationEntranceTestDocument.Where(x => x.BenefitID == 3).ToList();

            foreach (var doc in applicationTestDocuments)
            {
                if (!doc.EntrantDocumentID.HasValue)
                    continue;

                //Получим документ на льготу
                BaseDocumentViewModel baseDoc = dbContext.LoadEntrantDocument(doc.EntrantDocumentID.Value);
                baseDoc.FillData(dbContext, true, null, null);

                OlympicDocumentViewModel olympDoc = baseDoc as OlympicDocumentViewModel;

                if (olympDoc == null)
                    continue;

                var AllEgeResults = dbContext.ApplicationEntranceTestDocument.Where(x => x.ApplicationID == app.ApplicationID)
                    .Select(x => new
                    {
                        egeResult = x.ResultValue,
                        SubjectID = x.SubjectID,
                        ItemId = x.EntranceTestItemID,
                        SubjectName = x.Subject.Name
                    }).ToList();

                //Выберем все ВИ для данной КГ.
                var items = dbContext.EntranceTestItemC.Where(x => x.CompetitiveGroupID == doc.CompetitiveGroupID).Select(x => x.EntranceTestItemID).ToArray();

                // Отсечём те результаты ЕГЭ, что не относятся к данной КГ
                var EgeResults = AllEgeResults.Where(x => x.ItemId.HasValue && items.Contains(x.ItemId.Value)).ToList();

                if (!doc.EntranceTestItemID.HasValue)
                    continue; // ДОкумент не относится к льготе по предмету

                var benefits = dbContext.BenefitItemC.Where(x => x.EntranceTestItemID == doc.EntranceTestItemID).ToList();

                // Среди них попробуем найти ту, которая соответствует конкретной олимпиаде
                // Для начала отберём все олимпиады, непосредственно связанные с льготами
                var benefitsForSelectedOlympics = dbContext.BenefitItemCOlympicType.Where(
                    x => x.OlympicTypeID == olympDoc.OlympicID).Select(x => x.BenefitItemID).ToList();

                // Отберём льготу(ы), связанные с конкретной олимпиадой.
                // По логике вещей длина данного списка может быть либо 0, либо 1
                var olympicBenefits = benefits.Where(x => !x.IsForAllOlympic && benefitsForSelectedOlympics.Contains(x.BenefitItemID)).ToList();

                BenefitItemC benefitToCheck = null;

                if (olympicBenefits.Count == 0)
                {
                    // Льготы для конкретной олимпиады нет, ищем льготу по предмету для всех олимпиад
                    // По логике вещей она должна быть только одна, если больше - берём первую
                    benefitToCheck = benefits.FirstOrDefault(x => x.IsForAllOlympic);
                }
                else
                {
                    // Льгота для конкретной олимпиады есть.
                    // по логике вещей она должна быть только одна, но на всякий случай - берём только первую
                    benefitToCheck = olympicBenefits.FirstOrDefault();
                }

                if (benefitToCheck == null) // На всякий случай проверим - есть ли что-нибудь
                    continue;

                // Возьмём результаты ЕГЭ по испытанию.
                // Поскольку льгота приравняла баллы к 100, берём из нового поля
                var egeResultToCheck = dbContext.ApplicationEntranceTestDocument.Where(x => x.ApplicationID == app.ApplicationID && x.EntranceTestItemID == doc.EntranceTestItemID)
                    .Select(x => new { x.EgeResultValue, x.Subject.Name }).FirstOrDefault();

                // Проверяем
                if (!egeResultToCheck.EgeResultValue.HasValue || !benefitToCheck.EgeMinValue.HasValue) // Чего-то нет - либо балла ЕГЭ, либо минимально необходимого балла
                    continue;

                if (egeResultToCheck.EgeResultValue.Value < (decimal)benefitToCheck.EgeMinValue.Value)
                    errors.Add(string.Format("Для использования льготы минимальный балл по предмету {0} не может быть меньше {1} баллов (есть {2} баллов)", egeResultToCheck.Name, benefitToCheck.EgeMinValue.Value, egeResultToCheck.EgeResultValue.Value));
            }

            return errors.ToArray();
        }
    }
}