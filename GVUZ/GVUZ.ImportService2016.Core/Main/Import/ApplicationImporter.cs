using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GVUZ.ImportService2016.Core.Dto.DataReaders.Applications;
using log4net;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    public class ApplicationImporter : BaseImporter
    {
        public static readonly ILog ai_logger = LogManager.GetLogger("ApplicationImporter");
        //TODO: Жуткий хардкод для ошибок прервышения длины полей! Надо все это выносить в XSD!!!
        public static int ENTRANTDOCUMENTEDU_REGISTRATIONNUMBER = 20;

        public ApplicationImporter(PackageData packageData, VocabularyStorage vocabularyStorage, ImportConflictStorage importConflictStorage, bool deleteBulk) : base(packageData, vocabularyStorage, importConflictStorage, deleteBulk) { }

        protected override void Validate()
        {
            if (packageData.GetApplications != null)
            {
                Console.WriteLine("Валидация пакета заявлений № {0}, всего: {1}...", packageData.ImportPackageId, packageData.GetApplications.Length);

                // №2. Проверяем совпадения IdentityDocument в случае нескольких заявлений от 1 абитуриента 
                CheckIdentityDocumentsDuplicates();
                List<string> applicationUIDs = new List<string>(); // Для проверки уникальности UID-а application в пакете
                List<string> appNumbers = new List<string>(); // проверка уникальности номероов заявлений
                List<Tuple<IBaseDocument, EntrantDocumentType, string>> parsedDocs = new List<Tuple<IBaseDocument, EntrantDocumentType, string>>();
                Dictionary<int, bool> checkEntrantIsAgreedDate = new Dictionary<int, bool>();

                foreach (var application in packageData.GetApplications)
                {
                    // №1. Проверка уникальности Application.UID в рамках пакета
                    if (applicationUIDs.Contains(application.UID))
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, "Заявление", application.UID);
                    else
                        applicationUIDs.Add(application.UID);


                    // № 3.5. Проверка на уникальность номера заявления (в рамках загружаемого массива)
                    if (appNumbers.Contains(application.ApplicationNumber))
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationNumberIsNotUnique);
                    else
                        appNumbers.Add(application.ApplicationNumber);

                    // № 3.6. Проверка на соответствие номера заявления и UID (по базе)
                    //(Если есть заявление с таким же номером но др. UID или таким же UID, но другим номером
                    //Или таким же UID и номером, но другой датой регистрации – в Конфликт)
                    var sameNumberApps = vocabularyStorage.ApplicationVoc.Items.Where(t => t.ApplicationNumber == application.ApplicationNumber);
                    foreach (var sameNumberApp in sameNumberApps)
                    {
                        if (sameNumberApp.UID != application.UID)
                            conflictStorage.SetObjectIsBroken(application, null, ConflictMessages.ApplicationNumberIsNotCorrelateWithUID);
                    }
                    var sameUIDApps = vocabularyStorage.ApplicationVoc.Items.Where(t => t.UID == application.UID);
                    foreach (var sameUIDApp in sameUIDApps)
                    {
                        if (sameUIDApp.ApplicationNumber != application.ApplicationNumber)
                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationUIDIsNotCorrelateWithNumber);
                        else
                        {
                            if (sameUIDApp.RegistrationDate.Date != application.RegistrationDate.Date)
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationUIDNumberIsNotCorrelateWithDate);
                        }
                    }

                    // 3.8.1.	Отклонили раньше, чем подали => конфликт
                    //if (application.ApplicationDocuments.EduDocuments.First.LastDenyDateSpecified && application.LastDenyDate < application.RegistrationDate)
                    //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationLastDenyDateShouldBeGreaterRegistrationDate);

                    if (application.RegistrationDate > DateTime.Now.Date.AddDays(1.0))
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.IAIncorrentFieldValue, "RegistrationDate");

                    // 3.8.3.	Есть ли статус в справочнике
                    if (!VocabularyStatic.ApplicationStatusTypeVoc.Items.Any(t => t.StatusID == application.StatusID))
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "StatusID");


                    // Если статус == "В приказе" - конфликт
                    // даем импортировать новые заявления со статусами 2 (Новое), 4 (Принято), 6 (отозвано)
                    if (application.StatusID != GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted 
                        && application.StatusID != GVUZ.ServiceModel.Import.ApplicationStatusType.New
                        && application.StatusID != GVUZ.ServiceModel.Import.ApplicationStatusType.Denied)
                    {
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationCannotBeImportedExceptAcceptedOrNewOrDenied);
                    }

                    // Для статуса 6 - надо передать поля ReturnDocumentsTypeId, ReturnDocumentsDate
                    // Проверям, что поля переданы и что нужный тип в справочнике
                    if (application.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Denied
                        && application.ReturnDocumentsDate == DateTime.MinValue)
                    {
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationNeedReturnDocumentsInfo);
                    }

                    if (application.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Denied
                        && !VocabularyStatic.ReturnDocumentsType.Items.Any(t => t.ID == application.ReturnDocumentsTypeId))
                                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "Application.ReturnDocumentsTypeId");

                    //если статус не отозванно, надо игнорировать поля (на уровне базы)

                    ApplicationVocDto applicationDB = sameUIDApps.FirstOrDefault();
                    // 3.8.2.	Проверка текущего статуса, если заявление в приказе, то нельзя его обновлять
                    if (applicationDB != null)
                    {
                        if (applicationDB.OrderOfAdmissionID != 0) // HasValue?
                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationsInOrderIsNotAllowedToUpdate, application.ApplicationNumber);

                        if (vocabularyStorage.RecomendedListsVoc.Items.Any(t => !t.DateDelete.HasValue && t.ApplicationID == applicationDB.ID))
                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationsInRecListIsNotAllowedToUpdate, application.ApplicationNumber);

                        // Заполнить ID и GUID, если они уже есть в базе
                        if (!application.IsBroken)
                        {
                            application.ID = applicationDB.ID;
                            application.GUID = applicationDB.ApplicationGUID.HasValue && !applicationDB.ApplicationGUID.Value.Equals(Guid.Empty) ? applicationDB.ApplicationGUID.Value : Guid.NewGuid();
                        }

                        // Новая проверка по статусам!

                        // заявления со статусами 1 и 8 импортировать и обновлять не даем
                        if (applicationDB.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.InOrder || applicationDB.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Draft)
                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationStatus1Or8CannotBeImported);

                        // статусы 2 и 3 - полный импорт со всеми проверками. Т.е. ничего дополнительно не надо!

                        // нельзя statusID 4, 6 => 2
                        // 23.06.2016 решили, что можно!
                        //if ((applicationDB.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted 
                        //    || applicationDB.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Denied)
                        //    && application.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.New)
                        //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationStatus4Or6CannotBeImportedStatus2);


                        // Проверка, что нет ВИ для заявлений со статусом не равным 2 или 3, иначе сообщение об ошибке
                        //if (applicationDB.StatusID != GVUZ.ServiceModel.Import.ApplicationStatusType.New &&
                        //    applicationDB.StatusID != GVUZ.ServiceModel.Import.ApplicationStatusType.Failed &&
                        //    application.EntranceTestResults != null && application.EntranceTestResults.Length > 0)
                        //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationCannotBeImportedExceptNewOrFailed);


                        // статусы 4, 6 - нельзя редактировать, можно только несколько полей изменить
                        // можно обновлять поля: UID(???), ApplicationNumber, CustomInformation, OriginalReceivedDate(для всех доков), IsAgreedDate
                        if (applicationDB.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted
                            || applicationDB.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.Denied)
                        {
                            // По идее, надо проверить, что указанные CompetitiveGroup в FinSourceAndEduForms полностью соответствуют данным в БД, 
                            // а еще наверно есть какая-то проверка на дату IsAgreedDate
                            application.ShortUpdate = true;
                            //continue;
                        }

                        // статусы 5, 7 - ???

                    }


                    #region "Entrant"
                    var entrant = application.Entrant;
                    if (!application.ShortUpdate)
                    {
                        // 3.7 Справочник Gender
                        int gender = entrant.GenderID.To(0);
                        if (gender != GVUZ.Model.Entrants.GenderType.Male && gender != GVUZ.Model.Entrants.GenderType.Female)
                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "Entrant.GenderID");

                        var entrantItem = vocabularyStorage.EntrantVoc.GetItemByUid(entrant.UID);
                        if (entrantItem != null)
                        {
                            entrant.ID = entrantItem.ID;
                            entrant.GUID = entrantItem.EntrantGUID.HasValue && !entrantItem.EntrantGUID.Value.Equals(Guid.Empty) ? entrantItem.EntrantGUID.Value : Guid.NewGuid();
                        }

                        // 3.1 Проверка непустого Entrant.UID
                        if (string.IsNullOrEmpty(entrant.UID))
                        {
                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.EntrantUIDIsMissing);
                        }

                        // Должен быть заполнен либо Email, либо Адрес, либо и то и то
                        if (string.IsNullOrWhiteSpace(entrant.EmailOrMailAddress.Email) && entrant.EmailOrMailAddress.MailAddress == null)
                        {
                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.EntrantMustHaveEmailOrAddress);
                        }

                        entrant.Email = entrant.EmailOrMailAddress.Email;

                        var mailAddress = entrant.EmailOrMailAddress.MailAddress;
                        if (mailAddress != null)
                        {
                            if (!VocabularyStatic.RegionTypeVoc.Items.Any(t => t.ID == mailAddress.RegionID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "Entrant.EmailOrMailAddress.MailAddress.RegionID");
                            if (!VocabularyStatic.TownTypeVoc.Items.Any(t => t.ID == mailAddress.TownTypeID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "Entrant.EmailOrMailAddress.MailAddress.TownTypeID");
                            if (string.IsNullOrWhiteSpace(mailAddress.Address))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.EntrantMailAddressMustHaveAddress);

                            entrant.RegionID = mailAddress.RegionID.To(0);
                            entrant.TownTypeID = mailAddress.TownTypeID.To(0);
                            entrant.Address = mailAddress.Address;
                        }



                        if (entrant.IsFromKrym != null && !string.IsNullOrWhiteSpace(entrant.IsFromKrym.DocumentUID))
                        {
                            // DocumentTypeID = 1, 15, документ есть в БД или в пакете
                            var inPackage = false;
                            if (application.ApplicationDocuments != null)
                            {
                                if (application.ApplicationDocuments.IdentityDocument != null && application.ApplicationDocuments.IdentityDocument.UID == entrant.IsFromKrym.DocumentUID)
                                    inPackage = true;

                                if (application.ApplicationDocuments.OtherIdentityDocuments != null && application.ApplicationDocuments.OtherIdentityDocuments.Any(t => t.UID == entrant.IsFromKrym.DocumentUID))
                                    inPackage = true;

                                if (application.ApplicationDocuments.CustomDocuments != null && application.ApplicationDocuments.CustomDocuments.Any(t => t.UID == entrant.IsFromKrym.DocumentUID))
                                    inPackage = true;

                                // А не нужно  application.ApplicationDocuments.CompatriotDocuments?
                            }
                            var inDb = false;
                            //if (!inPackage) // Зачем тратить время, если нужный документ уже нашли в пакете
                            //{
                            //    inDb = vocabularyStorage.EntrantDocumentVoc.Items.Any(t => t.UID == entrant.IsFromKrym.DocumentUID && (new int[] { 1, 15 }).Contains(t.DocumentTypeID));
                            //}

                            if (!inPackage && !inDb)
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.EntrantDocumentFromKrymUIDNotFound, entrant.IsFromKrym.DocumentUID);
                        }
                    }
                    #endregion "Entrant"

                    var campaignID = 0;
                    CampaignVocDto dbCampaign = null;

                    #region "FinSourceAndEduForms"
                    if (application.FinSourceAndEduForms != null)
                    {
                        var loadedAcgiDates = new Dictionary<int, List<AcgiDates>>();

                        foreach (var finSource in application.FinSourceAndEduForms)
                        {
                            var competitiveGroup = vocabularyStorage.CompetitiveGroupVoc.GetItemByUid(finSource.CompetitiveGroupUID);

                            // проверка, что есть такой конкурс
                            if (competitiveGroup == null)
                            {
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationContainsCompetitiveGroupNotSpecified, finSource.CompetitiveGroupUID);
                                continue;
                            }
                            else if (competitiveGroup.EducationSourceId == GVUZ.Model.Institutions.EDSourceConst.Target && string.IsNullOrWhiteSpace(finSource.TargetOrganizationUID))
                            {
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationContainsIncorrectTargetOrganizationUID);
                            }

                            finSource.CompetitiveGroupID = competitiveGroup.ID;

                            // проверка #2, что все выбранные КГ относятся к одной и той же ПК (по полю CompetitiveGroup.CampaignID), эта ПК относится к текущему вузу (InstitutonID).  
                            if (campaignID == 0)
                            {
                                campaignID = competitiveGroup.CampaignID;
                                dbCampaign = vocabularyStorage.CampaignVoc.Items.FirstOrDefault(t => t.CampaignID == campaignID);
                            }
                            else if (campaignID != competitiveGroup.CampaignID)
                            {
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationCannotImportedForDifferentCampaigns);
                                continue;
                            }

                            // Если не СПО или Межправительственные соглашения, то нельзя использовать конкурсы без вступительных испытаний
                            //var spo = competitiveGroup.EducationLevelID == GVUZ.Model.Institutions.EDLevelConst.SPO || (dbCampaign != null && dbCampaign.CampaignTypeID == CampaignTypesView.Foreigners);
                            //if (!application.ShortUpdate && !spo && !vocabularyStorage.EntranceTestItemCVoc.Items.Where(x => x.CompetitiveGroupID == competitiveGroup.ID).Any())
                            //{
                            //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationCannotImportedForCGWithoutEntranceTests);
                            //}

                            if (application.SelectedCompetitiveGroupsFull == null)
                                application.SelectedCompetitiveGroupsFull = new List<CompetitiveGroupVocDto>();
                            application.SelectedCompetitiveGroupsFull.Add(competitiveGroup);

                            //finSource.CompetitiveGroupDict = GetCompetitiveGroupDict(application, finSource.CompetitiveGroupUID);

                            if (!string.IsNullOrWhiteSpace(finSource.TargetOrganizationUID))
                            {
                                var cgTarget = vocabularyStorage.CompetitiveGroupTargetVoc.Items.Where(t => t.UID == finSource.TargetOrganizationUID).FirstOrDefault();
                                if (cgTarget == null)
                                {
                                    // FIS-1123, 2015-08-19
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationContainsIncorrectTargetOrganizationUID);
                                }
                            }


                            // Проверки на IsDisagreedDate - несогласия без согласия быть не может FIS-1750 (01.08.2017).
                            #region DisagreedDateCheck
                            if (finSource.IsDisagreedDateSpecified)
                            {
                                var acgiDates = new List<AcgiDates>();
                                if (loadedAcgiDates.ContainsKey(entrant.ID))
                                {
                                    acgiDates = loadedAcgiDates[entrant.ID];
                                }
                                else
                                {
                                    acgiDates = ADOApplicationRepository.CheckEntrantDisagreedDate(entrant.ID);

                                    //var appUIDs = packageData.Applications.Select(t => t.UID);
                                    //acgiDates = acgiDates.Where(t => !appUIDs.Contains(t.UID)).ToList();

                                    foreach (var otherApp in packageData.GetApplications.Where(t => t.EntrantUID == entrant.UID))
                                    {
                                        var agreedFinSources = otherApp.FinSourceAndEduForms.Where(t => t.IsDisagreedDateSpecified);
                                        foreach (var fs in agreedFinSources)
                                        {
                                            var cg = vocabularyStorage.CompetitiveGroupVoc.GetItemByUid(fs.CompetitiveGroupUID);
                                            if (cg != null)
                                            {
                                                var acgiDate = new AcgiDates()
                                                {
                                                    ApplicationID = otherApp.ID,
                                                    UID = otherApp.UID,
                                                    IsAgreedDate = finSource.IsAgreedDateSpecified ? (DateTime?)finSource.IsAgreedDate : (DateTime?)null,
                                                    IsDisagreedDate = finSource.IsDisagreedDate,
                                                    CompetitiveGroupID = cg.CompetitiveGroupID
                                                };                                               
                                            }
                                        }                                     
                                    }
                                    loadedAcgiDates.Add(entrant.ID, acgiDates);
                                }

                                var acgiNoAgreedAndDisagreed = acgiDates.Where(t => t.IsAgreedDate == null && t.IsDisagreedDate != null);

                                    // FIS-1750 (01.08.2017) Несогласие без согласия
                                    if ((acgiNoAgreedAndDisagreed.Count() >= 1))
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationContainsAgreedDateWithoutDisagreed);
                                
                            }
                            #endregion

                            if (finSource.IsAgreedDateSpecified)
                            {
                                //if (application.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.New)
                                //{
                                //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationStatus2ContainsAgreedDate);
                                //}
                                //else 

                                if (new int[] { GVUZ.Model.Institutions.EDLevelConst.Bachelor, GVUZ.Model.Institutions.EDLevelConst.Speciality }.Contains(competitiveGroup.EducationLevelID)
                                    //&& (campaign != null && campaign.CampaignTypeID != 5)
                                    && new int[] { EDFormsConst.O, EDFormsConst.OZ }.Contains(competitiveGroup.EducationFormId)
                                    && competitiveGroup.EducationSourceId != GVUZ.Model.Institutions.EDSourceConst.Paid
                                    )
                                {
                                    var acgiDates = new List<AcgiDates>();
                                    if (loadedAcgiDates.ContainsKey(entrant.ID))
                                    {
                                        acgiDates = loadedAcgiDates[entrant.ID];
                                    }
                                    else
                                    {
                                        acgiDates = ADOApplicationRepository.CheckEntrantAgreedDate(entrant.ID);

                                        //var appUIDs = packageData.Applications.Select(t => t.UID);
                                        //acgiDates = acgiDates.Where(t => !appUIDs.Contains(t.UID)).ToList();



                                        foreach (var otherApp in packageData.GetApplications.Where(t => t.EntrantUID == entrant.UID))
                                        {
                                            var agreedFinSources = otherApp.FinSourceAndEduForms.Where(t => t.IsAgreedDateSpecified);
                                            foreach (var fs in agreedFinSources)
                                            {
                                                var cg = vocabularyStorage.CompetitiveGroupVoc.GetItemByUid(fs.CompetitiveGroupUID);
                                                if (cg != null)
                                                {
                                                    if (new int[] { GVUZ.Model.Institutions.EDLevelConst.Bachelor, GVUZ.Model.Institutions.EDLevelConst.Speciality }.Contains(cg.EducationLevelID)
                                                            && new int[] { EDFormsConst.O, EDFormsConst.OZ }.Contains(cg.EducationFormId)
                                                            && cg.EducationSourceId != GVUZ.Model.Institutions.EDSourceConst.Paid
                                                            )

                                                    {
                                                        var acgiDate = new AcgiDates()
                                                        {
                                                            ApplicationID = otherApp.ID,
                                                            UID = otherApp.UID,
                                                            IsAgreedDate = finSource.IsAgreedDate,
                                                            IsDisagreedDate = finSource.IsDisagreedDateSpecified ? (DateTime?)finSource.IsDisagreedDate : (DateTime?)null,
                                                            CompetitiveGroupID = cg.CompetitiveGroupID
                                                        };
                                                    }
                                                }
                                            }
                                        }

                                        loadedAcgiDates.Add(entrant.ID, acgiDates);
                                    }

                                    /*
                                        Допускается: 
                                        - 0 или 1 Acgi c IsAgreedDate 
                                        - 2, если 1 или 2 с IsDisagreeDate
                                        
                                        Т.е. если (IsAgreeDate - IsDisagreeDate) > 1 или IsAgreeDate > 2 - то ошибка!
                                    */

                                    var dbCampaignYear = dbCampaign.YearStart;
                                    var acgiAgreedAndNoDisagreed = acgiDates.Where(t => t.IsAgreedDate != null && t.IsDisagreedDate == null && t.IsAgreedDate.Value.Year == dbCampaignYear);
                                    var acgiAgreedAndDisagreed = acgiDates.Where(t => t.IsAgreedDate != null && t.IsDisagreedDate != null && t.IsAgreedDate.Value.Year == dbCampaignYear && t.IsDisagreedDate.Value.Year == dbCampaignYear);

                                    // проверка числа согласий
                                    /*TODO:
                                        FIS - 1827
                                        1) 1 - скорей всего тут должно быть >= 1 и >=2!
                                        2) нужна проверка что дата согласия = году начала ПК!
                                    */
                                    if ((acgiAgreedAndNoDisagreed.Count() > 1) || 
                                        (acgiAgreedAndNoDisagreed.Count() + acgiAgreedAndDisagreed.Count() > 2)
                                        )
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationContainsMoreThenOneAgreedDate);
                                }




                                // В сервисе добавить проверку, давать ставить согласие только если у заявления Application.OriginalDocumentsReceived = 1 (FIS-1627)
                                // А он, в свою очередь, когда среди ApplicationDocuments есть с DocumentType.CategoryID = 3 и проставленной OriginalReceivedDate
                                bool noReceivedDate = true;
                                if (application.ApplicationDocuments != null)
                                {
                                    if (application.ApplicationDocuments.EduDocuments != null)
                                    {
                                        noReceivedDate = !application.ApplicationDocuments.EduDocuments
                                            .Select(t => t.Item as IEduDocument)
                                            .Where(t => t != null)
                                            .Any(t => t.OriginalReceivedDateSpecified);
                                    }
                                    
                                    // А у него все равно нет OriginalReceivedDate
                                    if (application.ApplicationDocuments.StudentDocument != null)
                                    {
                                    }
                                }

                                if (noReceivedDate)
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationHasAgreedDateButNoDocumentsWithReceicedDate);
                            }

                            // Проверка - у конкурса в БД или пакете есть ВИ с IsForSPOandVO = 1
                            if (!application.ShortUpdate && finSource.IsForSPOandVO)
                            {
                                if (!vocabularyStorage.EntranceTestItemCVoc.Items.Any(t => t.CompetitiveGroupID == competitiveGroup.ID && t.IsForSPOandVO))
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.CompetitiveGroupMustHaveEntranceTestWithSPO);
                            }

                        } // foreach

                        // Соответствие формы и финансирования обучения - проверка убрана 07.06.2016 FIS-1451  
                        //if (!application.ShortUpdate)
                        //{
                        //    List<Tuple<int, int>> avForms = new List<Tuple<int, int>>();
                        //    foreach (var cg in application.SelectedCompetitiveGroupsFull)
                        //    {
                        //        avForms.AddRange(GetAvailableEducationFormsForCompetitiveGroup(cg.UID));
                        //    }

                        //    int cnt = application.SelectedCompetitiveGroupsFull
                        //        .Select(x => new Tuple<int, int>(x.EducationFormId, x.EducationSourceId))
                        //        .Except(avForms).Count();
                        //    if (cnt > 0)
                        //    {
                        //        conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationContainsNotAllowedFinSourceEducationForm);
                        //    }
                        //}
                    }

                    #endregion "FinSourceAndEduForms"



                    #region "EntranceTestResults"
                    if (!application.ShortUpdate && application.EntranceTestResults != null)
                    {
                        var entrTestWithAppEntrTestIds = new Dictionary<int, string>(); // Для поиска дублей по EntranceTestItemCID
                        // 3.2 проверить уникальность UID в рамках данного типа
                        // CheckUniqueUID(application.EntranceTestResults, application);
                        var dupUIDs = application.EntranceTestResults.GroupBy(c => c.UID).Where(c => c.Count() > 1).Select(c => c.Key).ToList();
                        foreach(var uid in dupUIDs)
                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, "application.EntranceTestResults", uid);

                        foreach (var etResult in application.EntranceTestResults)
                        {
                            // 3.7 проверить наличие значений справочников
                            // check etResult.CompetitiveGroupID
                            etResult.CompetitiveGroupDict = GetCompetitiveGroupDict(application, etResult.CompetitiveGroupUID);

                            // 3.17. Есть ли для данных результатов ВИ соответствующая КГ
                            if (!string.IsNullOrWhiteSpace(etResult.CompetitiveGroupUID) && !application.SelectedCompetitiveGroupsFull.Any(t=> t.UID == etResult.CompetitiveGroupUID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.EntranceTestIsNotPartOfCompetitiveGroup, etResult.UID);

                            // check etResult.EntranceTestTypeID 
                            if (!VocabularyStatic.EntranceTestTypeVoc.Items.Any(t => t.EntranceTestTypeID == etResult.EntranceTestTypeID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EntranceTestResult.EntranceTestTypeID");


                            // check etResult.ResultSourceTypeID (3.10.2)
                            if (!VocabularyStatic.EntranceTestResultSourceVoc.Items.Any(t => t.SourceID == etResult.ResultSourceTypeID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EntranceTestResult.ResultSourceTypeID");




                            // check etResult.EntranceTestSubject.SubjectId или SubjectName - просто проверить на наличие в справочнике Subject (3.10.1)
                            SubjectVocDto subjectVoc = null;
                            int? subjectId = null;
                            string subjectName = null;
                            if (etResult.EntranceTestSubject != null)
                            {
                                if (etResult.EntranceTestSubject.Item is uint)
                                {
                                    subjectId = ((uint)etResult.EntranceTestSubject.Item).To(0);
                                    subjectVoc = VocabularyStatic.SubjectVoc.Items.Where(t => t.SubjectID == subjectId).FirstOrDefault();
                                    if (subjectVoc == null)
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EntranceTestResult.EntranceTestSubject.SubjectId");
                                }
                                else
                                {
                                    subjectName = etResult.EntranceTestSubject.Item.ToString();
                                    // Примечательно, что в базе есть EntranceTestItemC, у которых такие SubjectName, которых нет в справочнике Subject!
                                    //if (!vocabularyStorage.SubjectVoc.Items.Any(t => t.Name == subjectName))
                                    //    conflictStorage.SetApplicationIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EntranceTestResult.EntranceTestSubject.SubjectName");
                                }
                            }


                            if (etResult.CompetitiveGroupDict != null && (subjectId.HasValue || !string.IsNullOrEmpty(subjectName))) // А иначе уже был записан конфликт
                            {
                                int CompetitiveGroupId = etResult.CompetitiveGroupDict.ID;
                                etResult.EntranceTestItemC = vocabularyStorage.EntranceTestItemCVoc.Items.Where(t => t.CompetitiveGroupID == CompetitiveGroupId
                                                                            && t.EntranceTestTypeID == etResult.EntranceTestTypeID
                                                                            && ((subjectId.HasValue && t.SubjectID == subjectId)
                                                                                || (!string.IsNullOrEmpty(subjectName) && t.SubjectName == subjectName))).FirstOrDefault();
                            }

                            if (etResult.ResultDocument != null)
                            {
                                if (etResult.ResultDocument.Item is TInstitutionDocument)
                                {
                                    var doc = etResult.ResultDocument.Item as TInstitutionDocument;
                                    if (doc.DocumentTypeIDSpecified)
                                    {
                                        // check doc.DocumentTypeID
                                        if (!VocabularyStatic.InstitutionDocumentTypeVoc.Items.Any(t => t.InstitutionDocumentTypeID == doc.DocumentTypeID))
                                            conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "InstitutionDocument.DocumentTypeID");

                                        // 3.9.1.	Проверка «Указан неверный тип документа-основания для документа о внутреннем испытании ОУ»
                                        // если EntranceTestResult.ResultDocument - InstitutionDocument, то ResultSourceTypeID д.б. = 2 (InstitutionEntranceTest)
                                        if (etResult.ResultSourceTypeID != (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.InstitutionEntranceTest)
                                            //conflictStorage.SetApplicationIsBroken(application, ConflictMessages.DocumentNotAttachedToEntranceTestResult, "Указан неверный тип документа-основания для документа о внутреннем испытании ОУ");
                                            conflictStorage.SetObjectIsBroken(application,
                                               new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                               {
                                                   Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                                                   Message = "Указан неверный тип документа-основания для документа о внутреннем испытании ОУ"
                                               });
                                    }
                                }
                                else if (etResult.ResultDocument.Item is TOlympicDocument)
                                {
                                    var doc = etResult.ResultDocument.Item as TOlympicDocument;
                                    CheckOlympicDocument(application, doc);

                                    CheckBenefitOlympic(application, etResult, doc);

                                }
                                else if (etResult.ResultDocument.Item is TOlympicTotalDocument)
                                {
                                    var doc = etResult.ResultDocument.Item as TOlympicTotalDocument;
                                    CheckOlympicTotalDocument(application, doc);

                                    CheckBenefitOlympic(application, etResult, doc);

                                    //// 3.9.2.	Проверка  «Указан неверный тип документа-основания для диплома победителя/призера олимпиады»
                                    //// если EntranceTestResult.ResultDocument - OlympicDocument или OlympicTotalDocument, то ResultSourceTypeID д.б. = 3 (OlympicDocument)
                                    //if (etResult.ResultSourceTypeID != (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.OlympicDocument)
                                    //    conflictStorage.SetObjectIsBroken(application,
                                    //           new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                    //           {
                                    //               Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                                    //               Message = "Указан неверный тип документа-основания для диплома победителя/призера олимпиады"
                                    //           });

                                }
                                //FIS - 1768 (30.10.2017 akopylov) не хватает документов для льготы 100 баллов
                                //TSportDocument
                                //TUkraineOlympic
                                //TInternationalOlympic
                                //TCustomDocument
                                else if (etResult.ResultDocument.Item is TSportDocument)
                                {
                                    var doc = etResult.ResultDocument.Item as TSportDocument;
                                    CheckSportDocument(application, doc);
                                }
                                else if (etResult.ResultDocument.Item is TUkraineOlympic)
                                {
                                    var doc = etResult.ResultDocument.Item as TUkraineOlympic;
                                    CheckUkraineOlympicDocument(application, doc);
                                }
                                else if (etResult.ResultDocument.Item is TInternationalOlympic)
                                {
                                    var doc = etResult.ResultDocument.Item as TInternationalOlympic;
                                    CheckInternationalOlympicDocument(application, doc);
                                }
                                else if (etResult.ResultDocument.Item is TCustomDocument)
                                {
                                    var doc = etResult.ResultDocument.Item as TCustomDocument;
                                    //Добавлю на всякий случай сразу проверку, что тип ВИ - льгота 100 баллов, а также что ПК не бак/спец
                                    if (dbCampaign.CampaignTypeID == CampaignTypesView.BachelorAndSpeciality ||
                                        etResult.ResultSourceTypeID != (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.OlympicDocument)
                                        conflictStorage.SetObjectIsBroken(application,
                                        new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                        {
                                            Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                                            Message = "Иной документ может подтверждать только льготу 100 баллов (не для бакалавриата/специалитета)"
                                        });
                                }


                                else if (etResult.ResultDocument.Item is string)
                                {
                                    // 3.9.3.	Проверка  «Указан неверный тип документа-основания»
                                    // если EntranceTestResult.ResultDocument - строка-ссылка на документ ЕГЭ или ГИА, то ResultSourceTypeID д.б. = 1 (EgeDocument) или 4 (GiaDocument)
                                    if (etResult.ResultSourceTypeID != (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.EgeDocument &&
                                        etResult.ResultSourceTypeID != (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.GiaDocument)
                                        //conflictStorage.SetApplicationIsBroken(application, ConflictMessages.DocumentNotAttachedToEntranceTestResult, "Указан неверный тип документа-основания");
                                        conflictStorage.SetObjectIsBroken(application,
                                               new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                               {
                                                   Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                                                   Message = "Указан неверный тип документа-основания"
                                               });

                                    // 3.9.4, 3.9.5 UID -> ID, проверка на наличие свидетельства ЕГЭ/ГИА, прилож. к заявлению
                                    // ищем ЕГЭ свидетельство в данных пользователя
                                    var egeDocumentUID = etResult.ResultDocument.Item.ToString();
                                    bool isCorrect = false;
                                    if (application.ApplicationDocuments != null && application.ApplicationDocuments.EgeDocuments != null
                                        && etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.EgeDocument)
                                    {
                                        if (application.ApplicationDocuments.EgeDocuments.Count(x => !String.IsNullOrWhiteSpace(x.UID) && x.UID == egeDocumentUID) == 1)
                                            isCorrect = true;
                                    }

                                    //ищем справку ГИА в данных
                                    if (application.ApplicationDocuments != null && application.ApplicationDocuments.GiaDocuments != null
                                        && etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.GiaDocument)
                                    {
                                        if (application.ApplicationDocuments.GiaDocuments.Count(x => !String.IsNullOrWhiteSpace(x.UID) && x.UID == egeDocumentUID) == 1)
                                            isCorrect = true;
                                    }

                                    if (!isCorrect)
                                    {
                                        //мы тут, если правильные случаи кончились
                                        if (application.ApplicationDocuments != null && application.ApplicationDocuments.EgeDocuments != null)
                                        {
                                            //приложен документ к не тому типу РВИ
                                            if (application.ApplicationDocuments.EgeDocuments.Count(x => !String.IsNullOrWhiteSpace(x.UID) && x.UID == egeDocumentUID) == 1)
                                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.NotAllowedEgeDocumentForEntranceTestResult);
                                        }
                                        else
                                        {
                                            //нет нужных документов
                                            if (etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.GiaDocument)
                                            {
                                                conflictStorage.SetObjectIsBroken(application,
                                                new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                                {
                                                    Code = ConflictMessages.ReferencedEgeDocumentIsAbsent,
                                                    Message = String.Format("В списке документов заявления не найдено указанной справки ГИА {0} для вступительного испытания {1}.",
                                                                            egeDocumentUID, etResult.UID)
                                                });
                                            }
                                            else
                                            {
                                                conflictStorage.SetObjectIsBroken(application,
                                                    new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                                    {
                                                        Code = ConflictMessages.ReferencedEgeDocumentIsAbsent,
                                                        Message = String.Format("В списке документов заявления не найдено указанного ЕГЭ документа {0} для вступительного испытания {1}.",
                                                                            egeDocumentUID, etResult.UID)
                                                    });
                                            }
                                        }
                                    }

                                    // 3.10.3.	проверка на ЕГЭ предмет
                                    bool isEge = subjectVoc != null ? subjectVoc.IsEge : false;

                                    // 3.10.3.1 кастомный предмет, а источник свидетельство
                                    // 3.10.3.2 не ЕГЭ предмет со свидетельством
                                    if (etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.EgeDocument &&
                                        (subjectVoc == null || (subjectVoc != null && !isEge)))
                                    {
                                        conflictStorage.SetObjectIsBroken(application, etResult, ConflictMessages.NotAllowedSubjectForEgeDocument);
                                    }


                                    // 3.10.3.3 для ГИА проверка - кастомный предмет со справкой
                                    if (subjectVoc == null && etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.GiaDocument)
                                    {
                                        conflictStorage.SetObjectIsBroken(application, etResult, ConflictMessages.NotAllowedSubjectForGiaDocument);
                                    }

                                    // 3.10.3.4 указан ли предмет в справке ГИА
                                    if (subjectVoc != null && etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.GiaDocument)
                                    {
                                        //проверка на корректность документа в другом месте. Тут именно предмет проверяем
                                        if (application.ApplicationDocuments != null && application.ApplicationDocuments.GiaDocuments != null)
                                        {
                                            var giaDoc = application.ApplicationDocuments.GiaDocuments.Where(x => x.UID == etResult.ResultDocument.Item.ToString()).FirstOrDefault();
                                            if (giaDoc != null)
                                            {
                                                if (!giaDoc.Subjects.Any(x => x.SubjectID == subjectVoc.SubjectID))
                                                {
                                                    conflictStorage.SetObjectIsBroken(application, etResult, ConflictMessages.NotAllowedSubjectForGiaDocument);
                                                }
                                            }
                                        }
                                    }
                                }
                            } // if (etResult.ResultDocument != null)

                            // 3.10.4. ещё раз проверяем предмет, если документ основание не приложен. Но ЕГЭ
                            if (etResult.ResultDocument == null && etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.EgeDocument)
                            {
                                if (subjectVoc == null)
                                {
                                    conflictStorage.SetObjectIsBroken(application, etResult, ConflictMessages.NotAllowedSubjectForEgeDocument);
                                }
                            }

                            // Шкала 100 баллов для LevelID - 2, 4, 5, 1000 баллов для 17, 18
                            decimal maxValue = etResult.CompetitiveGroupDict != null &&
                                new int[] { Model.Institutions.EDLevelConst.Bachelor,
                                    //Model.Institutions.EDLevelConst.Magistracy,  //FIS-1679
                                    Model.Institutions.EDLevelConst.Speciality }.Contains(etResult.CompetitiveGroupDict.EducationLevelID) ?
                                100 : 999;
                            if (etResult.ResultValue < 0 || etResult.ResultValue > maxValue)
                            {
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ResultValueRange, etResult.ResultValue.ToString(), maxValue.ToString());

                                //new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                //{
                                //    Code = ConflictMessages.ResultValueRange,
                                //    Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.ResultValueRange), etResult.ResultValue)
                                //});
                            }

                            //switch ((GVUZ.Model.Entrants.EntranceTestResultSourceEnum)etResult.ResultSourceTypeID)
                            //{
                            //    case GVUZ.Model.Entrants.EntranceTestResultSourceEnum.EgeDocument:
                            //    case GVUZ.Model.Entrants.EntranceTestResultSourceEnum.GiaDocument:
                            //    case GVUZ.Model.Entrants.EntranceTestResultSourceEnum.InstitutionEntranceTest:
                            //        //if (etResult.ResultValue != null /* 0 */ ) //есть результат, проверям, подходит ли он
                            //        {
                            //            if (etResult.ResultValue < 0 || etResult.ResultValue > 100)
                            //            {
                            //                conflictStorage.SetObjectIsBroken(application,
                            //                    new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                            //                    {
                            //                        Code = ConflictMessages.ResultValueRange,
                            //                        Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.ResultValueRange), etResult.ResultValue)
                            //                    });
                            //            }
                            //        }
                            //        break;
                            //    case GVUZ.Model.Entrants.EntranceTestResultSourceEnum.OlympicDocument:
                            //          break;
                            //}





                            // 3.11.1.	ищем в базе нужное испытание
                            if (etResult.CompetitiveGroupDict != null && (subjectId.HasValue || !string.IsNullOrEmpty(subjectName))) // А иначе уже был записан конфликт
                            {
                                //int CompetitiveGroupId = etResult.CompetitiveGroupDict.ID;
                                //etResult.EntranceTestItemC = vocabularyStorage.EntranceTestItemCVoc.Items.Where(t => t.CompetitiveGroupID == CompetitiveGroupId
                                //                                            && t.EntranceTestTypeID == etResult.EntranceTestTypeID
                                //                                            && ((subjectId.HasValue && t.SubjectID == subjectId)
                                //                                                || (!string.IsNullOrEmpty(subjectName) && t.SubjectName == subjectName))).FirstOrDefault();

                                if (etResult.EntranceTestItemC == null)
                                {
                                    conflictStorage.SetObjectIsBroken(application, etResult, ConflictMessages.CompetitiveGroupEntranceTestNotFoundForEntranceTestResult);
                                }
                                else
                                {
                                    // 3.11.2.	проверяем на дубли
                                    if (entrTestWithAppEntrTestIds.ContainsKey(etResult.EntranceTestItemC.EntranceTestItemID))
                                    {
                                        GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage conflictMessage = new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                        {
                                            Code = ConflictMessages.EntranceTestResultAlreadyImportedForEntranceTestItem,
                                            Message = String.Format(
                                                ConflictMessages.GetMessage(ConflictMessages.EntranceTestResultAlreadyImportedForEntranceTestItem),
                                                etResult.EntranceTestItemC.UID, entrTestWithAppEntrTestIds[etResult.EntranceTestItemC.EntranceTestItemID])
                                        };

                                        conflictStorage.SetObjectIsBroken(application, conflictMessage);
                                    }
                                    else
                                    {
                                        entrTestWithAppEntrTestIds.Add(etResult.EntranceTestItemC.EntranceTestItemID, etResult.UID);
                                    }
                                }
                            }

                            // Выполняется проверка что ApplicationEntranceTestDocument.EgeResultValue >= BenefitItemC.EgeMinValue , 
                            // т.е. ApplicationEntranceTestDocument.EgeResultValue >= BenefitItemSubject.EgeMinValue 
                            // (надо учесть, что в обоих случаях EgeMinValue может быть NULL, тогда проверку не делаем)
                            if (etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.OlympicDocument)
                            {
                                if (etResult.EntranceTestItemC != null)
                                {
                                    //int eticID = etResult.EntranceTestItemC.EntranceTestItemID;
                                    //var benefitC = vocabularyStorage.BenefitItemCVoc.Items.Where(t => t.EntranceTestItemID == eticID);
                                    // ну хорошо, а если их несколько тут?
                                    int cgID = etResult.CompetitiveGroupDict != null ? etResult.CompetitiveGroupDict.ID : 0;
                                    int subjectID = subjectId.HasValue ? subjectId.Value : 0;
                                    TOlympicDocument olympicDocument = etResult.ResultDocument != null ? etResult.ResultDocument.Item as TOlympicDocument : null;

                                    int olympicID = (olympicDocument != null) ? olympicDocument.OlympicID.To(0) : 0;
                                    int egeMinValue = ADOApplicationRepository.GetBenefitEgeMinValue(cgID, subjectID, olympicID);

                                    if (etResult.ResultValue < egeMinValue)
                                    {
                                        // Результат испытания (значение {0}) должен принимать значения от 0 до 100 включительно
                                        conflictStorage.SetObjectIsBroken(application,
                                            new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                            {
                                                Code = ConflictMessages.ResultValueRange,
                                                Message = String.Format("Балл ЕГЭ ({0}) для использования льготы не может быть меньше минимального балла, указанного для ВИ в конкурсе", etResult.ResultValue)
                                            });
                                    }
                                }
                            }

                            // 3.11.4. проверяем в зависимости от типа
                            string egeDocumentID = etResult.ResultDocument != null && etResult.ResultDocument.Item is string ? etResult.ResultDocument.Item.ToString() : "";
                            switch ((GVUZ.Model.Entrants.EntranceTestResultSourceEnum)etResult.ResultSourceTypeID)
                            {
                                case GVUZ.Model.Entrants.EntranceTestResultSourceEnum.EgeDocument:
                                    if (etResult.ResultDocument == null || String.IsNullOrWhiteSpace(egeDocumentID))
                                    {
                                        bool isInNotOrderDb = applicationDB == null || applicationDB.OrderOfAdmissionID == 0;
                                        if (isInNotOrderDb) //если не включено в приказ, тогда разрешено не иметь источника. #22904
                                            break;

                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
                                        continue;
                                    }

                                    //указано свидетельство ЕГЭ, а самого свидетельства нет
                                    if (application.ApplicationDocuments == null || application.ApplicationDocuments.EgeDocuments == null ||
                                        !application.ApplicationDocuments.EgeDocuments.Where(x => x.UID == egeDocumentID).Any())
                                    {
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
                                        continue;
                                    }

                                    break;

                                case GVUZ.Model.Entrants.EntranceTestResultSourceEnum.GiaDocument:
                                    if (etResult.ResultDocument == null || String.IsNullOrWhiteSpace(egeDocumentID))
                                    {
                                        bool isInNotOrderDb = applicationDB == null || applicationDB.OrderOfAdmissionID == 0;
                                        if (isInNotOrderDb) //если не включено в приказ, тогда разрешено не иметь источника. #22904
                                            break;

                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
                                        continue;
                                    }


                                    //указана справка ГИА, а самой справки нет
                                    if (application.ApplicationDocuments == null || application.ApplicationDocuments.GiaDocuments == null ||
                                        !application.ApplicationDocuments.GiaDocuments.Where(x => x.UID == egeDocumentID).Any())
                                    {
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
                                        continue;
                                    }

                                    break;

                                case GVUZ.Model.Entrants.EntranceTestResultSourceEnum.OlympicDocument:
                                    //право на 100 баллов, а документа нет
                                    if (etResult.ResultDocument == null ||
                                        (!(etResult.ResultDocument.Item is TOlympicDocument)
                                        && !(etResult.ResultDocument.Item is TOlympicTotalDocument)
                                        && !(etResult.ResultDocument.Item is TSportDocument)
                                        && !(etResult.ResultDocument.Item is TUkraineOlympic)
                                        && !(etResult.ResultDocument.Item is TInternationalOlympic)
                                        && !(etResult.ResultDocument.Item is TCustomDocument)
                                        ))
                                    {
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
                                        continue;
                                    }

                                    //FIS-1768 (24.08.2017) - Для бакалавриата/специалитета не может быть иной документ
                                    if (dbCampaign != null && dbCampaign.CampaignTypeID == CampaignTypesView.BachelorAndSpeciality && etResult.ResultDocument.Item is TCustomDocument)
                                    {
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
                                        continue;
                                    }
                                    break;
                            }

                            if ((etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.InstitutionEntranceTest)
                                && (etResult.ResultDocument == null || !(etResult.ResultDocument.Item is TInstitutionDocument) || (!(etResult.ResultDocument.Item as TInstitutionDocument).DocumentTypeIDSpecified)))
                            {
                                conflictStorage.SetObjectIsBroken(application,
                                   new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                                   {
                                       Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                                       Message = "Для внутренних вступительных испытаний необходимо передавать подтверждающий документ соответствующего типа (InstitutionDocument)"
                                   });
                            }

                            if (etResult.IsDisabled != null)
                            {
                                // Проверить, что такой документ есть в БД или пакете c UID = etResult.IsDisabled.DisabledDocumentUID

                                // DocumentTypeID = 1, 15, документ есть в БД или в пакете
                                var uid = etResult.IsDisabled.DisabledDocumentUID;
                                var inPackage = false;
                                if (application.ApplicationDocuments != null)
                                {
                                    inPackage = application.ApplicationDocuments.AllDocuments().Any(t => t.UID == uid);
                                    
                                    //if (application.ApplicationDocuments.IdentityDocument != null && application.ApplicationDocuments.IdentityDocument.UID == uid)
                                    //    inPackage = true;
                                    //if (application.ApplicationDocuments.OtherIdentityDocuments != null && application.ApplicationDocuments.OtherIdentityDocuments.IdentityDocument.UID == uid)
                                    //    inPackage = true;
                                    //if (application.ApplicationDocuments.CustomDocuments != null && application.ApplicationDocuments.CustomDocuments.Any(t => t.UID == uid))
                                    //    inPackage = true;
                                }
                                //var inDb = false;
                                //if (!inPackage) // Зачем тратить время, если нужный документ уже нашли в пакете
                                //{
                                //    inDb = vocabularyStorage.EntrantDocumentVoc.Items.Any(t => t.UID == uid 
                                //                //&& (new int[] { 1, 15 }).Contains(t.DocumentTypeID)
                                //            );
                                //}

                                if (!inPackage) //  && !inDb)
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.EntrantDocumentVIisDisabledUIDNotFound, uid);
                            }

                        } // foreach
                    }
                    #endregion "EntranceTestResults"

                    #region "Benefits"
                    if (!application.ShortUpdate)
                    {
                        // 3.3 проверить уникальность UID в рамках данного типа
                        CheckUniqueUID(application.getAllBenefits().ToArray(), application);
                        foreach (var benefit in application.getAllBenefits())
                        {

                            // 3.7 проверить наличие значений справочников
                            // 1) BenefitKindID
                            if (!VocabularyStatic.BenefitVoc.Items.Any(t => t.BenefitID == benefit.BenefitKindID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "ApplicationCommonBenefit.BenefitKindID");

                            // 3.10.6.	проверить целостность данных для общей льготы 
                            var benefitKindID = benefit.BenefitKindID;
                            if (benefitKindID != 1 && benefitKindID != 4 && benefitKindID != 5)
                            {
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.NotAllowedCommonBenefitTypeForApplication);
                            }



                            if (benefit.DocumentReason == null)
                            {
                                //#40117 Сделать документ-основание общей льготы необязательной
                                // ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.DocumentNotSpecifiedForBenefit);
                                // return;
                            }

                            // 2)  DocumentReason
                            if (benefit.DocumentReason != null)
                            {


                                Model.Entrants.Documents.EntrantDocumentType documentType = EntrantDocumentType.CustomDocument; //1 - Olympic, 2 - OlympicTotal, 3 - MedicalDocuments, 4 - Custom
                                if (benefit.DocumentReason.Item is TOlympicDocument)
                                {
                                    var olympicDocument = benefit.DocumentReason.Item as TOlympicDocument;
                                    CheckOlympicDocument(application, olympicDocument);
                                    CheckDocumentTypeCorrectness(benefit.DocumentReason.Item as IBaseDocument, application, EntrantDocumentType.OlympicDocument, parsedDocs);
                                    documentType = olympicDocument.EntrantDocumentType;
                                }
                                else if (benefit.DocumentReason.Item is TOlympicTotalDocument)
                                {
                                    var olympicTotalDocument = benefit.DocumentReason.Item as TOlympicTotalDocument;
                                    CheckOlympicTotalDocument(application, olympicTotalDocument);
                                    CheckDocumentTypeCorrectness(benefit.DocumentReason.Item as IBaseDocument, application, EntrantDocumentType.OlympicTotalDocument, parsedDocs);
                                    documentType = olympicTotalDocument.EntrantDocumentType;
                                }
                                else if (benefit.DocumentReason.Item is PackageDataApplicationApplicationCommonBenefitDocumentReasonMedicalDocuments)
                                {
                                    var medicalDocuments = benefit.DocumentReason.Item as PackageDataApplicationApplicationCommonBenefitDocumentReasonMedicalDocuments;
                                    if (medicalDocuments.BenefitDocument != null)
                                    {
                                        if (medicalDocuments.BenefitDocument.Item is TDisabilityDocument)
                                        {
                                            var disabilityDocument = medicalDocuments.BenefitDocument.Item as TDisabilityDocument;
                                            // check disabilityDocument.DisabilityTypeID
                                            if (!VocabularyStatic.DisabilityTypeVoc.Items.Any(t => t.DisabilityID == disabilityDocument.DisabilityTypeID))
                                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "DisabilityDocument.DisabilityTypeID");
                                            CheckDocumentTypeCorrectness(disabilityDocument, application, EntrantDocumentType.DisabilityDocument, parsedDocs);
                                            documentType = disabilityDocument.EntrantDocumentType;
                                        }
                                        else
                                        {
                                            var medicalDocument = medicalDocuments.BenefitDocument.Item as TMedicalDocument;
                                            CheckDocumentTypeCorrectness(medicalDocument, application, EntrantDocumentType.MedicalDocument, parsedDocs);
                                            documentType = medicalDocument.EntrantDocumentType;
                                        }
                                    }
                                    else
                                    {
                                        CheckDocumentTypeCorrectness(medicalDocuments.AllowEducationDocument, application, EntrantDocumentType.AllowEducationDocument, parsedDocs);
                                        documentType = medicalDocuments.AllowEducationDocument.EntrantDocumentType;
                                    }

                                }
                                else if (benefit.DocumentReason.Item is TInternationalOlympic)
                                {
                                    var document = benefit.DocumentReason.Item as TInternationalOlympic;
                                    documentType = document.EntrantDocumentType;
                                    CheckInternationalOlympicDocument(application, document);
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);

                                }
                                else if (benefit.DocumentReason.Item is TOrphanDocument)
                                {
                                    var document = benefit.DocumentReason.Item as TOrphanDocument;
                                    documentType = document.EntrantDocumentType;
                                    CheckOrphanDocument(application, document);
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);
                                }
                                else if (benefit.DocumentReason.Item is TSportDocument)
                                {
                                    var document = benefit.DocumentReason.Item as TSportDocument;
                                    documentType = document.EntrantDocumentType;
                                    CheckSportDocument(application, document);
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);
                                }
                                else if (benefit.DocumentReason.Item is TUkraineOlympic)
                                {
                                    var document = benefit.DocumentReason.Item as TUkraineOlympic;
                                    documentType = document.EntrantDocumentType;
                                    CheckUkraineOlympicDocument(application, document);
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);
                                }
                                else if (benefit.DocumentReason.Item is TVeteranDocument)
                                {
                                    var document = benefit.DocumentReason.Item as TVeteranDocument;
                                    documentType = document.EntrantDocumentType;
                                    CheckVeteranDocument(application, document);
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);
                                }
                                else if (benefit.DocumentReason.Item is TPauperDocument)
                                {
                                    var document = benefit.DocumentReason.Item as TPauperDocument;
                                    documentType = document.EntrantDocumentType;
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);
                                }
                                else if (benefit.DocumentReason.Item is TParentsLostDocument)
                                {
                                    var document = benefit.DocumentReason.Item as TParentsLostDocument;
                                    documentType = document.EntrantDocumentType;
                                    CheckParentsLostDocument(application, document);
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);
                                }
                                else if (benefit.DocumentReason.Item is TStateEmployeeDocument)
                                {
                                    var document = benefit.DocumentReason.Item as TStateEmployeeDocument;
                                    documentType = document.EntrantDocumentType;
                                    CheckStateEmployeeDocument(application, document);
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);
                                }
                                else if (benefit.DocumentReason.Item is TRadiationWorkDocument)
                                {
                                    var document = benefit.DocumentReason.Item as TRadiationWorkDocument;
                                    documentType = document.EntrantDocumentType;
                                    CheckRadiationWorkDocument(application, document);
                                    CheckDocumentTypeCorrectness(document, application, documentType, parsedDocs);
                                }
                                else
                                {
                                    // Иначе customDocument
                                    CheckDocumentTypeCorrectness(benefit.DocumentReason.Item as IBaseDocument, application, EntrantDocumentType.CustomDocument, parsedDocs);
                                }


                                //#22899 Если документ-основание (DocumentReason) не соответствует виду льготы (BenefitKindID), то ошибки не возникает
                                if (benefitKindID == 1) //Зачисление без вступительных испытаний
                                {
                                    //DocumentTypeID судя по всему не используется
                                    /*CustomDocument == null && OlympicDocument == null && OlympicTotalDocument == null*/
                                    if (documentType != EntrantDocumentType.OlympicDocument
                                        && documentType != EntrantDocumentType.OlympicTotalDocument
                                        && documentType != EntrantDocumentType.SportOlympicChampion
                                        && documentType != EntrantDocumentType.SportParaolympicChampion
                                        && documentType != EntrantDocumentType.SportDeaflympicChampion
                                        && documentType != EntrantDocumentType.SportWorldChampion
                                        && documentType != EntrantDocumentType.SportEuropeChampion
                                        && documentType != EntrantDocumentType.UkraineOlympic
                                        && documentType != EntrantDocumentType.InternationalOlympic
                                        && documentType != EntrantDocumentType.CustomDocument)
                                    {
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
                                    }

                                    //FIS-1768 (24.08.2017) - Для бакалавриата/специалитета не может быть иной документ
                                    if (dbCampaign != null && dbCampaign.CampaignTypeID == CampaignTypesView.BachelorAndSpeciality && documentType == EntrantDocumentType.CustomDocument)
                                    {
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
                                    }
                                }

                                if (benefitKindID == 4) //По квоте приёма лиц, имеющих особое право
                                {
                                    //if (appCommonBenefitDto.DocumentReason.CustomDocument == null && appCommonBenefitDto.DocumentReason.MedicalDocuments == null)
                                    if (documentType != EntrantDocumentType.AllowEducationDocument  // 2) дети-инвалиды, инвалиды I и II групп
                                        && documentType != EntrantDocumentType.DisabilityDocument   // 2) дети-инвалиды, инвалиды I и II групп
                                        && documentType != EntrantDocumentType.MedicalDocument      // 2) дети-инвалиды, инвалиды I и II групп
                                        && documentType != EntrantDocumentType.OrphanDocument
                                        && documentType != EntrantDocumentType.VeteranDocument
                                        )
                                    {
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
                                    }
                                }

                                if (benefitKindID == 5) //Преимущественное право
                                {
                                    //п. 35 порядка приема 1147  преимущественное право зачисления предоставляется следующим лицам
                                    if (documentType != EntrantDocumentType.OrphanDocument              // 1) дети-сироты и дети, оставшиеся без попечения родителей
                                        && documentType != EntrantDocumentType.DisabilityDocument       // 2) дети-инвалиды, инвалиды I и II групп
                                        && documentType != EntrantDocumentType.AllowEducationDocument   // 2) дети-инвалиды, инвалиды I и II групп
                                        && documentType != EntrantDocumentType.MedicalDocument          // 2) дети-инвалиды, инвалиды I и II групп - не понятно надо ли это, в интерфейсе нет!                                       
                                        && documentType != EntrantDocumentType.PauperDocument           // 3) граждане в возрасте до двадцати лет, имеющие только одного родителя инвалида
                                        && documentType != EntrantDocumentType.RadiationWorkDocument    // 4,12) граждане, непосредственно принимавшие участие в испытаниях ядерного оружия и тд
                                        && documentType != EntrantDocumentType.ParentsLostDocument      // 5,6,7,8) дети ..., погибших при исполнении ими обязанностей
                                        && documentType != EntrantDocumentType.StateEmployeeDocument    // 9,10,13) сотрудники госорганов РФ
                                        && documentType != EntrantDocumentType.VeteranDocument          // 11) ветераны боевых действий                                                                             
                                        && documentType != EntrantDocumentType.CustomDocument)
                                    { 
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
                                    }

                                    //FIS-1768 (24.08.2017) - Для бакалавриата/специалитета не может быть иной документ
                                    if (dbCampaign != null && dbCampaign.CampaignTypeID == CampaignTypesView.BachelorAndSpeciality && documentType == EntrantDocumentType.CustomDocument)
                                    {
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
                                    }
                                }
                            }

                            // 3) DocumentTypeID - если указан, то должен быть в справочнике?
                            // Поле не используется при импорте, проверка отменена
                            //benefit.DocumentTypeID

                            // 4) CompetitiveGroupID
                            benefit.CompetitiveGroupDict = GetCompetitiveGroupDict(application, benefit.CompetitiveGroupUID);

                            if (!application.SelectedCompetitiveGroupsFull.Any(t => t.UID == benefit.CompetitiveGroupUID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationBenefitsContainsCompetitiveGroupNotSpecified, benefit.CompetitiveGroupUID);

                            if (benefitKindID == 4 && benefit.CompetitiveGroupDict != null && benefit.CompetitiveGroupDict.EducationSourceId != Model.Institutions.EDSourceConst.Quota)
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationBenefitsContainsBenefit4InCGNotSource20, benefit.CompetitiveGroupUID);

                        }
                    }
                    #endregion "Benefits"

                    #region "ApplicationDocuments"
                    // RomanNB: может часть проверок по заявлениям стоит оставить и для "быстрого импорта"?
                    if (!application.ShortUpdate && application.ApplicationDocuments != null) 
                    {
                        var applicationDocuments = application.ApplicationDocuments;

                        // 3.4 проверить уникальность UID у документы
                        // 3.4.1.	проверка уникальности UID (допускается UID = null или совпадение UID c уже имеющимся в базе документом такого же типа)
                        // 3.4.2.	проверка типа документа и UID: Конфликт если: 
                        // 3.4.2.1.	у этого же Entrant существует документ другого типа с таким же UID
                        // 3.4.2.2.	у др. Entrant сущ. Документ с таким же UID

                        CheckDocumentTypeCorrectness(applicationDocuments.CustomDocuments, application, EntrantDocumentType.CustomDocument, parsedDocs);
                        if (applicationDocuments.CustomDocuments != null)
                        {
                            foreach (var customDocument in applicationDocuments.CustomDocuments)
                            {
                                // нечего проверять
                            }
                        }

                        CheckDocumentTypeCorrectness(applicationDocuments.CompatriotDocuments, application, EntrantDocumentType.CompatriotDocument, parsedDocs);
                        if (applicationDocuments.CompatriotDocuments != null)
                        {
                            foreach(var document in applicationDocuments.CompatriotDocuments)
                            {
                                CheckCompatriotDocument(application, document);
                            }
                        }

                        var attachedDocs = new List<EntrantDocumentType>(); // для проверки 3.14.	Проверка на наличие необходимых документов
                        if (applicationDocuments.EduDocuments != null)
                        {

                            foreach (var doc in applicationDocuments.EduDocuments)
                            {
                                switch (doc.ItemElementName)
                                {
                                    case ItemChoiceType2.AcademicDiplomaDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.AcademicDiplomaDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.AcademicDiplomaDocument);
                                        break;
                                    case ItemChoiceType2.BasicDiplomaDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.BasicDiplomaDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.BasicDiplomaDocument);
                                        break;
                                    case ItemChoiceType2.EduCustomDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.EduCustomDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.EduCustomDocument);
                                        break;
                                    case ItemChoiceType2.HighEduDiplomaDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.HighEduDiplomaDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.HighEduDiplomaDocument);
                                        break;
                                    case ItemChoiceType2.IncomplHighEduDiplomaDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.IncomplHighEduDiplomaDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.IncomplHighEduDiplomaDocument);
                                        break;
                                    case ItemChoiceType2.MiddleEduDiplomaDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.MiddleEduDiplomaDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.MiddleEduDiplomaDocument);
                                        break;
                                    case ItemChoiceType2.PhDDiplomaDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.PhDDiplomaDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.PhDDiplomaDocument);
                                        break;
                                    case ItemChoiceType2.PostGraduateDiplomaDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.PostGraduateDiplomaDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.PostGraduateDiplomaDocument);
                                        break;
                                    case ItemChoiceType2.SchoolCertificateBasicDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.SchoolCertificateBasicDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.SchoolCertificateBasicDocument);
                                        break;
                                    case ItemChoiceType2.SchoolCertificateDocument:
                                        CheckDocumentTypeCorrectness(doc.Item as IBaseDocument, application, EntrantDocumentType.SchoolCertificateDocument, parsedDocs);
                                        attachedDocs.Add(EntrantDocumentType.SchoolCertificateDocument);
                                        break;
                                }
                                if (doc.ItemElementName == ItemChoiceType2.SchoolCertificateBasicDocument || doc.ItemElementName == ItemChoiceType2.SchoolCertificateDocument)
                                {
                                    var schoolCertificateDocument = doc.Item as TSchoolCertificateDocument;
                                    if (schoolCertificateDocument != null)
                                    {
                                        if (schoolCertificateDocument.Subjects != null)
                                        {
                                            foreach (var subject in schoolCertificateDocument.Subjects)
                                            {
                                                var subjectVoc = VocabularyStatic.SubjectVoc.Items.Where(t => t.SubjectID == subject.SubjectID).FirstOrDefault();
                                                if (subjectVoc == null) // || !subjectVoc.IsEge)
                                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EduDocument.Subjects.SubjectID");

                                                if (subject.Value < 1 || subject.Value > 999)
                                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.SubjectHasIncorrectValue, subject.Value.ToString());
                                            }

                                            var subjectIDs = schoolCertificateDocument.Subjects.Where(t => t.SubjectID != 0).Select(t => t.SubjectID);
                                            if (subjectIDs.Distinct().Count() != subjectIDs.Count())
                                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DublicateSubjects);
                                        }
                                    }
                                }

                                var eduDocument = doc.Item as IEduDocument;
                                if (eduDocument != null)
                                {
                                    // eduDocument.SpecialityID
                                    //if (eduDocument.SpecialityID != 0 && !vocabularyStorage.DirectionVoc.Items.Any(t => t.Code == eduDocument.SpecialityID.ToString()))
                                    //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EduDocument.SpecialityID");

                                    //FIS-1809
                                    //По идее документ об образовании из России не может быть IsNostrificated, превращаем в null
                                    if (eduDocument.IsNostrificated != null)
                                    {
                                        if (applicationDocuments.IdentityDocument != null)
                                        {
                                            var identityDocument = applicationDocuments.IdentityDocument;
                                            var idDocType = VocabularyStatic.IdentityDocumentTypeVoc.Items.FirstOrDefault(t => t.IdentityDocumentTypeID == identityDocument.IdentityDocumentTypeID);

                                            int nationalityRussain = 1; /* хардкод! */
                                            //если гражданство Россия, или документ российский
                                            if (identityDocument.NationalityTypeID == nationalityRussain || (idDocType != null && idDocType.IsRussianNationality))
                                                eduDocument.IsNostrificated = null;
                                        }
                                    }

                                    if (eduDocument.SpecialityID != 0 && !VocabularyStatic.DirectionVoc.Items.Any(t => t.DirectionID == eduDocument.SpecialityID))
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EduDocument.SpecialityID");

                                    // eduDocument.QualificationTypeID
                                    //if (eduDocument.QualificationTypeID != 0 && !vocabularyStorage.DirectionVoc.Items.Any(t => t.QUALIFICATIONCODE == eduDocument.QualificationTypeID.ToString()))
                                    //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EduDocument.QualificationTypeID");
                                }
                            }
                        }

                        // 3.14.	Проверка на наличие необходимых документов
                        // Переделано на связь по таблице [EduLevelDocumentType] 
                        // Проверка перенесена только на принятие заявления (FIS-1711, 05.06.2017)
                        if (application.StatusID == 4) { 
                            var loadedTypes = attachedDocs.Select(x => (int)x).ToArray();

                            var educationLevels = vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => application.FinSourceAndEduForms != null && application.FinSourceAndEduForms.Any(y => y.CompetitiveGroupID == x.CompetitiveGroupID))
                                        .Select(x => x.EducationLevelID).ToArray();
                            List<string> errors = new List<string>();
                            Func<int[], string> checkDocType = arr =>
                            {
                                if (!loadedTypes.Intersect(arr).Any())
                                {
                                    return string.Join(", ", arr.Select(VocabularyStatic.DocumentTypeVoc.GetDocumentTypeName));
                                }

                                return null;
                            };

                            foreach (int educationLevel in educationLevels)
                            {
                                errors.Add(checkDocType(VocabularyStatic.EduLevelDocumentTypeVoc.Items.Where(t => t.AdmissionItemTypeID == educationLevel).Select(t => t.DocumentTypeId).ToArray()));
                            }
                            errors = errors.Where(x => x != null).ToList();
                            if (errors.Count != 0)
                            {
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationDoesNotContainRequiredEduDocument,
                                    string.Join(". А также один из следующих: ", errors));
                            }
                        }


                        CheckDocumentTypeCorrectness(applicationDocuments.EgeDocuments, application, EntrantDocumentType.EgeDocument, parsedDocs);
                        if (applicationDocuments.EgeDocuments != null)
                            foreach (var egeDocument in applicationDocuments.EgeDocuments)
                            {
                                // проверяем на дубли egeDocument.Subjects
                                if (egeDocument.Subjects.Select(x => x.SubjectID).Distinct().Count() < egeDocument.Subjects.Length)
                                {
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.SubjectsInEgeDocumentIsDupllicated);
                                }

                                // egeDocument.Subjects
                                foreach (var subject in egeDocument.Subjects)
                                {
                                    var subjectVoc = VocabularyStatic.SubjectVoc.Items.Where(t => t.SubjectID == subject.SubjectID).FirstOrDefault();
                                    if (subjectVoc == null || !subjectVoc.IsEge)
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "EgeDocuments.Subjects.SubjectID");

                                    if (subject.Value < 1 || subject.Value > 999)
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.SubjectHasIncorrectValue, subject.Value.ToString());
                                }

                                if (egeDocument.DocumentYear == 0 && !egeDocument.DocumentDateSpecified)
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.EgeDocumentDateMissing);
                                else if (egeDocument.DocumentDateSpecified && egeDocument.DocumentDateSpecified && egeDocument.DocumentYear != egeDocument.DocumentDate.Year)
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.EgeDocumentYearDateInvalid);
                            }

                        CheckDocumentTypeCorrectness(applicationDocuments.GiaDocuments, application, EntrantDocumentType.GiaDocument, parsedDocs);
                        if (applicationDocuments.GiaDocuments != null)
                            foreach (var giaDocument in applicationDocuments.GiaDocuments)
                            {
                                // проверяем на дубли giaDocument.Subjects
                                if (giaDocument.Subjects.Select(x => x.SubjectID).Distinct().Count() < giaDocument.Subjects.Length)
                                {
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.SubjectsInGiaDocumentIsDupllicated);
                                }

                                // giaDocument.Subjects
                                foreach (var subject in giaDocument.Subjects)
                                {
                                    if (!VocabularyStatic.SubjectVoc.Items.Any(t => t.SubjectID == subject.SubjectID))
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "GiaDocuments.Subjects.SubjectID");
                                }
                            }

                        CheckDocumentTypeCorrectness(applicationDocuments.IdentityDocument, application, EntrantDocumentType.IdentityDocument, parsedDocs);
                        if (applicationDocuments.IdentityDocument != null)
                        {
                            var identityDocument = applicationDocuments.IdentityDocument;

                            CheckIdentityDocument(application, identityDocument, true);
                        }
                        if (applicationDocuments.OtherIdentityDocuments != null)
                        {
                            foreach (var document in applicationDocuments.OtherIdentityDocuments)
                                CheckIdentityDocument(application, document, false);
                        }

                        CheckDocumentTypeCorrectness(applicationDocuments.MilitaryCardDocument, application, EntrantDocumentType.MilitaryCardDocument, parsedDocs);
                        if (applicationDocuments.MilitaryCardDocument != null)
                        {
                            // нечего проверять
                        }

                        CheckDocumentTypeCorrectness(applicationDocuments.StudentDocument, application, EntrantDocumentType.StudentDocument, parsedDocs);
                        if (applicationDocuments.StudentDocument != null)
                        {
                            // нечего проверять
                        }
                    } // application.ApplicationDocuments
                    #endregion "ApplicationDocuments"

                    #region "IndividualAchievements"
                    if (!application.ShortUpdate && application.IndividualAchievements != null)
                    {
                        //FIS-1710 14.06.2017 - проверку убираем, теперь обрезаем сумму баллов
                        // Только для бакалавриата, специалитета (фильтровать по типу ПК)
                        //if (dbCampaign != null && dbCampaign.CampaignTypeID == CampaignTypesView.BachelorAndSpeciality)
                        //{
                        //    var sumMark = application.IndividualAchievements.Sum(t => t.IAMarkSpecified ? t.IAMark : 0);
                        //    if (sumMark > 10)
                        //        conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementSumMarksMoreThen10);
                        //}

                        List<string> uniqueIAUID = new List<string>();
                        foreach (var archievement in application.IndividualAchievements)
                        {
                            // IAUID - Проверка элемента на наличие и непустоту
                            if (string.IsNullOrWhiteSpace(archievement.IAUID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementUIDIsEmpty);

                            // IAUID - проверка на уникальность в базе и пакете
                            if (vocabularyStorage.IndividualAchivementVoc.Items.Any(t => t.IAUID == archievement.IAUID && t.ApplicationID != application.ID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementUIDIsNotUniqueInDb, archievement.IAUID);

                            if (uniqueIAUID.Contains(archievement.IAUID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementUIDIsNotUniqueInPackage, archievement.IAUID);
                            else
                                uniqueIAUID.Add(archievement.IAUID);

                            // Проверка задания имени индивидуального достижения
                            //if (string.IsNullOrEmpty(archievement.IAName))
                            //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementNameIsEmpty);

                            InstitutionAchievementsVocDto institutionArchievement = null;
                            // Не обязательный. Проверка, что указанный UID есть в БД в таблице InstitutionAchievements 
                            // и что запись таблицы c таким UID привязана к ПК, выбранной в заявлении, иначе конфликт.

                            // 2015-10-16 Теперь это поле обязательно. Изменения в xsd будут позже
                            if (string.IsNullOrWhiteSpace(archievement.InstitutionAchievementUID))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.InstitutionAchievementUIDNotSpecified);
                            else
                            {
                                institutionArchievement = vocabularyStorage.InstitutionAchievementsVoc.Items.Where(t => t.UID == archievement.InstitutionAchievementUID).FirstOrDefault();
                                if (institutionArchievement == null)
                                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementInstitutionAchievementUIDNotExists, archievement.IAUID);
                                else
                                {
                                    archievement.InstitutionAchievementID = institutionArchievement.IdAchievement;

                                    if (campaignID != 0 && institutionArchievement.CampaignID != 0 && campaignID != institutionArchievement.CampaignID)
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementInstitutionAchievementUIDWrongCampaign, archievement.IAUID);

                                    if (archievement.IAMarkSpecified && archievement.IAMark > institutionArchievement.MaxValue)
                                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementMarkBiggerMaxValue, archievement.IAUID, archievement.IAMark.ToString(), institutionArchievement.MaxValue.ToString());
                                }
                            }


                            // Проверка указания подтверждающего документа
                            if (!(!string.IsNullOrWhiteSpace(archievement.IADocumentUID) || (institutionArchievement != null && institutionArchievement.IdCategory == InstitutionAchievementsVocDto.FinalEssey)))
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementDocumentUIDIsEmpty);

                            // Проверка существования указанного подтверждающего документа
                            var dbDocument = vocabularyStorage.EntrantDocumentVoc.Items.Where(x => x.UID == archievement.IADocumentUID && x.DocumentTypeID == 15 && x.EntrantUID == entrant.UID).FirstOrDefault();
                            if (dbDocument != null)
                                archievement.EntrantDocumentID = dbDocument.EntrantDocumentID;

                            bool isDocInDB = dbDocument != null;
                            bool isDocInPackage = (application.ApplicationDocuments.CustomDocuments != null) && application.ApplicationDocuments.CustomDocuments.Any(x => x.UID == archievement.IADocumentUID);

                            // Если документ, на который ссылаемся есть в этом пакете - он будет перезатерт при обновлении, значит надо ориентироваться на его UID, а не на его ID!
                            if (isDocInPackage)
                                archievement.EntrantDocumentID = null;

                            if (!(isDocInDB || isDocInPackage ||
                                (institutionArchievement != null && institutionArchievement.IdCategory == InstitutionAchievementsVocDto.FinalEssey && string.IsNullOrWhiteSpace(archievement.IADocumentUID))))
                            {
                                conflictStorage.SetObjectIsBroken(application, ConflictMessages.IndividualAchivementDocumentNotFound);
                            }

                        }
                    }
                    #endregion "IndividualAchievements"
                }// foreach Application
            }
        }

        private void CheckBenefitOlympic(PackageDataApplication application, PackageDataApplicationEntranceTestResult etResult, IOlympicDocument doc)
        {
            if (doc.ClassNumber > 11)
                return;

            // 3.9.2.	Проверка  «Указан неверный тип документа-основания для диплома победителя/призера олимпиады»
            // если EntranceTestResult.ResultDocument - OlympicDocument или OlympicTotalDocument, то ResultSourceTypeID д.б. = 3 (OlympicDocument)
            if (etResult.ResultSourceTypeID != (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.OlympicDocument)
                conflictStorage.SetObjectIsBroken(application,
                       new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
                       {
                           Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                           Message = "Указан неверный тип документа-основания для диплома победителя/призера олимпиады"
                       });

            //var etic = vocabularyStorage.EntranceTestItemCVoc.Items.FirstOrDefault(t => t.UID == etResult.UID && t.CompetitiveGroupID == (etResult.CompetitiveGroupDict != null ? etResult.CompetitiveGroupDict.ID : 0));
            //if (etic == null)
            //{
            //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationETResultHasWrongUID, etResult.UID, etResult.CompetitiveGroupUID);
            //    continue;
            //}
            //var eticId = etic.EntranceTestItemID;

            var eticId = etResult.EntranceTestItemC != null ? etResult.EntranceTestItemC.EntranceTestItemID : 0;


            var res = ADOApplicationRepository.CheckBenefitOlympic(eticId,
                etResult.CompetitiveGroupDict != null ? etResult.CompetitiveGroupDict.ID : 0,
                doc.OlympicTypeProfileID,
                doc.DiplomaTypeID.To(0),
                doc.OlympicID.To(0),
                doc.ClassNumber.To(0),
                0
                );

            if (!string.IsNullOrWhiteSpace(res.Item1))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.BenefitOlympicHasError, etResult.UID, res.Item1);
            if (!string.IsNullOrWhiteSpace(res.Item2))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.BenefitOlympicHasError, etResult.UID, res.Item2);
        }

        private void CheckUkraineOlympicDocument(PackageDataApplication application, TUkraineOlympic document)
        {
            //document.DiplomaTypeID
            if (!VocabularyStatic.OlympicDiplomTypeVoc.Items.Any(t => t.OlympicDiplomTypeID == document.DiplomaTypeID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "UkraineOlympic.DiplomaTypeID");
        }

        private void CheckSportDocument(PackageDataApplication application, TSportDocument document)
        {
            // document.SportCategoryID 
            if (!VocabularyStatic.DocumentTypeVoc.SportDocuments.Any(t=> t.ID == document.SportCategoryID))
               conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "SportDocument.SportCategoryID");
        }

        private void CheckOrphanDocument(PackageDataApplication application, TOrphanDocument document)
        {
            // document.OrphanCategoryID
            if (!VocabularyStatic.OrphanCategory.Items.Any(t => t.ID == document.OrphanCategoryID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OrphanDocument.OrphanCategoryID");
        }

        private void CheckVeteranDocument(PackageDataApplication application, TVeteranDocument document)
        {
            // document.VeteranCategoryID
            if (!VocabularyStatic.VeteranCategory.Items.Any(t => t.ID == document.VeteranCategoryID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "VeteranDocument.VeteranCategoryID");
        }

        private void CheckParentsLostDocument(PackageDataApplication application, TParentsLostDocument document)
        {
            // document.VeteranCategoryID
            if (!VocabularyStatic.ParentsLostCategory.Items.Any(t => t.ID == document.ParentsLostCategoryID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "ParentsLostDocument.ParentsLostCategoryID");
        }

        private void CheckStateEmployeeDocument(PackageDataApplication application, TStateEmployeeDocument document)
        {
            // document.VeteranCategoryID
            if (!VocabularyStatic.StateEmployeeCategory.Items.Any(t => t.ID == document.StateEmployeeCategoryID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "StateEmployeeDocument.StateEmployeeCategoryID");
        }

        private void CheckRadiationWorkDocument(PackageDataApplication application, TRadiationWorkDocument document)
        {
            // document.VeteranCategoryID
            if (!VocabularyStatic.RadiationWorkCategory.Items.Any(t => t.ID == document.RadiationWorkCategoryID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "RadiationWorkDocument.RadiationWorkCategoryID");
        }
        //private void CheckPauperDocument(PackageDataApplication application, TPauperDocument document)
        //{
        //    // document.VeteranCategoryID
        //    if (!VocabularyStatic.VeteranCategory.Items.Any(t => t.ID == document.VeteranCategoryID))
        //        conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "VeteranDocument.VeteranCategoryID");
        //}

        private void CheckCompatriotDocument(PackageDataApplication application, TCompatriotDocument document)
        {
            // document.CompatriotCategoryID
            if (!VocabularyStatic.CompatriotCategory.Items.Any(t => t.ID == document.CompatriotCategoryID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "CompatriotDocument.CompatriotCategoryID");
        } 

        private void CheckInternationalOlympicDocument(PackageDataApplication application, TInternationalOlympic document)
        {
            // document.CountryID
            if (!VocabularyStatic.CountryTypeVoc.Items.Any(t => t.CountryID == document.CountryID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "InternationalOlympic.CountryID");
        }

        private void CheckIdentityDocument(PackageDataApplication application, IIdentityDocument identityDocument, bool isPrimaryDocument)
        {
            //IdentityDocument.NationalityTypeID - пока опустим, потому что в справочнике в БД нет значений, кроме РФ, а в файлах - есть!
            if (!VocabularyStatic.CountryTypeVoc.Items.Any(t => t.CountryID == identityDocument.NationalityTypeID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "IdentityDocument.NationalityTypeID");

            //IdentityDocument.IdentityDocumentTypeID
            var idDocType = VocabularyStatic.IdentityDocumentTypeVoc.Items.FirstOrDefault(t => t.IdentityDocumentTypeID == identityDocument.IdentityDocumentTypeID);
            if (idDocType == null)
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "IdentityDocument.IdentityDocumentTypeID");
            else
            {
                // Проверка только для основного удостоверяющего документа
                if (isPrimaryDocument && identityDocument.IdentityDocumentTypeID != 9)
                {
                    int nationalityRussain = 1; /* хардкод! */
                    // Если национальность = РФ, то и документ должен иметь IsRussian = true и наоборот!
                    if ((identityDocument.NationalityTypeID == nationalityRussain && !idDocType.IsRussianNationality)
                        || (identityDocument.NationalityTypeID != nationalityRussain && idDocType.IsRussianNationality))
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.IdentityDocumentContainsInvalidNationality);
                }
            }

            // 
            if (String.IsNullOrEmpty(identityDocument.DocumentSeries) && IdentityDocumentViewModel.IsSeriesRequired(identityDocument.IdentityDocumentTypeID.To(0)))
            {
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.IdentityDocumentRequireSeries);
            }

            // FirstName, LastName, MiddleName и GenderID - должны совпадать с Entrant, но только для основного IdentityDocument
            if (isPrimaryDocument)
            {
                if (string.IsNullOrWhiteSpace(identityDocument.LastName))
                    identityDocument.LastName = application.Entrant.LastName;
                if (string.IsNullOrWhiteSpace(identityDocument.FirstName))
                    identityDocument.FirstName = application.Entrant.FirstName;
                if (string.IsNullOrWhiteSpace(identityDocument.MiddleName))
                    identityDocument.MiddleName = application.Entrant.MiddleName;
                if (!identityDocument.GenderIDSpecified)
                    identityDocument.GenderID = application.Entrant.GenderID;

                if (identityDocument.LastName != application.Entrant.LastName
                    || identityDocument.FirstName != application.Entrant.FirstName
                    || identityDocument.MiddleName != application.Entrant.MiddleName
                    || identityDocument.GenderID != application.Entrant.GenderID
                    )
                {
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.IdentityDocumentHasOtherValues);
                }
            }
                

               
                    

            // У всех документов должна быть одинаковая BirthDate
            if (application.Entrant.BirthDate.HasValue)
            {
                if (application.Entrant.BirthDate.Value != identityDocument.BirthDate)
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.IdentityDocumentHasOtherBirthDate);
            }
            else
                application.Entrant.BirthDate = identityDocument.BirthDate;
        }

        #region "Вспомогательные методы
        // TODO: Удалить после 01.09.2016
        ///// <summary>
        ///// Получаем разрешённые формы обучения (по наличию мест) в выбранных конкурсных группах (направлениях)
        ///// </summary>
        //private Tuple<int, int>[] GetAvailableEducationFormsForCompetitiveGroup(string competitiveGroupUID)
        //{
        //    var competitiveGroupItemUIDs = vocabularyStorage.CompetitiveGroupItemVoc.Items
        //        .Where(x => x.CompetitiveGroupUID == competitiveGroupUID)
        //        .Select(x => x.UID)
        //        .ToArray();

        //    return GetAvailableEducationFormsForCompetitiveGroup(competitiveGroupItemUIDs);
        //}

        ///// <summary>
        ///// Получаем разрешённые формы обучения (по наличию мест) в выбранных конкурсных группах (направлениях)
        ///// </summary>
        //private Tuple<int, int>[] GetAvailableEducationFormsForCompetitiveGroup(string[] competitiveGroupUIDs)
        //{
        //    if (competitiveGroupUIDs == null) return new Tuple<int, int>[0];
        //    var groupItems = vocabularyStorage.CompetitiveGroupItemVoc.Items
        //                .Where(x => competitiveGroupUIDs.Contains(x.CompetitiveGroupUID))
        //                .Select(x => new
        //                {
        //                    x.NumberBudgetO,
        //                    x.NumberBudgetOZ,
        //                    x.NumberBudgetZ,
        //                    x.NumberPaidO,
        //                    x.NumberPaidOZ,
        //                    x.NumberPaidZ,
        //                    x.NumberQuotaO,
        //                    x.NumberQuotaOZ,
        //                    x.NumberQuotaZ
        //                }).ToArray();
        //    List<Tuple<int, int>> availableForms = new List<Tuple<int, int>>();
        //    if (groupItems.Sum(x => x.NumberBudgetO) > 0)
        //        availableForms.Add(new Tuple<int, int>(11, 14));
        //    if (groupItems.Sum(x => x.NumberBudgetOZ) > 0)
        //        availableForms.Add(new Tuple<int, int>(12, 14));
        //    if (groupItems.Sum(x => x.NumberBudgetZ) > 0)
        //        availableForms.Add(new Tuple<int, int>(10, 14));
        //    if (groupItems.Sum(x => x.NumberPaidO) > 0)
        //        availableForms.Add(new Tuple<int, int>(11, 15));
        //    if (groupItems.Sum(x => x.NumberPaidOZ) > 0)
        //        availableForms.Add(new Tuple<int, int>(12, 15));
        //    if (groupItems.Sum(x => x.NumberPaidZ) > 0)
        //        availableForms.Add(new Tuple<int, int>(10, 15));
        //    if (groupItems.Sum(x => x.NumberQuotaO) > 0)
        //        availableForms.Add(new Tuple<int, int>(11, 20));
        //    if (groupItems.Sum(x => x.NumberQuotaOZ) > 0)
        //        availableForms.Add(new Tuple<int, int>(12, 20));
        //    if (groupItems.Sum(x => x.NumberQuotaZ) > 0)
        //        availableForms.Add(new Tuple<int, int>(10, 20));

        //    // TODO:
        //    var target = (from x in vocabularyStorage.CompetitiveGroupTargetItemVoc.Items
        //                  where competitiveGroupUIDs.Contains(x.CompetitiveGroupUID)
        //                  select new { x.NumberTargetO, x.NumberTargetOZ, x.NumberTargetZ }).ToArray();

        //    if (target.Sum(x => x.NumberTargetO) > 0)
        //        availableForms.Add(new Tuple<int, int>(11, 16));
        //    if (target.Sum(x => x.NumberTargetOZ) > 0)
        //        availableForms.Add(new Tuple<int, int>(12, 16));
        //    if (target.Sum(x => x.NumberTargetZ) > 0)
        //        availableForms.Add(new Tuple<int, int>(10, 16));
        //    if (target.Sum(x => x.NumberTargetO + x.NumberTargetOZ + x.NumberTargetZ) > 0)
        //    {
        //        availableForms.Add(new Tuple<int, int>(11, 16));
        //        availableForms.Add(new Tuple<int, int>(12, 16));
        //        availableForms.Add(new Tuple<int, int>(10, 16));
        //    }
        //    return availableForms.ToArray();
        //}

        private void CheckOlympicDocument(PackageDataApplication application, TOlympicDocument olympicDocument)
        {
            // check olympicDocument.DiplomaTypeID
            if (!VocabularyStatic.OlympicDiplomTypeVoc.Items.Any(t => t.OlympicDiplomTypeID == olympicDocument.DiplomaTypeID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicDocument.DiplomaTypeID");

            // check olympicDocument.OlympicID
            if (!VocabularyStatic.OlympicTypeVoc.Items.Any(t => t.OlympicID == olympicDocument.OlympicID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicDocument.OlympicID");

            var otp = VocabularyStatic.OlympicTypeProfileVoc.Items.Where(t =>
                        t.OlympicTypeID == olympicDocument.OlympicID
                        && (t.OlympicProfileID == olympicDocument.ProfileID || olympicDocument.ProfileID == 255)
                    ).FirstOrDefault();

            if (otp == null)
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicDocument.ProfileID");
            else
                olympicDocument.OlympicTypeProfileID = otp.ID;

            if (olympicDocument.ClassNumber < 7 || olympicDocument.ClassNumber > 11)
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicDocument.ClassNumber");

            var olympicSubject = VocabularyStatic.SubjectVoc.Items.FirstOrDefault(t => t.SubjectID == olympicDocument.OlympicSubjectID);
            var egeSubject = VocabularyStatic.SubjectVoc.Items.FirstOrDefault(t => t.SubjectID == olympicDocument.EgeSubjectID && t.IsEge);

            // olympicDocument.OlympicSubjectID: если указан, то должен соответствовать справочнику предметов
            if (olympicDocument.OlympicSubjectIDSpecified && olympicSubject == null)
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicDocument.OlympicSubjectID");
            // olympicDocument.EgeSubjectID: если указан, то должен соответствовать справочнику предметов (c IsEGE)
            if (olympicDocument.EgeSubjectIDSpecified && egeSubject == null)
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicDocument.EgeSubjectID");

            // OlympicSubjectID - должен соответствовать профилю олимпиады
            if (olympicSubject != null &&
                !VocabularyStatic.OlympicSubjectVoc.Items.Any(t => t.OlympicTypeProfileID == olympicDocument.OlympicTypeProfileID && t.SubjectID == olympicSubject.SubjectID) )
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.OlympicDocumentOlympicSubjectMustMatchProfile);

            // если в OlympicSubject передан предмет, не являющийся общеобразовательным, то требовать заполнения EgeSubject
            if (olympicSubject != null && !olympicSubject.IsEge && egeSubject == null)
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.OlympicDocumentSpecifiedSubjectMustBeEge);

            if (!olympicDocument.EgeSubjectIDSpecified && olympicDocument.OlympicSubjectIDSpecified)
                olympicDocument.EgeSubjectID = olympicDocument.OlympicSubjectID;
        }

        private void CheckOlympicTotalDocument(PackageDataApplication application, TOlympicTotalDocument olympicTotalDocument)
        {
            // check olympicTotalDocument.DiplomaTypeID
            if (!VocabularyStatic.OlympicDiplomTypeVoc.Items.Any(t => t.OlympicDiplomTypeID == olympicTotalDocument.DiplomaTypeID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicTotalDocument.DiplomaTypeID");

            // check olympicTotalDocument.OlympicID
            if (!VocabularyStatic.OlympicTypeVoc.Items.Any(t => t.OlympicID == olympicTotalDocument.OlympicID))
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicTotalDocument.OlympicID");

            if (olympicTotalDocument.Subjects != null)
            {
                // Но на самом деле это не subjectID, а ProfileID!
                var subjectID = olympicTotalDocument.Subjects.SubjectID;

                //check subject.SubjectID
                //if (!VocabularyStatic.SubjectVoc.Items.Any(t => t.SubjectID == subjectID))
                //    conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicTotalDocument.SubjectBriefData.SubjectID");

                var otp = VocabularyStatic.OlympicTypeProfileVoc.Items.Where(t => t.OlympicTypeID == olympicTotalDocument.OlympicID && t.OlympicProfileID == subjectID).FirstOrDefault();
                if (otp == null)
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.DictionaryItemAbsent, "OlympicTotalDocument.Subjects.SubjectID");
                else {
                    olympicTotalDocument.OlympicTypeProfileID = otp.ID;
                }
            }
        }

        private CompetitiveGroupVocDto GetCompetitiveGroupDict(PackageDataApplication application, string competitiveGroupUID)
        {
            CompetitiveGroupVocDto competitiveGroupDict = null;
            var competitiveGroup = vocabularyStorage.CompetitiveGroupVoc.GetItemByUid(competitiveGroupUID);
            if (competitiveGroup == null)
            {
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationContainsInvalidCompetitiveGroupID, competitiveGroupUID);
                competitiveGroupDict = new CompetitiveGroupVocDto() { ID = 0, UID = competitiveGroupUID };
            }
            else
            {
                // 3.15. Корректный ли статус у кампании, в которую импортируется заявление
                if (competitiveGroup.StatusID != 1)
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.ApplicationCannotImportedForCampaignStatus);

                competitiveGroupDict = competitiveGroup;
            }
            return competitiveGroupDict;
        }



        /// <summary>
        /// Формирование текстового сообщения про неуникальность объекта
        /// </summary>
        private void SetNonUniqueUIDMessage(IUid uidObject, IBroken brokenObject)
        {
            string dtoDisplayType = uidObject.GetType().Name;

            if (uidObject is PackageDataApplicationEntranceTestResult) dtoDisplayType = "Результаты вступительных испытаний";
            else if (uidObject is PackageDataApplicationApplicationCommonBenefit) dtoDisplayType = "Льгота";
            else if (uidObject is IBaseDocument) dtoDisplayType = "Документ";

            else
            {
#if DEBUG
                throw new Exception("SetNonUniqueUIDMessage - необрабатываемый тип: " + dtoDisplayType);
#endif
            }
            //if (dtoObject is ApplicationDto) dtoDisplayType = "Заявление";
            //else if (dtoObject is AdmissionVolumeDto) dtoDisplayType = "Объем приема";
            //else if (dtoObject is ApplicationCommonBenefitDto) dtoDisplayType = "Общая льгота заявления";
            //else if (dtoObject is ApplicationDocumentDto) dtoDisplayType = "Документ";

            //else if (dtoObject is BenefitItemDto) dtoDisplayType = "Льгота";
            //else if (dtoObject is CampaignDateDto) dtoDisplayType = "Дата приемной кампании";
            //else if (dtoObject is CampaignDto) dtoDisplayType = "Приемная кампания";
            //else if (dtoObject is CompetitiveGroupDto) dtoDisplayType = "Конкурс";
            //else if (dtoObject is CompetitiveGroupItemDto) dtoDisplayType = "Направление конкурсной группы";
            //else if (dtoObject is CompetitiveGroupTargetDto) dtoDisplayType = "Организация целевого приема";
            //else if (dtoObject is CompetitiveGroupTargetItemDto) dtoDisplayType = "Места для целевого приема";
            //else if (dtoObject is EntranceTestAppItemDto) dtoDisplayType = "Результаты вступительных испытаний";
            //else if (dtoObject is EntranceTestItemDto) dtoDisplayType = "Вступительные испытания конкурсной группы";
            //else if (dtoObject is EntrantDto) dtoDisplayType = "Абитуриент";
            //else if (dtoObject is OrderOfAdmissionItemDto) dtoDisplayType = "Приказ";

            conflictStorage.SetObjectIsBroken(brokenObject, uidObject, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, dtoDisplayType, uidObject.UID);
        }


        private void CheckDocumentTypeCorrectness(IBaseDocument[] documentsArray, PackageDataApplication application, EntrantDocumentType docType, List<Tuple<IBaseDocument, EntrantDocumentType, string>> parsedDocs)
        {
            if (documentsArray == null) return;

            List<IBaseDocument> documents = documentsArray.ToList();

            /* Ругаемся на дубликаты UID в коллекциях документов */
            var dupUIDs = documents.GroupBy(c => c.UID).Where(c => c.Count() > 1).Select(c => c.Key).ToList();
            dupUIDs.ForEach(uid =>
                documents.Where(c => c.UID == uid).ToList().ForEach(c => SetNonUniqueUIDMessage(c, application)));
            // <==

            foreach (var document in documents)
                CheckDocumentTypeCorrectness(document, application, docType, parsedDocs);
        }

        /// <summary>
        /// Проверка на корректность типа документа по UID
        /// </summary>
        private void CheckDocumentTypeCorrectness(IBaseDocument documentDto, PackageDataApplication application, EntrantDocumentType docType, List<Tuple<IBaseDocument, EntrantDocumentType, string>> parsedDocs)
        {
            // в базе есть документ с тем же уидом но другим типом - явная ошибка
            // в остальных случаях, считаем что один и тот же документ, т.к. не проверяем содержимое
            if (documentDto == null) return;

            string entrantUID = application.Entrant.UID;

            //у документов могут быть отсутствующие UID'ы
            if (documentDto.UID == null)
            {

                //var res = ADOApplicationRepository.GetEntrantDocumentByData(application.EntrantID, (int)docType, documentDto.DocumentSeries, documentDto.DocumentNumber,
                //    documentDto.DocumentDate, documentDto.DocumentOrganization, null);
                //if (res.Item1 != 0)
                //{
                //    documentDto.ID = res.Item1;
                //    if (res.Item2 != Guid.Empty)
                //        documentDto.GUID = res.Item2;
                //}

                // Отказались от поиска документа по данным. Если UID задан - по ищем документ соотв. типа у соотв абитуриента, если UID не задан - всегда новый документ!
                return;
            }

            if (documentDto is IEduDocument)
            {
                var doc = documentDto as IEduDocument;

                // ХАРДКОД: длина поля RegistrationNumber
                if (doc != null && doc.RegistrationNumber != null && doc.RegistrationNumber.Length >= ENTRANTDOCUMENTEDU_REGISTRATIONNUMBER)
                {
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.FieldLengthExceeded, "EduDocument.*.RegistrationNumber", ENTRANTDOCUMENTEDU_REGISTRATIONNUMBER.ToString());
                }
            }

            var uidDocs = vocabularyStorage.EntrantDocumentVoc.GetItemsByUid(documentDto.UID);

            var existingDoc = uidDocs.Where(t => t.EntrantUID == entrantUID);
            var existingDocOtherEntrant = uidDocs.Where(t => t.EntrantUID != entrantUID);

            // в базе есть документ с тем же уидом но другим типом - ошибка!
            if (existingDoc.Any(t => t.DocumentTypeID != (int)docType))
            {
                conflictStorage.SetObjectIsBroken(application, ConflictMessages.InvalidDocumentTypeRelatedToExisting, documentDto.UID);
            }
            // документ принадлежит другому энтранту - ошибка!
            if (existingDocOtherEntrant.Any())
            {
                SetNonUniqueUIDMessage(documentDto, application);
            }

            if (existingDoc.Any(t => t.DocumentTypeID == (int)docType))
            {
                // Значит этот документ уже есть в БД
                documentDto.ID = existingDoc.First().EntrantDocumentID;
            }

            // Есть ли уже этот же документ в пакете?
            var existingParsed = parsedDocs.Where(x => !string.IsNullOrEmpty(documentDto.UID) && x.Item1.UID == documentDto.UID).FirstOrDefault();
            if (existingParsed != null)
            {
                if (existingParsed.Item2 != docType)
                {
                    // В пакете есть документ с таким уид, но другим типом - ошибка!
                    conflictStorage.SetObjectIsBroken(application, ConflictMessages.InvalidDocumentTypeRelatedToExisting, documentDto.UID);
                }
                else if (existingParsed.Item3 != application.Entrant.UID)
                {
                    // В пакете есть документ с таким же уид, но у другого Entrant'а - ошибка!
                    SetNonUniqueUIDMessage(documentDto, application);
                }
                else
                {
                    // В пакете 1 документ (уид, тип, Entrant) - встречается несколько раз
                    documentDto.GUID = existingParsed.Item1.GUID;

                    // FIS-1558: У одного заявления 1 документ льготы DocumentTypeID in (9, 10) не может быть использован более одного раза!
                    if (documentDto.EntrantDocumentType == EntrantDocumentType.OlympicDocument || documentDto.EntrantDocumentType == EntrantDocumentType.OlympicTotalDocument)
                        conflictStorage.SetObjectIsBroken(application, ConflictMessages.EntrantDocumentOlympicCannotBeUserTwiceInApplicaiton, documentDto.UID);


                    // Вот тут важный момент: нельзя допустить, чтобы 2 документа с одинаковым GUID имели разные данные. 
                    // Тогда хранимка ImportApplicaitons ругнется на Merge EntrantDocument!

                    // Пока не будем генерить ошибку, просто перезапишем одинаковыми данными
                    //documentDto.OriginalReceived = existingParsed.Item1.OriginalReceived;
                    //documentDto.OriginalReceivedDate = existingParsed.Item1.OriginalReceivedDate;
                    //documentDto.DocumentDate = existingParsed.Item1.DocumentDate;
                    //documentDto.DocumentNumber = existingParsed.Item1.DocumentNumber;
                    //documentDto.DocumentOrganization = existingParsed.Item1.DocumentOrganization;
                    //documentDto.DocumentSeries = existingParsed.Item1.DocumentSeries;
                }
            }
            else // (existingParsed == null) - не было в пакете документа с таким uid
            {
                parsedDocs.Add(new Tuple<IBaseDocument, EntrantDocumentType, string>(documentDto, docType, application.Entrant.UID));
            }
        }



        /// <summary>
        /// Проверяем совпадения IdentityDocument в случае нескольких заявлений от 1 абитуриента 
        /// </summary>
        public void CheckIdentityDocumentsDuplicates()
        {
            /* Проверяем что, если у нас импортируются несколько заявлений от 1 абитуриента 
             * с IdentityDocument, то если эти документы одинкаковые - грузим только 1 документ, если разные
             * не грузим оба заявления с ошибкой */
            if (packageData.GetApplications == null)
                return;

            var duplicatedEntrantUIDs = from c in packageData.GetApplications
                                        group c by new { c.Entrant.UID } //, c.Entrant.LastName, c.Entrant.FirstName, c.Entrant.MiddleName }
                                            into g
                                            where g.Count() > 1
                                            select g.Key;

            foreach (var key in duplicatedEntrantUIDs)
            {
                IEnumerable<PackageDataApplicationApplicationDocumentsIdentityDocument> identityDocuments = packageData.GetApplications.Where(
                    c => c.Entrant.UID == key.UID //&& c.Entrant.LastName == key.LastName && c.Entrant.FirstName == key.FirstName && c.Entrant.MiddleName == key.MiddleName
                         && c.ApplicationDocuments != null &&
                         c.ApplicationDocuments.IdentityDocument != null)
                                                                                    .Select(
                                                                                        c =>
                                                                                        c.ApplicationDocuments
                                                                                         .IdentityDocument);

                /* Если это пройдет в импорт, то одинаковые документы должны иметь одинаковые Id,
                 * чтобы их не размножить в EntrantDocument таблице для каждого из заявлений */
                Guid documentId = Guid.NewGuid();
                var id1 = identityDocuments.FirstOrDefault();
                foreach (PackageDataApplicationApplicationDocumentsIdentityDocument identityDocumentDto in identityDocuments)
                {
                    identityDocumentDto.GUID = documentId;
                    identityDocumentDto.UID = id1.UID;
                    identityDocumentDto.DocumentSeries = id1.DocumentSeries;
                    identityDocumentDto.DocumentNumber = id1.DocumentNumber;
                    identityDocumentDto.DocumentDate = id1.DocumentDate;
                    identityDocumentDto.DocumentOrganization = id1.DocumentOrganization;
                }

                if (identityDocuments.Distinct().Count() > 1)
                {
                    foreach (PackageDataApplication applicationDto in
                            packageData.GetApplications.Where(c => c.Entrant.UID == key.UID //&& c.Entrant.LastName == key.LastName && c.Entrant.FirstName == key.FirstName && c.Entrant.MiddleName == key.MiddleName
                            ))
                    {
                        conflictStorage.SetObjectIsBroken(applicationDto, ConflictMessages.TooManyIdentityDocumentsForEntrant);
                    }

                }
            }
        }

        #endregion
        protected override List<string> ImportDb()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            ai_logger.DebugFormat("Импорт пакета заявлений №{0} (всего: {1})...", packageData.ImportPackageId, packageData.GetApplications.Length);
            List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();
            int app_to_imp = packageData.ApplicationsToImport().Count;
            #region Applications
            bulks.Add(new Tuple<string, IDataReader>("bulk_Application", new BulkApplicationReader(packageData)));
            bulks.Add(new Tuple<string, IDataReader>("bulk_Entrant", new BulkEntrantReader(packageData)));
            // Competitives
            //bulks.Add(new Tuple<string,IDataReader>("bulk_ApplicationSelectedCompetitiveGroup", new BulkApplicationSelectedCompetitiveGroupReader(packageData, vocabularyStorage)));
            //bulks.Add(new Tuple<string,IDataReader>("bulk_ApplicationSelectedCompetitiveGroupItem", new BulkApplicationSelectedCompetitiveGroupItemReader(packageData, vocabularyStorage)));
            //bulks.Add(new Tuple<string,IDataReader>("bulk_ApplicationSelectedCompetitiveGroupTarget", new BulkApplicationSelectedCompetitiveGroupTargetReader(packageData, vocabularyStorage)));
            bulks.Add(new Tuple<string, IDataReader>("bulk_ApplicationCompetitiveGroupItem", new BulkApplicationCompetitiveGroupItemReader(packageData, vocabularyStorage)));
            // Documents
            var bulkEntrantDocumentReader = new BulkEntrantDocumentReader(packageData, vocabularyStorage);
            bulks.Add(new Tuple<string, IDataReader>("bulk_EntrantDocument", bulkEntrantDocumentReader));
            bulks.Add(new Tuple<string, IDataReader>("bulk_EntrantDocumentSubject", bulkEntrantDocumentReader.BulkEntrantDocumentSubjectReader));

            foreach(var app in packageData.Applications)
            {
                //if(app.ApplicationDocuments.)
            }

            bulks.Add(new Tuple<string, IDataReader>("bulk_ApplicationEntranceTestDocument", new BulkApplicationEntranceTestDocumentReader(packageData, vocabularyStorage)));

            bulks.Add(new Tuple<string, IDataReader>("bulk_ApplicationIndividualAchievements", new BulkApplicationIndividualAchievementsReader(packageData, vocabularyStorage)));

            bulks.Add(new Tuple<string, IDataReader>("bulk_ApplicationShortUpdate", new BulkApplicationShortUpdateReader(packageData, vocabularyStorage)));

            bulks.Add(new Tuple<string, IDataReader>("bulk_EntrantCitizenship", new BulkEntrantCitizenshipReader(packageData)));
            #endregion Applications

            // Должен прийти список импортированных заявлений - пополнить справочники и записать количество в successfulImportStatisticsDto 
            var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportApplications", deleteBulk, ai_logger);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                conflictStorage.successfulImportStatisticsDto.Applications = dsResult.Tables[0].Rows.Count.ToString();

                foreach (DataRow row in dsResult.Tables[0].Rows)
                    conflictStorage.ImportedApplicationIDs.Add((int)row["ApplicationID"]);

                //vocabularyStorage.CampaignVoc.AddItems(dsResult.Tables[0]);
                //if (dsResult.Tables.Count > 1)
                //    vocabularyStorage.CampaignDateVoc.AddItems(dsResult.Tables[1]);
                //if (dsResult.Tables.Count > 2)
                //    vocabularyStorage.CampaignEducationLevelVoc.AddItems(dsResult.Tables[2]);
                //ai_logger.ErrorFormat("Пакет заявлений №{0} испортирован с ошибками (всего: {1}) за {2} с.", packageData.ImportPackageId, res.Item2.Count, sw.Elapsed.TotalSeconds);
            }
            //else
            ai_logger.DebugFormat("Пакет заявлений №{0} успешно импортирован (всего: {1}) за {2} с.", packageData.ImportPackageId, app_to_imp, sw.Elapsed.TotalSeconds);
            Console.WriteLine("Пакет заявлений №{0} успешно импортирован (всего: {1}) за {2} с.", packageData.ImportPackageId, app_to_imp, sw.Elapsed.TotalSeconds);
            return res.Item2;
        }
        

    }
}
