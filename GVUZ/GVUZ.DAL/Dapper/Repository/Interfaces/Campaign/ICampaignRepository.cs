using GDDVC = GVUZ.DAL.Dapper.ViewModel.Campaign;
using GDDMC = GVUZ.DAL.Dapper.Model.Campaigns;
using GDDMD = GVUZ.DAL.Dapper.ViewModel.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.Campaign
{
    public interface ICampaignRepository
    {
        IEnumerable<GDDMC.Campaign> GetCampaigns(int institutionId);
        //IEnumerable<GDDMC.CampaignStatus> GetCampaignStatus();
        //IEnumerable<GDDMC.CampaignTypes> GetCampaignTypes();
        IEnumerable<GDDMD.CampaignTypesView> GetEditCampaignTypes(int institutionId, int yearStart);
        IEnumerable<GDDMC.CampaignEducationLevel> GetCampaignEducationLevel(int campaignId);
        IEnumerable<GDDMD.AdmissionItemTypeView> GetAdmissionItemType();
        //IEnumerable<string> GetLevelsEducation(int campaignId);
        GDDVC.CampaignViewModel GetCampaignList(int institutionId);
        Task<IEnumerable<GDDVC.CampaignViewModel.CampaignDataModel>> GetCampaignListAsync(int institutionId);
        GDDVC.CampaignViewModel FillCampaignEditModel(int? campaignId, int? institutionId);
        bool ValidateUpdateCampaign(GDDVC.CampaignViewModel.CampaignEditModel model, ModelStateDictionary errors);
        int UpdateCampaign(GDDVC.CampaignViewModel.CampaignEditModel model);
        GDDVC.CampaignViewModel.CampaignDataModel SwitchCampaignStatus(int institutionId, int campaignId);
    }
}
