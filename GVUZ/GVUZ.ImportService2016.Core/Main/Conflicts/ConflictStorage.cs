using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ServiceModel.Import;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Main
{
    public class ImportConflictStorage
    {
        private readonly VocabularyStorage vocabularyStorage;
        public ImportConflictStorage(VocabularyStorage _vocStorage)
        {
            vocabularyStorage = _vocStorage;
            ImportedApplicationIDs = new List<int>();
        }

        /// <summary>
        /// Сообщения об ошибках проверок: объект - ошибка 
        /// </summary>
        private Dictionary<IBroken, List<ConflictStorage.ConflictMessage>> conflictMessages = new Dictionary<IBroken, List<ConflictStorage.ConflictMessage>>();

        // Каждый словарь содержит dto объект, который не удалось импортировать со ссылкой на объекты в БД, 
        // с которыми dto объект вступил в конфликт.
        // В данном классе объекты хранятся в качестве ключа. Dto объекты добавляются уже с ParentUID, чтобы выводить информацию после импорта.
        // На основании этих словарей будет создана секция Log / Failed и секция Conflicts.

        // Что используем
        // Для сообщений о конфликтах
        public readonly Dictionary<IBroken, HashSet<CompetitiveGroupVocDto>> _linkedCompetitiveGroupsConflict = new Dictionary<IBroken, HashSet<CompetitiveGroupVocDto>>();
        public readonly Dictionary<IBroken, HashSet<int>> _linkedAdmissionVolumesConflict = new Dictionary<IBroken, HashSet<int>>();
        public readonly Dictionary<IBroken, HashSet<int>> _linkedInstitutionArchievementsConflict = new Dictionary<IBroken, HashSet<int>>();
        public readonly Dictionary<IBroken, HashSet<int>> _linkedRecomendedListsConflict = new Dictionary<IBroken, HashSet<int>>();
        public readonly Dictionary<IBroken, HashSet<ApplicationShortRef>> _linkedOrderOfAdmissionsConflict = new Dictionary<IBroken, HashSet<ApplicationShortRef>>();

        // э... нужно?
        //private readonly HashSet<int> _competitiveGroupsConflict = new HashSet<int>();
        
        // Для сообщений об ошибках
        // Блок Applications
        private readonly List<ApplicationFailDetailsDto> _appList = new List<ApplicationFailDetailsDto>();
        // AdmissionInfo
        private readonly List<AdmissionVolumeFailDetailsDto> _admVolumeList = new List<AdmissionVolumeFailDetailsDto>();
        private readonly List<DistributedAdmissionVolumeFailDetailsDto> _distrAdmVolumeList = new List<DistributedAdmissionVolumeFailDetailsDto>();
        private readonly List<CompetitiveGroupFailDetailsDto> _cgList = new List<CompetitiveGroupFailDetailsDto>();
        // CampaignInfo
        private readonly List<CampaignDetailsFailDto> _campaignList = new List<CampaignDetailsFailDto>();
        // InstitutionArchievements
        private readonly List<InstitutionAchievementFailDetailsDto> _institutionAchievementList = new List<InstitutionAchievementFailDetailsDto>();

        // OrderOfAdmission
        private readonly List<OrderFailDetailsDto> _orderList = new List<OrderFailDetailsDto>();
        private readonly List<OrderApplicationFailDetailsDto> _orderApplicationList = new List<OrderApplicationFailDetailsDto>();

        //InstitutionPrograms
        private readonly List<InstitutionProgramFailDetailsDto> _institutionProgramList = new List<InstitutionProgramFailDetailsDto>();

        //TargetOrganization
        private readonly List<TargetOrganizationFailDetailsDto> _targetOrgList = new List<TargetOrganizationFailDetailsDto>();

        // что НЕ используем
        //public readonly Dictionary<IBroken, HashSet<int>> _linkedCompetitiveGroupItemsConflict = new Dictionary<IBroken, HashSet<int>>(); 
        //private readonly Dictionary<IBroken, List<ApplicationShortRef>> _linkedApplicationsConflict = new Dictionary<IBroken, List<ApplicationShortRef>>();
        //private readonly Dictionary<IBroken, List<ApplicationShortRef>> _linkedOrdersOfAdmissionConflict = new Dictionary<IBroken, List<ApplicationShortRef>>();
        //private readonly Dictionary<IBroken, HashSet<int>> _linkedEntranceTestResultConflict = new Dictionary<IBroken, HashSet<int>>();
        //private readonly Dictionary<IBroken, HashSet<int>> _linkedApplicationCommonBenefitConflict = new Dictionary<IBroken, HashSet<int>>();

        // это объекты в БД, эти объекты не привязаны к Dto объектам		
        //private readonly HashSet<int> _competitiveGroupItemsConflict = new HashSet<int>();
        //
        //private List<ApplicationShortRef> _applicationsConflict = new List<ApplicationShortRef>();
        //private List<ApplicationShortRef> _consideredApplicationsConflict = new List<ApplicationShortRef>();
        //private List<ApplicationShortRef> _recommendedApplicationsConflict = new List<ApplicationShortRef>();
        //private readonly List<ApplicationShortRef> _ordersOfAdmissionConflictList = new List<ApplicationShortRef>();
        //private readonly HashSet<int> _entranceTestResultConflict = new HashSet<int>();
        //private readonly HashSet<int> _applicationCommonBenefitConflict = new HashSet<int>();
        //private readonly List<RecommendedListShort> _recommendedListConflict = new List<RecommendedListShort>();


        //private readonly List<CompetitiveGroupItemFailDetailsDto> _cgItemList = new List<CompetitiveGroupItemFailDetailsDto>();
        //private readonly List<TargetOrganizationDirectionFailDetailsDto> _cgtItemList = new List<TargetOrganizationDirectionFailDetailsDto>();
        //private readonly List<CampaignDateFailDto> _campaignDateList = new List<CampaignDateFailDto>();
        //private readonly List<ConsideredApplicationFailDetailsDto> _consideredAppList = new List<ConsideredApplicationFailDetailsDto>();
        //private readonly List<RecommendedApplicationFailDetailsDto> _recommendedAppList = new List<RecommendedApplicationFailDetailsDto>();
        //private readonly List<CommonBenefitFailDetailsDto> _commonBenefitList = new List<CommonBenefitFailDetailsDto>();

        //private readonly List<EntranceTestBenefitItemFailDetailsDto> _etBenefitItemList = new List<EntranceTestBenefitItemFailDetailsDto>();
        //private readonly List<EntranceTestItemFailDetailsDto> _etItemList = new List<EntranceTestItemFailDetailsDto>();

        //private readonly List<ApplicationCommonBenefitFailDetailsDto> _appCommonBenefitList = new List<ApplicationCommonBenefitFailDetailsDto>();
        //private readonly List<EntranceTestResultFailDetailsDto> _etResultItemList = new List<EntranceTestResultFailDetailsDto>();
        //private readonly List<RecommendedListFailDetailsDto> _recommendedList = new List<RecommendedListFailDetailsDto>();


        private ServiceModel.Import.Package.ImportResultPackage resultPackage = null;
        public SuccessfulImportStatisticsDto successfulImportStatisticsDto = new SuccessfulImportStatisticsDto();
        

        public ServiceModel.Import.Package.ImportResultPackage GetResultPackage(int importPackageId)
        {
            resultPackage = new ServiceModel.Import.Package.ImportResultPackage
            {
                Log = new LogDto
                {
                    Successful = successfulImportStatisticsDto,
                    Failed = GetFailedImportInfoDto()
                },
                Conflicts = GetConflictsResultDto(),
                PackageID = importPackageId.ToString()
            };
            return resultPackage;
        }

        /// <summary>
        /// ID импортированных заявлений для последующей проверки 
        /// </summary>
        public List<int> ImportedApplicationIDs { get; set; }
        public string ImportedApplicationIDsString { get { return string.Join(";", ImportedApplicationIDs); } }

        public void SetObjectIsBroken(IBroken importObject, int messageCode, params string[] messageParams)
        {
            SetObjectIsBroken(importObject, importObject, messageCode, messageParams);
        }

        public void SetObjectIsBroken(IBroken importObject, object notImported, int messageCode, params string[] messageParams)
        {
            importObject.IsBroken = true;

            ConflictStorage.ConflictMessage conflictMessage = new ConflictStorage.ConflictMessage()
            {
                Code = messageCode,
                Message = String.Format(ConflictMessages.GetMessage(messageCode), messageParams)
            };

            SetObjectIsBroken(importObject, conflictMessage);
        }

        public void SetObjectIsBroken(IBroken importObject, ConflictStorage.ConflictMessage conflictMessage)
        {
            importObject.IsBroken = true;

            if (!conflictMessages.ContainsKey(importObject))
                conflictMessages.Add(importObject, new List<ConflictStorage.ConflictMessage>() { conflictMessage });
            else
            {
                if (conflictMessages[importObject] == null)
                    conflictMessages[importObject] = new List<ConflictStorage.ConflictMessage>();

                conflictMessages[importObject].Add(conflictMessage);
            }
        }

        public void AddCompetitiveGroups(IBroken brokenObj, HashSet<CompetitiveGroupVocDto> cgIDs)
        {
            brokenObj.IsBroken = true;

            if (_linkedCompetitiveGroupsConflict.ContainsKey(brokenObj))
                _linkedCompetitiveGroupsConflict[brokenObj].UnionWith(cgIDs);
            else
                _linkedCompetitiveGroupsConflict.Add(brokenObj, cgIDs);

            //_competitiveGroupsConflict.UnionWith(cgIDs);
        }
        public void AddAdmissionVolumes(IBroken brokenObj, HashSet<int> cgIDs)
        {
            brokenObj.IsBroken = true;

            if (_linkedAdmissionVolumesConflict.ContainsKey(brokenObj))
                _linkedAdmissionVolumesConflict[brokenObj].UnionWith(cgIDs);
            else
                _linkedAdmissionVolumesConflict.Add(brokenObj, cgIDs);
        }
        public void AddinstitutionAchievements(IBroken brokenObj, HashSet<int> iaIDs)
        {
            brokenObj.IsBroken = true;

            if (_linkedInstitutionArchievementsConflict.ContainsKey(brokenObj))
                _linkedInstitutionArchievementsConflict[brokenObj].UnionWith(iaIDs);
            else
                _linkedInstitutionArchievementsConflict.Add(brokenObj, iaIDs);
        }
        public void AddRecomendedLists(IBroken brokenObj, HashSet<int> cgIDs)
        {
            brokenObj.IsBroken = true;

            if (_linkedRecomendedListsConflict.ContainsKey(brokenObj))
                _linkedRecomendedListsConflict[brokenObj].UnionWith(cgIDs);
            else
                _linkedRecomendedListsConflict.Add(brokenObj, cgIDs);
        }
        public void AddOrderOfAdmissions(IBroken brokenObj, HashSet<OrderOfAdmissionVocDto> orderOfAdmissions)
        {
            brokenObj.IsBroken = true;
            HashSet<ApplicationShortRef> items = new HashSet<ApplicationShortRef>();
            items.UnionWith(orderOfAdmissions.Select(t => new ApplicationShortRef() { ApplicationNumber = t.ApplicationNumber, RegistrationDateDate = t.ApplicationRegistrationDate }));

            if (_linkedOrderOfAdmissionsConflict.ContainsKey(brokenObj))
                _linkedOrderOfAdmissionsConflict[brokenObj].UnionWith(items);
            else
                _linkedOrderOfAdmissionsConflict.Add(brokenObj, items);
        }



        /// <summary>
        /// Формирования блока сообщений о конфликтах
        /// </summary>
        /// <returns></returns>
        public ConflictsResultDto GetConflictsResultDto()
        {
            HashSet<CompetitiveGroupVocDto> cgIDs = new HashSet<CompetitiveGroupVocDto>();
            foreach (HashSet<CompetitiveGroupVocDto> cgConflictIDs in _linkedCompetitiveGroupsConflict.Values)
                cgIDs.UnionWith(cgConflictIDs);
            //cgIDs.UnionWith(_competitiveGroupsConflict); ??

            HashSet<ApplicationShortRef> orderOfAdmissions = new HashSet<ApplicationShortRef>();
            foreach(HashSet<ApplicationShortRef> ooa in _linkedOrderOfAdmissionsConflict.Values)
                orderOfAdmissions.UnionWith(ooa);

            HashSet<int> iaIDs = new HashSet<int>();
            foreach (HashSet<int> iaConflictIDs in _linkedInstitutionArchievementsConflict.Values)
                iaIDs.UnionWith(iaConflictIDs);

            // todo import: Конфликты для Campaign по AdmissionVolume и RecommendedLists



            //HashSet<int> cgItemIDs = new HashSet<int>();
            //foreach (HashSet<int> cgItemConflictIDs in _linkedCompetitiveGroupItemsConflict.Values)
            //    cgItemIDs.UnionWith(cgItemConflictIDs);
            //cgItemIDs.UnionWith(_competitiveGroupItemsConflict);

            //HashSet<int> appCommonBenefits = new HashSet<int>();
            //foreach (HashSet<int> appCommonBenefitIDs in _linkedApplicationCommonBenefitConflict.Values)
            //    appCommonBenefits.UnionWith(appCommonBenefitIDs);
            //appCommonBenefits.UnionWith(_applicationCommonBenefitConflict);

            //HashSet<int> entranceTests = new HashSet<int>();
            //foreach (HashSet<int> entrTestIDs in _linkedEntranceTestResultConflict.Values)
            //    entranceTests.UnionWith(entrTestIDs);
            //entranceTests.UnionWith(_entranceTestResultConflict);

            var conflictsResultDto = new ConflictsResultDto();
            conflictsResultDto.CompetitiveGroups = !cgIDs.Any() ? null : cgIDs.Select(t=> t.CompetitiveGroupID.ToString()).Distinct().ToArray();
            conflictsResultDto.OrdersOfAdmission = (orderOfAdmissions.Count == 0) ? null : orderOfAdmissions.Distinct().ToArray();
            conflictsResultDto.InstitutionAchievements = (iaIDs.Count == 0) ? null : iaIDs.Distinct().Select(t=> t.ToString()).ToArray();
            // todo import: Конфликты для Campaign по AdmissionVolume и RecommendedLists


            //conflictsResultDto.Applications = (_applicationsConflict.Count == 0) ? null : _applicationsConflict.Distinct().ToArray();
            //conflictsResultDto.CompetitiveGroupItems = (cgItemIDs.Count == 0) ? null : cgItemIDs.Select(x => x.ToString()).Distinct().ToArray();
            //conflictsResultDto.ApplicationCommonBenefits = (appCommonBenefits.Count == 0) ? null : appCommonBenefits.Select(x => x.ToString()).Distinct().ToArray();
            //conflictsResultDto.EntranceTestResults = (entranceTests.Count == 0) ? null : entranceTests.Select(x => x.ToString()).Distinct().ToArray();
            //conflictsResultDto.OrdersOfAdmission = (_ordersOfAdmissionConflictList.Count == 0) ? null : _ordersOfAdmissionConflictList.Distinct().ToArray();
            //conflictsResultDto.ConsideredApplications = (_consideredApplicationsConflict.Count == 0) ? null : _consideredApplicationsConflict.Distinct().ToArray();
            //conflictsResultDto.RecommendedApplications = (_recommendedApplicationsConflict.Count == 0) ? null : _recommendedApplicationsConflict.Distinct().ToArray();
            //conflictsResultDto.RecommendedLists = (_recommendedListConflict.Count == 0) ? null : _recommendedListConflict.Distinct().ToArray();
            return conflictsResultDto;
        }

        public FailedImportInfoDto GetFailedImportInfoDto() 
        {
            foreach (IBroken broken in  conflictMessages.Keys)
                foreach (var message in conflictMessages[broken])
                {
                    AddConflictInfo(broken, message);
                }

            // Для тех, у кого конфликты пишутся в 
            foreach (IBroken broken in _linkedCompetitiveGroupsConflict.Keys)
                AddConflictInfo(broken, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                    ConflictMessages.DependedObjectsExists, 
                    new ConflictsResultDto()
                    {
                        CompetitiveGroups = _linkedCompetitiveGroupsConflict[broken].Select(x => x.CompetitiveGroupID.ToString()).ToArray()
                    },
                    _linkedCompetitiveGroupsConflict[broken].FirstOrDefault() != null ?
                        string.Format("(Конкурс, Название: {0}, UID: {1})", _linkedCompetitiveGroupsConflict[broken].FirstOrDefault().Name, _linkedCompetitiveGroupsConflict[broken].FirstOrDefault().UID)
                        : ""
                    )
                );

            foreach (IBroken broken in _linkedOrderOfAdmissionsConflict.Keys)
                AddConflictInfo(broken, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                    ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
                    {
                        OrdersOfAdmission = _linkedOrderOfAdmissionsConflict[broken].ToArray()
                    }
                    , _linkedOrderOfAdmissionsConflict[broken].FirstOrDefault() != null ?
                        string.Format("(Заявление в приказе, UID: {0})", _linkedOrderOfAdmissionsConflict[broken].FirstOrDefault().UID)
                        : ""
                ));

            foreach (IBroken broken in _linkedInstitutionArchievementsConflict.Keys)
                AddConflictInfo(broken, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                        ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
                        {
                            InstitutionAchievements = _linkedInstitutionArchievementsConflict[broken].Select(t=> t.ToString()).ToArray()
                        }
                        ,string.Format("(Индивидуальное достижение, ID: {0})", _linkedInstitutionArchievementsConflict[broken].First())
                    )
                );

            //foreach (var notImportedDto in _linkedCompetitiveGroupItemsConflict.Keys)
            //    AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
            //        ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
            //        {
            //            CompetitiveGroupItems = _linkedCompetitiveGroupItemsConflict[notImportedDto]
            //                                                .Select(x => x.ToString()).ToArray().NullOnEmpty()
            //        }));
            //foreach (var notImportedDto in _linkedEntranceTestResultConflict.Keys)
            //    AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
            //        ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
            //        {
            //            EntranceTestResults = _linkedEntranceTestResultConflict[notImportedDto]
            //                .Select(x => x.ToString()).ToArray().NullOnEmpty()
            //        }));

            //foreach (var notImportedDto in _linkedApplicationsConflict.Keys)
            //    AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
            //        ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
            //        {
            //            Applications = _linkedApplicationsConflict[notImportedDto].ToArray()
            //        }));

            //foreach (var notImportedDto in _linkedApplicationCommonBenefitConflict.Keys)
            //    AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
            //        ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
            //        {
            //            ApplicationCommonBenefits = _linkedApplicationCommonBenefitConflict[notImportedDto]
            //                .Select(x => x.ToString()).ToArray().NullOnEmpty()
            //        }));


            FailedImportInfoDto failedImportInfoDto = GetFailedImportInfo();
            return failedImportInfoDto;
        }

        /// <summary>
        /// Ошибки импорта объектов
        /// </summary>
        /// <returns></returns>
        private FailedImportInfoDto GetFailedImportInfo()
        {
            return new FailedImportInfoDto
            {
                Campaigns = (_campaignList.Count == 0) ? null : _campaignList.ToArray(),

                AdmissionVolumes = (_admVolumeList.Count == 0) ? null : _admVolumeList.ToArray(),
                DistributedAdmissionVolumes = (_distrAdmVolumeList.Count == 0) ? null : _distrAdmVolumeList.ToArray(),
                CompetitiveGroups = (_cgList.Count == 0) ? null : _cgList.ToArray(),

                InstitutionAchievements = (_institutionAchievementList.Count == 0) ? null : _institutionAchievementList.ToArray(),
                Applications = (_appList.Count == 0) ? null : _appList.ToArray(),

                OrdersOfAdmissions = (_orderList.Count == 0) ? null : _orderList.ToArray(),
                ApplicationsInOrders = (_orderApplicationList.Count == 0) ? null : _orderApplicationList.ToArray(),

                InstitutionPrograms = (_institutionProgramList.Count == 0) ? null : _institutionProgramList.ToArray(),

                TargetOrganizations = (_targetOrgList.Count == 0) ? null : _targetOrgList.ToArray(),

                // Все остальное. пока ненужное!
                CompetitiveGroupItems = null, //(_cgItemList.Count == 0)? null : _cgItemList.ToArray(),
                TargetOrganizationDirections = null, //(_cgtItemList.Count == 0) ? null : _cgtItemList.ToArray(),

                ConsideredApplications = null, // (_consideredAppList.Count == 0) ? null : _consideredAppList.ToArray(),
                RecommendedApplications = null, // (_recommendedAppList.Count == 0) ? null : _recommendedAppList.ToArray(),
                CommonBenefit = null, //(_commonBenefitList.Count == 0) ? null : _commonBenefitList.ToArray(),

                EntranceTestBenefits = null, //(_etBenefitItemList.Count == 0) ? null : _etBenefitItemList.ToArray(),
                EntranceTestItems = null, //(_etItemList.Count==0) ? null : _etItemList.ToArray(),
           
                ApplicationCommonBenefits = null, //(_appCommonBenefitList.Count == 0) ? null : _appCommonBenefitList.ToArray(),
                EntranceTestResults = null, //(_etResultItemList.Count == 0) ? null : _etResultItemList.ToArray(),

                CampaignDates = null, //(_campaignDateList.Count == 0) ? null : _campaignDateList.ToArray(),
                RecommendedLists = null, //(_recommendedList.Count == 0) ? null : _recommendedList.ToArray(),

            };
        }

        /// <summary>
        /// Добавление сообщения об ошибке в список Fail
        /// </summary>
        /// <param name="iBroken">Объект с ошибкой импорта</param>
        /// <param name="conflictMessage">Сообщение о конфликте</param>
        public void AddConflictInfo(IBroken iBroken, GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage conflictMessage)
        {
            // AdmissionInfo
            if (iBroken.GetType() == typeof(PackageDataAdmissionInfoItem)) // AdmissionVolume
                AdmissionVolumeConflict(iBroken as PackageDataAdmissionInfoItem, conflictMessage);
            else if (iBroken.GetType() == typeof(PackageDataAdmissionInfoItem1)) // DistributedAdmissionVolume
                DistributedAdmissionVolumeConflict(iBroken as PackageDataAdmissionInfoItem1, conflictMessage);
            else if (iBroken.GetType() == typeof(PackageDataAdmissionInfoCompetitiveGroup)) // CompetitiveGroup
                CompetitiveGroupConflict(iBroken as PackageDataAdmissionInfoCompetitiveGroup, conflictMessage);

            //if (iBroken.GetType() == typeof(DistributedAdmissionVolumeDto))
            //    DistributedAdmissionVolumeConflict(notImportedDto as DistributedAdmissionVolumeDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(CompetitiveGroupDto))
            //    CompetitiveGroupConflict(notImportedDto as CompetitiveGroupDto, conflictMessage);

            // CampaignInfo
            else if (iBroken.GetType() == typeof(PackageDataCampaignInfoCampaign))
                CampaignConflict(iBroken as PackageDataCampaignInfoCampaign, conflictMessage);
            
            // Applications
            else if (iBroken.GetType() == typeof(PackageDataApplication))
                ApplicationConflict(iBroken as PackageDataApplication, conflictMessage);

            // InstitutionArchievements
            else if (iBroken.GetType() == typeof(PackageDataInstitutionAchievement))
                InstitutionArchievementConflict(iBroken as PackageDataInstitutionAchievement, conflictMessage);

            else if (iBroken.GetType() == typeof(CompetitiveGroupTargetDto))
                CompetitiveGroupTargetConflict(iBroken as CompetitiveGroupTargetDto, conflictMessage);

            //else if (iBroken.GetType() == typeof(PackageDataOrdersOrderBase))
            //OrderOfAdmissionConflict(iBroken as PackageDataOrdersOrderBase, conflictMessage);
            else if (iBroken is IOrder)
                OrderOfAdmissionConflict(iBroken as IOrder, conflictMessage);

            else if (iBroken.GetType() == typeof(PackageDataOrdersApplication))
                OrderApplicationConflict(iBroken as PackageDataOrdersApplication, conflictMessage);

            else if (iBroken.GetType() == typeof(PackageDataInstitutionProgram))
                InstitutionProgramConflict(iBroken as PackageDataInstitutionProgram, conflictMessage);


            #region Все остальное, пока ненужное
            //else if (notImportedDto.GetType() == typeof(CompetitiveGroupItemDto))
            //    CompetitiveGroupItemConflict(notImportedDto as CompetitiveGroupItemDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(CompetitiveGroupTargetDto))
            //    CompetitiveGroupTargetConflict(notImportedDto as CompetitiveGroupTargetDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(CompetitiveGroupTargetItemDto))
            //    CompetitiveGroupTargetItemConflict(notImportedDto as CompetitiveGroupTargetItemDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(EntranceTestItemDto))
            //    EntranceTestConflict(notImportedDto as EntranceTestItemDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(BenefitItemDto))
            //    BenefitItemConflict(notImportedDto as BenefitItemDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(EntranceTestResultDocumentsDto))
            //    BenefitItemConflict(notImportedDto as BenefitItemDto, conflictMessage);

            //else if (notImportedDto.GetType() == typeof(ApplicationCommonBenefitDto))
            //    ApplicationCommonBenefitConflict(notImportedDto as ApplicationCommonBenefitDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(EntranceTestAppItemDto))
            //    ApplicationEntranceTestConflict(notImportedDto as EntranceTestAppItemDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(OrderOfAdmissionItemDto))
            //    OrderOfAdmissionConflict(notImportedDto as OrderOfAdmissionItemDto, conflictMessage);

            //else if (iBroken.GetType() == typeof(PackageDataCampaignInfoCampaignCampaignDate))
            //    CampaignDateConflict(iBroken as PackageDataCampaignInfoCampaignCampaignDate, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(ConsideredApplicationDto))
            //    ConsideredApplicationConflict(notImportedDto as ConsideredApplicationDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(RecommendedApplicationDto))
            //    RecommendedApplicationConflict(notImportedDto as RecommendedApplicationDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(RecommendedListDto))
            //    RecommendedListDtoConflict(notImportedDto as RecommendedListDto, conflictMessage);
            #endregion
        }


        /// <summary>
        /// Сообщение об ошибке импорта
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="conflictMessage"></param>
        /// <returns></returns>
        public ErrorInfoImportDto GetErrorInfo(IUid dto, GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage conflictMessage)
        {
            return new ErrorInfoImportDto
            {
                ErrorCode = conflictMessage.Code.ToString(),
                Message =
                        string.Format("{2} ({0}{1})",
                        dto.GetDescription(),
                        (string.IsNullOrEmpty(dto.UID) ? "" : ", UID:" + dto.UID),
                        String.IsNullOrWhiteSpace(conflictMessage.Message) ?
                        ConflictMessages.GetMessage(conflictMessage.Code) : conflictMessage.Message),
                ConflictItemsInfo = conflictMessage.ConflictItemsInfo
            };
        }

        #region Добавление сообщений о конфликтах
        // CampaignInfo
        private void CampaignConflict(PackageDataCampaignInfoCampaign cDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var fDto = new CampaignDetailsFailDto();
            fDto.Name = cDto.Name;
            fDto.ErrorInfo = GetErrorInfo(cDto, conflictMessage);
            _campaignList.Add(fDto);
        }

        // AdmissionInfo
        private void AdmissionVolumeConflict(PackageDataAdmissionInfoItem dto, ConflictStorage.ConflictMessage conflictMessage)
		{
            var admVolumeFailDto = new AdmissionVolumeFailDetailsDto();
            admVolumeFailDto.EducationLevelName = VocabularyStatic.AdmissionItemTypeVoc.GetNameByID(dto.EducationLevelID.To(0)); 
            admVolumeFailDto.DirectionName = VocabularyStatic.DirectionVoc.GetNameByID(dto.DirectionID);
            admVolumeFailDto.ErrorInfo = GetErrorInfo(dto, conflictMessage);
            _admVolumeList.Add(admVolumeFailDto);
		}

        private void DistributedAdmissionVolumeConflict(PackageDataAdmissionInfoItem1 dto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var davFailDto = new DistributedAdmissionVolumeFailDetailsDto();
            davFailDto.AdmissionVolumeUID = dto.AdmissionVolumeUID;
            davFailDto.LevelBudget = VocabularyStatic.LevelBudgetVoc.GetNameByID(dto.LevelBudget.To(0)); 
            davFailDto.ErrorInfo = GetErrorInfo(dto, conflictMessage);
            _distrAdmVolumeList.Add(davFailDto);
        }

        private void CompetitiveGroupConflict(PackageDataAdmissionInfoCompetitiveGroup dto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var cgFailDto = new CompetitiveGroupFailDetailsDto();
            cgFailDto.CompetitiveGroupName = dto.Name;
            cgFailDto.UID = dto.UID;
            cgFailDto.ErrorInfo = GetErrorInfo(dto, conflictMessage);
            _cgList.Add(cgFailDto);
        }

        // InstitutionArchievements
        private void InstitutionArchievementConflict(PackageDataInstitutionAchievement dto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new InstitutionAchievementFailDetailsDto();
            failDto.IAUID = dto.InstitutionAchievementUID;
            failDto.Name = dto.Name;
            failDto.ErrorInfo = GetErrorInfo(dto, conflictMessage);
            _institutionAchievementList.Add(failDto);
        }

        // Applications
        private void ApplicationConflict(PackageDataApplication applicationDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new ApplicationFailDetailsDto();
            failDto.ApplicationNumber = applicationDto.ApplicationNumber;
            failDto.RegistrationDate = applicationDto.RegistrationDate.GetDateTimeAsString();
            failDto.ErrorInfo = GetErrorInfo(applicationDto, conflictMessage);
            _appList.Add(failDto);
        }

        // OrderOfAdmission
        //private void OrderOfAdmissionConflict(PackageDataOrdersOrderBase orderDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new OrderFailDetailsDto();

        //    failDto.UID = orderDto.UID;
        //    failDto.ErrorInfo = GetErrorInfo(orderDto, conflictMessage);
        //    _orderList.Add(failDto);
        //}
        private void OrderOfAdmissionConflict(IOrder orderDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new OrderFailDetailsDto();

            failDto.OrderUID = orderDto.UID;
            failDto.OrderName = orderDto.OrderName;
            failDto.OrderNumber = orderDto.OrderNumber;
            failDto.OrderDate = orderDto.OrderDateSpecified ? orderDto.OrderDate.ToShortDateString() : string.Empty;
            failDto.ErrorInfo = GetErrorInfo(orderDto, conflictMessage);
            _orderList.Add(failDto);
        }
        // Applications in Orders 
        private void OrderApplicationConflict(PackageDataOrdersApplication applicationDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new OrderApplicationFailDetailsDto();
            failDto.UID = applicationDto.UID;
            failDto.OrderUID = applicationDto.OrderUID;
            failDto.ErrorInfo = GetErrorInfo(applicationDto, conflictMessage);
            _orderApplicationList.Add(failDto);
        }

        private void InstitutionProgramConflict(PackageDataInstitutionProgram programDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new InstitutionProgramFailDetailsDto();
            failDto.UID = programDto.UID;
            failDto.Name = programDto.Name;
            failDto.Code = programDto.Code;
            failDto.ErrorInfo = GetErrorInfo(programDto, conflictMessage);
            _institutionProgramList.Add(failDto);
        }


        private void CompetitiveGroupTargetConflict(CompetitiveGroupTargetDto cgTargetDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new TargetOrganizationFailDetailsDto();
            failDto.TargetOrganizationName = cgTargetDto.TargetOrganizationName;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(cgTargetDto);
            _targetOrgList.Add(failDto);
        }

        #region Все остальное, пока ненужное!

        //private void CampaignDateConflict(PackageDataCampaignInfoCampaignCampaignDate cDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var fDto = new CampaignDateFailDto();
        //    fDto.ErrorInfo = GetErrorInfo(cDto, conflictMessage);
        //    fDto.UID = cDto.UID;
        //    _campaignDateList.Add(fDto);
        //}

        //private void CompetitiveGroupTargetConflict(CompetitiveGroupTargetDto cgTargetDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new TargetOrganizationFailDetailsDto();
        //    failDto.TargetOrganizationName = cgTargetDto.TargetOrganizationName;
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(cgTargetDto);
        //    _targetOrgList.Add(failDto);
        //}

        //private void CompetitiveGroupItemConflict(CompetitiveGroupItemDto cgItemDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(cgItemDto.ParentUID);
        //    if (cgDto == null)
        //        LogHelper.Log.ErrorFormat(
        //            "Институт: {0}. Не найден родительский объект с UID: {1} для НКГ UID: {2}",
        //            _dbObjectRepo.InstitutionId, cgItemDto.ParentUID, cgItemDto.UID);
        //    else
        //        cgItemDto.CompetitiveGroupName = cgDto.Name;
        //    string name, code;
        //    if (GetDirection(cgItemDto.DirectionID, out name, out code))
        //    {
        //        cgItemDto.DirectionCode = code;
        //        cgItemDto.DirectionName = name;
        //    }

        //    var cgItemFail = Mapper.Map(cgItemDto, new CompetitiveGroupItemFailDetailsDto());
        //    cgItemFail.ErrorInfo = conflictMessage.GetErrorInfo(cgItemDto);

        //    _cgItemList.Add(cgItemFail);
        //}

        //private void CompetitiveGroupTargetItemConflict(CompetitiveGroupTargetItemDto cgtItemDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failInfo = new TargetOrganizationDirectionFailDetailsDto();

        //    //ParentUID - это TargetOrganization
        //    var groupTargetDto = _processedDtoObjectStorage.FindCompetitiveGroupTargetDto(cgtItemDto.ParentUID);
        //    var cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(cgtItemDto.CompetitiveGroupUID);

        //    string name, code;
        //    failInfo.TargetOrganizationName = groupTargetDto.TargetOrganizationName;
        //    failInfo.CompetitiveGroupName = cgDto.Name;
        //    if (GetDirection(cgtItemDto.DirectionID, out name, out code))
        //        failInfo.DirectionName = name;
        //    failInfo.EducationLevelName = GetEducationLevelName(cgtItemDto.EducationLevelID);
        //    failInfo.ErrorInfo = conflictMessage.GetErrorInfo(cgtItemDto);

        //    _cgtItemList.Add(failInfo);
        //}

        //private void BenefitItemConflict(BenefitItemDto benefitItemDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    if (benefitItemDto.IsCommonBenefit)
        //    {
        //        var failDto = new CommonBenefitFailDetailsDto();
        //        failDto.BenefitKindName = GetBenefitName(benefitItemDto.BenefitKindID);
        //        CompetitiveGroupDto cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(benefitItemDto.ParentUID);
        //        failDto.CompetitiveGroupName = cgDto.Name;
        //        failDto.ErrorInfo = conflictMessage.GetErrorInfo(benefitItemDto);
        //        _commonBenefitList.Add(failDto);
        //    }
        //    else
        //    {
        //        var failDto = new EntranceTestBenefitItemFailDetailsDto();
        //        failDto.BenefitKindName = GetBenefitName(benefitItemDto.BenefitKindID);
        //        var etItemDto = _processedDtoObjectStorage.FindEntranceTestItemDto(benefitItemDto.ParentUID);

        //        //пытаемся найти в завалившихся конфликтах
        //        if (etItemDto == null)
        //            etItemDto = _notImportedobjectWithErrorMessages.Keys.FirstOrDefault(x => x.UID == 
        //                benefitItemDto.ParentUID && x.GetType() == typeof(EntranceTestItemDto)) as EntranceTestItemDto;

        //        var cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(etItemDto.ParentUID);
        //        failDto.CompetitiveGroupName = cgDto.Name;
        //        failDto.EntranceTestType = GetEntranceTestTypeName(etItemDto.EntranceTestTypeID);
        //        failDto.SubjectName = GetSubject(etItemDto.EntranceTestSubject);
        //        failDto.ErrorInfo = conflictMessage.GetErrorInfo(benefitItemDto);
        //        _etBenefitItemList.Add(failDto);
        //    }
        //}

        //private void EntranceTestConflict(EntranceTestItemDto etDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new EntranceTestItemFailDetailsDto();
        //    var cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(etDto.ParentUID);
        //    failDto.CompetitiveGroupName = cgDto.Name;
        //    failDto.EntranceTestType = GetEntranceTestTypeName(etDto.EntranceTestTypeID);
        //    failDto.SubjectName = GetSubject(etDto.EntranceTestSubject);
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(etDto);
        //    _etItemList.Add(failDto);
        //}



        //private void ConsideredApplicationConflict(ConsideredApplicationDto applicationDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new ConsideredApplicationFailDetailsDto();
        //    failDto.ConsideredApplication = applicationDto;
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(applicationDto);
        //    _consideredAppList.Add(failDto);
        //}

        //private void RecommendedApplicationConflict(RecommendedApplicationDto applicationDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new RecommendedApplicationFailDetailsDto();
        //    failDto.RecommendedApplication = applicationDto;
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(applicationDto);
        //    _recommendedAppList.Add(failDto);
        //}



        //private void ApplicationEntranceTestConflict(EntranceTestAppItemDto etDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new EntranceTestResultFailDetailsDto();
        //    var appDto = _processedDtoObjectStorage.FindApplicationDto(etDto.ParentUID);
        //    failDto.ResultSourceType = GetEntranceTestResultSourceName(etDto.ResultSourceTypeID);
        //    failDto.SubjectName = GetSubject(etDto.EntranceTestSubject);
        //    failDto.ResultValue = etDto.ResultValue;
        //    failDto.ApplicationNumber = appDto.ApplicationNumber;
        //    failDto.RegistrationDate = appDto.RegistrationDateString;
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(etDto);
        //    _etResultItemList.Add(failDto);
        //}

        //private void ApplicationCommonBenefitConflict(ApplicationCommonBenefitDto appCommonBenefitDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new ApplicationCommonBenefitFailDetailsDto();
        //    failDto.BenefitKindName = GetBenefitName(appCommonBenefitDto.BenefitKindID);
        //    ApplicationDto appDto = _processedDtoObjectStorage.FindApplicationDto(appCommonBenefitDto.ParentUID);
        //    failDto.ApplicationNumber = appDto.ApplicationNumber;
        //    failDto.RegistrationDate = appDto.RegistrationDateString;
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(appCommonBenefitDto);
        //    _appCommonBenefitList.Add(failDto);
        //}

        //private void OrderOfAdmissionConflict(OrderOfAdmissionItemDto orderOfAdmissionItemDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new ApplicationFailDetailsDto();
        //    failDto.ApplicationNumber = orderOfAdmissionItemDto.Application.ApplicationNumber;
        //    failDto.RegistrationDate = orderOfAdmissionItemDto.Application.RegistrationDateString;
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(orderOfAdmissionItemDto);
        //    _orderOfAdmList.Add(failDto);
        //}

        //private void RecommendedListDtoConflict(RecommendedListDto recListDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new RecommendedListFailDetailsDto();
        //    failDto.Stage = recListDto.Stage.ToString();
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(recListDto);
        //    _recommendedList.Add(failDto);
        //}
        #endregion
        #endregion
    }
}
