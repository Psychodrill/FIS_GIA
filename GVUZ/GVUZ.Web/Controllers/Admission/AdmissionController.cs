using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Benefits;
using GVUZ.Model.Entrants;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels;
using GVUZ.Web.ViewModels.KcpDistribution;
using Microsoft.Practices.ServiceLocation;
using FogSoft.Helpers;

using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using GVUZ.DAL.Dapper.Repository.Model.Admission;
using GVUZ.DAL.Dapper.Repository.Interfaces.Dictionary;
using GVUZ.DAL.Dapper.Repository.Model.Dictionary;
using GVUZ.DAL.Dapper.Repository.Interfaces.AllowedDirections;
using GVUZ.DAL.Dapper.Repository.Model.AllowedDirections;
using GVUZ.DAL.Dto;
using GVUZ.Web.Helpers;
using GVUZ.DAL.Dapper.Model.AllowedDirections;
using NLog;

namespace GVUZ.Web.Controllers.Admission
{
	[MenuSection("Institution")]
    //[AuthorizeAdm(Roles = UserRole.EduUser)]
    public partial class AdmissionController : BaseController
	{
        private IAdmissionVolumeRepository admissionVolumeRepository;
        private IDictionaryRepository dictionaryRepository;
        private IAllowedDirectionsRepository allowedDirectionsRepository;
        private IPlanAdmissionVolumeRepository planAdmissionVolumeRepository;
        private IVolumeTransferRepository volumeTransferRepository;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AdmissionController()
        {
            this.admissionVolumeRepository = new AdmissionVolumeRepository();
            this.dictionaryRepository = new DictionaryRepository();
            this.allowedDirectionsRepository = new AllowedDirectionsRepository();
            this.planAdmissionVolumeRepository = new PlanAdmissionVolumeRepository();
            this.volumeTransferRepository = new VolumeTransferRepository();
        }
        public AdmissionController(IAdmissionVolumeRepository admissionVolumeRepository, IDictionaryRepository dictionaryRepository, IAllowedDirectionsRepository allowedDirectionsRepository, IPlanAdmissionVolumeRepository planAdmissionVolumeRepository)
        {
            this.admissionVolumeRepository = admissionVolumeRepository;
            this.dictionaryRepository = dictionaryRepository;
            this.allowedDirectionsRepository = allowedDirectionsRepository;
            this.planAdmissionVolumeRepository = planAdmissionVolumeRepository;
        }
        private void ClearCache()
        {
            EntrantApplicationExtensions.ClearFilterDataCache(InstitutionID);
            EntrantExtensions.ClearFilterDataCache(InstitutionID);
            ExtendedApplicationListExtensions.ClearFilterDataCache(InstitutionID);
            ApplicationOrderExtensions.ClearFilterDataCache(InstitutionID);

            var cache = ServiceLocator.Current.GetInstance<ICache>();
            cache.RemoveAllWithPrefix(
                "CompetitiveGroupList_" + InstitutionID,
                "VolumeView_" + InstitutionID);
        }
        //public ActionResult CompetitiveGroupList()
        //{
        //    using (InstitutionsEntities dbContext = new InstitutionsEntities())
        //    {
        //        return View("CompetitiveGroupList", dbContext.FillCompetitiveGroupViewModel(new CompetitiveGroupListViewModel(), InstitutionID));
        //    }
        //}

        [Authorize]
		public ActionResult CompetitiveGroupList(int? sortID, int? pageNumber, string fName, int? fCourse, int? fCampaignID, int? fEducationLevelID, string fuid)
		{
            //using (var dbContext = new EntrantsEntities())
            //{
            //	var model = new CompetitiveGroupListViewModel();
            //	model.SortID = sortID;
            //	model.PageNumber = pageNumber;
            //	model.Filter = new CompetitiveGroupListViewModel.FilterData
            //	{
            //		CampaignID = fCampaignID ?? 0,
            //		EducationLevelID = fEducationLevelID ?? 0,
            //		Course = fCourse ?? 0,
            //		Name = fName,
            //                 UID = fuid
            //	};

            //             var key = string.Format("CompetitiveGroupList_{0}_{1}_{2}_{3}",
            //                 InstitutionID,
            //                 sortID,
            //                 pageNumber,
            //                 model.Filter);

            //             var cache = ServiceLocator.Current.GetInstance<ICache>();
            //             var cachedModel = cache.Get<CompetitiveGroupListViewModel>(key, null);
            //             if (cachedModel != null)
            //                 return View("CompetitiveGroupList", cachedModel);

            //    cachedModel = dbContext.FillCompetitiveGroupViewModel(model, InstitutionID);
            //             cache.Insert(key, cachedModel);
            //             return View("CompetitiveGroupList", cachedModel);
            //}
            return View();

        }

