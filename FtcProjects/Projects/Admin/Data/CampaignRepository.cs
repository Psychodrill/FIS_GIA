using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Admin.DBContext;
using Admin.Models;
using Admin.Models.DBContext;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Admin.Data
{
    public class CampaignRepository : CommonRepository

    {
        public CampaignRepository(ApplicationContext context) : base(context)
        {

        }
        public async Task<List<InstitutionDetail>> SearchInstitutions(string search)
        {
            var result = await Context.Institution
                    .Where(p => p.BriefName.ToLower().Contains(search.ToLower()) || p.FullName.ToLower().Contains(search.ToLower()))
                    .OrderBy(p => p.FullName)
                    .Take(20).ToListAsync();

            var institutions = new List<InstitutionDetail>();

            if (result != null)
            {
                foreach (var row in result)
                {
                    institutions.Add(new InstitutionDetail
                    {
                        FullName = row.FullName,
                        InstitutionId = row.InstitutionId,
                        BriefName = row.BriefName
                    });
                }
            }

            return institutions;
        }

        //Список приемной кампании по году и id ОО
        public async Task<List<CampaignViewModel>> GetCampaignList(int institution_id, int year, int CampaignID)
        {
            var campaignTypes = await GetCampaignTypes();
            var educationForms = await GetEducationForms();
            var campaignStatuses = await GetCampaignStatus();
            var years = await GetYears(institution_id);
            var data = new List<Campaign>();

            if (CampaignID != 0)
            {
                data = await Context.Campaign
                .Where(d => d.CampaignId == CampaignID).ToListAsync();
            }
            else
            {
                data = await Context.Campaign
                .Where(d => d.InstitutionId == institution_id && d.YearStart == year).ToListAsync();
                //.Where(d => d.InstitutionId == institution_id && d.StatusId < 2 && d.YearStart == year).ToListAsync();
            }

            var campaigns = new List<CampaignViewModel>();

            foreach (var row in data)
            {
                campaigns.Add(new CampaignViewModel
                {
                    CampaignId = row.CampaignId,
                    Name = row.Name,
                    YearStart = row.YearStart,
                    YearEnd = row.YearEnd,
                    CampaignTypeId = row.CampaignTypeId,
                    EducationFormFlag = row.EducationFormFlag,
                    StatusId = row.StatusId

                });
            }

            //формирование SelectList
            foreach (var item in campaigns)
            {
                item.CampaignTypes = new SelectList(campaignTypes, "CampaignTypeId", "Name", item.CampaignTypeId);
                item.EducationForms = new SelectList(educationForms, "Id", "Name", item.EducationFormFlag);
                item.CampaignStatus = new SelectList(campaignStatuses, "StatusID", "Name", item.StatusId);
                item.YearsStart = new SelectList(years, item.YearStart);
                item.YearsEnd = new SelectList(years, item.YearEnd);
            }

            return campaigns;
        }

        //Типы ПК
        public async Task<IEnumerable<CampaignType>> GetCampaignTypes()
        {
            var data = await Context.CampaignTypes.ToListAsync();

            var campaignType = new List<CampaignType>();

            foreach (var row in data)
            {
                campaignType.Add(new CampaignType
                {
                    CampaignTypeId = row.CampaignTypeId,
                    Name = row.Name
                });
            }

            return campaignType;
        }

        //Формы образования для ПК
        public async Task<IEnumerable<EducationForm>> GetEducationForms()
        {
            var data = await Context.EducationForms.ToListAsync();

            var educationForm = new List<EducationForm>();

            foreach (var row in data)
            {
                educationForm.Add(new EducationForm
                {
                    Id = (int)row.Id,
                    Name = row.Name
                });
            }

            return educationForm;
        }

        //Статус ПК
        public async Task<IEnumerable<CampaignStatuses>> GetCampaignStatus()
        {
            var data = await Context.CampaignStatus.ToListAsync();

            var campaignStatus = new List<CampaignStatuses>();

            foreach (var row in data)
            {
                campaignStatus.Add(new CampaignStatuses
                {
                    StatusID = row.StatusId,
                    Name = row.Name
                });
            }

            return campaignStatus;
        }

        //Список годов кампаний
        public async Task<List<int>> GetYears(int institution_id)
        {
            List<int> list = new List<int>();

            var years = await Context.Campaign
                        .Where(p => p.InstitutionId == institution_id)
                        //.Where(p => p.InstitutionId == institution_id && p.StatusId < 2)
                        .Select(p => new
                        {
                            p.YearStart,
                            //p.YearEnd
                        })
                        .Distinct()
                        .OrderBy(p => p.YearStart)
                        .ToListAsync();

            if (years != null)
            {
                foreach (var row in years)
                {
                    list.Add(row.YearStart);
                    //list.Add(row.YearEnd.ToString());
                }
            }

            return list;
        }

        // Изменение и сохранение полей ПК
        public async Task<int> CampaignSaveChanges(CampaignViewModel model)
        {
            var campaign = await Context.Campaign.Where(c => c.CampaignId == model.CampaignId).FirstOrDefaultAsync();

            var result = 0;

            if (campaign != null)
            {
                campaign.EducationFormFlag = model.EducationFormFlag;
                campaign.StatusId = model.StatusId;
                campaign.CampaignTypeId = (short)model.CampaignTypeId;
                campaign.YearStart = model.YearStart;
                campaign.YearEnd = model.YearEnd;
                result = await Context.SaveChangesAsync();
            }

            return result;
        }

        public async Task DeleteCampaign(CampaignViewModel model)
        {
            var campaign = await Context.Campaign.Where(c => c.CampaignId == model.CampaignId).FirstOrDefaultAsync();
            if (campaign != null)
            {
                Context.Remove(campaign);
                await Context.SaveChangesAsync();
            }
            
        }

    }
}
