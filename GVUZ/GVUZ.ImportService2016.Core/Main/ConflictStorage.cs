using GVUZ.ImportService2015.Dto.Import;
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
using FogSoft.Helpers;

namespace GVUZ.ImportService2015.Core
{
    public class ImportConflictStorage
    {
        /// <summary>
        /// Сообщения об ошибках проверок: объект - ошибка 
        /// </summary>
        private Dictionary<IBroken, List<GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage>> conflictMessages = new Dictionary<IBroken, List<ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage>>();

        // public readonly Dictionary<IBroken, HashSet<int>> _linkedCompetitiveGroupsConflict = new Dictionary<IBroken, HashSet<int>>();
        // private readonly HashSet<int> _competitiveGroupsConflict = new HashSet<int>();

        // Каждый словарь содержит dto объект, который не удалось импортировать со ссылкой на объекты в БД, 
        // с которыми dto объект вступил в конфликт.
        // В данном классе объекты хранятся в качестве ключа. Dto объекты добавляются уже с ParentUID, чтобы выводить информацию после импорта.
        // На основании этих словарей будет создана секция Log / Failed и секция Conflicts.
        public readonly Dictionary<IBroken, HashSet<int>> _linkedCompetitiveGroupItemsConflict = new Dictionary<IBroken, HashSet<int>>();
        public readonly Dictionary<IBroken, HashSet<int>> _linkedCompetitiveGroupsConflict = new Dictionary<IBroken, HashSet<int>>();
        private readonly Dictionary<IBroken, List<ApplicationShortRef>> _linkedApplicationsConflict = new Dictionary<IBroken, List<ApplicationShortRef>>();
        private readonly Dictionary<IBroken, List<ApplicationShortRef>> _linkedOrdersOfAdmissionConflict = new Dictionary<IBroken, List<ApplicationShortRef>>();
        private readonly Dictionary<IBroken, HashSet<int>> _linkedEntranceTestResultConflict = new Dictionary<IBroken, HashSet<int>>();
        private readonly Dictionary<IBroken, HashSet<int>> _linkedApplicationCommonBenefitConflict = new Dictionary<IBroken, HashSet<int>>();

        // это объекты в БД, эти объекты не привязаны к Dto объектам		
        private readonly HashSet<int> _competitiveGroupItemsConflict = new HashSet<int>();
        private readonly HashSet<int> _competitiveGroupsConflict = new HashSet<int>();
        private List<ApplicationShortRef> _applicationsConflict = new List<ApplicationShortRef>();
        private List<ApplicationShortRef> _consideredApplicationsConflict = new List<ApplicationShortRef>();
        private List<ApplicationShortRef> _recommendedApplicationsConflict = new List<ApplicationShortRef>();
        private readonly List<ApplicationShortRef> _ordersOfAdmissionConflictList = new List<ApplicationShortRef>();
        private readonly HashSet<int> _entranceTestResultConflict = new HashSet<int>();
        private readonly HashSet<int> _applicationCommonBenefitConflict = new HashSet<int>();
        private readonly List<RecommendedListShort> _recommendedListConflict = new List<RecommendedListShort>();


        private readonly List<CompetitiveGroupItemFailDetailsDto> _cgItemList = new List<CompetitiveGroupItemFailDetailsDto>();
        private readonly List<TargetOrganizationDirectionFailDetailsDto> _cgtItemList = new List<TargetOrganizationDirectionFailDetailsDto>();
        private readonly List<AdmissionVolumeFailDetailsDto> _admVolumeList = new List<AdmissionVolumeFailDetailsDto>();
        private readonly List<DistributedAdmissionVolumeFailDetailsDto> _distrAdmVolumeList = new List<DistributedAdmissionVolumeFailDetailsDto>();
        private readonly List<CampaignDetailsFailDto> _campaignList = new List<CampaignDetailsFailDto>();
        private readonly List<CampaignDateFailDto> _campaignDateList = new List<CampaignDateFailDto>();
        private readonly List<ApplicationFailDetailsDto> _appList = new List<ApplicationFailDetailsDto>();
        private readonly List<ConsideredApplicationFailDetailsDto> _consideredAppList = new List<ConsideredApplicationFailDetailsDto>();
        private readonly List<RecommendedApplicationFailDetailsDto> _recommendedAppList = new List<RecommendedApplicationFailDetailsDto>();
        private readonly List<CommonBenefitFailDetailsDto> _commonBenefitList = new List<CommonBenefitFailDetailsDto>();
        private readonly List<CompetitiveGroupFailDetailsDto> _cgList = new List<CompetitiveGroupFailDetailsDto>();
        private readonly List<EntranceTestBenefitItemFailDetailsDto> _etBenefitItemList = new List<EntranceTestBenefitItemFailDetailsDto>();
        private readonly List<EntranceTestItemFailDetailsDto> _etItemList = new List<EntranceTestItemFailDetailsDto>();
        private readonly List<ApplicationFailDetailsDto> _orderOfAdmList = new List<ApplicationFailDetailsDto>();
        private readonly List<ApplicationCommonBenefitFailDetailsDto> _appCommonBenefitList = new List<ApplicationCommonBenefitFailDetailsDto>();
        private readonly List<EntranceTestResultFailDetailsDto> _etResultItemList = new List<EntranceTestResultFailDetailsDto>();
        private readonly List<TargetOrganizationFailDetailsDto> _targetOrgList = new List<TargetOrganizationFailDetailsDto>();
        private readonly List<RecommendedListFailDetailsDto> _recommendedList = new List<RecommendedListFailDetailsDto>();
        private readonly List<InstitutionAchievementFailDetailsDto> _institutionAchievementList = new List<InstitutionAchievementFailDetailsDto>();

