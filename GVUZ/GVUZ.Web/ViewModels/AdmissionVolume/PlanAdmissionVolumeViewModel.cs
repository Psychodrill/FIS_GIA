using System.Collections.Generic;
using System.Linq;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Model.ParentDirections;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using GVUZ.DAL.Dapper.Repository.Interfaces.Dictionary;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GVUZ.DAL.Dto;

namespace GVUZ.Web.ViewModels.AdmissionVolume
{
    public class PlanAdmissionVolumeViewModel
    {
        public int CampaignId { get; private set; }

        public IEnumerable<AdmissionVolumeClassificatorItemViewModel> FinanceSources { get; private set; }

        public IEnumerable<AdmissionVolumeClassificatorItemViewModel> EducationForms { get; private set; }

        public IEnumerable<AdmissionVolumeClassificatorItemViewModel> AvailableEducationForms { get; private set; }

        public IEnumerable<AdmissionVolumeLevelViewModel> Levels { get; private set; }

        public int BudgetsCount { get; private set; }

        private IEnumerable<PlanAdmissionVolumeItemViewModel> _items;

        public PlanAdmissionVolumeItemViewModel GetDirectionItem(int financeSourceId, int educationFormId, int levelId, int directionId)
        {
            if (_items == null)
                return null;

            return _items.FirstOrDefault(x => (x.FinanceSourceId == financeSourceId) && (x.EducationFormId == educationFormId) && (x.LevelId == levelId) && (x.DirectionId == directionId));
        }

        public PlanAdmissionVolumeItemViewModel GetDirectionGroupItem(int financeSourceId, int educationFormId, int levelId, int directionGroupId)
        {
            if (_items == null)
                return null;

            return _items.FirstOrDefault(x => (x.FinanceSourceId == financeSourceId) && (x.EducationFormId == educationFormId) && (x.LevelId == levelId) && (x.DirectionGroupId == directionGroupId));
        }

        public void Load(int campaignId, IPlanAdmissionVolumeRepository dataRepository, IDictionaryRepository dictionaryRepository)
        {
            CampaignId = campaignId;

            BudgetsCount = dictionaryRepository.GetLevelBudget().Count();

            IEnumerable<int> campaignForms = dataRepository.GetCampaignEducationForms(CampaignId);

            IEnumerable<AdmissionItemTypeView> financeSources = dictionaryRepository.GetEducationFinanceSources();
            IEnumerable<AdmissionItemTypeView> educationForms = dictionaryRepository.GetEducationForms();

            FinanceSources = financeSources
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new AdmissionVolumeClassificatorItemViewModel() { Id = x.ID, Name = x.Name })
                .ToArray();
            EducationForms = educationForms
                //.Where(x => campaignForms.Any(y => y == x.ID))
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new AdmissionVolumeClassificatorItemViewModel() { Id = x.ID, Name = x.Name })
                .ToArray();
            AvailableEducationForms = EducationForms.Where(x => campaignForms.Any(y => y == x.Id));

            IEnumerable<PlanAdmissionVolume> pavs = dataRepository.GetPlanAdmissionVolumeByCampaign(CampaignId);
            IEnumerable<DistributedPlanAdmissionVolume> dpavs = dataRepository.GetDistributedPlanAdmissionVolumeByCampaign(CampaignId);

            IEnumerable<ParentDirection> directionGroups = dataRepository.GetPlanAdmissionVolumeDirectionGroupsByCampaign(CampaignId);
            IEnumerable<Direction> directions = dataRepository.GetPlanAdmissionVolumeDirectionsByCampaign(CampaignId);

            List<AdmissionVolumeLevelViewModel> levelViewModelsList = new List<AdmissionVolumeLevelViewModel>();
            Levels = levelViewModelsList;

            List<PlanAdmissionVolumeItemViewModel> directionGroupVolumeViewModelsList = new List<PlanAdmissionVolumeItemViewModel>();

