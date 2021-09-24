using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.Helpers;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.Model.ApplicationPriorities;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing.Orders
{
    /// <summary>
    ///     Импорт заявлений в приказе
    /// </summary>
    public class OrderCollectionImporter : ObjectImporter
    {
        private readonly Application[] _apps;
        private readonly ImportEntities _importEntities;
        private readonly int _institutionID;
        private readonly OrderOfAdmissionItemDto[] _orderOfAdmissionDtos;

        public OrderCollectionImporter(StorageManager storageManager, OrderOfAdmissionItemDto[] orderOfAdmissionDtos) :
            base(storageManager)
        {
            _orderOfAdmissionDtos = orderOfAdmissionDtos;
            _importEntities = storageManager.DbObjectRepository.ImportEntities;
            _institutionID = StorageManager.DbObjectRepository.InstitutionId;
            //загружаем все заявления
            _apps = _importEntities.Application
                                   .Include(x => x.Entrant)
                                   .Include(x => x.CompetitiveGroup)
                                   .Include(x => x.CompetitiveGroupItem)
                                   .Include(x => x.ApplicationEntranceTestDocument)
                                   .Include(x => x.ApplicationSelectedCompetitiveGroup)
                                   .Include(x => x.ApplicationSelectedCompetitiveGroupTarget)
                                   .Include(x => x.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup))
                                   .Include(x => x.ApplicationSelectedCompetitiveGroupItem)
                                   .Include(
                                       x =>
                                       x.ApplicationSelectedCompetitiveGroupItem.Select(y => y.CompetitiveGroupItem))
                                   .Include(
                                       x =>
                                       x.ApplicationSelectedCompetitiveGroupItem.Select(
                                           y => y.CompetitiveGroupItem.CompetitiveGroupTargetItem))
                                   .Where(x => x.InstitutionID == _institutionID).ToArray();
        }

        public int SuccessfullyImportedOrders { get; set; }

        protected override void FindExcludedObjectsInDbForDelete()
        {
            // удаление объектов, существующих БД, но не указанных в импорте
        }

        //уже всё проверили, обновляемся
        protected override void FindInsertAndUpdate()
        {
            // отсюда запускаем импорт через OrderImporter
            // логика включения/исключения из приказа заявлений			
            // смотри раздел "Импорт приказов о зачислении" в документе Предложения по импорту
            // делать в стиле уже существующих импортеров: см., например, CompetitiveGroupImporter.
            var processedApps = new List<Application>();
            var processedCampaigns = new List<int>();
            //включаем заявление в приказ
            DateTime now = DateTime.Now;
            var _processedOrders = new List<OrderOfAdmission>();
            List<OrderOfAdmission> instOrders =
                _importEntities.OrderOfAdmission.Where(x => x.InstitutionID == _institutionID).ToList();
            foreach (OrderOfAdmissionItemDto appOrderDto in _orderOfAdmissionDtos)
            {
                OrderOfAdmissionItemDto dto = appOrderDto;
                DateTime registrationDate = dto.Application.RegistrationDateDate;
                //находим заявление по номеру и дате
                Application app =
                    _apps.FirstOrDefault(
                        x =>
                        x.ApplicationNumber == dto.Application.ApplicationNumber &&
                        x.RegistrationDate == registrationDate);

                if (ConflictStorage.HasConflictOrNotImported(dto))
                {
                    if (app != null) processedApps.Add(app);
                    continue;
                }

                if (app == null)
                {
                    ConflictStorage.AddNotImportedDto(appOrderDto,
                                                      ConflictMessages
                                                          .OrderOfAdmissionContainsRefOnNotImportedApplication);
                    continue;
                }

                DeletePackageHandler.CalculateApplicationRating(_importEntities, app.ApplicationID);

                int appEdFormID = appOrderDto.EducationFormID.To(0);
                int appFinSourceID = appOrderDto.FinanceSourceID.To(0);
                int appEdLevelID = appOrderDto.EducationLevelID.To(0);
                int stage = appOrderDto.Stage.To(0);
                bool isForBeneficiary = appOrderDto.IsBeneficiary.To(false);
                if (isForBeneficiary) stage = 0;
                bool isForeigner = appOrderDto.IsForeigner.GetValueOrDefault();
                //берём любую КГ из него (нужные параметры у всех идентичные)
                /*CompetitiveGroup anyCG =
                    app.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup).FirstOrDefault() ??
                    new CompetitiveGroup();*/
                //ищём подходящее направление КГ
                int intDirectionID = appOrderDto.DirectionID.To(0);

                CompetitiveGroupItem[] cgItems = null;
                bool implicitCompetitiveGroup = !string.IsNullOrEmpty(dto.CompetitiveGroupUID);
                string implicitCompetitiveGroupUID = dto.CompetitiveGroupUID;
                
                if (implicitCompetitiveGroup)
                {
                    var item = app.ApplicationSelectedCompetitiveGroupItem
                                 .Where(
                                     x => x.CompetitiveGroupItem.CompetitiveGroup.UID == implicitCompetitiveGroupUID)
                                 .Select(x => x.CompetitiveGroupItem).ToArray().FirstOrDefault();
                    
                    if (item != null)
                    {
                        cgItems = new[] {item};
                    }
                }
                else
                {
                    cgItems = app.ApplicationSelectedCompetitiveGroupItem
                                     .Where(
                                         x =>
                                         x.CompetitiveGroupItem.DirectionID == intDirectionID &&
                                         x.CompetitiveGroupItem.EducationLevelID == appEdLevelID)
                                     .Select(x => x.CompetitiveGroupItem)
                                     .ToArray();    
                }
                

                CompetitiveGroupItem cgItem = null;
                //не нашли - ошибка
                int[] allowedGroups = null;

                if (implicitCompetitiveGroup)
                {
                    allowedGroups = app.ApplicationSelectedCompetitiveGroup
                                       .Where(
                                           x => x.CompetitiveGroup.UID == implicitCompetitiveGroupUID)
                                       .Select(x => x.CompetitiveGroupID).ToArray();
                }
                else
                {
                    allowedGroups = app.ApplicationSelectedCompetitiveGroup
                                       .Where(
                                           x =>
                                           (x.CalculatedBenefitID == 1 ||
                                            x.CalculatedBenefitID == 4) &&
                                           x.CompetitiveGroup.Course == 1)
                                       .Select(x => x.CompetitiveGroupID).ToArray();
                }

                if (cgItems == null || cgItems.Length == 0)
                {
                    if (implicitCompetitiveGroup)
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.OrderOfAdmissionImplicitCompetitiveGroupUidNotFound, appOrderDto.UID, implicitCompetitiveGroupUID);
                    }
                    else
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.OrderOfAdmissionWithWrongDirection);
                    }
                    continue;
                }

                if (cgItems.Length == 1)
                {
                    cgItem = cgItems[0];

                    if (implicitCompetitiveGroup && !allowedGroups.Contains(cgItem.CompetitiveGroupID))
                    {
                        ConflictStorage.AddNotImportedDto(dto, ConflictMessages.OrderOfAdmissionMismatchImplicitCompetitiveGroup);
                        continue;
                    }
                    
                    if (
                        (
                            !allowedGroups.Contains(cgItem.CompetitiveGroupID)
                            || (
                                   appFinSourceID != 14
                        /*TODO: добавил целевиков, вроде как бред, надо разбираться, вероятно придётся убирать*/
                                   && appFinSourceID != 16
                        //Льготники
                                   && appFinSourceID != 20
                               )
                        )
                        && isForBeneficiary
                        && !implicitCompetitiveGroup
                        ) //budget
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto,
                                                          ConflictMessages.OrderOfAdmissionBeneficiaryCannotInclude);
                        continue;
                    }
                }
                else //нашли более одного, пытаемся выбрать правильное нужное
                {
                    cgItem = null;
                    //угадайка
                    //если для льготинков - проверяем, есть ли льготы
                    if (isForBeneficiary)
                    {
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.O)
                            cgItem = cgItems.FirstOrDefault(
                                x => x.NumberBudgetO > 0 && allowedGroups.Contains(x.CompetitiveGroupID));
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.OZ)
                            cgItem = cgItems.FirstOrDefault(
                                x => x.NumberBudgetOZ > 0 && allowedGroups.Contains(x.CompetitiveGroupID));
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.Z)
                            cgItem = cgItems.FirstOrDefault(
                                x => x.NumberBudgetZ > 0 && allowedGroups.Contains(x.CompetitiveGroupID));
                        //если не нашли подходящую, берём первую попавшуюся
                        if (cgItem == null)
                            cgItem = cgItems[0];
                        if (!allowedGroups.Contains(cgItem.CompetitiveGroupID) || appFinSourceID != 14) //budget
                        {
                            ConflictStorage.AddNotImportedDto(appOrderDto,
                                                              ConflictMessages
                                                                  .OrderOfAdmissionBeneficiaryCannotInclude);
                            continue;
                        }
                    }
                    else
                    {
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.O)
                            cgItems = cgItems.Where(x => x.NumberBudgetO > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.OZ)
                            cgItems = cgItems.Where(x => x.NumberBudgetOZ > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.Z)
                            cgItems = cgItems.Where(x => x.NumberBudgetZ > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Paid && appEdFormID == EDFormsConst.O)
                            cgItems = cgItems.Where(x => x.NumberPaidO > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Paid && appEdFormID == EDFormsConst.OZ)
                            cgItems = cgItems.Where(x => x.NumberPaidOZ > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Paid && appEdFormID == EDFormsConst.Z)
                            cgItems = cgItems.Where(x => x.NumberPaidZ > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Target && appEdFormID == EDFormsConst.O)
                            cgItems =
                                cgItems.Where(x => x.CompetitiveGroupTargetItem.Sum(y => y.NumberTargetO) > 0)
                                       .ToArray();
                        if (appFinSourceID == EDSourceConst.Target && appEdFormID == EDFormsConst.OZ)
                            cgItems =
                                cgItems.Where(x => x.CompetitiveGroupTargetItem.Sum(y => y.NumberTargetOZ) > 0)
                                       .ToArray();
                        if (appFinSourceID == EDSourceConst.Target && appEdFormID == EDFormsConst.Z)
                            cgItems =
                                cgItems.Where(x => x.CompetitiveGroupTargetItem.Sum(y => y.NumberTargetZ) > 0)
                                       .ToArray();

                        foreach (CompetitiveGroupItem cgi in cgItems)
                        {
                            //нельзя включать в приказ если кол-во результатов вступительных испытаний не равно кол-ву вступительных испытаний
                            //и заявление НЕ со льготой "Без ВИ"
                            List<EntranceTestItemC> requiredEntranceTests =
                                cgi.CompetitiveGroup.EntranceTestItemC.ToList();
                            List<ApplicationEntranceTestDocument> existedEntranceTests =
                                app.ApplicationEntranceTestDocument.Where(x => x.ResultValue != null).ToList();
                            List<int> requiredEntranceTestsSubjects =
                                requiredEntranceTests.Select(x => x.EntranceTestItemID).ToList();
                            List<int> existedEntranceTestsSubjects =
                                existedEntranceTests.Select(x => x.EntranceTestItemID ?? 0).ToList();
                            ApplicationEntranceTestDocument benefitEntranceTestDocument =
                                app.ApplicationEntranceTestDocument.Any(x => x.BenefitID == 1)
                                    ? app.ApplicationEntranceTestDocument.FirstOrDefault(x => x.BenefitID == 1)
                                    : null;
                            if ((benefitEntranceTestDocument == null)
                                &&
                                !requiredEntranceTestsSubjects.Any(x => !existedEntranceTestsSubjects.Contains(x)))
                            {
                                cgItem = cgi;
                                break;
                            }
                        }
                        //если не нашли подходящую, берём первую попавшуюся
                        if (cgItem == null)
                            cgItem = cgItems[0];
                    }
                }
                processedCampaigns.Add(cgItem.CompetitiveGroup.CampaignID ?? 0);
                int? campaignID = cgItem.CompetitiveGroup.CampaignID; //у всех одинаковый
                short course = cgItem.CompetitiveGroup.Course; //у всех одинаковый

                int orderFinSource = appFinSourceID;
                if (orderFinSource == 20)//Льготников на бюджетный приказ
                {
                    orderFinSource = 14;
                }
                //находим подходящий приказ
                OrderOfAdmission dbOrder = instOrders
                    .FirstOrDefault(x => x.InstitutionID == _institutionID
                                         && x.EducationFormID == appEdFormID
                                         && x.EducationSourceID == orderFinSource
                                         && x.CampaignID == campaignID //у всех одинаковы
                                         && x.Course == course
                                         && x.EducationLevelID == appEdLevelID
                                         && x.Stage == stage
                                         && x.IsForBeneficiary == isForBeneficiary
                                         && x.IsForeigner == isForeigner);

                //нет приказа - создаём его
                if (dbOrder == null)
                {
                    dbOrder = new OrderOfAdmission
                        {
                            InstitutionID = _institutionID,
                            OrderStatus = 1, //Edited
                            DateCreated = now,
                            CampaignID = campaignID ?? 0,
                            Course = course,
                            EducationLevelID = (short) appEdLevelID,
                            EducationFormID = (short) appEdFormID,
                            EducationSourceID = (short)orderFinSource,
                            Stage = (short) stage,
                            IsForBeneficiary = isForBeneficiary,
                            UID = appOrderDto.UID,
                            IsForeigner = isForeigner
                        };

                    _importEntities.OrderOfAdmission.AddObject(dbOrder);
                    instOrders.Add(dbOrder);
                }

                //приказ обязан быть в статусе рпдектируется
                dbOrder.DateEdited = now;
                dbOrder.OrderStatus = 1; //Edited
                
                //должна найтись только одна группа с указанными параметрами
                /*CompetitiveGroupItem competitiveGroupItem = app.ApplicationSelectedCompetitiveGroupItem
                                                               .Where(
                                                                   x =>
                                                                   x.CompetitiveGroupItem.DirectionID == intDirectionID &&
                                                                   x.CompetitiveGroupItem.EducationLevelID ==
                                                                   appEdLevelID)
                                                               .Select(x => x.CompetitiveGroupItem).First();*/
                //присваивем приказные данные для заявления
                app.OrderOfAdmission = dbOrder;
                app.OrderCompetitiveGroupItemID = cgItem.CompetitiveGroupItemID;
                app.OrderEducationFormID = appOrderDto.EducationFormID.To<short>(0);
                app.OrderEducationSourceID = appOrderDto.FinanceSourceID.To<short>(0);
                app.OrderCompetitiveGroupID = cgItem.CompetitiveGroupID;
                app.OrderCalculatedRating = app.ApplicationSelectedCompetitiveGroup
                                               .Where(
                                                   x => x.CompetitiveGroupID == cgItem.CompetitiveGroupID)
                                               .Select(x => x.CalculatedRating).FirstOrDefault();
                app.OrderCalculatedBenefitID = app.ApplicationSelectedCompetitiveGroup
                                                  .Where(
                                                      x =>
                                                      x.CalculatedBenefitID != null)
                                                  .Select(x => x.CalculatedBenefitID).FirstOrDefault();

                //для ЦП проставляем что надо
                app.SetImportCompetitiveGroupTarget();

                app.StatusID = ApplicationStatusType.InOrder;
                processedApps.Add(app);
                if (_processedOrders.All(x => x.OrderID != dbOrder.OrderID))
                    _processedOrders.Add(dbOrder);
                SuccessfullyImportedOrders++;
            }

            #region #28929 Изменение механизма импорта приказов сервисом автоматизированного взаимодействие
            ////заявления, которые нужно исключить из приказа
            ////не трогаем заявления из других приказов
            //IEnumerable<Application> affected = _apps
            //    .Where(
            //        x =>
            //        processedCampaigns.Contains(
            //            x.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup.CampaignID)
            //             .FirstOrDefault() ?? 0));

            ////исключаем те, которые не передали из приказа
            //foreach (Application app in affected)
            //{
            //    //процессеные исключаем
            //    if (processedApps.Any(x => x.ApplicationID == app.ApplicationID)) continue;
            //    if (ConflictStorage.GetConflictApplications().Contains(app.GetApplicationShortRef())) continue;
            //    if (app.StatusID == ApplicationStatusType.InOrder)
            //    {
            //        app.OrderOfAdmission = null;
            //        app.OrderOfAdmissionID = null;
            //        app.OrderCompetitiveGroupItemID = null;
            //        app.OrderCompetitiveGroupID = null;
            //        app.OrderCalculatedBenefitID = null;
            //        app.OrderCompetitiveGroupTargetID = null;
            //        app.OrderCalculatedRating = 0;
            //        app.StatusID = ApplicationStatusType.Accepted;
            //    }
            //} 
            #endregion

            var sw = new Stopwatch();
            sw.Start();
            //пишем в базу
            _importEntities.SaveChanges();
            sw.Stop();
            Log.DebugFormat("Imported {0} orders. Total: {1} sec.", SuccessfullyImportedOrders, sw.Elapsed.TotalSeconds);
            sw.Restart();
            //публикуем приказы
            foreach (OrderOfAdmission orderDto in _processedOrders)
            {
                OrderOfAdmission dbOrder = orderDto;
                dbOrder.DatePublished = now;
                dbOrder.OrderStatus = 2; //Published
                int[] applications =
                    _importEntities.Application.Where(x => x.OrderOfAdmissionID == dbOrder.OrderID)
                                   .Select(x => x.ApplicationID)
                                   .ToArray();
                //пишем историю
                foreach (int appID in applications)
                {
                    var history = new OrderOfAdmissionHistory
                        {OrderID = dbOrder.OrderID, ApplicationID = appID, DatePublished = now};
                    _importEntities.OrderOfAdmissionHistory.AddObject(history);
                }
            }

            _importEntities.SaveChanges();
            sw.Stop();
            Log.DebugFormat("Imported {0} orders. History recorded: {1} sec.", SuccessfullyImportedOrders,
                            sw.Elapsed.TotalSeconds);
        }

        /// <summary>
        ///     Проверка формы обучения и источника финансирования
        /// </summary>
        private static bool ValidateAppState(int formID, int sourceID)
        {
            if (!new[] {10, 11, 12}.Contains(formID))
                return false;
            if (!new[] {14, 15, 16, 20}.Contains(sourceID))
                return false;
            return true;
        }

        /// <summary>
        ///     Проверка на количество мест в КГ и приёме
        /// </summary>
        private static AppNumberState ValiddateAppNumbers(Application app, CompetitiveGroupItem cgItem, int formID,
                                                          int sourceID, int appCounts)
        {
            Func<int, int, bool, int, int, AppNumberState> check =
                (appFormID, appSourceID, isSelected, cgNumbers, appCountNumbers) =>
                    {
                        if (formID == appFormID && sourceID == appSourceID)
                        {
                            if (!isSelected) return AppNumberState.NotSelected;
                            if (cgNumbers < appCountNumbers) return AppNumberState.ExceedPlaces;
                            return AppNumberState.Fine;
                        }

                        return AppNumberState.NotAffected;
                    };

            var priorities = new List<ApplicationCompetitiveGroupItem>();

            using (var prioritiesContext = new ImportEntities())
            {
                priorities = prioritiesContext.ApplicationCompetitiveGroupItem.Where(x => x.ApplicationId == app.ApplicationID).ToList();
            }

            Func<List<ApplicationCompetitiveGroupItem>, int, int, bool> IsNecessary =
                (selectedpriorities, edForm, edSource) =>
                {
                    return priorities.Any(x => x.EducationFormId == edForm && x.EducationSourceId == edSource && x.Priority.HasValue);
                };

            AppNumberState r = check(EDFormsConst.O, EDSourceConst.Budget, IsNecessary(priorities, EDFormsConst.O, EDSourceConst.Budget) , cgItem.NumberBudgetO,
                                     appCounts);
            if (r != AppNumberState.NotAffected) return r;
            r = check(EDFormsConst.O, EDSourceConst.Paid, IsNecessary(priorities, EDFormsConst.O, EDSourceConst.Paid), cgItem.NumberPaidO, appCounts);
            if (r != AppNumberState.NotAffected) return r;

            r = check(EDFormsConst.OZ, EDSourceConst.Budget, IsNecessary(priorities, EDFormsConst.OZ, EDSourceConst.Budget), cgItem.NumberBudgetOZ, appCounts);
            if (r != AppNumberState.NotAffected) return r;
            r = check(EDFormsConst.OZ, EDSourceConst.Paid, IsNecessary(priorities, EDFormsConst.OZ, EDSourceConst.Paid), cgItem.NumberPaidOZ, appCounts);
            if (r != AppNumberState.NotAffected) return r;

            r = check(EDFormsConst.Z, EDSourceConst.Budget, IsNecessary(priorities, EDFormsConst.Z, EDSourceConst.Budget), cgItem.NumberBudgetZ, appCounts);
            if (r != AppNumberState.NotAffected) return r;
            r = check(EDFormsConst.Z, EDSourceConst.Paid, IsNecessary(priorities, EDFormsConst.Z, EDSourceConst.Paid), cgItem.NumberPaidZ, appCounts);
            if (r != AppNumberState.NotAffected) return r;

            r = check(EDFormsConst.O, EDSourceConst.Target, IsNecessary(priorities, EDFormsConst.O, EDSourceConst.Target),
                      cgItem.CompetitiveGroupTargetItem.Sum(x => x.NumberTargetO), appCounts);
            if (r != AppNumberState.NotAffected) return r;
            r = check(EDFormsConst.OZ, EDSourceConst.Target, IsNecessary(priorities, EDFormsConst.OZ, EDSourceConst.Target),
                      cgItem.CompetitiveGroupTargetItem.Sum(x => x.NumberTargetOZ), appCounts);
            if (r != AppNumberState.NotAffected) return r;
            r = check(EDFormsConst.Z, EDSourceConst.Target, IsNecessary(priorities, EDFormsConst.Z, EDSourceConst.Target),
                      cgItem.CompetitiveGroupTargetItem.Sum(x => x.NumberTargetZ), appCounts);
            if (r != AppNumberState.NotAffected) return r;
            //какие-то левые числа, говорим, что всё плохо
            return AppNumberState.ExceedPlaces;
        }

        private bool ValidateAppExams(Application app)
        {
            //#22788 Описание: В приказ включен абитуриент, у которого результат экзамена не совпадает с результатом в свидетельстве ЕГЭ или данные об этом предмете в свидетельстве отсутствуют. Ошибка не возникает
            var docsWithResult = app.ApplicationEntranceTestDocument
                                    .Where(
                                        x =>
                                        x.SourceID == EntranceTestSource.EgeDocumentSourceId &&
                                        x.EntrantDocumentID != null)
                                    .Select(
                                        x =>
                                        new
                                            {
                                                x.ResultValue,
                                                x.SubjectID,
                                                x.EntrantDocument.DocumentSpecificData,
                                                x.EntrantDocument.DocumentTypeID
                                            }).ToArray();
            foreach (var data in docsWithResult)
            {
                var data1 = data;

                var model = (BaseDocumentViewModel)
                            new JavaScriptSerializer().Deserialize(data.DocumentSpecificData,
                                                                   EntrantDocumentExtensions
                                                                       .GetDocumentViewModelType(
                                                                           data.DocumentTypeID));
                var egeModel = model as EGEDocumentViewModel;
                if (egeModel == null) //не тот тип документа. явно неправильно
                    return false;
                decimal egeDetailsValue =
                    egeModel.Subjects.Where(x => x.SubjectID == data1.SubjectID).Select(x => x.Value).FirstOrDefault();

                if (data.ResultValue.HasValue && data.ResultValue != egeDetailsValue)
                    //не совпадают или отсутствуют результаты
                {
                    // если иностранный язык, ищем в свидетельстве любой подходящий c данным результатом
                    Subject subject = DbObjectRepository.GetSubject(data1.SubjectID.To(0));
                    if (subject != null && subject.Name == LanguageSubjects.ForeignLanguage)
                    {
                        egeDetailsValue = egeModel.Subjects.Where(x =>
                            {
                                Subject subj = DbObjectRepository.GetSubject(x.SubjectID);
                                return subj != null &&
                                       subj.Name.StartsWith(LanguageSubjects.ForeignLanguagePrefix,
                                                            StringComparison.Ordinal) &&
                                       x.Value == data1.ResultValue;
                            }).Select(x => x.Value).FirstOrDefault();
                    }
                }

                if (data.ResultValue.HasValue && data.ResultValue != egeDetailsValue)
                    //не совпадают или отсутствуют результаты
                    return false;
            }

            return true;
        }


        protected override bool CanUpdate()
        {
            return true;
        }

        protected override void ProcessChildren(bool isParentConflict)
        {
            // импорт дочерних объектов выполняется в FindInsertAndUpdate
        }

        protected override BaseDto GetDtoObject()
        {
            return null;
        }

        /// <summary>
        ///     Проверка целостности заявлений в приказе
        /// </summary>
        protected override void CheckIntegrity()
        {
            var processedApplications = new List<ApplicationRef>();
            var appCounts =
                new List<Tuple<Application, OrderOfAdmissionItemDto, CompetitiveGroupItem>>();
            List<CampaignDate> campaignDates =
                _importEntities.CampaignDate.Where(x => x.Campaign.InstitutionID == _institutionID).ToList();
            //бежим по всем заявлениям
            foreach (OrderOfAdmissionItemDto appOrderDto in _orderOfAdmissionDtos)
            {
                //проверяем на целостность типы приказов
                DateTime registrationDate = appOrderDto.Application.RegistrationDateDate;
                Application app =
                    _apps.FirstOrDefault(x => x.ApplicationNumber == appOrderDto.Application.ApplicationNumber
                                              && x.RegistrationDate == registrationDate);

                if (app == null)
                {
                    ConflictStorage.AddNotImportedDto(appOrderDto,
                                                      ConflictMessages
                                                          .OrderOfAdmissionContainsRefOnNotImportedApplication);
                    continue;
                }

                //1.	В приказ передаваться должны только заявления в статусе «Принятые»
                if (app.StatusID.To(0) != ApplicationStatusType.Accepted && app.StatusID.To(0) != ApplicationStatusType.InOrder)
                {
                    ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.ApplicationCannotIncludeInOrderForAppStatus);
                }

                int appEdFormID = appOrderDto.EducationFormID.To(0);
                int appFinSourceID = appOrderDto.FinanceSourceID.To(0);
                int appEdLevelID = appOrderDto.EducationLevelID.To(0);
                int stage = appOrderDto.Stage.To(0);
                bool isForBeneficiary = appOrderDto.IsBeneficiary.To(false);
                if (isForBeneficiary) stage = 0;

                #region Иностранцы
                bool isForeigner = appOrderDto.IsForeigner.GetValueOrDefault();
                if (isForeigner)
                {
                    if (isForBeneficiary)
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.ApplicationUnableToIncludeInBeneficiaryOrder);
                    }

                    if (!string.IsNullOrEmpty(appOrderDto.Stage))
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.ApplicationInvalidStage);
                    }

                    if (appFinSourceID != EDSourceConst.Budget || app.Entrant == null || app.Entrant.EntrantDocument_Identity == null || 
                        string.IsNullOrEmpty(app.Entrant.EntrantDocument_Identity.DocumentSpecificData))
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.ApplicationForeignEntrantUnacceptable);
                    }
                    else if (!string.IsNullOrEmpty(app.Entrant.EntrantDocument_Identity.DocumentSpecificData))
                    {
                        string data = app.Entrant.EntrantDocument_Identity.DocumentSpecificData;
                        var serializer = new JavaScriptSerializer();
                        IdentityDocumentDto doc = serializer.Deserialize<IdentityDocumentDto>(data);
                        if (doc.NationalityTypeID.To(1) == 1 || DbObjectRepository.IsRussianDoc(doc.IdentityDocumentTypeID.To(0)))
                        {
                            ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.ApplicationForeignEntrantUnacceptable);    
                        }
                    }
                }
                #endregion

                //берём любую КГ (нужные параметры одинаковые у всех)
                CompetitiveGroup anyCG =
                    app.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup).FirstOrDefault() ??
                    new CompetitiveGroup();
                int? campaignID = anyCG.CampaignID; //у всех одинаковый
                short course = anyCG.Course; //у всех одинаковый
                bool hasRelatedDate = campaignDates
                    .Any(x => x.EducationFormID == appEdFormID
                              && ((x.EducationSourceID == appFinSourceID) || (appFinSourceID == EDSourceConst.Quota && isForBeneficiary))
                              && x.CampaignID == campaignID
                              && x.Course == course
                              && x.EducationLevelID == appEdLevelID
                              && x.Stage == stage // || isForBeneficiary)
                              && x.IsActive);

                //нет даты кампании. Нельзя включать в приказ
                if (!hasRelatedDate && !isForeigner && !isForBeneficiary)
                {
                    //та же проверка, но без этапа
                    bool hasRelatedDateWithoutStage = campaignDates
                        .Any(x => x.EducationFormID == appEdFormID
                                  && ((x.EducationSourceID == appFinSourceID) || (appFinSourceID == EDSourceConst.Quota && isForBeneficiary))
                                  && x.CampaignID == campaignID
                                  && x.Course == course
                                  && x.EducationLevelID == appEdLevelID
                                  && x.IsActive);

                    //есть дата без этапа и этап передали -> неправильный этап
                    if (hasRelatedDateWithoutStage && stage != 0)
                        ConflictStorage.AddNotImportedDto(appOrderDto,
                                                          ConflictMessages.OrderOfAdmissionWithIncorrectStage);
                        //есть дата без этапа и этапа не передали -> отсутствует этап
                    else if (hasRelatedDateWithoutStage && stage == 0)
                        ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.OrderOfAdmissionWithMissingStage);
                        //нет даты и без этапа -> всё не так
                    else
                        ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.OrderOfAdmissionWithInvalidType);

                    continue;
                }

                OrderOfAdmissionItemDto dto = appOrderDto;

                //уже есть в обработанных - дубль
                if (processedApplications.Any(
                    x =>
                    x.ApplicationNumber == dto.Application.ApplicationNumber &&
                    x.RegistrationDateDate == dto.Application.RegistrationDateDate))
                {
                    ConflictStorage.AddNotImportedDto(dto, ConflictMessages.OrderOfAdmissionWithDuplicateApp);
                    continue;
                }

                /* При включении в приказ заявлений, для которых не предоставлены оригиналы  документов о 
                 * предыдущем уровне образования, должна производиться проверка на наличие прикрепленных 
                 * к заявлениям справок об обучении в другом ВУЗе   */
                /* Короче если нет оригиналов и нет документа StudentDocument - ругаемся */
                #region OriginalsChecks
                if (!app.OriginalDocumentsReceived && app.Institution.InstitutionTypeID == 2 && dto.FinanceSourceID == EDSourceConst.Budget.ToString())
                {
                    ConflictStorage.AddNotImportedDto(dto, ConflictMessages.OriginalDocumentsRequired);
                    continue;
                }
                if (!app.OriginalDocumentsReceived && dto.FinanceSourceID == EDSourceConst.Budget.ToString() && app.ApplicationEntrantDocument.All(c =>
                                                                                         c.EntrantDocument
                                                                                          .DocumentTypeID !=
                                                                                         (int)
                                                                                         EntrantDocumentType
                                                                                             .StudentDocument))
                {
                    ConflictStorage.AddNotImportedDto(dto, ConflictMessages.StudentDocumentRequired);
                    continue;
                }
                // Эта проверка по идее закрывает остатки недавней дыры https://redmine.armd.ru/issues/20116 ещё раз выбирая даты документов об образовании, даже если проставлены факты сдачи
