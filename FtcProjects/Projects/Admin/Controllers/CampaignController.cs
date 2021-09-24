using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Admin.DBContext;
using Admin.Models;
using Admin.Data;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Controllers
{
   [Authorize]
    public class CampaignController : Controller
    {

        private CampaignRepository repository;
        public CampaignController(CampaignRepository Repository)
        {
            repository = Repository;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        public IActionResult EditCampaign()
        {
            return PartialView();
        }

        public async Task<IActionResult> SearchInstitutions(string search)
        {
            ViewBag.Institution = await repository.SearchInstitutions(search);

            return PartialView("SearchInstitutionForYear");
        }

        public async Task<IActionResult> LoadCampaignYear(string Id)
        {
            int InstitutionId = (!String.IsNullOrEmpty(Id)) ? Convert.ToInt32(Id) : 0;
            ViewBag.Years = await repository.GetYears(InstitutionId);
            ViewBag.Id = Id;

            return PartialView();
        }

        public async Task<IActionResult> LoadCampaignList(string Year = null, string Id = null, int CampaignID = 0)
        {
            int InstitutionId = (!String.IsNullOrEmpty(Id)) ? Convert.ToInt32(Id) : 0;
            int year = (!String.IsNullOrEmpty(Year)) ? Convert.ToInt32(Year) : 0;

            List<CampaignViewModel> campaigns = await repository.GetCampaignList(InstitutionId, year, CampaignID);

            return PartialView(campaigns);
        }

        [HttpPost]
        public async Task<IActionResult> CampaignSaveChanges(List<CampaignViewModel> Campaign)
        {
            foreach (var c in Campaign)
            {
                var result = await repository.CampaignSaveChanges(c);

                if (result > 0)
                {
                    return Json(new { success = true, message = "Приемная кампания изменена" });
                }
                else {
                    return Json(new { success = true, message = "Ни одно поле не было изменено" });
                };
            }    

            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> DeleteCampaign(List<CampaignViewModel> Campaign)
        //{
        //    foreach (var c in Campaign)
        //    {
        //        //var result = await repository.DeleteCampaign(c);

        //        if (result > 0)
        //        {
        //            return Json(new { success = true, message = "Приемная кампания изменена" });
        //        }
        //        else
        //        {
        //            return Json(new { success = true, message = "Ни одно поле не было изменено" });
        //        };
        //    }

        //    return RedirectToAction("Index");
        //}


    }
}