            foreach (SimpleDto levelDto in dictionaryRepository.GetEducationLevelsList())
            {
                IEnumerable<Direction> directionsByLevel = directions
                    .Where(x => x.EducationLevelId == levelDto.Id);

                if (!directionsByLevel.Any())
                    continue;

                IEnumerable<ParentDirection> directionGroupsByLevel = directionGroups
                    .Where(x => directionsByLevel.Any(y => y.ParentID == x.ParentDirectionID));

                if (!directionGroupsByLevel.Any())
                    continue;

                AdmissionVolumeLevelViewModel levelViewModel = new AdmissionVolumeLevelViewModel();
                levelViewModelsList.Add(levelViewModel);

                levelViewModel.Id = levelDto.Id;
                levelViewModel.Name = levelDto.Name;

                List<AdmissionVolumeDirectionGroupViewModel> directionGroupViewModelsList = new List<AdmissionVolumeDirectionGroupViewModel>();
                foreach (ParentDirection directionGroup in directionGroupsByLevel)
                {
                    IEnumerable<Direction> directionsByLevelAndGroup = directionsByLevel
                        .Where(x => x.ParentID == directionGroup.ParentDirectionID);

                    AdmissionVolumeDirectionGroupViewModel directionGroupViewModel = new AdmissionVolumeDirectionGroupViewModel();
                    directionGroupViewModelsList.Add(directionGroupViewModel);

                    directionGroupViewModel.Id = directionGroup.ParentDirectionID;
                    directionGroupViewModel.Name = directionGroup.Name;
                    directionGroupViewModel.Code = directionGroup.Code;                   

                    List<AdmissionVolumeDirectionViewModel> directionViewModelsList = new List<AdmissionVolumeDirectionViewModel>();
                    foreach (Direction direction in directionsByLevelAndGroup)
                    {                       
                        IEnumerable <PlanAdmissionVolume> directionPavs = pavs
                            .Where(x => (x.AdmissionItemTypeID == levelDto.Id)
                                && (x.DirectionID == direction.DirectionID
                                || x.ParentDirectionID == direction.ParentID)
                                && (x.EducationSourceID != EDSourceConst.Paid));
                        IEnumerable<DistributedPlanAdmissionVolume> directionDpavs = dpavs
                            .Where(x => (directionPavs.Any(y => y.PlanAdmissionVolumeID == x.PlanAdmissionVolumeID)));

                        directionViewModelsList.Add(new AdmissionVolumeDirectionViewModel()
                        {
                            Id = direction.DirectionID,
                            Name = direction.Name,
                            Code = direction.NewCode,
                            AvailableForDistribution = directionPavs.Sum(x => x.Number),
                            TotalDistributed = directionDpavs.Sum(x => x.Number)
                        });

                        directionGroupViewModel.IsForUGS = pavs.Where(x => (x.AdmissionItemTypeID == levelDto.Id)
                                && (x.DirectionID == null
                                && x.ParentDirectionID == direction.ParentID)
                                && (x.EducationSourceID != EDSourceConst.Paid)).Count() > 0 ? true : false;

                        if (directionGroupViewModel.IsForUGS)
                        {
                            directionGroupViewModel.AvailableForDistribution = directionPavs.Sum(x => x.Number);
                            directionGroupViewModel.TotalDistributed = directionDpavs.Sum(x => x.Number);
                        }
                        
                    }
                    directionGroupViewModel.Directions = directionViewModelsList.OrderBy(x => x.Name);

                    foreach (AdmissionItemTypeView financeSource in financeSources)
                    {
                        foreach (AdmissionItemTypeView educationForm in educationForms)
                        {
                            PlanAdmissionVolumeItemViewModel directionGroupVolumeViewModel = new PlanAdmissionVolumeItemViewModel();
                            directionGroupVolumeViewModelsList.Add(directionGroupVolumeViewModel);

                            directionGroupVolumeViewModel.LevelId = levelDto.Id;
                            directionGroupVolumeViewModel.DirectionGroupId = directionGroup.ParentDirectionID;
                            directionGroupVolumeViewModel.FinanceSourceId = financeSource.ID;
                            directionGroupVolumeViewModel.EducationFormId = educationForm.ID;                
                            directionGroupVolumeViewModel.Number = pavs
                                .Where(x => (directionsByLevelAndGroup.Any(y => y.DirectionID == x.DirectionID || y.ParentID == x.ParentDirectionID)) 
                                && (x.EducationSourceID == financeSource.ID) 
                                && (x.EducationFormID == educationForm.ID))
                                .Sum(x => x.Number);
                        }
                    }
                }
                levelViewModel.DirectionGroups = directionGroupViewModelsList.OrderBy(x => x.Name);
            }

            _items = directionGroupVolumeViewModelsList
                .Union(pavs
                .Select(x => new PlanAdmissionVolumeItemViewModel()
                {
                    LevelId = x.AdmissionItemTypeID,
                    DirectionId = x.DirectionID,
                    EducationFormId = x.EducationFormID,
                    FinanceSourceId = x.EducationSourceID,
                    Number = x.Number
                }));
        }
    }
}