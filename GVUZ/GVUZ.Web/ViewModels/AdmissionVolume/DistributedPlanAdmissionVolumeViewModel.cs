using System.Collections.Generic;
using System.Linq;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Model.LevelBudgets;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using GVUZ.DAL.Dapper.Repository.Interfaces.Dictionary;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;

namespace GVUZ.Web.ViewModels.AdmissionVolume
{
    public class DistributedPlanAdmissionVolumeViewModel
    {
        public int CampaignId { get; private set; }
        public int LevelId { get; private set; }
        public int DirectionId { get; private set; }
        public int ParentDirectionId { get; private set; }

        public IEnumerable<AdmissionVolumeClassificatorItemViewModel> FinanceSources { get; private set; }

        public IEnumerable<AdmissionVolumeClassificatorItemViewModel> EducationForms { get; private set; }
   
        //public IEnumerable<AdmissionVolumeClassificatorItemViewModel> AvailableEducationForms { get; private set; }

        public IEnumerable<AdmissionVolumeClassificatorItemViewModel> Budgets { get; private set; }

        private IEnumerable<PlanAdmissionVolumeItemViewModel> _items;
        private IEnumerable<DistributedPlanAdmissionVolumeItemViewModel> _distributionItems;

        public DistributedPlanAdmissionVolumeItemViewModel GetItem(int budgetId, int financeSourceId, int educationFormId)
        {
            if (_distributionItems == null)
                return null;

            return _distributionItems.FirstOrDefault(x => (x.BudgetId == budgetId) && (x.FinanceSourceId == financeSourceId) && (x.EducationFormId == educationFormId));
        }

        public bool CanDistributeItem(int financeSourceId, int educationFormId)
        {
            if (_items == null)
                return false;

            return (_items
                .Where(x => (x.FinanceSourceId == financeSourceId) && (x.EducationFormId == educationFormId))
                .Sum(x => x.Number) > 0);
        }

        public int GetTotalDistributed(int budgetId)
        {
            if (_distributionItems == null)
                return 0;

            return _distributionItems.Where(x => x.BudgetId == budgetId).Sum(x => x.Number);
        }

        public void Load(int campaignId, int levelId, int directionId, int directionGroupId, 
            IPlanAdmissionVolumeRepository dataRepository,IDictionaryRepository dictionaryRepository)
        {
            CampaignId = campaignId;
            LevelId = levelId;
            DirectionId = directionId;
            ParentDirectionId = directionGroupId;

            IEnumerable<int> campaignForms = dataRepository.GetCampaignEducationForms(CampaignId);

            IEnumerable<AdmissionItemTypeView> financeSources = dictionaryRepository.GetEducationFinanceSources();
            IEnumerable<AdmissionItemTypeView> educationForms = dictionaryRepository.GetEducationForms();
            IEnumerable <LevelBudget> budgets = dictionaryRepository.GetLevelBudget();

            FinanceSources = financeSources
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new AdmissionVolumeClassificatorItemViewModel() { Id = x.ID, Name = x.Name })
                .ToArray();
            EducationForms = educationForms
                //.Where(x => campaignForms.Any(y => y == x.ID))
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new AdmissionVolumeClassificatorItemViewModel() { Id = x.ID, Name = x.Name })
                .ToArray();
            //AvailableEducationForms = EducationForms.Where(x => campaignForms.Any(y => y == x.Id));
            Budgets = budgets
                .OrderBy(x => x.IdLevelBudget)
                .Select(x => new AdmissionVolumeClassificatorItemViewModel() { Id = x.IdLevelBudget, Name = x.BudgetName })
                .ToArray();

            IEnumerable<PlanAdmissionVolume> pavs = dataRepository.GetPlanAdmissionVolumeByCampaign(CampaignId)
                .Where(x => (x.AdmissionItemTypeID == LevelId) && (x.DirectionID == DirectionId));

            pavs = pavs.Count() == 0 ? dataRepository.GetPlanAdmissionVolumeByCampaign(CampaignId)
                .Where(x => (x.AdmissionItemTypeID == LevelId) && (x.ParentDirectionID == ParentDirectionId)) : pavs;

            IEnumerable<DistributedPlanAdmissionVolume> dpavs = dataRepository.GetDistributedPlanAdmissionVolumeByCampaign(CampaignId);
                

            List<PlanAdmissionVolumeItemViewModel> itemsList = new List<PlanAdmissionVolumeItemViewModel>();
            List<DistributedPlanAdmissionVolumeItemViewModel> distributionItemsList = new List<DistributedPlanAdmissionVolumeItemViewModel>();
            foreach (PlanAdmissionVolume pav in pavs)
            {
                itemsList.Add(new PlanAdmissionVolumeItemViewModel()
                {
                    DirectionId = pav.DirectionID,
                    EducationFormId = pav.EducationFormID,
                    FinanceSourceId = pav.EducationSourceID,
                    LevelId = pav.AdmissionItemTypeID,
                    Number = pav.Number,
                    ParentDirectionId = pav.ParentDirectionID,
                });

                distributionItemsList.AddRange(dpavs
                    .Where(x => x.PlanAdmissionVolumeID == pav.PlanAdmissionVolumeID)
                    .Select(x => new DistributedPlanAdmissionVolumeItemViewModel()
                    {
                        EducationFormId = pav.EducationFormID,
                        FinanceSourceId = pav.EducationSourceID,
                        BudgetId = x.IdLevelBudget,
                        Number = x.Number
                    }));
            }
            _items = itemsList;
            _distributionItems = distributionItemsList;
        }
    }
}