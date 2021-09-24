using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Storages
{
	public class DtoObjectStorage
	{
		private readonly Dictionary<string, AdmissionVolumeDto> _admissionVolume = new Dictionary<string, AdmissionVolumeDto>();
        private readonly Dictionary<string, DistributedAdmissionVolumeDto> _distributedAdmissionVolume = new Dictionary<string, DistributedAdmissionVolumeDto>();
		private readonly Dictionary<string, CampaignDto> _campaign = new Dictionary<string, CampaignDto>();
		private readonly Dictionary<string, CompetitiveGroupDto> _competitiveGroup = new Dictionary<string, CompetitiveGroupDto>();
		private readonly Dictionary<string, CompetitiveGroupItemDto> _competitiveGroupItem = new Dictionary<string, CompetitiveGroupItemDto>();
		private readonly Dictionary<string, CompetitiveGroupTargetDto> _competitiveGroupTarget = new Dictionary<string, CompetitiveGroupTargetDto>();
		private readonly Dictionary<string, CompetitiveGroupTargetItemDto> _competitiveGroupTargetItem = new Dictionary<string, CompetitiveGroupTargetItemDto>();
		private readonly Dictionary<string, BenefitItemDto> _competitiveGroupBenefitItem = new Dictionary<string, BenefitItemDto>();
		private readonly Dictionary<string, EntranceTestItemDto> _competitiveGroupEntranceTestItem = new Dictionary<string, EntranceTestItemDto>();
		private readonly Dictionary<string, BenefitItemDto> _competitiveGroupEntranceTestBenefitItem = new Dictionary<string, BenefitItemDto>();
        private readonly Dictionary<string, RecommendedListDto> _recommendedListItem = new Dictionary<string, RecommendedListDto>();
        private readonly Dictionary<string, InstitutionAchievementDto> _institutionArchievements = new Dictionary<string, InstitutionAchievementDto>();

        public List<AdmissionVolume> packageAdmissionVolumes = new List<AdmissionVolume>();

		public List<AdmissionVolumeDto> AdmissionVolume
		{
			get { return _admissionVolume.Values.ToList(); }
		}

        public List<DistributedAdmissionVolumeDto> DistributedAdmissionVolumeDto
        {
            get { return _distributedAdmissionVolume.Values.ToList(); }
        }

		public List<CampaignDto> Campaign
		{
			get { return _campaign.Values.ToList(); }
		}

		public List<CompetitiveGroupDto> CompetitiveGroup
		{
			get { return _competitiveGroup.Values.ToList(); }
		}

		public List<CompetitiveGroupItemDto> CompetitiveGroupItem
		{
			get { return _competitiveGroupItem.Values.ToList(); }
		}

		public List<CompetitiveGroupTargetDto> CompetitiveGroupTarget
		{
			get { return _competitiveGroupTarget.Values.ToList(); }
		}

		public List<CompetitiveGroupTargetItemDto> CompetitiveGroupTargetItem
		{
			get { return _competitiveGroupTargetItem.Values.ToList(); }
		}

		public List<BenefitItemDto> CompetitiveGroupBenefitItem
		{
			get { return _competitiveGroupBenefitItem.Values.ToList(); }
		}

		public List<EntranceTestItemDto> CompetitiveGroupEntranceTestItem
		{
			get { return _competitiveGroupEntranceTestItem.Values.ToList(); }
		}

		public List<BenefitItemDto> CompetitiveGroupEntranceTestBenefitItem
		{
			get { return _competitiveGroupEntranceTestBenefitItem.Values.ToList(); }
		}

        public List<ConsideredApplicationDto> ConsideredApplications
        {
            get { return _consideredApplications; }
            private set { }
        }

        public List<RecommendedApplicationDto> RecommendedApplications
        {
            get { return _recommendedApplications; }
            private set { }
        }

        public List<RecommendedListDto> RecommendedLists
        {
            get { return _recommendedListItem.Values.ToList(); }
        }

        public List<InstitutionAchievementDto> InstitutionAchievements
        {
            get { return _institutionArchievements.Values.ToList(); }
        }

		#region Заявления

		private readonly Dictionary<string, ApplicationDto> _application = new Dictionary<string, ApplicationDto>();
        private readonly List<ConsideredApplicationDto> _consideredApplications = new List<ConsideredApplicationDto>();
        private readonly List<RecommendedApplicationDto> _recommendedApplications = new List<RecommendedApplicationDto>();

		private readonly Dictionary<string, EntranceTestAppItemDto> _entranceTestAppItemDtos = new Dictionary<string, EntranceTestAppItemDto>();
		private readonly Dictionary<string, ApplicationCommonBenefitDto> _applicationCommonBenefitDtos = new Dictionary<string, ApplicationCommonBenefitDto>();		

        List<ApplicationDto> _apps;
        public List<ApplicationDto> Application
		{
			get
			{
                if (_apps == null)
                    _apps = _application.Values.ToList();
			    return _apps;
			}
		}

		public List<EntranceTestAppItemDto> EntranceTestAppItemDtos
		{
			get { return _entranceTestAppItemDtos.Values.ToList(); }
		}

		#endregion

		#region Приказы

		//public List<ApplicationDto> OrdersOfAd = new List<ApplicationDto>();

		#endregion

		#region Добавление объектов

		public void AddAdmissionVolume(AdmissionVolumeDto avDto)
		{
			if (_admissionVolume.ContainsKey(avDto.UID)) return;
			_admissionVolume.Add(avDto.UID, avDto);
		}
        public void AddDistributedAdmissionVolume(DistributedAdmissionVolumeDto davDto)
        {
            if (_distributedAdmissionVolume.ContainsKey(davDto.Key())) return;
            _distributedAdmissionVolume.Add(davDto.Key(), davDto);
        }

		public void AddCampaign(CampaignDto cDto)
		{
			if (_campaign.ContainsKey(cDto.UID)) return;
			_campaign.Add(cDto.UID, cDto);
		}

		public void AddCompetitiveGroup(CompetitiveGroupDto competitiveGroupDto)
		{
			if (_competitiveGroup.ContainsKey(competitiveGroupDto.UID)) return;
			_competitiveGroup.Add(competitiveGroupDto.UID, competitiveGroupDto);
		}

		public void AddCompetitiveGroupItem(CompetitiveGroupItemDto competitiveGroupItemDto)
		{
			if (_competitiveGroupItem.ContainsKey(competitiveGroupItemDto.UID)) return;
			_competitiveGroupItem.Add(competitiveGroupItemDto.UID, competitiveGroupItemDto);
		}

		public void AddCompetitiveGroupTarget(CompetitiveGroupTargetDto competitiveGroupTargetDto)
		{
			if (_competitiveGroupTarget.ContainsKey(competitiveGroupTargetDto.UID)) return;
			_competitiveGroupTarget.Add(competitiveGroupTargetDto.UID, competitiveGroupTargetDto);
		}

		public void AddCompetitiveGroupTargetItem(CompetitiveGroupTargetItemDto competitiveGroupTargetItemDto)
		{
			if (_competitiveGroupTargetItem.ContainsKey(competitiveGroupTargetItemDto.UID)) return;
			_competitiveGroupTargetItem.Add(competitiveGroupTargetItemDto.UID, competitiveGroupTargetItemDto);
		}

		public void AddCompetitiveGroupBenefitItem(BenefitItemDto cgBenefitItemDto)
		{
			if (_competitiveGroupBenefitItem.ContainsKey(cgBenefitItemDto.UID)) return;
			_competitiveGroupBenefitItem.Add(cgBenefitItemDto.UID, cgBenefitItemDto);
		}

		public void AddCompetitiveGroupEntranceTestBenefitItem(BenefitItemDto cgBenefitItemDto)
		{
			if (_competitiveGroupEntranceTestBenefitItem.ContainsKey(cgBenefitItemDto.UID)) return;
			_competitiveGroupEntranceTestBenefitItem.Add(cgBenefitItemDto.UID, cgBenefitItemDto);
		}

		public void AddCompetitiveGroupEntranceTestItem(EntranceTestItemDto entranceTestItemDto)
		{
			if (_competitiveGroupEntranceTestItem.ContainsKey(entranceTestItemDto.UID)) return;
			_competitiveGroupEntranceTestItem.Add(entranceTestItemDto.UID, entranceTestItemDto);
		}

		public void AddApplication(ApplicationDto applicationDto)
		{
			if (_application.ContainsKey(applicationDto.UID)) return;
			_application.Add(applicationDto.UID, applicationDto);
		}

        public void AddConsideredApplication(ConsideredApplicationDto dto)
        {
            _consideredApplications.Add(dto);
        }

        public void AddRecommendedApplication(RecommendedApplicationDto dto)
        {
            _recommendedApplications.Add(dto);
        }

		public void AddApplicationEntranceTestItem(EntranceTestAppItemDto entranceTestAppItemDto)
		{
			if (_application.ContainsKey(entranceTestAppItemDto.UID)) return;
			_entranceTestAppItemDtos.Add(entranceTestAppItemDto.UID, entranceTestAppItemDto);
		}

		public void AddApplicationCommonBenefit(ApplicationCommonBenefitDto applicationCommonBenefitDto)
		{
			if (_application.ContainsKey(applicationCommonBenefitDto.UID)) return;
			_applicationCommonBenefitDtos.Add(applicationCommonBenefitDto.UID, applicationCommonBenefitDto);
		}

        public void AddRecommendedList(RecommendedListDto recommendedListDto)
        {
            if (_recommendedListItem.ContainsKey(recommendedListDto.UID)) return;
            _recommendedListItem.Add(recommendedListDto.UID, recommendedListDto);
        }

        public void AddInstitutionAchievement(InstitutionAchievementDto dto)
        {
            if (_institutionArchievements.ContainsKey(dto.IAUID)) return;
            _institutionArchievements.Add(dto.IAUID, dto);
        }
		#endregion

		#region Поиск объектов

		public CompetitiveGroupDto FindCompetitiveGroupDto(string uid)
		{
			return _competitiveGroup.ContainsKey(uid) ? _competitiveGroup[uid] : null;
		}

		public CompetitiveGroupItemDto FindCompetitiveGroupItemDto(string uid)
		{
			return _competitiveGroupItem.ContainsKey(uid) ? _competitiveGroupItem[uid] : null;
		}

		public CompetitiveGroupTargetDto FindCompetitiveGroupTargetDto(string uid)
		{			
			return _competitiveGroupTarget.ContainsKey(uid) ? _competitiveGroupTarget[uid] : null;
		}

		public EntranceTestItemDto FindEntranceTestItemDto(string uid)
		{
			return _competitiveGroupEntranceTestItem.ContainsKey(uid) ? _competitiveGroupEntranceTestItem[uid] : null;
		}

		public ApplicationDto FindApplicationDto(string uid)
		{
			return _application.ContainsKey(uid) ? _application[uid] : null;
		}

		#endregion 

	}
}
