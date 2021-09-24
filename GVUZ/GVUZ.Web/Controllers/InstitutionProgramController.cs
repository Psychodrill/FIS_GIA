using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GVUZ.Helper.MVC;
using GVUZ.DAL.Dapper.ViewModel.InstitutionProgram;

using GVUZ.DAL.Dapper.Repository.Interfaces.InstitutionProgram;
using GVUZ.DAL.Dapper.Repository.Model.InstitutionProgram;

namespace GVUZ.Web.Controllers
{
    [MenuSection("Institution")]
    [Authorize]
    public class InstitutionProgramController : BaseController
    {
        IInstitutionProgramRepository institutionProgramRepository;
        public InstitutionProgramController()
        {
            this.institutionProgramRepository = new InstitutionProgramRepository();
        }
        public InstitutionProgramController(IInstitutionProgramRepository institutionProgramRepository)
        {
            this.institutionProgramRepository = institutionProgramRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult LoadRecords()
        {
            var result = institutionProgramRepository.GetInstitutionProgram(InstitutionID);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result), "application/json");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateProgram(InstitutionProgramModel data)
        {
            dynamic result = null;
            if (ModelState.IsValid)
            {
                result = institutionProgramRepository.UpdateProgram(InstitutionID, data);
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result), "application/json");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ProgramDelete(int? InstitutionProgramID)
        {
            var errors = new List<string>();
            try
            {
                errors = institutionProgramRepository.CanDeleteProgram(InstitutionID, InstitutionProgramID);
                if (errors.Any())
                {

                    return Json(new { success = false, errors });
                }
                else
                {
                    var res = institutionProgramRepository.DeleteProgram(InstitutionID, InstitutionProgramID);
                    return Json(new { success = true });
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
