using System.Collections.Generic;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.ParentDirections;
using GVUZ.DAL.Dto;
using GDDMC = GVUZ.DAL.Dapper.Model.Campaigns;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.Admission
{
    public interface IVolumeTransferRepository
    {
        GDDMC.Campaign GetCampaign(int institutionId, int campaignId);
        bool CheckIfTransferAllowed(int campaignId, int institutionId);
        bool CheckIfTransferHasBenefit(int campaignId);
        IEnumerable<TransferCheckDto> CheckTransferVolume(int campaignId);
        dynamic VolumeTransferByCampaign(int campaignId, int institutionId);
        dynamic BeginVolumeTransfer(int[] competitiveGroupIDs, int campaignId, int institutionID);
        IEnumerable<int> GetCampaignEducationForms(int campaignId);

    }
}
