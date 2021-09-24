using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;

namespace GVUZ.ServiceModel.Import.Core.Storages
{
	public class NotImportedHelper
	{
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

		private readonly DtoObjectStorage _processedDtoObjectStorage;
		private readonly DbObjectRepositoryBase _dbObjectRepo;

		private readonly Dictionary<BaseDto, ConflictStorage.ConflictMessage> _notImportedobjectWithErrorMessages = new Dictionary<BaseDto, ConflictStorage.ConflictMessage>();

		public NotImportedHelper(DbObjectRepositoryBase dbObjectRepo, DtoObjectStorage processedDtoObjectStorage, Dictionary<BaseDto, ConflictStorage.ConflictMessage> notImportedObjects)
		{
			_dbObjectRepo = dbObjectRepo;
			_processedDtoObjectStorage = processedDtoObjectStorage;
			_notImportedobjectWithErrorMessages = notImportedObjects;
			Mapper.CreateMap<CompetitiveGroupItemDto, CompetitiveGroupItemFailDetailsDto>();			
		}

		public CompetitiveGroupItemFailDetailsDto PrepareCompetitiveGroupItem(CompetitiveGroupItemDto cgItemDto)
		{
			return Mapper.Map(cgItemDto, new CompetitiveGroupItemFailDetailsDto());
		}

		#region Получение данных из справочников

		private string GetEducationLevelName(string educationLevelID)
		{
            var level = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.EducationLevel).ToArray()
                .SingleOrDefault(x => x.Key == educationLevelID.To(0));
            return level.Value ?? "Не корректный идентификатор уровня образования";
		}

		private string GetDirectionName(string directionID)
		{
			Direction direction = _dbObjectRepo.GetDirection(directionID.To(0));
			return direction == null ? "Направление отсутствует в справочнике" : direction.Name;
		}

		private string GetBenefitName(string benefitKindID)
		{
			Benefit benefit = _dbObjectRepo.GetBenefit(benefitKindID.To(0));
			return benefit == null ? "Вид льготы указан не корректно" : benefit.Name;
		}

		private string GetEntranceTestTypeName(string entTestTypeID)
		{
			EntranceTestType entranceTestType = _dbObjectRepo.GetEntranceTestType(entTestTypeID.To(0));
			return entranceTestType == null ? "Тип вступительного испытания указано не корректно" : entranceTestType.Name;
		}

		private string GetEntranceTestResultSourceName(string sourceID)
		{
			var resultSource = _dbObjectRepo.GetEnranceTestResultSource(sourceID.To(0));
			return resultSource == null ? "Источник вступительного испытания указано не корректно" : resultSource.Description;
		}

		private string GetSubject(EntranceTestSubjectDto subjectDto)
		{
			if (!String.IsNullOrWhiteSpace(subjectDto.SubjectName)) return subjectDto.SubjectName;
			if (!String.IsNullOrWhiteSpace(subjectDto.SubjectID))
			{
				Subject subject = _dbObjectRepo.GetSubject(subjectDto.SubjectID.To(0));
				if(subject != null)
					return subject.Name;
				return String.Format("Предмет для идентификатора {0} отсутствует", subjectDto.SubjectID);
			}
			return "Идентификатор предмета не указан";
		}

		#endregion

		#region Обработчики добавления конфликтов для dto объектов

		private bool GetDirection(string directionID, out string name, out string code)
		{
			name = code = null;
			Direction direction = _dbObjectRepo.GetDirection(directionID.To(0));
			if (direction == null) return false;

			code = direction.Code;
			name = direction.Name;
			return true;
		}

		private void AdmissionVolumeConflict(AdmissionVolumeDto aVolumeDto, ConflictStorage.ConflictMessage conflictMessage)
		{
            var admVolumeFailDto = new AdmissionVolumeFailDetailsDto();
			admVolumeFailDto.EducationLevelName = GetEducationLevelName(aVolumeDto.EducationLevelID);
			admVolumeFailDto.DirectionName = GetDirectionName(aVolumeDto.DirectionID);
            admVolumeFailDto.ErrorInfo = conflictMessage.GetErrorInfo(aVolumeDto);
			_admVolumeList.Add(admVolumeFailDto);
		}

