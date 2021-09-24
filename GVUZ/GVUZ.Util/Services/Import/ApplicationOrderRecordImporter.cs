using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.Helpers;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;
using Application = GVUZ.ServiceModel.Import.Application;
using ApplicationEntranceTestDocument = GVUZ.ServiceModel.Import.ApplicationEntranceTestDocument;
using ApplicationStatusType = GVUZ.ServiceModel.Import.ApplicationStatusType;
using CampaignDate = GVUZ.ServiceModel.Import.CampaignDate;
using CompetitiveGroup = GVUZ.ServiceModel.Import.CompetitiveGroup;
using CompetitiveGroupItem = GVUZ.ServiceModel.Import.CompetitiveGroupItem;
using EntranceTestItemC = GVUZ.ServiceModel.Import.EntranceTestItemC;
using OrderOfAdmission = GVUZ.ServiceModel.Import.OrderOfAdmission;
using OrderOfAdmissionHistory = GVUZ.ServiceModel.Import.OrderOfAdmissionHistory;
using Subject = GVUZ.ServiceModel.Import.Subject;

namespace GVUZ.Util.Services.Import
{
    public class ApplicationOrderRecordImporter
    {
        private readonly string _connectionString;
        private readonly ImportEntities _importEntities;
        
        public ApplicationOrderRecordImporter(string connectionString)
        {
            _connectionString = connectionString;
            _importEntities = new ImportEntities();
            DbStableObjectRepository.Load(_importEntities);
        }

        public void Import(ApplicationOrderRecord record)
        {
            SetRecordProcessing(record);

            try
            {
                ProcessRecord(record);
                SetRecordProcessed(record);
            }
            catch (ApplicationException ae)
            {
                SetRecordError(record, ae);
                throw;
            }
        }

        private void ProcessRecord(ApplicationOrderRecord record)
        {
            var applink = FindApplication(record);
            
            if (applink == null)
            {
                throw new ApplicationException("Заявление не найдено");
            }

            var app = _importEntities.Application.FirstOrDefault(x => x.ApplicationID == applink.ApplicationId);

            if (app == null)
            {
                throw Conflict(ConflictMessages.OrderOfAdmissionContainsRefOnNotImportedApplication);
            }

            /*
            * Проверить соответствие заявления в таблице-источнике на возможность включения в приказ. 
            * Перечень проверок должен быть аналогичен проверкам, которые осуществляются сервисом автоматизированного взаимодействия 
            * при включении заявлений в приказ из пакетов 
             *
             * см. GVUZ.ServiceModel\Import\Core\Operations\Importing\Orders\OrderCollectionImporter.cs
            */
            CheckIntegrity(app, record);

            if (app.StatusID == ApplicationStatusType.Accepted)
            {
                IncludeApplicationInOrder(app, record);
            }
            else if (app.StatusID == ApplicationStatusType.InOrder)
            {
                UpdateApplicationInOrder(app, record);
            }
        }

        /// <summary>
        /// Включение заявления со статусом "Принято" в приказ
        /// </summary>
        private void IncludeApplicationInOrder(Application app, ApplicationOrderRecord record)
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
            int _institutionID = app.InstitutionID;

