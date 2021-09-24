using NLog;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.DAL.Dapper.Repository.Interfaces.CompetitiveGroups;
using GVUZ.DAL.Dapper.Repository.Model.CompetitiveGroups;
using GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups;
using GVUZ.Web.Security;
using System.Linq;
using System.Collections.Generic;
using System;

namespace GVUZ.Web.Controllers {
    [MenuSection("Institution")]
	public class CompetitiveGroupController: BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        

        ICompetitiveGroupRepository competitiveGroupRepository;

        public CompetitiveGroupController()
        {
            this.competitiveGroupRepository = new CompetitiveGroupRepository();
        }
        public CompetitiveGroupController(ICompetitiveGroupRepository competitiveGroupRepository)
        {
            this.competitiveGroupRepository = competitiveGroupRepository;
        }


        public ActionResult CompetitiveGroupList()
        {
            logger.Debug("CompetitiveGroupList");
            var model = competitiveGroupRepository.GetCompetitiveGroupList(InstitutionID);
            //return Json(model.CompetitiveGroupList.ToArray());
            return View("CompetitiveGroupList", model);
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetCompetitiveGroups()
        {
            var model = competitiveGroupRepository.GetCompetitiveGroupList(InstitutionID);
            return Json(model.CompetitiveGroupList.ToArray());
            //return View("CompetitiveGroupList", model);
        }



        [HttpPost]
        [Authorize]
        public ActionResult GetCompetitiveGroupList(CompetitiveGroupViewModel model)
        {
            model = competitiveGroupRepository.GetCompetitiveGroupList(InstitutionID);
            model.CompetitiveGroupEdit.CanEdit = model.CompetitiveGroupEdit.CanEdit; //& !UserRole.IsCurrentUserReadonly();
            return new AjaxResultModel { Data = model };
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetCompetitiveGroupsByCampaign(int? InstitutionID, int? CampaignId, int? EducationLevelID)
        {
            if (InstitutionID == null || CampaignId == null || EducationLevelID == null)
            {
                return new AjaxResultModel("Ошибка параметров!");
            }
            var data = ContextExtensionsSQL.SQL.GetCompetitiveGroups(InstitutionID.Value, CampaignId.Value, EducationLevelID.Value);
            return new AjaxResultModel { Data =  data};
        }

        [Authorize]
        public ActionResult CompetitiveGroupEdit(int? competitiveGroupID)
        {
            var model = competitiveGroupRepository.FillCompetitiveGroupEditModel(competitiveGroupID, InstitutionID);
            //model.CompetitiveGroupEdit.CanEdit = model.CompetitiveGroupEdit.CanEdit & !UserRole.IsCurrentUserReadonly();
            return View("CompetitiveGroupEdit", model);
        }

        [Authorize]
        public ActionResult MultiProfileCompetitiveGroupEdit(int? competitiveGroupID)
        {
            var model = competitiveGroupRepository.FillCompetitiveGroupEditModel(competitiveGroupID, InstitutionID, true);
            //model.CompetitiveGroupEdit.CanEdit = model.CompetitiveGroupEdit.CanEdit & !UserRole.IsCurrentUserReadonly();
            return View("CompetitiveGroupEdit", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CompetitiveGroupDelete(int? competitiveGroupID)
        {
            var errors = new List<string>();

            if (!competitiveGroupID.HasValue)
                return new AjaxResultModel(AjaxResultModel.DataError);
            
            //if (campaign.StatusID == CampaignStatusType.Finished)
            //    return new AjaxResultModel("Невозможно удалить завершённую кампанию.");
            //if (dbContext.CompetitiveGroup.Any(x => x.CampaignID == campaignID))
            //    return new AjaxResultModel("Невозможно удалить кампанию, т.к. существуют конкурсные группы, привязанные к этой приемной кампании.");
            //if (dbContext.InstitutionAchievements.Any(x => x.CampaignID == campaignID))
            //    return new AjaxResultModel("Невозможно удалить кампанию, т.к. существуют индивидуальные достижения, привязанные к этой приемной кампании.");

            var result = competitiveGroupRepository.DeleteCompetitiveGroup(competitiveGroupID.Value);
            if (result.Code == 0)
                return Json(new { success = true });
            else {
                errors.Add(result.Message);
                return Json(new { success = false, errors });
            }
            //if (errors.Any())
            //    return Json(new { success = false, errors });

            //return Json(new { success = true });
        }


        [HttpPost]
        [Authorize]
        public ActionResult CompetitiveGroupUpdate(CompetitiveGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.CompetitiveGroupEdit.InstitutionID = InstitutionID;
                
                try
                {
                    if (competitiveGroupRepository.ValidateUpdateCompetitiveGroup(model, ModelState))
                    {
                        return new AjaxResultModel { Data = competitiveGroupRepository.UpdateCompetitiveGroup(model) };
                    }

                }
                catch (Exception ex)
                {

                    logger.Error(ex, "CompetitiveGroupUpdate error");
                }

               
            }
            return new AjaxResultModel(ModelState);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CompetitiveGroupCopy(CompetitiveGroupCopyModel data)
        {
            dynamic result = null;
            try
            {
                //var errors = new List<string>();
                //int[] competitiveGroupIDs = data.competitiveGroupIDs;
                //int copy_year = data.copy_year;
                //int copy_сampaignType = data.copy_сampaignType;
                data.InstitutionID = InstitutionID;
                result = competitiveGroupRepository.CompetitiveGroupCopy(data.competitiveGroupIDs, data.copy_year, data.copy_сampaignType, data.copy_levelBudget, data.InstitutionID);
                //if (result.Code == 0)
                //    return Json(new { success = true });
                //else
                //{
                //errors.Add(result.Message);
            }
            catch (Exception ex)
            {

                logger.Error(ex);
            }
            

            // Использовать вместо return Json(result) для dynamic
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result), "application/json");
            //}
        }
    }
}