        private void DistributedAdmissionVolumeConflict(DistributedAdmissionVolumeDto davDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var davFailDto = new DistributedAdmissionVolumeFailDetailsDto();
            davFailDto.AdmissionVolumeUID = davDto.AdmissionVolumeUID;
            davFailDto.LevelBudget = davDto.LevelBudget; // TODO Roman: GetLevelBudgetName( davDto.LevelBudget);
            davFailDto.ErrorInfo = //conflictMessage.GetErrorInfo(davDto);
                new ErrorInfoImportDto { 
                    ConflictItemsInfo = conflictMessage.ConflictItemsInfo, 
                    ErrorCode = conflictMessage.Code.ToString(),
				    Message = String.IsNullOrWhiteSpace(conflictMessage.Message) ? GVUZ.ServiceModel.Import.Core.Operations.Conflicts.ConflictMessages.GetMessage(conflictMessage.Code) : conflictMessage.Message
				};
            _distrAdmVolumeList.Add(davFailDto);
        }

		private void CampaignConflict(CampaignDto cDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var fDto = new CampaignDetailsFailDto();
			fDto.Name = cDto.Name;
            fDto.ErrorInfo = conflictMessage.GetErrorInfo(cDto);
			_campaignList.Add(fDto);
		}

		private void CampaignDateConflict(CampaignDateDto cDto, ConflictStorage.ConflictMessage conflictMessage)
		{
            var fDto = new CampaignDateFailDto();
            fDto.ErrorInfo = conflictMessage.GetErrorInfo(cDto);
			fDto.UID = cDto.UID;
			_campaignDateList.Add(fDto);
		}
		
		private void CompetitiveGroupConflict(CompetitiveGroupDto cgDto, ConflictStorage.ConflictMessage conflictMessage)
		{
            var cgFailDto = new CompetitiveGroupFailDetailsDto();
			cgFailDto.CompetitiveGroupName = cgDto.Name;
            cgFailDto.ErrorInfo = conflictMessage.GetErrorInfo(cgDto);
			_cgList.Add(cgFailDto);
		}
		
		private void CompetitiveGroupTargetConflict(CompetitiveGroupTargetDto cgTargetDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var failDto = new TargetOrganizationFailDetailsDto();
			failDto.TargetOrganizationName = cgTargetDto.TargetOrganizationName;
			//failDto.CompetitiveGroupName = _processedDtoObjectStorage
				//.FindCompetitiveGroupDto(cgTargetDto.ParentUID).Name;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(cgTargetDto);
			_targetOrgList.Add(failDto);
		}

		private void CompetitiveGroupItemConflict(CompetitiveGroupItemDto cgItemDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(cgItemDto.ParentUID);
			if (cgDto == null)
				LogHelper.Log.ErrorFormat(
					"Институт: {0}. Не найден родительский объект с UID: {1} для НКГ UID: {2}",
                    _dbObjectRepo.InstitutionId, cgItemDto.ParentUID, cgItemDto.UID);
			else
				cgItemDto.CompetitiveGroupName = cgDto.Name;
			string name, code;
			if (GetDirection(cgItemDto.DirectionID, out name, out code))
			{
				cgItemDto.DirectionCode = code;
				cgItemDto.DirectionName = name;
			}

			var cgItemFail = Mapper.Map(cgItemDto, new CompetitiveGroupItemFailDetailsDto());
            cgItemFail.ErrorInfo = conflictMessage.GetErrorInfo(cgItemDto);

			_cgItemList.Add(cgItemFail);
		}

		private void CompetitiveGroupTargetItemConflict(CompetitiveGroupTargetItemDto cgtItemDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var failInfo = new TargetOrganizationDirectionFailDetailsDto();

            //ParentUID - это TargetOrganization
            var groupTargetDto = _processedDtoObjectStorage.FindCompetitiveGroupTargetDto(cgtItemDto.ParentUID);
            var cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(cgtItemDto.CompetitiveGroupUID);

			string name, code;
			failInfo.TargetOrganizationName = groupTargetDto.TargetOrganizationName;
			failInfo.CompetitiveGroupName = cgDto.Name;
			if (GetDirection(cgtItemDto.DirectionID, out name, out code))
				failInfo.DirectionName = name;
			failInfo.EducationLevelName = GetEducationLevelName(cgtItemDto.EducationLevelID);
            failInfo.ErrorInfo = conflictMessage.GetErrorInfo(cgtItemDto);

			_cgtItemList.Add(failInfo);
		}

