using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Management.Instrumentation;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Model.Institutions;
using GVUZ.Web.ViewModels;
using Campaign = GVUZ.Model.Entrants.Campaign;
using CampaignEducationLevel = GVUZ.Model.Entrants.CampaignEducationLevel;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с кампанией
	/// </summary>
	public static class CampaignExtensions
	{
		/// <summary>
		/// Загружаем модель кампании для редактирования
		/// </summary>
		public static CampaignEditViewModel FillCampaignEditModel(this EntrantsEntities dbContext, int? campaignID, int institutionID)
		{
			CampaignEditViewModel model = new CampaignEditViewModel();
			Campaign campaign = (campaignID != null ? dbContext.Campaign.FirstOrDefault(x => x.CampaignID == campaignID && x.InstitutionID == institutionID) : null);
			model.YearStart = DateTime.Now.Year;
			model.YearEnd = DateTime.Now.Year;
			if (campaign != null)
			{
				model.CampaignID = campaign.CampaignID;
				model.EducationFormFlags = campaign.EducationFormFlag;
				model.Name = campaign.Name;
				model.YearStart = campaign.YearStart;
				model.YearEnd = campaign.YearEnd;
				model.StatusID = campaign.StatusID;
				model.CanEdit = campaign.StatusID != CampaignStatusType.Finished;
				model.HasGroups = dbContext.CompetitiveGroup.Any(x => x.CampaignID == campaignID);
				model.UID = campaign.UID;
                // TODO: Убрать поля из модели
                //model.Additional = campaign.Additional;
                //model.IsFromKrym = campaign.IsFromKrym;

                //Для удаления Форм обучения нужно смотреть есть ли на эти формы соответсвующие даты, если нет - удаляем
                //model.HasODates = campaign.CampaignDate.Any(x => x.EducationFormID == EDFormsConst.O && x.IsActive);
                //model.HasZDates = campaign.CampaignDate.Any(x => x.EducationFormID == EDFormsConst.Z && x.IsActive);
                //model.HasOZDates = campaign.CampaignDate.Any(x => x.EducationFormID == EDFormsConst.OZ && x.IsActive);
			}
			else
			{
				model.CanEdit = true;
			}
			
			//2 года назад, 10 лет вперёд
            //model.YearRange = Enumerable.Range(Math.Min(DateTime.Now.Year - 2, model.YearStart), 10)
            //    .Select(x => new { ID = x, Name = x.ToString() }).ToArray();

            // Изменили #FIS-50
		    int yearStart;
		    int years;
            
            if (!Int32.TryParse(ConfigurationManager.AppSettings["CampaignYearRangeStart"], out yearStart) || yearStart < 2000)
            {
                yearStart = 2012;
            }
            if (!Int32.TryParse(ConfigurationManager.AppSettings["CampaignYearRangeLength"], out years) || years < 1)
            {
                years = 10;
            }

		    model.YearRange = Enumerable.Range(yearStart, years).Select(x => new {ID = x, Name = x.ToString(CultureInfo.InvariantCulture)}).ToArray();


		    model.EducationLevelNames = dbContext.AdmissionItemType.Where(x => x.ItemLevel == 2 
					&& dbContext.AllowedDirections.Any(y => y.InstitutionID == institutionID && y.AdmissionItemTypeID == x.ItemTypeID))
					.OrderBy(x => x.DisplayOrder)
				.Select(x => new CampaignEditViewModel.EducationLevelName { ID = x.ItemTypeID, Name = x.Name }).ToArray();

			//6 курсов
			model.Courses = Enumerable.Range(1, 6).ToArray();

			var levels = dbContext.CampaignEducationLevel.Where(x => x.CampaignID == campaignID).ToArray();
			List<CampaignEditViewModel.EducationLevelData> modelLevels = new List<CampaignEditViewModel.EducationLevelData>();

			//данные для таблицы с разрешёнными уровнями образования
			foreach (var course in model.Courses)
			{
				foreach (var educationLevelName in model.EducationLevelNames)
				{
					CampaignEditViewModel.EducationLevelData ldata = new CampaignEditViewModel.EducationLevelData();
					ldata.Course = course;
					ldata.EducationLevelID = educationLevelName.ID;
					ldata.IsSelected = levels.Any(x => x.Course == ldata.Course && x.EducationLevelID == ldata.EducationLevelID);
					ldata.IsAvailable = true;
					modelLevels.Add(ldata);
				}
			}
			//#31145 - изменение условий
			//•	Прием на СПО возможен только для 1 курса
			//modelLevels.Where(x => x.EducationLevelID == 17 && x.Course > 1).ToList().ForEach(x => x.IsAvailable = false);
			//•	Прием на бакалавриат (полн.) может проходить на 4 курса (теперь и на 5)
			modelLevels.Where(x => x.EducationLevelID == 2 && x.Course > 5).ToList().ForEach(x => x.IsAvailable = false);
			//•	Прием на бакалавриат (сокращ.) возможен только на 1 курс (теперь на 3)
			modelLevels.Where(x => x.EducationLevelID == 3 && x.Course > 3).ToList().ForEach(x => x.IsAvailable = false);
			//•	Прием на специалитет может проходить на 6 курсов
			//do nothing
			//•	Прием в магистратуру возможен только на 1 курс
			modelLevels.Where(x => x.EducationLevelID == 4 && x.Course > 1).ToList().ForEach(x => x.IsAvailable = false);

            // Приём кадров высшей квалификации возможен только на 1 курс
            modelLevels.Where(x => x.EducationLevelID == 18 && x.Course > 1).ToList().ForEach(x => x.IsAvailable = false);

            // Приём на прикладной бакалавриат возможен только на 1 курс
            modelLevels.Where(x => x.EducationLevelID == 19 && x.Course > 1).ToList().ForEach(x => x.IsAvailable = false);
            
            model.EducationLevels = modelLevels.ToArray();
			return model;
		}

		/// <summary>
		/// Сохраняем кампанию
		/// </summary>
		public static AjaxResultModel SaveCampaignEditModel(this EntrantsEntities dbContext, CampaignEditViewModel model, int institutionID)
		{
			Campaign campaign =
				dbContext.Campaign.FirstOrDefault(x => x.CampaignID == model.CampaignID && x.InstitutionID == institutionID);
			if (campaign == null)
			{
				campaign = new Campaign { InstitutionID = institutionID };
				dbContext.Campaign.AddObject(campaign);
			}
			else
			{
				if (campaign.StatusID == CampaignStatusType.Finished)
					return new AjaxResultModel(AjaxResultModel.DataError);
			}

			campaign.Name = model.Name;
			campaign.UID = model.UID;
			if (
				dbContext.Campaign.Any(
					x => x.InstitutionID == institutionID && x.Name == campaign.Name && x.CampaignID != model.CampaignID))
				return new AjaxResultModel().SetIsError("Name", "Существует кампания с данным именем");
			if (
				dbContext.Campaign.Any(
					x => x.InstitutionID == institutionID && x.UID == campaign.UID && x.CampaignID != model.CampaignID))
				return new AjaxResultModel().SetIsError("UID", "Существует кампания с данным UID'ом");

			campaign.YearStart = model.YearStart;
			campaign.YearEnd = model.YearEnd;
			if (campaign.YearStart > campaign.YearEnd)
				return new AjaxResultModel(" ")
				{
					Data = new[]
					{
						new { ControlID = "YearEnd", ErrorMessage = "Начало интервала должно быть меньше окончания" },
						new { ControlID = "YearStart", ErrorMessage = "" }
					}
				};
			campaign.EducationFormFlag = model.EducationFormFlags;
			//формы обучения больше не связаны
			if (campaign.EducationFormFlag == 0 /*|| ((campaign.EducationFormFlag & 1) > 0 &&  campaign.EducationFormFlag != 7)*/)
				return new AjaxResultModel().SetIsError("EducationFormFlags", "Некорректные формы обучения");

			if (model.EducationLevels == null || !model.EducationLevels.Where(x => x.IsSelected).Any())
				return new AjaxResultModel().SetIsError("EducationLevels", "Необходимо выбрать хотя бы один уровень образования");

            //bool addit = model.Additional;
            //if (campaign.Additional != addit)
            //{
            //    List<CampaignDate> cdt = dbContext.CampaignDate.Where(x => x.CampaignID == campaign.CampaignID).ToList(); ;
            //    if (addit)
            //    {
            //        foreach (CampaignDate cd in cdt)
            //        {
            //            if (cd.Stage == 2)
            //                dbContext.CampaignDate.DeleteObject(cd);
            //            if (cd.Stage == 1)
            //            {
            //                CampaignDate cd1 = new CampaignDate();

            //                cd1.AdmissionItemType = cd.AdmissionItemType;
            //                cd1.AdmissionItemType1 = cd.AdmissionItemType1;
            //                cd1.AdmissionItemType2 = cd.AdmissionItemType2;
            //                cd1.Campaign = cd.Campaign;
            //                cd1.CampaignDateID = cd.CampaignID;
            //                cd1.Course = cd.Course;
            //                cd1.DateEnd = cd.DateEnd;
            //                cd1.DateStart = cd.DateStart;
            //                cd1.DateOrder = cd.DateOrder;
            //                cd1.EducationFormID = cd.EducationFormID;
            //                cd1.EducationLevelID = cd.EducationLevelID;
            //                cd1.EducationSourceID = cd.EducationSourceID;
            //                cd1.IsActive = cd.IsActive;

            //                cd1.Stage = 0;
            //                dbContext.CampaignDate.AddObject(cd1);
            //                dbContext.CampaignDate.DeleteObject(cd);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        /*int[] allowedStages = campaign.Additional == 0 && campaignEducationLevel.Course == 1 && (edFormID == EDFormsConst.O | edFormID == EDFormsConst.OZ)
            //            && new int[]
            //               {
            //                EDLevelConst.Bachelor, EDLevelConst.Speciality
            //               }.Contains(campaignEducationLevel.EducationLevelID) ? new[] { 1, 2 } : new[] { 0 };*/
            //        foreach (CampaignDate cd in cdt)
            //        {
            //            if (cd.Stage == 0 && cd.Course == 1 
            //                && (cd.EducationFormID == EDFormsConst.O | cd.EducationFormID == EDFormsConst.OZ)
            //                && new int[]
            //               {
            //                EDLevelConst.Bachelor, EDLevelConst.Speciality
            //               }.Contains(cd.EducationLevelID))
            //            {
            //                DateTime? dtemp;
            //                {   
            //                    CampaignDate cd1 = new CampaignDate();

            //                    cd1.AdmissionItemType = cd.AdmissionItemType;
            //                    cd1.AdmissionItemType1 = cd.AdmissionItemType1;
            //                    cd1.AdmissionItemType2 = cd.AdmissionItemType2;
            //                    cd1.Campaign = cd.Campaign;
            //                    cd1.CampaignDateID = cd.CampaignID;
            //                    cd1.Course = cd.Course;
            //                    cd1.DateEnd = cd.DateEnd;
            //                    cd1.DateStart = cd.DateStart;
            //                    cd1.DateOrder = cd.DateOrder;
            //                    cd1.EducationFormID = cd.EducationFormID;
            //                    cd1.EducationLevelID = cd.EducationLevelID;
            //                    cd1.EducationSourceID = cd.EducationSourceID;
            //                    cd1.IsActive = cd.IsActive;

            //                    cd1.Stage = 1;
            //                    dbContext.CampaignDate.AddObject(cd1);

            //                    dtemp = cd1.DateOrder;
            //                }
            //                {
            //                    CampaignDate cd1 = new CampaignDate();

            //                    cd1.AdmissionItemType = cd.AdmissionItemType;
            //                    cd1.AdmissionItemType1 = cd.AdmissionItemType1;
            //                    cd1.AdmissionItemType2 = cd.AdmissionItemType2;
            //                    cd1.Campaign = cd.Campaign;
            //                    cd1.CampaignDateID = cd.CampaignID;
            //                    cd1.Course = cd.Course;
            //                    cd1.DateEnd = cd.DateEnd;
            //                    cd1.DateStart = cd.DateStart;
            //                    cd1.DateOrder = cd.DateOrder;
            //                    cd1.EducationFormID = cd.EducationFormID;
            //                    cd1.EducationLevelID = cd.EducationLevelID;
            //                    cd1.EducationSourceID = cd.EducationSourceID;
            //                    cd1.IsActive = cd.IsActive;

            //                    TimeSpan ts = new TimeSpan(5, 0, 0, 0);
            //                    cd1.DateOrder = dtemp + ts;
            //                    cd1.Stage = 2;

            //                    dbContext.CampaignDate.AddObject(cd1);
            //                }
            //                dbContext.CampaignDate.DeleteObject(cd);
            //            }
            //        }
            //    }
            //}
            //campaign.Additional = addit;

			//проверяем КГ
			var cgTypes = dbContext.CompetitiveGroupItem.Where(x => x.CompetitiveGroup.CampaignID == campaign.CampaignID)
				.Select(x => new { x.CompetitiveGroup.Course, x.EducationLevelID }).ToArray()
				.Select(x => x.Course + "@" + x.EducationLevelID).ToArray();
			cgTypes = cgTypes.Except(model.EducationLevels.Select(x => x.Course + "@" + x.EducationLevelID)).ToArray();
			if (cgTypes.Length > 0)
			{
				var errors = cgTypes.Select(x => String.Format("{0} курс, {1}",
					x.Split('@')[0], DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, x.Split('@')[1].To(0))));
				return new AjaxResultModel().SetIsError("EducationLevels",
					"Невозможно исключить следующие уровни: " + String.Join("; ", errors) + " по причине наличия конкурсов");
			}

			//проверяем объём приёма #37923
			cgTypes = dbContext.AdmissionVolume.Where(x => x.CampaignID == campaign.CampaignID
				&& x.NumberBudgetO + x.NumberBudgetOZ + x.NumberBudgetZ
					+ x.NumberPaidO + x.NumberPaidOZ + x.NumberPaidZ
					+ x.NumberTargetO + x.NumberTargetOZ + x.NumberTargetZ > 0)
				.Select(x => new { x.Course, EducationLevelID = x.AdmissionItemTypeID }).ToArray()
				.Select(x => x.Course + "@" + x.EducationLevelID).ToArray();
			cgTypes = cgTypes.Except(model.EducationLevels.Select(x => x.Course + "@" + x.EducationLevelID)).ToArray();
			if (cgTypes.Length > 0)
			{
				var errors = cgTypes.Select(x => String.Format("{0} курс, {1}",
					x.Split('@')[0], DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, x.Split('@')[1].To(0))));
				return new AjaxResultModel().SetIsError("EducationLevels",
					"Невозможно исключить следующие уровни: " + String.Join("; ", errors) + " по причине наличия объема приема");
			}

			List<short> forms = new List<short>();
			forms.Add(0);
			if ((campaign.EducationFormFlag & 1) > 0) forms.Add(EDFormsConst.O);
			if ((campaign.EducationFormFlag & 2) > 0) forms.Add(EDFormsConst.OZ);
			if ((campaign.EducationFormFlag & 4) > 0) forms.Add(EDFormsConst.Z);
			var appForms = dbContext.Application
				.Where(
					x =>
						x.InstitutionID == institutionID && x.OrderEducationFormID != null &&
							x.ApplicationSelectedCompetitiveGroup.Any(y => y.CompetitiveGroup.CampaignID == model.CampaignID))
				.Select(x => x.OrderEducationFormID.Value).ToArray();
			appForms = appForms.Except(forms).ToArray();
			if (appForms.Length > 0)
			{
				var errors = appForms
					.Select(x => DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.Study, x));
				return new AjaxResultModel().SetIsError("EducationFormFlags",
					"Невозможно исключить следующие формы: " + String.Join("; ", errors));
			}

			//удаляем существующие уровни и добавляем выбранные
			dbContext.CampaignEducationLevel.Where(x => x.CampaignID == model.CampaignID).ToList().ForEach(dbContext.CampaignEducationLevel.DeleteObject);
			foreach (var educationLevelData in model.EducationLevels)
			{
				CampaignEducationLevel el = new CampaignEducationLevel();
				el.Campaign = campaign;
				el.EducationLevelID = (short)educationLevelData.EducationLevelID;
				el.Course = educationLevelData.Course;
				dbContext.CampaignEducationLevel.AddObject(el);
			}

			//удаляем лишний объём приёма (он по нулям и нет КГ, так что допустимо)
			var campLevels = model.EducationLevels.Select(x => x.Course + "@" + x.EducationLevelID).ToArray();
			dbContext.AdmissionVolume.Where(x => x.CampaignID == campaign.CampaignID)
				.ToList()
				.Where(x => !campLevels.Contains(x.Course + "@" + x.AdmissionItemTypeID))
				.ToList()
				.ForEach(dbContext.AdmissionVolume.DeleteObject);

            // #27966
		    //campaign.IsFromKrym = model.IsFromKrym;

			dbContext.SaveChanges();

			//обновляем даты
			if (model.CampaignID > 0)
			{
				var dateEditModel = FillCampaignDateEditModel(dbContext, model.CampaignID, institutionID);
				SaveCampaignDateEditModel(dbContext, dateEditModel, institutionID);
			}

			return new AjaxResultModel { Data = campaign.CampaignID };
		}

		/// <summary>
		/// Загружаем даты кампании
		/// </summary>
		public static CampaignDateEditViewModel FillCampaignDateEditModel(this EntrantsEntities dbContext, int campaignID, int institutionID)
		{
			var model = new CampaignDateEditViewModel();
			var campaign = dbContext.Campaign.SingleOrDefault(x => x.CampaignID == campaignID && x.InstitutionID == institutionID);
            if (campaign == null)
                throw new ApplicationException(string.Format(
                    "Кампания ({0}) не найдена в БД. Она была удалена сервисом или другим пользователем", campaignID));

			model.CampaignID = campaign.CampaignID;
			model.YearStart = campaign.YearStart;
			model.YearEnd = campaign.YearEnd;
			model.StatusID = campaign.StatusID;

			model.CanEdit = campaign.StatusID != CampaignStatusType.Finished;

			List<int> allowedForms = new List<int>();
			if ((campaign.EducationFormFlag & 1) > 0)
				allowedForms.Add(EDFormsConst.O);
			if ((campaign.EducationFormFlag & 2) > 0)
				allowedForms.Add(EDFormsConst.OZ);
			if ((campaign.EducationFormFlag & 4) > 0)
				allowedForms.Add(EDFormsConst.Z);
			var campaignEducationLevels = dbContext.CampaignEducationLevel.Where(x => x.CampaignID == campaignID).ToArray();

			List<CampaignDateEditViewModel.CampaignDateDetails> cdList = new List<CampaignDateEditViewModel.CampaignDateDetails>();

			Action<int, int, int, int, int> addCampaignDateDetails = (course, form, level, source, stage) =>
			                                                         	{
																			CampaignDateEditViewModel.CampaignDateDetails details = new CampaignDateEditViewModel.CampaignDateDetails();
																			details.Course = course;
																			details.EducationFormID = form;
																			details.EducationLevelID = level;
																			details.EducationSourceID = source;
			                                                         		details.Stage = stage;
																			if ((form == EDFormsConst.O || form == EDFormsConst.OZ) && course == 1
																				&& (level == EDLevelConst.Bachelor || level == EDLevelConst.BachelorShort || level == EDLevelConst.Speciality || level == EDLevelConst.AppliedBachelor))
																			{
																				details.DateStart = GetCampaignDate(dbContext, model.YearStart, "CampaignDefaultDateStart");
                                                                                details.DateEnd = GetCampaignDate(dbContext, model.YearStart, "CampaignDefaultDateEnd");
																				if (source == AdmissionItemTypeConstants.TargetReception)
                                                                                    details.DateOrder = GetCampaignDate(dbContext, model.YearStart, "CampaignDefaultDateOrderTarget");
																				if (stage == 1)
                                                                                    details.DateOrder = GetCampaignDate(dbContext, model.YearStart, "CampaignDefaultDateOrderStage1");
																				if (stage == 2)
                                                                                    details.DateOrder = GetCampaignDate(dbContext, model.YearStart, "CampaignDefaultDateOrderStage2");
																				//для платников нет отдельной даты
                                                                                // а везде - она есть. Так что открываю...
																				if (source == AdmissionItemTypeConstants.PaidPlaces)
																					details.DateOrder = GetCampaignDate(dbContext, model.YearStart, "CampaignDateOrderPaidMax");
																			}

			                                                         		cdList.Add(details);
			                                                         	};
			//набираем даты по формам и правилам
			foreach (var edFormID in allowedForms)
			{
				foreach (var campaignEducationLevel in campaignEducationLevels)
				{
					//первый и второй этапы для бакалавриата и специалитета первого курса, очной и ОЗ формы (#34871)
					//#40897 - Для сокращенного бакалавриата не должно существовать этапов приема
                    //!campaign.Additional &&
					int[] allowedStages =  campaignEducationLevel.Course == 1 && (edFormID == EDFormsConst.O | edFormID == EDFormsConst.OZ)
						&& new int[]
						   {
						   	EDLevelConst.Bachelor, /*EDLevelConst.BachelorShort, */EDLevelConst.Speciality, EDLevelConst.AppliedBachelor
						   }.Contains(campaignEducationLevel.EducationLevelID) ? new[] { 1, 2 } : new[] { 0 };
					foreach (var allowedStage in allowedStages)
					{
						addCampaignDateDetails(campaignEducationLevel.Course, edFormID, campaignEducationLevel.EducationLevelID, EDSourceConst.Budget, allowedStage);
					}

					addCampaignDateDetails(campaignEducationLevel.Course, edFormID, campaignEducationLevel.EducationLevelID, EDSourceConst.Paid, 0);
					//if(edFormID == AdmissionItemTypeConstants.FullTimeTuition)
					//#31226 нет целевиков на второй курс, а на первый как угодно
					if (campaignEducationLevel.Course == 1 /*&& (
						campaignEducationLevel.EducationLevelID == AdmissionItemTypeConstants.Bachelor ||
						campaignEducationLevel.EducationLevelID == AdmissionItemTypeConstants.BachelorShort ||
						campaignEducationLevel.EducationLevelID == AdmissionItemTypeConstants.Speciality)*/)
						addCampaignDateDetails(campaignEducationLevel.Course, edFormID, campaignEducationLevel.EducationLevelID, EDSourceConst.Target, 0);
				}
			}

			// берём существующие даты
            //var campaignDates = dbContext.CampaignDate.Where(x => x.CampaignID == campaignID)
            //    .Select(x => new
            //                 {
            //                    CD = x,
            //                    CG = dbContext.CompetitiveGroupItem.Any(y => 
            //                        y.CompetitiveGroup.CampaignID == campaignID 
            //                        && y.EducationLevelID == x.EducationLevelID 
            //                        && y.CompetitiveGroup.Course == x.Course)
            //                 })
            //    .ToArray();

            ////проставляем данные из существующих в список по правилам
            //foreach (var campaignDate in campaignDates)
            //{
            //    var ext = cdList.FirstOrDefault(x => x.EducationFormID == campaignDate.CD.EducationFormID
            //                                && x.EducationLevelID == campaignDate.CD.EducationLevelID
            //                                && x.EducationSourceID == campaignDate.CD.EducationSourceID
            //                                && x.Course == campaignDate.CD.Course
            //                                && x.Stage == campaignDate.CD.Stage);
            //    if (ext != null)
            //    {
            //        ext.IsActive = campaignDate.CD.IsActive;
            //        ext.DateStart = campaignDate.CD.DateStart ?? ext.DateStart;
            //        ext.DateEnd = campaignDate.CD.DateEnd ?? ext.DateEnd;
            //        ext.DateOrder = campaignDate.CD.DateOrder ?? ext.DateOrder;
            //        ext.UID = campaignDate.CD.UID;
            //        ext.CanRemoveDate = !campaignDate.CG;
            //    }
            //}

			cdList.Sort();

			model.CampaignDates = cdList.ToArray();
			return model;
		}

		/// <summary>
		/// Сохраняем даты кампании
		/// </summary>
		public static AjaxResultModel SaveCampaignDateEditModel(this EntrantsEntities dbContext, CampaignDateEditViewModel model, int institutionID)
		{
			Campaign campaign =
				dbContext.Campaign.FirstOrDefault(x => x.CampaignID == model.CampaignID && x.InstitutionID == institutionID);
			if (campaign == null || model.CampaignDates == null || campaign.StatusID == CampaignStatusType.Finished)
				return new AjaxResultModel(AjaxResultModel.DataError);

            //dbContext.CampaignDate.Where(x => x.CampaignID == campaign.CampaignID).ToList().ForEach(
            //    dbContext.CampaignDate.DeleteObject);
			List<Tuple<string, int>> dateErrors = new List<Tuple<string, int>>();
            List<string> uidErrors = new List<string>();

            var uids = model.CampaignDates.Where(x => !string.IsNullOrEmpty(x.UID)).Select(x => x.UID).ToArray();

		    Func<CampaignDateEditViewModel.CampaignDateDetails, bool> isUniqueUID = m => uids.Count(uid => uid.Equals(m.UID)) == 1;

            //// функция, создающая дату и проверяющая на ошибки
            //Action<CampaignDateEditViewModel.CampaignDateDetails> createCD = (m) =>
            //{
            //    if (!string.IsNullOrEmpty(m.UID) && !isUniqueUID(m))
            //    {
            //        uidErrors.Add(m.CombinedID);
            //    }

            //    CampaignDate cd = new CampaignDate();
            //    dbContext.CampaignDate.AddObject(cd);
            //    cd.Campaign = campaign;
            //    cd.DateStart = m.DateStart == DateTime.MinValue ? (DateTime?)null : m.DateStart;
            //    cd.DateEnd = m.DateEnd == DateTime.MinValue ? (DateTime?)null : m.DateEnd;
            //    cd.DateOrder = m.DateOrder == DateTime.MinValue ? (DateTime?)null : m.DateOrder;
            //    cd.EducationFormID = (short)m.EducationFormID;
            //    cd.EducationLevelID = (short)m.EducationLevelID;
            //    cd.EducationSourceID = (short)m.EducationSourceID;
            //    cd.Course = m.Course;
            //    cd.Stage = m.Stage;
            //    cd.UID = m.IsActive ? m.UID : null;
            //    cd.IsActive = m.IsActive;
            //    if (m.IsActive)
            //    {
            //        if (m.DateStart < new DateTime(campaign.YearStart, 1, 1) || m.DateStart > new DateTime(campaign.YearEnd, 12, 31))
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 0));
            //        if (m.DateEnd < new DateTime(campaign.YearStart, 1, 1) || m.DateEnd > new DateTime(campaign.YearEnd, 12, 31))
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 1));
            //        if (m.DateOrder < new DateTime(campaign.YearStart, 1, 1) || m.DateOrder > new DateTime(campaign.YearEnd, 12, 31))
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 2));
            //        if (m.DateStart > m.DateEnd && m.DateEnd != DateTime.MinValue)
            //        {
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 0));
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 1));
            //        }

            //        if (m.DateEnd > m.DateOrder && m.DateOrder != DateTime.MinValue)
            //        {
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 1));
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 2));
            //        }

            //        if (m.DateStart > m.DateOrder && m.DateOrder != DateTime.MinValue)
            //        {
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 0));
            //            dateErrors.Add(new Tuple<string, int>(m.CombinedID, 2));
            //        }
            //        // для платных нет отдельных правил
            //        //if (m.EducationSourceID == AdmissionItemTypeConstants.PaidPlaces && m.DateOrder > GetCampaignDate(campaign.YearStart, "CampaignDateOrderPaidMax"))
            //        //	dateErrors.Add(new Tuple<string, int>(m.CombinedID, 2));
            //    }
            //};

		    

			//создаём объекты в базе
            //foreach (var modelCd in model.CampaignDates)
            //{
            //    createCD(modelCd);
            //}

			if (dateErrors.Count > 0 || uidErrors.Count > 0)
				return new AjaxResultModel(" ")
				{
					Data = dateErrors.Select(x => new
					{
						ControlID = "dp_" + x.Item1 + "_" + x.Item2,
						ErrorMessage = ""
					})
                    .Union(
                    uidErrors.Select(x => new
                    {
                        ControlID = "uid_" + x,
                        ErrorMessage = "Дублирующиеся значения UID"
                    }))
				};
			dbContext.SaveChanges();
			return new AjaxResultModel { Data = campaign.CampaignID };
		}

		private static readonly int ListPageSize = AppSettings.Get("Search.PageSize", 25);

        /// <summary>
        /// Возвращаем список кампаний
        /// </summary>
        public static AjaxResultModel GetCampaignListModel(this EntrantsEntities dbContext, CampaignListViewModel model, int institutionID, int? specificCampaignID = null)
        {
            var tq = dbContext.Campaign.Where(x => x.InstitutionID == institutionID);
            //возвращаем одну строчку. Удобно для обновления данных на странице
            if (specificCampaignID != null)
                tq = tq.Where(x => x.CampaignID == specificCampaignID);
            model.TotalItemCount = tq.Count();
            model.TotalPageCount = ((Math.Max(model.TotalItemCount, 1) - 1) / ListPageSize) + 1;

            if (!model.SortID.HasValue)
                model.SortID = 1;

            //if(model.SortID.HasValue)
            {
                if (model.SortID == 1) tq = tq.OrderBy(x => x.Name);
                if (model.SortID == -1) tq = tq.OrderByDescending(x => x.Name);
                if (model.SortID == 2) tq = tq.OrderBy(x => x.YearStart).ThenBy(x => x.YearEnd);
                if (model.SortID == -2) tq = tq.OrderByDescending(x => x.YearStart).ThenByDescending(x => x.YearEnd);
                if (model.SortID == 5) tq = tq.OrderBy(x => x.StatusID);
                if (model.SortID == -5) tq = tq.OrderByDescending(x => x.StatusID);
            }

            tq = tq.Skip((model.PageNumber ?? 0) * ListPageSize).Take(ListPageSize);
            var res = tq.Select(x => new
            {
                x.CampaignID,
                x.YearStart,
                x.YearEnd,
                x.EducationFormFlag,
                Course1 = x.CampaignEducationLevel.Any(y => y.Course == 1),
                Course2 = x.CampaignEducationLevel.Any(y => y.Course > 1),
                EducationLevels = x.CampaignEducationLevel.Select(y => y.AdmissionItemType.Name).Distinct(),
                x.StatusID,
                x.Name,
                DateOfEnd = x.ModifiedDate
            }).ToArray();
            string[] formNames = new[] { "Очная", "Очно-заочная", "Заочная" };
            string[] statusNames = new[] { "Набор не начался", "Идет набор", "Завершена" };
            model.Campaigns = res.Select(x => new CampaignListViewModel.CampaignData
            {
                CampaignID = x.CampaignID,
                Name = x.Name,
                CampaignYearRange = x.YearStart.ToString() + (x.YearEnd != x.YearStart ? "-" + x.YearEnd : ""),
                EducationForms = String.Join(", ", new[]
                                                {
                                                    (x.EducationFormFlag & 1) > 0 ? formNames[0] : null,
                                                    (x.EducationFormFlag & 2) > 0 ? formNames[1] : null,
                                                    (x.EducationFormFlag & 4) > 0 ? formNames[2] : null
                                                }.Where(y => y != null)),
                Courses = (x.Course1 ? "1" : "") + (x.Course1 && x.Course2 ? ", " : "") + (x.Course2 ? "2+" : ""),
                EducationLevels = String.Join(", ", x.EducationLevels),
                StatusID = x.StatusID,
                DateOfEnd = x.DateOfEnd.Format("dd-MM-yyyy").ToString(),
                StatusName = statusNames[x.StatusID]
            }).ToArray();
            return new AjaxResultModel { Data = model };
        }

		///// <summary>
		///// Меняем статус кампании. Щёлкаем в зависимости от текущего
		///// </summary>
		//public static AjaxResultModel SwitchCampaignStatus(this EntrantsEntities dbContext, int campaignID, int institutionID)
		//{
		//	Campaign campaign = dbContext.Campaign.FirstOrDefault(x => x.CampaignID == campaignID && x.InstitutionID == institutionID);
		//	if (campaign == null)
		//		return new AjaxResultModel(AjaxResultModel.DataError);
		//	if (campaign.StatusID == CampaignStatusType.NotStart || campaign.StatusID == CampaignStatusType.Finished)
		//		campaign.StatusID = CampaignStatusType.Started;
		//	else
		//		campaign.StatusID = CampaignStatusType.Finished;
		//	dbContext.SaveChanges();
		//	return dbContext.GetCampaignListModel(new CampaignListViewModel(), institutionID, campaignID);
		//}

		/// <summary>
		/// Удаляем кампанию, если можно
		/// </summary>
		public static AjaxResultModel DeleteCampaign(this EntrantsEntities dbContext, int campaignID, int institutionID)
		{
			Campaign campaign = dbContext.Campaign.FirstOrDefault(x => x.CampaignID == campaignID && x.InstitutionID == institutionID);
			if (campaign == null)
				return new AjaxResultModel(AjaxResultModel.DataError);
			if (campaign.StatusID == CampaignStatusType.Finished)
				return new AjaxResultModel("Невозможно удалить завершённую кампанию.");
			if(dbContext.CompetitiveGroup.Any(x => x.CampaignID == campaignID))
				return new AjaxResultModel("Невозможно удалить кампанию, т.к. существуют конкурсы, привязанные к этой приемной кампании.");
            if (dbContext.InstitutionAchievements.Any(x => x.CampaignID == campaignID))
                return new AjaxResultModel("Невозможно удалить кампанию, т.к. существуют индивидуальные достижения, привязанные к этой приемной кампании.");
			dbContext.Campaign.DeleteObject(campaign);
			dbContext.SaveChanges();

			return new AjaxResultModel();
		}

		/// <summary>
		/// Берём дату кампании из справочника по её году
        /// Если запрашиваемого года нет в справочнике или неверен тип завпрашиваемой даты,
        /// генерируется исключение ArgumentOutOfRangeException
		/// </summary>
		private static DateTime GetCampaignDate(EntrantsEntities dbContext, int year, string key)
		{
            CampaignOrderDateCatalog yearDates = dbContext.CampaignOrderDateCatalog.FirstOrDefault(x => x.YearStart == year);

            if (yearDates == null)
                throw new ArgumentOutOfRangeException("year", string.Format("В справочнике дат приёмных кампаний отсутствуют даты для {0} года", year));

			switch (key)
			{
				case "CampaignDefaultDateStart":
                    return yearDates.StartDate;
				case "CampaignDefaultDateEnd":
					return yearDates.EndDate;
				case "CampaignDefaultDateOrderTarget":
					return yearDates.TargetOrderDate;
				case "CampaignDefaultDateOrderStage1":
					return yearDates.Stage1OrderDate;
				case "CampaignDefaultDateOrderStage2":
					return yearDates.Stage2OrderDate;
				case "CampaignDateOrderPaidMax":
					return yearDates.PaidOrderDate;
			}

			throw new ArgumentOutOfRangeException("key", "Invalid date key");
		}
	}
}