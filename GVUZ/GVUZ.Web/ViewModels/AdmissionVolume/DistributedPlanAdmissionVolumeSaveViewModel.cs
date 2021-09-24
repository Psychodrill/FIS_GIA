using System.Collections.Generic;
using System.Linq;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using GVUZ.DAL.Dto;

namespace GVUZ.Web.ViewModels.AdmissionVolume
{
    public class DistributedPlanAdmissionVolumeSaveViewModel
    {
        public int CampaignId { get; set; }
        public int LevelId { get; set; }
        public int DirectionId { get; set; }
        public int ParentDirectionId { get; set; }

        public IEnumerable<DistributedPlanAdmissionVolumeItemViewModel> Items { get; set; }

        public bool Validate(IPlanAdmissionVolumeRepository dataRepository, out string errorMessage, out IEnumerable<DistributedPlanAdmissionVolumeItemViewModel> errorItems)
        {
            errorMessage = null;
            List<DistributedPlanAdmissionVolumeItemViewModel> errorItemsList = new List<DistributedPlanAdmissionVolumeItemViewModel>();
            errorItems = errorItemsList;

            if ((Items == null) || (!Items.Any()))
                return true;

            IEnumerable<PlanAdmissionVolume> pavs = dataRepository.GetPlanAdmissionVolumeByCampaign(CampaignId)
                  .Where(x => (x.AdmissionItemTypeID == LevelId) && (x.DirectionID == DirectionId));

            pavs = pavs.Count() == 0 ? dataRepository.GetPlanAdmissionVolumeByCampaign(CampaignId)
               .Where(x => (x.AdmissionItemTypeID == LevelId) && (x.ParentDirectionID == ParentDirectionId)) : pavs;

            foreach (DistributedPlanAdmissionVolumeItemViewModel savingItem in Items)
            {
                PlanAdmissionVolume pav = pavs
                    .FirstOrDefault(x => (x.EducationFormID == savingItem.EducationFormId) && (x.EducationSourceID == savingItem.FinanceSourceId));
                if ((pav == null) || (pav.Number <= 0))
                {
                    errorItemsList.Add(savingItem);
                    continue;
                }

                IEnumerable<DistributedPlanAdmissionVolumeItemViewModel> savingItemsByPav = Items
                     .Where(x => (x.EducationFormId == pav.EducationFormID) && (x.FinanceSourceId == pav.EducationSourceID));

                if (savingItemsByPav.Sum(x => x.Number) > pav.Number)
                {
                    errorItemsList.Add(savingItem);
                }
            }
            bool success = !errorItems.Any();
            if (!success)
            {
                errorMessage = "Количество распределенных значений по всем уровням бюджета не должно превышать количество мест по соответствующей форме обучения и источнику финансирования";
            }
            return success;
        }

        public void Save(IPlanAdmissionVolumeRepository dataRepository)
        {
            if (Items == null)
                return;

            dataRepository.SavePlanDistribution(CampaignId, LevelId, DirectionId, Items.Select(x => new DistributedPlanAdmissionVolumeSaveDto()
            {
                EducationFormId = x.EducationFormId,
                FinanceSourceId = x.FinanceSourceId,
                BudgetId = x.BudgetId,
                Number = x.Number
            }));
        }
    }
}