using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GVUZ.Web.ViewModels;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Helper;
using GVUZ.Model.Helpers;
using FogSoft.Helpers;

namespace GVUZ.Web.ContextExtensions
{
    /// <summary>
    /// Класс-расширение для работы с индивидуальными достижениями
    /// </summary>
    public static class IndividualAchivementsExtension
    {
        public static IndividualAchivementsViewModel GetApplicationIndividualAchivements(this EntrantsEntities dbContext, int applicationID, int entrantID)
        {
            IndividualAchivementsViewModel result = new IndividualAchivementsViewModel();
            result.ApplicationID = applicationID;

            result.EntrantID = entrantID;

            result.Items = dbContext.IndividualAchivement
                .Include("EntrantDocumentCustom")
                .Where(x => x.ApplicationID == applicationID)
                .Select(x => new IndividualAchivementsViewModel.IndividualAchivementViewModel()
                {
                    IAID = x.IAID,
                    UID = x.IAUID,
                    IAName = x.IAName,
                    IAMark = x.IAMark,
                    IADocument = new CustomDocumentViewModel()
                    {
                        DocumentDate = x.EntrantDocument.DocumentDate,
                        DocumentNumber = x.EntrantDocument.DocumentNumber,
                        DocumentSeries = x.EntrantDocument.DocumentSeries,
                        EntrantDocumentID = x.EntrantDocumentID.Value
                    }
                }).ToArray();

            foreach (var item in result.Items)
            {
#warning Переделать!!!!
                BaseDocumentViewModel baseDoc = dbContext.LoadEntrantDocument(item.IADocument.EntrantDocumentID);
                baseDoc.FillData(dbContext, true, null, null);
                item.IADocument.DocumentTypeNameText = (baseDoc is CustomDocumentViewModel ? ((CustomDocumentViewModel)baseDoc).DocumentTypeNameText : baseDoc.DocumentTypeName);
            }
            return result;
        }

        public static AjaxResultModel SaveIndividualAchievement(this EntrantsEntities dbContext, IndividualAchivementsViewModel.IndividualAchivementViewModel model, int applicationID)
        {
            if (!model.IADocumentID.HasValue)
                return new AjaxResultModel("Не выбран подтверждающий документ");

            BaseDocumentViewModel baseDoc = dbContext.LoadEntrantDocument(model.IADocumentID.Value);
            if (baseDoc == null || !(baseDoc is CustomDocumentViewModel))
                return new AjaxResultModel("Не найден подтверждающий документ");

            var newIA = dbContext.IndividualAchivement.CreateObject();
            newIA.ApplicationID = applicationID;
            newIA.EntrantDocumentID = model.IADocumentID.Value;
            newIA.IAMark = string.IsNullOrEmpty(model.IAMarkString) ? (decimal?)null : model.IAMarkString.ToDecimal();
            newIA.IAName = model.IAName;
            newIA.IAUID = model.UID;

            dbContext.IndividualAchivement.AddObject(newIA);
            dbContext.SaveChanges();
            return new AjaxResultModel();
        }

        public static AjaxResultModel SaveNewAchivementDocument(this EntrantsEntities context, CustomDocumentViewModel model)
        {
            var result = context.SaveEntrantDocument(model, string.Empty);

            if (result.IsError)
                return new AjaxResultModel("Ошибка сохранения документа - " + result.Message);

            return new AjaxResultModel();
        }

        public static AjaxResultModel DeleteIndividualAchievement(this EntrantsEntities context, int IAID)
        {
            var objectToDelete = context.IndividualAchivement.FirstOrDefault(x => x.IAID == IAID);

            if (objectToDelete == null)
                return new AjaxResultModel("Не найдено индивидуальное достижение для удаления");

            context.DeleteObject(objectToDelete);
            context.SaveChanges();
            context.DeleteEntrantDocument(objectToDelete.EntrantDocumentID.Value, UserInfo.Default);

            return new AjaxResultModel();
        }
    }
}