		private void BenefitItemConflict(BenefitItemDto benefitItemDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			if (benefitItemDto.IsCommonBenefit)
			{
				var failDto = new CommonBenefitFailDetailsDto();
				failDto.BenefitKindName = GetBenefitName(benefitItemDto.BenefitKindID);
				CompetitiveGroupDto cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(benefitItemDto.ParentUID);
				failDto.CompetitiveGroupName = cgDto.Name;
                failDto.ErrorInfo = conflictMessage.GetErrorInfo(benefitItemDto);
				_commonBenefitList.Add(failDto);
			}
			else
			{
				var failDto = new EntranceTestBenefitItemFailDetailsDto();
				failDto.BenefitKindName = GetBenefitName(benefitItemDto.BenefitKindID);
				var etItemDto = _processedDtoObjectStorage.FindEntranceTestItemDto(benefitItemDto.ParentUID);

				//пытаемся найти в завалившихся конфликтах
				if (etItemDto == null)
					etItemDto = _notImportedobjectWithErrorMessages.Keys.FirstOrDefault(x => x.UID == 
                        benefitItemDto.ParentUID && x.GetType() == typeof(EntranceTestItemDto)) as EntranceTestItemDto;

				var cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(etItemDto.ParentUID);
				failDto.CompetitiveGroupName = cgDto.Name;
				failDto.EntranceTestType = GetEntranceTestTypeName(etItemDto.EntranceTestTypeID);
				failDto.SubjectName = GetSubject(etItemDto.EntranceTestSubject);
                failDto.ErrorInfo = conflictMessage.GetErrorInfo(benefitItemDto);
				_etBenefitItemList.Add(failDto);
			}
		}

		private void EntranceTestConflict(EntranceTestItemDto etDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var failDto = new EntranceTestItemFailDetailsDto();
			var cgDto = _processedDtoObjectStorage.FindCompetitiveGroupDto(etDto.ParentUID);
			failDto.CompetitiveGroupName = cgDto.Name;
			failDto.EntranceTestType = GetEntranceTestTypeName(etDto.EntranceTestTypeID);
			failDto.SubjectName = GetSubject(etDto.EntranceTestSubject);
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(etDto);
			_etItemList.Add(failDto);
		}

		private void ApplicationConflict(ApplicationDto applicationDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var failDto = new ApplicationFailDetailsDto();
			failDto.ApplicationNumber = applicationDto.ApplicationNumber;
			failDto.RegistrationDate = applicationDto.RegistrationDateString;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(applicationDto);
			_appList.Add(failDto);
		}

        private void ConsideredApplicationConflict(ConsideredApplicationDto applicationDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new ConsideredApplicationFailDetailsDto();
            failDto.ConsideredApplication = applicationDto;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(applicationDto);
            _consideredAppList.Add(failDto);
        }

        private void RecommendedApplicationConflict(RecommendedApplicationDto applicationDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new RecommendedApplicationFailDetailsDto();
            failDto.RecommendedApplication = applicationDto;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(applicationDto);
            _recommendedAppList.Add(failDto);
        }

        private void InstitutionArchievementConflict(InstitutionAchievementDto dto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new InstitutionAchievementFailDetailsDto();
            failDto.IAUID = dto.IAUID;
            failDto.Name = dto.Name;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(dto);
            _institutionAchievementList.Add(failDto);
        }

		private void ApplicationEntranceTestConflict(EntranceTestAppItemDto etDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var failDto = new EntranceTestResultFailDetailsDto();
			var appDto = _processedDtoObjectStorage.FindApplicationDto(etDto.ParentUID);
			failDto.ResultSourceType = GetEntranceTestResultSourceName(etDto.ResultSourceTypeID);
			failDto.SubjectName = GetSubject(etDto.EntranceTestSubject);
			failDto.ResultValue = etDto.ResultValue;
			failDto.ApplicationNumber = appDto.ApplicationNumber;
			failDto.RegistrationDate = appDto.RegistrationDateString;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(etDto);
			_etResultItemList.Add(failDto);
		}

		private void ApplicationCommonBenefitConflict(ApplicationCommonBenefitDto appCommonBenefitDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var failDto = new ApplicationCommonBenefitFailDetailsDto();
			failDto.BenefitKindName = GetBenefitName(appCommonBenefitDto.BenefitKindID);
			ApplicationDto appDto = _processedDtoObjectStorage.FindApplicationDto(appCommonBenefitDto.ParentUID);
			failDto.ApplicationNumber = appDto.ApplicationNumber;
			failDto.RegistrationDate = appDto.RegistrationDateString;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(appCommonBenefitDto);
			_appCommonBenefitList.Add(failDto);
		}

