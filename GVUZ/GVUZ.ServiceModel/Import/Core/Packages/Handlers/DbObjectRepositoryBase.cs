using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model;
using System.Data.Entity;
using GVUZ.ServiceModel.Import.WebService.Dto;
using System.Configuration;

namespace GVUZ.ServiceModel.Import.Core.Packages.Handlers
{
    /// <summary>
    /// Хранилище объектов, находящихся в данный момет в базе по институту
    /// </summary>
    public abstract class DbObjectRepositoryBase : IDisposable
    {
        public static int DecisionApplicationsCount
        {
            get
            {
                int result;
                if (Int32.TryParse(ConfigurationManager.AppSettings["EnableFullDataLoadAppsCount"], out result))
                    return result;
                return 10;
            }
        }

        public static DbObjectRepositoryBase CreateWithCache(int institutionID)
        {
            return new DbObjectRepositoryWithCache(institutionID);
        }

        public static DbObjectRepositoryBase Create(int institutionID)
        {
            return new DbObjectRepository(institutionID);
        }

        //Коллекции
        public abstract IQueryable<AdmissionVolume> AdmissionVolumes { get; }

        public abstract CompetitiveGroup GetCompetitiveGroupDictById(int id);

        public abstract IQueryable<CompetitiveGroup> CompetitiveGroups { get; }

        public abstract IQueryable<CompetitiveGroupItem> CompetitiveGroupItems { get; }

        public abstract IQueryable<CompetitiveGroupTarget> CompetitiveGroupTargets { get; }

        public abstract IQueryable<CompetitiveGroupTargetItem> CompetitiveGroupTargetItems { get; }

        public abstract IEnumerable<Campaign> Campaigns { get; }

        public abstract IQueryable<BenefitItemC> CompetitiveGroupBenefitItemsForEntranceTest { get; }

        public abstract IQueryable<BenefitItemC> CompetitiveGroupCommonBenefitItems { get; }

        public abstract IEnumerable<EntranceTestItemC> CompetitiveGroupEntranceTestItems { get; }

        public abstract IQueryable<ApplicationShortRef> Applications { get; }

#warning Не используется?
        public abstract IQueryable<ApplicationEntranceTestDocumentShortRef> ApplicationEntranceTestResults { get; }

        public abstract IQueryable<ApplicationEntranceTestDocument> ApplicationEntranceTestBenefits { get; }

        public abstract IQueryable<EntrantDocument> EntrantDocuments { get; }

#warning Не используется?
        public abstract IQueryable<OrderOfAdmission> OrdersOfAdmission { get; }

        public abstract IQueryable<AllowedDirections> AllowedDirections { get; }

        public abstract IQueryable<EntranceTestProfileDirection> EntranceTestProfileDirections { get; }

        public abstract IEnumerable<DirectionSubjectLinkDirection> DirectionSubjectLinkDirections { get; }

        public abstract IEnumerable<DirectionSubjectLink> DirectionSubjectLinks { get; }

        public abstract IEnumerable<DirectionSubjectLinkSubject> DirectionSubjectLinkSubjects { get; }

        public abstract IEnumerable<EntranceTestCreativeDirection> EntranceTestCreativeDirections { get; }

        public abstract IEnumerable<OlympicTypeSubjectLink> OlympicTypeSubjectLinks { get; }

        public ImportEntities ImportEntities { get; private set; }
        public int InstitutionId { get; private set; }

        protected DbObjectRepositoryBase(int institutionID)
        {
            ImportEntities = new ImportEntities();
            InstitutionId = institutionID;
        }

        /// <summary>
        /// Получение объектов по UID, исключая общую льготу. Общая льгота имеет тот же тип, что и льгота для ВИ.
        /// Для получения общей льготы использовать метод GetCommonBenefitItemCObject.
        /// </summary>
        public abstract T GetObject<T>(string uid) where T : class, IObjectWithUID;

        public abstract string GetParentUID<T>(string uid) where T : class,  IObjectWithUID;

        /// <summary>
        /// Получение общей льготы по UID.
        /// </summary>
        public BenefitItemC GetCommonBenefitItemCObject(string uid)
        {
            return CompetitiveGroupCommonBenefitItems.FirstOrDefault(x => x.UID == uid);
        }

