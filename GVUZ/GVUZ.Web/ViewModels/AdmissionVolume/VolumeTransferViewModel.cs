using System.Collections.Generic;
using System.Linq;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Model.ParentDirections;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using GVUZ.DAL.Dapper.Repository.Interfaces.Dictionary;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GDDMC = GVUZ.DAL.Dapper.Model.Campaigns;
using GVUZ.DAL.Dto;

namespace GVUZ.Web.ViewModels.AdmissionVolume
{
    public class VolumeTransferViewModel
    {
        public int CampaignId { get; set; }
        public bool AccessDenied { get; set; }
        public bool HasPlan { get; set; }
        public bool TransferBenefitCheck { get; set; }
        public GDDMC.Campaign Campaign { get; set; }
        public IEnumerable<TransferCheckDto> TransferVolumeCheck { get; set; }
        public IEnumerable<AdmissionVolumeClassificatorItemViewModel> FinanceSources { get; set; }
        public IEnumerable<AdmissionVolumeClassificatorItemViewModel> EducationForms { get; set; }


        public void Load(int campaignId, int InstitutionId, IVolumeTransferRepository volumeTransferRepository, IDictionaryRepository dictionaryRepository)
        {
            CampaignId = campaignId;
            Campaign = volumeTransferRepository.GetCampaign(InstitutionId, CampaignId);
            if (Campaign.InstitutionID != InstitutionId)
            {
                AccessDenied = true;
                Campaign = null;
                return;
            }
            HasPlan = volumeTransferRepository.CheckIfTransferAllowed(campaignId, Campaign.InstitutionID);
            TransferBenefitCheck = volumeTransferRepository.CheckIfTransferHasBenefit(campaignId);
            TransferVolumeCheck = volumeTransferRepository.CheckTransferVolume(campaignId);
            //справочники
            IEnumerable<int> campaignForms = volumeTransferRepository.GetCampaignEducationForms(CampaignId);
            IEnumerable<AdmissionItemTypeView> educationForms = dictionaryRepository.GetEducationForms();

            EducationForms = educationForms
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new AdmissionVolumeClassificatorItemViewModel() { Id = x.ID, Name = x.Name })
                .ToArray().Where(x => campaignForms.Any(y => y == x.Id));
        }
    }

}