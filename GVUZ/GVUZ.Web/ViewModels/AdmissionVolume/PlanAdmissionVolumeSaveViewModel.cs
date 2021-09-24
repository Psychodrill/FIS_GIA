using System.Collections.Generic;
using System.Linq;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;

namespace GVUZ.Web.ViewModels.AdmissionVolume
{
    public class PlanAdmissionVolumeSaveViewModel
    {
        public int CampaignId { get; set; }

        public IEnumerable<PlanAdmissionVolumeItemViewModel> Items { get; set; }

        public void Save(IPlanAdmissionVolumeRepository dataRepository)
        {
            dataRepository.SavePlan(CampaignId, Items.Select(x => new PlanAdmissionVolume()
            {
                DirectionID = x.DirectionId,
                AdmissionItemTypeID = (short)x.LevelId,
                EducationFormID = (short)x.EducationFormId,
                EducationSourceID = (short)x.FinanceSourceId,
                Number = x.Number,
                ParentDirectionID = x.ParentDirectionId
            }));
        }
    }
}