		private void OrderOfAdmissionConflict(OrderOfAdmissionItemDto orderOfAdmissionItemDto, ConflictStorage.ConflictMessage conflictMessage)
		{
			var failDto = new ApplicationFailDetailsDto();
			failDto.ApplicationNumber = orderOfAdmissionItemDto.Application.ApplicationNumber;
			failDto.RegistrationDate = orderOfAdmissionItemDto.Application.RegistrationDateString;
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(orderOfAdmissionItemDto);
			_orderOfAdmList.Add(failDto);
		}

        private void RecommendedListDtoConflict(RecommendedListDto recListDto, ConflictStorage.ConflictMessage conflictMessage)
        {
            var failDto = new RecommendedListFailDetailsDto();
            failDto.Stage = recListDto.Stage.ToString();
            failDto.ErrorInfo = conflictMessage.GetErrorInfo(recListDto);
            _recommendedList.Add(failDto);
        }
        #endregion

		public void AddConflictInfo(BaseDto notImportedDto, ConflictStorage.ConflictMessage conflictMessage)
		{
            if (notImportedDto.GetType() == typeof(AdmissionVolumeDto))
                AdmissionVolumeConflict(notImportedDto as AdmissionVolumeDto, conflictMessage);
            if (notImportedDto.GetType() == typeof(DistributedAdmissionVolumeDto))
                DistributedAdmissionVolumeConflict(notImportedDto as DistributedAdmissionVolumeDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(CompetitiveGroupDto))
                CompetitiveGroupConflict(notImportedDto as CompetitiveGroupDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(CompetitiveGroupItemDto))
                CompetitiveGroupItemConflict(notImportedDto as CompetitiveGroupItemDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(CompetitiveGroupTargetDto))
                CompetitiveGroupTargetConflict(notImportedDto as CompetitiveGroupTargetDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(CompetitiveGroupTargetItemDto))
                CompetitiveGroupTargetItemConflict(notImportedDto as CompetitiveGroupTargetItemDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(EntranceTestItemDto))
                EntranceTestConflict(notImportedDto as EntranceTestItemDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(BenefitItemDto))
                BenefitItemConflict(notImportedDto as BenefitItemDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(EntranceTestResultDocumentsDto))
                BenefitItemConflict(notImportedDto as BenefitItemDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(ApplicationDto))
                ApplicationConflict(notImportedDto as ApplicationDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(ApplicationCommonBenefitDto))
                ApplicationCommonBenefitConflict(notImportedDto as ApplicationCommonBenefitDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(EntranceTestAppItemDto))
                ApplicationEntranceTestConflict(notImportedDto as EntranceTestAppItemDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(OrderOfAdmissionItemDto))
                OrderOfAdmissionConflict(notImportedDto as OrderOfAdmissionItemDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(CampaignDto))
                CampaignConflict(notImportedDto as CampaignDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(CampaignDateDto))
                CampaignDateConflict(notImportedDto as CampaignDateDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(ConsideredApplicationDto))
                ConsideredApplicationConflict(notImportedDto as ConsideredApplicationDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(RecommendedApplicationDto))
                RecommendedApplicationConflict(notImportedDto as RecommendedApplicationDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(RecommendedListDto))
                RecommendedListDtoConflict(notImportedDto as RecommendedListDto, conflictMessage);
            else if (notImportedDto.GetType() == typeof(InstitutionAchievementDto))
                InstitutionArchievementConflict(notImportedDto as InstitutionAchievementDto, conflictMessage);
		}


		public FailedImportInfoDto GetFailedImportInfoDto()
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
								//OrdersOfAdmissions = (_orderList.Count==0) ? null : _orderOfAdmList.ToArray(),
								TargetOrganizations = (_targetOrgList.Count == 0) ? null : _targetOrgList.ToArray(),
								ApplicationCommonBenefits = (_appCommonBenefitList.Count == 0) ? null : _appCommonBenefitList.ToArray(),
								EntranceTestResults = (_etResultItemList.Count == 0) ? null : _etResultItemList.ToArray(),
								Campaigns = (_campaignList.Count == 0) ? null : _campaignList.ToArray(),
								CampaignDates = (_campaignDateList.Count == 0) ? null : _campaignDateList.ToArray(),
                                RecommendedLists = (_recommendedList.Count == 0) ? null : _recommendedList.ToArray(),
                                InstitutionAchievements = (_institutionAchievementList.Count == 0) ? null : _institutionAchievementList.ToArray()
			       	};
		}
	}
}