		[Authorize]
		public ActionResult CompetitiveGroupViewPopup(int? groupID)
		{
			using (var dbContext = new EntrantsEntities())
			{
				CompetitiveGroupEditViewModel model = dbContext.FillCompetitiveGroupEditViewModel(new CompetitiveGroupEditViewModel(), InstitutionID, groupID ?? 0);
                model.CanEdit = model.CanEdit && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection));
				return PartialView("Admission/CompetitiveGroupView", model);
			}
		}

		[Authorize]
		public ActionResult CompetitiveGroupEdit(int groupID, int? navCampaign)
		{
			using (var dbContext = new EntrantsEntities())
			{
				CompetitiveGroupEditViewModel model = dbContext.FillCompetitiveGroupEditViewModel(new CompetitiveGroupEditViewModel(), InstitutionID, groupID);
                model.CanEdit = model.CanEdit && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection));
                model.FilterCampaign = navCampaign ?? 0;
				return View("CompetitiveGroupEdit", model);
			}
		}

		[Authorize]		
		[HttpPost]
		public ActionResult CompetitiveGroupCheckTargetOrganizationsUnique(int? groupID, CompetitiveGroupEditViewModel.OrganizationData org)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				return dbContext.CompetitiveGroupCheckTargetOrganizationsUnique(groupID ?? 0, org, InstitutionID);
			}
		}

		[Authorize]
		public ActionResult CompetitiveGroupEntranceTestEdit(int groupID)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				var model = dbContext.FillCompetitiveGroupEditViewModelForEntranceTestInfo(new CompetitiveGroupEditViewModel(), InstitutionID, groupID);
				model.CanEdit = model.CanEdit && !UserRole.IsCurrentUserReadonly();
				return View("EntranceTestEdit", model);
			}
		}

		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		[Authorize]
		public ActionResult CompetitiveGroupAdd()
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				return PartialView("Admission/AddCompetitiveGroup",
													 dbContext.FillCompetitiveGroupAddViewModel(new CompetitiveGroupAddViewModel(), InstitutionID));
			}
		}



		[Authorize]
		[AuthorizeDeny(Roles =  UserRole.FbdRonUser)]
		public ActionResult CompetitiveGroupAddWithName(string groupName)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				return PartialView("Admission/AddCompetitiveGroup",
					dbContext.FillCompetitiveGroupAddViewModel(
						new CompetitiveGroupAddViewModel { Name = groupName }, InstitutionID));
			}
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		[Authorize]
		public ActionResult SaveCompetitiveGroupAdd(CompetitiveGroupAddViewModel viewModel)
		{
			if (!ModelState.IsValid)
				return Json(new AjaxResultModel(ModelState));

			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				Dictionary<string, string> valMsgs = new Dictionary<string, string>();
				if (!dbContext.IsValidGroupAddViewModel(viewModel, InstitutionID, ref valMsgs))
					return JsonReturnModelErrors(valMsgs);

				int groupID = dbContext.SaveCompetitiveGroupAddViewModel(Url, viewModel, InstitutionID);
                ClearCache();
                return new AjaxResultModel { Data = new { GroupEditUrl = Url.Generate<AdmissionController>(c => c.CompetitiveGroupEdit(groupID, viewModel.CampaignID)) } };
			}
		}

		[Authorize]
		[AuthorizeDeny(Roles =  UserRole.FbdRonUser)]
		public AjaxResultModel CompetitiveGroupDelete(int? competitiveGroupID)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				var competitiveGroup = dbContext.CompetitiveGroup
				                                .Include(x => x.CompetitiveGroupTargetItem)
				                                .Include(x => x.CompetitiveGroupTargetItem.Select(y => y.CompetitiveGroupTarget)).FirstOrDefault(x => x.CompetitiveGroupID == competitiveGroupID);
				if (competitiveGroup == null || competitiveGroup.InstitutionID != InstitutionID)
					return new AjaxResultModel(AjaxResultModel.DataError);

				try
				{
					using (EntrantsEntities eContext = new EntrantsEntities())
					{
						if (eContext.ApplicationSelectedCompetitiveGroup.Any(x => x.CompetitiveGroupID == competitiveGroupID))
						{
							return new AjaxResultModel("Невозможно удалить группу, так как есть заявления, включенные в неё");
						}

						if (eContext.Campaign.Where(x => x.CampaignID == competitiveGroup.CampaignID).Select(x => x.StatusID).FirstOrDefault() == CampaignStatusType.Finished)
						{
							return new AjaxResultModel("Невозможно удалить группу, так как приемная кампания, в которую входит данная группа уже завершена");
						}
					}

					using (BenefitsEntities bContext = new BenefitsEntities())
					{
                        bContext.BenefitItemSubject.Where(x => x.BenefitItemC.CompetitiveGroupID == competitiveGroupID).ToList().ForEach(bContext.BenefitItemSubject.DeleteObject);
						bContext.BenefitItemC.Where(x => x.CompetitiveGroupID == competitiveGroupID && x.EntranceTestItemID == null).ToList().ForEach(bContext.BenefitItemC.DeleteObject);
						bContext.SaveChanges();
					}

					//competitiveGroup.CompetitiveGroupItem.SelectMany(x => x.CompetitiveGroupTarget).ToList().ForEach(dbContext.CompetitiveGroupTargetItem.DeleteObject);
					competitiveGroup.CompetitiveGroupItem.ToList().ForEach(dbContext.CompetitiveGroupItem.DeleteObject);
                    competitiveGroup.ApplicationEntranceTestDocument.Select(x => x.EntranceTestItemC).ToList().ForEach(dbContext.EntranceTestItemC.DeleteObject);
					dbContext.DeleteObject(competitiveGroup);
					dbContext.SaveChanges();

					//удаляем ненужные организации ЦП
					dbContext.CompetitiveGroupTarget
						.Where(x => x.InstitutionID == InstitutionID &&
                            !x.ApplicationSelectedCompetitiveGroupTarget.Any() &&
                            !x.Application.Any() &&
                            !x.CompetitiveGroupTargetItem.Any()).ToList().ForEach(dbContext.CompetitiveGroupTarget.DeleteObject);
					dbContext.SaveChanges();
                    ClearCache();
				}
				catch (Exception ex)
				{
					SqlException inner = ex.InnerException as SqlException;
                    //if (inner != null && inner.Message.Contains("FK_Application_CompetitiveGroup"))
                    //{
                    return new AjaxResultModel("Невозможно удалить группу, так как есть заявления, включенные в неё");
				    //}
                    //throw;
				}

				return new AjaxResultModel();
			}
		}

		[HttpPost]
		[AuthorizeDeny(Roles =  UserRole.FbdRonUser)]
		[Authorize]
		public AjaxResultModel CompetitiveGroupSave(CompetitiveGroupEditViewModel model)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				var result = dbContext.SaveCompetitiveGroupEditViewModel(model, InstitutionID);
                ClearCache();
			    return result;
			}
		}

		[HttpPost]
		[Authorize]
		public AjaxResultModel CompetitiveGroupGetCount(int? groupID, int? directionID, int? edLevelID)
		{
			if (!groupID.HasValue || !directionID.HasValue || !edLevelID.HasValue)
				return new AjaxResultModel(AjaxResultModel.DataError);
            using (InstitutionsEntities dbContext1 = new InstitutionsEntities())
            {
                if (dbContext1.RequestDirection.Any(x => x.Direction_ID == directionID && edLevelID == x.AdmissionItemType && InstitutionID == x.Request_ID))
                {
                    return new AjaxResultModel("Невозможно добавить направление в КГ, т.к. оно помечено к удалению в заявке.");
                }
            }
            
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
                //Если в КГ уже есть какой-нибудь уровень образования, то добавлять можно только такой же
                //исключение - бакалавриат и бакалавриат сокращенный {2,3}
                //http://redmine.armd.ru/issues/20835
			    CompetitiveGroup group = dbContext.CompetitiveGroup.FirstOrDefault(x => x.CompetitiveGroupID == groupID);
			    if (group != null && group.CompetitiveGroupItem.FirstOrDefault() != null)
			    {
			        if (group.CompetitiveGroupItem.FirstOrDefault().CompetitiveGroup.EducationLevelID != edLevelID)
			        {
			            if(!(new List<int>{2,3}).Contains(group.CompetitiveGroupItem.FirstOrDefault().CompetitiveGroup.EducationLevelID.Value)
                            && !(new List<int>{2,3}).Contains(edLevelID ?? 0))
                            return new AjaxResultModel("Прием на обучение должен осуществляться раздельно по программам бакалавриата, прикладного бакалавриата, специалитета, магистратуры, программам СПО и программам подготовки кадров высшей квалификации");
			        }
			    }
			    return dbContext.GetCompetitiveGroupAvailableDirectionCount(InstitutionID, groupID.Value, directionID.Value, edLevelID.Value);
			}
		}

		[HttpPost]
		[Authorize]
		public AjaxResultModel CanDeleteCompetitiveGroupDirection(int? groupID, int? directionID, int? edLevelID, int? dirCount)
		{
			if (!groupID.HasValue || !directionID.HasValue || !dirCount.HasValue || !edLevelID.HasValue)
				return new AjaxResultModel(AjaxResultModel.DataError);
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				return dbContext.CanDeleteCompetitiveGroupDirection(InstitutionID, groupID.Value, directionID.Value, edLevelID.Value, dirCount.Value);
			}
		}

		[HttpPost]
		[AuthorizeDeny(Roles = UserRole.FbdRonUser)]
		public AjaxResultModel GetAvailableDirectionsForCompetitiveGroup(CompetitiveGroupSelectedDirectionsViewModel model)
		{
			if (model == null)
				return new AjaxResultModel(AjaxResultModel.DataError);
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				return dbContext.GetRemainedAvailableDirections(
					model.CompetitiveGroupID,
					InstitutionID,
					model.EducationLevelID,
					model.SelectedDirections,
					(CompetitiveGroupExtensions.AvailableDirectionsFilter)model.FilterType);
			}
		}

		[HttpPost]
        [Authorize]
		public ActionResult AllowedDirectionsAddEdu()
		{
			using (InstitutionsEntities dbContext = new InstitutionsEntities())
			{
				var model = dbContext.FillAllowedDirectionAddModel();
				return PartialView("Admission/AddAllowedDirectionEdu", model);
			}
		}

        [HttpPost]
        [Authorize]
        public ActionResult AllowedDirectionsAddProf()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                var model = dbContext.FillAllowedDirectionAddModel();
                return PartialView("Admission/AddAllowedDirectionProf", model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AllowedDirectionsAddArt()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                var model = dbContext.FillAllowedDirectionAddModel();
                return PartialView("Admission/AddAllowedDirectionProf", model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AllowedDirectionsAdd()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                var model = dbContext.FillAllowedDirectionAddModel();
                ClearCache();
                return PartialView("Admission/AddAllowedDirection", model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddDirectionRequest(int? directionid, string comment, int? admissiontype)
        {
            if (directionid == null) return new AjaxResultModel();
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                try
                {
                    GVUZ.Model.Institutions.RequestDirection req = Model.Institutions.RequestDirection.CreateRequestDirection(directionid ?? 0, InstitutionID, "W", "Add", admissiontype ?? 0);
                    dbContext.RequestDirection.AddObject(req);
                    //dbContext.SaveChanges();


                int y;
                /*try
                {*/
                    y = dbContext.RequestComments.Count();
                /*}
                catch (System.InvalidOperationException)
                { y = 0; }*/
                    GVUZ.Model.Institutions.RequestComments rc = Model.Institutions.RequestComments.CreateRequestComments(y + 1, "U", directionid ?? 0, InstitutionID);
                rc.Comment = comment;
                dbContext.RequestComments.AddObject(rc);
                dbContext.SaveChanges();
                }
                catch (System.Data.UpdateException )
                {
                    throw;
                }

                return new AjaxResultModel();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteDirectionRequest(int? directionid, string comment, int? admissiontype)
        {
            int DirID = directionid ?? 0;
            int AdType = admissiontype ?? 0;

            if (DirID == 0) return new AjaxResultModel("");
            if (AdType == 0) return new AjaxResultModel("");

            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                using (EntrantsEntities dbContext1 = new EntrantsEntities())
                {
                    if (dbContext1.IsInCompetitiveGroup(InstitutionID, DirID, AdType))
                    {
                        string s =  dbContext.Direction.Where(x => x.DirectionID == DirID).Select(x => x.Name).Single().ToString();
                        //return new AjaxResultModel(@"Нельзя удалить направление """ + s + @""" т.к. оно содержится в конкурсных группах.");
                        return new AjaxResultModel(s);
                    }
                }
                try
                {
                    GVUZ.Model.Institutions.RequestDirection req = Model.Institutions.RequestDirection.CreateRequestDirection(DirID, InstitutionID, "W", "Delete", AdType);
                    dbContext.RequestDirection.AddObject(req);
                    dbContext.SaveChanges();


                int y;
                /*try
                {*/
                    y = dbContext.RequestComments.Select(x => x.Comment_ID).Count();
                /*}
                catch (System.InvalidOperationException)
                { y = 0; }*/
                    GVUZ.Model.Institutions.RequestComments rc = Model.Institutions.RequestComments.CreateRequestComments(y + 1, "U", DirID, InstitutionID); ;
                rc.Comment = comment;
                dbContext.RequestComments.AddObject(rc);
                dbContext.SaveChanges();

                }
                catch (System.Data.UpdateException)
                {
                    return new AjaxResultModel();
                }

                return new AjaxResultModel();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult RequestDirectionListToAdd()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.GetRequestedToAddDirections(InstitutionID);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult RequestDirectionListToDelete()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.GetRequestedToDeleteDirections(InstitutionID);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult RequestDirectionListDenied()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.GetDeniedDirections(InstitutionID);
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult ClearAllDenied()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.DeleteDenied(InstitutionID);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AllowedDirectionsDeleteEdu()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                var model = dbContext.FillAllowedDirectionAddModel();
                return PartialView("Admission/DeleteAllowedDirectionEdu", model);
            }
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult AllowedDirectionsGetAvailable(AllowedDirectionAddViewModel model)
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.GetRemainedAvailableAllowedDirections(model, InstitutionID);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AllowedDirectionsGetAvailableEdu(AllowedDirectionAddViewModel model)
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.GetRemainedAvailableAllowedDirections(model, InstitutionID);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult AllowedDirectionsGetExistingEdu(AllowedDirectionAddViewModel model)
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return dbContext.GetAllowedDirections(model, InstitutionID);
            }
        }

		[HttpPost]
        [Authorize]
		public ActionResult AllowedDirectionsAddSave(AllowedDirectionAddViewModel model)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				return dbContext.AddAllowedDirection(model, InstitutionID);
			}
		}

		[HttpPost]
        [Authorize]
		public ActionResult AllowedDirectionsDelete(int? educationLevelID, int? directionID)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
			    try
			    {
                    return dbContext.DeleteAllowedDirection(educationLevelID ?? 0, directionID ?? 0, InstitutionID);
			    }
			    finally
			    {
                    ClearCache();
			    }
			}
		}

		[HttpPost]
		public ActionResult GetDirectionInfo(int? directionID)
		{
			using (EntrantsEntities dbContext = new EntrantsEntities())
			{
				return dbContext.GetDirectionInfo(directionID ?? 0);
			}
		}

        [HttpPost]
        [Authorize]
        public ActionResult CancelRequest(string directionID)
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                int dir = Convert.ToInt32(directionID);
                //IQueryable<int> tlist = dbContext.Request.Where(x => x.InstitutionID == InstitutionID).Select(x => x.ID_Request);
                GVUZ.Model.Institutions.RequestDirection toCancel = dbContext.RequestDirection.Where(x => x.Direction_ID == dir && x.Request_ID == InstitutionID).First();
                               
                if (toCancel.Activity == "W")
                {
                    dbContext.DeleteObject(toCancel);
                    dbContext.SaveChanges();
                    // TODO: Finish Comment Deleting
                    return new AjaxResultModel();
                }
                else
                    return new AjaxResultModel("Данное направление в данный момент обрабатывается администратором.");
            }
        }

        [HttpPost]
        [Authorize]
        public AjaxResultModel RequestDisabled()
        {
            using (InstitutionsEntities dbContext = new InstitutionsEntities())
            {
                return new AjaxResultModel { Data = (dbContext.RequestDirection.Any(x => x.Activity == "A")) };
            }
        }

        #region Распределение КЦП по уровням бюджета FIS-69

        [HttpPost]
        [Authorize]
	    public ActionResult GetDistributionOptions(int? admissionVolumeId)
	    {
            using (var dbContext = new EntrantsEntities())
            {
                var model = dbContext.LoadDistributionOptions(admissionVolumeId.GetValueOrDefault(), InstitutionID);
                return Json(new { success = model != null, data = model });
            }
	    }

        [HttpPost]
        [Authorize]
	    public ActionResult UpdateDistribution(KcpUpdateViewModel model)
	    {
            #region CheckDistributionKCP
            bool error = false;
            foreach (var item in model.BudgetLevels)
            {
                //FIS - 1790 - проверить, что по каждому уровню бюджета сумма мест в конкурсах меньше!
                var data = admissionVolumeRepository.CheckDistributionKCP(model.AdmissionVolumeId, item.BudgetLevelId);
                string errorMessage = "Количество мест по уровню бюджета ({0}), не должно быть меньше суммарного количества мест, выделенных для соответствующего направления в конкурсах ({1})";
                
                // бюджет
                if (item.Budget.O.Value < data.NumberBudgetO)
                {
                    error = true;
                    item.Budget.O.ErrorMessage = string.Format(errorMessage, item.Budget.O.Value, data.NumberBudgetO);
                }
                if (item.Budget.OZ.Value < data.NumberBudgetOZ)
                {
                    error = true;
                    item.Budget.OZ.ErrorMessage = string.Format(errorMessage, item.Budget.OZ.Value, data.NumberBudgetOZ);
                }
                if (item.Budget.Z.Value < data.NumberBudgetZ)
                {
                    error = true;
                    item.Budget.Z.ErrorMessage = string.Format(errorMessage, item.Budget.Z.Value, data.NumberBudgetZ);
                }
                // квота особое право
                if (item.Quota.O.Value < data.NumberQuotaO)
                {
                    error = true;
                    item.Quota.O.ErrorMessage = string.Format(errorMessage, item.Quota.O.Value, data.NumberQuotaO);
                }
                if (item.Quota.OZ.Value < data.NumberQuotaOZ)
                {
                    error = true;
                    item.Quota.OZ.ErrorMessage = string.Format(errorMessage, item.Quota.OZ.Value, data.NumberQuotaOZ);
                }
                if (item.Quota.Z.Value < data.NumberQuotaZ)
                {
                    error = true;
                    item.Quota.Z.ErrorMessage = string.Format(errorMessage, item.Quota.Z.Value, data.NumberQuotaZ);
                }
                // целевой прием
                if (item.Target.O.Value < data.NumberTargetO)
                {
                    error = true;
                    item.Target.O.ErrorMessage = string.Format(errorMessage, item.Target.O.Value, data.NumberTargetO);
                }
                if (item.Target.OZ.Value < data.NumberTargetOZ)
                {
                    error = true;
                    item.Target.OZ.ErrorMessage = string.Format(errorMessage, item.Target.OZ.Value, data.NumberTargetOZ);
                }
                if (item.Target.Z.Value < data.NumberTargetZ)
                {
                    error = true;
                    item.Target.Z.ErrorMessage = string.Format(errorMessage, item.Target.Z.Value, data.NumberTargetZ);
                }
            }
            if (error)
            {
                return Json(new { success = false, data = model });
            }
            #endregion

            using (var dbContext = new EntrantsEntities())
            {

                if (dbContext.UpdateDistribution(model, InstitutionID))
                {
                    ClearCache();
                    return Json(new { success = true, data = new { model.TotalDistributed } });
                }
                //admissionVolumeRepository.UpdateAdmissionVolume(model);
                //ClearCache();


                return Json(new {success = false, data = model});
            }
        }

        #endregion

        #region Разрешенные направления и заявки FIS-1345
        [HttpPost]
        [Authorize]
        public ActionResult SearchDirectionsDialog()
        {
            var model = new SearchDirectionsDialogViewModel
            {
                EducationLevels = dictionaryRepository.GetEducationLevelsList(),
                Ugs = dictionaryRepository.GetUgsList()
            };

            return PartialView("Admission/SearchDirectionsDialog", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchDirections(SearchDirectionsDialogViewModel model)
        {
            var items = this.allowedDirectionsRepository.FindDirections(InstitutionID, model.GetSearchCommand());
            return Json(items.Select(x => new SimpleDto(x.DirectionId, x.DisplayName())).OrderBy(x => x.Name).ToArray());
        }

        private static readonly InstitutionDirectionRequestType[] RequestDirectionTypes = new[] { InstitutionDirectionRequestType.AddAllowedDirection, InstitutionDirectionRequestType.RemoveAllowedDirection };
        private static readonly InstitutionDirectionRequestType[] RequestProfDirectionTypes = new[] { InstitutionDirectionRequestType.AddProfDirection };

        /// <summary>
        /// Список заявок на включение/исключение направлений в/из списка разрешенных
        /// </summary>
        [HttpPost]
        [Authorize]
        public ActionResult RequestDirectionsData()
        {
            var items = allowedDirectionsRepository.GetDirectionRequestsByTypes(InstitutionID, RequestDirectionTypes);

            var model = new RequestDirectionsDataViewModel(items);
            return Json(model);
        }

        /// <summary>
        /// Список заявок на включение направлений в список с профильными ВИ
        /// </summary>
        [HttpPost]
        [Authorize]
        public ActionResult RequestProfDirectionsData()
        {
            var items = allowedDirectionsRepository.GetDirectionRequestsByTypes(InstitutionID, RequestProfDirectionTypes);
            var model = new RequestProfDirectionsDataViewModel(items);
            return Json(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteRequest(int? requestId)
        {
            allowedDirectionsRepository.DeleteDirectionRequest(InstitutionID, requestId.GetValueOrDefault());
            return new JsonResult();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ClearDeniedRequests(bool? isProf)
        {
            allowedDirectionsRepository.RemoveDeniedRequests(InstitutionID, isProf.GetValueOrDefault());
            return new JsonResult();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SubmitDirectionRequest(RequestDirectionsDataViewModel model)
        {
            allowedDirectionsRepository.SubmitDirectionRequests(InstitutionID, model.GetSubmits());
            return new JsonResult();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SubmitProfDirectionRequest(RequestProfDirectionsDataViewModel model)
        {
            allowedDirectionsRepository.SubmitDirectionRequests(InstitutionID, model.GetSubmits());
            return new JsonResult();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddAllowedDirections(AddAllowedDirectionsViewModel model)
        {
            try
            {
                allowedDirectionsRepository.AddAllowedDirections(InstitutionID, model.GetDto());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
           
            return new JsonResult();
        }
        #endregion

    }
}