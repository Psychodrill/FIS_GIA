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
    public class CompetitiveGroupController : Controller
    {

        private CompetitiveGroupRepository repository;
        public CompetitiveGroupController(CompetitiveGroupRepository Repository)
        {
            repository = Repository;
        }
        public IActionResult Index()
        {
            return PartialView();
        }

        public IActionResult EditCompetitiveGroup()
        {
            return PartialView();
        }

        public async Task<IActionResult> SearchInstitutions(string search)
        {
            ViewBag.Institution = await repository.SearchInstitutions(search);
            //var institution = await repository.SearchInstitutions(search);

            return PartialView("SearchInstitutions");
        }

        public async Task<IActionResult> CompetitiveGroupList(string InstitutionId, string cgName)
        {
            if (!String.IsNullOrEmpty(cgName))
            {
                var modelList = await repository.SearchCompetitiveGroup(int.Parse(InstitutionId), cgName);
                return PartialView(modelList);
            }
            else {
                return PartialView();
            }
        }

        public async Task<IActionResult> CompetitiveGroupSaveChanges(List<CompetitiveGropViewModel> modelList)
        {           
            var result = 0;

            foreach (var model in modelList)
            {
                result = await repository.CompetitiveGroupSaveChanges(model);

                if (result > 0)
                {
                    //var changed = await repository.SearchCompetitiveGroup(model.InstitutionId, null, model.CompetitiveGroupId);

                    return Json(new { success = true, message = "Конкурсная группа изменена" });
                } else { 
                    return Json(new { success = true, message = "Ни одно поле не было изменено" });
                };
            }

            return PartialView("_SaveError");
        }
    }
}