using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model;
using System.Data.Entity;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Packages.Handlers
{
    public class DbObjectRepositoryWithCache : DbObjectRepositoryBase
    {
        public DbObjectRepositoryWithCache(int institutionID)
            : base(institutionID)
        {
        } 

        private Dictionary<Type, Dictionary<string, IObjectWithUID>> _objectsWithUID = new Dictionary<Type, Dictionary<string, IObjectWithUID>>();
        private Dictionary<Type, Dictionary<string, string>> _objectsWithParentUID = new Dictionary<Type, Dictionary<string, string>>();

        public override IQueryable<AdmissionVolume> AdmissionVolumes { get { return _admissionVolumes.AsQueryable(); } }
        private List<AdmissionVolume> _admissionVolumes = new List<AdmissionVolume>();
                
        private Dictionary<int, CompetitiveGroup> _competitiveGroupDictByID = new Dictionary<int, CompetitiveGroup>();

        public override IQueryable<CompetitiveGroup> CompetitiveGroups { get { return _competitiveGroups.AsQueryable(); } }
        private List<CompetitiveGroup> _competitiveGroups = new List<CompetitiveGroup>();

        public override IQueryable<CompetitiveGroupItem> CompetitiveGroupItems { get { return _competitiveGroupItems.AsQueryable(); } }
        private List<CompetitiveGroupItem> _competitiveGroupItems = new List<CompetitiveGroupItem>();

        public override IQueryable<CompetitiveGroupTarget> CompetitiveGroupTargets { get { return _competitiveGroupTargets.AsQueryable(); } }
        private List<CompetitiveGroupTarget> _competitiveGroupTargets = new List<CompetitiveGroupTarget>();

        public override IQueryable<CompetitiveGroupTargetItem> CompetitiveGroupTargetItems { get { return _competitiveGroupTargetItems.AsQueryable(); } }
        private List<CompetitiveGroupTargetItem> _competitiveGroupTargetItems = new List<CompetitiveGroupTargetItem>();

        public override IEnumerable<Campaign> Campaigns { get { return _campaigns; } }
        private List<Campaign> _campaigns = new List<Campaign>();

        public override IQueryable<BenefitItemC> CompetitiveGroupBenefitItemsForEntranceTest { get { return _competitiveGroupBenefitItemsForEntranceTest.AsQueryable(); } }
        private List<BenefitItemC> _competitiveGroupBenefitItemsForEntranceTest = new List<BenefitItemC>();

        public override IQueryable<BenefitItemC> CompetitiveGroupCommonBenefitItems { get { return _competitiveGroupCommonBenefitItems.AsQueryable(); } }
        private List<BenefitItemC> _competitiveGroupCommonBenefitItems = new List<BenefitItemC>();

        public override IEnumerable<EntranceTestItemC> CompetitiveGroupEntranceTestItems { get { return _competitiveGroupEntranceTestItems; } }
        private List<EntranceTestItemC> _competitiveGroupEntranceTestItems = new List<EntranceTestItemC>();

        public override IQueryable<ApplicationShortRef> Applications { get { return _applications.AsQueryable(); } }
        private HashSet<ApplicationShortRef> _applications = new HashSet<ApplicationShortRef>();

        public override IQueryable<ApplicationEntranceTestDocumentShortRef> ApplicationEntranceTestResults { get { return _applicationEntranceTestResults.AsQueryable(); } }
        private HashSet<ApplicationEntranceTestDocumentShortRef> _applicationEntranceTestResults = new HashSet<ApplicationEntranceTestDocumentShortRef>();

        public override IQueryable<ApplicationEntranceTestDocument> ApplicationEntranceTestBenefits { get { return _applicationEntranceTestBenefits.AsQueryable(); } }
        private List<ApplicationEntranceTestDocument> _applicationEntranceTestBenefits = new List<ApplicationEntranceTestDocument>();

        public override IQueryable<EntrantDocument> EntrantDocuments { get { return _entrantDocuments.AsQueryable(); } }
        private List<EntrantDocument> _entrantDocuments = new List<EntrantDocument>();

        public override IQueryable<OrderOfAdmission> OrdersOfAdmission { get { return _ordersOfAdmission.AsQueryable(); } }
        private List<OrderOfAdmission> _ordersOfAdmission = new List<OrderOfAdmission>();

        public override IQueryable<AllowedDirections> AllowedDirections { get { return _allowedDirections.AsQueryable(); } }
        private List<AllowedDirections> _allowedDirections = new List<AllowedDirections>();

        public override IQueryable<EntranceTestProfileDirection> EntranceTestProfileDirections { get { return _entranceTestProfileDirections.AsQueryable(); } }
        private List<EntranceTestProfileDirection> _entranceTestProfileDirections = new List<EntranceTestProfileDirection>();

        public override IEnumerable<DirectionSubjectLinkDirection> DirectionSubjectLinkDirections { get { return _directionSubjectLinkDirections; } }
        private List<DirectionSubjectLinkDirection> _directionSubjectLinkDirections = new List<DirectionSubjectLinkDirection>();

        public override IEnumerable<DirectionSubjectLink> DirectionSubjectLinks { get { return _directionSubjectLinks; } }
        private List<DirectionSubjectLink> _directionSubjectLinks = new List<DirectionSubjectLink>();

        public override IEnumerable<DirectionSubjectLinkSubject> DirectionSubjectLinkSubjects { get { return _directionSubjectLinkSubjects; } }
        private List<DirectionSubjectLinkSubject> _directionSubjectLinkSubjects = new List<DirectionSubjectLinkSubject>();

        public override IEnumerable<EntranceTestCreativeDirection> EntranceTestCreativeDirections { get { return _entranceTestCreativeDirections; } }
        private List<EntranceTestCreativeDirection> _entranceTestCreativeDirections = new List<EntranceTestCreativeDirection>();

        public override IEnumerable<OlympicTypeSubjectLink> OlympicTypeSubjectLinks { get { return _olympicTypeSubjectLinks; } }
        private List<OlympicTypeSubjectLink> _olympicTypeSubjectLinks = new List<OlympicTypeSubjectLink>();

        public override CompetitiveGroup GetCompetitiveGroupDictById(int id)
        {
            if (_competitiveGroupDictByID.ContainsKey(id))
                return _competitiveGroupDictByID[id];
            return null;
        }

        public override T GetObject<T>(string uid)
        {
            if (uid == null) throw new ArgumentNullException("uid");
            Dictionary<string, IObjectWithUID> objectWithUids = _objectsWithUID[typeof(T)];
            if (objectWithUids == null) return default(T);
            return objectWithUids.ContainsKey(uid) ? (T)objectWithUids[uid] : default(T);
        }

        public override string GetParentUID<T>(string uid)
        {
            Dictionary<string, string> objectWithParentUids = null;
            if (_objectsWithParentUID.ContainsKey(typeof(T)))
                objectWithParentUids = _objectsWithParentUID[typeof(T)];
            if (objectWithParentUids == null)
                return null;
            if (objectWithParentUids.ContainsKey(uid))
                return objectWithParentUids[uid];
            return null;
        }

        public  override void LoadData()
        {
            var sw = new Stopwatch();
            sw.Start();

            _objectsWithUID.Clear();
            _objectsWithParentUID.Clear();

            _admissionVolumes = ImportEntities.AdmissionVolume
                .Include(x => x.Campaign)
                .Where(x => x.InstitutionID == InstitutionId).ToList();

            _campaigns = ImportEntities.Campaign
                .Include(x => x.CampaignDate)
                .Include(x => x.CampaignEducationLevel)
                .Where(x => x.InstitutionID == InstitutionId).ToList();

            _competitiveGroups = ImportEntities.CompetitiveGroup
                .Include(x => x.Campaign)
                .Where(x => x.InstitutionID == InstitutionId).ToList();

            _competitiveGroupDictByID = _competitiveGroups.ToDictionary(x => x.CompetitiveGroupID, x => x);

            _competitiveGroupItems = ImportEntities.CompetitiveGroupItem
                .Where(x => x.CompetitiveGroup.InstitutionID == InstitutionId).ToList();

            _competitiveGroupTargets = ImportEntities.CompetitiveGroupTarget
                .Include(x => x.CompetitiveGroupTargetItem)
                .Where(x => x.InstitutionID == InstitutionId).ToList();

            _competitiveGroupTargetItems = ImportEntities.CompetitiveGroupTargetItem
                .Where(x => x.CompetitiveGroupTarget.InstitutionID == InstitutionId).ToList();

            _competitiveGroupBenefitItemsForEntranceTest = ImportEntities.BenefitItemC
                .Include(x => x.BenefitItemCOlympicType)
                .Where(x => x.CompetitiveGroup.InstitutionID == InstitutionId && x.EntranceTestItemID.HasValue).ToList();

            _competitiveGroupCommonBenefitItems = ImportEntities.BenefitItemC
                .Include(x => x.BenefitItemCOlympicType)
                .Where(x => x.CompetitiveGroup.InstitutionID == InstitutionId && !x.EntranceTestItemID.HasValue).ToList();

            _competitiveGroupEntranceTestItems = ImportEntities.EntranceTestItemC
                .Include(x => x.CompetitiveGroup)
                .Where(x => x.CompetitiveGroup.InstitutionID == InstitutionId).ToList();

            _applicationEntranceTestResults.Clear();

#warning Дичайшие тормоза!!!
            _applicationEntranceTestResults.UnionWith(ImportEntities.ApplicationEntranceTestDocument.Where(x =>
                    x.Application.InstitutionID == InstitutionId &&
                    x.EntranceTestItemID != null &&
                    x.Application.UID != null &&
                    x.UID != null)
                .Select(c => new ApplicationEntranceTestDocumentShortRef
                {
                    ApplicationID = c.ApplicationID,
                    ApplicationUID = c.Application.UID,
                    UID = c.UID
                }));

            _applicationEntranceTestBenefits = ImportEntities.ApplicationEntranceTestDocument
                .Include(x => x.Application) //ради application uid
                .Include(x => x.CompetitiveGroup) //ради competitiveGroup uid
                .Where(x => x.Application.InstitutionID == InstitutionId && x.EntranceTestItemID == null).ToList();

            _applications.Clear();
            _applications.UnionWith(ImportEntities.Application.Where(c => c.InstitutionID == InstitutionId).
                Select(c => new ApplicationShortRef
                {
                    ApplicationNumber = c.ApplicationNumber,
                    RegistrationDateDate = c.RegistrationDate,
                    OrderOfAdmissionId = c.OrderOfAdmissionID,
                    OriginalDocumentsReceivedDate = c.OriginalDocumentsReceivedDate,
                    UID = c.UID
                }));

#warning Дичайшие тормоза!!!
            _entrantDocuments = ImportEntities.EntrantDocument.Where(x => x.Entrant.InstitutionID == InstitutionId).ToList();

            _ordersOfAdmission = ImportEntities.OrderOfAdmission.Where(x => x.InstitutionID == InstitutionId).ToList();

            _objectsWithUID.Add(typeof(AdmissionVolume),
                _admissionVolumes.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));

            _objectsWithUID.Add(typeof(Campaign),
                _campaigns.Where(x => !string.IsNullOrEmpty(x.UID)).ToDictionary(x => x.UID, x => (IObjectWithUID)x));

            _objectsWithUID.Add(typeof(CompetitiveGroup),
                _competitiveGroups.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));

            _objectsWithUID.Add(typeof(CompetitiveGroupItem),
                _competitiveGroupItems.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));

            _objectsWithParentUID.Add(typeof(CompetitiveGroupItem),
                _competitiveGroupItems.Where(x => !string.IsNullOrEmpty(x.UID))
                .ToDictionary(x => x.UID, x => x.CompetitiveGroup.UID));

            _objectsWithUID.Add(typeof(CompetitiveGroupTarget),
                _competitiveGroupTargets.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));

            _objectsWithUID.Add(typeof(CompetitiveGroupTargetItem),
                _competitiveGroupTargetItems.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));

            _objectsWithParentUID.Add(typeof(CompetitiveGroupTargetItem),
                _competitiveGroupTargetItems.Where(x => !string.IsNullOrEmpty(x.UID)).ToDictionary(x => x.UID, x => x.CompetitiveGroupTarget.UID));

            _objectsWithUID.Add(typeof(BenefitItemC),
                _competitiveGroupBenefitItemsForEntranceTest.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));

            _objectsWithParentUID.Add(typeof(BenefitItemC),
                _competitiveGroupBenefitItemsForEntranceTest.Where(x => !string.IsNullOrEmpty(x.UID)).ToDictionary(x => x.UID, x => x.EntranceTestItemC.UID));

            _objectsWithUID.Add(typeof(EntranceTestItemC),
                _competitiveGroupEntranceTestItems.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));

            _objectsWithParentUID.Add(typeof(EntranceTestItemC),
                _competitiveGroupEntranceTestItems.Where(x => !string.IsNullOrEmpty(x.UID)).ToDictionary(x => x.UID, x => x.CompetitiveGroup.UID));

            _objectsWithUID.Add(typeof(OrderOfAdmission),
                _ordersOfAdmission.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));
            try
            {
                _objectsWithParentUID.Add(typeof(ApplicationEntranceTestResult),
                                          _applicationEntranceTestResults.Where(x => !string.IsNullOrEmpty(x.UID))
                                                                        .ToDictionary(x => x.UID, x => x.ApplicationUID));
            }
            catch (Exception ex)
            {

                /* Для поиска дубликатов в UIDах
                 * foreach (ApplicationEntranceTestDocumentShortRef result in ApplicationEntranceTestResults)
                {
                    LogHelper.Log.ErrorFormat("UIDы ApplicationEntranceTestResults {0};", result.UID);
                }*/
                LogHelper.Log.ErrorFormat("Ошибка добавления ApplicationEntranceTestResults в _objectsWithParentUID Exception: {0} \r\n Stack Trace: {1}", ex.Message, ex.StackTrace);
            }