        #region Извлечение из БД
        public IQueryable<Application> FindApplicationsBySelectedCompetitiveGroup(int competitiveGroupId)
        {
            return ImportEntities.ApplicationSelectedCompetitiveGroup.Where(c =>
                c.CompetitiveGroupID == competitiveGroupId).Select(c => c.Application);
        }

        public IQueryable<Application> FindApplicationsByOrderCompetitiveGroup(int competitiveGroupId)
        {
            return ImportEntities.Application.Where(c => c.OrderCompetitiveGroupID == competitiveGroupId);
        }

        public IQueryable<Application> FindApplicationsByOrderCompetitiveGroupItem(int orderCompetitiveGroupItemId)
        {
            return ImportEntities.Application.Where(c => c.OrderCompetitiveGroupItemID == orderCompetitiveGroupItemId);
        }

        public IQueryable<Application> FindApplicationsBySelectedCompetitiveGroupItem(int competitiveGroupItemId)
        {
            return ImportEntities.ApplicationSelectedCompetitiveGroupItem.Where(c =>
                c.CompetitiveGroupItemID == competitiveGroupItemId).Select(c => c.Application);
        }

        public Application[] TargetOrganizationItemLinkWithAppsInOrder(int competitiveGroupTargetId)
        {
            return ImportEntities.Application.Where(x => x.OrderCompetitiveGroupTargetID == competitiveGroupTargetId).ToArray();
        }

        public IQueryable<Application> FindApplicationsByCampaign(int campaignId)
        {
            return ImportEntities.Application.Where(c => c.OrderOfAdmission.CampaignID == campaignId);
        }

        public Application FindApplicationByNumber(string applicationNumber, bool seekInDb)
        {
            /* Быстро смотрим стоит ли лезть в БД за полным заявлением */
            var appShortRef = Applications.FirstOrDefault(c => c.ApplicationNumber == applicationNumber);
            if (appShortRef == null) return null;

            if (!seekInDb) return new Application
            {
                RegistrationDate = appShortRef.RegistrationDateDate,
                ApplicationNumber = appShortRef.ApplicationNumber,
                OrderOfAdmissionID = appShortRef.OrderOfAdmissionId,
                OriginalDocumentsReceivedDate = appShortRef.OriginalDocumentsReceivedDate,
                UID = appShortRef.UID
            };

            return ImportEntities.Application.FirstOrDefault(x =>
                x.InstitutionID == InstitutionId && x.ApplicationNumber == applicationNumber);
        }

        public Application FindApplicationByUID(string UID, bool seekInDb)
        {
            /* Быстро смотрим стоит ли лезть в БД за полным заявлением */
            var appShortRef = Applications.FirstOrDefault(c => c.UID == UID);
            if (appShortRef == null) return null;

            if (!seekInDb) return new Application
                {
                    RegistrationDate = appShortRef.RegistrationDateDate,
                    ApplicationNumber = appShortRef.ApplicationNumber,
                    OrderOfAdmissionID = appShortRef.OrderOfAdmissionId,
                    OriginalDocumentsReceivedDate = appShortRef.OriginalDocumentsReceivedDate,
                    UID = appShortRef.UID
                };

            return ImportEntities.Application.FirstOrDefault(x =>
                x.InstitutionID == InstitutionId && x.UID == UID);
        }

        public IQueryable<ApplicationEntranceTestDocument> FindApplicationEntranceTestResultsByEntranceTestItem(int entranceTestItemId)
        {
            return ImportEntities.ApplicationEntranceTestDocument
                .Where(x => x.EntranceTestItemID != null && x.EntranceTestItemID == entranceTestItemId);
        }

        public IQueryable<ApplicationEntranceTestDocument> FindApplicationEntranceTestResultsByApplication(int applicationId)
        {
            return ImportEntities.ApplicationEntranceTestDocument
                .Where(x =>
                    x.Application.InstitutionID == InstitutionId &&
                    x.ApplicationID == applicationId &&
                    x.EntranceTestItemID != null && x.SourceID != null);
        }
        #endregion

        /// <summary>
        /// Загружаем данные
        /// </summary>

        public abstract void LoadData();

        private static TValue GetDictionaryValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
                return value;
            return default(TValue);
        }

        public Direction GetDirection(int directionId)
        {
            return GetDictionaryValue(DbStableObjectRepository.Directions, directionId);
        }

