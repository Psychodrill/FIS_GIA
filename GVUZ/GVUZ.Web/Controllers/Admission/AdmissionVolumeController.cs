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
using GVUZ.Web.ViewModels.AdmissionVolume;

using avv = GVUZ.DAL.Dapper.ViewModel.Admission;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using NLog;

namespace GVUZ.Web.Controllers.Admission
{
    public partial class AdmissionController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Authorize]
        public ActionResult VolumeView(int? campaignID, int? course)
        {
            var key = string.Format("VolumeView_{0}_{1}_{2}",
                InstitutionID,
                campaignID ?? 0,
                course ?? 0);

            // 2017-05-04, Roman.N.Bukin - убираем серверный кэш

            //var cache = ServiceLocator.Current.GetInstance<ICache>();
            //var cachedModel = cache.Get<avv.AdmissionVolumeViewModel> (key, null);
            //if (cachedModel != null) { return View("VolumeView", cachedModel); }

            var model = new avv.AdmissionVolumeViewModel();
            model.InstitutionID = InstitutionID;
            model.SelectedCampaignID = campaignID ?? 0;

            var cachedModel = admissionVolumeRepository.FillAdmissionVolumeViewModel(model, false);
            //cache.Insert(key, cachedModel);

            return View("VolumeView", cachedModel);
        }

        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult VolumeEdit(int? campaignID, int? course)
        {
            var model = new avv.AdmissionVolumeViewModel();
            model.InstitutionID = InstitutionID;
            model.SelectedCampaignID = campaignID ?? 0;
            return View("VolumeEdit", admissionVolumeRepository.FillAdmissionVolumeViewModel(model, true));
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public AjaxResultModel VolumeSave(GVUZ.DAL.Dapper.ViewModel.Admission.AdmissionVolumeViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    //вычищаем сообщения об ошибках, на данной странице они не нужны(будет подсветка контролов)
            //    foreach (var key in ModelState.Values)
            //        for (int i = 0; i < key.Errors.Count; i++)
            //            key.Errors[i] = new ModelError("");
            //    return new AjaxResultModel(ModelState);
            //}

            Dictionary<string, string> errors = new Dictionary<string, string>();
            Dictionary<string, List<Tuple<int, int>>> errorIdxes = new Dictionary<string, List<Tuple<int, int>>>();


            foreach (var item in model.Items)
            {
                List<Tuple<int, int>> errorIdx = new List<Tuple<int, int>>();

                if (item.DirectionID != 0)
                {
                    var data = admissionVolumeRepository.CheckKCP(model.SelectedCampaignID, item.AdmissionItemTypeID, 
                        item.DirectionID, item.DirectionID == null ? item.ParentDirectionID : null);

                    if (item.NumberBudgetO < data.NumberBudgetO)
                        errorIdx.Add(new Tuple<int, int>(0, data.NumberBudgetO));
                    if (item.NumberBudgetOZ < data.NumberBudgetOZ)
                        errorIdx.Add(new Tuple<int, int>(1, data.NumberBudgetOZ));
                    if (item.NumberBudgetZ < data.NumberBudgetZ)
                        errorIdx.Add(new Tuple<int, int>(2, data.NumberBudgetZ));
                    if (item.NumberQuotaO.GetValueOrDefault() < data.NumberQuotaO.GetValueOrDefault())
                        errorIdx.Add(new Tuple<int, int>(3, data.NumberQuotaO.GetValueOrDefault()));
                    if (item.NumberQuotaOZ.GetValueOrDefault() < data.NumberQuotaOZ.GetValueOrDefault())
                        errorIdx.Add(new Tuple<int, int>(4, data.NumberQuotaOZ.GetValueOrDefault()));
                    if (item.NumberQuotaZ.GetValueOrDefault() < data.NumberQuotaZ.GetValueOrDefault())
                        errorIdx.Add(new Tuple<int, int>(5, data.NumberQuotaZ.GetValueOrDefault()));
                    if (item.NumberPaidO < data.NumberPaidO)
                        errorIdx.Add(new Tuple<int, int>(6, data.NumberPaidO));
                    if (item.NumberPaidOZ < data.NumberPaidOZ)
                        errorIdx.Add(new Tuple<int, int>(7, data.NumberPaidOZ));
                    if (item.NumberPaidZ < data.NumberPaidZ)
                        errorIdx.Add(new Tuple<int, int>(8, data.NumberPaidZ));
                    if (item.NumberTargetO < data.NumberTargetO)
                        errorIdx.Add(new Tuple<int, int>(9, data.NumberTargetO));
                    if (item.NumberTargetOZ < data.NumberTargetOZ)
                        errorIdx.Add(new Tuple<int, int>(10, data.NumberTargetOZ));
                    if (item.NumberTargetZ < data.NumberTargetZ)
                        errorIdx.Add(new Tuple<int, int>(11, data.NumberTargetZ));
                }              
                  
                if (errorIdx.Count > 0)
                {
                    errorIdxes.Add(item.AdmissionItemTypeID + "," + item.DirectionID, errorIdx.ToList());
                    errors.Add(item.AdmissionItemTypeID + "," + item.DirectionID, "Недостаточно мест");
                }
            }

            if (errors.Count > 0)
                return new AjaxResultModel("df")
                {
                    Data = errors.Select(x =>
                        new
                        {
                            DirectionID = x.Key.Split(',')[1],
                            AdmID = x.Key.Split(',')[0],
                            Error = x.Value,
                            ErrorIdx = errorIdxes[x.Key].ToArray()
                        }).ToArray()
                };

            var results = new AjaxResultModel();

            try
            {
                using (var dbContext = new EntrantsEntities())
                {

                    results = dbContext.SaveAdmissionVolumeViewModel(model, InstitutionID);
                    admissionVolumeRepository.SaveAdmissionVolume(model);
                    ClearCache();
                    return results;

                }

            }
            catch (Exception ex)
            {

                logger.Error(ex, "VolumeSave error");
            }

            return results;
        }

        [Authorize]
        public ActionResult PlanVolumeView(int campaignId)
        {
            PlanAdmissionVolumeViewModel model = new PlanAdmissionVolumeViewModel();
            model.Load(campaignId, planAdmissionVolumeRepository, dictionaryRepository);

            return View("PlanVolumeView", model);
        }

        [Authorize]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult VolumeTransfer(int campaignId)
        {
            VolumeTransferViewModel model = new VolumeTransferViewModel();
            try
            {                
                model.Load(campaignId, InstitutionID, volumeTransferRepository, dictionaryRepository);               
            }
            catch (Exception ex)
            {

                logger.Error(ex, "VolumeTransfer error");
            }

            // Три проверки:
            // 1. строгая - что не задан объем приема
            // 2. распределение мест объема приема в конкурсах
            // 3. наличие зачисленных
            // + проверка по InstitutionID!
            if (model.AccessDenied)
            {
                return new AjaxResultModel("Нет доступа");
            }

            return View("VolumeTransfer", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult LoadVolumeTransfer(int campaignId)
        {
            dynamic result = null;

            try
            {
                result = volumeTransferRepository.VolumeTransferByCampaign(campaignId, InstitutionID);              
            }
            catch (Exception ex)
            {

                logger.Error(ex, "LoadVolumeTransfer error");
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result), "application/json");

        }

        [Authorize]
        [HttpPost]
        public ActionResult BeginVolumeTransfer(int[] competitiveGroupIDs, int campaignId)
        {
            var result = volumeTransferRepository.BeginVolumeTransfer(competitiveGroupIDs, campaignId, InstitutionID);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result), "application/json");
        }


        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult CreateAdmissionPlan(int campaignId)
        {
            planAdmissionVolumeRepository.CreatePlan(campaignId);
            return new AjaxResultModel();
        }

        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult PlanVolumeEdit(int campaignId)
        {
            PlanAdmissionVolumeViewModel model = new PlanAdmissionVolumeViewModel();
            model.Load(campaignId, planAdmissionVolumeRepository, dictionaryRepository);

            return View("PlanVolumeEdit", model);
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult PlanVolumeSave(PlanAdmissionVolumeSaveViewModel model)
        {
            try
            {
                model.Save(planAdmissionVolumeRepository);
            }
            catch (Exception ex)
            {

                logger.Error(ex, "PlanVolumeSave error");
            }
            
            return new AjaxResultModel();
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult DistributedPlanVolumeEdit(int campaignId, int levelId, int directionId, int directionGroupId)
        {
            DistributedPlanAdmissionVolumeViewModel model = new DistributedPlanAdmissionVolumeViewModel();
            try
            {
                model.Load(campaignId, levelId, directionId, directionGroupId, planAdmissionVolumeRepository, dictionaryRepository);
            }
            catch (Exception ex)
            {

                logger.Error(ex, "DistributedPlanVolumeEdit error");
            }
                     
            return PartialView("DistributedPlanVolumeEdit", model);
        }

        [HttpPost]
        [AuthorizeDeny(Roles = UserRole.FbdRonUser)]
        public ActionResult DistributedPlanVolumeSave(DistributedPlanAdmissionVolumeSaveViewModel model)
        {
            string errorMessage;
            IEnumerable<DistributedPlanAdmissionVolumeItemViewModel> errorItems;           
            try
            {
                if (!model.Validate(planAdmissionVolumeRepository, out errorMessage, out errorItems))
                    return new AjaxResultModel(errorMessage) { IsError = true, Data = errorItems };
                model.Save(planAdmissionVolumeRepository);
            }
            catch (Exception ex)
            {

                logger.Error(ex, "DistributedPlanVolumeSave error") ;
            }            
            return new AjaxResultModel();
        }
    }
}
