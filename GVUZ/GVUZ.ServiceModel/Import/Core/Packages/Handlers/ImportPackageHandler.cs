using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Packages.Handlers
{
    /// <summary>
    /// Обработчик пакетов импорта
    /// </summary>
    public class ImportPackageHandler : PackageHandler
    {
        private int InstitutionId { get { return _repositoryPackageData.InstitutionID; } }
        private readonly ImportPackage _repositoryPackageData;
        private readonly PackageData _importPackage;
        private readonly DbObjectRepositoryBase _dbObjectRepository;
        private readonly StorageManager _storageManager;
        private List<Tuple<int, int, int>> _importedApplicationIDList = new List<Tuple<int, int, int>>();

        public ImportPackageHandler(ImportPackage repositoryPackageData)
        {
            _repositoryPackageData = repositoryPackageData;
            if (InstitutionId <= 0) return;

            _importPackage = new Serializer().Deserialize<PackageData>(repositoryPackageData.PackageData);
            if (_importPackage == null)
                throw new ImportException("Ошибка в структуре пакета");

            if (_importPackage.Applications != null)
                _importPackage.Applications.All(x => { x.CreateApplicationCompetitiveGroupItems(); return true; });

            int applicationsCount = 0;
            if (_importPackage.Applications != null)
            {
                applicationsCount = _importPackage.Applications.Length;
            }

            if ((applicationsCount > 0) && (applicationsCount < DbObjectRepositoryBase.DecisionApplicationsCount))
            {
                _dbObjectRepository = DbObjectRepositoryBase.Create(InstitutionId);
            }
            else
            {
                _dbObjectRepository = DbObjectRepositoryBase.CreateWithCache(InstitutionId);
            }

            var conflictStorage = new ConflictStorage(_dbObjectRepository, _importPackage);
            _storageManager = new StorageManager(_dbObjectRepository, conflictStorage, repositoryPackageData.UserLogin);
        }

        public override string Process()
        {
            return "";
        //    var error = ValidatePackage(_repositoryPackageData.PackageData, (PackageType)_repositoryPackageData.TypeID);
        //    if (error != null) return error;

        //    var successfulImportStatisticsDto = new SuccessfulImportStatisticsDto();

        //    InstitutionCampaignImport(successfulImportStatisticsDto);
        //    InstitutionStructureImport(successfulImportStatisticsDto);
        //    ApplicationsImport(successfulImportStatisticsDto, _repositoryPackageData.PackageID);
        //    OrdersImport(successfulImportStatisticsDto);

        //    RecommendedListsImport(successfulImportStatisticsDto, _repositoryPackageData.PackageID);

        //    InstitutionAchievementsImport(successfulImportStatisticsDto, _repositoryPackageData.PackageID, _repositoryPackageData.InstitutionID);



        //    // Conflict Storage должен быть накопителем и передаваться на все целостные этапы импорта (структура приема, заявления, приказы)
        //    // Это используется для целостного вывода информации о результатах импорта.
        //    var resultPackage = new ImportResultPackage
        //                    {
        //                        Log = new LogDto
        //                        {
        //                            Successful = successfulImportStatisticsDto,
        //                            Failed = _storageManager.ConflictStorage.GetFailedImportInfoDto(_storageManager.ProcessedDtoStorage)
        //                        },
        //                        Conflicts = _storageManager.ConflictStorage.GetConflictsResultDto(),
        //                        PackageID = _repositoryPackageData.PackageID + ""
        //                    };

        //    return PackageHelper.GenerateXmlPackageIntoString(resultPackage);

        }

        /// <summary>
        /// добавляем в пакет успешно импортированные заявления
        /// </summary>
        /// <param name="importPackage"></param>
        public override void AddExtraInfoToPackage(ImportPackage importPackage)
        {
            importPackage.ImportedAppIDs = String.Join(";", _importedApplicationIDList.Select(x => x.Item1 + (x.Item2 > 0 ? "_" + x.Item2 + "_" + x.Item3 : "")));
        }

        ///// <summary>
        ///// Импорт кампаний
        ///// </summary>
        //private void InstitutionCampaignImport(SuccessfulImportStatisticsDto successfulImportStatisticsDto)
        //{
        //    if (_importPackage.CampaignInfo != null)
        //    {
        //        _dbObjectRepository.LoadData();
        //        // анализ
        //        new CampaignInfoImporter(_storageManager, _importPackage.CampaignInfo).AnalyzeImportPackage();
        //        // удаление
        //        new DbDataDeleteManager(_storageManager).DeleteCampaigns();
        //        // вставка
        //        new DbDataInsertManager(_storageManager, successfulImportStatisticsDto).InsertCampaigns();
        //        // обновление
        //        new DbDataUpdateManager(_storageManager, successfulImportStatisticsDto).UpdateCampaigns();
        //    }
        //}

        ///// <summary>
        ///// Импорт объёма и КГ
        ///// </summary>
        //private void InstitutionStructureImport(SuccessfulImportStatisticsDto successfulImportStatisticsDto)
        //{
        //    if (_importPackage.AdmissionInfo != null)
        //    {
        //        _dbObjectRepository.LoadData();
        //        // анализ
        //        new AdmissionInfoImporter(_storageManager, _importPackage.AdmissionInfo).AnalyzeImportPackage();
        //        // удаление
        //        new DbDataDeleteManager(_storageManager).DeleteInstitutionStructure();
        //        // вставка
        //        new DbDataInsertManager(_storageManager, successfulImportStatisticsDto).InsertInstitutionStructure();
        //        // обновление
        //        new DbDataUpdateManager(_storageManager, successfulImportStatisticsDto).UpdateInstitutionStructure();

        //        // вставить записи DistributedAdmissionVolume
        //        new DbDataInsertManager(_storageManager, successfulImportStatisticsDto).InsertDistributedAdmissionVolumes();
        //    }
        //}

//        /// <summary>
//        /// Импорт заявлений
//        /// </summary>
//        private void ApplicationsImport(SuccessfulImportStatisticsDto successfulImportStatisticsDto, int importPackageId)
//        {
//            if (_importPackage.Applications == null) return;

//            var dbDataInsertManager = new DbDataInsertManager(_storageManager, successfulImportStatisticsDto);

//            _dbObjectRepository.LoadData();

//            // анализ
//            new ApplicationCollectionImporter(_storageManager, _importPackage.Applications).AnalyzeImportPackage();

//#warning Убрать отсюда!! Вынести в ApplicationImporter!!!
//            // подстановка OriginalDocumentsReceivedDate для существующих заявлений
//            SetOriginalDocumentsReceivedDateDocumentsByApplicationsFromDb();

//            // грузим заявления
//            dbDataInsertManager.InsertApplications(importPackageId);

//            //сохраняем список импортированных заявлений
//            _importedApplicationIDList = dbDataInsertManager.ImportedApplicationIDList;
//        }

        ///// <summary>
        ///// Заплатка, ловим документы без даты сдачи, но с фактом сдачи и сами ставим даты.
        ///// </summary>
        //private void SetOriginalDocumentsReceivedDateDocumentsByApplicationsFromDb()
        //{
        //    var importedApplicationsUids = new HashSet<string>();
        //    var existentsApplicationsInDb = new HashSet<ApplicationShortRef>();

        //    /* Все загружаемые UID-ы заявлений */
        //    importedApplicationsUids.UnionWith(_importPackage.Applications.Where(c => c.UID != null &&
        //        c.ApplicationDocuments != null && c.ApplicationDocuments.EduDocuments != null).Select(x => x.UID.ToUpper()));

        //    /* Смотрим какие есть заявления уже в БД по UID */
        //    var oldApplicationInImport = _dbObjectRepository.Applications.Where(c => c.UID != null && importedApplicationsUids.Contains(c.UID.ToUpper()));
        //    existentsApplicationsInDb.UnionWith(oldApplicationInImport);

        //    foreach (var importedApp in _importPackage.Applications.Where(c =>
        //        c.ApplicationDocuments != null && c.ApplicationDocuments.EduDocuments != null))
        //    {
        //        /* Берем импортируемое заявление с документами */
        //        var importedAppsWithUId = existentsApplicationsInDb.SingleOrDefault(c => c.UID.Equals(importedApp.UID,
        //            StringComparison.InvariantCultureIgnoreCase));
        //        var oldAppReceivedDate = importedAppsWithUId != null ? importedAppsWithUId.OriginalDocumentsReceivedDate : null;

        //        /* Бежим по всем документам */
        //        foreach (var eduDocument in importedApp.ApplicationDocuments.EduDocuments)
        //        {
        //            var newDate = ApplicationImporter.CheckDate(eduDocument.HighEduDiplomaDocument, oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.HighEduDiplomaDocument.OriginalReceivedDate = newDate;
        //            }
        //            newDate = ApplicationImporter.CheckDate(eduDocument.IncomplHighEduDiplomaDocument,
        //                                                    oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.IncomplHighEduDiplomaDocument.OriginalReceivedDate = newDate;
        //            }
        //            newDate = ApplicationImporter.CheckDate(eduDocument.BasicDiplomaDocument, oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.BasicDiplomaDocument.OriginalReceivedDate = newDate;
        //            }

        //            newDate = ApplicationImporter.CheckDate(eduDocument.AcademicDiplomaDocument, oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.AcademicDiplomaDocument.OriginalReceivedDate = newDate;
        //            }

        //            newDate = ApplicationImporter.CheckDate(eduDocument.EduCustomDocument, oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.EduCustomDocument.OriginalReceivedDate = newDate;
        //            }
        //            newDate = ApplicationImporter.CheckDate(eduDocument.MiddleEduDiplomaDocument, oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.MiddleEduDiplomaDocument.OriginalReceivedDate = newDate;
        //            }
        //            newDate = ApplicationImporter.CheckDate(eduDocument.SchoolCertificateBasicDocument,
        //                                                    oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.SchoolCertificateBasicDocument.OriginalReceivedDate = newDate;
        //            }
        //            newDate = ApplicationImporter.CheckDate(eduDocument.SchoolCertificateDocument, oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.SchoolCertificateDocument.OriginalReceivedDate = newDate;
        //            }

        //            newDate = ApplicationImporter.CheckDate(eduDocument.PostGraduateDiplomaDocument, oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.PostGraduateDiplomaDocument.OriginalReceivedDate = newDate;
        //            }

        //            newDate = ApplicationImporter.CheckDate(eduDocument.PhDDiplomaDocument, oldAppReceivedDate);
        //            if (newDate != null)
        //            {
        //                if (newDate == "") newDate = null;
        //                eduDocument.PhDDiplomaDocument.OriginalReceivedDate = newDate;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Импорт приказов
        ///// </summary>
        //private void OrdersImport(SuccessfulImportStatisticsDto successfulImportStatisticsDto)
        //{
        //    if (_importPackage.OrdersOfAdmission != null)
        //    {
        //        _dbObjectRepository.LoadData();

        //        // вставка приказов производится здесь же
        //        var importer = new OrderCollectionImporter(_storageManager, _importPackage.OrdersOfAdmission);
        //        importer.AnalyzeImportPackage();
        //        successfulImportStatisticsDto.ordersOfAdmissionsImported = importer.SuccessfullyImportedOrders;
        //        // удаление
        //        new DbDataDeleteManager(_storageManager).DeleteOrders();
        //    }
        //}

        //private void RecommendedListsImport(SuccessfulImportStatisticsDto successfulImportStatisticsDto, int importPackageId)
        //{
        //    if (_importPackage.RecommendedLists != null)
        //    {
        //        var importer = new RecommendedListCollectionImporter(_storageManager, _importPackage.RecommendedLists);
        //        importer.AnalyzeImportPackage();

        //        new DbDataInsertManager(_storageManager, successfulImportStatisticsDto).InsertRecommendedLists(importPackageId);
        //    }
        //}

        //private void InstitutionAchievementsImport(SuccessfulImportStatisticsDto successfulImportStatisticsDto, int importPackageId, int institutionId)
        //{
        //    if (_importPackage.InstitutionAchievements != null)
        //    {
        //        var importer = new InstitutionAchievementCollectionImporter(_storageManager, _importPackage.InstitutionAchievements);
        //        importer.AnalyzeImportPackage();

        //        new DbDataInsertManager(_storageManager, successfulImportStatisticsDto).InsertInstitutionAchievements(importPackageId, institutionId);
        //    }
        //}

        /// <summary>
        /// Валидация
        /// </summary>
        public override string ValidatePackage(string packageData, PackageType type)
        {
            if (type == PackageType.Import)
                return ValidatePackage(packageData, XsdManager.XsdName.DoImportServiceRequest);
            if (type == PackageType.ImportApplicationSingle)
                return ValidatePackage(packageData, XsdManager.XsdName.DoImportApplicationSingleServiceRequest);

            throw new ApplicationException("Не найден XSD для валидации этого типа пакета");
        }

        public override void Dispose()
        {
            if (_dbObjectRepository != null)
            {
                _dbObjectRepository.Dispose();

                GC.SuppressFinalize(this);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
    }
}