            List<OrderOfAdmission> instOrders =
                _importEntities.OrderOfAdmission.Where(x => x.InstitutionID == _institutionID).ToList();
            //foreach (OrderOfAdmissionItemDto appOrderDto in _orderOfAdmissionDtos)
            {
                //ApplicationOrderRecord dto = record;
                
                DeletePackageHandler.CalculateApplicationRating(_importEntities, app.ApplicationID);

                int appEdFormID = record.EducationFormId.GetValueOrDefault();
                int appFinSourceID = record.FinanceSourceId.GetValueOrDefault();
                int appEdLevelID = record.EducationLevelId.GetValueOrDefault();
                int stage = record.Stage.GetValueOrDefault();
                bool isForBeneficiary = record.IsBeneficiary;
                if (isForBeneficiary) stage = 0;
                bool isForeigner = record.IsForeigner;
                
                //ищём подходящее направление КГ
                int intDirectionID = record.DirectionId.GetValueOrDefault();

                CompetitiveGroupItem[] cgItems = null;
                
                //bool implicitCompetitiveGroup = !string.IsNullOrEmpty(dto.CompetitiveGroupUID);
                //string implicitCompetitiveGroupUID = dto.CompetitiveGroupUID;
                bool implicitCompetitiveGroup = false;
                string implicitCompetitiveGroupUID = null;

                if (implicitCompetitiveGroup)
                {
                    var item = app.ApplicationSelectedCompetitiveGroupItem
                                 .Where(
                                     x => x.CompetitiveGroupItem.CompetitiveGroup.UID == implicitCompetitiveGroupUID)
                                 .Select(x => x.CompetitiveGroupItem).ToArray().FirstOrDefault();

                    if (item != null)
                    {
                        cgItems = new[] { item };
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
                        // throw Conflict(ConflictMessages.OrderOfAdmissionImplicitCompetitiveGroupUidNotFound, record.OrderUid, implicitCompetitiveGroupUID));
                    }
                    else
                    {
                        throw Conflict(ConflictMessages.OrderOfAdmissionWithWrongDirection);
                    }
                }

                if (cgItems.Length == 1)
                {
                    cgItem = cgItems[0];

                    if (implicitCompetitiveGroup && !allowedGroups.Contains(cgItem.CompetitiveGroupID))
                    {
                        throw Conflict(ConflictMessages.OrderOfAdmissionMismatchImplicitCompetitiveGroup);
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
                        throw Conflict(ConflictMessages.OrderOfAdmissionBeneficiaryCannotInclude);
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
                            throw Conflict(ConflictMessages.OrderOfAdmissionBeneficiaryCannotInclude);
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
                        EducationLevelID = (short)appEdLevelID,
                        EducationFormID = (short)appEdFormID,
                        EducationSourceID = (short)orderFinSource,
                        Stage = (short)stage,
                        IsForBeneficiary = isForBeneficiary,
                        // UID = record.OrderUid,
                        IsForeigner = isForeigner,
                        DateEdited = now,
                    };
                    
                    _importEntities.OrderOfAdmission.AddObject(dbOrder);
                    instOrders.Add(dbOrder);
                }
            

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
                app.OrderEducationFormID = record.EducationFormId.GetValueOrDefault().To<short>();
                app.OrderEducationSourceID = record.FinanceSourceId.GetValueOrDefault().To<short>();
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
                //processedApps.Add(app);
                // ReSharper disable SimplifyLinqExpression НЕ НАДО
                if (!_processedOrders.Any(x => x.OrderID != dbOrder.OrderID))
                    _processedOrders.Add(dbOrder);
                // ReSharper restore SimplifyLinqExpression
                //SuccessfullyImportedOrders++;
            }

            //заявления, которые нужно исключить из приказа
            //не трогаем заявления из других приказов
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

            //var sw = new Stopwatch();
            //sw.Start();
            //пишем в базу
            _importEntities.SaveChanges();
            //sw.Stop();
            //Log.DebugFormat("Imported {0} orders. Total: {1} sec.", SuccessfullyImportedOrders, sw.Elapsed.TotalSeconds);
            //sw.Restart();
            //публикуем приказы
            foreach (OrderOfAdmission orderDto in _processedOrders)
            {
                OrderOfAdmission dbOrder = orderDto;
                if (dbOrder.OrderStatus == 1) // edited = вновь создан
                {
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
                            {
                                OrderID = dbOrder.OrderID,
                                ApplicationID = appID,
                                DatePublished = now
                            };
                        _importEntities.OrderOfAdmissionHistory.AddObject(history);
                    }
                }
            }

            _importEntities.SaveChanges();
            //sw.Stop();
            //Log.DebugFormat("Imported {0} orders. History recorded: {1} sec.", SuccessfullyImportedOrders,
            //                sw.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// Обновление записи о включении заявления в приказ (со статусом "Включено в приказ")
        /// </summary>
        private void UpdateApplicationInOrder(Application app, ApplicationOrderRecord record)
        {
            IncludeApplicationInOrder(app, record);
        }