#warning: chaplygin added https://redmine.armd.ru/issues/20116 в идеале этот код никогда не будет вызван
                /*var docs = app.ApplicationEntrantDocument.Select(x => new {isReceived = x.OriginalReceivedDate, DocType = x.EntrantDocument.DocumentTypeID});
			    bool ReceivedEduDocument = false;
			    var eduDocs = new List<int>();
                eduDocs.Add((int)EntrantDocumentType.BasicDiplomaDocument);
                eduDocs.Add((int)EntrantDocumentType.SchoolCertificateDocument);
                eduDocs.Add((int)EntrantDocumentType.HighEduDiplomaDocument);
                eduDocs.Add((int)EntrantDocumentType.MiddleEduDiplomaDocument);
                eduDocs.Add((int)EntrantDocumentType.IncomplHighEduDiplomaDocument);
                eduDocs.Add((int)EntrantDocumentType.EduCustomDocument);
                
                foreach (var doc in docs)
			    {
			        ReceivedEduDocument = (doc.isReceived != null && eduDocs.Contains(doc.DocType));
			    }

                if (!ReceivedEduDocument)
                {
                    ConflictStorage.AddNotImportedDto(dto, ConflictMessages.OriginalDocumentsRequired);
                    continue;
                }*/
                #endregion OriginalsChecks

                

                processedApplications.Add(dto.Application);

                //такого не может быть, но надо же проверить
                if (anyCG.CompetitiveGroupID == 0)
                {
                    ConflictStorage.AddNotImportedDto(appOrderDto,
                                                      ConflictMessages.OrderOfAdmissionWithAbsentCompetitiveGroup);
                    continue;
                }


                bool hasInvalidCampaigns = DbObjectRepository.Campaigns.Where(x => x.CampaignID == campaignID)
                                                             .Select(x => x.StatusID).Where(x => x != 1).Any();
                //идёт набор
                if (hasInvalidCampaigns)
                {
                    ConflictStorage.AddNotImportedDto(dto,
                                                      ConflictMessages.ApplicationCannotIncludeInOrderForCampaignStatus);
                    continue;
                }

                //ищём подходящее направление КГ
                int intDirectionID = appOrderDto.DirectionID.To(0);
                CompetitiveGroupItem[] cgItems = app.ApplicationSelectedCompetitiveGroupItem
                                                    .Where(
                                                        x =>
                                                        x.CompetitiveGroupItem.DirectionID == intDirectionID &&
                                                        x.CompetitiveGroupItem.EducationLevelID == appEdLevelID)
                                                    .Select(x => x.CompetitiveGroupItem)
                                                    .ToArray();
                CompetitiveGroupItem cgItem;
                //не нашли - ошибка
                IEnumerable<int> allowedGroups = app.ApplicationSelectedCompetitiveGroup
                                                            .Where(
                                                                x =>
                                                                (x.CalculatedBenefitID == 1 ||
                                                                 x.CalculatedBenefitID == 4) &&
                                                                x.CompetitiveGroup.Course == 1)
                                                            .Select(x => x.CompetitiveGroupID);
                if (cgItems.Length == 0)
                {
                    ConflictStorage.AddNotImportedDto(appOrderDto, ConflictMessages.OrderOfAdmissionWithWrongDirection);
                    continue;
                }
                if (cgItems.Length == 1)
                {   
                    cgItem = cgItems[0];
                    if (
                            (
                                !allowedGroups.Contains(cgItem.CompetitiveGroupID) 
                                || (
                                    appFinSourceID != 14 
                                    /*TODO: добавил целевиков, вроде как бред, надо разбираться, вероятно придётся убирать*/
                                    && appFinSourceID != 16
                                    //Льготники
                                    && appFinSourceID != 20
                                    )
                            )
                        && isForBeneficiary
                       ) //budget
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto,
                                                          ConflictMessages.OrderOfAdmissionBeneficiaryCannotInclude);
                        continue;
                    }
                }
                else //нашли более одного, пытаемся выбрать правильное нужное
                {
                    cgItem = null;
                    //угадайка
                    //если для льготников - проверяем, есть ли льготы
                    if (isForBeneficiary)
                    {
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.O)
                                cgItem = cgItems.FirstOrDefault(
                                        x => x.NumberBudgetO > 0 && allowedGroups.Contains(x.CompetitiveGroupID));
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.OZ)
                                cgItem = cgItems.FirstOrDefault(
                                        x => x.NumberBudgetOZ > 0 && allowedGroups.Contains(x.CompetitiveGroupID));
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.Z)
                                cgItem = cgItems.FirstOrDefault(
                                        x => x.NumberBudgetZ > 0 && allowedGroups.Contains(x.CompetitiveGroupID));
                        //если не нашли подходящую, берём первую попавшуюся
                        if (cgItem == null)
                            cgItem = cgItems[0];
                        if (!allowedGroups.Contains(cgItem.CompetitiveGroupID) || appFinSourceID != 14) //budget
                        {
                            ConflictStorage.AddNotImportedDto(appOrderDto,
                                                              ConflictMessages.OrderOfAdmissionBeneficiaryCannotInclude);
                            continue;
                        }
                    }
                    else
                    {
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.O)
                            cgItems = cgItems.Where(x => x.NumberBudgetO > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.OZ)
                            cgItems = cgItems.Where(x => x.NumberBudgetOZ > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Budget && appEdFormID == EDFormsConst.Z)
                            cgItems = cgItems.Where(x => x.NumberBudgetZ > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Paid && appEdFormID == EDFormsConst.O)
                            cgItems = cgItems.Where(x => x.NumberPaidO > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Paid && appEdFormID == EDFormsConst.OZ)
                            cgItems = cgItems.Where(x => x.NumberPaidOZ > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Paid && appEdFormID == EDFormsConst.Z)
                            cgItems = cgItems.Where(x => x.NumberPaidZ > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Target && appEdFormID == EDFormsConst.O)
                            cgItems =
                                cgItems.Where(x => x.CompetitiveGroupTargetItem.Sum(y => y.NumberTargetO) > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Target && appEdFormID == EDFormsConst.OZ)
                            cgItems =
                                cgItems.Where(x => x.CompetitiveGroupTargetItem.Sum(y => y.NumberTargetOZ) > 0).ToArray();
                        if (appFinSourceID == EDSourceConst.Target && appEdFormID == EDFormsConst.Z)
                            cgItems =
                                cgItems.Where(x => x.CompetitiveGroupTargetItem.Sum(y => y.NumberTargetZ) > 0).ToArray();
                    }
                }
                bool foundLowResult = false;
                bool notEnaughEntranceTestResults = false;
                Decimal lowResult = 0;
                foreach (CompetitiveGroupItem cgi in cgItems)
                {
                    //нельзя включать в приказ если кол-во результатов вступительных испытаний не равно кол-ву вступительных испытаний
                    //и заявление НЕ со льготой "Без ВИ"
                    foundLowResult = false;
                    notEnaughEntranceTestResults = false;
                    List<EntranceTestItemC> requiredEntranceTests =
                        cgi.CompetitiveGroup.EntranceTestItemC.ToList();
                    List<ApplicationEntranceTestDocument> existedEntranceTests =
                        app.ApplicationEntranceTestDocument.Where(x => x.ResultValue != null).ToList();
                    List<int> requiredEntranceTestsSubjects = requiredEntranceTests.Select(x => x.EntranceTestItemID).ToList();
                    List<int> existedEntranceTestsSubjects =
                        existedEntranceTests.Select(x => x.EntranceTestItemID ?? 0).ToList();
                    ApplicationEntranceTestDocument benefitEntranceTestDocument = app.ApplicationEntranceTestDocument.Any(x => x.BenefitID == 1) ?
                        app.ApplicationEntranceTestDocument.FirstOrDefault(x => x.BenefitID == 1) : null;
                    if (benefitEntranceTestDocument == null)
                    {
                        if (requiredEntranceTestsSubjects.Any(x => !existedEntranceTestsSubjects.Contains(x)))
                        {
                            notEnaughEntranceTestResults = true;
                        }
                        else
                        {
                            //Нельзя включать в приказ, если результаты ВИ не удовлетворяют минимальным требованиям
                            foreach (EntranceTestItemC requiredEntranceTest in requiredEntranceTests)
                            {
                                if (requiredEntranceTest.MinScore != null && requiredEntranceTest.MinScore > 0 &&
                                    existedEntranceTests.Any(
                                        y => y.EntranceTestItemID == requiredEntranceTest.EntranceTestItemID) &&
                                    requiredEntranceTest.MinScore > existedEntranceTests.
                                                                        FirstOrDefault(
                                                                            y =>
                                                                            y.EntranceTestItemID ==
                                                                            requiredEntranceTest.EntranceTestItemID)
                                                                                        .ResultValue)
                                {
                                    lowResult = requiredEntranceTest.MinScore ?? 0;
                                    foundLowResult = true;
                                    break;
                                }
                            }
                            if (!foundLowResult)
                            {
                                cgItem = cgi;
                                break;
                            }
                        }
                    }
                }

                if (!isForeigner)
                {
                    if (notEnaughEntranceTestResults)
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto,
                                                              ConflictMessages
                                                                  .ApplicationCannotIncludeInOrderMissingEntranceTestResult);
                        continue;
                    }
                    if (foundLowResult)
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto,
                                                                      ConflictMessages
                                                                          .ApplicationCannotIncludeInOrderLowResults,
                                                                      lowResult.ToString());
                    }
                }
                

                if (cgItem == null)
                {
                    ConflictStorage.AddNotImportedDto(appOrderDto,
                                                      ConflictMessages.OrderOfAdmissionWithWrongDirection);
                    continue;
                }
                
                /*List<EntranceTestItemC> reqEntranceTests =
                                cgItem.CompetitiveGroup.EntranceTestItemC.ToList();
                List<ApplicationEntranceTestDocument> exEntranceTests =
                                app.ApplicationEntranceTestDocument.Where(x => x.ResultValue != null).ToList();
                
                List<int> reqEntranceTestsSubjects = reqEntranceTests.Select(x => x.EntranceTestItemID).ToList();
                List<int> exEntranceTestsSubjects =
                                exEntranceTests.Select(x => x.EntranceTestItemID ?? 0).ToList();
                ApplicationEntranceTestDocument benEntranceTestDocument = app.ApplicationEntranceTestDocument.Any(x => x.BenefitID == 1) ?
                                app.ApplicationEntranceTestDocument.FirstOrDefault(x => x.BenefitID == 1) : null;
                //нельзя включать в приказ если кол-во результатов вступительных испытаний не равно кол-ву вступительных испытаний
                //и заявление НЕ со льготой "Без ВИ"
                if ((benEntranceTestDocument == null)
                                && reqEntranceTestsSubjects.Any(x => !exEntranceTestsSubjects.Contains(x)))
                {
                    ConflictStorage.AddNotImportedDto(appOrderDto,
                                                          ConflictMessages
                                                              .ApplicationCannotIncludeInOrderMissingEntranceTestResult);
                    continue;
                }
                //Нельзя включать в приказ, если результаты ВИ не удовлетворяют минимальным требованиям
                /*foreach (EntranceTestItemC requiredEntranceTest in reqEntranceTests)
                {
                    if (requiredEntranceTest.MinScore != null && requiredEntranceTest.MinScore > 0 &&
                        exEntranceTests.Any(y => y.EntranceTestItemID == requiredEntranceTest.EntranceTestItemID) &&
                        requiredEntranceTest.MinScore > exEntranceTests.FirstOrDefault(y => y.EntranceTestItemID == requiredEntranceTest.EntranceTestItemID).ResultValue)
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto,
                                                          ConflictMessages
                                                              .ApplicationCannotIncludeInOrderLowResults, requiredEntranceTest.MinScore.ToString());
                        foundLowResult = true;
                        break;
                    }
                }
                if(foundLowResult)
                    continue;*/
                //нормальные ли данные
                if (!ValidateAppState(appOrderDto.EducationFormID.To<short>(0), appOrderDto.FinanceSourceID.To<short>(0)))
                {
                    ConflictStorage.AddNotImportedDto(appOrderDto,
                                                      ConflictMessages.OrderOfAdmissionWithWrongFinSourceAndEduForm);
                    continue;
                }

                if (!isForeigner)
                {
                    //хорошо ли с РВИ
                    if (!ValidateAppExams(app))
                    {
                        ConflictStorage.AddNotImportedDto(appOrderDto,
                                                          ConflictMessages.OrderOfAdmissionWithInvalidAppExamData);
                        continue;
                    }
                }
                appCounts.Add(new Tuple<Application, OrderOfAdmissionItemDto, CompetitiveGroupItem>(app, appOrderDto,
                                                                                                    cgItem));
            }

            //считаем количество мест по всем, включённым в приказ (остальных всё равно исключим, так что по тем, что передали)
            var grouped =
                appCounts.GroupBy(
                    x =>
                    new
                        {
                            x.Item3.CompetitiveGroupItemID,
                            EF = x.Item2.EducationFormID.To<short>(0),
                            FS = x.Item2.FinanceSourceID.To<short>(0)
                        });
            foreach (var gr in grouped)
            {
                foreach (var tuple in gr)
                {
                    AppNumberState valState = ValiddateAppNumbers(tuple.Item1, tuple.Item3, gr.Key.EF, gr.Key.FS,
                                                                  gr.Count());
                    //#41465 - Убрать ограничения на кол-во мест при включении в приказ
                    //if (valState == AppNumberState.ExceedPlaces)
                    //		ConflictStorage.AddNotImportedDto(tuple.Item2, ConflictMessages.OrderOfAdmissionWithIncorrectAppCountForPlaces);
                    if (valState == AppNumberState.NotSelected)
                        ConflictStorage.AddNotImportedDto(tuple.Item2,
                                                          ConflictMessages.OrderOfAdmissionWithNonSelectedForm);
                }
            }
        }

        private enum AppNumberState
        {
            NotAffected = 0,
            NotSelected = 1,
            ExceedPlaces = 2,
            Fine = 3
        }
    }
}