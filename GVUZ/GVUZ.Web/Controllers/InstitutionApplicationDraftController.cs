using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Entrants;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using Microsoft.Practices.ServiceLocation;
using GVUZ.Model.ApplicationPriorities;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.DAL.Dapper.Repository.Model;
using System;
using log4net;

namespace GVUZ.Web.Controllers {
	[MenuSection("ApplicationsDraft")]
	[AuthorizeAdm(Roles=UserRole.EduUser)]
	public class InstitutionApplicationDraftController:BaseController {

		public static readonly ILog a_logger = log4net.LogManager.GetLogger("TargetOrganizationLogger");
		public ActionResult ApplicationListDraft() {
			using(EntrantsEntities dbContext=new EntrantsEntities()) {
				return View("InstitutionApplicationListDraft",
					dbContext.FillInstitutionApplicationList(new InstitutionApplicationListViewModel { InstitutionID=InstitutionID,TabID=-1 },true,InstitutionID));
			}
		}

		[AuthorizeDeny(Roles=UserRole.FBDReadonly+","+UserRole.FbdRonUser)]
		public ActionResult PrepareNewApplication() {
			using(EntrantsEntities dbContext=new EntrantsEntities()) {
				return View("InstitutionApplicationSNILS",dbContext.FillPrepareApplicationViewModel(InstitutionID));
			}
		}

		public ActionResult GetCompetitionGroups(string term) {
			using(EntrantsEntities dbContext=new EntrantsEntities()) {
				var loadCompetitiveGroups=dbContext.LoadCompetitiveGroups(InstitutionID,null,term);
				return Json(loadCompetitiveGroups.OrderBy(x => x.Name).Select(cg => cg.Name).ToArray(),JsonRequestBehavior.AllowGet);
			}
		}

		public const int ShowCompetitiveGroupAddDialog=1;
		public const int ShowMessageExistingEntrant=2;

		[HttpPost]
		[AuthorizeDeny(Roles=UserRole.FBDReadonly+","+UserRole.FbdRonUser)]
		public ActionResult CreateApplication(InstitutionPrepareApplicationViewModel model) {
			model.ApplicationNumber=model.ApplicationNumber.Trim();

			if(!ModelState.IsValid)
				return new AjaxResultModel(ModelState);

			using(EntrantsEntities dbContext=new EntrantsEntities()) {
				var valMsgs=new Dictionary<string,string>();
				bool isGroupAbsent;
				if(!dbContext.IsValidInstitutionPrepareApplicationViewModel(model,InstitutionID,ref valMsgs,out isGroupAbsent)) {
					AddModelErrors(valMsgs);
					AjaxResultModel ajaxResultModel=new AjaxResultModel(ModelState);
					if(isGroupAbsent) ajaxResultModel.Extra=ShowCompetitiveGroupAddDialog;
					return Json(ajaxResultModel);
				}
			}

			int? applicationID;
			int? entrandId;
			using(var dbContext=new EntrantsEntities()) {
				if(!dbContext.CheckApplicationNumberIsUnique(InstitutionID, model.ApplicationNumber))
					return new AjaxResultModel().SetIsError("ApplicationNumber","Данный номер заявления ОО уже используется");

				applicationID=dbContext.CreateAndGetApplicationAndEntrant(InstitutionID, model,out entrandId);
			}

			if(applicationID==0||applicationID==null)
				return new AjaxResultModel("Невозможно создать заявление");

			//Тут у заявления должен появиться ID - можно сохранить всё, что касается приоритетов
			using(var prioityContext=new ApplicationPrioritiesEntities()) {
				foreach(var priority in model.Priorities.ApplicationPriorities) {
					var dataObject=prioityContext.ApplicationCompetitiveGroupItem.CreateObject();
					dataObject.ApplicationId=applicationID.Value;
					dataObject.CompetitiveGroupId=priority.CompetitiveGroupId;
					dataObject.CompetitiveGroupItemId=priority.CompetitiveGroupItemId;
					dataObject.CompetitiveGroupTargetId=priority.CompetitiveGroupTargetId;
					dataObject.EducationFormId=priority.EducationFormId;
					dataObject.EducationSourceId=priority.EducationSourceId;
					dataObject.Priority=priority.Priority;

					prioityContext.AddToApplicationCompetitiveGroupItem(dataObject);
				}
				prioityContext.SaveChanges();
			}

			var session=ServiceLocator.Current.GetInstance<ISession>();
			session.SetValue(ApplicationController.ApplicationSessionKey,applicationID);
			return new AjaxResultModel {	Data=applicationID,	Extra=entrandId.HasValue?ShowMessageExistingEntrant:-1};
		}


