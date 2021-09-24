using System.Collections.Generic;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.ParentDirections;
using GVUZ.DAL.Dto;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.Admission
{
    public interface IPlanAdmissionVolumeRepository
    {
        void CreatePlan(int campaignId);
        void SavePlan(int campaignId, IEnumerable<PlanAdmissionVolume> planAdmissionVolumes);
        void SavePlanDistribution(int campaignId, int levelId, int directionId, IEnumerable<DistributedPlanAdmissionVolumeSaveDto> distributedPlanAdmissionVolumeDtos);
        IEnumerable<int> GetCampaignEducationForms(int campaignId);
        IEnumerable<PlanAdmissionVolume> GetPlanAdmissionVolumeByCampaign(int campaignId);
        IEnumerable<DistributedPlanAdmissionVolume> GetDistributedPlanAdmissionVolumeByCampaign(int campaignId);
        IEnumerable<Direction> GetPlanAdmissionVolumeDirectionsByCampaign(int campaignId);
        IEnumerable<ParentDirection> GetPlanAdmissionVolumeDirectionGroupsByCampaign(int campaignId);
    }
}
