using System.Linq;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Model.Helpers;
using GVUZ.Web.Controllers;

namespace GVUZ.Web.Filters
{
	/// <summary>
	/// Атрибут, добавляющий имя пользователя в контекст, чтобы можно было выводить на страницах, не записывая в отдельную модель
	/// </summary>
	public class EntrantNameFilterAttribute : ActionFilterAttribute
	{
		public const string EntrantFullName = "Entrant.FullName";
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);

			var applicationController = context.Controller as ApplicationController;
			if (applicationController == null) return;

			if (applicationController.ApplicationID > 0)
			{
				using (EntrantsEntities entities = new EntrantsEntities())
				{
					Application application = entities.GetApplication(applicationController.ApplicationID);
					if (application == null) return;

					var person = (from e in entities.Entrant
								where e.EntrantID == application.EntrantID
					            select new { e.FirstName, e.LastName, e.MiddleName, e.EntrantID, e.InstitutionID, EntrantUID = e.UID }).FirstOrDefault();
					if (person != null)
					{
						string fio = NameHelper.GetFullName(person.FirstName, person.LastName, person.MiddleName);
						if (!string.IsNullOrWhiteSpace(fio))
							context.Controller.ViewData[EntrantFullName] = fio;

						entities.AddEntrantAccessToLog(new[] { new PersonalDataAccessLogger.AppData { EntrantID = person.EntrantID, EntrantUID = person.EntrantUID } },
							"EntrantInfo", person.InstitutionID ?? 0, person.EntrantID);
					}
				}
			}
		}
	}
}