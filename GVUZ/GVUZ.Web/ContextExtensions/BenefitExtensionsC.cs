using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model.Applications;
using GVUZ.Model.Benefits;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels;
using BenefitItemC = GVUZ.Model.Benefits.BenefitItemC;
using BenefitItemCOlympicType = GVUZ.Model.Benefits.BenefitItemCOlympicType;
using System.Text;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Льготы конкурсной группы. Суффикс "C" исторически получился
	/// </summary>
	public static class BenefitExtensionsC
	{
		/// <summary>
		/// Загружаем льготу
		/// </summary>
		public static AddBenefitViewModelC LoadBenefitItem(this BenefitsEntities dbContext, AddBenefitViewModelC model)
		{
			BenefitItemC item = null;
			if (model.BenefitItemID > 0)
				item = dbContext.BenefitItemC.Single(x => x.BenefitItemID == model.BenefitItemID);

            model.OlympicYears = dbContext.OlympicType.Select(c => c.OlympicYear).Distinct().OrderByDescending(c => c).ToArray();
			if (item != null)
			{
				model.BenefitTypeID = item.BenefitID;
				model.DiplomaTypeID = item.OlympicDiplomTypeID;
				model.IsForAllOlympic = item.IsForAllOlympic;
				model.IsProfileSubject = item.IsProfileSubject;
				model.OlympicLevelFlags = item.OlympicLevelFlags;
				model.AttachedOlympic = dbContext.BenefitItemCOlympicType
							.Where(x => x.BenefitItemID == model.BenefitItemID)
							.Select(x => x.OlympicTypeID)
							.ToArray();
				model.EntranceTestItemID = item.EntranceTestItemID ?? 0;
				model.CompetitiveGroupID = item.CompetitiveGroupID;
				model.UID = item.UID;
                model.OlympicYearID = item.OlympicYear;
                model.MinEgeValue = item.EgeMinValue;
			}
			else
			{
				if (model.CompetitiveGroupID == 0)
					throw new ArgumentNullException("model", "CompetitiveGroupID should be specified");
				model.OlympicLevelFlags = AddBenefitViewModelC.OLYMPIC_ALL;
				model.AttachedOlympic = new int[0];
				model.IsForAllOlympic = true;
                model.OlympicYearID = model.OlympicYears.First();
			}

			int viewType = 3; //ЕГЭ

            model.SubjectID = 0;
			//прячем профильный предмет у олимпиады, в зависимости от типа ВИ
			using (EntrantsEntities eContext = new EntrantsEntities())
			{
				if (model.EntranceTestItemID > 0)
				{
					var subjectData = eContext.EntranceTestItemC
						.Where(x => x.EntranceTestItemID == model.EntranceTestItemID)
						.Select(x => new { x.SubjectID, x.EntranceTestTypeID })
						.Single();
					if (subjectData.SubjectID.HasValue)
                        model.SubjectID = subjectData.SubjectID.Value;
					if (subjectData.EntranceTestTypeID != EntranceTestType.MainType)
					{
						model.HideProfileSubject = true;
						viewType = 2; //Доп
					}
				}
				else //общее
				{
					model.HideProfileSubject = true;
					viewType = 1; //без ВУ
				}
			}

            //все олимпиады
            model.AllOlympic =
                dbContext.OlympicType
                .Include("OlympicTypeSubjectLink")
                .Include("OlympicTypeSubjectLink.Subject")
                .Where(c => c.OlympicYear == model.OlympicYearID)
                .OrderBy(x => x.OlympicNumber).ToList()
                .Select(x => new AddBenefitViewModelC.OlympicData
                {
                    Level =
                        x.OlympicLevelID != null ?
                        new[] { x.OlympicLevelID.Value } :
                        x.OlympicTypeSubjectLink.Select(c => c.SubjectLevelID.Value).Distinct().ToArray(),
                    Year = x.OlympicYear,
                    Name = 
                        x.OlympicLevelID != null ?
                        x.Name :
                        x.Name + "(" + 
                        string.Join(", ", x.OlympicTypeSubjectLink.Select(c => c.Subject.Name).Distinct().ToArray()) + ")",

                    NumberInt = x.OlympicNumber,
                    OlympicID = x.OlympicID,
                    IsProfileSubject = x.OlympicTypeSubjectLink.Any(y => y.SubjectID == model.SubjectID)
                }).ToArray();

			//подходящие льготы
			var benefitTypes = dbContext.Benefit
				.Where(x => x.BenefitID == viewType)
				.OrderBy(x => x.Name)
				.Select(x => new { ID = x.BenefitID, Name = x.Name }).ToList();
			model.BenefitTypes = benefitTypes;
			model.FirstBenefitTypeName = benefitTypes[0].Name;

            model.BenefitItemSubjects = dbContext.BenefitItemSubject
                .Include("Subject")
                .Where(x => x.BenefitItemId == model.BenefitItemID)
                .Select(x => new BenefitItemSubjectViewModel
                {
                    BenefitItemId = x.BenefitItemId,
                    Id = x.Id,
                    EgeMinValue = x.EgeMinValue,
                    SubjectId = x.SubjectId,
                    SubjectName = x.Subject.Name
                }).ToArray();

            model.AllSubjects = dbContext.Subject
                .Where(x => x.IsEge)
                .Select(x => new GVUZ.Web.ViewModels.AddBenefitViewModelC.Subject
                {
                    Id = x.SubjectID,
                    Name = x.Name
                }).ToArray();


			return model;
		}

		/// <summary>
		/// Проверка, можно ли сохранить льготу
		/// </summary>
		private static string CheckAbilityToSaveBenefitItem(this BenefitsEntities dbContext, BenefitItemC item, int[] attachedOlympicIds)
		{
            /* Если это льгота по предмету и выбран призер - 
             * смотрим есть-ли с такой олимпиадой общая льгота */

            
            //if (item.EntranceTestItemID != null && !item.IsForAllOlympic && item.OlympicLevelFlags != 255)
            //{
            //    /* Если в общей льготе указан победитель и мы создаем в льготе по предмету призера с уровнем не ниже чем в общей - все ок */
            //    /* Если в общей льготе указан победитель и мы создаем в льготе по предмету победителя уровня ниже указанного в общей льготе - все ок */
            //    var commonBenefitByOlympicExists = dbContext.BenefitItemC.FirstOrDefault(x =>
            //        x.CompetitiveGroupID == item.CompetitiveGroupID
            //        && x.BenefitItemID != item.BenefitItemID
            //        && x.OlympicYear == item.OlympicYear
            //        && x.EntranceTestItemID == null
            //        && x.BenefitItemCOlympicType.Select(c => c.OlympicType).Any(m => attachedOlympicIds.Contains(m.OlympicID))
            //        &&
            //            ((x.OlympicLevelFlags >= item.OlympicLevelFlags && item.OlympicDiplomTypeID == 2) ||
            //             (x.OlympicLevelFlags < item.OlympicLevelFlags && item.OlympicDiplomTypeID == 1))
            //
            //        && x.OlympicDiplomTypeID == 1);
            //    if (commonBenefitByOlympicExists != null)
            //        return null;
            //
            //    /* Если в общей льготе указан победитель и мы создаем в льготе по предмету победителя уровня выше или равного в общей льготе - ругаемся */
            //    /* Если в общей льготе указан победитель и мы создаем в льготе по предмету призера уровня ниже чем в общей льготе - ругаемся */
            //    commonBenefitByOlympicExists = dbContext.BenefitItemC.FirstOrDefault(x =>
            //        x.CompetitiveGroupID == item.CompetitiveGroupID
            //        && x.BenefitItemID != item.BenefitItemID
            //        && x.OlympicYear == item.OlympicYear
            //        && x.EntranceTestItemID == null
            //        && x.BenefitItemCOlympicType.Select(c => c.OlympicType).Any(m => attachedOlympicIds.Contains(m.OlympicID))
            //        &&
            //            ((x.OlympicLevelFlags >= item.OlympicLevelFlags && item.OlympicDiplomTypeID == 1) ||
            //             (x.OlympicLevelFlags < item.OlympicLevelFlags && item.OlympicDiplomTypeID == 2))
            //
            //        && x.OlympicDiplomTypeID == 1);
            //    if (commonBenefitByOlympicExists != null)
            //        return "Льгота для победителя олимпиады указана в общей льготе 'Без вступительных испытаний'";
            //}
            

			/* 1. в случае предоставления льготы призерам олимпиады должна быть предоставлена льгота того же 
               или более высокого порядка также и победителям олимпиады */
#warning arzyanin - внимание!! проверяем только общие льготы!!
			if (item.OlympicDiplomTypeID == 2 && item.EntranceTestItemID == null)
			{
				int cnt = dbContext.BenefitItemC.Count(x => 
                    x.CompetitiveGroupID == item.CompetitiveGroupID 
                    && x.OlympicYear == item.OlympicYear 
                    && x.EntranceTestItemID == null
                    //&& ((x.EntranceTestItemID == null && item.EntranceTestItemID == null) || x.EntranceTestItemID == item.EntranceTestItemID)
                    && x.BenefitItemID != item.BenefitItemID
				    && x.OlympicDiplomTypeID != 2
				    && x.BenefitID <= item.BenefitID);
				if (cnt == 0)
					return "Победителям олимпиады должна быть предоставлена льгота того же или более высокого порядка";
			}
			//2. в случае предоставления льготы победителям (призерам) олимпиад III уровня должна быть предоставлена льгота 
			// того же или более высокого порядка также и победителям (призерам) олимпиад I и II уровней;
			if ((item.OlympicLevelFlags & 4) != 0) //III
			{
				if ((item.OlympicLevelFlags & 2) == 0) //не 2
				{
					int cnt = dbContext.BenefitItemC.Count(x => 
                        x.CompetitiveGroupID == item.CompetitiveGroupID
                        && x.OlympicYear == item.OlympicYear 
                        && ((x.EntranceTestItemID == null && item.EntranceTestItemID == null) || x.EntranceTestItemID == item.EntranceTestItemID)
					    && x.BenefitItemID != item.BenefitItemID
					    && (x.OlympicLevelFlags & 2) != 0
					    && x.BenefitID <= item.BenefitID);
					if (cnt == 0)
						return "Для II-ого уровня олимпиад должна быть предоставлена льгота того же или более высокого порядка";
				}

				if ((item.OlympicLevelFlags & 1) == 0) //не 1
				{
					int cnt = dbContext.BenefitItemC.Count(x => 
                        x.CompetitiveGroupID == item.CompetitiveGroupID
                        && x.OlympicYear == item.OlympicYear 
                        && ((x.EntranceTestItemID == null && item.EntranceTestItemID == null) || x.EntranceTestItemID == item.EntranceTestItemID)
					    && x.BenefitItemID != item.BenefitItemID
					    && (x.OlympicLevelFlags & 1) != 0
					    && x.BenefitID <= item.BenefitID);
					if (cnt == 0)
						return "Для I-ого уровня олимпиад должна быть предоставлена льгота того же или более высокого порядка";
				}
			}
			//3. в случае предоставления льготы победителям (призерам) олимпиад II уровня должна быть предоставлена льгота 
			//того же или более высокого порядка также и победителям (призерам) олимпиад I уровня.
			if ((item.OlympicLevelFlags & 2) != 0) //II
			{
				if ((item.OlympicLevelFlags & 1) == 0) //не 1
				{
					int cnt = dbContext.BenefitItemC.Count(x => 
                        x.CompetitiveGroupID == item.CompetitiveGroupID
                        && x.OlympicYear == item.OlympicYear 
                        && ((x.EntranceTestItemID == null && item.EntranceTestItemID == null) || x.EntranceTestItemID == item.EntranceTestItemID)
					    && x.BenefitItemID != item.BenefitItemID
					    && (x.OlympicLevelFlags & 1) != 0
					    && x.BenefitID <= item.BenefitID);
					if (cnt == 0)
						return "Для I-ого уровня олимпиад должна быть предоставлена льгота того же или более высокого порядка";
				}
			}

			return null;
		}

		/// <summary>
		/// Сохраняем льготу
		/// </summary>
		public static AjaxResultModel SaveBenefitItem(this BenefitsEntities dbContext, AddBenefitViewModelC model, int institutionID)
		{
			bool isEdit = model.BenefitItemID > 0;
			BenefitItemC item;
			if (isEdit)
			{
				item = dbContext.BenefitItemC.FirstOrDefault(x => x.BenefitItemID == model.BenefitItemID);
				if (item == null)
					return new AjaxResultModel("Не найдена льгота");
				using (var eContext = new EntrantsEntities())
				{
					if (!eContext.CompetitiveGroup.Any(x => x.CompetitiveGroupID == item.CompetitiveGroupID &&
					                                        (x.Campaign == null || x.Campaign.StatusID != CampaignStatusType.Finished)))
						return new AjaxResultModel("Не найдена льгота");
				}
			}
			else
				item = new BenefitItemC();
			if (!isEdit)
			{
				item.CompetitiveGroupID = model.CompetitiveGroupID;
				if (model.EntranceTestItemID > 0)
					item.EntranceTestItemID = model.EntranceTestItemID;
			}

			if (!model.IsForAllOlympic && (model.AttachedOlympic == null || model.AttachedOlympic.Length == 0))
			{
				return new AjaxResultModel("Не выбраны олимпиады, для которых предоставляются льготы");
			}

			item.Benefit = dbContext.Benefit.First(x => x.BenefitID == model.BenefitTypeID);
			if (model.DiplomaTypeID == 0)
				return new AjaxResultModel("Льгота должна быть для победителя или призера олимпиады");
			item.OlympicDiplomType = dbContext.OlympicDiplomType.First(x => x.OlympicDiplomTypeID == model.DiplomaTypeID);
			if (model.OlympicLevelFlags == 0)
				return new AjaxResultModel("Льгота должна быть хотя бы для одного из уровней олимпиады");
			item.OlympicLevelFlags = (short)model.OlympicLevelFlags;
			item.IsForAllOlympic = model.IsForAllOlympic;
			item.IsProfileSubject = model.IsProfileSubject;
		    item.OlympicYear = model.OlympicYearID;
			item.UID = model.UID;
            item.EgeMinValue = model.MinEgeValue;

            // Проверим на соответствие общесистемному минимальному баллу
            // Но только если указан год олимпиады больше или равный 2014 - это вводится в 2014 году
            var systemMinEge = dbContext.GlobalMinEge.FirstOrDefault(x => x.EgeYear == item.OlympicYear);

            // Проверка необходима, если выбрана олимпиада(ы) с творческими дисциплинами и только они.

            bool isCheckMinMarkNecessary;

            if (model.IsForAllOlympic)
                isCheckMinMarkNecessary = true;
            else
            {
                var olympics = dbContext.OlympicType.Where(x => model.AttachedOlympic.Contains(x.OlympicID));

                // Массив ID предметов не ЕГЭ
                var subjects = dbContext.Subject.Where(x => !x.IsEge).Select(x => x.SubjectID).ToArray();

                var allExcludedOlympics = dbContext.OlympicTypeSubjectLink.Where(x => subjects.Contains(x.SubjectID)).Select(x => x.OlympicID).Distinct().ToArray();

                int allOlympicsCount = olympics.Count();
                int excludedOlympicsCount = olympics.Count(x => allExcludedOlympics.Contains(x.OlympicID));

                isCheckMinMarkNecessary = (allOlympicsCount != excludedOlympicsCount);
            }

            if (isCheckMinMarkNecessary)
            {
                if (model.EntranceTestItemID > 0)
                {

                    if (systemMinEge == null && item.OlympicYear >= 2014)
                        return new AjaxResultModel("Для указанного года олимпиады не задан общий минимальный балл ЕГЭ");

                    if (!item.EgeMinValue.HasValue && item.OlympicYear >= 2014)
                        return new AjaxResultModel("Для олимпиад 2014 года и более поздних должен быть указан минимальный балл ЕГЭ");

                    if (item.EgeMinValue.HasValue && item.OlympicYear >= 2014 && systemMinEge != null && systemMinEge.MinEgeScore > item.EgeMinValue)
                        return new AjaxResultModel(string.Format("Минимальный балл ЕГЭ для использования льготы ({0}) не может быть меньше общесистемного минимального балла ЕГЭ ({1})", item.EgeMinValue, systemMinEge.MinEgeScore));
                }
                else
                {
                    if (item.OlympicYear >= 2014)
                    {
                        if (model.BenefitItemSubjects == null)
                            return new AjaxResultModel("Для олимпиад 2014 года и более поздних должен быть задан хотя бы один предмет, минимально необходимый балл по которому даст право на использование льготы");

                        if (systemMinEge == null)
                            return new AjaxResultModel("Для указанного года олимпиады не задан общий минимальный балл ЕГЭ");

                        var lowSubjects = model.BenefitItemSubjects.Where(x => x.EgeMinValue < systemMinEge.MinEgeScore).ToArray();
                        if (lowSubjects.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var lowSubject in lowSubjects)
                                sb.AppendFormat("Для предмета {0} задан балл ЕГЭ {1}, что меньше установленного минимального балла ({2}) на {3} год\r\n", lowSubject.SubjectName, lowSubject.EgeMinValue, systemMinEge.MinEgeScore, item.OlympicYear);

                            return new AjaxResultModel(sb.ToString());
                        }
                    }
                }
            }

			//проверяем уиды на уникальность
			using (var eContext = new EntrantsEntities())
			{
				var compGroupIDs = eContext.CompetitiveGroup.Where(x => x.InstitutionID == institutionID).Select(x => x.CompetitiveGroupID).ToArray();

				if (item.UID != null && dbContext.BenefitItemC.Any(x => compGroupIDs.Contains(x.CompetitiveGroupID)
				                                                        && x.BenefitItemID != model.BenefitItemID
				                                                        && x.UID == item.UID))
					return new AjaxResultModel("").SetIsError("benefitItemUID", "UID уже существует");
			}

            string benefitValidationError = CheckAbilityToSaveBenefitItem(dbContext, item, model.AttachedOlympic);
			if (benefitValidationError != null)
				return new AjaxResultModel(benefitValidationError);
			if (isEdit)
			{
				dbContext.BenefitItemCOlympicType
					.Where(x => x.BenefitItemID == model.BenefitItemID)
					.ToList().ForEach(dbContext.BenefitItemCOlympicType.DeleteObject);
			}

			if (model.AttachedOlympic == null) model.AttachedOlympic = new int[0];
			if (model.AttachedOlympic.Distinct().Count() < model.AttachedOlympic.Length)
				return new AjaxResultModel("Дублируются олимпиады");

			// записываем все выбранные олимпиады
			foreach (int olympicID in model.AttachedOlympic)
			{
				var t = new BenefitItemCOlympicType();
				t.BenefitItemC = item;
				t.OlympicTypeID = olympicID;
				var dbOlympic = dbContext.OlympicType.FirstOrDefault(x => x.OlympicID == olympicID);
				if (dbOlympic == null)
					return new AjaxResultModel("Не найдена олимпиада");

			    var levelId = dbOlympic.OlympicLevelID;
                /* Новая логика по SubjectLevelID */
                if (levelId == null)
                {
                    bool hasCorrectLevel = false;
                    foreach(var level in dbContext.OlympicTypeSubjectLink.Where(x => x.OlympicID == olympicID).Select(c => c.SubjectLevelID.Value))
                    {
                        int flagOlympic = 0;
                        if (level == 2) flagOlympic |= 1;
                        if (level == 3) flagOlympic |= 2;
                        if (level == 4) flagOlympic |= 4;
                        if (!model.IsForAllOlympic && (model.OlympicLevelFlags & flagOlympic) == 0) {}
                        else hasCorrectLevel = true;
                    }
                    if (!hasCorrectLevel)
                        return new AjaxResultModel("Выбранная олимпиада " + dbOlympic.Name + " имеет несоответствующий уровень.");
                }
                else
                {
                    int flagOlympic = 0;
                    if (levelId == 2) flagOlympic |= 1;
                    if (levelId == 3) flagOlympic |= 2;
                    if (levelId == 4) flagOlympic |= 4;
                    if (!model.IsForAllOlympic && (model.OlympicLevelFlags & flagOlympic) == 0)
                        return new AjaxResultModel("Выбранная олимпиада " + dbOlympic.Name + " имеет несоответствующий уровень.");
                }

				dbContext.BenefitItemCOlympicType.AddObject(t);
			}

            // Удаляем открепленные предметы #30095
		    var currentSubjects = dbContext.BenefitItemSubject.Where(x => x.BenefitItemId == item.BenefitItemID).ToArray();

            // удаляем то что не в модели
            var modelSubjectsId = (model.BenefitItemSubjects ?? new BenefitItemSubjectViewModel[0]).Select(x => x.SubjectId).ToArray();

            foreach (var currentSubject in currentSubjects)
            {
                if (!modelSubjectsId.Any(x => x == currentSubject.SubjectId))
                {
                    dbContext.BenefitItemSubject.DeleteObject(currentSubject);
                }
            }

            // Сохраняем все прикреплённые предметы
            if (model.BenefitItemSubjects != null)
            {
                foreach (BenefitItemSubjectViewModel subject in model.BenefitItemSubjects)
                {
                    GVUZ.Model.Benefits.BenefitItemSubject existingSubject = dbContext.BenefitItemSubject.FirstOrDefault(x => x.Id == subject.Id);

                    if (existingSubject == null)
                    {
                        existingSubject = new GVUZ.Model.Benefits.BenefitItemSubject()
                        {
                            BenefitItemId = model.BenefitItemID,
                            EgeMinValue = subject.EgeMinValue,
                            SubjectId = subject.SubjectId
                        };

                        dbContext.BenefitItemSubject.AddObject(existingSubject);
                    }
                    else
                    {
                        existingSubject.SubjectId = subject.SubjectId;
                        existingSubject.EgeMinValue = subject.EgeMinValue;
                    }
                }
            }

			if (!isEdit)
				dbContext.BenefitItemC.AddObject(item);
			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				SqlException inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("UK_BenefitItem"))
					return new AjaxResultModel("Уже существует данная льгота");
				throw;
			}

			//возвращаем сохранённые данные клиенту
			var benefitData = new BenefitViewModelC.BenefitData
			{
				BenefitItemID = item.BenefitItemID,
				BenefitType = item.Benefit.Name,
				CompetitionLevelFlags = item.OlympicLevelFlags,
				DiplomaType = item.OlympicDiplomType.Name,
                OlympicYear = item.OlympicYear,
				IsAllOlympic = item.IsForAllOlympic,
				UID = item.UID,
                MinEgeValue = item.EgeMinValue
			};

			benefitData.AttachedOlympic = dbContext.BenefitItemCOlympicType
				.OrderBy(x => x.OlympicType.OlympicNumber)
				.Where(x => x.BenefitItemID == item.BenefitItemID)
				.Select(x => new { x.OlympicType.OlympicNumber, x.OlympicType.OlympicID, x.OlympicType.Name, x.BenefitItemID })
				.ToArray()
				.Select(x => new BenefitViewModelC.OlympicData
				{
					Name = x.Name,
					Number = x.OlympicNumber,
					OlympicID = x.OlympicID
				}).ToArray();

            benefitData.AttachedSubjects = dbContext.BenefitItemSubject
                .Where(x => x.BenefitItemId == item.BenefitItemID)
                .Select(x => new GVUZ.Web.ViewModels.BenefitViewModelC.SubjectData()
                {
                    SubjectName = x.Subject.Name,
                    EgeMinValue = x.EgeMinValue
                }).ToArray();

			return new AjaxResultModel
			{
				Data = benefitData
			};
		}

		/// <summary>
		/// Загружаем список льгот
		/// </summary>
		public static BenefitViewModelC FillBenefitItems(this BenefitsEntities dbContext, BenefitViewModelC model)
		{
			var q = from bi in dbContext.BenefitItemC
					join dt in dbContext.OlympicDiplomType on bi.OlympicDiplomTypeID equals dt.OlympicDiplomTypeID
					join bt in dbContext.Benefit on bi.BenefitID equals bt.BenefitID
					orderby dt.Name, bt.Name
					where ((model.EntranceTestItemID > 0 && bi.EntranceTestItemID == model.EntranceTestItemID)
							|| (model.EntranceTestItemID == 0 && bi.EntranceTestItemID == null))
						&& model.CompetitiveGroupID == bi.CompetitiveGroupID
					select new BenefitViewModelC.BenefitData
					{
						BenefitItemID = bi.BenefitItemID,
						BenefitTypeID = bt.BenefitID,
						BenefitType = bt.Name,
						DiplomaTypeID = dt.OlympicDiplomTypeID,
						DiplomaType = dt.Name,
						CompetitionLevelFlags = bi.OlympicLevelFlags,
                        OlympicYear = bi.OlympicYear,
						IsAllOlympic = bi.IsForAllOlympic,
						UID = bi.UID,
                        MinEgeValue = bi.EgeMinValue
					};
			model.Benefits = q.ToArray();
			var olympicData = dbContext.BenefitItemCOlympicType
				.OrderBy(x => x.OlympicType.OlympicNumber)
				.Where(x => x.BenefitItemC.CompetitiveGroupID == model.CompetitiveGroupID && 
					((model.EntranceTestItemID > 0 && x.BenefitItemC.EntranceTestItemID == model.EntranceTestItemID)
						|| (model.EntranceTestItemID == 0 && x.BenefitItemC.EntranceTestItemID == null)))
				.Select(x => new { x.OlympicType.OlympicNumber, x.OlympicType.OlympicID, x.OlympicType.Name, x.BenefitItemID }).ToArray();

			//прикрепляем олимпиады
			foreach (var benefit in model.Benefits)
			{
				BenefitViewModelC.BenefitData benefit1 = benefit;
				benefit.AttachedOlympic = olympicData.Where(x => x.BenefitItemID == benefit1.BenefitItemID)
					.OrderBy(x => x.OlympicNumber)
					.Select(x => new BenefitViewModelC.OlympicData
					             {
					             	Name = x.Name,
									Number = x.OlympicNumber,
									OlympicID = x.OlympicID
					             }).ToArray();
                benefit.AttachedSubjects = dbContext.BenefitItemSubject
                    .Include("Subject")
                    .Where(x => x.BenefitItemId == benefit.BenefitItemID)
                    .Select(x => new BenefitViewModelC.SubjectData()
                    {
                        SubjectName = x.Subject.Name,
                        EgeMinValue = x.EgeMinValue
                    }).ToArray();
			}

			//можно ли редактировать
			using (var eContext = new EntrantsEntities())
			{
				model.CanEdit = eContext.CompetitiveGroup.Any(x => x.CompetitiveGroupID == model.CompetitiveGroupID &&
				                                                   (x.Campaign == null || x.Campaign.StatusID != CampaignStatusType.Finished));
			}

			return model;
		}

		/// <summary>
		/// Удаляем льготу
		/// </summary>
		public static AjaxResultModel DeleteBenefitItemC(this BenefitsEntities dbContext, int? benefitItemID)
		{
			BenefitItemC item = dbContext.BenefitItemC.Where(x => x.BenefitItemID == benefitItemID).FirstOrDefault();
			if (item == null)
				return new AjaxResultModel("Не найдна льгота");
			using (EntrantsEntities eContext = new EntrantsEntities())
			{
				if (!eContext.CompetitiveGroup.Any(x => x.CompetitiveGroupID == item.CompetitiveGroupID &&
				                                        (x.Campaign == null || x.Campaign.StatusID != CampaignStatusType.Finished)))
					return new AjaxResultModel("Не найдна льгота");
			}
			//выбранные олимпиады уходят каскадом
			//item.BenefitItemCOlympicType.Load();
			//foreach (var t in item.BenefitItemCOlympicType)
			//	dbContext.BenefitItemCOlympicType.DeleteObject(t);

            var attachedSubjects = dbContext.BenefitItemSubject.Where(x => x.BenefitItemId == item.BenefitItemID);

            foreach (var bs in attachedSubjects)
                dbContext.BenefitItemSubject.DeleteObject(bs);

			dbContext.BenefitItemC.DeleteObject(item);
			dbContext.SaveChanges();
			return new AjaxResultModel();
		}

		/// <summary>
		/// количество льгот для ВИ
		/// </summary>
		public static int GetBenefitItemCount(this BenefitsEntities dbContext, int entranceTestItemID, int competitiveGroupID)
		{
			return dbContext.BenefitItemC.Count(x => x.CompetitiveGroupID == competitiveGroupID
			                                         && ((entranceTestItemID == 0 && x.EntranceTestItemID == null) ||
			                                             entranceTestItemID == x.EntranceTestItemID));
		}

        public static AjaxResultModel GetOlympicData(this BenefitsEntities dbContext, int subjectID, int olympicYear)
        {
            var items = new List<AddBenefitViewModelC.OlympicData>();
            items.AddRange(dbContext.OlympicType
                    .Include("OlympicTypeSubjectLink")
                    .Include("OlympicTypeSubjectLink.Subject")
                    .Where(c => c.OlympicYear == olympicYear)
                    .OrderBy(x => x.OlympicNumber).ToList()
                    .Select(x => new AddBenefitViewModelC.OlympicData
                    {
                        Level = 
                            x.OlympicLevelID != null ? 
                            new [] { x.OlympicLevelID.Value } :
                            x.OlympicTypeSubjectLink.Select(c => c.SubjectLevelID.Value).Distinct().ToArray(),
                            
                        Year = x.OlympicYear,
                        Name =
                            x.OlympicLevelID != null ?
                            x.Name :
                            x.Name + "(" +
                            string.Join(", ", x.OlympicTypeSubjectLink.Select(c => c.Subject.Name).Distinct().ToArray()) + ")",                        

                        NumberInt = x.OlympicNumber,
                        OlympicID = x.OlympicID,
                        IsProfileSubject = x.OlympicTypeSubjectLink.Any(y => y.SubjectID == subjectID)
                    }));
            return new AjaxResultModel { Data = items };
        }
	}
}