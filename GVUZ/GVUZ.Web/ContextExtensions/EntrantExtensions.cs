using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Web.ViewModels;
using GVUZ.Model.Entrants.ContextExtensions;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы работы с абитуриентами
	/// </summary>
	public static class EntrantExtensions
	{
		/// <summary>
		/// Взять id настоящего абитуриента
		/// </summary>
		public static int GetEntrantID(this EntrantsEntities dbContext, UserInfo userInfo)
		{
			return dbContext.Entrant.Where(x => x.SNILS == userInfo.SNILS).Select(x => x.EntrantID).FirstOrDefault();
		}

		/// <summary>
		/// Сохраняем уид абитуриента
		/// </summary>
		public static AjaxResultModel SaveEntrantUid(this EntrantsEntities dbContext, int entrantId, int institutionID, string uid)
		{
			Entrant entrant = dbContext.Entrant.FirstOrDefault(x => x.EntrantID == entrantId && x.InstitutionID == institutionID);
			if (entrant == null)
				return new AjaxResultModel().SetIsError("Uid", "Карточка абитуриента не найдена.");
			if (uid != null)
			{
				if (dbContext.Entrant.Any(x => x.EntrantID != entrantId && x.InstitutionID == institutionID && x.UID == uid))
					return new AjaxResultModel().SetIsError("Uid", "Уже существует абитуриент с данным идентификатором.");
			}

			entrant.UID = String.IsNullOrEmpty(uid) ? null : uid;

			dbContext.SaveChanges();

			return new AjaxResultModel("");
		}

		/// <summary>
		/// Загружаем модель с информацией абитуриента
		/// </summary>
		public static EntrantInfoViewModelC GetEntrantInfo(this EntrantsEntities dbContext, int entrantID, int institutionID)
		{
			var queryable = from en in dbContext.Entrant
			                join d in dbContext.EntrantDocument on en.IdentityDocumentID equals d.EntrantDocumentID
							where en.EntrantID == entrantID && en.InstitutionID == institutionID
							select en;

			var e = queryable.FirstOrDefault();
			if (e == null)
				return new EntrantInfoViewModelC();

			//заявления абитуриента
			ApplicationData[] applications = dbContext.Application
				.Where(x => x.InstitutionID == institutionID && x.EntrantID == e.EntrantID)
				.Select(x => new 
				             	{
				             	ApplicationID = x.ApplicationID,
				             	ApplicationNumber = x.ApplicationNumber ?? "",
				             	OrderCompetitiveGroup = (x.CompetitiveGroup == null ? "" : x.CompetitiveGroup.Name),
								AnyCompetitiveGroups = x.CompetitiveGroup.Name,
				             	ApplicationRegistrationDateTime = x.RegistrationDate,
				             	ApplicationStatus = x.ApplicationStatusType.Name,
				             	ApplicationStatusID = x.ApplicationStatusType.StatusID,
				             	ApplicationBenefit = x.Benefit == null ? "Нет" : x.Benefit.ShortName,
								AnyBenefit = x.Benefit.ShortName,
				             	ApplicationRating = x.OrderCalculatedRating,
								AnyRatings = x.OrderCalculatedRating,
								StatusID = x.StatusID
				             }).ToArray()
				.Select(x =>
				    {
                        var campaignName = dbContext.Application.Where(c =>
                            c.ApplicationID == x.ApplicationID).Select(c => c.CompetitiveGroup.Campaign.Name).FirstOrDefault();

				        return new ApplicationData
				        {
				            ApplicationID = x.ApplicationID,
				            ApplicationNumber = x.ApplicationNumber ?? "",
				            ApplicationCompetitiveGroup = x.StatusID == ApplicationStatusType.InOrder ? (x.OrderCompetitiveGroup) : (String.Join(", ", x.AnyCompetitiveGroups)),
				            ApplicationRegistrationDateTime = x.ApplicationRegistrationDateTime,
				            ApplicationStatus = x.ApplicationStatus,
				            ApplicationStatusID = x.ApplicationStatusID,
				           // ApplicationBenefit = x.StatusID == ApplicationStatusType.InOrder ? x.ApplicationBenefit : (x.AnyBenefit.Count() == 1 ? x.AnyBenefit.First() : ""),
				            //ApplicationRating = x.StatusID == ApplicationStatusType.InOrder ? (x.ApplicationRating ?? 0m) : (x.AnyRatings.Count() == 1 ? x.AnyRatings.First() ?? 0m : 0),
                            ApplicationCampaign = campaignName
				        };
				    }).ToArray();

			var identityDocumentViewModel = new JavaScriptSerializer().Deserialize<IdentityDocumentViewModel>(e.EntrantDocument_Identity.DocumentSpecificData);
            EntrantDocumentExtensions.ConvertDatesToLocal(identityDocumentViewModel);
            identityDocumentViewModel.FillData(dbContext, true, null, null);
			dbContext.AddEntrantAccessToLog(e, "EntrantInfo");
			return new EntrantInfoViewModelC
			       	{
				Applications = applications,
				EntrantID = e.EntrantID,
				LastName = e.LastName,
				FirstName = e.FirstName,
				MiddleName = e.MiddleName,
				Uid = e.UID,
				BirthDateTime = identityDocumentViewModel.BirthDate,
				BirthPlace = identityDocumentViewModel.BirthPlace,
				Gender = identityDocumentViewModel.GenderTypeName,
				DocumentType = identityDocumentViewModel.IdentityDocumentTypeName,
				DocumentSeriesNumber = identityDocumentViewModel.DocumentSeries + " " + identityDocumentViewModel.DocumentNumber
			};			
		}

		public static readonly int EntrantListPageSize = AppSettings.Get("Search.PageSize", 25);

        public static void ClearFilterDataCache(int institutionID)
        {
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            cache.RemoveAllWithPrefix("EntrantsList_FilterData_" + institutionID);
        }

		/// <summary>
		/// Список абитуриентов
		/// </summary>
		public static EntrantsListViewModel GetEntrantsList(this EntrantsEntities dbContext, int institutionID, EntrantsListViewModel model, bool loadFilter)
		{
            var cache = ServiceLocator.Current.GetInstance<ICache>();
			if (!loadFilter)
			{
				//считаем общее количество без фильтра
                var total = cache.Get("EntrantsList_TotalItemCount_" + institutionID, -1);
                if (total != -1)
                {
                    model.TotalItemCount = total;
                }
                else
                {
                    model.TotalItemCount = dbContext.Entrant.Count(x => x.IdentityDocumentID != null && x.Application.Any(z => z.InstitutionID == institutionID));
                    cache.Insert("EntrantsList_TotalItemCount_" + institutionID, model.TotalItemCount);
                }

			    var tmpAppQuery = dbContext.Application.Where(x => x.InstitutionID == institutionID);
				//////////отфильтровываем заявления, чтобы потом срезать энтрантов
				if (model.Filter != null)
				{
					if (!String.IsNullOrEmpty(model.Filter.ApplicationNumber))
						tmpAppQuery = tmpAppQuery.Where(x => model.Filter.ApplicationNumber == x.ApplicationNumber);
					if (model.Filter.ApplicationStatusID != null && model.Filter.ApplicationStatusID.Length > 0)
						tmpAppQuery = tmpAppQuery.Where(x => model.Filter.ApplicationStatusID.Contains(x.StatusID));
					if (model.Filter.DateBegin.HasValue)
						tmpAppQuery = tmpAppQuery.Where(x => x.RegistrationDate >= model.Filter.DateBegin);
					if (model.Filter.DateEnd.HasValue)
					{
						DateTime endDateAdj = model.Filter.DateEnd.Value.Date.AddDays(1).AddSeconds(-1);
						tmpAppQuery = tmpAppQuery.Where(x => x.RegistrationDate <= endDateAdj);
					}

                    if (!String.IsNullOrEmpty(model.Filter.CompetitiveGroupName))
                    {
                        if (!String.IsNullOrEmpty(model.Filter.CampaignName))
                            tmpAppQuery =
                                tmpAppQuery.Where(
                                    x =>
                                    x.CompetitiveGroup.Campaign.Name == model.Filter.CampaignName);

                        tmpAppQuery =
                            tmpAppQuery.Where(
                                x =>
                                x.CompetitiveGroup.Name == model.Filter.CompetitiveGroupName);
                    }
				    if (!String.IsNullOrEmpty(model.Filter.CampaignName) && String.IsNullOrEmpty(model.Filter.CompetitiveGroupName)) 
                    {   
                        List<int> Itemp = new List<int>();
                        //var tempCG = tmpAppQuery.Select(x => x.ApplicationSelectedCompetitiveGroup);
                        //foreach (var p in tempCG)
                        //{
                        //    Itemp.AddRange(p.Where(x => x.CompetitiveGroup.Campaign.Name == model.Filter.CampaignName)
                        //            .Select(x => x.Application.ApplicationID));
                        //}
                        //tmpAppQuery = tmpAppQuery.Where(x => Itemp.Contains(x.ApplicationID));
                    }
				}

				//тянем энтрантов с нужными заявлениями
				var query = from x in dbContext.Entrant
				            join d in dbContext.EntrantDocument on x.IdentityDocumentID equals d.EntrantDocumentID
				            where (tmpAppQuery.Where(z => z.EntrantID == x.EntrantID)).Any()
				            select new { E = x,  D = d };
				///////////сортируем
				if (!model.SortID.HasValue)
					model.SortID = 2;
				switch (model.SortID.Value)
				{
					case 1:
						query = query.OrderBy(x => x.E.EntrantID);
						break;
					case -1:
						query = query.OrderByDescending(x => x.E.EntrantID);
						break;
					case 2:
						query =
							query.OrderBy(x => x.E.LastName).ThenBy(x => x.E.FirstName);
						break;
					case -2:
						query = query.OrderByDescending(x => x.E.LastName)
							.ThenByDescending(x => x.E.FirstName);
						break;
					case 3:
						query = query.OrderBy(x => x.E.EntrantDocument_Identity.DocumentSeries + x.D.DocumentNumber);
						break;
					case -3:
						query = query.OrderByDescending(x => x.D.DocumentSeries + x.D.DocumentNumber);
						break;
					default:
						throw new ArgumentException("sortId");
				}
				///////////фильтруем энтрантов
				if (model.Filter != null)
				{
					if (!String.IsNullOrEmpty(model.Filter.EntrantLastName))
						query = query.Where(x => x.E.LastName.Contains(model.Filter.EntrantLastName));
					if (!String.IsNullOrEmpty(model.Filter.EntrantFirstName))
						query = query.Where(x => x.E.FirstName.Contains(model.Filter.EntrantFirstName));
					if (!String.IsNullOrEmpty(model.Filter.EntrantMiddleName))
						query = query.Where(x => x.E.MiddleName.Contains(model.Filter.EntrantMiddleName));
					if (!String.IsNullOrEmpty(model.Filter.EntrantDocSeries))
						query = query.Where(x => x.E.EntrantDocument_Identity.DocumentSeries.Contains(model.Filter.EntrantDocSeries));
					if (!String.IsNullOrEmpty(model.Filter.EntrantDocNumber))
						query = query.Where(x => x.E.EntrantDocument_Identity.DocumentNumber.Contains(model.Filter.EntrantDocNumber));
				}

				///////////материализуем энтрантов
				var pageNumber = model.PageNumber;
				if (!pageNumber.HasValue || pageNumber < 0) pageNumber = 0;

				int totalCount = query.Count();
				model.TotalItemFilteredCount = totalCount;
				model.TotalPageCount = ((Math.Max(totalCount, 1) - 1) / EntrantListPageSize) + 1;

				var entrants = query
					.Skip(pageNumber.Value * EntrantListPageSize)
					.Take(EntrantListPageSize).ToArray();

				int[] entrantIDs = entrants.Select(x => x.E.EntrantID).ToArray();
				//////тянем заявления ещё раз, уже отдельно по существующим энтрантам
				var queryApp = from a in dbContext.Application
				               where a.InstitutionID == institutionID && entrantIDs.Contains(a.EntrantID)
				               select a;

				//////////отфильтровываем ненужные
				if (model.Filter != null)
				{
					if (!String.IsNullOrEmpty(model.Filter.ApplicationNumber))
						queryApp = queryApp.Where(x => model.Filter.ApplicationNumber == x.ApplicationNumber);
					if (model.Filter.ApplicationStatusID != null && model.Filter.ApplicationStatusID.Length > 0)
						queryApp = queryApp.Where(x => model.Filter.ApplicationStatusID.Contains(x.StatusID));
					if (model.Filter.DateBegin.HasValue)
						queryApp = queryApp.Where(x => x.RegistrationDate >= model.Filter.DateBegin);
					if (model.Filter.DateEnd.HasValue)
					{
						DateTime endDateAdj = model.Filter.DateEnd.Value.Date.AddDays(1).AddSeconds(-1);
						queryApp = queryApp.Where(x => x.RegistrationDate <= endDateAdj);
					}

                    if (!String.IsNullOrEmpty(model.Filter.CompetitiveGroupName))
						queryApp = queryApp.Where(x => x.CompetitiveGroup.Name == model.Filter.CompetitiveGroupName);
				}

				///////////filling
				model.Entrants = new EntrantsListViewModel.EntrantData[entrants.Count()];
				var apps = queryApp.OrderBy(x => x.ApplicationNumber).ToArray();
				var i = 0;
				foreach (var entrant in entrants)
				{
					var applications = apps
                        .Where(x => x.EntrantID == entrant.E.EntrantID)
                          .Select(x => new ApplicationData
                          {
                              ApplicationID = x.ApplicationID,
                              ApplicationNumber = x.ApplicationNumber ?? "",
                              ApplicationCompetitiveGroup = x.ApplicationStatusType.StatusID == ApplicationStatusType.InOrder ?
                                  (x.CompetitiveGroup == null ? "" : x.CompetitiveGroup.Name) : String.Join(", ", x.CompetitiveGroup.Name),
                              ApplicationRegistrationDateTime = x.RegistrationDate,
                              ApplicationStatus = x.ApplicationStatusType.Name,
                              ApplicationStatusID = x.ApplicationStatusType.StatusID,
                              ApplicationCampaign = String.Join(", ", x.CompetitiveGroup.Campaign.Name)
                          }).ToArray();

					model.Entrants[i++] = new EntrantsListViewModel.EntrantData
					                      {
					                      	Applications = applications,
					                      	EntrantID = entrant.E.EntrantID,
											EntrantUID = entrant.E.UID,
					                      	FullName = entrant.E.LastName + " "
					                      	           + entrant.E.FirstName + " "
					                      	           + entrant.E.MiddleName,
					                      	IdentityDocument = entrant.D == null
					                      	                   	? ""
					                      	                   	: entrant.D.DocumentSeries
					                      	                   	  + " " + entrant.D.DocumentNumber
					                      };
				}
			}

			if (loadFilter)
			{
                var filter = cache.Get<EntrantsListViewModel>("EntrantsList_FilterData_" + institutionID, null);
                if (filter != null)
                    return filter;

                model.Campaigns = dbContext.Campaign.Where(x => x.InstitutionID == institutionID).Select(x => new { ID = x.CampaignID, x.Name }).ToArray();
                var currcams = dbContext.Campaign.Where(x => x.InstitutionID == institutionID).Select(y => y.CampaignID).ToList();
				model.CompetitiveGroups = dbContext.CompetitiveGroup.Where(x => x.InstitutionID == institutionID).Where(x => currcams.Contains(x.CampaignID ?? -25)).Select(x => new { ID = x.CompetitiveGroupID, x.Name }).ToArray();
                model.ApplicationStatuses = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.ApplicationStatusType).OrderBy(x => x.Key).Select(x => new { ID = x.Key, x.Value }).ToArray();

                cache.Insert("EntrantsList_FilterData_" + institutionID, model);
			    return model;
			}

			if (model.Entrants != null)
				dbContext.AddEntrantAccessToLog(model.Entrants.Where(x => x != null)
					.Select(x => new PersonalDataAccessLogger.AppData { EntrantID = x.EntrantID, EntrantUID = x.EntrantUID }).ToArray(),
					"EntrantList", institutionID);

			return model;
		}


        /// <summary>
        /// Проверка выполнения условия наличия минимально необходимых
        /// баллов ЕГЭ для использования общей льготы "Без вступительных испытаний"
        /// </summary>
        /// <param name="dbContext">Контекст БД</param>
        /// <param name="competitiveGroupID">Код конкурсной группы</param>
        /// <param name="applicationID">Код заявления</param>
        /// <param name="entrantDocumentID">Код приложенного документа, для которого производится проверка</param>
        /// <returns>Список кодов испытаний конкурсной группы, для которых проверка завершилась неудачно</returns>
        public static List<int> CheckEGEScoresForCommonOlympics(this EntrantsEntities dbContext, int competitiveGroupID, int applicationID, int entrantDocumentID)
        {
            Dictionary<int, string> errorMessages = new Dictionary<int, string>();

            return CheckEGEScoresForCommonOlympics(dbContext, competitiveGroupID, applicationID, entrantDocumentID, out errorMessages);
        }

        /// <summary>
        /// Проверка выполнения условия наличия минимально необходимых
        /// баллов ЕГЭ для использования общей льготы "Без вступительных испытаний"
        /// </summary>
        /// <param name="dbContext">Контекст БД</param>
        /// <param name="competitiveGroupID">Код конкурсной группы</param>
        /// <param name="applicationID">Код заявления</param>
        /// <param name="entrantDocumentID">Код приложенного документа, для которого производится проверка</param>
        /// <param name="errorMessages">Список сообщений об ошибках</param>
        /// <returns>Список кодов испытаний конкурсной группы, для которых проверка завершилась неудачно</returns>
        public static List<int> CheckEGEScoresForCommonOlympics(this EntrantsEntities dbContext, int competitiveGroupID, int applicationID, int entrantDocumentID, out Dictionary<int, string> errorMessages)
        {
            errorMessages = new Dictionary<int, string>();
            // Получим результаты ЕГЭ по всем испытаниям
            var AllEgeResults = dbContext.ApplicationEntranceTestDocument.Where(x => x.ApplicationID == applicationID)
                .Select(x => new
                {
                    egeResult = x.ResultValue,
                    SubjectID = x.SubjectID,
                    ItemId = x.EntranceTestItemID,
                    SubjectName = x.Subject.Name
                }).ToList();

            //Выберем все ВИ для данной КГ.
            var items = dbContext.EntranceTestItemC.Where(x => x.CompetitiveGroupID == competitiveGroupID).Select(x => x.EntranceTestItemID).ToArray();

            // Отсечём те результаты ЕГЭ, что не относятся к данной КГ
            var EgeResults = AllEgeResults.Where(x => x.ItemId.HasValue && items.Contains(x.ItemId.Value)).ToList();

            //Получим документ на льготу
            BaseDocumentViewModel baseDoc = dbContext.LoadEntrantDocument(entrantDocumentID);
            baseDoc.FillData(dbContext, true, null, null);

            OlympicDocumentViewModel olympDoc = baseDoc as OlympicDocumentViewModel;

            if (olympDoc == null)
                return new List<int>();

            // Документ - правильный, из него можно взять олимпиаду. Льгота - корректна, можно дальше обрабатывать
            // Получим ВСЕ общие льготы для конкурсной группы
            var groupBenefits = dbContext.BenefitItemC.Where(x => x.CompetitiveGroupID == competitiveGroupID && !x.EntranceTestItemID.HasValue).ToList();

            BenefitItemSubject[] subjects = null;

            if (groupBenefits.Count == 0)
                return new List<int>();

            // Если общие льготы есть - продолжаем
            // Проверим, есть ли льгота именно для этой олимпиады.
            // Если есть - она приоритетна.
            // Логика тут такая - ВУЗ может одной олимпиаде, например, своей, дать большие преференции, чем другим олимпиадам
            // Тогда в списке льгот будет 2 элемента - льгота для конкретной олимпиады О1 с набором баллов Б1
            // и льгота для всех олимпиад с набором баллов Б2. Если приложен документ с олимпиады О1, то используется 
            // набор баллов Б1, в случае любой другой олимпиады - набор Б2.

            var olympicsBenefitItemIds = dbContext.BenefitItemCOlympicType.Where(x => x.OlympicTypeID == olympDoc.OlympicID).Select(x => x.BenefitItemID);

            if (olympicsBenefitItemIds.Count() != 0) // Что-то есть, можно брать данные
            {
                var benefitItemToUse = groupBenefits.FirstOrDefault(x => olympicsBenefitItemIds.Contains(x.BenefitItemID));
                if (benefitItemToUse != null)
                {
                    // Нашли и будем его использовать для получения баллов
                    subjects = dbContext.BenefitItemSubject.Where(x => x.BenefitItemId == benefitItemToUse.BenefitItemID).ToArray();
                }
                else
                {
                    // Для конкретной олимпиады данных нет - ищем для всех
                    var commonBenefit = groupBenefits.FirstOrDefault(x => x.IsForAllOlympic);
                    if (commonBenefit != null) // Есть льгота для всех олимпиад
                    {
                        subjects = dbContext.BenefitItemSubject.Where(x => x.BenefitItemId == commonBenefit.BenefitItemID).ToArray();
                    }
                }
            }
            else
            {
                // Для конкретной олимпиады данных нет - ищем для всех
                var commonBenefit = groupBenefits.FirstOrDefault(x => x.IsForAllOlympic);
                if (commonBenefit != null) // Есть льгота для всех олимпиад
                {
                    subjects = dbContext.BenefitItemSubject.Where(x => x.BenefitItemId == commonBenefit.BenefitItemID).ToArray();
                }
            }

            if (subjects != null && subjects.Count() > 0) // Баллы, которые надо проверить, найдены
            {
                if (EgeResults.Count > 0 && EgeResults.Any(x => x.egeResult.HasValue))
                {
                    List<int> errorSubjects = new List<int>();
                    foreach (var res in EgeResults)
                    {
                        if (!res.egeResult.HasValue) continue;

                        int? minValue = subjects.Where(x => x.SubjectId == res.SubjectID).Select(x => x.EgeMinValue).FirstOrDefault();
                        if (minValue.HasValue && res.egeResult.Value < (decimal)minValue)
                        {
                            errorSubjects.Add(res.ItemId.Value);
                            errorMessages.Add(res.ItemId.Value, string.Format("Для использования льготы минимальный балл по предмету {0} не может быть меньше {1} баллов", res.SubjectName, minValue));
                        }
                    }

                    if (errorSubjects.Count > 0)
                        return errorSubjects;
                }
            }

            return new List<int>();
        }

        public static bool CheckEGEScoreForSubject(this EntrantsEntities dbContext, int testItemID, int applicationID, int olympicTypeID)
        {
            // Возьмём всё льготы по предмету.
            var benefits = dbContext.BenefitItemC.Where(x => x.EntranceTestItemID == testItemID).ToList();

            // Среди них попробуем найти ту, которая соответствует конкретной олимпиаде
            // Для начала отберём все олимпиады, непосредственно связанные с льготами
            var benefitsForSelectedOlympics = dbContext.BenefitItemCOlympicType.Where(
                x => x.OlympicTypeID == olympicTypeID).Select( x => x.BenefitItemID).ToList();

            // Отберём льготу(ы), связанные с конкретной олимпиадой.
            // По логике вещей длина данного списка может быть либо 0, либо 1
            var olympicBenefits = benefits.Where(x => !x.IsForAllOlympic && benefitsForSelectedOlympics.Contains(x.BenefitItemID)).ToList();

            BenefitItemC benefitToCheck = null;

            if (olympicBenefits.Count == 0)
            {
                // Льготы для конкретной олимпиады нет, ищем льготу по предмету для всех олимпиад
                // По логике вещей она должна быть только одна, если больше - берём первую
                benefitToCheck = benefits.FirstOrDefault(x => x.IsForAllOlympic);
            }
            else
            {
                // Льгота для конкретной олимпиады есть.
                // по логике вещей она должна быть только одна, но на всякий случай - берём только первую
                benefitToCheck = olympicBenefits.FirstOrDefault();
            }

            if (benefitToCheck == null) // На всякий случай проверим - есть ли что-нибудь
                return true; // и если вдруг нет, то вернём ответ, что всё хорошо.

            // Возьмём результаты ЕГЭ по испытанию.
            // Поскольку льгота приравняла баллы к 100, берём из нового поля
            var egeResultToCheck = dbContext.ApplicationEntranceTestDocument.Where(x => x.ApplicationID == applicationID && x.EntranceTestItemID == testItemID)
                .Select(x => x.EgeResultValue).FirstOrDefault();

            // Проверяем
            if (!egeResultToCheck.HasValue || !benefitToCheck.EgeMinValue.HasValue) // Чего-то нет - либо балла ЕГЭ, либо минимально необходимого балла
                return true; // Всё хорошо, ибо нечего проверять

            return (!(egeResultToCheck.Value < (decimal)benefitToCheck.EgeMinValue.Value)); // Меньше - всё плохо. Больше или равно - хорошо. Это и скажем.
        }
	}
}
