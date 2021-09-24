using GVUZ.DAL.Dapper.Repository.Model.OlympicDiplomant;
using GVUZ.DAL.Helpers;
using GVUZ.Data.Helpers;
using GVUZ.Data.Model;
using GVUZ.Helper;
using GVUZ.Web.ViewModels.OlympicDiplomant;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GVUZ.Web.Controllers
{
    public class OlympicDiplomantController : BaseController
    {
        private OlympicDiplomantFilterViewModel GetStoredFilter()
        {
            return Session["OlympicDiplomantFilterViewModel"] as OlympicDiplomantFilterViewModel;
        }

        private void StoreFilter(OlympicDiplomantFilterViewModel filter)
        {
            Session["OlympicDiplomantFilterViewModel"] = filter;
        }

        //-----------------------------------------------------------------------------------------------------------

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index(OlympicDiplomantFilterViewModel filter)
        {
            if (filter != null)
            {
                if ((filter.Filtered))
                {
                    StoreFilter(filter);
                }
                else
                {
                    OlympicDiplomantFilterViewModel storedFilter = GetStoredFilter();
                    if ((storedFilter != null) && (storedFilter.Filtered))
                    {
                        filter.CopyFilter(storedFilter);
                    }
                }
            }
            var model = new OlympicDiplomantListViewModel(filter, this.InstitutionID);

            if (Request.IsAjaxRequest())
                return PartialView("OlympicDiplomant/OlympicDiplomantTableView", model);
            else
                return View("Index", model);
        }

        //-----------------------------------------------------------------------------------------------------------

        //[Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Edit(long? id)
        {
            if (id.HasValue)
            {
                var model = new OlympicDiplomantEditViewModel((long)id, this.InstitutionID);
                return PartialView("OlympicDiplomant/OlympicDiplomantEditView", model);
            }
            return new AjaxResultModel();
        }

        //-----------------------------------------------------------------------------------------------------------

        //[Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Add()
        {
            var model = new OlympicDiplomantEditViewModel(0, this.InstitutionID);
            return PartialView("OlympicDiplomant/OlympicDiplomantEditView", model);
        }

        //-----------------------------------------------------------------------------------------------------------

        //[Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Delete(long? id)
        {
            new OlympicDiplomantRepository().DeleteOlympicDiplomant((long)id);
            return new AjaxResultModel();
        }

        //-----------------------------------------------------------------------------------------------------------

        //[Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult Save(OlympicDiplomantEditViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            model.Save();
            return new AjaxResultModel();
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult GetOlympicsForYear(int? year)
        {
            var olympics = new List<SelectorItem>();

            if (year.HasValue && year > 0)
            {
                olympics = Repository.GetOlympicsData().
                    Where(p => p.OlympicType.OlympicYear == (int)year && p.OrgOlympicEnterID == this.InstitutionID).
                    DistinctBy(p => p.OlympicType.Name).
                    Select(s => new SelectorItem
                    {
                        Id = s.OlympicType.OlympicID,
                        Name = s.OlympicType.Name
                    }).ToList();
            }

            var list = new List<SelectorItem>(){ new SelectorItem
            {
                Id = 0,
                Name = "Не выбрано"
            } }.Concat(olympics);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult GetProfilesForOlympic(int? olympic)
        {
            var profiles = new List<SelectorItem>();

            if (olympic.HasValue && olympic > 0)
            {
                profiles = Repository.GetOlympicsData().
                    Where(p => p.OlympicTypeID == (int)olympic && p.OrgOlympicEnterID == this.InstitutionID).
                    Select(s => new SelectorItem
                    {
                        Id = s.OlympicTypeProfileID,
                        Name = s.OlympicProfile.ProfileName
                    }).ToList();
            }

            var list = new List<SelectorItem>(){ new SelectorItem
            {
                Id = 0,
                Name = "Не выбрано"
            } }.Concat(profiles);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //-----------------------------------------------------------------------------------------------------------

        //[Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult AddCanceled(OlympicDiplomantDocument doc)
        {
            var model = new OlympicDiplomantCanceledEditViewModel();
            model.Data = doc;
            //model.Data.DateIssue = DateTime.Now;
            return PartialView("OlympicDiplomant/OlympicDiplomantCanceledEditView", model);
        }

        //-----------------------------------------------------------------------------------------------------------

        //[Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult PresentCanceledRow(OlympicDiplomantDocument doc)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);

            doc.IdentityDocumentType = new OlympicDiplomantRepository().GetIdentityDocumentTypeById(doc.IdentityDocumentTypeID);
            return PartialView("OlympicDiplomant/OlympicDiplomantRowCanceledView", doc);
        }

        //-----------------------------------------------------------------------------------------------------------

        //[Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult EditCanceled(OlympicDiplomantDocument doc)
        {
            var model = new OlympicDiplomantCanceledEditViewModel();
            model.Data = doc;
            return PartialView("OlympicDiplomant/OlympicDiplomantCanceledEditView", model);
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult FindOlympicDiplomant(long? id)
        {
            //var olympicsRepository = new OlympicDiplomantRepository();
            //olympicsRepository.SyncOlympicDiplomant((long)id);
            ///// ???
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult SyncOlympicDiplomant(OlympicDiplomantEditViewModel model)
        {
            var docs = model.Data.OlympicDiplomantDocumentCanceled.AsEnumerable().
                Concat(new List<OlympicDiplomantDocument> { model.Data.OlympicDiplomantDocument });

            var result = Repository.SyncOlympicDiplomant(docs);

            if (result.Status == 1)
            {
                //var info = result.Persons.FirstOrDefault() ?? new FindPerson { };
                return PartialView("OlympicDiplomant/OlympicDiplomantFindInfo1View", result);
            }
            else
            if (result.Status == 3)
            {
                return PartialView("OlympicDiplomant/OlympicDiplomantFindInfo3View", result);
            }
            else
            if (result.Status == 4)
            {
                return PartialView("OlympicDiplomant/OlympicDiplomantFindInfo4View", result);
            }

            return new AjaxResultModel();
        }


        //-----------------------------------------------------------------------------------------------------------

        OlympicDiplomantRepository repository;
        public OlympicDiplomantRepository Repository
        {
            get
            {
                if (repository == null)
                    repository = new OlympicDiplomantRepository();
                return repository;
            }
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult FilterPersons(FindPerson model)
        {
            var result = Repository.FindPersons(model);
            return PartialView("OlympicDiplomant/OlympicDiplomantFindResultsView", result);
        }


        //-----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult UpdateInfo(int? status, int? person, string adoptionUnfoundedComment)
        {
            var model = new OlympicDiplomantEditViewModel(0, this.InstitutionID);
            model.Data.AdoptionUnfoundedComment = adoptionUnfoundedComment;
            model.Data.OlympicDiplomantStatus = Repository.GetOlympicDiplomantStatusById(status);
            model.Data.PersonId = person;
            if (person != null && person > 0)
                model.Data.RVIPersons = Repository.GetRVIPersonsById((int)person);

            return PartialView("OlympicDiplomant/OlympicDiplomantInfoView", model);
        }

        //-----------------------------------------------------------------------------------------------------------

        string RenderViewToString(string viewName, object model = null)
        {
            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult UploadFiles()
        {
            var model = new OlympicDiplomantImportViewModel(this.InstitutionID);

            if(Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0 && !file.FileName.EndsWith(".csv"))
                    model.ErrorMessage = "Это не CSV файл!";
                else
                    model.ReadFile(file);
            }

            return new AjaxResultModel
            {
                Data = RenderViewToString("~/Views/Shared/OlympicDiplomant/OlympicDiplomantImportView.ascx", model)
            };
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpPost]
        public ActionResult Find(long[] list)
        {
            if(list != null && list.Count() > 0)
                Repository.SyncOlympicDiplomantMultiple(list);

            return new AjaxResultModel();
        }

        //-----------------------------------------------------------------------------------------------------------

    }
}