        public OlympicDiplomType GetOlympicDiplomType(int olympicDiplomTypeId)
        {
            return GetDictionaryValue(DbStableObjectRepository.OlympicDiplomTypes, (short)olympicDiplomTypeId);
        }

        public OlympicType GetOlympicType(int olympicId)
        {
            return GetDictionaryValue(DbStableObjectRepository.OlympicTypes, olympicId);
        }

        public int GetGender(int genderId)
        {
            return genderId;
        }

        public CountryType GetNationality(int nationalityId)
        {
            return GetDictionaryValue(DbStableObjectRepository.CountryTypes, nationalityId);
        }

        public IdentityDocumentType GetIdentityDocumentType(int docTypeId)
        {
            return GetDictionaryValue(DbStableObjectRepository.IdentityDocumentTypes, docTypeId);
        }

        public bool IsRussianDoc(int docTypeId)
        {
            var d = GetIdentityDocumentType(docTypeId);
            if (d != null) return d.IsRussianNationality;
            return false;
        }

        public DisabilityType GetDisabilityType(int disabilityId)
        {
            return GetDictionaryValue(DbStableObjectRepository.DisabilityTypes, disabilityId);
        }

        public ApplicationStatusType GetApplicationStatus(int statusId)
        {
            return GetDictionaryValue(DbStableObjectRepository.ApplicationStatusTypes, statusId);
        }

        public Benefit GetBenefit(int benefitKindId)
        {
            return GetDictionaryValue(DbStableObjectRepository.Benefits, (short)benefitKindId);
        }

        public InstitutionDocumentType GetInstitutionDocument(int institutitonDocumentId)
        {
            return GetDictionaryValue(DbStableObjectRepository.InstitutionDocumentTypes, institutitonDocumentId);
        }

        public EntranceTestType GetEntranceTestType(int entrTestTypeId)
        {
            return GetDictionaryValue(DbStableObjectRepository.EntranceTestTypes, (short)entrTestTypeId);
        }

        public Subject GetSubject(int subjectId)
        {
            return GetDictionaryValue(DbStableObjectRepository.Subjects, subjectId);
        }

        public int GetSubjectEgeMinValue(int subjectId)
        {
            var d = GetDictionaryValue(DbStableObjectRepository.SubjectEgeMinValues, subjectId);
            if (d != null) return d.MinValue;
            return 0;
        }

        public AdmissionItemType GetEducationalLevel(int admissionItemTypeId)
        {
            var it = GetDictionaryValue(DbStableObjectRepository.AdmissionItemTypes, (short)admissionItemTypeId);
            if (it != null && it.ItemLevel == 2) return it;
            return null;
        }

        public AdmissionItemType GetFinanceSource(int admissionItemTypeId)
        {
            var it = GetDictionaryValue(DbStableObjectRepository.AdmissionItemTypes, (short)admissionItemTypeId);
            if (it != null && it.ItemLevel == 8) return it;
            return null;
        }

        public AdmissionItemType GetEducationForm(int admissionItemTypeId)
        {
            var it = GetDictionaryValue(DbStableObjectRepository.AdmissionItemTypes, (short)admissionItemTypeId);
            if (it != null && it.ItemLevel == 7) return it;
            return null;
        }

        public RegionType GetRegion(int regionId)
        {
            return GetDictionaryValue(DbStableObjectRepository.RegionTypes, regionId);
        }

        public CountryType GetCountry(int countryId)
        {
            return GetDictionaryValue(DbStableObjectRepository.CountryTypes, countryId);
        }

        public EntranceTestResultSource GetEnranceTestResultSource(int sourceId)
        {
            return GetDictionaryValue(DbStableObjectRepository.EntranceTestResultSources, sourceId);
        }

        public string GetDocumentTypeName(int documentTypeId)
        {
            var d = GetDictionaryValue(DbStableObjectRepository.DocumentTypes, documentTypeId);
            if (d != null) return d.Name;
            return null;
        }

        public GVUZ.Model.Entrants.LevelBudget GetLevelBudget(int levelBudgetId)
        {
            return GetDictionaryValue(DbStableObjectRepository.LevelBudgets, levelBudgetId);
        }

        public virtual void Dispose()
        {
            ImportEntities.Flush();
            ImportEntities.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
