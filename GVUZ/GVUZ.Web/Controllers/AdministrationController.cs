using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Administration;
using GVUZ.Model.Cache;
using GVUZ.Model.Entrants;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;
using GVUZ.Web.Pagers.Administration.Catalogs;
using GVUZ.Web.Security;
using GVUZ.Web.ViewModels.Administration;
using GVUZ.Web.ViewModels.Administration.Catalogs;
using GVUZ.DAL.Dapper.Repository.Model.Olympics;
using GVUZ.DAL.Dapper.ViewModel.Olympics;
using GVUZ.Web.Helpers;
using System.Collections.Generic;
using GVUZ.DAL.Helpers;
using GVUZ.Data.Helpers;

namespace GVUZ.Web.Controllers
{
    [Authorize(Roles = UserRole.EduAdmin + "," + UserRole.RonAdmin + "," + UserRole.FbdRonUser), MenuSection("Administration")]
    public class AdministrationController : BaseController
    {
        [Authorize(Roles = UserRole.EduAdmin + "," + UserRole.FbdRonUser)]
        public ActionResult EduList()
        {
            if (InstitutionID == 0)
            {
                if (User.IsInRole(UserRole.FbdRonUser))
                {
                    return RedirectToAction("List", "InstitutionAdmin");
                }

                //return RedirectToAction("List", "InstitutionAdmin");
                return RedirectToAction("CatalogsList");
            }

            using (AdministrationEntities usersEntities = new AdministrationEntities())
            {
                return View(usersEntities.GetUsersList(InstitutionID, UserOffice.Fbd, 1).Users);
            }
        }

        [Authorize(Roles = UserRole.RonAdmin)]
        public ActionResult RonList(int? sortID)
        {
            if (!sortID.HasValue) sortID = 1;
            using (AdministrationEntities usersEntities = new AdministrationEntities())
            {
                return View(usersEntities.GetUsersList(InstitutionID, UserOffice.Ron, sortID.Value));
            }
        }

        [Authorize(Roles = UserRole.FBDAdmin + "," + UserRole.RonAdmin)]
        public ActionResult CatalogsList()
        {
            return View("CatalogsList");
        }

