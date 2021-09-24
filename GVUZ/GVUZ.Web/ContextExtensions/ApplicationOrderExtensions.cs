using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Applications;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.Web.ViewModels;
using Microsoft.Practices.ServiceLocation;
using GVUZ.Model.ApplicationPriorities;
using GVUZ.DAL.Dapper.Repository.Model;

using Application = GVUZ.Model.Entrants.Application;
using CompetitiveGroupItem = GVUZ.Model.Entrants.CompetitiveGroupItem;
using OrderOfAdmissionType = GVUZ.Data.Model.OrderOfAdmissionType;
using OrderOfAdmissionStatus = GVUZ.Data.Model.OrderOfAdmissionStatus;
using OrderOfAdmission = GVUZ.Data.Model.OrderOfAdmission;
using OrderOfAdmissionHistory = GVUZ.Data.Model.OrderOfAdmissionHistory;

namespace GVUZ.Web.ContextExtensions
{
    /// <summary>
    /// Включение заявлений в приказ
    /// </summary>
    public static class ApplicationOrderExtensions
    {
        // MISSING: используется пустая таблица ApplicationSelectedCompetitiveGroup! 
        /// <summary>
        /// Заполняем модель для интерфейса включения в приказ
        /// </summary>
        public static ApplicationIncludeInOrderViewModel FillIncludeInOrderViewModel(this EntrantsEntities dbContext,
                                                                                     ApplicationIncludeInOrderViewModel model)
        {
            // берём все данные по заявлению
            Application app = dbContext.Application
                .Include(x => x.Entrant)
                .Include(x => x.Entrant.EntrantDocument_Identity)
                //.Include(x => x.ApplicationSelectedCompetitiveGroup)
                //.Include(x => x.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup))
                .Include(x => x.ApplicationSelectedCompetitiveGroupItem)
                .Include(x => x.ApplicationSelectedCompetitiveGroupItem.Select(y => y.CompetitiveGroupItem))
                //.Include(
                //    x => x.ApplicationSelectedCompetitiveGroupItem.Select(y => y.CompetitiveGroupTargetItem))
                .Include(x => x.CompetitiveGroup.CompetitiveGroupTargetItem)
                .Include(x => x.ApplicationSelectedCompetitiveGroupTarget)
                .Include(x => x.ApplicationSelectedCompetitiveGroupTarget.Select(y => y.CompetitiveGroupTarget))
                .Single(x => x.ApplicationID == model.ApplicationID);
            dbContext.AddApplicationAccessToLog(app, "IncludeInOrder");
            model.ApplicationNumber = app.ApplicationNumber;
            model.FIO = app.Entrant.LastName + " " + app.Entrant.FirstName + " " + app.Entrant.MiddleName;
            IdentityDocumentViewModel docModel =
                dbContext.LoadEntrantDocument(app.Entrant.EntrantDocument_Identity.EntrantDocumentID) as IdentityDocumentViewModel;
            if (docModel == null)
            {
                model.DocumentData = app.Entrant.EntrantDocument_Identity.DocumentSeries + " " +
                    app.Entrant.EntrantDocument_Identity.DocumentNumber;
            }
            else
            {
                docModel.FillData(dbContext, true, null, null);
                model.DocumentData = String.Format("{0} {1} {2}", docModel.IdentityDocumentTypeName, docModel.DocumentSeries,
                    docModel.DocumentNumber);
            }

            //нет выбранных КГ - нет данных
            //if (app.ApplicationSelectedCompetitiveGroup.Count == 0)
            //{
            //    model.NoData = true;
            //}

            string benefitErrorMessage;
            // проверяем на ошибку с льготами
            if (ApplicationCountValidator.IsCommonBenefitMoreThanOnce(app.ApplicationID, app.RegistrationDate, out benefitErrorMessage))
                model.OrderErrorMessage = benefitErrorMessage;

            // список подходящих организаций ЦП (для отображения)
            model.TargetOrganizationNameO =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForO).Select(x => x.CompetitiveGroupTarget.Name)
                    .FirstOrDefault();
            model.TargetOrganizationNameOZ =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForOZ).Select(x => x.CompetitiveGroupTarget.Name)
                    .FirstOrDefault();
            model.TargetOrganizationNameZ =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForZ).Select(x => x.CompetitiveGroupTarget.Name)
                    .FirstOrDefault();

            model.NoOriginalDocuments = !app.OriginalDocumentsReceived.To(false);
            model.IsVuz = dbContext.Institution.Where(x => x.InstitutionID == app.InstitutionID).Single().InstitutionTypeID == 1;

            model.ForListener = app.ApplicationEntrantDocument.Any(y => dbContext.EntrantDocument
                        .Where(x => x.DocumentTypeID == 18).Select(x => x.EntrantDocumentID).Contains(y.EntrantDocumentID));


            // существующие источники финансирования и формы обучения
            model.EducationSources = dbContext
                .AdmissionItemType.Where(x => x.ItemLevel == 8).OrderBy(x => x.Name).Select(x => new { ID = x.ItemTypeID, x.Name }).
                ToArray();
            model.EducationForms =
                dbContext.AdmissionItemType.Where(x => x.ItemLevel == 7).ToArray()
                    .OrderBy(x => x.Name).Select(x => new { ID = x.ItemTypeID, x.Name });
            if (model.DirectionID == 0)
                model.DirectionID = app.OrderCompetitiveGroupItemID ?? 0;

            var appCGIs = app.ApplicationSelectedCompetitiveGroupItem.Select(x => x.CompetitiveGroupItemID).ToArray();
            //количество нецелевых заявлений в приказе
            var otherAppsNonTarget = dbContext.Application
                .Where(
                    x =>
                        appCGIs.Contains(x.OrderCompetitiveGroupItemID ?? 0) && x.ApplicationID != app.ApplicationID &&
                            x.OrderEducationSourceID != EDSourceConst.Target && x.InstitutionID == app.InstitutionID)
                .Select(x => new { x.OrderCompetitiveGroupItemID, x.OrderEducationFormID, x.OrderEducationSourceID }).ToList();

            //сейчас по контексту может быть только одна. Так что делаем такую логику
            var targetOrgIDO =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForO).Select(x => x.CompetitiveGroupTargetID)
                    .FirstOrDefault();
            var targetOrgIDOZ =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForOZ).Select(x => x.CompetitiveGroupTargetID)
                    .FirstOrDefault();
            var targetOrgIDZ =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForZ).Select(x => x.CompetitiveGroupTargetID)
                    .FirstOrDefault();

            //количество целевых заявлений в приказе
            var otherAppsTarget = dbContext.Application
                .Where(x => appCGIs.Contains(x.OrderCompetitiveGroupItemID ?? 0)
                    && x.ApplicationID != app.ApplicationID
                    &&
                    (x.OrderCompetitiveGroupTargetID == targetOrgIDO || x.OrderCompetitiveGroupTargetID == targetOrgIDOZ ||
                        x.OrderCompetitiveGroupTargetID == targetOrgIDZ)
                    && x.OrderEducationSourceID == EDSourceConst.Target
                    && x.InstitutionID == app.InstitutionID)
                .Select(x => new { x.OrderCompetitiveGroupItemID, x.OrderEducationFormID, x.OrderEducationSourceID }).ToList();

            //данные по направлениям
            var groupItems = app.ApplicationSelectedCompetitiveGroupItem
                .Select(
                    x =>
                        new
                        {
                            G = x.CompetitiveGroupItem,
                            DirectionName = x.CompetitiveGroupItem.CompetitiveGroup.Direction.Name,
                            EducationLevelID = x.CompetitiveGroupItem.CompetitiveGroup.EducationLevelID,
                            CompetitiveGroupName = x.CompetitiveGroupItem.CompetitiveGroup.Name,
                            TargetCountO =
                                x.CompetitiveGroupItem.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.CompetitiveGroupTargetID == targetOrgIDO)
                                .Sum(y => (int?)y.NumberTargetO) ?? 0,
                            TargetCountOZ =
                                x.CompetitiveGroupItem.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.CompetitiveGroupTargetID == targetOrgIDOZ)
                                .Sum(y => (int?)y.NumberTargetOZ) ?? 0,
                            TargetCountZ =
                                x.CompetitiveGroupItem.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.CompetitiveGroupTargetID == targetOrgIDZ)
                                .Sum(y => (int?)y.NumberTargetZ) ?? 0,
                        }).ToList();

            //направления
            model.Directions = groupItems
                .OrderBy(x => x.CompetitiveGroupName)
                .ThenBy(x => x.EducationLevelID)
                .Select(x => new
                {
                    ID = x.G.CompetitiveGroupItemID,
                    Name = x.CompetitiveGroupName
                        + ", "
                        + DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, x.EducationLevelID.Value)
                        + ", " + x.DirectionName
                }).ToArray();

            //без ВИ и вне конкурса
            //var groupsBeneficiary =
            //    app.ApplicationSelectedCompetitiveGroup.Where(
            //        x => (x.CalculatedBenefitID == 1 || x.CalculatedBenefitID == 4) && x.CompetitiveGroup.Course == 1)
            //        .Select(x => x.CompetitiveGroupID).ToArray();
            //model.BeneficiaryDirections = groupItems
            //    .Where(x => groupsBeneficiary.Contains(x.G.CompetitiveGroupID))
            //    .Select(x => x.G.CompetitiveGroupItemID).ToArray();

            List<string> availableForms = new List<string>();
            List<string> availableSources = new List<string>();
            //вычисляем допустимость формы
            Func<int, int, int, int> otherCntNonTarget =
                (g, f, s) =>
                    otherAppsNonTarget.Count(
                        x => x.OrderCompetitiveGroupItemID == g && x.OrderEducationFormID == f && x.OrderEducationSourceID == s);
            Func<int, int, int, int> otherCntTarget =
                (g, f, s) =>
                    otherAppsTarget.Count(
                        x => x.OrderCompetitiveGroupItemID == g && x.OrderEducationFormID == f && x.OrderEducationSourceID == s);

            Action<int, CompetitiveGroupItem, int, int, bool, Func<int, int, int, int>> addData =
                (n, gri, source, form, e, cntFunc) =>
                {
                    if (n - cntFunc(gri.CompetitiveGroupItemID, form, source) > 0 && e)
                        availableSources.Add(gri.CompetitiveGroupItemID + "." +
                            form + "." + source);
                };
            // #40492 - в интерфейсе пусть включают сколько угодно.
            // возвращаем 0 - типа никого ещё не включили
            otherCntNonTarget = (g, f, s) => 0;
            otherCntTarget = (g, f, s) => 0;

            dbContext.Connection.Close();
            ApplicationPrioritiesViewModel priorities;

            using (var prioritiesContext = new ApplicationPrioritiesEntities())
            {
                priorities = prioritiesContext.FillExistingPriorities(app.ApplicationID);
            }

            dbContext.Connection.Open();

            Func<ApplicationPrioritiesViewModel, int, int, bool> IsRequiresFormSource = (pr, form, source) =>
            {
                if (source != EDSourceConst.Budget)
                    return pr.ApplicationPriorities.Any(x => x.EducationFormId == form && x.EducationSourceId == source && x.Priority.HasValue);
                else return pr.ApplicationPriorities.Any(x => x.EducationFormId == form && (x.EducationSourceId == source || x.EducationSourceId == EDSourceConst.Quota) && x.Priority.HasValue);
            };

            //в зависимости от того, разрешено ли в заявлении и есть ли места, добавляем формы и источники
            foreach (var g in groupItems)
            {
                var gi = g.G.CompetitiveGroupItemID;
                if (g.G.NumberBudgetO + g.G.NumberPaidO + g.TargetCountO
                    - otherCntNonTarget(gi, 14, 11) - otherCntNonTarget(gi, 15, 11) - otherCntTarget(gi, 16, 11) > 0
                    && (priorities.ApplicationPriorities.Any(x => x.EducationFormId == EDFormsConst.O && x.Priority.HasValue)))
                {
                    availableForms.Add(g.G.CompetitiveGroupItemID + ".11");
                    addData(g.G.NumberBudgetO.Value, g.G, 14, 11, IsRequiresFormSource(priorities, EDFormsConst.O, EDSourceConst.Budget), otherCntNonTarget);
                    addData(g.G.NumberPaidO.Value, g.G, 15, 11, IsRequiresFormSource(priorities, EDFormsConst.O, EDSourceConst.Paid), otherCntNonTarget);
                    addData(g.TargetCountO, g.G, 16, 11, IsRequiresFormSource(priorities, EDFormsConst.O, EDSourceConst.Target), otherCntTarget);
                }

                if (g.G.NumberBudgetOZ + g.G.NumberPaidOZ + g.TargetCountOZ
                    - otherCntNonTarget(gi, 14, 12) - otherCntNonTarget(gi, 15, 12) - otherCntTarget(gi, 16, 12) > 0
                    && (priorities.ApplicationPriorities.Any(x => x.EducationFormId == EDFormsConst.O && x.Priority.HasValue)))
                {
                    availableForms.Add(g.G.CompetitiveGroupItemID + ".12");
                    addData(g.G.NumberBudgetOZ.Value, g.G, 14, 12, IsRequiresFormSource(priorities, EDFormsConst.OZ, EDSourceConst.Budget), otherCntNonTarget);
                    addData(g.G.NumberPaidOZ.Value, g.G, 15, 12, IsRequiresFormSource(priorities, EDFormsConst.OZ, EDSourceConst.Paid), otherCntNonTarget);
                    addData(g.TargetCountOZ, g.G, 16, 12, IsRequiresFormSource(priorities, EDFormsConst.OZ, EDSourceConst.Target), otherCntTarget);
                }

                if (g.G.NumberBudgetZ + g.G.NumberPaidZ + g.TargetCountZ
                    - otherCntNonTarget(gi, 14, 10) - otherCntNonTarget(gi, 15, 10) - otherCntTarget(gi, 16, 10) > 0
                    && (priorities.ApplicationPriorities.Any(x => x.EducationFormId == EDFormsConst.O && x.Priority.HasValue)))
                {
                    availableForms.Add(g.G.CompetitiveGroupItemID + ".10");
                    addData(g.G.NumberBudgetZ.Value, g.G, 14, 10, IsRequiresFormSource(priorities, EDFormsConst.Z, EDSourceConst.Budget), otherCntNonTarget);
                    addData(g.G.NumberPaidZ.Value, g.G, 15, 10, IsRequiresFormSource(priorities, EDFormsConst.Z, EDSourceConst.Paid), otherCntNonTarget);
                    addData(g.TargetCountZ, g.G, 16, 10, IsRequiresFormSource(priorities, EDFormsConst.Z, EDSourceConst.Target), otherCntTarget);
                }
            }

            model.AvailableForms = availableForms.ToArray();
            model.AvailableSources = availableSources.ToArray();

            model.CanIncludeInOrder = false;

            if (priorities.ApplicationPriorities.Any(x => x.EducationSourceId == EDSourceConst.Paid))
                model.CanIncludeInOrder = true;

            //нельзя включать в КГ без ВИ
            /* Для СПО не выдавать этого сообщения */
            //if (app.ApplicationSelectedCompetitiveGroup.Any(x =>
            //    x.CompetitiveGroup.CompetitiveGroupItem.Any(c => c.EducationLevelID != EDLevelConst.SPO) && !x.CompetitiveGroup.EntranceTestItemC.Any()))
            //    model.OrderErrorMessage = "В заявлении есть конкурс без вступительных испытаний. Необходимо добавить вступительные испытания или исключить конкурс из заявления.";

            //если почему-то остались данные по приказу, делаем изначальный выбор
            if (app.OrderOfAdmissionID.HasValue)
            {
                model.EducationFormID = app.OrderEducationFormID ?? 0;
                model.EducationSourceID = app.OrderEducationSourceID ?? 0;
            }

            model.IsForeignCitizen = app.IsForeignCitizen();

            return model;
        }

        static string[] GetCommonList(string[] firstArr, string[] secondArr)
        {
            List<string> strsCommon = new List<string>();

            foreach (string str in secondArr)
            {
                if (firstArr.Contains(str))
                    strsCommon.Add(str);
            }

            return strsCommon.ToArray();
        }

        static int[] GetCommonList(int[] firstArr, int[] secondArr)
        {
            List<int> listCommon = new List<int>();

            foreach (int n in secondArr)
            {
                if (firstArr.Contains(n))
                    listCommon.Add(n);
            }

            return listCommon.ToArray();
        }

        /// <summary>
        /// Заполняем общую модель для диалога включения в приказ группы заявлений
        /// </summary>
        public static ApplicationIncludeInOrderViewModel FillCommonModel4AllApps(this EntrantsEntities dbContext, ApplicationIncludeInOrderViewModel model)
        {
            if ((model.applicationIds == null) || (model.applicationIds.Length == 0))
            {
                model.OrderErrorMessage = "Ошибка: в модели отсутствуют идентификаторы заявлений.";
                return model;
            }

            ApplicationIncludeInOrderViewModel commonModel = new ApplicationIncludeInOrderViewModel();
            ApplicationIncludeInOrderViewModel customModel = null;

            List<int> entrants = new List<int>();

            for (int i = 0; i < model.applicationIds.Length; i++)
            {
                customModel = FillModel4OneApplication(dbContext, model.applicationIds[i]);

                // возвращаем модель с ошибкой, если она уже есть при заполнении
                if (!string.IsNullOrEmpty(customModel.OrderErrorMessage))
                    return customModel;

                // проверяем соответствие полученной модели требованиям
                if ((customModel.AvailableForms == null) || (customModel.AvailableSources == null) || (customModel.DirectionIDs == null) || (customModel.DirectionIDs == null))
                {
                    customModel.OrderErrorMessage = "Ошибка: неправильные данные заявления";
                    return customModel;
                }

                if (customModel.AvailableSources.Length == 0)
                {
                    customModel.OrderErrorMessage = "Ошибка: Отсутствуют источники финансирования заявления " + customModel.ApplicationNumber;
                    return customModel;
                }

                if ((customModel.DirectionIDs.Length == 0) || (customModel.DirectionNames.Length == 0))
                {
                    customModel.OrderErrorMessage = "Ошибка: Отсутствуют направления подготовки для заявления " + customModel.ApplicationNumber;
                    return customModel;
                }

                if (entrants.Contains(customModel.EntrantID))
                {
                    customModel.OrderErrorMessage = "Ошибка: несколько заявлений от одного и того же абитуриента не может быть включено в один и тот же приказ.";
                    return customModel;
                }
                entrants.Add(customModel.EntrantID);

                if (i == 0) // устанавливаем данные первого элемента как общие данные модели
                {
                    commonModel.AvailableForms = customModel.AvailableForms;
                    commonModel.AvailableSources = customModel.AvailableSources;
                    commonModel.DirectionIDs = customModel.DirectionIDs;
                    commonModel.DirectionNames = customModel.DirectionNames;
                    commonModel.BeneficiaryDirections = customModel.BeneficiaryDirections;
                    commonModel.IsForeignCitizen = customModel.IsForeignCitizen;
                }
                else
                {
                    commonModel.AvailableForms = GetCommonList(customModel.AvailableForms, commonModel.AvailableForms);
                    commonModel.AvailableSources = GetCommonList(customModel.AvailableSources, commonModel.AvailableSources);
                    commonModel.DirectionIDs = GetCommonList(customModel.DirectionIDs, commonModel.DirectionIDs);
                    commonModel.DirectionNames = GetCommonList(customModel.DirectionNames, commonModel.DirectionNames);
                    if ((commonModel.BeneficiaryDirections != null) && (commonModel.BeneficiaryDirections.Length > 0))
                    {
                        commonModel.BeneficiaryDirections = GetCommonList(customModel.BeneficiaryDirections, commonModel.BeneficiaryDirections);
                    }

                    if (commonModel.AvailableForms.Length == 0)
                    {
                        commonModel.OrderErrorMessage = "Ошибка: отсутствуют общие формы обучения или направления подготовки для выбранных заявлений";
                        return commonModel;
                    }

                    if (commonModel.AvailableSources.Length == 0)
                    {
                        commonModel.OrderErrorMessage = "Ошибка: Отсутствуют общие источники финансирования или направления подготовки для выбранных заявлений";
                        return commonModel;
                    }

                    if ((commonModel.DirectionIDs.Length == 0) || (commonModel.DirectionNames.Length == 0))
                    {
                        commonModel.OrderErrorMessage = "Ошибка: Отсутствуют общие направления подготовки для выбранных заявлений";
                        return commonModel;
                    }
                }
            }

            // заполняем полные списки ФО и ИФ
            commonModel.EducationSources = dbContext.AdmissionItemType.Where(x => x.ItemLevel == 8).OrderBy(x => x.Name).Select(x => new { ID = x.ItemTypeID, x.Name }).
                ToArray();
            commonModel.EducationForms =
                dbContext.AdmissionItemType.Where(x => x.ItemLevel == 7).ToArray()
                .OrderBy(x => x.Name).Select(x => new { ID = x.ItemTypeID, x.Name });

            // сами заполняем Directions по собранным массивам DirectionIDs, DirectionNames - вроде бы в нужном формате
            List<object> dirList = new List<object>();
            for (int j = 0; j < commonModel.DirectionIDs.Length; j++)
                dirList.Add(new { ID = commonModel.DirectionIDs[j], Name = commonModel.DirectionNames[j] });

            commonModel.Directions = dirList;
            commonModel.applicationIds = model.applicationIds;
            return commonModel;
        }

        static ApplicationIncludeInOrderViewModel FillModel4OneApplication(this EntrantsEntities dbContext, int appID)
        {
            // это временная модель, заполненная для одного заявления 
            ApplicationIncludeInOrderViewModel model = new ApplicationIncludeInOrderViewModel();

            // берём все данные по заявлению
            Application app = dbContext.Application
                .Include(x => x.Entrant)
                .Include(x => x.Entrant.EntrantDocument_Identity)
                .Include(x => x.CompetitiveGroup)
                //.Include(x => x.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup))
                .Include(x => x.ApplicationSelectedCompetitiveGroupItem)
                .Include(x => x.ApplicationSelectedCompetitiveGroupItem.Select(y => y.CompetitiveGroupItem))
                .Include(x => x.CompetitiveGroup.CompetitiveGroupTargetItem)
                .Include(x => x.ApplicationSelectedCompetitiveGroupTarget)
                .Include(x => x.ApplicationSelectedCompetitiveGroupTarget.Select(y => y.CompetitiveGroupTarget))
                .Single(x => x.ApplicationID == appID);
            dbContext.AddApplicationAccessToLog(app, "IncludeInOrder");

            model.ApplicationNumber = app.ApplicationNumber;

            model.EntrantID = app.EntrantID;

            IdentityDocumentViewModel docModel = dbContext.LoadEntrantDocument(app.Entrant.EntrantDocument_Identity.EntrantDocumentID) as IdentityDocumentViewModel;
            if (docModel == null)
            {
                model.DocumentData = app.Entrant.EntrantDocument_Identity.DocumentSeries + " " +
                    app.Entrant.EntrantDocument_Identity.DocumentNumber;
            }
            else
            {
                docModel.FillData(dbContext, true, null, null);
                model.DocumentData = String.Format("{0} {1} {2}", docModel.IdentityDocumentTypeName, docModel.DocumentSeries,
                    docModel.DocumentNumber);
            }

            //нет выбранных КГ - нет данных
            //if (app.ApplicationSelectedCompetitiveGroup.Count == 0)
            //{
            //    // сейчас возврашается ошибка, если в заявлении нет выбранных КГ
            //    model.OrderErrorMessage = string.Format("Ошибка: в заявлении {0} не выбраны конкурсы", app.ApplicationNumber);
            //    return model;
            //}

            string benefitErrorMessage;
            // проверяем на ошибку с льготами
            // **                
            if (ApplicationCountValidator.IsCommonBenefitMoreThanOnce(app.ApplicationID, app.RegistrationDate, out benefitErrorMessage))
            {
                model.OrderErrorMessage = "Ошибка. " + benefitErrorMessage;
                return model;
            }

            // список организаций ЦП (заносится в модель, только если совпадает для всех заявлений)
            model.TargetOrganizationNameO =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForO).Select(x => x.CompetitiveGroupTarget.Name)
                    .FirstOrDefault();
            model.TargetOrganizationNameOZ =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForOZ).Select(x => x.CompetitiveGroupTarget.Name)
                    .FirstOrDefault();
            model.TargetOrganizationNameZ =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForZ).Select(x => x.CompetitiveGroupTarget.Name)
                    .FirstOrDefault();

            model.DirectionID = app.OrderCompetitiveGroupItemID ?? 0;

            var appCGIs = app.ApplicationSelectedCompetitiveGroupItem.Select(x => x.CompetitiveGroupItemID).ToArray();
            //количество нецелевых заявлений в приказе
            var otherAppsNonTarget = dbContext.Application
                .Where(
                    x =>
                        appCGIs.Contains(x.OrderCompetitiveGroupItemID ?? 0) && x.ApplicationID != app.ApplicationID &&
                            x.OrderEducationSourceID != EDSourceConst.Target && x.InstitutionID == app.InstitutionID)
                .Select(x => new { x.OrderCompetitiveGroupItemID, x.OrderEducationFormID, x.OrderEducationSourceID }).ToList();

            //сейчас по контексту может быть только одна. Так что делаем такую логику
            var targetOrgIDO =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForO).Select(x => x.CompetitiveGroupTargetID)
                    .FirstOrDefault();
            var targetOrgIDOZ =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForOZ).Select(x => x.CompetitiveGroupTargetID)
                    .FirstOrDefault();
            var targetOrgIDZ =
                app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForZ).Select(x => x.CompetitiveGroupTargetID)
                    .FirstOrDefault();

            //количество целевых заявлений в приказе
            var otherAppsTarget = dbContext.Application
                .Where(x => appCGIs.Contains(x.OrderCompetitiveGroupItemID ?? 0)
                    && x.ApplicationID != app.ApplicationID
                    &&
                    (x.OrderCompetitiveGroupTargetID == targetOrgIDO || x.OrderCompetitiveGroupTargetID == targetOrgIDOZ ||
                        x.OrderCompetitiveGroupTargetID == targetOrgIDZ)
                    && x.OrderEducationSourceID == EDSourceConst.Target
                    && x.InstitutionID == app.InstitutionID)
                .Select(x => new { x.OrderCompetitiveGroupItemID, x.OrderEducationFormID, x.OrderEducationSourceID }).ToList();

            //данные по направлениям
            var groupItems = app.ApplicationSelectedCompetitiveGroupItem
                .Select(
                    x =>
                        new
                        {
                            G = x.CompetitiveGroupItem,
                            DirectionName = x.CompetitiveGroupItem.CompetitiveGroup.Direction.Name,
                            EducationLevelID = x.CompetitiveGroupItem.CompetitiveGroup.EducationLevelID,
                            CompetitiveGroupName = x.CompetitiveGroupItem.CompetitiveGroup.Name,
                            TargetCountO =
                                x.Application.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.CompetitiveGroupTargetID == targetOrgIDO)
                                .Sum(y => (int?)y.NumberTargetO) ?? 0,
                            TargetCountOZ =
                                x.Application.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.CompetitiveGroupTargetID == targetOrgIDOZ)
                                .Sum(y => (int?)y.NumberTargetOZ) ?? 0,
                            TargetCountZ =
                                x.Application.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.CompetitiveGroupTargetID == targetOrgIDZ)
                                .Sum(y => (int?)y.NumberTargetZ) ?? 0,
                        }).ToList();

            //направления
            model.Directions = groupItems
                .OrderBy(x => x.CompetitiveGroupName)
                .ThenBy(x => x.EducationLevelID)
                .Select(x => new
                {
                    ID = x.G.CompetitiveGroupItemID,
                    Name = x.CompetitiveGroupName
                        + ", "
                        + DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, x.EducationLevelID.Value)
                        + ", " + x.DirectionName
                }).ToArray();

            // заполнение дополнительных данных в модели (отдельно идентификаторы направлений и имена)
            model.DirectionIDs = groupItems
            .OrderBy(x => x.CompetitiveGroupName)
            .ThenBy(x => x.EducationLevelID)
            .Select(x => x.G.CompetitiveGroupItemID).ToArray();

            model.DirectionNames = groupItems
            .OrderBy(x => x.CompetitiveGroupName)
            .ThenBy(x => x.EducationLevelID)
            .Select(x => x.CompetitiveGroupName + "," + DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, x.EducationLevelID.Value) + "," + x.DirectionName).ToArray();


            //без ВИ и вне конкурса
            //var groupsBeneficiary =
            //    app.ApplicationSelectedCompetitiveGroup.Where(
            //        x => (x.CalculatedBenefitID == 1 || x.CalculatedBenefitID == 4) && x.CompetitiveGroup.Course == 1)
            //        .Select(x => x.CompetitiveGroupID).ToArray();
            //model.BeneficiaryDirections = groupItems
            //    .Where(x => groupsBeneficiary.Contains(x.G.CompetitiveGroupID))
            //    .Select(x => x.G.CompetitiveGroupItemID).ToArray();

            List<string> availableForms = new List<string>();
            List<string> availableSources = new List<string>();
            //вычисляем допустимость формы
            Func<int, int, int, int> otherCntNonTarget =
                (g, f, s) =>
                    otherAppsNonTarget.Count(
                        x => x.OrderCompetitiveGroupItemID == g && x.OrderEducationFormID == f && x.OrderEducationSourceID == s);
            Func<int, int, int, int> otherCntTarget =
                (g, f, s) =>
                    otherAppsTarget.Count(
                        x => x.OrderCompetitiveGroupItemID == g && x.OrderEducationFormID == f && x.OrderEducationSourceID == s);

            Action<int, CompetitiveGroupItem, int, int, bool, Func<int, int, int, int>> addData =
                (n, gri, source, form, e, cntFunc) =>
                {
                    if (n - cntFunc(gri.CompetitiveGroupItemID, form, source) > 0 && e)
                        availableSources.Add(gri.CompetitiveGroupItemID + "." +
                            form + "." + source);
                };
            // #40492 - в интерфейсе пусть включают сколько угодно.
            // возвращаем 0 - типа никого ещё не включили
            otherCntNonTarget = (g, f, s) => 0;
            otherCntTarget = (g, f, s) => 0;

            dbContext.Connection.Close();
            ApplicationPrioritiesViewModel priorities;

            using (var prioritiesContext = new ApplicationPrioritiesEntities())
            {
                priorities = prioritiesContext.FillExistingPriorities(app.ApplicationID);
            }

            dbContext.Connection.Open();

            Func<ApplicationPrioritiesViewModel, int, int, bool> IsRequiresFormSource = (pr, form, source) =>
                {
                    if (source != EDSourceConst.Budget)
                        return pr.ApplicationPriorities.Any(x => x.EducationFormId == form && x.EducationSourceId == source && x.Priority.HasValue);
                    else return pr.ApplicationPriorities.Any(x => x.EducationFormId == form && (x.EducationSourceId == source || x.EducationSourceId == EDSourceConst.Quota) && x.Priority.HasValue);
                };

            //в зависимости от того, разрешено ли в заявлении и есть ли места, добавляем формы и источники
            foreach (var g in groupItems)
            {
                var gi = g.G.CompetitiveGroupItemID;
                if (g.G.NumberBudgetO + g.G.NumberPaidO + g.TargetCountO
                    - otherCntNonTarget(gi, 14, 11) - otherCntNonTarget(gi, 15, 11) - otherCntTarget(gi, 16, 11) > 0
                    && (priorities.ApplicationPriorities.Any(x => x.EducationFormId == EDFormsConst.O && x.Priority.HasValue)))
                {
                    availableForms.Add(g.G.CompetitiveGroupItemID + ".11");
                    addData(g.G.NumberBudgetO.Value, g.G, 14, 11, IsRequiresFormSource(priorities, EDFormsConst.O, EDSourceConst.Budget), otherCntNonTarget);
                    addData(g.G.NumberPaidO.Value, g.G, 15, 11, IsRequiresFormSource(priorities, EDFormsConst.O, EDSourceConst.Paid), otherCntNonTarget);
                    addData(g.TargetCountO, g.G, 16, 11, IsRequiresFormSource(priorities, EDFormsConst.O, EDSourceConst.Target), otherCntTarget);
                }

                if (g.G.NumberBudgetOZ + g.G.NumberPaidOZ + g.TargetCountOZ
                    - otherCntNonTarget(gi, 14, 12) - otherCntNonTarget(gi, 15, 12) - otherCntTarget(gi, 16, 12) > 0
                    && (priorities.ApplicationPriorities.Any(x => x.EducationFormId == EDFormsConst.O && x.Priority.HasValue)))
                {
                    availableForms.Add(g.G.CompetitiveGroupItemID + ".12");
                    addData(g.G.NumberBudgetOZ.Value, g.G, 14, 12, IsRequiresFormSource(priorities, EDFormsConst.OZ, EDSourceConst.Budget), otherCntNonTarget);
                    addData(g.G.NumberPaidOZ.Value, g.G, 15, 12, IsRequiresFormSource(priorities, EDFormsConst.OZ, EDSourceConst.Paid), otherCntNonTarget);
                    addData(g.TargetCountOZ, g.G, 16, 12, IsRequiresFormSource(priorities, EDFormsConst.OZ, EDSourceConst.Target), otherCntTarget);
                }

                if (g.G.NumberBudgetZ + g.G.NumberPaidZ + g.TargetCountZ
                    - otherCntNonTarget(gi, 14, 10) - otherCntNonTarget(gi, 15, 10) - otherCntTarget(gi, 16, 10) > 0
                    && (priorities.ApplicationPriorities.Any(x => x.EducationFormId == EDFormsConst.O && x.Priority.HasValue)))
                {
                    availableForms.Add(g.G.CompetitiveGroupItemID + ".10");
                    addData(g.G.NumberBudgetZ.Value, g.G, 14, 10, IsRequiresFormSource(priorities, EDFormsConst.Z, EDSourceConst.Budget), otherCntNonTarget);
                    addData(g.G.NumberPaidZ.Value, g.G, 15, 10, IsRequiresFormSource(priorities, EDFormsConst.Z, EDSourceConst.Paid), otherCntNonTarget);
                    addData(g.TargetCountZ, g.G, 16, 10, IsRequiresFormSource(priorities, EDFormsConst.Z, EDSourceConst.Target), otherCntTarget);
                }
            }

            model.AvailableForms = availableForms.ToArray();
            model.AvailableSources = availableSources.ToArray();

            //нельзя включать в приказ КГ без ВИ
            //if (app.ApplicationSelectedCompetitiveGroup.Any(x =>
            //    x.CompetitiveGroup.CompetitiveGroupItem.Any(c => c.EducationLevelID != EDLevelConst.SPO) && !x.CompetitiveGroup.EntranceTestItemC.Any()))
            //{
            //    model.OrderErrorMessage = "Ошибка: в заявлении есть конкурс без вступительных испытаний. Необходимо добавить вступительные испытания или исключить конкурс из заявления.";
            //    return model;
            //}

            model.IsForeignCitizen = app.IsForeignCitizen();

            return model;
        }


        /// <summary>
        /// Выключаем заявление в приказ
        /// </summary>
        public static AjaxResultModel IncludeApplicationInOrder(this EntrantsEntities dbContext,
                                                                ApplicationIncludeInOrderViewModel model)
        {
            Application app = dbContext.Application
                                       .Include(x => x.CompetitiveGroup)
                                       //.Include(x => x.ApplicationSelectedCompetitiveGroup)
                                       //.Include(x => x.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup))
                                       .Include(x => x.ApplicationSelectedCompetitiveGroupItem)
                                       .Include(x => x.ApplicationSelectedCompetitiveGroupItem.Select(y => y.CompetitiveGroupItem))
                                       .Include(x => x.ApplicationSelectedCompetitiveGroupTarget).Single(x => x.ApplicationID == model.ApplicationID);
            if (app.OrderOfAdmissionID.HasValue)
                return new AjaxResultModel("Заявление уже включено в приказ");
            if (app.ApplicationSelectedCompetitiveGroupItem.Count == 0)
                return new AjaxResultModel("Заявлению не назначен конкурс");

            if (!app.OriginalDocumentsReceived.To(false))
            {
                if (dbContext.Institution.Single(x => x.InstitutionID == app.InstitutionID).InstitutionTypeID == 2)
                    return new AjaxResultModel("В приказ можно включить только заявления, у которых предоставлены оригиналы документов");
            }
            var cgi = dbContext.CompetitiveGroupItem
                               .Include(x => x.CompetitiveGroup).FirstOrDefault(x => x.CompetitiveGroupItemID == model.DirectionID);
            if (cgi == null || app.ApplicationSelectedCompetitiveGroupItem.All(y => y.CompetitiveGroupItemID != cgi.CompetitiveGroupItemID))
                return new AjaxResultModel("Неверное направление");

            //если указали льготников, проверяем ещё раз, допустимо ли
            //if (model.IsForBeneficiary)
            //{
            //    var groups = app.ApplicationSelectedCompetitiveGroup.Where(x => (x.CalculatedBenefitID == 1 || x.CalculatedBenefitID == 4) && x.CompetitiveGroup.Course == 1)
            //        .Select(x => x.CompetitiveGroupID).ToArray();
            //    var dirs = app.ApplicationSelectedCompetitiveGroupItem.Where(x => groups.Contains(x.CompetitiveGroupItem.CompetitiveGroupID))
            //        .Select(x => x.CompetitiveGroupItemID);
            //    //убираем льготников если запрещены
            //    if (!dirs.Contains(model.DirectionID) || (model.EducationSourceID != EDSourceConst.Budget && model.EducationSourceID != EDSourceConst.Target && model.EducationSourceID != EDSourceConst.Quota))
            //        model.IsForBeneficiary = false;
            //}

            var orderOfAdmissionRepository = new OrderOfAdmissionRepository();

            var orders = orderOfAdmissionRepository.Find(app.InstitutionID, cgi.CompetitiveGroup.CampaignID.GetValueOrDefault(), cgi.CompetitiveGroup.EducationLevelID.Value, model.EducationFormID, (model.EducationSourceID == 20 ? 14 : model.EducationSourceID));

            var sourceId = 0;
            if (model.EducationSourceID == 20)
                sourceId = 14;
            else sourceId = model.EducationSourceID;

            DateTime dateSecondStage = DateTime.MaxValue;
            int stage = 0;
            if (model.IsForBeneficiary) stage = 0; //для льготников нет этапов
            if (stage > 0) //если есть этапы
            {
                if (DateTime.Now < dateSecondStage || model.IsBudgetForeigner) //ставим первый, если не дошла ещё дата второго
                    stage = 1;
                else
                {
                    if (model.OrderTypeSelection > 0) // если пользователь уже выбрал, ставим нужный тип
                    {
                        //if (model.OrderTypeSelection == 1)
                            stage = 1;
                        //else
                        //    stage = 2;
                    }
                    else // если не выбрал, возвращаем информацию о этапах
                    {
                        OrderOfAdmission order1 = orders.Where(x => x.Stage.GetValueOrDefault() == 1).FirstOrDefault();
                        //OrderOfAdmission order2 = orders.Where(x => x.Stage.GetValueOrDefault() == 2).FirstOrDefault();
                        string order1Text = "Приказ 1 этапа. Не создан";
                        //string order2Text = "Приказ 2 этапа. Не создан";
                        if (order1 != null)
                            order1Text = String.Format("Приказ 1 этапа. Дата публикации: {0:dd.MM.yyyy} ({1})", order1.DatePublished, GetOrderStatusName(order1.OrderOfAdmissionStatusID));
                        //if (order2 != null)
                        //    order2Text = String.Format("Приказ 2 этапа. Дата публикации: {0:dd.MM.yyyy} ({1})", order2.DatePublished, GetOrderStatusName(order2.OrderOfAdmissionStatusID));
                        //ещё раз спрашиваем)
                        return new AjaxResultModel
                            {
                                Data = new[] { "0", order1Text}
                            };
                    }
                }
            }
            //находим приказ
            OrderOfAdmission order = model.IsBudgetForeigner ?
                orders.FirstOrDefault(x => x.IsForeigner == model.IsBudgetForeigner)
                : orders.FirstOrDefault(x => x.Stage == stage && x.IsForBeneficiary == model.IsForBeneficiary && x.IsForeigner == model.IsBudgetForeigner);

            //если приказ опубликован, а пользователь не уточнил что точно хочет, возращаем это
            if (order != null && order.OrderOfAdmissionStatusID == OrderOfAdmissionStatus.Published && !model.OrderForcePublished)
                return new AjaxResultModel { Data = new[] { "1" } };

            if (order == null) //создаём новый приказ
            {
                order = new OrderOfAdmission
                            {
                                InstitutionID = app.InstitutionID,
                                OrderOfAdmissionStatusID = OrderOfAdmissionStatus.NoApplications,
                                DateCreated = DateTime.Now,
                                DateEdited = DateTime.Now,

                                CampaignID = cgi.CompetitiveGroup.CampaignID ?? 0,
                                EducationLevelID = cgi.CompetitiveGroup.EducationLevelID,

                                EducationFormID = (short)model.EducationFormID,
                                EducationSourceID = (model.EducationSourceID == EDSourceConst.Quota ? EDSourceConst.Budget : (short)model.EducationSourceID),

                                Stage = model.IsBudgetForeigner ? (short)1 : (short)stage,
                                IsForBeneficiary = model.IsForBeneficiary,
                                IsForeigner = model.IsBudgetForeigner
                            };

                orderOfAdmissionRepository.Insert(order);
            }
            else
            {
                order.DateEdited = DateTime.Now;
                order.OrderOfAdmissionStatusID = OrderOfAdmissionStatus.NoApplications;
                orderOfAdmissionRepository.Update(order);
            }
            app.OrderOfAdmissionID = order.OrderID;
            app.OrderCompetitiveGroupItemID = model.DirectionID; //так в модели заполняется для удобства
            app.OrderEducationFormID = (short)model.EducationFormID;
            app.OrderEducationSourceID = (short)model.EducationSourceID;

            app.OrderCompetitiveGroupID = cgi.CompetitiveGroupID;
            //app.OrderCalculatedBenefitID = app.ApplicationSelectedCompetitiveGroup
            //    .Where(x => x.CompetitiveGroupID == cgi.CompetitiveGroupID)
            //    .Select(x => x.CalculatedBenefitID).FirstOrDefault();
            //app.OrderCalculatedRating = app.ApplicationSelectedCompetitiveGroup
            //    .Where(x => x.CompetitiveGroupID == cgi.CompetitiveGroupID)
            //    .Select(x => x.CalculatedRating).FirstOrDefault();

            // для целевиков ставим выбранную организацию ЦП
            app.SetCompetitiveGroupTarget();

            //если по какой-то причине сломалась клиентская валидация и пришли дурные данные, чтобы не страшно падать по SQL
            if (app.OrderEducationSourceID == 0 || app.OrderEducationFormID == 0 || app.OrderCompetitiveGroupItemID == 0)
                return new AjaxResultModel(AjaxResultModel.DataError);
            app.StatusID = ApplicationStatusType.InOrder;
            // если нет номера в заявлении, ставим его из модели (сейчас должен быть всегда)
            if (app.ApplicationNumber == null)
            {
                bool isUnique = dbContext.CheckApplicationNumberIsUnique(app, model.ApplicationNumber);
                if (!isUnique)
                    return new AjaxResultModel("Данный номер заявления ОО уже используется");
                app.ApplicationNumber = model.ApplicationNumber;
            }
            //нельзя включать в приказ если кол-во результатов вступительных испытаний не равно кол-ву вступительных испытаний
            //и заявление НЕ со льготой "Без ВИ"
            List<EntranceTestItemC> requiredEntranceTests =
                //app.ApplicationSelectedCompetitiveGroup.Any(
                //    x => x.CompetitiveGroupID.Equals(app.OrderCompetitiveGroupID))
                //    ? app.ApplicationSelectedCompetitiveGroup.FirstOrDefault(
                //        x => x.CompetitiveGroupID.Equals(app.OrderCompetitiveGroupID))
                //         .CompetitiveGroup.EntranceTestItemC.ToList()
                    //:
            new List<EntranceTestItemC>();
            List<ApplicationEntranceTestDocument> existedEntranceTests =
                app.ApplicationEntranceTestDocument.Where(x => x.ResultValue != null).ToList();
            List<int> requiredEntranceTestsSubjects = requiredEntranceTests.Select(x => x.EntranceTestItemID).ToList();
            List<int> existedEntranceTestsSubjects =
                existedEntranceTests.Select(x => x.EntranceTestItemID ?? 0).ToList();
            ApplicationEntranceTestDocument benefitEntranceTestDocument = app.ApplicationEntranceTestDocument.Any(x => x.BenefitID == 1) ? app.ApplicationEntranceTestDocument.FirstOrDefault(x => x.BenefitID == 1) : null;
            if (!app.IsForeignCitizen() &&
                (benefitEntranceTestDocument == null)
                && requiredEntranceTestsSubjects.Any(x => !existedEntranceTestsSubjects.Contains(x)))
                return new AjaxResultModel(String.Format("В заявлении №{0} не указаны необходимые результаты вступительных испытаний. Необходимо добавить результаты вступительных испытаний или исключить группу из заявления.", app.ApplicationNumber));
            //Нельзя включать в приказ, если результаты ВИ не удовлетворяют минимальным требованиям
            foreach (EntranceTestItemC requiredEntranceTest in requiredEntranceTests)
            {
                if (requiredEntranceTest.MinScore != null && requiredEntranceTest.MinScore > 0 &&
                    existedEntranceTests.Any(y => y.EntranceTestItemID == requiredEntranceTest.EntranceTestItemID) &&
                    requiredEntranceTest.MinScore > existedEntranceTests.FirstOrDefault(y => y.EntranceTestItemID == requiredEntranceTest.EntranceTestItemID).ResultValue)
                    return new AjaxResultModel("Минимальный балл, указанный для вступительного испытания, не должен быть меньше {0} (минимального количества баллов  по результатам ЕГЭ по предмету)".FormatWith(requiredEntranceTest.MinScore));
            }
            app.StatusID = ApplicationStatusType.InOrder;
            dbContext.SaveChanges();
            return new AjaxResultModel();
        }


        /// <summary>
        /// Выключаем список заявлений в приказ
        /// </summary>
        public static AjaxResultModel IncludeAllApplicationsInOrder(this EntrantsEntities dbContext, ApplicationIncludeInOrderViewModel model, int InstitutionID)
        {
            // вначале формируем общие данные для работы с заявлениями по имеющейся модели (cgi =  специальность, order, etc)
            var cgi = dbContext.CompetitiveGroupItem.Include(x => x.CompetitiveGroup).FirstOrDefault(x => x.CompetitiveGroupItemID == model.DirectionID);
            if (cgi == null)
                return new AjaxResultModel("Неверное направление!");

            var orderOfAdmissionRepository = new OrderOfAdmissionRepository();
            var orders = orderOfAdmissionRepository.Find(InstitutionID, cgi.CompetitiveGroup.CampaignID.GetValueOrDefault(), cgi.CompetitiveGroup.EducationLevelID.Value, model.EducationFormID, model.EducationSourceID);

            // TODO: Разобраться со stage
            int stage = 1;
            DateTime dateSecondStage = DateTime.MaxValue;
            if (model.IsForBeneficiary) stage = 0; //для льготников нет этапов
            if (stage > 0) //если есть этапы
            {
                if (DateTime.Now < dateSecondStage || model.IsBudgetForeigner) //ставим первый, если не дошла ещё дата второго
                    stage = 1;
                else
                {
                    if (model.OrderTypeSelection > 0) // если пользователь уже выбрал, ставим нужный тип
                    {
                        //if (model.OrderTypeSelection == 1)
                            stage = 1;
                        //else
                            //stage = 2;
                    }
                    else // если не выбрал, возвращаем информацию о этапах
                    {
                        OrderOfAdmission order1 = orders.Where(x => x.Stage.GetValueOrDefault() == 1).FirstOrDefault();
                        //OrderOfAdmission order2 = orders.Where(x => x.Stage.GetValueOrDefault() == 2).FirstOrDefault();
                        string order1Text = "Приказ 1 этапа. Не создан";
                        //string order2Text = "Приказ 2 этапа. Не создан";
                        if (order1 != null)
                            order1Text = String.Format("Приказ 1 этапа. Дата публикации: {0:dd.MM.yyyy} ({1})", order1.DatePublished, GetOrderStatusName(order1.OrderOfAdmissionStatusID));
                        //if (order2 != null)
                            //order2Text = String.Format("Приказ 2 этапа. Дата публикации: {0:dd.MM.yyyy} ({1})", order2.DatePublished, GetOrderStatusName(order2.OrderOfAdmissionStatusID));
                        //ещё раз спрашиваем)
                        return new AjaxResultModel
                        {
                            Data = new[] { "0", order1Text }
                        };
                    }

                }
            }

            //находим приказ
            OrderOfAdmission order = orders.FirstOrDefault(x => x.Stage == stage && x.IsForBeneficiary == model.IsForBeneficiary && x.IsForeigner == model.IsBudgetForeigner);

            //если приказ опубликован, а пользователь не уточнил что точно хочет, возращаем это
            if (order != null && order.OrderOfAdmissionStatusID == OrderOfAdmissionStatus.Published && !model.OrderForcePublished)
                return new AjaxResultModel { Data = new[] { "1" } };
            if (order == null) //создаём новый приказ
            {
                order = new OrderOfAdmission
                {
                    InstitutionID = InstitutionID,
                    OrderOfAdmissionStatusID = OrderOfAdmissionStatus.NoApplications,
                    DateCreated = DateTime.Now,
                    DateEdited = DateTime.Now,

                    CampaignID = cgi.CompetitiveGroup.CampaignID ?? 0,
                    EducationLevelID = cgi.CompetitiveGroup.EducationLevelID,

                    EducationFormID = (short)model.EducationFormID,
                    EducationSourceID = (short)model.EducationSourceID,

                    Stage = (short)stage,
                    IsForBeneficiary = model.IsForBeneficiary,
                    IsForeigner = model.IsBudgetForeigner
                };
                orderOfAdmissionRepository.Insert(order);
            }
            else
            {
                order.DateEdited = DateTime.Now;
                order.OrderOfAdmissionStatusID = OrderOfAdmissionStatus.NoApplications;
                orderOfAdmissionRepository.Update(order);
            }

            // теперь включаем все переданные заявления в приказ
            foreach (int appID in model.applicationIds)
            {
                Application app = dbContext.Application
                    .Include(x => x.CompetitiveGroup)
                    //.Include(x => x.ApplicationSelectedCompetitiveGroup)
                    //.Include(x => x.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup))
                    .Include(x => x.ApplicationSelectedCompetitiveGroupItem)
                    .Include(x => x.ApplicationSelectedCompetitiveGroupItem.Select(y => y.CompetitiveGroupItem))
                    .Include(x => x.ApplicationSelectedCompetitiveGroupTarget)
                    .Where(x => x.ApplicationID == appID).Single();

                app.OrderOfAdmissionID = order.OrderID;
                app.OrderCompetitiveGroupItemID = model.DirectionID; //так в модели заполняется для удобства
                app.OrderEducationFormID = (short)model.EducationFormID;
                app.OrderEducationSourceID = (short)model.EducationSourceID;

                app.OrderCompetitiveGroupID = cgi.CompetitiveGroupID;
                //app.OrderCalculatedBenefitID = app.ApplicationSelectedCompetitiveGroup
                //    .Where(x => x.CompetitiveGroupID == cgi.CompetitiveGroupID)
                //    .Select(x => x.CalculatedBenefitID).FirstOrDefault();
                //app.OrderCalculatedRating = app.ApplicationSelectedCompetitiveGroup
                    //.Where(x => x.CompetitiveGroupID == cgi.CompetitiveGroupID)
                    //.Select(x => x.CalculatedRating).FirstOrDefault();

                // для целевиков ставим выбранную организацию ЦП
                app.SetCompetitiveGroupTarget();

                //нельзя включать в приказ если кол-во результатов вступительных испытаний не равно кол-ву вступительных испытаний
                //и заявление НЕ со льготой "Без ВИ"
                List<EntranceTestItemC> requiredEntranceTests =
                    //app.ApplicationSelectedCompetitiveGroup.Any(
                    //    x => x.CompetitiveGroupID.Equals(app.OrderCompetitiveGroupID))
                    //    ? app.ApplicationSelectedCompetitiveGroup.FirstOrDefault(
                    //        x => x.CompetitiveGroupID.Equals(app.OrderCompetitiveGroupID))
                    //         .CompetitiveGroup.EntranceTestItemC.ToList()
                    //    : 
                        new List<EntranceTestItemC>();
                List<ApplicationEntranceTestDocument> existedEntranceTests =
                    app.ApplicationEntranceTestDocument.Where(x => x.ResultValue != null).ToList();
                List<int> requiredEntranceTestsSubjects = requiredEntranceTests.Select(x => x.EntranceTestItemID).ToList();
                List<int> existedEntranceTestsSubjects =
                    existedEntranceTests.Select(x => x.EntranceTestItemID ?? 0).ToList();
                ApplicationEntranceTestDocument benefitEntranceTestDocument = app.ApplicationEntranceTestDocument.Any(x => x.BenefitID == 1) ? app.ApplicationEntranceTestDocument.FirstOrDefault(x => x.BenefitID == 1) : null;
                if (!app.IsForeignCitizen() &&
                    (benefitEntranceTestDocument == null)
                    && requiredEntranceTestsSubjects.Any(x => !existedEntranceTestsSubjects.Contains(x)))
                    return new AjaxResultModel(String.Format("В заявлении №{0} не указаны необходимые результаты вступительных испытаний. Необходимо добавить результаты вступительных испытаний или исключить группу из заявления.", app.ApplicationNumber));
                //Нельзя включать в приказ, если результаты ВИ не удовлетворяют минимальным требованиям
                foreach (EntranceTestItemC requiredEntranceTest in requiredEntranceTests)
                {
                    if (requiredEntranceTest.MinScore != null && requiredEntranceTest.MinScore > 0 &&
                        existedEntranceTests.Any(y => y.EntranceTestItemID == requiredEntranceTest.EntranceTestItemID) &&
                        requiredEntranceTest.MinScore > existedEntranceTests.FirstOrDefault(y => y.EntranceTestItemID == requiredEntranceTest.EntranceTestItemID).ResultValue)
                        return new AjaxResultModel("Минимальный балл, указанный для вступительного испытания \"{1}\", не должен быть меньше {0} (минимального количества баллов  по результатам ЕГЭ по предмету)".FormatWith(requiredEntranceTest.MinScore, requiredEntranceTest.Subject.Name));
                }
                app.StatusID = ApplicationStatusType.InOrder;
            }

            // сохраняем изменения
            dbContext.SaveChanges();
            return new AjaxResultModel();
        }



        /// <summary>
        /// Исключаем заявление из приказа
        /// </summary>
        public static AjaxResultModel ExcludeApplicationFromOrder(this EntrantsEntities dbContext, int applicationID)
        {
            Application app = dbContext.Application.Single(x => x.ApplicationID == applicationID);

            if (!app.OrderOfAdmissionID.HasValue)
                return new AjaxResultModel("Заявление не включено в приказ");

            var orderOfAdmissionRepository = new OrderOfAdmissionRepository();
            var order = orderOfAdmissionRepository.Get(app.OrderOfAdmissionID.Value);
            order.DateEdited = DateTime.Now;
            order.OrderOfAdmissionStatusID = OrderOfAdmissionStatus.NoApplications;
            orderOfAdmissionRepository.Update(order);

            app.OrderOfAdmissionID = null;
            app.OrderCompetitiveGroupItemID = null;
            app.OrderCompetitiveGroupID = null;
            app.OrderCalculatedBenefitID = null;
            app.OrderCompetitiveGroupTargetID = null;
            app.OrderCalculatedRating = null;
            app.StatusID = ApplicationStatusType.Accepted;

            dbContext.SaveChanges();
            return new AjaxResultModel();
        }

        /// <summary>
        /// Публикуем/распубликуем приказ
        /// </summary>
        public static AjaxResultModel PublishOrder(this EntrantsEntities dbContext, int institutionID, int orderID,
                                                   bool isPublish)
        {
            OrderOfAdmissionRepository orderOfAdmissionRepository = new OrderOfAdmissionRepository();

            var order = orderOfAdmissionRepository.Get(orderID);
            if (order == null)
                return new AjaxResultModel("Приказ не найден");
            DateTime now = DateTime.Now;
            if (isPublish)
            {
                order.DatePublished = now;
                order.OrderOfAdmissionStatusID = OrderOfAdmissionStatus.Published;
                int[] applications =
                    dbContext.Application.Where(x => x.OrderOfAdmissionID == order.OrderID).Select(x => x.ApplicationID)
                        .ToArray();

                //пишем в историю информацию об опубликованных заявлениях

                foreach (var appID in applications)
                {

                    OrderOfAdmissionHistory historyItem = new OrderOfAdmissionHistory
                                                        {
                                                            OrderID = order.OrderID,
                                                            ApplicationID = appID,
                                                            DatePublished = now
                                                        };
                    orderOfAdmissionRepository.HistoryItem_Insert(historyItem);
                }
            }
            else
            {
                order.DateEdited = now;
                order.OrderOfAdmissionStatusID = OrderOfAdmissionStatus.NoApplications;
            }

            orderOfAdmissionRepository.Update(order);
            //возвращать нечего
            return new AjaxResultModel();
        }

        /// <summary>
        /// Список приказов
        /// </summary>
        public static AjaxResultModel GetOrderList(this EntrantsEntities dbContext, IncludeInOrderListViewModel model, int institutionID)
        {
            bool filterForeigner = model.SelectedOrderType == "999";
            var dtos = new List<IncludeInOrderListViewModel.OrderData>();

            // добавляем приказы для иностранцев
            if (model.SelectedOrderType == null || filterForeigner) // если фильтр не указан или указан только для иностранцев
            {
                DateTime foreignOrderDate;
                const string dateFormat = "dd.MM.yyyy";

                if (
                    !DateTime.TryParseExact(ConfigurationManager.AppSettings["CampaignForeignerOrderDate"], dateFormat,
                                            CultureInfo.InvariantCulture, DateTimeStyles.None,
                                            out foreignOrderDate))
                {
                    foreignOrderDate = DateTime.MinValue;
                }

                string foreignOrderDateString = foreignOrderDate.ToString(dateFormat);

                IEnumerable<OrderOfAdmission> orders;
                OrderOfAdmissionRepository orderOfAdmissionRepository = new OrderOfAdmissionRepository();
                if (model.SelectedCampaignID == 0)
                {
                    orders = orderOfAdmissionRepository.Find(institutionID);
                }
                else
                {
                    orders = orderOfAdmissionRepository.Find(institutionID, model.SelectedCampaignID);
                }
                orders = orders.Where(x => x.IsForeigner);

                foreach (var order in orders)
                {
                    int appCount = orderOfAdmissionRepository.CountApplications(order.OrderID);
                    if (appCount == 0)
                        continue;

                    var orderDto = new IncludeInOrderListViewModel.OrderData
                        {
                            ApplicationCount = appCount,
                            CampaignName = order.Campaign.Name,
                            OrderDate = foreignOrderDateString,
                            OrderID = order.OrderID,
                            // OrderTypeName = "",
                            StatusID = order.OrderOfAdmissionStatusID,
                            // StatusName = order.StatusText,
                            UID = order.UID ?? string.Empty
                        };

                    StringBuilder orderTypeNameBuilder = new StringBuilder();
                    orderTypeNameBuilder.Append(DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.Study, order.EducationFormID.GetValueOrDefault()));
                    orderTypeNameBuilder.Append(", ");
                    orderTypeNameBuilder.Append(DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, order.EducationLevelID.GetValueOrDefault()));
                    orderTypeNameBuilder.Append(", ");
                    orderTypeNameBuilder.Append("Иностранные граждане по межправительственным соглашениям");

                    orderDto.OrderTypeName = orderTypeNameBuilder.ToString();

                    orderDto.StatusName = GetOrderStatusName(order.OrderOfAdmissionStatusID); 

                    dtos.Add(orderDto);
                }
            }

            return new AjaxResultModel { Data = dtos.ToArray() };
        }

        /// <summary>
        /// Базовая загрузка данных для страницы с приказами (фильтры)
        /// </summary>
        public static IncludeInOrderListViewModel InitialFillIncludeInOrderListViewModel(this EntrantsEntities dbContext, int institutionID)
        {
            IncludeInOrderListViewModel model = new IncludeInOrderListViewModel();
            var camp = dbContext.Campaign.Where(x => x.InstitutionID == institutionID)
                .Select(x => new { ID = x.CampaignID, Name = x.Name }).ToList();
            camp.Insert(0, new { ID = 0, Name = "[Любая]" });
            model.Campaigns = camp.ToArray();

            var oTypes = (new[] { new { ID = "999", Name = "Иностранные граждане по межправительственным соглашениям" } }).ToList();
            oTypes.Insert(0, new { ID = "", Name = "[Любой]" });
            model.OrderTypes = oTypes.ToArray();

            return model;
        }

        public static void ClearFilterDataCache(int institutionId)
        {
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            cache.RemoveAllWithPrefix(
                "ApplicationListIncludeInOrder_FilterData_" + institutionId);
        }

        /// <summary>
        /// Базовая загрузка данных для страницы с приказами (фильтры)
        /// </summary>
        public static InstitutionApplicationListIncludeInOrderViewModel FillInstitutionApplicationListIncludeInOrder(this EntrantsEntities dbContext, InstitutionApplicationListIncludeInOrderViewModel model, int institutionID)
        {
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var filter = cache.Get<InstitutionApplicationListIncludeInOrderViewModel>("ApplicationListIncludeInOrder_FilterData_" + institutionID, null);
            if (filter != null)
                return filter;

            model.Order = dbContext.InitialFillIncludeInOrderListViewModel(institutionID);
            model.Directions =
                dbContext.AllowedDirections.Where(x => x.InstitutionID == institutionID)
                    .OrderBy(x => x.Direction.Name)
                    .Select(x => x.Direction.Name).Distinct().ToArray();
            model.CompetitiveGroups = dbContext.CompetitiveGroup.Where(x => x.InstitutionID == institutionID)
                    .OrderBy(x => x.Name).Select(x => x.Name).ToArray();
            model.Applications = new InstitutionApplicationListIncludeInOrderViewModel.ApplicationData[0];

            cache.Insert("ApplicationListIncludeInOrder_FilterData_" + institutionID, model);
            return model;
        }

        private static readonly int IncludeInOrderPageSize = AppSettings.Get("Search.PageSize", 25);

        /// <summary>
        /// Список заявлений в приказе
        /// </summary>
        public static AjaxResultModel GetInstitutionApplicationListIncludeInOrder(this EntrantsEntities dbContext, int institutionID, InstitutionApplicationListIncludeInOrderViewModel model, bool usePaging = true)
        {
            var tempQuery =
                dbContext.Application.Where(
                    a => a.OrderOfAdmissionID != null && a.InstitutionID == institutionID && a.OrderOfAdmissionID.Value == model.OrderID);
            Action<string, Expression<Func<Application, bool>>> filterFunction = (cr, func) =>
            {
                if (!String.IsNullOrWhiteSpace(cr))
                    tempQuery = tempQuery.Where(func);
            };
            model.TotalItemCount = tempQuery.Count();
            if (model.Filter != null)
            {
                filterFunction(model.Filter.ApplicationNumber, a => a.ApplicationNumber == model.Filter.ApplicationNumber);
                filterFunction(model.Filter.LastName, a => a.Entrant.LastName == model.Filter.LastName);
                filterFunction(model.Filter.FirstName, a => a.Entrant.FirstName == model.Filter.FirstName);
                filterFunction(model.Filter.MiddleName, a => a.Entrant.MiddleName == model.Filter.MiddleName);
                filterFunction(model.Filter.DocumentSeries, a => a.Entrant.EntrantDocument_Identity.DocumentSeries == model.Filter.DocumentSeries);
                filterFunction(model.Filter.DocumentNumber, a => a.Entrant.EntrantDocument_Identity.DocumentNumber == model.Filter.DocumentNumber);
                if (model.Filter.DateBegin.HasValue)
                    filterFunction("x", a => a.RegistrationDate >= model.Filter.DateBegin);
                if (model.Filter.DateEnd.HasValue)
                    filterFunction("x", a => a.RegistrationDate >= model.Filter.DateEnd);
                filterFunction(model.Filter.DirectionName,
                               a => a.OrderCompetitiveGroupItemID != null &&
                               a.CompetitiveGroup.Direction.Name == model.Filter.DirectionName);
                if (!String.IsNullOrWhiteSpace(model.Filter.CompetitiveGroupName))
                {
                    int[] allowedItems = dbContext.CompetitiveGroup
                            .Where(x => x.Name == model.Filter.CompetitiveGroupName && x.InstitutionID == institutionID)
                            .Select(x => x.CompetitiveGroupID).ToArray();
                    tempQuery = tempQuery.Where(a => a.OrderCompetitiveGroupID != null && allowedItems.Contains(a.OrderCompetitiveGroupID.Value));
                }
            }

            if (!model.SortID.HasValue)
                model.SortID = 1;

            switch (model.SortID.Value)
            {
                case 1:
                    tempQuery = tempQuery.OrderBy(x => x.ApplicationNumber);
                    break;
                case 2:
                    tempQuery = tempQuery.OrderBy(x => x.Entrant.LastName)
                        .ThenBy(x => x.Entrant.FirstName)
                        .ThenBy(x => x.Entrant.MiddleName);
                    break;
                case 3:
                    tempQuery = tempQuery.OrderBy(x => x.Entrant.EntrantDocument_Identity.DocumentSeries)
                        .ThenBy(x => x.Entrant.EntrantDocument_Identity.DocumentNumber);
                    break;
                case 4:
                    tempQuery = tempQuery.OrderBy(x => x.CompetitiveGroup.AdmissionItemType.DisplayOrder);
                    break;
                case 5:
                    tempQuery = tempQuery.OrderBy(x => x.AdmissionItemType.Name);
                    break;
                case 6:
                    tempQuery = tempQuery.OrderBy(x => x.CompetitiveGroup.Direction.Name);
                    break;
                case 7:
                    tempQuery = tempQuery.OrderBy(x => x.Benefit.Name);
                    break;
                case 8:
                    tempQuery = tempQuery.OrderBy(x => SqlFunctions.StringConvert(x.OrderCalculatedRating));
                    break;
                case -1:
                    tempQuery = tempQuery.OrderByDescending(x => x.ApplicationNumber);
                    break;
                case -2:
                    tempQuery = tempQuery.OrderByDescending(x => x.Entrant.LastName)
                        .ThenByDescending(x => x.Entrant.FirstName)
                        .ThenByDescending(x => x.Entrant.MiddleName);
                    break;
                case -3:
                    tempQuery = tempQuery.OrderByDescending(x => x.Entrant.EntrantDocument_Identity.DocumentSeries)
                        .ThenByDescending(x => x.Entrant.EntrantDocument_Identity.DocumentNumber);
                    break;
                case -4:
                    tempQuery = tempQuery.OrderByDescending(x => x.CompetitiveGroup.AdmissionItemType.DisplayOrder);
                    break;
                case -5:
                    tempQuery = tempQuery.OrderByDescending(x => x.AdmissionItemType.Name);
                    break;
                case -6:
                    tempQuery = tempQuery.OrderByDescending(x => x.CompetitiveGroup.Direction.Name);
                    break;
                case -7:
                    tempQuery = tempQuery.OrderByDescending(x => x.Benefit.Name);
                    break;
                case -8:
                    tempQuery = tempQuery.OrderByDescending(x => SqlFunctions.StringConvert(x.OrderCalculatedRating));
                    break;
            }

            if (!model.PageNumber.HasValue || model.PageNumber < 0)
                model.PageNumber = 0;
            model.TotalItemFilteredCount = tempQuery.Count();
            model.TotalPageCount = ((Math.Max(model.TotalItemFilteredCount, 1) - 1) / IncludeInOrderPageSize) + 1;
            if (usePaging)
                tempQuery = tempQuery.Skip(model.PageNumber.Value * IncludeInOrderPageSize).Take(IncludeInOrderPageSize);

            var q = (from a in tempQuery
                     select new
                     {
                         a.ApplicationID,
                         a.RegistrationDate,
                         a.ApplicationNumber,
                         a.UID,
                         a.Entrant.LastName,
                         a.Entrant.FirstName,
                         a.Entrant.MiddleName,
                         a.Entrant.EntrantDocument_Identity.DocumentSeries,
                         a.Entrant.EntrantDocument_Identity.DocumentNumber,
                         FormName = a.AdmissionItemType.Name,
                         LevelName = a.CompetitiveGroup.AdmissionItemType.Name,
                         a.OrderCompetitiveGroupItemID,
                         DirectionName = a.CompetitiveGroup.Direction.Name,
                         BenefitName = a.Benefit.ShortName,
                         a.OrderCalculatedRating,
                         StatusName = a.ApplicationStatusType.Name,
                         EducationalFormID = a.OrderEducationFormID,
                         EducationalSourceID = a.OrderEducationSourceID,
                         DocumentReceived = a.OriginalDocumentsReceived,
                         TargetOrganisationName = a.CompetitiveGroupTarget.Name,
                         CompetitiveGroupName = a.CompetitiveGroup != null ? a.CompetitiveGroup.Name : ""
                     }).ToArray();

            int orderStatus = 0;
            var order = new OrderOfAdmissionRepository().Get(model.OrderID);
            if (order != null)
            {
                orderStatus = order.OrderOfAdmissionStatusID;
            }

            var apps = q.Select(x => new InstitutionApplicationListIncludeInOrderViewModel.ApplicationData
            {
                ApplicationID = x.ApplicationID,
                ApplicationNumber = x.ApplicationNumber,
                ApplicationDate = x.RegistrationDate,
                ApplicationUID = x.UID,
                EntrantFIO = x.LastName + " " + x.FirstName + " " + x.MiddleName,
                EntrantDocData = x.DocumentSeries + " " + x.DocumentNumber,
                BallCount = x.OrderCalculatedRating ?? 0,
                BenefitName = x.BenefitName ?? "Нет",
                EducationForm = x.FormName,
                EducationLevel = x.LevelName,
                DirectionName = x.DirectionName,
                CanBeDeleted = orderStatus == OrderOfAdmissionStatus.NoApplications,
                StatusName = x.StatusName,
                EducationalSource = x.EducationalSourceID.HasValue ? DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.AdmissionType, x.EducationalSourceID.Value) : "",
                OriginalDocumentsRecieved = x.DocumentReceived,
                ViolationName = "",
                DenyDate = DateTime.MinValue,
                CompetitiveGroupName = x.CompetitiveGroupName,
                TargetOrganisationName = x.TargetOrganisationName
            }).ToArray();
            model.Applications = apps;

            dbContext.AddApplicationAccessToLog(apps.Select(x => new PersonalDataAccessLogger.AppData
                {
                    ApplicationNumber = x.ApplicationNumber,
                    ApplicationUID = x.ApplicationUID,
                    ApplicationID = x.ApplicationID,
                    ApplicationDate = x.ApplicationDate,
                }).ToArray(), "IncludeInOrderList", institutionID);

            return new AjaxResultModel { Data = model };
        }

        /// <summary>
        /// Сохраняем UID у приказа
        /// </summary>
        public static AjaxResultModel SaveOrderUid(this EntrantsEntities dbContext, int orderId, int institutionId, string uid)
        {
            OrderOfAdmissionRepository orderOfAdmissionRepository = new OrderOfAdmissionRepository();

            var order = orderOfAdmissionRepository.Get(orderId);
            if (order == null)
                return new AjaxResultModel().SetIsError("", "Приказ не найден.");

            if (orderOfAdmissionRepository.Find(institutionId, uid).Any(x => x.OrderID != orderId))
                return new AjaxResultModel().SetIsError("OrderUID", "UID дублируется.");

            order.UID = String.IsNullOrEmpty(uid) ? null : uid;

            orderOfAdmissionRepository.Update(order);

            return new AjaxResultModel("");
        }

        private static string GetOrderStatusName(int statusId)
        {
            string name = DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.OrderOfAdmissionStatus, statusId);
            if (String.IsNullOrEmpty(name))
            {
                name = new OrderOfAdmissionRepository().OrderStatus_GetName(statusId);
            }
            return name;
        }

        public static string GetOrderTypeName(int typeId)
        {
            string name = DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.OrderOfAdmissionType, typeId);
            if (String.IsNullOrEmpty(name))
            {
                name = new OrderOfAdmissionRepository().OrderType_GetName(typeId);
            }
            return name; 
        }
    }
}