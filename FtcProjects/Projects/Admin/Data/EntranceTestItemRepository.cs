using Admin.DBContext;
using Admin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Data
{
    public class EntranceTestItemRepository : CommonRepository
    {
        public EntranceTestItemRepository(ApplicationContext context) : base(context)
        {

        }

        //Поиск ВИ по id КГ
        public async Task<List<EntranceTestItemsViewModel>> SearchEntranceTestItems(int competitiveGroupID)
        {
            var data = await (from e in Context.EntranceTestItemC
                                           from s in Context.Subject.Where(s => s.SubjectId == e.SubjectId)
                                           select new
                                           {
                                               e.EntranceTestItemId,
                                               e.CompetitiveGroupId,
                                               e.EntranceTestTypeId,
                                               e.ReplacedEntranceTestItemId,
                                               e.MinScore,
                                               e.EntranceTestPriority,
                                               Name = e.SubjectName ?? s.Name
                                           })
                                         .Where(e => e.CompetitiveGroupId == competitiveGroupID).ToListAsync();

            var entranceTestItems = new List<EntranceTestItemsViewModel>();

            foreach (var row in data)
            {
                entranceTestItems.Add(new EntranceTestItemsViewModel
                {
                    EntranceTestItemId = row.EntranceTestItemId,
                    CompetitiveGroupId = row.CompetitiveGroupId,
                    EntranceTestTypeId = row.EntranceTestTypeId,
                    EntranceTestPriority = row.EntranceTestPriority,
                    ReplacedEntranceTestItemId =row.ReplacedEntranceTestItemId,
                    MinScore = row.MinScore,
                    Name = row.Name

                });
   
            }

            return entranceTestItems;
        }

        //Изменение мин балла ВИ
        public async Task<int> SaveEntranceTestItem(EntranceTestItemsViewModel model)
        {
            var testItem = await Context.EntranceTestItemC.Where(e => e.EntranceTestItemId == model.EntranceTestItemId).FirstOrDefaultAsync();

            var result = 0;

            if (testItem != null)
            {
                testItem.MinScore = model.MinScore;
                result = await Context.SaveChangesAsync();
            }

            return result;
        }
    }
}