        /// <summary>
        /// Проверка целостности заявлений в приказе перетащено из <see cref="OrderCollectionImporter"/>
        /// </summary>
        private void CheckIntegrity(Application app, ApplicationOrderRecord record)
        {
            var appCounts = new List<Tuple<Application, ApplicationOrderRecord, CompetitiveGroupItem>>();

            int institutionId = record.InstitutionId.GetValueOrDefault();
            List<CampaignDate> campaignDates = _importEntities.CampaignDate.Where(x => x.Campaign.InstitutionID == institutionId).ToList();

            var dbObjectRepository = new DbObjectRepository(app.InstitutionID);

            //бежим по всем заявлениям
            //foreach (OrderOfAdmissionItemDto appOrderDto in _orderOfAdmissionDtos)
            {
                //проверяем на целостность типы приказов
                if (app.StatusID != ApplicationStatusType.Accepted && app.StatusID != ApplicationStatusType.InOrder)
                {
                    throw Conflict(ConflictMessages.ApplicationCannotIncludeInOrderForAppStatus);
                }

                int appEdFormID = record.EducationFormId.GetValueOrDefault();
                int appFinSourceID = record.FinanceSourceId.GetValueOrDefault();
                int appEdLevelID = record.EducationLevelId.GetValueOrDefault();
                int stage = record.Stage.GetValueOrDefault();
                bool isForBeneficiary = record.IsBeneficiary;
                if (isForBeneficiary) stage = 0;

                #region Иностранцы

                bool isForeigner = record.IsForeigner;
                if (isForeigner)
                {
                    if (isForBeneficiary)
                    {
                        throw Conflict(ConflictMessages.ApplicationUnableToIncludeInBeneficiaryOrder);
                    }

                    if (record.Stage.GetValueOrDefault() != 0)
                    {
                        throw Conflict(ConflictMessages.ApplicationInvalidStage);
                    }

                    if (appFinSourceID != EDSourceConst.Budget || app.Entrant == null || app.Entrant.EntrantDocument_Identity == null ||
                        string.IsNullOrEmpty(app.Entrant.EntrantDocument_Identity.DocumentSpecificData))
                    {
                        throw Conflict(ConflictMessages.ApplicationForeignEntrantUnacceptable);
                    }
                    else if (!string.IsNullOrEmpty(app.Entrant.EntrantDocument_Identity.DocumentSpecificData))
                    {
                        string data = app.Entrant.EntrantDocument_Identity.DocumentSpecificData;
                        var serializer = new JavaScriptSerializer();
                        IdentityDocumentDto doc = serializer.Deserialize<IdentityDocumentDto>(data);
                        if (doc.NationalityTypeID.To(1) == 1 || dbObjectRepository.IsRussianDoc(doc.IdentityDocumentTypeID.To(0)))
                        {
                            throw Conflict(ConflictMessages.ApplicationForeignEntrantUnacceptable);
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
                              // && x.Stage == stage #29340
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
                        throw Conflict(ConflictMessages.OrderOfAdmissionWithIncorrectStage);
                    //есть дата без этапа и этапа не передали -> отсутствует этап
                    else if (hasRelatedDateWithoutStage && stage == 0)
                        throw Conflict(ConflictMessages.OrderOfAdmissionWithMissingStage);
                    //нет даты и без этапа -> всё не так
                    else
                        throw Conflict(ConflictMessages.OrderOfAdmissionWithInvalidType);
                }

                ApplicationOrderRecord dto = record;

                ////уже есть в обработанных - дубль
                //if (processedApplications.Any(
                //    x =>
                //    x.ApplicationNumber == dto.Application.ApplicationNumber &&
                //    x.RegistrationDateDate == dto.Application.RegistrationDateDate))
                //{
                //    throw Conflict(ConflictMessages.OrderOfAdmissionWithDuplicateApp);
                //}

                /* При включении в приказ заявлений, для которых не предоставлены оригиналы  документов о 
                 * предыдущем уровне образования, должна производиться проверка на наличие прикрепленных 
                 * к заявлениям справок об обучении в другом ВУЗе   */
                /* Короче если нет оригиналов и нет документа StudentDocument - ругаемся */
                #region OriginalsChecks
                if (!app.OriginalDocumentsReceived && app.Institution.InstitutionTypeID == 2 && dto.FinanceSourceId.GetValueOrDefault() == EDSourceConst.Budget)
                {
                    throw Conflict(ConflictMessages.OriginalDocumentsRequired);
                }

                if (!app.OriginalDocumentsReceived && dto.FinanceSourceId.GetValueOrDefault() == EDSourceConst.Budget && 
                    app.ApplicationEntrantDocument.All(c =>
                                                                                         c.EntrantDocument
                                                                                          .DocumentTypeID !=
                                                                                         (int)
                                                                                         EntrantDocumentType
                                                                                             .StudentDocument))
                {
                    throw Conflict(ConflictMessages.StudentDocumentRequired);
                }
                
                #endregion OriginalsChecks

                //processedApplications.Add(dto.Application);

                //такого не может быть, но надо же проверить
                if (anyCG.CompetitiveGroupID == 0)
                {
                    throw Conflict(ConflictMessages.OrderOfAdmissionWithAbsentCompetitiveGroup);
                }

                //#29403
                //bool hasInvalidCampaigns = dbObjectRepository.Campaigns.Where(x => x.CampaignID == campaignID)
                //                                             .Select(x => x.StatusID).Where(x => x != 1).Any();
                ////идёт набор
                //if (hasInvalidCampaigns)
                //{
                //    throw Conflict(ConflictMessages.ApplicationCannotIncludeInOrderForCampaignStatus);
                //}

                //ищём подходящее направление КГ
                int intDirectionID = record.DirectionId.GetValueOrDefault();
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
                    throw Conflict(ConflictMessages.OrderOfAdmissionWithWrongDirection);
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
                        throw Conflict(ConflictMessages.OrderOfAdmissionBeneficiaryCannotInclude);
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
                            throw Conflict(ConflictMessages.OrderOfAdmissionBeneficiaryCannotInclude);
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
                        throw Conflict(ConflictMessages.ApplicationCannotIncludeInOrderMissingEntranceTestResult);
                    }
                    if (foundLowResult)
                    {
                        throw Conflict(ConflictMessages.ApplicationCannotIncludeInOrderLowResults, lowResult);
                    }
                }


                if (cgItem == null)
                {
                    throw Conflict(ConflictMessages.OrderOfAdmissionWithWrongDirection);
                }

                //нормальные ли данные
                if (!ValidateAppState(record.EducationFormId.GetValueOrDefault(), record.FinanceSourceId.GetValueOrDefault()))
                {
                    throw Conflict(ConflictMessages.OrderOfAdmissionWithWrongFinSourceAndEduForm);
                }

                if (!isForeigner)
                {
                    //хорошо ли с РВИ
                    if (!ValidateAppExams(app, dbObjectRepository))
                    {
                        throw Conflict(ConflictMessages.OrderOfAdmissionWithInvalidAppExamData);
                    }
                }

                appCounts.Add(new Tuple<Application, ApplicationOrderRecord, CompetitiveGroupItem>(app, record, cgItem));
            }

            //считаем количество мест по всем, включённым в приказ (остальных всё равно исключим, так что по тем, что передали)
            //var grouped =
            //    appCounts.GroupBy(
            //        x =>
            //        new
            //        {
            //            x.Item3.CompetitiveGroupItemID,
            //            EF = x.Item2.EducationFormID.To<short>(0),
            //            FS = x.Item2.FinanceSourceID.To<short>(0)
            //        });
            //foreach (var gr in grouped)
            //{
            //    foreach (var tuple in gr)
            //    {
            //        AppNumberState valState = ValiddateAppNumbers(tuple.Item1, tuple.Item3, gr.Key.EF, gr.Key.FS,
            //                                                      gr.Count());
            //        //#41465 - Убрать ограничения на кол-во мест при включении в приказ
            //        //if (valState == AppNumberState.ExceedPlaces)
            //        //		ConflictStorage.AddNotImportedDto(tuple.Item2, ConflictMessages.OrderOfAdmissionWithIncorrectAppCountForPlaces);
            //        if (valState == AppNumberState.NotSelected)
            //            ConflictStorage.AddNotImportedDto(tuple.Item2,
            //                                              ConflictMessages.OrderOfAdmissionWithNonSelectedForm);
            //    }
            //}
        }