        #region display catalogs

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult QualificationsCatalog()
        {
            //using (var dbContext = new EntrantsEntities())
            //{
            //    return View("Catalogs/Qualifications", new QualificationsPager()
            //        .Fill(dbContext.QualificationType, new QualificationsViewModel()));
            //}
            return null;
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult LanguagesCatalog()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return View("Catalogs/ForeignLanguages", new ForeignLanguagesPager()
                    .Fill(dbContext.ForeignLanguageType, new ForeignLanguagesViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GeneralSujectsCatalog()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return View("Catalogs/GeneralSubjects", new GeneralSubjectsPager()
                    .Fill(dbContext.Subject, new GeneralSubjectsViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult SubjectsAndEgeMinScore()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return View("Catalogs/SubjectsAndEgeMinScore", new SubjectsAndEgeMinScorePager()
                    .Fill(dbContext.Subject, new SubjectsAndEgeMinScoreViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GetSubjectsAndEgeMinScore(SubjectsAndEgeMinScoreViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel
                {
                    Data = new SubjectsAndEgeMinScorePager()
                                   .Fill(dbContext.Subject, model)
                };
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult EntranceTestCreativeDirections()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return View("Catalogs/EntranceTestCreativeDirections", new CreativeDirectionPager()
                    .Fill(dbContext.EntranceTestCreativeDirection, new CreativeDirectionViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GetCreativeDirections(CreativeDirectionViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel
                {
                    Data = new CreativeDirectionPager()
                        .Fill(dbContext.EntranceTestCreativeDirection, model)
                };
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult EntranceTestProfileDirections()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return View("Catalogs/EntranceTestProfileDirections", new ProfileDirectionPager()
                    .Fill(dbContext.EntranceTestProfileDirection, new ProfileDirectionViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GetProfileDirections(ProfileDirectionViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel
                {
                    Data = new ProfileDirectionPager()
                        .Fill(dbContext.EntranceTestProfileDirection, model)
                };
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult EntranceTests()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return View("Catalogs/EntranceTests", new EntranceTestsPager()
                    .Fill(dbContext.DirectionSubjectLink
                    .Include(x => x.DirectionSubjectLinkDirection)
                    .Include(x => x.DirectionSubjectLinkSubject),
                    new EntranceTestsViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GetEntranceTests(EntranceTestsViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel
                {
                    Data = new EntranceTestsPager()
                        .Fill(dbContext.DirectionSubjectLink
                        .Include(x => x.DirectionSubjectLinkDirection)
                        .Include(x => x.DirectionSubjectLinkSubject),
                        model)
                };
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GetLanguages(ForeignLanguagesViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel
                {
                    Data = new ForeignLanguagesPager().Fill(dbContext.ForeignLanguageType, model)
                };
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GetGeneralSubjects(GeneralSubjectsViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel
                {
                    Data = new GeneralSubjectsPager()
                        .Fill(dbContext.Subject, model)
                };
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GetQualifications(QualificationsViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel
                {
                    //Data = new QualificationsPager()
                    //    .Fill(dbContext.QualificationType, model)
                };
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult GetOlympics(OlympicsViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return new AjaxResultModel
                {
                    Data = new OlympicsPager()
                        .Fill(dbContext.OlympicType, model)
                };
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult CampaignOrderDateCatalog()
        {
            using (var dbContext = new EntrantsEntities())
            {
                CampaignOrderDateCatalogViewModel viewModel = new CampaignOrderDateCatalogViewModel();
                viewModel.CampaignOrderDates = dbContext.CampaignOrderDateCatalog.ToList().Select(x => new CampaignOrderDateCatalogViewModel.CampaignOrderDateCatalogData
                {
                    YearStart = x.YearStart,
                    StartDate = x.StartDate.ToString("dd.MM.yyyy"),
                    EndDate = x.EndDate.ToString("dd.MM.yyyy"),
                    TargetOrderDate = x.TargetOrderDate.ToString("dd.MM.yyyy"),
                    Stage1OrderDate = x.Stage1OrderDate.ToString("dd.MM.yyyy"),
                    Stage2OrderDate = x.Stage2OrderDate.ToString("dd.MM.yyyy"),
                    PaidOrderDate = x.PaidOrderDate.ToString("dd.MM.yyyy"),
                    PreviousUseDepth = x.PreviousUseDepth.HasValue ? x.PreviousUseDepth.Value : 1
                }).ToArray();
                return View("Catalogs/CampaignOrderDateCatalog", viewModel);
            }
        }

        public ActionResult EditCampaignOrderDateCatalogItem(CampaignOrderDateCatalog model)
        {
            // Необходимо реализовать проверку данных

            string errors = string.Empty;

            ModelStateDictionary errorDictionary = new ModelStateDictionary();

            if (model.StartDate.Year != model.YearStart)
                errorDictionary.AddModelError("startDatePicker", "Дата начала приёма документов не соответствует году начала ПК");
            if (model.EndDate.Year != model.YearStart)
                errorDictionary.AddModelError("endDatePicker", "Дата окончания приёма документов не соответствует году начала ПК");
            if (model.TargetOrderDate.Year != model.YearStart)
                errorDictionary.AddModelError("order0DatePicker", "Дата издания приказа о зачислении - целевой приём не соответствует году начала ПК");
            if (model.Stage1OrderDate.Year != model.YearStart)
                errorDictionary.AddModelError("order1DatePicker", "Дата издания приказа о зачислении - 1 этап не соответствует году начала ПК");
            if (model.Stage2OrderDate.Year != model.YearStart)
                errorDictionary.AddModelError("order2DatePicker", "Дата издания приказа о зачислении - 2 этап не соответствует году начала ПК");
            if (model.PaidOrderDate.Year != model.YearStart)
                errorDictionary.AddModelError("order3DatePicker", "Дата издания приказа о зачислении - с оплатой обучения не соответствует году начала ПК");

            if (errorDictionary.Count != 0)
            {
                return new AjaxResultModel(errorDictionary);
            }

            using (var dbContext = new EntrantsEntities())
            {
                CampaignOrderDateCatalog current = dbContext.CampaignOrderDateCatalog.FirstOrDefault(x => x.YearStart == model.YearStart);
                if (current != null)
                    dbContext.CampaignOrderDateCatalog.DeleteObject(current);
                dbContext.CampaignOrderDateCatalog.AddObject(model);
                dbContext.SaveChanges();

                return new AjaxResultModel
                {
                    Data = new CampaignOrderDateCatalogViewModel.CampaignOrderDateCatalogData
                    {
                        YearStart = model.YearStart,
                        StartDate = model.StartDate.ToString("dd.MM.yyyy"),
                        EndDate = model.EndDate.ToString("dd.MM.yyyy"),
                        TargetOrderDate = model.TargetOrderDate.ToString("dd.MM.yyyy"),
                        Stage1OrderDate = model.Stage1OrderDate.ToString("dd.MM.yyyy"),
                        Stage2OrderDate = model.Stage2OrderDate.ToString("dd.MM.yyyy"),
                        PaidOrderDate = model.PaidOrderDate.ToString("dd.MM.yyyy"),
                        PreviousUseDepth = model.PreviousUseDepth.HasValue ? model.PreviousUseDepth.Value : 1
                    }
                };
            }
        }

        [HttpPost]
        public ActionResult DeleteCampaignOrderDateCatalogItem(int? id)
        {
            if (!id.HasValue)
            {
                return new AjaxResultModel("Неверный параметр - год начала кампании");
            }

            using (var dbContext = new EntrantsEntities())
            {
                var setToDelete = dbContext.CampaignOrderDateCatalog.FirstOrDefault(x => x.YearStart == id.Value);

                if (setToDelete != null)
                {
                    dbContext.CampaignOrderDateCatalog.DeleteObject(setToDelete);
                    dbContext.SaveChanges();

                    return new AjaxResultModel(string.Format("Набор дат для {0} года удалён", id.Value));
                }
                else return new AjaxResultModel(string.Format("Для {0} года набор дат не найден", id.Value));
            }
        }

        public ActionResult GetDataForNewDateSet(object val)
        {
            using (var dbContext = new EntrantsEntities())
            {
                CampaignOrderDateCatalog last = dbContext.CampaignOrderDateCatalog.OrderByDescending(x => x.YearStart).FirstOrDefault();

                if (last == null)
                {
                    int yearCreate = DateTime.Today.Year;
                    return new AjaxResultModel
                    {
                        Data = new CampaignOrderDateCatalogViewModel.CampaignOrderDateCatalogData
                        {
                            YearStart = yearCreate,
                            StartDate = string.Format("20.06.{0}", yearCreate),
                            EndDate = string.Format("25.07.{0}", yearCreate),
                            TargetOrderDate = string.Format("31.07.{0}", yearCreate),
                            Stage1OrderDate = string.Format("05.08.{0}", yearCreate),
                            Stage2OrderDate = string.Format("11.08.{0}", yearCreate),
                            PaidOrderDate = string.Format("22.08.{0}", yearCreate),
                            PreviousUseDepth = 0
                        }
                    };
                }
                else
                {
                    return new AjaxResultModel
                    {
                        Data = new CampaignOrderDateCatalogViewModel.CampaignOrderDateCatalogData
                        {
                            YearStart = last.YearStart + 1,
                            StartDate = last.StartDate.AddYears(1).ToString("dd.MM.yyyy"),
                            EndDate = last.EndDate.AddYears(1).ToString("dd.MM.yyyy"),
                            TargetOrderDate = last.TargetOrderDate.AddYears(1).ToString("dd.MM.yyyy"),
                            Stage1OrderDate = last.Stage1OrderDate.AddYears(1).ToString("dd.MM.yyyy"),
                            Stage2OrderDate = last.Stage2OrderDate.AddYears(1).ToString("dd.MM.yyyy"),
                            PaidOrderDate = last.PaidOrderDate.AddYears(1).ToString("dd.MM.yyyy"),
                            PreviousUseDepth = last.PreviousUseDepth.HasValue ? last.PreviousUseDepth.Value : 1
                        }
                    };
                }
            }
        }

        #endregion

        #region edit foreign languages

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult AddForeignLanguage()
        {
            return PartialView("Catalogs/AddForeignLanguage", new AddForeignLanguageViewModel());
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult EditForeignLanguage(int? id)
        {
            if (id.HasValue)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    return PartialView("Catalogs/AddForeignLanguage",
                        dbContext.LoadForeignLanguage(new AddForeignLanguageViewModel(id.Value)));
                }
            }

            return new EmptyResult();
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult DeleteForeignLanguage(int? id)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.DeleteForeignLanguage(id);
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult CreateForeignLanguage(AddForeignLanguageViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.CreateForeignLanguage(model);
            }
        }

        #endregion

        #region edit olympics

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult AddOlympic()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return PartialView("Catalogs/AddOlympic", dbContext.InitOlympic(new AddOlympicViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult EditOlympic(int? id)
        {
            if (id.HasValue)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    return PartialView("Catalogs/AddOlympic",
                        dbContext.LoadOlympic(new AddOlympicViewModel(id.Value)));
                }
            }

            return new EmptyResult();
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult DeleteOlympic(int? id)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.DeleteOlympic(id);
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult CreateOlympic(AddOlympicViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            var errorString = model.Validate();
            if (!String.IsNullOrEmpty(errorString))
                return new AjaxResultModel().SetIsError("subjectNameTrick", errorString);

            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.CreateOlympic(model);
            }
        }

        #endregion

        #region edit qualifications

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult AddQualification()
        {
            return PartialView("Catalogs/AddQualification", new AddQualificationViewModel());
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult EditQualification(int? id)
        {
            if (id.HasValue)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    return PartialView("Catalogs/AddQualification",
                        dbContext.LoadQualification(new AddQualificationViewModel(id.Value)));
                }
            }

            return new EmptyResult();
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult DeleteQualification(int? id)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.DeleteQualification(id);
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult CreateQualification(AddQualificationViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.CreateQualification(model);
            }
        }

        #endregion

        #region edit subjects
        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult AddGeneralSubject()
        {
            return PartialView("Catalogs/AddGeneralSubject", new AddGeneralSubjectViewModel());
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult EditGeneralSubject(int? id)
        {
            if (id.HasValue)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    return PartialView("Catalogs/AddGeneralSubject",
                        dbContext.LoadGeneralSubject(new AddGeneralSubjectViewModel(id.Value)));
                }
            }

            return new EmptyResult();
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult DeleteGeneralSubject(int? id)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.DeleteGeneralSubject(id);
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult CreateGeneralSubject(AddGeneralSubjectViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.CreateGeneralSubject(model);
            }
        }
        #endregion

        #region edit subjects EGE minimal marks				

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult UpdateSubjectsMinEgeScores(SubjectsAndEgeMinScoreViewModel model)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.UpdateSubjectsMinEgeScores(model);
            }
        }

        #endregion

        #region edit entrance test CREATIVE directions

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult AddCreativeDirection()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return PartialView("Catalogs/AddCreativeDirection", dbContext.InitEntranceTestCreativeDirection(new AddCreativeDirectionViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult DeleteCreativeDirection(int? id)
        {
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.DeleteEnranceTestCreativeDirection(id);
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult CreateCreativeDirection(AddCreativeDirectionViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.CreateEnranceTestCreativeDirection(model);
            }
        }

        #endregion

        #region edit entrance test PROFILE directions

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult AddProfileDirection()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return PartialView("Catalogs/AddProfileDirection", dbContext.InitEntranceTestProfileDirection(new AddProfileDirectionViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult EditProfileDirection(int? id)
        {
            if (id.HasValue)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    return PartialView("Catalogs/AddProfileDirection",
                        dbContext.LoadProfileDirection(new AddProfileDirectionViewModel(id.Value)));
                }
            }

            return new EmptyResult();
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult DeleteProfileDirection(int? id)
        {
            if (id.HasValue)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    return dbContext.DeleteEnranceTestProfileDirection(id.Value);
                }
            }

            return new EmptyResult();
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult CreateProfileDirection(AddProfileDirectionViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.CreateEntranceTestProfileDirection(model);
            }
        }

        #endregion

        #region edit entrance tests

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult AddTest()
        {
            using (var dbContext = new EntrantsEntities())
            {
                return PartialView("Catalogs/AddTest", dbContext.InitTest(new AddTestViewModel()));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult EditTest(int? id)
        {
            if (id.HasValue)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    return PartialView("Catalogs/AddTest",
                        dbContext.LoadTest(new AddTestViewModel(id.Value)));
                }
            }

            return new EmptyResult();
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult DeleteTest(int? id)
        {
            if (id.HasValue)
                using (var dbContext = new EntrantsEntities())
                {
                    return dbContext.DeleteTest(id.Value);
                }

            return new EmptyResult();
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult CreateTest(AddTestViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (var dbContext = new EntrantsEntities())
            {
                return dbContext.CreateTest(model);
            }
        }

        #endregion

        [Authorize(Roles = UserRole.RonAdmin)]
        public ActionResult AAAddUser()
        {
            UserViewModel model = new UserViewModel();
            FillRoles(model);
            return View("EditUser", model);
        }

        private static void FillRoles(UserViewModel model)
        {
            using (AdministrationEntities dbContext = new AdministrationEntities())
            {
                model.ExistingRoles = dbContext.GetExistingRolesStrings();
                model.AssignedRoles = new string[0];
            }
        }

        //[Authorize(Roles = UserRole.RonAdmin + "," + UserRole.EduAdmin)]
        [Authorize(Roles = UserRole.RonAdmin)]
        public ActionResult EEEditUser(Guid userID)
        {
            using (AdministrationEntities dbContext = new AdministrationEntities())
            {
                return View("EditUser", dbContext.FillUserModel(userID));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        public ActionResult EditFbdUser(Guid userID)
        {
            using (AdministrationEntities dbContext = new AdministrationEntities())
            {
                return View("EditFbdUser", dbContext.FillUserModel(userID));
            }
        }

        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult SaveFbdUser(UserViewModel user)
        {
            using (var dbContext = new AdministrationEntities())
            {
                var changedUserModel = dbContext.FillUserModel(user.UserID);
                changedUserModel.Subroles = user.Subroles;
                changedUserModel.FilialID = user.FilialID == 0 ? null : user.FilialID;
                string saveUser = dbContext.SaveUser(changedUserModel, InstitutionID);
                if (saveUser != null)
                    return new AjaxResultModel("Ошибка сохранения пользователя");

                try
                {
                    /* Сбрасываем его кешированные роли */
                    WebCacheManager.ClearUserRights(user.UserID.ToString());
                }
                catch (Exception ex)
                {
                    LogHelper.Log.ErrorFormat("Ошибка в SaveFbdUser: {0}", ex.Message);
                }
            }

            return new AjaxResultModel();
        }

        public ActionResult ViewUser(Guid userID)
        {
            using (var dbContext = new AdministrationEntities())
            {
                return View("ViewUser", dbContext.FillUserModel(userID));
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRole.RonAdmin)]
        public ActionResult SaveUser(UserViewModel model)
        {
            // не используем поле "Пароль" при обновлении данных пользователя
            if (model.UserID != Guid.Empty)
                ModelState.Remove("Password");

            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);
            using (AdministrationEntities dbContext = new AdministrationEntities())
            {
                string saveUser = dbContext.SaveUser(model, InstitutionID);
                if (saveUser != null)
                    return new AjaxResultModel("Ошибка сохранения пользователя");
            }

            return new AjaxResultModel();
        }

        [HttpPost]
        public ActionResult SaveUserRoles(UserViewModel model)
        {
            AdministrationExtensions.SaveUserRoles(model);
            return new AjaxResultModel();
        }

        [HttpPost]
        [Authorize(Roles = UserRole.EduAdminOnly)]
        public ActionResult DeleteUser(string userID)
        {
            using (AdministrationEntities dbContext = new AdministrationEntities())
            {
                dbContext.DeleteUser(new Guid(userID));
            }

            return new AjaxResultModel();
        }

        public ActionResult ChangeUserPassword(string userName, Guid userID)
        {
            ViewData["PasswordLength"] = Membership.Provider.MinRequiredPasswordLength;
            return View(new ChangeUserPasswordViewModel { UserName = userName, UserID = userID });
        }

        [HttpPost]
        public ActionResult ChangeUserPassword(ChangeUserPasswordViewModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
                AddModelError("Введенные пароли не совпадают");

            if (ModelState.IsValid)
            {
                MembershipUser user = Membership.GetUser(model.UserName);
                if (user != null)
                {
                    if (user.ChangePassword(user.ResetPassword(), model.NewPassword))
                        return EEEditUser(model.UserID);

                    AddModelError("Пользователю не удалось сменить пароль");
                }
                else AddModelError("Пользователь не найден.");
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = Membership.Provider.MinRequiredPasswordLength;
            return View(model);
        }

        public ActionResult ManageEntranceCampaign()
        {
            var viewModel = new AppCampaignViewModel();
            using (InstitutionsEntities entities = new InstitutionsEntities())
            {
                var inst = entities.Institution.Where(x => x.InstitutionID == InstitutionID).FirstOrDefault();
                if (inst != null && inst.ReceivingApplicationDate.HasValue)
                    viewModel.ReceiveApplicationsDate = FormatReceiveAppDateTime(inst.ReceivingApplicationDate.Value);
            }

            using (EntrantsEntities entities = new EntrantsEntities())
            {
                viewModel.ExtraFields = entities.ApplicationExtraDefinition.ToArray();
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult BeginReceiveApplications()
        {
            using (InstitutionsEntities entities = new InstitutionsEntities())
            {
                var inst = entities.Institution.Where(x => x.InstitutionID == InstitutionID).FirstOrDefault();
                if (inst == null)
                    return Json(new AjaxResultModel("У пользователя нет привязки к ОО"));
                if (!inst.ReceivingApplicationDate.HasValue)
                {
                    DateTime dt = DateTime.Now;
                    inst.ReceivingApplicationDate = dt;
                    entities.SaveChanges();
                    return Json(FormatReceiveAppDateTime(dt));
                }

                return Json(new AjaxResultModel("Дата приема заявлений уже установлена: " +
                        FormatReceiveAppDateTime(inst.ReceivingApplicationDate.Value)));
            }
        }

        private static string FormatReceiveAppDateTime(DateTime dt)
        {
            return dt.ToString("f", CultureInfo.CurrentUICulture);
        }

        public ActionResult ImportantInformation()
        {
            return View();
        }


        //-----------------------------------------------------------------------------------------------------------
        // Olympics 
        //-----------------------------------------------------------------------------------------------------------

        private OlympicsListViewModel.FilterData GetStoredFilter()
        {
            return Session["OlympicsListViewModel.FilterData"] as OlympicsListViewModel.FilterData;
        }

        private void StoreFilter(OlympicsListViewModel.FilterData filter)
        {
            Session["OlympicsListViewModel.FilterData"] = filter;
        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        public ActionResult OlympicsCatalog(OlympicsListViewModel.FilterData filter)
        {
            if (filter != null)
            {
                if (filter.Filtered)
                {
                    StoreFilter(filter);
                }
                else
                {
                    OlympicsListViewModel.FilterData storedFilter = GetStoredFilter();
                    if ((storedFilter != null) && (storedFilter.Filtered))
                    {
                        filter.CopyFilter(storedFilter);
                    }
                }
            }
            var model = new OlympicsListViewModel(filter);

            if (Request.IsAjaxRequest())
                return PartialView("Olympics/TableView", model);
            else
                return View("Catalogs/Olympics", model);
        }


        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public ActionResult OlympicsCatalogDelete(int? id)
        {
            if (id.HasValue)
            {
                var repository = new OlympicsRepository();
                repository.OlympicDelete((int)id);
            }
            return new AjaxResultModel();
        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public ActionResult OlympicsCatalogAdd()
        {
            var model = new OlympicsCatalogEditViewModel();
            // подготовка к созданию новой записи
            model.InitialEdit(0);

            return PartialView("Olympics/OlympicsCatalogEdit", model);
        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public ActionResult OlympicsCatalogEdit(int? id)
        {
            if (id.HasValue)
            {
                var model = new OlympicsCatalogEditViewModel();
                // подготовка к редактированию записи
                model.InitialEdit((int)id);

                return PartialView("Olympics/OlympicsCatalogEdit", model);
            }
            return new AjaxResultModel();
        }

        [Authorize(Roles = UserRole.FBDAdmin)]
        [HttpPost]
        public ActionResult OlympicsCatalogModify(OlympicsCatalogEditViewModel model)
        {
            if (!ModelState.IsValid)
                return new AjaxResultModel(ModelState);

            var repository = new OlympicsRepository();
            var result = repository.OlympicUpdate(model);
            if (!result)
            { 
                ModelState.AddModelError("error", "Олимпиада по такому профилю уже существует!");
                return new AjaxResultModel(ModelState);
            }


            // !!!
            return new AjaxResultModel { Data = "Ok!" };
        }

        OlympicsRepository olympicsRepository;

        public ActionResult GetInstitution(string term)
        {
            if (olympicsRepository == null)
                olympicsRepository = new OlympicsRepository();

            List<AutoComplete> list = olympicsRepository.GetInstitutionFilter(term).Select(s=> new AutoComplete {
                id = s.InstitutionID,
                label = s.FullName,
                value = s.FullName
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult GetAddressForInstitution(int? id)
        {
            if (id.HasValue)
            {
                if (olympicsRepository == null)
                    olympicsRepository = new OlympicsRepository();

                var data = olympicsRepository.GetAddressForInstitution((int)id);

                return Json(data.Address, JsonRequestBehavior.AllowGet);
            }

            return new AjaxResultModel();
        }

        //-----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Возврашает олимпиады по гуду, используется несколькими контроллерами для фильтров, списков
        /// </summary>
        [Authorize(Roles = UserRole.EduAdmin)]
        [HttpPost]
        public ActionResult GetOlympicsForYear(int? year)
        {
            if (year.HasValue && year > 0)
            {
                var olympicsRepository = new OlympicsRepository();
                var data = olympicsRepository.GetOlympicTypeNamesByYear((int)year);
                var olympics = data.Select(s => new SelectorItem
                {
                    Id = s.OlympicID,
                    Name = s.Name
                });
                olympics = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(olympics);

                return Json(olympics, JsonRequestBehavior.AllowGet);
            }

            return new EmptyResult();
        }

        //-----------------------------------------------------------------------------------------------------------

    }
}