		[HttpPost]
		public ActionResult GetDirectionsForCompetitiveGroups(int[] competitiveGroupIDs) {
			return new AjaxResultModel { Data=SQL.GetDirectionsForCompetitiveGroups(InstitutionID,competitiveGroupIDs) };
		}

		[HttpPost]
		public ActionResult GetCompetitiveGroupsByCampaign(int? CampaignId, int? EducationLevelID) {
			return new AjaxResultModel { Data=SQL.GetCompetitiveGroups(InstitutionID,CampaignId.Value, EducationLevelID.Value) };
		}

		[HttpPost]
		public ActionResult GetAvailableFormsForCompetitiveGroups(int[] competitiveGroupIDs,string[] directionKeys) {
			using(var dbContext=new EntrantsEntities()) {
				return dbContext.GetEducationFormsForCompetitiveGroups(competitiveGroupIDs,directionKeys,InstitutionID);
			}
		}

        [HttpPost]
        public ActionResult GetCompetitiveGroupInfo(int? competitiveGroupId)
        {
            if (competitiveGroupId == null)
                return new AjaxResultModel() { Data = null };
            var repository = new ApplicationRepository();
            var group = repository.GetCompetitiveGroupById(competitiveGroupId.Value);
            
            return new AjaxResultModel() { Data = group };
        }


        [HttpPost]
		public ActionResult CreatePrioritiesData(int[] competitiveGroupIds,string[] directionKeys) {
			if(competitiveGroupIds==null||directionKeys==null)
				return new AjaxResultModel() { Data=null };

			var key=string.Format("PrioritiesData_{0}_{1}_{2}",InstitutionID,string.Join(";",competitiveGroupIds.Distinct()),
                string.Join(";",directionKeys.Distinct()));

			var cache=ServiceLocator.Current.GetInstance<ICache>();
			var model=cache.Get<ActionResult>(key,null);
			if(model!=null) { return model; }

			var PrioritiesData=SQL.CreatePrioritiesData(InstitutionID,competitiveGroupIds,directionKeys);
			model=new AjaxResultModel() { Data=PrioritiesData };
			cache.Insert(key,model);
			return model;
		}

        [HttpPost]
        public ActionResult GetPrioritiesData(int? competitiveGroupId)
        {
            var PrioritiesData = SQL.GetPrioritiesData(competitiveGroupId.Value);
            return new AjaxResultModel() { Data = PrioritiesData };
        }

        public ActionResult GetConditionRow(int? id)
        {
			a_logger.ErrorFormat("-------------------------CompetitiveGroupId {0}------------------------------------", id);

			try
			{
				var group = new ApplicationRepository().GetCompetitiveGroupById(id.Value);
				var model = new ApplicationPriorityViewModel(InstitutionID, group);
				var prioritiesData = SQL.GetPrioritiesData(id.Value);
				var singlePriority = prioritiesData.ApplicationPriorities.FirstOrDefault();
				if (singlePriority != null)
				{
					model.TargetOrganizations = singlePriority.TargetOrganizations;
				}

				return PartialView("Application/ApplicationWz5_Conditions_Row", model);
			}
			catch(ArgumentNullException ex)
            {
				ViewData["JavaScriptErrorMessage"] = ex.Message;
				return View("JavaScriptError");
			}
        }

        public ActionResult RefreshConditions(int? id)
        {
            ApplicationWz5ViewModel.Wz5SendingViewModel wz5 = ApplicationSQL.GetApplicationWz5(id.Value, InstitutionID);

            // заполнение комбо с приемными компаниями
            if (wz5.InstitutionID > 0)
                wz5.Campaigns = SQL.GetCampaigns(wz5.InstitutionID);

            // заполнение комбо с полом
            wz5.Genders = SQL.GetGenders();

            return PartialView("Application/ApplicationWz5_Conditions", wz5);
        }

    }
}