        private GVUZ.ServiceModel.Import.Package.ImportResultPackage resultPackage = null;
        public GVUZ.ServiceModel.Import.WebService.Dto.Result.Import.SuccessfulImportStatisticsDto successfulImportStatisticsDto = new SuccessfulImportStatisticsDto();
        

        public GVUZ.ServiceModel.Import.Package.ImportResultPackage GetResultPackage(int importPackageId)
        {
            resultPackage = new GVUZ.ServiceModel.Import.Package.ImportResultPackage
            {
                Log = new GVUZ.ServiceModel.Import.WebService.Dto.Result.Import.LogDto
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
        /// ID импортированных заявлений для поля 
        /// </summary>
        public string ImportedApplicationIDs {get; set;}

        public void SetObjectIsBroken(IBroken importObject, int messageCode, params string[] messageParams)
        {
            SetObjectIsBroken(importObject, importObject, messageCode, messageParams);
        }

        public void SetObjectIsBroken(IBroken importObject, object notImported, int messageCode, params string[] messageParams)
        {
            importObject.IsBroken = true;

            GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage conflictMessage = new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage()
            {
                Code = messageCode,
                Message = String.Format(ConflictMessages.GetMessage(messageCode), messageParams)
            };

            SetObjectIsBroken(importObject, conflictMessage);
        }

        public void SetObjectIsBroken(IBroken importObject, ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage conflictMessage)
        {
            if (!conflictMessages.ContainsKey(importObject))
                conflictMessages.Add(importObject, new List<ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage>() { conflictMessage });
            else
            {
                if (conflictMessages[importObject] == null)
                    conflictMessages[importObject] = new List<ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage>();

                conflictMessages[importObject].Add(conflictMessage);
            }
        }

        public void AddCompetitiveGroups(IBroken brokenObj, HashSet<int> cgIDs)
        {
            brokenObj.IsBroken = true;

            if (_linkedCompetitiveGroupsConflict.ContainsKey(brokenObj))
                _linkedCompetitiveGroupsConflict[brokenObj].UnionWith(cgIDs);
            else
                _linkedCompetitiveGroupsConflict.Add(brokenObj, cgIDs);

            _competitiveGroupsConflict.UnionWith(cgIDs);
        }


        public GVUZ.ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto GetConflictsResultDto()
        {
            HashSet<int> cgIDs = new HashSet<int>();
            foreach (HashSet<int> cgConflictIDs in _linkedCompetitiveGroupsConflict.Values)
                cgIDs.UnionWith(cgConflictIDs);
            cgIDs.UnionWith(_competitiveGroupsConflict);

            HashSet<int> cgItemIDs = new HashSet<int>();
            foreach (HashSet<int> cgItemConflictIDs in _linkedCompetitiveGroupItemsConflict.Values)
                cgItemIDs.UnionWith(cgItemConflictIDs);
            cgItemIDs.UnionWith(_competitiveGroupItemsConflict);

            HashSet<int> appCommonBenefits = new HashSet<int>();
            foreach (HashSet<int> appCommonBenefitIDs in _linkedApplicationCommonBenefitConflict.Values)
                appCommonBenefits.UnionWith(appCommonBenefitIDs);
            appCommonBenefits.UnionWith(_applicationCommonBenefitConflict);

            HashSet<int> entranceTests = new HashSet<int>();
            foreach (HashSet<int> entrTestIDs in _linkedEntranceTestResultConflict.Values)
                entranceTests.UnionWith(entrTestIDs);
            entranceTests.UnionWith(_entranceTestResultConflict);

            var conflictsResultDto = new GVUZ.ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto();
            conflictsResultDto.Applications = (_applicationsConflict.Count == 0) ? null : _applicationsConflict.Distinct().ToArray();
            conflictsResultDto.CompetitiveGroupItems = (cgItemIDs.Count == 0) ? null : cgItemIDs.Select(x => x.ToString()).Distinct().ToArray();
            conflictsResultDto.CompetitiveGroups = (cgIDs.Count == 0) ? null : cgIDs.Select(x => x).Distinct().ToArray();
            conflictsResultDto.ApplicationCommonBenefits = (appCommonBenefits.Count == 0) ? null : appCommonBenefits.Select(x => x.ToString()).Distinct().ToArray();
            conflictsResultDto.EntranceTestResults = (entranceTests.Count == 0) ? null : entranceTests.Select(x => x.ToString()).Distinct().ToArray();
            conflictsResultDto.OrdersOfAdmission = (_ordersOfAdmissionConflictList.Count == 0) ? null : _ordersOfAdmissionConflictList.Distinct().ToArray();
            conflictsResultDto.ConsideredApplications = (_consideredApplicationsConflict.Count == 0) ? null : _consideredApplicationsConflict.Distinct().ToArray();
            conflictsResultDto.RecommendedApplications = (_recommendedApplicationsConflict.Count == 0) ? null : _recommendedApplicationsConflict.Distinct().ToArray();
            conflictsResultDto.RecommendedLists = (_recommendedListConflict.Count == 0) ? null : _recommendedListConflict.Distinct().ToArray();
            return conflictsResultDto;
        }

        public GVUZ.ServiceModel.Import.WebService.Dto.Result.Import.FailedImportInfoDto GetFailedImportInfoDto() //DtoObjectStorage processedDtoStorage)
        {
            //var notImportedHelper = new NotImportedHelper(_dbObjectRepo, processedDtoStorage, _notImportedobjectWithErrorMessages);
            foreach (IBroken broken in  conflictMessages.Keys)
                foreach (var message in conflictMessages[broken])
                {
                    AddConflictInfo(broken, message);
                }
            //foreach (KeyValuePair<BaseDto, ConflictMessage> notImportedObject in _notImportedobjectWithErrorMessages)
            //    notImportedHelper.AddConflictInfo(notImportedObject.Key, notImportedObject.Value);

            foreach (var notImportedDto in _linkedCompetitiveGroupsConflict.Keys)
                AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                    ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
                    {
                        CompetitiveGroups = _linkedCompetitiveGroupsConflict[notImportedDto].Select(x => x).ToArray()
                    }));
            foreach (var notImportedDto in _linkedCompetitiveGroupItemsConflict.Keys)
                AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                    ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
                    {
                        CompetitiveGroupItems = _linkedCompetitiveGroupItemsConflict[notImportedDto]
                                                            .Select(x => x.ToString()).ToArray().NullOnEmpty()
                    }));
            foreach (var notImportedDto in _linkedEntranceTestResultConflict.Keys)
                AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                    ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
                    {
                        EntranceTestResults = _linkedEntranceTestResultConflict[notImportedDto]
                            .Select(x => x.ToString()).ToArray().NullOnEmpty()
                    }));

            foreach (var notImportedDto in _linkedApplicationsConflict.Keys)
                AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                    ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
                    {
                        Applications = _linkedApplicationsConflict[notImportedDto].ToArray()
                    }));

            foreach (var notImportedDto in _linkedApplicationCommonBenefitConflict.Keys)
                AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                    ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
                    {
                        ApplicationCommonBenefits = _linkedApplicationCommonBenefitConflict[notImportedDto]
                            .Select(x => x.ToString()).ToArray().NullOnEmpty()
                    }));
            foreach (var notImportedDto in _linkedOrdersOfAdmissionConflict.Keys)
                AddConflictInfo(notImportedDto, new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage(
                    ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
                    {
                        OrdersOfAdmission = _linkedOrdersOfAdmissionConflict[notImportedDto].ToArray()
                    }));

            GVUZ.ServiceModel.Import.WebService.Dto.Result.Import.FailedImportInfoDto failedImportInfoDto = GetFailedImportInfo();
            return failedImportInfoDto;
        }

        private FailedImportInfoDto GetFailedImportInfo()
		{
			return new FailedImportInfoDto
			       	{
								CompetitiveGroupItems = (_cgItemList.Count == 0)? null : _cgItemList.ToArray(),
								TargetOrganizationDirections = (_cgtItemList.Count == 0) ? null : _cgtItemList.ToArray(),
								AdmissionVolumes = (_admVolumeList.Count == 0) ? null : _admVolumeList.ToArray(),
                                DistributedAdmissionVolumes = (_distrAdmVolumeList.Count == 0) ? null : _distrAdmVolumeList.ToArray(),
								Applications = (_appList.Count == 0) ? null : _appList.ToArray(),
                                ConsideredApplications = (_consideredAppList.Count == 0) ? null : _consideredAppList.ToArray(),
                                RecommendedApplications = (_recommendedAppList.Count == 0) ? null : _recommendedAppList.ToArray(),
								CommonBenefit = (_commonBenefitList.Count == 0) ? null : _commonBenefitList.ToArray(),
								CompetitiveGroups = (_cgList.Count == 0) ? null : _cgList.ToArray(),
								EntranceTestBenefits = (_etBenefitItemList.Count == 0) ? null : _etBenefitItemList.ToArray(),
								EntranceTestItems = (_etItemList.Count==0) ? null : _etItemList.ToArray(),
								OrdersOfAdmissions = (_orderOfAdmList.Count==0) ? null : _orderOfAdmList.ToArray(),
								TargetOrganizations = (_targetOrgList.Count == 0) ? null : _targetOrgList.ToArray(),
								ApplicationCommonBenefits = (_appCommonBenefitList.Count == 0) ? null : _appCommonBenefitList.ToArray(),
								EntranceTestResults = (_etResultItemList.Count == 0) ? null : _etResultItemList.ToArray(),
								Campaigns = (_campaignList.Count == 0) ? null : _campaignList.ToArray(),
								CampaignDates = (_campaignDateList.Count == 0) ? null : _campaignDateList.ToArray(),
                                RecommendedLists = (_recommendedList.Count == 0) ? null : _recommendedList.ToArray(),
                                InstitutionAchievements = (_institutionAchievementList.Count == 0) ? null : _institutionAchievementList.ToArray()
			       	};
		}

        public void AddConflictInfo(IBroken iBroken, GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage conflictMessage)
        {
            if (iBroken.GetType() == typeof(PackageDataAdmissionInfoItem))
                AdmissionVolumeConflict(iBroken as PackageDataAdmissionInfoItem, conflictMessage);
            //if (iBroken.GetType() == typeof(DistributedAdmissionVolumeDto))
            //    DistributedAdmissionVolumeConflict(notImportedDto as DistributedAdmissionVolumeDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(CompetitiveGroupDto))
            //    CompetitiveGroupConflict(notImportedDto as CompetitiveGroupDto, conflictMessage);
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
            else if (iBroken.GetType() == typeof(PackageDataApplication))
                ApplicationConflict(iBroken as PackageDataApplication, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(ApplicationCommonBenefitDto))
            //    ApplicationCommonBenefitConflict(notImportedDto as ApplicationCommonBenefitDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(EntranceTestAppItemDto))
            //    ApplicationEntranceTestConflict(notImportedDto as EntranceTestAppItemDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(OrderOfAdmissionItemDto))
            //    OrderOfAdmissionConflict(notImportedDto as OrderOfAdmissionItemDto, conflictMessage);
            else if (iBroken.GetType() == typeof(PackageDataCampaignInfoCampaign))
                CampaignConflict(iBroken as PackageDataCampaignInfoCampaign, conflictMessage);
            else if (iBroken.GetType() == typeof(PackageDataCampaignInfoCampaignCampaignDate))
                CampaignDateConflict(iBroken as PackageDataCampaignInfoCampaignCampaignDate, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(ConsideredApplicationDto))
            //    ConsideredApplicationConflict(notImportedDto as ConsideredApplicationDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(RecommendedApplicationDto))
            //    RecommendedApplicationConflict(notImportedDto as RecommendedApplicationDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(RecommendedListDto))
            //    RecommendedListDtoConflict(notImportedDto as RecommendedListDto, conflictMessage);
            //else if (notImportedDto.GetType() == typeof(InstitutionAchievementDto))
            //    InstitutionArchievementConflict(notImportedDto as InstitutionAchievementDto, conflictMessage);
        }

        public ErrorInfoImportDto GetErrorInfo(IUid dto, GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage conflictMessage)
        {
            return new ErrorInfoImportDto
            {
                ErrorCode = conflictMessage.Code.ToString(),
                Message =
                        string.Format("{2} ({0}, UID: {1})",
                        dto.GetDescription(),
                        (string.IsNullOrEmpty(dto.UID) ? "Не указан" : dto.UID),
                        String.IsNullOrWhiteSpace(conflictMessage.Message) ?
                        ConflictMessages.GetMessage(conflictMessage.Code) : conflictMessage.Message),
                ConflictItemsInfo = conflictMessage.ConflictItemsInfo
            };
        }

        #region Добавление сообщений о конфликтах
        private void AdmissionVolumeConflict(PackageDataAdmissionInfoItem aVolumeDto, ConflictStorage.ConflictMessage conflictMessage)
		{
            //var admVolumeFailDto = new AdmissionVolumeFailDetailsDto();
            //admVolumeFailDto.EducationLevelName = aVolumeDto.EducationLevelID.ToString(); // GetEducationLevelName(aVolumeDto.EducationLevelID);
            //admVolumeFailDto.DirectionName = GetDirectionName(aVolumeDto.DirectionID);
            //admVolumeFailDto.ErrorInfo = conflictMessage.GetErrorInfo(aVolumeDto);
            //_admVolumeList.Add(admVolumeFailDto);
		}

        //private void DistributedAdmissionVolumeConflict(DistributedAdmissionVolumeDto davDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var davFailDto = new DistributedAdmissionVolumeFailDetailsDto();
        //    davFailDto.AdmissionVolumeUID = davDto.AdmissionVolumeUID;
        //    davFailDto.LevelBudget = davDto.LevelBudget; // TODO Roman: GetLevelBudgetName( davDto.LevelBudget);
        //    davFailDto.ErrorInfo = //conflictMessage.GetErrorInfo(davDto);
        //        new ErrorInfoImportDto { 
        //            ConflictItemsInfo = conflictMessage.ConflictItemsInfo, 
        //            ErrorCode = conflictMessage.Code.ToString(),
        //            Message = String.IsNullOrWhiteSpace(conflictMessage.Message) ? GVUZ.ServiceModel.Import.Core.Operations.Conflicts.ConflictMessages.GetMessage(conflictMessage.Code) : conflictMessage.Message
        //        };
        //    _distrAdmVolumeList.Add(davFailDto);
        //}

        private void CampaignConflict(PackageDataCampaignInfoCampaign cDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var fDto = new CampaignDetailsFailDto();
			fDto.Name = cDto.Name;
            fDto.ErrorInfo = GetErrorInfo(cDto, conflictMessage);
			_campaignList.Add(fDto);
		}

        private void CampaignDateConflict(PackageDataCampaignInfoCampaignCampaignDate cDto, ConflictStorage.ConflictMessage conflictMessage)
		{
            var fDto = new CampaignDateFailDto();
            fDto.ErrorInfo = GetErrorInfo(cDto, conflictMessage);
			fDto.UID = cDto.UID;
			_campaignDateList.Add(fDto);
		}
		
        //private void CompetitiveGroupConflict(CompetitiveGroupDto cgDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var cgFailDto = new CompetitiveGroupFailDetailsDto();
        //    cgFailDto.CompetitiveGroupName = cgDto.Name;
        //    cgFailDto.ErrorInfo = conflictMessage.GetErrorInfo(cgDto);
        //    _cgList.Add(cgFailDto);
        //}
		
        //private void CompetitiveGroupTargetConflict(CompetitiveGroupTargetDto cgTargetDto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new TargetOrganizationFailDetailsDto();
        //    failDto.TargetOrganizationName = cgTargetDto.TargetOrganizationName;
        //    failDto.CompetitiveGroupName = _processedDtoObjectStorage
        //        .FindCompetitiveGroupDto(cgTargetDto.ParentUID).Name;
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

        private void ApplicationConflict(PackageDataApplication applicationDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var failDto = new ApplicationFailDetailsDto();
			failDto.ApplicationNumber = applicationDto.ApplicationNumber;
			failDto.RegistrationDate = applicationDto.RegistrationDate.ToShortDateString();
            failDto.ErrorInfo = GetErrorInfo(applicationDto, conflictMessage);
			_appList.Add(failDto);
		}

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

        //private void InstitutionArchievementConflict(InstitutionAchievementDto dto, ConflictStorage.ConflictMessage conflictMessage)
        //{
        //    var failDto = new InstitutionAchievementFailDetailsDto();
        //    failDto.IAUID = dto.IAUID;
        //    failDto.Name = dto.Name;
        //    failDto.ErrorInfo = conflictMessage.GetErrorInfo(dto);
        //    _institutionAchievementList.Add(failDto);
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
    }
}
