using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Web.Security;
using GDDVMC = GVUZ.DAL.Dapper.ViewModel.Campaign;
using GVUZ.DAL.Dapper.Repository.Interfaces.Campaign;
using GVUZ.DAL.Dapper.Repository.Model.Campaigns;
using GVUZ.Model.Entrants;
using System.Collections.Generic;
using System.Linq;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Administration")]
	public class CampaignController : BaseController
	{
        ICampaignRepository campaignRepository;
        public CampaignController()
        {
            this.campaignRepository = new CampaignRepository();
        }
        public CampaignController(ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
        }
        [Authorize]
        public ActionResult CampaignList()
        {
            return View("CampaignList", new GDDVMC.CampaignViewModel());
        }
        [HttpPost]
        [Authorize]
        public ActionResult GetCampaignList(GDDVMC.CampaignViewModel model)
        {
            model = campaignRepository.GetCampaignList(InstitutionID);
            return new AjaxResultModel { Data = model };
        }
        [Authorize]
        public ActionResult CampaignEdit(int? campaignID)
        {
            var model = campaignRepository.FillCampaignEditModel(campaignID, InstitutionID);
            model.CampaignEdit.CanEdit = model.CampaignEdit.CanEdit & !UserRole.IsCurrentUserReadonly();
            return View("CampaignEdit", model);
        }
        [HttpPost]
        [Authorize]
        public ActionResult CampaignUpdate(GDDVMC.CampaignViewModel.CampaignEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.InstitutionID = InstitutionID;
                if (campaignRepository.ValidateUpdateCampaign(model, ModelState))
                {
                    return new AjaxResultModel { Data = campaignRepository.UpdateCampaign(model) };
                }
            }
            return new AjaxResultModel(ModelState);
        }
        [HttpPost]
        [Authorize]
        public ActionResult GetEditCampaignTypes(int yearStart)
        {
            return new AjaxResultModel { Data = campaignRepository.GetEditCampaignTypes(InstitutionID, yearStart) };
        }

        //      [HttpPost]
        //[Authorize]		
        //public ActionResult CampaignSave(CampaignEditViewModel model)
        //{
        //	if (!ModelState.IsValid)
        //		return new AjaxResultModel(ModelState);
        //	using (EntrantsEntities dbContext = new EntrantsEntities())
        //	{
        //		return dbContext.SaveCampaignEditModel(model, InstitutionID);
        //	}
        //}
        //[Authorize]
        //public ActionResult CampaignDateEdit(int? campaignID)
        //{
        //	if (!campaignID.HasValue)
        //		return new EmptyResult();
        //	using (EntrantsEntities dbContext = new EntrantsEntities())
        //	{
        //		var model = dbContext.FillCampaignDateEditModel(campaignID.Value, InstitutionID);
        //		model.CanEdit = model.CanEdit & !UserRole.IsCurrentUserReadonly();
        //		return View("CampaignDateEdit", model);
        //	}
        //}

        //[HttpPost]
        //      [Authorize]		
        //public ActionResult CampaignDateSave(CampaignDateEditViewModel model)
        //{
        //	//ошибки проверяем дальше
        //	//if (!ModelState.IsValid)
        //	//	return new AjaxResultModel(ModelState);
        //	using (EntrantsEntities dbContext = new EntrantsEntities())
        //	{
        //		return dbContext.SaveCampaignDateEditModel(model, InstitutionID);
        //	}
        //}


        [HttpPost]
        [Authorize]
        public ActionResult CampaignDelete(int? campaignID)
        {
            using (EntrantsEntities dbContext = new EntrantsEntities())
            {
                //return dbContext.DeleteCampaign(campaignID ?? 0, InstitutionID);

                Campaign campaign = dbContext.Campaign.FirstOrDefault(x => x.CampaignID == campaignID && x.InstitutionID == InstitutionID);
                if (campaign == null)
                    return new AjaxResultModel(AjaxResultModel.DataError);
                if (campaign.StatusID == CampaignStatusType.Finished)
                    return new AjaxResultModel("Невозможно удалить завершённую кампанию.");
                if (dbContext.CompetitiveGroup.Any(x => x.CampaignID == campaignID))
                    return new AjaxResultModel("Невозможно удалить кампанию, т.к. существуют конкурсы, привязанные к этой приемной кампании.");
                if (dbContext.InstitutionAchievements.Any(x => x.CampaignID == campaignID))
                    return new AjaxResultModel("Невозможно удалить кампанию, т.к. существуют индивидуальные достижения, привязанные к этой приемной кампании.");
                dbContext.Campaign.DeleteObject(campaign);
                dbContext.SaveChanges();

                return new AjaxResultModel();
            }
        }
        [HttpPost]
        [Authorize]		
		public ActionResult CampaignSwitchStatus(int? campaignID)
		{
            return new AjaxResultModel { Data = campaignRepository.SwitchCampaignStatus(InstitutionID, campaignID ?? 0) };
		}
	}
}