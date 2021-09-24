using Admin.DBContext;
using Admin.Models;
using Admin.Models.DBContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Data
{
    public class CompetitiveGroupRepository : CommonRepository
    {
        public CompetitiveGroupRepository(ApplicationContext context) : base(context)
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

        public async Task<List<CompetitiveGropViewModel>> SearchCompetitiveGroup(int InstitutionId, string cgName, int CompetitiveGroupId = 0)
        {
            dynamic data;

            if (CompetitiveGroupId != 0)
            {
                  data = await (from cg in Context.CompetitiveGroup
                                  from c in Context.Campaign.Where(c => c.CampaignId == cg.CampaignId)
                                  select new
                                  {
                                      cg.Name,
                                      cg.InstitutionId,
                                      cg.CompetitiveGroupId,
                                      cg.CampaignId,
                                      cg.EducationFormId,
                                      cg.EducationLevelId,
                                      cg.DirectionId,
                                      cg.EducationSourceId,
                                      CampaignName = c.Name
                                  })
                              .Where(cg => cg.CompetitiveGroupId == CompetitiveGroupId)
                              .ToListAsync();
            }
            else {
                   data = await (from cg in Context.CompetitiveGroup
                                  from c in Context.Campaign.Where(c => c.CampaignId == cg.CampaignId)
                                  select new
                                  {
                                      cg.Name,
                                      cg.InstitutionId,
                                      cg.CompetitiveGroupId,
                                      cg.CampaignId,
                                      cg.EducationFormId,
                                      cg.EducationLevelId,
                                      cg.DirectionId,
                                      cg.EducationSourceId,
                                      CampaignName = c.Name
                                  })
                              .Where(cg => cg.Name.ToLower().Contains(cgName.ToLower()) && (cg.InstitutionId == InstitutionId))
                              .Take(20)
                              .ToListAsync();
            }


            var competitiveGroups = new List<CompetitiveGropViewModel>();

            var educationForms = await GetEducationFormsCg();
            var educationLevels = await GetEducationLevels();
            var directions = await GetDirections(InstitutionId);


            foreach (var row in data)
            {
                competitiveGroups.Add(new CompetitiveGropViewModel
                {
                    CampaignId = row.CampaignId,
                    Name = row.Name,
                    InstitutionId = row.InstitutionId,
                    CompetitiveGroupId = row.CompetitiveGroupId,
                    EducationFormId = row.EducationFormId,
                    EducationLevelId =row.EducationLevelId,
                    DirectionId =row.DirectionId,
                    EducationSourceId = row.EducationSourceId,
                    CampaignName = row.CampaignName

                });
            }

            //формирование SelectList
            foreach (var item in competitiveGroups)
            {
                item.EducationForms = new SelectList(educationForms, "ItemTypeId", "Name", item.EducationFormId);
                item.EducationLevels = new SelectList(educationLevels, "ItemTypeId", "Name", item.EducationLevelId);
                item.Directions = new SelectList(directions, "DirectionID", "Name", item.DirectionId);
            }


            return competitiveGroups;
        }

        //Сохранение изменений КГ
        public async Task<int> CompetitiveGroupSaveChanges(CompetitiveGropViewModel model)
        {
            var competitiveGroup = await Context.CompetitiveGroup.Where(c => c.CompetitiveGroupId == model.CompetitiveGroupId).FirstOrDefaultAsync();

            var result = 0;

            if (competitiveGroup != null)
            {
                competitiveGroup.EducationFormId = model.EducationFormId;
                competitiveGroup.EducationLevelId = model.EducationLevelId;
                competitiveGroup.DirectionId = model.DirectionId;

                result = await Context.SaveChangesAsync();
            }

            return result;
        }

        //Список форм образования КГ
        public async Task<List<EducationFormCg>> GetEducationFormsCg()
        {
            var data = await Context.AdmissionItemType.Where(p => p.ItemLevel == 7).ToListAsync();
            var educationForms = new List<EducationFormCg>();

            foreach (var row in data)
            {
                educationForms.Add(new EducationFormCg
                {
                    ItemTypeId = row.ItemTypeId,
                    Name = row.Name
                });
            }

            return educationForms;
        }

        //Список уровней образования КГ
        public async Task<List<EducationLevelCg>> GetEducationLevels()
        {
            var data = await Context.AdmissionItemType.Where(p => p.ItemLevel == 2).ToListAsync();
            var educationLevels = new List<EducationLevelCg>();

            foreach (var row in data)
            {
                educationLevels.Add(new EducationLevelCg
                {
                    ItemTypeId = row.ItemTypeId,
                    Name = row.Name
                });
            }

            return educationLevels;
        }

        //Список направлений из разрешенных для выбранного ОО
        public async Task<List<DirectionCg>> GetDirections(int InstitutionId)
        {
            var data = await ( from a in Context.AllowedDirections
                               from d in Context.Direction.Where( d => d.DirectionId == a.DirectionId)
                               select new 
                               { 
                                   a.InstitutionId,
                                   d.DirectionId,
                                   d.Name
                               })
                .Where(a => a.InstitutionId == InstitutionId).ToListAsync();

            //var data = await Context.Direction.ToListAsync();
            var directions = new List<DirectionCg>();

            foreach (var row in data)
            {
                directions.Add(new DirectionCg
                {
                    DirectionID = row.DirectionId,
                    Name = row.Name
                });
            }

            return directions;
        }


    }
}
