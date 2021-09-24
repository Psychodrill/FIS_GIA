using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model.Administration;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.Web.Auth;
using GVUZ.Web.Models.Account;
using GVUZ.Web.Security;
using Institution = GVUZ.Model.Institutions.Institution;
using UserPolicy = GVUZ.Model.Administration.UserPolicy;
using System.Web;

namespace GVUZ.Web.Controllers
{
	[HandleError]
	public class AccountController : BaseController
	{
		public IFormsAuthenticationService FormsService { get; set; }
		public IMembershipService MembershipService { get; set; }

        public static object x = new object();

        public JsonResult UserConnected()
        {
            try
            {
                int connected;
                lock (x)
                {
                    var ip = UserHelper.GetCurrentUserID();
                    if (!string.IsNullOrEmpty(ip))
                    {
                        if (MvcApplication.ConnectedtUsers.ContainsKey(ip))
                            MvcApplication.ConnectedtUsers[ip] = DateTime.Now;
                        else
                            MvcApplication.ConnectedtUsers.Add(ip, DateTime.Now);
                    }

                    connected = MvcApplication.ConnectedtUsers.Count(c => c.Value.AddSeconds(900d) > DateTime.Now);
                    var toremove = MvcApplication.ConnectedtUsers.Where(c => c.Value.AddSeconds(900d) < DateTime.Now).Select(c => c.Key).ToList();

                    foreach (string key in toremove)
                        MvcApplication.ConnectedtUsers.Remove(key);
                }

                return Json(new { count = connected }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
            }

            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
        }

		protected override void Initialize(RequestContext requestContext)
		{
			if (FormsService == null)
			{
				FormsService = new FormsAuthenticationService();
			}

			if (MembershipService == null)
			{
				MembershipService = new AccountMembershipService();
			}

			base.Initialize(requestContext);
		}

		protected new void AddModelError(string message, string key = "")
		{
			ModelState.AddModelError(key, message);
		}

		// **************************************
		// URL: /Account/LogOn
		// **************************************

		public ActionResult LogOn()
		{
			RedirectUserByRoles();
			return View(new LogOnModel());            
		}

		public void RedirectUserByRoles()
		{
			if (Roles.IsUserInRole(UserRole.RonNsi))
				Response.Redirect(Url.Generate<ImportDicController>(x => x.ImportDictionaries()));
			else if (Roles.IsUserInRole(UserRole.RonAdmin))
				Response.Redirect(Url.Generate<AdministrationController>(x => x.RonList(null)));
			else if (Roles.IsUserInRole(UserRole.EduAdminOnly) || Roles.IsUserInRole(UserRole.FBDAdmin))
				Response.Redirect(Url.Generate<AdministrationController>(x => x.EduList()));
			else if (Roles.IsUserInRole(UserRole.RonInst))
				Response.Redirect(Url.Generate<InstitutionController>(x => x.ViewRonInst(null)));
			else if (Roles.IsUserInRole(UserRole.FbdUser) || Roles.IsUserInRole(UserRole.FbdAuthorizedStaff) || Roles.IsUserInRole(UserRole.EduUserOnly))
				Response.Redirect(Url.Generate<InstitutionController>(x => x.View(null)));
			else if (Roles.IsUserInRole(UserRole.FbdRonUser))
				Response.Redirect(Url.Generate<InstitutionAdminController>(x => x.List()));
		}

		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (MembershipService.ValidateUser(model.UserName, model.Password))
				{
					FormsService.SignIn(model.UserName, model.RememberMe);					

					if (!String.IsNullOrEmpty(returnUrl))
					{
						return Redirect(returnUrl);
					}

					return RedirectToAction("View", "Institution");
				}

				AddModelError("Имя пользователя или пароль указаны неверно.");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		// **************************************
		// URL: /Account/LogOff
		// **************************************

		public ActionResult LogOff()
		{
			FormsService.SignOut();
			Session.Clear();
            Session.Abandon();
			EsrpAuthModule.SignOut();

			//return RedirectToRoute("Default");
			return RedirectToAction("LogOn", "Account");
		}

		// **************************************
		// URL: /Account/Register
		// **************************************

		public ActionResult Register()
		{
			ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
			return View(new RegisterModel());
		}

		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			AccountValidation accountValidation = null;
			if (ModelState.IsValid)
			{
				MembershipCreateStatus createStatus;
				// получаем и проверяем или регистрируем пользователя

				using (InstitutionsEntities context = new InstitutionsEntities())
				{
					object providerUserKey = MembershipService.GetProviderUserKey(model.Email);
					if (providerUserKey == null)
					{
						createStatus = MembershipService.CreateUser(model.Email, model.Password, model.Email, true, out providerUserKey);
					}
					else
					{
						if (context.UserPolicy.Where(x => x.UserID == (Guid)providerUserKey).FirstOrDefault() != null
						    || MembershipService.ValidateUser(model.Email, model.Password))
						{
							accountValidation = AccountValidation.Create(MembershipCreateStatus.DuplicateEmail);
							goto lExit;
						}

						createStatus = MembershipCreateStatus.Success;
					}

					if (createStatus == MembershipCreateStatus.Success)
					{
						try
						{
							Institution institution = context.CreateInstitution(
								model.InstitutionName, model.Inn, model.Ogrn, model.Email,
								model.AdministratorName, (Guid)providerUserKey, model.InstitutionType);

							MembershipService.ApproveUser(model.Email);
							FormsService.SignIn(model.Email, false, institution.InstitutionID, model.AdministratorName);
							return RedirectToAction("View", "Institution");
						}
						catch (StackOverflowException)
						{
							throw;
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch (Exception e)
						{
                            LogHelper.Log.Error(e.Message, e);

							if (e.InnerException is SqlException)
							{
								if (e.InnerException.Message.Contains("UK_Institution_INN"))
									accountValidation = AccountValidation.Create(InstitutionCreationError.DuplicatedINN);
								else if (e.InnerException.Message.Contains("UK_Institution_OGRN"))
									accountValidation = AccountValidation.Create(InstitutionCreationError.DuplicatedOGRN);
								else
									accountValidation = AccountValidation.Create(InstitutionCreationError.Unknown);
							}
							else
							{
								accountValidation = AccountValidation.Create(MembershipCreateStatus.InvalidProviderUserKey);
							}

							MembershipService.DeleteUser(model.Email);
							
							goto lExit;
						}
					}
				}

				accountValidation = AccountValidation.Create(createStatus);
			}

			lExit:
			if (accountValidation != null)
				ModelState.AddModelError(accountValidation.Key, accountValidation.ErrorMessage);
			// If we got this far, something failed, redisplay form
			ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
			return View(model);
		}

		// **************************************
		// URL: /Account/ChangePassword
		// **************************************

		[Authorize]
		public ActionResult Personal()
		{
			ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
			PersonalModel model = new PersonalModel();
			AddPinCodeToModel(model);
			return View(model);
		}

		private static void AddPinCodeToModel(PersonalModel model)
		{
			using (AdministrationEntities entities = new AdministrationEntities())
			{
				UserPolicy userPolicy = GetCurrentUserPolicy(entities);
				model.PinCode = userPolicy.PinCode;
				model.AvailableEgeCheckCount = userPolicy.AvailableEgeCheckCount;
			}
		}

		[Authorize]
		[HttpPost, CallByParameter]
		public ActionResult GetPinCode(PersonalModel model)
		{
			// в этом блоке ничего не валидируем, 
			if (!ModelState.IsValid)
			{
				// если будет больше блоков с валидацие данных, то нужно будет усложнить логику (например: http://blogs.imeta.co.uk/MBest/archive/2010/01/19/833.aspx)
				ModelState.Clear();
			}

			model.PinCode = RandomHelper.GetSixRandomDigits();
			model.AvailableEgeCheckCount = 100;

			using (AdministrationEntities entities = new AdministrationEntities())
			{
				UserPolicy userPolicy = GetCurrentUserPolicy(entities);
				userPolicy.PinCode = model.PinCode;
				userPolicy.AvailableEgeCheckCount = model.AvailableEgeCheckCount.Value;
				entities.SaveChanges();
			}

			ViewBag.PinCodeStatus = "Получен новый проверочный код.";
			return View("Personal", model);
		}

		private static UserPolicy GetCurrentUserPolicy(AdministrationEntities entities)
		{
			string userName = UserHelper.GetAuthenticatedUserName();
			return entities.UserPolicy.First(x => x.UserName == userName);
		}

		[Authorize]
		[HttpPost, CallByParameter]
		public ActionResult ChangePassword(PersonalModel model)
		{
			if (ModelState.IsValid)
			{
				if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
				{
					ViewBag.PasswordChangeStatus = "Ваш пароль был успешно изменен.";
				}
				else
				{
					AddModelError("Текущий пароль или новый пароль указаны неверно.");
				}	
			}

			// If we got this far, something failed, redisplay form
			// ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

			AddPinCodeToModel(model);
			return View("Personal", model);
		}

		public ActionResult AuthError(int? statusID)
		{
			if (statusID.HasValue)
				ViewData["StatusID"] = statusID.Value;
			return View("AuthError");
		}

		public ActionResult AjaxError(int? statusID)
		{
			return View("AjaxError");
		}

		public ActionResult AuthRedirect()
		{
			return View("AuthRedirect");
		}

		[Authorize]
		public ActionResult DefaultPage()
		{
			if (User.IsInRole(UserRole.FBDAdmin) || User.IsInRole(UserRole.RonAdmin))
			{
				Response.Redirect("~/Administration/EduList");
			}
			else if (User.IsInRole(UserRole.FbdRonUser))
			{
				Response.Redirect("~/InstitutionAdmin/List");
			}
            else if (User.IsInRole(UserRole.FbdAuthorizedStaffOlympic))
            {
                Response.Redirect("~/OlympicDiplomant/Index");
            }
            else
            {
                Response.Redirect("~/InstitutionApplication/ApplicationList");
            }

			Response.End();
			return new EmptyResult();
		}
	}
}