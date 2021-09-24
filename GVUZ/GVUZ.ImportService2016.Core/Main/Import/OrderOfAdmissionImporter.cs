using GVUZ.ImportService2016.Core.Dto.DataReaders.OrderOfAdmission;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Application;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Dto.Import;
using log4net;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    public class OrderOfAdmissionImporter : BaseImporter
    {
        public static readonly ILog oai_logger = LogManager.GetLogger("OrderOfAdmissionImporter");
        public OrderOfAdmissionImporter(Dto.Import.PackageData packageData, VocabularyStorage vocabularyStorage, ImportConflictStorage importConflictStorage, bool deleteBulk) : base(packageData, vocabularyStorage, importConflictStorage, deleteBulk) { }

        List<Tuple<int, string, string>> packageOrderNumbers = new List<Tuple<int, string, string>>(); // Уникальность OrderNumber в пакете
        protected override void Validate()
        {
            // 2016-07-11 Пока нельзя в одном пакете передавать Applications и Orders
            if (packageData.Applications != null && packageData.Orders != null && packageData.Orders.Applications != null)
            {
                foreach (var app in packageData.Orders.Applications)
                {
                    if (packageData.Applications.Any(t => t.UID == app.ApplicationUID))
                    {
                        conflictStorage.SetObjectIsBroken(app, ConflictMessages.OrderOfAdmissionApplicationInImportedApplications, app.ApplicationUID, app.OrderUID);
                    }
                }
            }

            //throw new Exception("OrderOfAdmissionImporter is not implemented");

            // OrderOfAdmissionUID + OrderOfExceptionUID - Проверка на уникальность в рамках пакета
            List<string> orderUIDs = new List<string>();
            //CheckUniqueUID(packageData.OrdersOfAdmission, null);

            List<ApplicationVocDto> processedApplications = new List<ApplicationVocDto>(); // Проверка, что одно заявление не включается дважды в разные Приказы в рамках пакета
           


            if ((packageData.Orders != null) && (
                (packageData.Orders.Applications != null)
                || (packageData.Orders.OrdersOfAdmission != null)
                || (packageData.Orders.OrdersOfException != null)
                )
            )
            {

                if (packageData.Orders.OrdersOfAdmission != null)
                    foreach (var order in packageData.Orders.OrdersOfAdmission)
                    {
                        CheckOrder(order);
                    }

                if (packageData.Orders.OrdersOfException != null)
                    foreach (var order in packageData.Orders.OrdersOfException)
                    {
                        CheckOrder(order);
                    }

                ImportOrders();

                if (packageData.Orders.Applications != null)
                    foreach (var application in packageData.Orders.Applications)
                    {
                        CheckApplication(application);
                        if (!application.IsBroken)
                        {
                            ImportApplication(application);
                        }
                    }
            }
        }

 //        private void OldCheck()
//        {
//            {

////                if (!orderChange)
////                {
////                    // 1.2. Однозначно определить CompetitiveGroupItem (она же проверка 2.1 из спецификации)
////                    var applicationCompetitiveGroupItemVoc = ADOOrderOfAdmissionRepository.GetApplicationCompetitiveGroupItemVoc(dbApplication.ApplicationID);
////                    if (applicationCompetitiveGroupItemVoc == null)
////                    {
////                        conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionWithAbsentCompetitiveGroup);
////                        continue;
////                    }
////                    /*
////                     * .CompetitiveGroupItem.EducationLevelID=OrderOfAdmission.EducationLevelID, 
////                     * .CompetitiveGroup.Course = OrderOfAdmission.Course, 
////                     * .EducationFormId = OrderOfAdmission.EducationFormID, 
////                     * .EducationSourceId = OrderOfAdmission.EducationSourceID (не проверять эти поля, если NULL) 
////                     * .Priority is not NULL
////                     */

////                    //var educationLevel = dbOrder != null ? dbOrder.EducationLevelID : order.EducationLevelID.To(0);
////                    //var educationForm = dbOrder != null ? dbOrder.EducationFormID : order.EducationFormID.To(0);
////                    //var educationSource = dbOrder != null ? dbOrder.EducationSourceID : order.FinanceSourceID.To(0);
////                    //var suitableACGI = applicationCompetitiveGroupItemVoc.Items.Where(t =>
////                    //        (educationLevel == 0 || t.EducationLevelID == educationLevel)
////                    //        && (educationForm == 0 || t.EducationFormId == educationForm)
////                    //        && (educationSource == 0 || t.EducationSourceId == educationSource)
////                    //        && (!order.DirectionIDSpecified || t.DirectionID == order.DirectionID) // это пока оставляем без изменений, потому что directionID нет в приказе
////                    //        && (string.IsNullOrWhiteSpace(order.CompetitiveGroupUID) || t.CompetitiveGroupUID == order.CompetitiveGroupUID) // и это тоже оставляем старый код
////                    //        && t.Priority != 0
////                    //        && t.CampaignStatusID < 2
////                    //    );

////                    var suitableACGI = applicationCompetitiveGroupItemVoc.Items.Where(t =>
////                            (!order.EducationLevelIDSpecified || t.EducationLevelID == order.EducationLevelID)
////                            && (!order.EducationFormIDSpecified || t.EducationFormId == order.EducationFormID)
////                            && (!order.FinanceSourceIDSpecified || t.EducationSourceId == order.FinanceSourceID)
////#warning XSD 2016
////                            //&& (!order.DirectionIDSpecified || t.DirectionID == order.DirectionID)
////                            && (orderApplicationLink == null) || (string.IsNullOrWhiteSpace(orderApplicationLink.CompetitiveGroupUID) || t.CompetitiveGroupUID == orderApplicationLink.CompetitiveGroupUID)
////                            && t.Priority != -1
////                            && t.CampaignStatusID < 2
////                        );
////                    if (suitableACGI.Count() == 0)
////                    {
////                        conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionNoneApplicationCGI);
////                        continue;
////                    }
////                    else if (suitableACGI.Count() == 1)
////                    {
////                        // Если в пакете не указан FinanceSourceID и при этом удается однозначно определить ApplicationCompetitiveGroupItem 
////                        // и ApplicationCompetitiveGroupItem.EducationSourceID = 15, 
////                        // то: Application.OrderIdLevelBudget необязательно для заполнения. Если указано 1,2,3 - превращаем в NULL
////                        var cgi = suitableACGI.First();
////                        if (!order.FinanceSourceIDSpecified && cgi.EducationSourceId == GVUZ.Model.Institutions.AdmissionItemTypeConstants.PaidPlaces)
////                        {
////                            if (orderApplicationLink != null)
////                            {
////                                order.FinanceSourceID = 15;
////                                order.FinanceSourceIDSpecified = true;
////                                orderApplicationLink.OrderIdLevelBudget = 0;
////                                orderApplicationLink.OrderIdLevelBudgetSpecified = false;
////                            }
////                        }
////                    }
////                    else if (suitableACGI.Count() >= 2)
////                    {
////                        //conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionToManyApplicationCGI);
////                        //continue;

////                        int applicationID = dbApplication.ApplicationID;
////                        int idLevelBudget = orderApplicationLink == null ? 0 : orderApplicationLink.OrderIdLevelBudget.To(0);

////                        suitableACGI.OrderBy(t => t.Priority).ThenBy(t => t.ID);
////                        foreach (var item in suitableACGI)
////                        {
////                            var res = ADOOrderOfAdmissionRepository.GetResultCheck8(applicationID, idLevelBudget, item.ID);
////                            if (res)
////                            {
////                                acgi = item;
////                                order.ApplicationCompetitiveGroupItemID = acgi.ID;
////                                break;
////                            }
////                        }

////                    }

////                    if (acgi == null)
////                    {
////                        acgi = suitableACGI.First();
////                        order.ApplicationCompetitiveGroupItemID = acgi.ID;
////                    }
////                }


//                // 1.4. Заодно можно проверить, вдруг это заявление уже включено в этот приказ!
//                //  Если включено в другой приказ - проверять запись в OrderOfAdmissionHistory (связь заявления со «старым» приказом); ModifiedDate is not NULL - перезапись, ModifiedDate is NULL - (ошибка №)
//                //if (dbOrder != null && dbApplication!= null && //dbApplication.OrderOfAdmissionID == dbOrder.OrderID)
//                //    ADOOrderOfAdmissionRepository.GetApplicationOrderOfAdmissionsByHistory(dbApplication.ApplicationID).Any())
//                //if (dbApplication != null && ADOOrderOfAdmissionRepository.GetApplicationOrderOfAdmissionsByHistory(dbApplication.ApplicationID).Any())
//                //{
//                //    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionApplicationAlreadyIn);
//                //    continue;
//                //}

//                //order.CampaignID = acgi != null ? acgi.CampaignID :
//                //    competitiveGroup != null ? competitiveGroup.CampaignID :
//                //    dbOrder != null ? dbOrder.CampaignID : 0;


//                //// Нельзя, чтобы Кампания была не задана, ибо это поле с FK записывать в БД!
//                //if (order.CampaignID == 0)
//                //{
//                //    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionImplicitCompetitiveGroupUidNotFound, order.UID, orderApplicationLink.CompetitiveGroupUID);
//                //    continue;
//                //}
//                //else if (campaign == null)
//                //{
//                //    campaign = vocabularyStorage.CampaignVoc.Items.Where(t => t.CampaignID == order.CampaignID).FirstOrDefault();
//                //}
//                //// Проверка 2.14 на соответствие приемной кампании в заявлении и в приказе (OrderOfAdmission.CampaignID  = CompetitiveGroup.CampaignID)
//                //if (dbOrder != null && dbOrder.OrderStatus > 1 && dbOrder.CampaignID != order.CampaignID)
//                //{
//                //    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionCampaignCantBeChanged);
//                //    continue;
//                //}


                

//                // 2. проверки:
//                // проверки 2.1 - 2.10 - в хранимой процедуре добавления Заявления в Приказ. Тут их не дублируем, только остальные проверки

//                //2.11. Если OrderOfAdmission.IsForeigner = 0, то д.б. Application.Entrant.EntrantDocumentIdentity.IdentityDocumentType.IsRussianNationality = 1 И Application.Entrant.EntrantDocumentIdentity.NationalityTypeID = 1
//                //                    //- Если OrderOfAdmission.IsForeigner = 1, то д.б. Application.Entrant.IdentityDocumentType.IsRussianNationality = 0 И Application.Entrant.EntrantDocumentIdentity.NationalityTypeID != 1
//                //                    if (dbApplication != null)
//                //                    {
//                //#warning XSD 2016
//                //                        //bool isForeigner = order.IsForeignerSpecified ? order.IsForeigner : false;
//                //                        //var appNationalityType = ADOOrderOfAdmissionRepository.GetApplicationNationalityType(dbApplication.ApplicationID);
//                //                        //if (appNationalityType != null)
//                //                        //{
//                //                        //    bool isRussian = appNationalityType.Item2;
//                //                        //    int nationalityTypeID = appNationalityType.Item1;

//                //                        //    // Начиная со спецификации от 20.07.2015 эта проверка отменена
//                //                        //    //if (!isForeigner && (!isRussian || nationalityTypeID != 1))
//                //                        //    //    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionForeignerWrongDocumentData);

//                //                        //    // А эта проверка остается. В приказ для иностранцев по квоте должно быть нельзя включить россиян
//                //                        //    if (isForeigner && (isRussian || nationalityTypeID == 1))
//                //                        //        conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionForeignerWrongDocumentData);
//                //                        //}
//                //                    }

                

               

                


                

                
//                // TODO:
//                // Импорт в базу
//                //if (!order.IsBroken)
//                //{
//                //    List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();
//                //    bulks.Add(new Tuple<string, IDataReader>("bulk_OrderOfAdmission", new BulkOrderOfAdmissionReader(packageData, order)));

//                //    // Должен прийти список импортированных заявлений - пополнить справочники и записать количество в successfulImportStatisticsDto 
//                //    var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportSingleOrderOfAdmission", deleteBulk);
//                //    DataSet dsResult = res.Item1;
//                //    if (dsResult != null && dsResult.Tables.Count >= 1 && dsResult.Tables[0].Rows.Count > 0)
//                //    {
//                //        bool warnings = true;

//                //        // Получить ошибки проверки и добавить соотв. SetObjectIsBroken!
//                //        foreach (DataRow row in dsResult.Tables[0].Rows)
//                //        {
//                //            conflictStorage.SetObjectIsBroken(order, new ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage { Code = (int)row[0], Message = row[1].ToString() });
//                //            if (!(bool)row[2])
//                //                warnings = false;
//                //        }
//                //        if (warnings)
//                //        {
//                //            order.IsBroken = false;
//                //            conflictStorage.successfulImportStatisticsDto.ordersOfAdmissionsImported++;
//                //        }
//                //    }
//                //    else
//                //        conflictStorage.successfulImportStatisticsDto.ordersOfAdmissionsImported++;


//                //    importErrors.AddRange(res.Item2);
//                //}

//            } //foreach

//        }//if


        private void CheckOrder(IOrder order)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            oai_logger.DebugFormat("Проверка пакета {0} для ОО {1}... Приказ (uid): '{2}'", packageData.ImportPackageId, packageData.InstitutionId, order.UID);
            //  UID не пустой
            if (string.IsNullOrEmpty(order.UID))
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionNoUID);
                return;
            }
            // UID должен быть уникальный в рамках ОО (причем даже у разных типов приказов)
            // в базе
            var currentType = order is PackageDataOrdersOrderOfAdmission ? OrderOfAdmissionVocDto.OrderTypeAdmission : OrderOfAdmissionVocDto.OrderTypeException;

            if (vocabularyStorage.OrderOfAdmissionVoc.Items.Any(t => t.UID == order.UID && t.OrderOfAdmissionTypeID != currentType))
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionHasSameUIDAndWrongTypeInDB, order.UID);
                return;
            }
            oai_logger.DebugFormat("Проверка на уникальность пройдена. Всего времени: {0} с.", sw.Elapsed.TotalSeconds);
            Console.WriteLine("Проверка на уникальность пройдена. Всего времени: {0} с.", sw.Elapsed.TotalSeconds);

            // и пакете
            if ((packageData.Orders.OrdersOfAdmission != null &&
                packageData.Orders.OrdersOfAdmission.Any(t => t.OrderOfAdmissionUID == order.UID && (t.GUID != order.GUID || currentType != OrderOfAdmissionVocDto.OrderTypeAdmission))
                ) || (
                packageData.Orders.OrdersOfException != null &&
                packageData.Orders.OrdersOfException.Any(t => t.OrderOfExceptionUID == order.UID && (t.GUID != order.GUID || currentType != OrderOfAdmissionVocDto.OrderTypeException))
                ))
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionHasSameUIDAndWrongTypeInPackage, order.UID);
                return;
            }
            oai_logger.DebugFormat("Проверка на уникальность пройдена. Всего времени: {0} с.", sw.Elapsed.TotalSeconds);
            Console.WriteLine("Проверка на уникальность пройдена. Всего времени: {0} с.", sw.Elapsed.TotalSeconds);


            order.OrderStatus = 1; // Новый, но если найдем этот Приказ в БД, то заменим на тот, что там.
                                   // 1.3. Если OrderOfAdmission уже есть в базе - проверить, изменились ли поля и можно ли их менять (статус = 1 или 2)


            var dbOrder = vocabularyStorage.OrderOfAdmissionVoc.GetItemByUid(order.UID);
            if (dbOrder != null) // Приказ уже есть в БД
            {
                order.ID = dbOrder.OrderID;
                order.OrderStatus = dbOrder.OrderOfAdmissionStatusID;
                if (order.OrderDatePublishedSpecified)
                    order.OrderStatus = 3;
                else if (dbOrder.OrderOfAdmissionStatusID == 3 && !order.OrderDatePublishedSpecified)
                {
                    order.OrderStatus = dbOrder.HasApplications > 0 ? 2 : 1; // Если есть заявления в этом приказе, то статус 2, если нет, то статус 1
                }

                // Если импортируется приказ с заявлением, то поля Form, Source и Level не перезаписываются
                if (dbOrder.HasApplications > 0)
                {
                    // todo ???
                    order.EducationFormIDSpecified = dbOrder != null && dbOrder.EducationFormID > 0;
                    order.EducationLevelIDSpecified = dbOrder != null && dbOrder.EducationLevelID > 0;
                    order.FinanceSourceIDSpecified = dbOrder != null && dbOrder.EducationSourceID > 0;

                    order.EducationFormID = (uint)dbOrder.EducationFormID;
                    order.EducationLevelID = (uint)dbOrder.EducationLevelID;
                    order.FinanceSourceID = (uint)dbOrder.EducationSourceID;

                    if (dbOrder.Stage < 0)
                        order.StageSpecified = false;
                    else
                        order.Stage = (uint)dbOrder.Stage;
                }

                var hasError = false;

                if (dbOrder.OrderOfAdmissionStatusID >= 2) // && order.OrderDatePublishedSpecified)
                {
                    //if (order.IsBeneficiarySpecified) && order.IsBeneficiary != dbOrder.IsForBeneficiary)
                    //{
                    //    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionFieldCannotBeChanged, "IsBeneficiary");
                    //    hasError = true;
                    //}
                    //if (order.IsForeignerSpecified && order.IsForeigner != dbOrder.IsForeigner)
                    //{
                    //    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionFieldCannotBeChanged, "IsForeigner");
                    //    hasError = true;
                    //}
                    //if (order.StageSpecified && order.Stage != dbOrder.Stage)
                    //{
                    //    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionFieldCannotBeChanged, "Stage");
                    //    hasError = true;
                    //}
                    // todo: остальные поля???

                    
                }

                // А если статус 3 (Опубликован) и задана дата публикации (статус не меняется на 2), то нельзя и эти 3 поля менять!
                if (dbOrder.OrderOfAdmissionStatusID == 3 && order.OrderDatePublishedSpecified)
                {
                    if (!string.IsNullOrWhiteSpace(order.OrderName) && order.OrderName != dbOrder.OrderName)
                    {
                        conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionFieldCannotBeChanged, "OrderName");
                        hasError = true;
                    }
                    if (!string.IsNullOrWhiteSpace(order.OrderNumber) && order.OrderNumber != dbOrder.OrderNumber)
                    {
                        conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionFieldCannotBeChanged, "OrderNumber");
                        hasError = true;
                    }
                    if (order.OrderDateSpecified && order.OrderDate != dbOrder.OrderDate)
                    {
                        conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionFieldCannotBeChanged, "OrderDate");
                        hasError = true;
                    }
                }

                // 1.3.2. Если задано .OrderDatePublished, то также должны быть заданы OrderNumber, OrderDate (или уже были заданы ранее, а сейчас пустые)
                if (order.OrderDatePublishedSpecified)
                {
                    if (string.IsNullOrWhiteSpace(dbOrder.OrderNumber))
                    {
                        conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionMustHaveValues, "OrderNumber");
                        hasError = true;
                    }
                    if (!order.OrderDateSpecified && (dbOrder.OrderDate == null || dbOrder.OrderDate.Year < 1977))
                    {
                        conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionMustHaveValues, "OrderDate");
                        hasError = true;
                    }
                }

                if (hasError)
                    return;
            }

            //oai_logger.DebugFormat("Проверка на уникальность пройдена. Всего времени: {1} с.", sw.Elapsed.TotalSeconds);
            Console.WriteLine("Проверка формы обучения. Всего времени: {0} с.", sw.Elapsed.TotalSeconds);

            // 3.3 EducationFormID
            if (order.EducationFormIDSpecified && !VocabularyStatic.AdmissionItemTypeVoc.GetEducationForm().Any(t => t.ItemTypeID == order.EducationFormID))
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.DictionaryItemAbsent, "EducationFormID");
            }

            // 3.4 EducationLevelID
            if (order.EducationLevelIDSpecified && !VocabularyStatic.AdmissionItemTypeVoc.GetEducationLevel().Any(t => t.ItemTypeID == order.EducationLevelID))
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.DictionaryItemAbsent, "EducationLevelID");
            }

            // 3.5 FinanceSourceID
            if (order.FinanceSourceIDSpecified && !VocabularyStatic.AdmissionItemTypeVoc.GetFinanceSource().Any(t => t.ItemTypeID == order.FinanceSourceID))
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.DictionaryItemAbsent, "FinanceSourceID");
            }

            var dbCampaign = vocabularyStorage.CampaignVoc.GetItemByUid(order.CampaignUID);
            if (dbCampaign == null)
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.DictionaryItemAbsent, "CampaignUID");
                return;
            }
            if (dbCampaign.StatusID >= 2)
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.ApplicationCannotIncludeInOrderForCampaignStatus);
                return;
            }

            order.CampaignID = dbCampaign.CampaignID;

            // 3.6 Stage
            // Необязательно для заполнения. Принимает значения 0, 1, 2.
            if (order.StageSpecified && !new int[] { 0, 1, 2 }.Contains(order.Stage.To(0)))
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionWithIncorrectStage);
            }

            //// 2015-10-15. Новое условие: если campaing.IsAdditional - то stage ставить Null и пропускать 
            //if (competitiveGroup != null && competitiveGroup.IsAdditional)
            //{
            //    order.StageSpecified = false;
            //}
            //else
            //{
            //if (order.StageSpecified && !new int[] { 0, 1, 2 }.Contains(order.Stage.To(0)))
            //{
            //    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionWithIncorrectStage);
            //}
            // Если уровень образования – бакалавриат, специалитет (EducationLevelID in (2,3,5,19)) И форма обучения – очная, очно-заочная (EducationFormId in (11,12)), то:
            // - если источник финансирования  – бюджетные места (EducationSourceId in (14)), то допустимые значения – «1», «2», «0» (0 - добавилось 07.08.2015)
            // - если источник финансирования  – целевой прием, квота приема лиц, имеющих особое право (EducationSourceId in (16,20)), то допустимые значения – «0»
            if (new int[] { EDLevelConst.Bachelor, EDLevelConst.Speciality }.Contains(order.EducationLevelID.To(0))
                && new int[] { EDFormsConst.O, EDFormsConst.OZ }.Contains(order.EducationFormID.To(0)))
            {
                // FinanceSourceID == 14 - тогда ошибка если поле Stage не задано или отличается от 0,1,2
                if (order.FinanceSourceID == EDSourceConst.Budget && (!order.StageSpecified || !(new int[] { 0, 1, 2 }.Contains(order.Stage.To(0)))))
                {
                    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionWithIncorrectStage);
                }

                // изменения в спецификации от 30.07.2015
                // если (EducationSourceId in (16,20)), то допустимые значения – любые или можно не заполнять поле (в этом случается всегда подразумевается что задано "0" и в БД пишется "0").
                if (new int[] { EDSourceConst.Target, EDSourceConst.Quota }.Contains(order.FinanceSourceID.To(0)))
                {
                    order.StageSpecified = true;
                    order.Stage = 0;
                }
            }
            //}

            //// Изменение от 30.07.2015: Если (EducationSourceId in (15)), то значение тега <Stage> игнорируем (считаем что не задано) и в БД пишется Null
            if (order.FinanceSourceID == (int)EDSourceConst.Paid)
            {
                order.StageSpecified = false;
                //if (orderApplicationLink != null)
                //    orderApplicationLink.OrderIdLevelBudget = 0;
            }


            // Если передан OrderDatePublished, и при этом в пакете не передано ни одного заявления И нет ни одного заявления в БД с таким orderid, то конфликт.
            if (order.OrderDatePublishedSpecified && dbOrder == null) //!ADOOrderOfAdmissionRepository.HasApplicationsInOrder(order.ID))  // !vocabularyStorage.ApplicationVoc.Items.Any(t=> t.StatusID == 8 && t.OrderOfAdmissionID == order.ID))
            {
                conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionNoAppCantBePublished);
            }

            Console.WriteLine("Финальная проверка.... Всего времени: {0} с.", sw.Elapsed.TotalSeconds);

            // OrderNunber - Проверка на уникальность регистрационного номера приказа о зачислении в рамках выбранной приемной кампании
            if (!string.IsNullOrWhiteSpace(order.OrderNumber))
            {
                var sameNumberOrders = vocabularyStorage.OrderOfAdmissionVoc.Items.Where(t => t.OrderNumber == order.OrderNumber && t.UID != order.UID && t.CampaignID == order.CampaignID);
                if (sameNumberOrders.Any())
                {
                    string dbNumberText = dbOrder != null && !string.IsNullOrWhiteSpace(dbOrder.OrderNumber) ?
                        string.Format(". Для приказа с UID {0} должен быть передан номер {1}", order.UID, dbOrder.OrderNumber)
                        : "";
                    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionNumberMustBeUnique, order.OrderNumber, sameNumberOrders.First().UID , dbNumberText);
                    return;
                }

                // А еще проверить, что в данном пакете он не встречался дважды у одной ПК у разных приказов
                var packageSameNumber =
                    packageOrderNumbers.Where(
                        t => t.Item1 == order.CampaignID && t.Item2 == order.OrderNumber && t.Item3 != order.UID);
                if (packageSameNumber.Any())
                {
                    conflictStorage.SetObjectIsBroken(order, ConflictMessages.OrderOfAdmissionNumberMustBeUniqueInPackage,
                        order.OrderNumber, string.Join(",", packageSameNumber.Select(t => t.Item3)));
                    return;
                }
                else
                {
                    packageOrderNumbers.Add(new Tuple<int, string, string>(order.CampaignID, order.OrderNumber, order.UID));
                }
            }
            sw.Stop();
            oai_logger.DebugFormat("Пакет №{0} приказов на зачисление проверен за {1} с.", packageData.ImportPackageId, sw.Elapsed.TotalSeconds);
            Console.WriteLine("Пакет №{0} пакет приказов на зачисление за  проверен за {1} с.", packageData.ImportPackageId, sw.Elapsed.TotalSeconds);

        }


        private void ImportOrders()
        {
            List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();
            bulks.Add(new Tuple<string, IDataReader>("bulk_OrderOfAdmission", new BulkOrderOfAdmissionReader(packageData)));
            bulks.Add(new Tuple<string, IDataReader>("bulk_OrderOfAdmission", new BulkOrderOfExceptionReader(packageData)));

            oai_logger.DebugFormat("Загрузка списка импортированных заявлений в БД [пакет: {0}, ОО: {1}",
                                   packageData.ImportPackageId, packageData.InstitutionId);

            // Должен прийти список импортированных заявлений - пополнить справочники и записать количество в successfulImportStatisticsDto 
            var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportOrders", deleteBulk, oai_logger);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count >= 1)
            {
                conflictStorage.successfulImportStatisticsDto.Orders = dsResult.Tables[0].Rows.Count.ToString();

                vocabularyStorage.OrderOfAdmissionVoc.AddItems(dsResult.Tables[0]);
            }
        }

        private void CheckApplication(PackageDataOrdersApplication application)
        {
            application.UID = application.ApplicationUID;

            // 1.1. Найти Application
            var dbApplication = vocabularyStorage.ApplicationVoc.GetItemByUid(application.UID);
            if (dbApplication == null)
            {
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfAdmissionContainsRefOnNotImportedApplication);
                return;
            }

            application.ID = dbApplication.ApplicationID; // Нужно для оптимизированной проверки 2.12

            var dbOrder = vocabularyStorage.OrderOfAdmissionVoc.Items.FirstOrDefault(t=> t.UID == application.OrderUID && t.OrderOfAdmissionTypeID == application.OrderTypeID);
            if (dbOrder == null)
            {
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfAdmissionNotFound, application.OrderUID);
                return;
            }

            application.OrderID = dbOrder.ID;

            // Однозначно определить ApplicationCompetitiveGroupItem (она же проверка 2.1 из спецификации)
            var competitiveGroup = vocabularyStorage.CompetitiveGroupVoc.GetItemByUid(application.CompetitiveGroupUID);
            if (competitiveGroup == null)
            {
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfAdmissionApplicationCompetitiveGroupNotFound, application.CompetitiveGroupUID);
                return;
            }

            var acgi = vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.FirstOrDefault(t => t.CompetitiveGroupId == competitiveGroup.CompetitiveGroupID && t.ApplicationId == application.ID);
            if (acgi == null)
            {
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfAdmissionApplicationCompetitiveGroupNotFound, application.CompetitiveGroupUID);
                return;
            }
            application.ApplicationCompetitiveGroupItemID = acgi.ID;

            if (dbOrder.OrderOfAdmissionTypeID == OrderOfAdmissionVocDto.OrderTypeAdmission)
            {
                // 2.13. Статус заявления перед включением в приказ (перед присвоением Application.StatusID = 8) может быть "Принято" (4) 
                if ((dbApplication.StatusID != 4 && dbApplication.StatusID != 8) || dbApplication.ViolationID == -1)  //dbApplication.ViolationID != 0
                {
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationCannotIncludeInOrderForAppStatus);
                    return;
                }

                //2.11. Если OrderOfAdmission.IsForeigner = 0, то д.б. Application.Entrant.EntrantDocumentIdentity.IdentityDocumentType.IsRussianNationality = 1 И Application.Entrant.EntrantDocumentIdentity.NationalityTypeID = 1
                //      Если OrderOfAdmission.IsForeigner = 1, то д.б. Application.Entrant.IdentityDocumentType.IsRussianNationality = 0 И Application.Entrant.EntrantDocumentIdentity.NationalityTypeID != 1

                // ACGI.OrderOfAdmissionID дб null, иначе что за повторное включение?
                if (acgi.OrderOfAdmissionID != 0)
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfAdmissionAcgiAlreadyIncluded);

                if (acgi.IsDisagreed)
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfAdmissionAcgiHasDisagreedDate);

                // Проверить, вдруг это заявление уже включено в этот приказ!
                /*  Level: Бак + спец, Form: O, OZ, Source: Budget  - тогда допускается 1 OOA, на него 1 OOE и затем еще 1 OOA   */
                if ((new int[] { EDLevelConst.Bachelor, EDLevelConst.Speciality }.Contains(competitiveGroup.EducationLevelID))
                    && (new int[] { EDFormsConst.O, EDFormsConst.OZ }.Contains(competitiveGroup.EducationFormId))
                    && competitiveGroup.EducationSourceId == EDSourceConst.Budget
                    )
                {
                    var appAcgis = vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.Where(t => t.ApplicationId == dbApplication.ApplicationID && t.OrderOfAdmissionID != 0);

                    if (appAcgis.Count() > 1)
                    {
                        var badOrders = vocabularyStorage.OrderOfAdmissionVoc.Items.Where(o => appAcgis.Any(a => o.ID == a.OrderOfAdmissionID));
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfAdmissionApplicationAlreadyHas2Orders, string.Join(",", badOrders!= null ? badOrders.Select(t => t.UID) : new string[] { } ));
                    }
                    if (appAcgis.Count() == 1 && appAcgis.First().OrderOfExceptionID == 0)
                    {
                        var badOrder = vocabularyStorage.OrderOfAdmissionVoc.Items.FirstOrDefault(t => t.ID == appAcgis.First().OrderOfAdmissionID);
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfAdmissionApplicationAlreadyHasOrder,
                            badOrder != null ? badOrder.UID : string.Empty);
                    }

                }


                // Если OOA - должны быть OrderIdLevelBudget, BenefitKindID если задана - должна соотв. словарю
                if (application.BenefitKindIDSpecified && !VocabularyStatic.BenefitVoc.Items.Any(t => t.BenefitID == application.BenefitKindID))
                {
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "Application.BenefitKindID");
                }

                // 3. Проверки по полям: в основном, что корректные значения из справочников
                // 3.1 Application.OrderIdLevelBudget
                //if (application.BenefitKindIDSpecified && !VocabularyStatic.BenefitVoc.Items.Any(t => t.BenefitID == application.BenefitKindID))
                //{
                //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "Application.BenefitKindID");
                //}

                var dbCampaign = vocabularyStorage.CampaignVoc.Items.FirstOrDefault(t => t.CampaignID == competitiveGroup.CampaignID);

                if (competitiveGroup.IdLevelBudget != null && competitiveGroup.IdLevelBudget != 0)
                {
                    application.OrderIdLevelBudgetSpecified = true;
                    application.OrderIdLevelBudget = (uint)competitiveGroup.IdLevelBudget;
                }
                
                    // 3.1.1 Если CompetitiveGroup\FinanceSourceID = 15 - то необязательно для заполнения. Иначе - обязательно. (Если обязательно и не заполнено - ошибка номер)
                if (competitiveGroup.EducationSourceId != (int)EDSourceConst.Paid
                    && (dbCampaign != null && dbCampaign.CampaignTypeID != GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.Foreigners)
                    ) //&& !application.OrderIdLevelBudgetSpecified)
                {
                    if (!application.OrderIdLevelBudgetSpecified || !VocabularyStatic.LevelBudgetVoc.Items.Any(t => t.IdLevelBudget == application.OrderIdLevelBudget))
                    {
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "Application.OrderIdLevelBudget");
                    }
                }
                else
                {
                    application.OrderIdLevelBudgetSpecified = false;
                    application.OrderIdLevelBudget = 0;
                }
               
            }
            else if (dbOrder.OrderOfAdmissionTypeID == OrderOfAdmissionVocDto.OrderTypeException)
            {
                // ACGI.OrderOfAdmissionID дб не null, иначе из какого приказа исключаем???
                if (acgi.OrderOfAdmissionID == 0)
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfExceptionAcgiHasNoOOA);
                if (acgi.OrderOfExceptionID != 0)
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfExceptionAcgiAlreadyHasBeenExcluded);


                /*  Level: Бак + спец, Form: O, OZ, Source: Budget  - тогда допускается 1 OOA, на него 1 OOE и затем еще 1 OOA. 2й OOE нельзя!  */
                if ((new int[] { EDLevelConst.Bachelor, EDLevelConst.Speciality }.Contains(competitiveGroup.EducationLevelID))
                    && (new int[] { EDFormsConst.O, EDFormsConst.OZ }.Contains(competitiveGroup.EducationFormId))
                    && competitiveGroup.EducationSourceId == EDSourceConst.Budget
                    )
                {
                    var appAcgis = vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.Where(t => t.ApplicationId == dbApplication.ApplicationID && t.OrderOfExceptionID != 0 && t.ID != acgi.ID);
                    if (appAcgis.Any())
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfExceptionApplicationAlreadyHasExceptionOrder, string.Join(",", appAcgis.Select(t => t.UID)));
                }

                // Если OOE - должна быть IsDisagreedDate
                if (!application.IsDisagreedDateSpecified)
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.OrderOfExceptionDisagreedDateMustBeSpecified);
            }
        }

        private void ImportApplication(PackageDataOrdersApplication application)
        {
            List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();
            bulks.Add(new Tuple<string, IDataReader>("bulk_OrderOfAdmission", new BulkApplicationInOrderReader(packageData, application)));

            // Должен прийти список импортированных заявлений - пополнить справочники и записать количество в successfulImportStatisticsDto
            oai_logger.DebugFormat("Загрузка импортированного заявления в БД [пакет: {0}, ОО: {1}",
                                   packageData.ImportPackageId, packageData.InstitutionId);
            var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportSingleOrderOfAdmission", deleteBulk, oai_logger);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count >= 1 && dsResult.Tables[0].Rows.Count > 0)
            {
                bool warnings = true;

                // Получить ошибки проверки и добавить соотв. SetObjectIsBroken!
                foreach (DataRow row in dsResult.Tables[0].Rows)
                {
                    conflictStorage.SetObjectIsBroken(application, new ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage { Code = (int)row[0], Message = row[1].ToString() });
                    if (!(bool)row[2])
                        warnings = false;
                }
                if (warnings)
                {
                    application.IsBroken = false;
                    conflictStorage.successfulImportStatisticsDto.applicationsInOrdersImported++;
                }
            }
            else
                conflictStorage.successfulImportStatisticsDto.applicationsInOrdersImported++;


            importErrors.AddRange(res.Item2);
        }

        List<string> importErrors = new List<string>();

        protected override List<string> ImportDb()
        {
            // 2015-07-23 Больше не делаем общей балк-загрузки и обработки приказов, потому что обрабатываем их пошагово
            return importErrors;


            //List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();
            //bulks.Add(new Tuple<string, IDataReader>("bulk_OrderOfAdmission", new BulkOrderOfAdmissionReader(packageData)));

            //// Должен прийти список импортированных заявлений - пополнить справочники и записать количество в successfulImportStatisticsDto 
            //var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportOrderOfAdmission");
            //DataSet dsResult = res.Item1;
            //if (dsResult != null && dsResult.Tables.Count >= 2)
            //{
            //    conflictStorage.successfulImportStatisticsDto.OrdersOfAdmissions = dsResult.Tables[0].Rows.Count.ToString();

            //    // Получить ошибки проверки и добавить соотв. SetObjectIsBroken!
            //    foreach (DataRow row in dsResult.Tables[1].Rows)
            //    {
            //        var applicationID = (int)row[0];
            //        var order = packageData.OrdersOfAdmission.Where(t => t.Application.ApplicationID == applicationID).FirstOrDefault();
            //        if (order != null)
            //        {
            //            conflictStorage.SetObjectIsBroken(order, new ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage { Code = (int)row[1], Message = row[2].ToString() });
            //        }
            //    }
            //}
            //return res.Item2;
        }
    }
}