#warning Похоже, параметр типа ApplicationEntranceTestBenefit нигде не запрашивается
            _objectsWithUID.Add(typeof(ApplicationEntranceTestBenefit),
                _applicationEntranceTestBenefits.Where(x => !string.IsNullOrEmpty(x.UID))
                .CheckDuplicates().ToDictionary(x => x.UID, x => (IObjectWithUID)x));



            _objectsWithParentUID.Add(typeof(ApplicationEntranceTestBenefit),
                _applicationEntranceTestBenefits.Where(x => !string.IsNullOrEmpty(x.UID))
                .ToDictionary(x => x.UID, x => x.Application.UID));

            _allowedDirections = ImportEntities.AllowedDirections.Where(x => x.InstitutionID == InstitutionId).ToList();

            _entranceTestProfileDirections = ImportEntities.EntranceTestProfileDirection
                .Where(x => x.InstitutionID == InstitutionId).ToList();

            //загружаем объекты, которые не зависят от вуза
            DbStableObjectRepository.Load(ImportEntities);
            _directionSubjectLinkDirections = new List<DirectionSubjectLinkDirection>(DbStableObjectRepository.DirectionSubjectLinkDirections);
            _directionSubjectLinks = new List<DirectionSubjectLink>(DbStableObjectRepository.DirectionSubjectLinks);
            _directionSubjectLinkSubjects = new List<DirectionSubjectLinkSubject>(DbStableObjectRepository.DirectionSubjectLinkSubjects);
            _entranceTestCreativeDirections = new List<EntranceTestCreativeDirection>(DbStableObjectRepository.EntranceTestCreativeDirections);
            _olympicTypeSubjectLinks = new List<OlympicTypeSubjectLink>(DbStableObjectRepository.OlympicTypeSubjectLinks);
            sw.Stop();
            LogHelper.Log.DebugFormat("Загрузка LoadData ({0}) миллисекунд или ({1}) секунд", sw.Elapsed.Milliseconds, sw.Elapsed.Seconds);
        }

        public override void Dispose()
        {
            _objectsWithParentUID.Clear();
            _objectsWithParentUID = null;
            _objectsWithUID.Clear();
            _objectsWithUID = null; 

            _admissionVolumes.Clear();
            _admissionVolumes = null;
            _allowedDirections.Clear();
            _allowedDirections = null;
            _applicationEntranceTestBenefits.Clear();
            _applicationEntranceTestBenefits = null;
            _applicationEntranceTestResults.Clear();
            _applicationEntranceTestResults = null;
            _applications.Clear();
            _applications = null;
            _campaigns.Clear();
            _campaigns = null;
            _competitiveGroupBenefitItemsForEntranceTest.Clear();
            _competitiveGroupBenefitItemsForEntranceTest = null;
            _competitiveGroupCommonBenefitItems.Clear();
            _competitiveGroupCommonBenefitItems = null;
            _competitiveGroupDictByID.Clear();
            _competitiveGroupDictByID = null;
            _competitiveGroupEntranceTestItems.Clear();
            _competitiveGroupEntranceTestItems = null;
            _competitiveGroupItems.Clear();
            _competitiveGroupItems = null;
            _competitiveGroupTargetItems.Clear();
            _competitiveGroupTargetItems = null;
            _competitiveGroupTargets.Clear();
            _competitiveGroupTargets = null;
            _competitiveGroups.Clear();
            _competitiveGroups = null;
            _directionSubjectLinkDirections.Clear();
            _directionSubjectLinkDirections = null;
            _directionSubjectLinkSubjects.Clear();
            _directionSubjectLinkSubjects = null;
            _directionSubjectLinks.Clear();
            _directionSubjectLinks = null;
            _entranceTestCreativeDirections.Clear();
            _entranceTestCreativeDirections = null;
            _olympicTypeSubjectLinks.Clear();
            _olympicTypeSubjectLinks = null;
            _entranceTestProfileDirections.Clear();
            _entranceTestProfileDirections = null;
            _entrantDocuments.Clear();
            _entrantDocuments = null;

            base.Dispose();//Вызывать именно после очистки всех коллекций
        }
    }
}