        private bool ValidateAppState(int formId, int sourceId)
        {
            if (!new[] { 10, 11, 12 }.Contains(formId))
                return false;
            if (!new[] { 14, 15, 16, 20 }.Contains(sourceId))
                return false;
            return true;
        }

        private bool ValidateAppExams(Application app, DbObjectRepository dbObjectRepository)
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
                    Subject subject = dbObjectRepository.GetSubject(data1.SubjectID.To(0));
                    if (subject != null && subject.Name == LanguageSubjects.ForeignLanguage)
                    {
                        egeDetailsValue = egeModel.Subjects.Where(x =>
                        {
                            Subject subj = dbObjectRepository.GetSubject(x.SubjectID);
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

        private ApplicationException Conflict(int messageCode, params object[] args)
        {
            return new ApplicationException(string.Format(ConflictMessages.GetMessage(messageCode), args));
        }

        private void SetRecordProcessing(ApplicationOrderRecord record)
        {
            record.Status = ApplicationOrderRecord.StatusProcessing;
            record.Comment = null;
            record.ModifiedDate = DateTime.Now;
            UpdateRecordStatus(record);
        }

        private void SetRecordProcessed(ApplicationOrderRecord record)
        {
            record.Status = ApplicationOrderRecord.StatusComplete;
            record.Comment = null;
            record.ModifiedDate = DateTime.Now;
            UpdateRecordStatus(record);
        }

        private void SetRecordError(ApplicationOrderRecord record, ApplicationException e)
        {
            record.Status = ApplicationOrderRecord.StatusError;
            record.Comment = e.Message;
            record.ModifiedDate = DateTime.Now;
            UpdateRecordStatus(record);
        }

        private void UpdateRecordStatus(ApplicationOrderRecord record)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    using (SqlCommand cmd = cn.CreateCommand())
                    {
                        cmd.Transaction = tx;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = Properties.Resources.Import_UpdateRecordStatus;
                        SqlParameter pStatus = cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Int));
                        SqlParameter pComment = cmd.Parameters.Add(new SqlParameter("@comment", SqlDbType.Text));
                        SqlParameter pModifiedDate = cmd.Parameters.Add(new SqlParameter("@modifiedDate", SqlDbType.DateTime));
                        SqlParameter pId = cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                        pStatus.Value = record.Status;
                        pComment.Value = record.Comment ?? (object)DBNull.Value;
                        pModifiedDate.Value = record.ModifiedDate.GetValueOrDefault(DateTime.Now);
                        pId.Value = record.Id;

                        try
                        {
                            cmd.ExecuteNonQuery();
                            tx.Commit();
                        }
                        catch (Exception e)
                        {
                            tx.Rollback();
                            throw new ApplicationException(string.Format("Ошибка при обновлении статуса записи: {0}", e.Message));
                        }
                    }
                }
            }
        }

        private ApplicationEntity FindApplication(ApplicationOrderRecord record)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = Properties.Resources.Import_FindApplicationByRecord;
                            SqlParameter pId = cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                            pId.Value = record.Id;

                            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult | CommandBehavior.SingleRow))
                            {
                                if (reader.Read())
                                {
                                    return ParseApplicationEntity(reader);
                                }

                                return null;
                            }
                        }
                    }
                    finally 
                    {
                        tx.Commit();
                    }
                    
                }
            }
        }

        private static ApplicationEntity ParseApplicationEntity(SqlDataReader reader)
        {
            return new ApplicationEntity
                {
                    ApplicationId = reader.GetInt32(0),
                    RegistrationNumber = reader.GetString(1),
                    RegistrationDate = reader.GetDateTime(2),
                    InstitutionId = reader.GetInt32(3),
                    StatusId = reader.GetInt32(4)
                };
        }
    }
}