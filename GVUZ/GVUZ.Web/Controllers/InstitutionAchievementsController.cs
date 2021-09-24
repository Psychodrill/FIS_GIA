using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GVUZ.Helper.MVC;
using GVUZ.Model.Entrants;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.ViewModels.InstitutionAchievements;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Institution")]
    [Authorize]
    public class InstitutionAchievementsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadRecords(int? campaignId, string sortKey, bool? sortDesc)
        {
            // UI пока без сортировки (sortKey, sortDesc не передаются)
            using (var db = new EntrantsEntities())
            {
                return Json(db.LoadInstitutionAchievementsForCampaign(InstitutionID, campaignId.GetValueOrDefault(0), sortKey, sortDesc.GetValueOrDefault()).ToArray());
            }
        }

        [HttpPost]
        public ActionResult LoadCampaignsAndCategories()
        {
            using (var db = new EntrantsEntities())
            {
                return Json(new {Campaigns = db.LoadAchievementCampaigns(InstitutionID).ToArray() , Categories = db.LoadAchievementCategories().ToArray()});    
            }
        }

        [HttpPost]
        public ActionResult UpdateRecord([ModelBinder(typeof(InstitutionAchievementModelBinder))]InstitutionAchievementUpdateRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new EntrantsEntities())
                {
                    if (db.ValidateInstitutionAchievementUpdateRecordViewModel(InstitutionID, model, ModelState))
                    {
                        return Json(new { success = true, record = db.UpdateInstitutionAchievementRecord(InstitutionID, model) });    
                    }
                }
            }

            return Json(new {success = false, errors = ModelState.ToKeyValueDictionary()});
        }

        [HttpPost]
        public ActionResult DeleteRecord(int id)
        {
            var errors = new List<string>();

            using (var db = new EntrantsEntities())
            {
                if (db.DeleteInstitutionAchievementRecord(InstitutionID, id, errors))
                {
                    return Json(new {success = true});
                }
            }

            return Json(new { success = false, errors});
        }
    }
}