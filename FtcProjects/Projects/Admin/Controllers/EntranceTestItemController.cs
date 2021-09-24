using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Data;
using Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [Authorize]
    public class EntranceTestItemController : Controller
    {
        private EntranceTestItemRepository repository;
        public EntranceTestItemController(EntranceTestItemRepository Repository)
        {
            repository = Repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetEntranceTestItems(int CompetitiveGroupId)
        {
            var entranceTestItems = await repository.SearchEntranceTestItems(CompetitiveGroupId);

            return PartialView("_ListEntranceTestItem", entranceTestItems);
        }

        public async Task<IActionResult> EditEntranceTestItems(List<EntranceTestItemsViewModel> modelList)
        {
            var result = 0;
            foreach (var model in modelList)
            {
                result = await repository.SaveEntranceTestItem(model);
                if (result > 0)
                {
                    return Json(new { success = true, message = "Минимальный балл изменен" });
                    //return PartialView("CompetitiveGroupList");
                }
               
            }

            return Json(new { success = true, notChangedMessage = "Изменения не приняты" });


        }
    }
}
