using System;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels;
using GVUZ.Model.Helpers;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с расширенным списком заявлений
	/// </summary>
	public static class ExtendedApplicationListExtensions
	{
		private static readonly int ExtendedApplicationListPageSize = AppSettings.Get("Search.PageSize", 25);

        public static void ClearFilterDataCache(int institutionID)
        {
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            cache.RemoveAllWithPrefix("ExtendedApplicationList_FilterData_" + institutionID);
        }

		/// <summary>
		/// Загружаем список заявлений
		/// </summary>
		public static ExtendedApplicationListViewModel FillExtendedApplicationList(this EntrantsEntities dbContext, ExtendedApplicationListViewModel model, bool loadFilterData, int institutionID, string fastSearchTerm = null, bool usePaging = true)
		{
            var cache = ServiceLocator.Current.GetInstance<ICache>();
			//если грузим фильтр, то вытаскиваем данные для него
			if (loadFilterData)
			{
                var filter = cache.Get<ExtendedApplicationListViewModel>("ExtendedApplicationList_FilterData_" + institutionID, null);
                if (filter != null)
                {
                    model = filter;
                }
                else
                {
                    model.CompetitiveGroups = dbContext.CompetitiveGroup.Where(x => x.InstitutionID == institutionID).Select(x => new { ID = x.CompetitiveGroupID, Name = x.Name }).ToArray();
                    model.ApplicationStatuses = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.ApplicationStatusType).OrderBy(x => x.Key).Select(x => new { ID = x.Key, Name = x.Value }).ToArray();
                    cache.Insert("ExtendedApplicationList_FilterData_" + institutionID, model);
                }
			}

			IQueryable<Application> tq = dbContext.Application;
			//если быстрый поиск, то пробуем разобраться, это фио или номер
            //новая логика
			if (!String.IsNullOrEmpty(fastSearchTerm))
			{
				if (model.Filter == null)
					model.Filter = new ExtendedApplicationListViewModel.FilterDetails();

                bool isNumber = false;

                foreach (char c in fastSearchTerm)
                {
                    isNumber = Char.IsDigit(c);
                    if (isNumber) break;
                }
                
                if (isNumber)
					model.Filter.ApplicationNumber = fastSearchTerm;
				else
				{
					var split = fastSearchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					if (split.Length > 0)
						model.Filter.EntrantLastName = split[0];
					if (split.Length > 1)
						model.Filter.EntrantFirstName = split[1];
					if (split.Length > 2)
						model.Filter.EntrantMiddleName = split[2];
				}
			}

			if (loadFilterData) return model; //нам больше не нужны заявления на данном шаге
			int? sortID = model.SortID;
			if (!sortID.HasValue)
				sortID = 1;
			if (true)
			{
				if (sortID.Value == 1) tq = tq.OrderBy(x => x.ApplicationNumber);
				if (sortID.Value == -1) tq = tq.OrderByDescending(x => x.ApplicationNumber);

				if (sortID.Value == 2) tq = tq.OrderBy(x => x.RegistrationDate);
				if (sortID.Value == -2) tq = tq.OrderByDescending(x => x.RegistrationDate);

				if (sortID.Value == 3) tq = tq.OrderBy(x => x.CompetitiveGroup.Name);
				if (sortID.Value == -3) tq = tq.OrderByDescending(x => x.CompetitiveGroup.Name);

				if (sortID.Value == 4) tq = tq.OrderBy(x => x.ApplicationStatusType.Name);
				if (sortID.Value == -4) tq = tq.OrderByDescending(x => x.ApplicationStatusType.Name);

				if (sortID.Value == 5)
					tq = tq.OrderBy(x => x.Entrant.LastName)
						.ThenBy(x => x.Entrant.FirstName)
						.ThenBy(x => x.Entrant.MiddleName);
				if (sortID.Value == -5)
					tq = tq.OrderByDescending(x => x.Entrant.LastName)
						.ThenByDescending(x => x.Entrant.FirstName)
						.ThenByDescending(x => x.Entrant.MiddleName);

				if (sortID.Value == 6)
					tq = tq.OrderBy(x => x.Entrant.EntrantDocument_Identity.DocumentSeries)
						.ThenBy(x => x.Entrant.EntrantDocument_Identity.DocumentNumber);
				if (sortID.Value == -6)
					tq = tq.OrderByDescending(x => x.Entrant.EntrantDocument_Identity.DocumentSeries)
						.ThenByDescending(x => x.Entrant.EntrantDocument_Identity.DocumentNumber);
			}

			tq = tq.Where(x => x.InstitutionID == institutionID);
			model.TotalItemCount = tq.Count();
			if (model.Filter != null)
			{
				if (model.Filter.DateBegin.HasValue)
					tq = tq.Where(x => x.RegistrationDate >= model.Filter.DateBegin);
				if (model.Filter.DateEnd.HasValue)
				{
					DateTime endDateAdj = model.Filter.DateEnd.Value.Date.AddDays(1).AddSeconds(-1);
					tq = tq.Where(x => x.RegistrationDate <= endDateAdj);
				}

				if (model.Filter.CompetitiveGroupID.HasValue && model.Filter.CompetitiveGroupID > 0)
					tq = tq.Where(x => (x.OrderCompetitiveGroupID == model.Filter.CompetitiveGroupID && x.StatusID == ApplicationStatusType.InOrder)
						|| (x.OrderCompetitiveGroupID == model.Filter.CompetitiveGroupID) && x.StatusID != ApplicationStatusType.InOrder);
				if (!String.IsNullOrEmpty(model.Filter.EntrantLastName))
					tq = tq.Where(x => x.Entrant.LastName.Contains(model.Filter.EntrantLastName));
				if (!String.IsNullOrEmpty(model.Filter.EntrantFirstName))
					tq = tq.Where(x => x.Entrant.FirstName.Contains(model.Filter.EntrantFirstName));
				if (!String.IsNullOrEmpty(model.Filter.EntrantMiddleName))
					tq = tq.Where(x => x.Entrant.MiddleName.Contains(model.Filter.EntrantMiddleName));
				if (!String.IsNullOrEmpty(model.Filter.EntrantDocSeries))
					tq = tq.Where(x => x.Entrant.EntrantDocument_Identity.DocumentSeries.Contains(model.Filter.EntrantDocSeries));
				if (!String.IsNullOrEmpty(model.Filter.EntrantDocNumber))
					tq = tq.Where(x => x.Entrant.EntrantDocument_Identity.DocumentNumber.Contains(model.Filter.EntrantDocNumber));
				if (model.Filter.ApplicationStatusID != null && model.Filter.ApplicationStatusID.Length > 0)
					tq = tq.Where(x => model.Filter.ApplicationStatusID.Contains(x.StatusID));

				if (!String.IsNullOrEmpty(model.Filter.ApplicationNumber))
					tq = tq.Where(x => model.Filter.ApplicationNumber == x.ApplicationNumber);
			}

			if (!model.PageNumber.HasValue || model.PageNumber < 0)
				model.PageNumber = 0;
			model.TotalItemFilteredCount = tq.Count();
			model.TotalPageCount = ((Math.Max(model.TotalItemFilteredCount, 1) - 1) / ExtendedApplicationListPageSize) + 1;
			if (usePaging)
				tq = tq.Skip(model.PageNumber.Value * ExtendedApplicationListPageSize).Take(ExtendedApplicationListPageSize);

			var q = tq
				.Select(x => new
				{
					ApplicationDate = x.RegistrationDate,
					ApplicationID = x.ApplicationID,
					ApplicationNumber = x.ApplicationNumber,
					DocSeries = x.Entrant.EntrantDocument_Identity.DocumentSeries,
					DocNumber = x.Entrant.EntrantDocument_Identity.DocumentNumber,
					F = x.Entrant.LastName,
					I = x.Entrant.FirstName,
					O = x.Entrant.MiddleName,
					StatusID = x.StatusID,
					StatusName = x.ApplicationStatusType.Name,
					PublishDate = x.PublishDate,
					OrderCompetitiveGroupName = x.CompetitiveGroup.Name,
					AnyCompetitiveGroupNames = x.CompetitiveGroup.Name,
					EntrantID = x.EntrantID,
					OrderBenefitName = x.Benefit == null ? "" : x.Benefit.ShortName,
					OrderRating = x.OrderCalculatedRating,
					DirectionName = x.OrderCompetitiveGroupItemID == null ? "" : x.CompetitiveGroup.Direction.Name,
					EducationalFormID = x.OrderEducationFormID,
					EducationalSourceID = x.OrderEducationSourceID,
					DocumentReceived = x.OriginalDocumentsReceived,
					ViolationName = x.ViolationType.BriefName,
					ViolationErrors = x.ViolationErrors,
					ViolationID = x.ViolationID,
					StatusDecision = x.StatusDecision,
					LastDenyDate = x.LastDenyDate,
				}).ToArray();
			model.Applications = q.Select(x => new ExtendedApplicationListViewModel.ApplicationData
			{
				ApplicationDate = x.ApplicationDate.ToString("dd.MM.yyyy"),
				ApplicationID = x.ApplicationID,
				ApplicationNumber = String.IsNullOrWhiteSpace(x.ApplicationNumber) ? "не задан" : x.ApplicationNumber,
				EntrantDocData = x.DocSeries + " " + x.DocNumber,
				EntrantID = x.EntrantID,
				EntrantFIO = x.F + " " + x.I + " " + x.O,
				StatusID = x.StatusID,
				CompetitiveGroupName = x.StatusID == ApplicationStatusType.InOrder ? (x.OrderCompetitiveGroupName ?? "") 
						: (String.Join(", ", x.AnyCompetitiveGroupNames)),
				StatusName = x.StatusName,
				Benefit = x.StatusID == ApplicationStatusType.InOrder ? x.OrderBenefitName : "",
				Rating = x.StatusID == ApplicationStatusType.InOrder ? x.OrderRating.ToString() : "",
				DirectionName = x.DirectionName,
				EducationalSource = x.EducationalFormID.HasValue ? DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.Study, x.EducationalFormID.Value) : "",
				EducationalForm = x.EducationalFormID.HasValue ? DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.Study, x.EducationalFormID.Value) : "",
				OriginalDocumentsRecieved = x.DocumentReceived,
				ViolationName = x.StatusID == ApplicationStatusType.Failed || x.StatusID == ApplicationStatusType.Denied ?
					x.ViolationName + ((x.ViolationID == 3 && !String.IsNullOrWhiteSpace(x.StatusDecision)) ? " (" + x.StatusDecision + ")" : "")
							+ ((x.ViolationID == 2 && !String.IsNullOrWhiteSpace(x.ViolationErrors)) ? " (" + x.ViolationErrors + ")" : "") : "",
				DenyDate = x.LastDenyDate ?? DateTime.MinValue
			}).ToArray();
			dbContext.AddApplicationAccessToLog(model.Applications.Select(x => new PersonalDataAccessLogger.AppData
				{
					ApplicationNumber = x.ApplicationNumber,
					ApplicationUID = x.ApplicationUID,
					ApplicationID = x.ApplicationID,
					ApplicationDate = x.ApplicationDateDate,
					EntrantID = x.EntrantID
				}).ToArray(), "ExtendedApplicationList", institutionID);
			return model;
		}
	}
}