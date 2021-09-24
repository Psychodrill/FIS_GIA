using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using GVUZ.Helper.MVC;
using GVUZ.Model.Entrants;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Helpers;
using GVUZ.Web.ViewModels.TargetOrganizations;

using GVUZ.DAL.Dapper.Repository.Interfaces.TargetOrganization;
using GVUZ.DAL.Dapper.Repository.Model.TargetOrganization;
using System.Configuration;
using NLog;
using System;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Institution")]
    [Authorize]
    public class TargetOrganizationController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        ICompetitiveGroupTargetRepository competitiveGroupTargetRepository;

        public TargetOrganizationController()
        {
            this.competitiveGroupTargetRepository = new CompetitiveGroupTargetRepository();
        }
        public TargetOrganizationController(ICompetitiveGroupTargetRepository competitiveGroupTargetRepository)
        {
            this.competitiveGroupTargetRepository = competitiveGroupTargetRepository;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoadRecords()
        {
            var result = competitiveGroupTargetRepository.GetCompetitiveGroupTarget(InstitutionID);
            return Json(result.ToArray());

        }
           
        [HttpPost]
        public ActionResult UpdateRecord([ModelBinder(typeof(TargetOrganizationModelBinder))]DAL.Dapper.Model.TargetOrganization.CompetitiveGroupTarget model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.InstitutionID = InstitutionID;
                    if (competitiveGroupTargetRepository.ValidateUpdateCompetitiveGroupTarget(model, ModelState))
                    {
                        return Json(new { success = true, record = competitiveGroupTargetRepository.UpdateCompetitiveGroupTarget(model) });
                    }
                }

            }
            catch (Exception ex)
            {

                logger.Error(ex, "Target Update error");
            }
           
            return Json(new { success = false, errors = ModelState.ToKeyValueDictionary() });
        }

        [HttpPost]
        public ActionResult DeleteRecord(int competitiveGroupTargetId)
        {
            var errors = new List<string>();
            try
            {
                errors = competitiveGroupTargetRepository.CanDeleteCompetitiveGroupTarget(InstitutionID, competitiveGroupTargetId);
                if (errors.Any())
                {
                   
                    return Json(new { success = false, errors });
                }
                else
                {
                    var res = competitiveGroupTargetRepository.DeleteCompetitiveGroupTarget(InstitutionID, competitiveGroupTargetId);
                    
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return Json(new { success = true });

        }
    }
}
