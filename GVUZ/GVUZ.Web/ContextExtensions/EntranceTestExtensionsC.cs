using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

using FogSoft.Helpers;

using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Benefits;
using GVUZ.Model.Entrants;
using GVUZ.Model.Institutions;
using GVUZ.Web.ViewModels;

using EntranceTestItemC = GVUZ.Model.Entrants.EntranceTestItemC;
using EntranceTestType = GVUZ.Model.Entrants.EntranceTestType;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Вступительные испытания. Суффикс C - исторически
	/// </summary>
	public static class EntranceTestExtensionsC
	{
		/// <summary>
		/// Начальное создание ВИ для КГ
		/// </summary>
		public static void InitialFillEntranceTest(this EntrantsEntities dbContext, int competitiveGroupID, int institutionID)
		{
			var filterType =
				dbContext.CompetitiveGroup.Where(x => x.CompetitiveGroupID == competitiveGroupID).Select(x => x.DirectionFilterType)
					.FirstOrDefault();
			//если разрешены любые, ничего не добавляем
			if (filterType == 2) return;
			//теперь вообще не надо автоматически добавлять испытания
			if (filterType != 2) return;

			//сюда уже нп попадаем. Оставлено на случай возврата к идее об обязательных ВИ

			bool hasO =
				dbContext.CompetitiveGroupItem.Any(x => x.CompetitiveGroupID == competitiveGroupID && (x.NumberBudgetO > 0 || x.NumberPaidO > 0));
			bool hasTargetO =
				dbContext.CompetitiveGroupTargetItem.Any(x => x.CompetitiveGroupID == competitiveGroupID && x.NumberTargetO > 0);
			//если нет очки, то ничего не добавляем
			if (!hasO && !hasTargetO) return;

			int directionID =
				dbContext.CompetitiveGroupItem
                .Where(x => x.CompetitiveGroupID == competitiveGroupID)
                .Select(x => x.CompetitiveGroup.DirectionID.Value).FirstOrDefault();

			if (directionID == 0) return;

			var link =
				dbContext.DirectionSubjectLinkDirection.Where(x => x.DirectionID == directionID).Select(
					x => new { LinkID = x.LinkID, ProfileSubjectID = x.DirectionSubjectLink.ProfileSubjectID }).FirstOrDefault();
			if (link == null) return;
			//профайловый не обязателен
			/*bool allowProfile = (from cgi in dbContext.CompetitiveGroupItem
								 from ct in dbContext.EntranceTestProfileDirection
								 where cgi.DirectionID == ct.DirectionID && cgi.CompetitiveGroupID == competitiveGroupID
									   && cgi.CompetitiveGroup.InstitutionID == institutionID
								 select ct.DirectionID).Count() > 0;
			if (allowProfile)
			{
				EntranceTestViewModelC.EntranceTestItemData[] profileItems = LoadEntranceTestItems(dbContext, competitiveGroupID, EntranceTestType.ProfileType);
				if(profileItems.Length == 0)
				{
					EntranceTestViewModelC prModel = new EntranceTestViewModelC(competitiveGroupID);
					prModel.CompetitiveGroupID = competitiveGroupID;
					prModel.MinScore = dbContext.Subject.Where(x => x.SubjectID == link.ProfileSubjectID).Select(x => x.MinValue).FirstOrDefault();
					prModel.Form = "не определён";
					prModel.TestTypeID = EntranceTestType.ProfileType;
					prModel.EntranceTestID = link.ProfileSubjectID;
					SaveEntranceTestItem(dbContext, prModel);
				}
			}*/
			
			EntranceTestViewModelC.EntranceTestItemData[] mainItems = LoadEntranceTestItems(dbContext, competitiveGroupID, EntranceTestType.MainType);
			EntranceTestViewModelC.EntranceTestItemData mainProfileItem = mainItems.FirstOrDefault(x => x.TestType == link.ProfileSubjectID);

			if (mainProfileItem == null) //вставляем основной профильный
			{
				EntranceTestViewModelC prModel = new EntranceTestViewModelC(competitiveGroupID);
				prModel.CompetitiveGroupID = competitiveGroupID;

				prModel.MinScore = dbContext.SubjectEgeMinValue
									.Where(x => x.SubjectID == link.ProfileSubjectID).Select(x => x.MinValue)
									.FirstOrDefault();

				prModel.Form = "ЕГЭ";
				prModel.TestTypeID = EntranceTestType.MainType;
				prModel.EntranceTestID = link.ProfileSubjectID;
				SaveEntranceTestItem(dbContext, prModel);
			}

            int russianSubjectID = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).Where(x => x.Value.ToLower() == "русский язык").Select(x => x.Key).FirstOrDefault();

			if (russianSubjectID != 0 && russianSubjectID != link.ProfileSubjectID)
			{
				EntranceTestViewModelC.EntranceTestItemData mainRussianItem = mainItems.FirstOrDefault(x => x.TestType == russianSubjectID);
				if (mainRussianItem == null)
				{
					EntranceTestViewModelC prModel = new EntranceTestViewModelC(competitiveGroupID);
					prModel.CompetitiveGroupID = competitiveGroupID;
					prModel.MinScore =
						dbContext.SubjectEgeMinValue.Where(x => x.SubjectID == russianSubjectID).Select(x => x.MinValue).FirstOrDefault();

					prModel.Form = "ЕГЭ";
					prModel.TestTypeID = EntranceTestType.MainType;
					prModel.EntranceTestID = russianSubjectID;
					SaveEntranceTestItem(dbContext, prModel);
				}
			}
		}

		/// <summary>
		/// Загружаем ВИ для КГ
		/// </summary>
		public static EntranceTestViewModelC CreateEntranceTestDataByCompetitiveGroup(this EntrantsEntities dbContext, EntranceTestViewModelC model)
		{
			var competitiveGroup =
				dbContext.CompetitiveGroup
					.Include(x => x.Campaign)
					.Single(x => x.CompetitiveGroupID == model.CompetitiveGroupID);

			int institutitionID = competitiveGroup.InstitutionID;

			model.CanEdit = competitiveGroup.Campaign == null ||
			                competitiveGroup.Campaign.StatusID != CampaignStatusType.Finished;

			// для первого курса загружаем основные ВИ
			if (competitiveGroup.Course == 1)
			{
				InitialFillEntranceTest(dbContext, model.CompetitiveGroupID, institutitionID);
				model.TestItems = LoadEntranceTestItems(dbContext, model.CompetitiveGroupID, EntranceTestType.MainType);
			}
			else
			{
				model.HideMainType = true;
				model.TestItems = new EntranceTestViewModelC.EntranceTestItemData[0];
			}

			bool allowCreative = (from cgi in dbContext.CompetitiveGroupItem
					from ct in dbContext.EntranceTestCreativeDirection
					where cgi.CompetitiveGroup.DirectionID == ct.DirectionID && cgi.CompetitiveGroupID == model.CompetitiveGroupID
					select ct.DirectionID).Any();
			// если разрешены творческие, загружаем их
			if (allowCreative/* && competitiveGroup.Course == 1*/)
			{
				model.CreativeTestItems = LoadEntranceTestItems(dbContext, model.CompetitiveGroupID, EntranceTestType.CreativeType);
				model.CreativeTestTypeID = EntranceTestType.CreativeType;
			}
			// для второго и последующих курсов - аттестационные ВИ
			if (competitiveGroup.Course > 1)
			{
                model.CustomTestItems = LoadEntranceTestItems(dbContext, model.CompetitiveGroupID, EntranceTestType.MainType);
                model.CustomTestTypeID = EntranceTestType.MainType;
			}

			bool allowProfile = (from cgi in dbContext.CompetitiveGroupItem
			                     from ct in dbContext.EntranceTestProfileDirection
			                     where cgi.CompetitiveGroup.DirectionID == ct.DirectionID && cgi.CompetitiveGroupID == model.CompetitiveGroupID
			                           && cgi.CompetitiveGroup.InstitutionID == institutitionID
									  // && ct.InstitutionID == institutitionID
			                     select ct.DirectionID).Any();

			// для первого курса профильные
			if (allowProfile && competitiveGroup.Course == 1)
			{
				model.ProfileTestItems = LoadEntranceTestItems(dbContext, model.CompetitiveGroupID, EntranceTestType.ProfileType);

				//foreach (var item in model.ProfileTestItems)
				//	item.CanRemove = false;
			}

			var testTypeNamesDict = dbContext.EntranceTestType.Select(x => new { EntranceTestTypeID = (int)x.EntranceTestTypeID, x.Name })
				.ToDictionary(x => x.EntranceTestTypeID, x => x.Name);

			if (model.CreativeTestTypeID > 0)
				model.CreativeTestItemName = testTypeNamesDict[model.CreativeTestTypeID];
			if (model.CustomTestTypeID > 0)
				model.CustomTestItemName = testTypeNamesDict[model.CustomTestTypeID];

			if (model.ProfileTestItems != null)
				model.ProfileTestItemName = testTypeNamesDict[EntranceTestType.ProfileType];
			int[] directionIDs = dbContext.CompetitiveGroupItem
				.Where(x => x.CompetitiveGroupID == model.CompetitiveGroupID).Select(x => x.CompetitiveGroup.DirectionID.Value)
				.ToArray();

			//если разрешёны любые направления, даём выбирать всё подряд
			if (competitiveGroup.DirectionFilterType == 2)
			{
				model.MainTestAllowedCount = 999; //#35885 - убрано ограничение
                model.EntranceSubjects = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject)
					.OrderBy(x => x.Value)
					.Select(x => new
					{
						ID = x.Key,
						x.Value,
						MinValueX = dbContext.SubjectEgeMinValue.FirstOrDefault(y => y.SubjectID == x.Key),
					})
					.ToArray()
					.Select(x =>
					new
					{
						x.ID,
						x.Value,
						MinValue = x.MinValueX == null ? 0 : x.MinValueX.MinValue,
					});
			}
			else
			{
				if (competitiveGroup.DirectionFilterType == 0)
					model.MainTestAllowedCount = 4; //для 505-ого приказа была просьба ограничить до 4х
				else
					model.MainTestAllowedCount = 999;
				if (directionIDs.Length == 0)
				{
					model.EditDisabled = true;
				}
				else
				{
					//var links = dbContext.GetAvailableLinksWithSameEntranceTests(directionIDs, false);
					int[] links = dbContext.DirectionSubjectLinkDirection
									.Where(x => directionIDs.Contains(x.DirectionID)).Select(x => x.LinkID)
									.Distinct()
									.ToArray();

					if (links.Length == 0)
						model.EditDisabled = true;
					else
					{
						model.EntranceSubjects = dbContext.DirectionSubjectLinkSubject
							.Where(x => links.Contains(x.LinkID))
							.OrderBy(x => x.Subject.Name)
							.Select(x => new
							             {
							             	ID = x.SubjectID,
							             	Value = x.Subject.Name,
							             	MinValueX = x.Subject.SubjectEgeMinValue.FirstOrDefault()
							             })
							.Distinct()
							.ToArray()
							.Select(x =>
							        new
							        {
							        	x.ID,
                                        x.Value,
							        	MinValue = x.MinValueX == null ? 0 : x.MinValueX.MinValue
							        });

						//int russianSubjectID =
						//	dbContext.Subject.Where(x => x.Name.ToLower() == "русский язык").Select(x => x.SubjectID).FirstOrDefault();
						//у всех испытаний одинаковый
						var firstLinkID = links[0];
						var profileSubjectID =
							dbContext.DirectionSubjectLink.Where(x => x.ID == firstLinkID).Select(x => x.ProfileSubjectID).FirstOrDefault();

						//теперь разрешено добавлять/удалять любые испытания в любых случаях #37972
						//bool hasO =
						//	dbContext.CompetitiveGroupItem.Any(x => x.CompetitiveGroupID == model.CompetitiveGroupID && (x.NumberBudgetO > 0 || x.NumberPaidO > 0));
						//bool hasTargetO =
						//	dbContext.CompetitiveGroupTargetItem.Any(x => x.CompetitiveGroupItem.CompetitiveGroupID == model.CompetitiveGroupID && x.NumberTargetO > 0);

						//bool canRemove = !hasO && !hasTargetO;
						
						//если нет очки, то ничего не добавляем
						//foreach (var item in model.TestItems)
						//{
						//	if (item.TestType == profileSubjectID || item.TestType == russianSubjectID)
						//		item.CanRemove = canRemove;
						//}

						model.ProfileTestSubjectID = profileSubjectID;
					}
				}
			}

			// количество льгот для показа чисел в интерфейсе
			using (BenefitsEntities bContext = new BenefitsEntities())
			{
				model.GeneralBenefitCount = bContext.GetBenefitItemCount(0, model.CompetitiveGroupID);
			}

			model.IsCustomEntranceTestsForMainIsAllowed = IsCustomEntranceTestsForMainIsAllowed(dbContext, model.CompetitiveGroupID);
			model.IsAnyCountOfProfileItemsAllowed = competitiveGroup.DirectionFilterType != 1;
			return model;
		}

		/// <summary>
		/// Загружаем ВИ по типу и КГ
		/// </summary>
		private static EntranceTestViewModelC.EntranceTestItemData[] LoadEntranceTestItems(EntrantsEntities dbContext, 
			int competitiveGroupID, int testType)
		{
			var q = (from x in dbContext.EntranceTestItemC
			        where x.CompetitiveGroupID == competitiveGroupID && x.EntranceTestTypeID == testType
					orderby x.Subject.Name
			        select new EntranceTestViewModelC.EntranceTestItemData
			               {
			               	ItemID = x.EntranceTestItemID,
							TestType = x.Subject == null ? 0 : x.Subject.SubjectID,
							TestName = x.Subject == null ? x.SubjectName : x.Subject.Name,
			               	//Form = x.Form,
			               	Value = x.MinScore,
							CanRemove = true,
							UID = x.UID,
                            EntranceTestPriority = x.EntranceTestPriority
			               }).ToArray();
			//TODO: optimize
			using (BenefitsEntities bContext = new BenefitsEntities())
			{
				foreach (var item in q)
					item.BenefitCount = bContext.GetBenefitItemCount(item.ItemID, competitiveGroupID);
			}

			return q;
		}

		/// <summary>
		/// Профильный предмет для КГ
		/// </summary>
		public static int GetProfileSubjectID(this EntrantsEntities dbContext, int competitiveGroupID)
		{
			//если у КГ выбраны любые направления подготовки, профильный предмет невалиден
			if (dbContext.CompetitiveGroup.Any(x => x.CompetitiveGroupID == competitiveGroupID && x.DirectionFilterType == 2))
				return 0;
			int anyDirectionID = dbContext.CompetitiveGroupItem
				.Where(x => x.CompetitiveGroupID == competitiveGroupID)
				.Select(x => x.CompetitiveGroup.DirectionID.Value).FirstOrDefault();

			return dbContext.DirectionSubjectLinkDirection.Where(x => x.DirectionID == anyDirectionID).Select(x => x.DirectionSubjectLink.ProfileSubjectID ?? 0).FirstOrDefault();
		}

		/// <summary>
		/// Удаляем ВИ
		/// </summary>
		public static AjaxResultModel DeleteEntranceTestItem(this EntrantsEntities dbContext, int testItemID)
		{
			EntranceTestItemC item = dbContext.EntranceTestItemC.FirstOrDefault(x => x.EntranceTestItemID == testItemID);
			if (item == null)
				return new AjaxResultModel("Не найден элемент");
			if (dbContext.EntranceTestItemC.Any(x => x.EntranceTestItemID == testItemID && x.CompetitiveGroup.Campaign.StatusID == CampaignStatusType.Finished))
				return new AjaxResultModel("Невозможно редактировать данные, так как приемная кампания завершена");
			var count = dbContext.ApplicationEntranceTestDocument.Count(x => x.EntranceTestItemID == testItemID);
			if (count > 0)
				return new AjaxResultModel("Невозможно удалить испытание, т.к. есть заявления с данным вступительным испытанием");
			if (!dbContext.EntranceTestItemC.Any(x => x.CompetitiveGroupID == item.CompetitiveGroupID && x.EntranceTestItemID != item.EntranceTestItemID)
				&& dbContext.Application.Any(x => x.OrderCompetitiveGroupID == item.CompetitiveGroupID)
                && item.CompetitiveGroup.CompetitiveGroupItem.Any(c => c.CompetitiveGroup.EducationLevelID != EDLevelConst.SPO))//Для СПО не выводится эта ошибка
				return new AjaxResultModel("Невозможно удалить испытание, т.к. это последнее вступительное испытание в конкурсе и есть заявления, поданные на данный конкурс.");
			dbContext.EntranceTestItemC.DeleteObject(item);
			dbContext.SaveChanges();
			return new AjaxResultModel { Data = "" };
		}

		/// <summary>
		/// Разрешены ли собственные ВИ для основных испытаний
		/// </summary>
		public static bool IsCustomEntranceTestsForMainIsAllowed(this EntrantsEntities dbContext, int competitiveGroupID)
		{
			//#35030 Для очной и очно-заочной формы тоже добавить возможность ввода испытаний с клавиатуры.
			return true;

			//На все курсы и формы обучения, кроме первого курса очной и очно-заочной, добавить возможность ввода вступительного испытания с клавиатуры.
			//не первый курс
			//if (dbContext.CompetitiveGroup.Where(x => x.CompetitiveGroupID == competitiveGroupID && x.Course > 1).Any())
			//	return true;
			//var edLevels = new[] {EDLevelConst.Bachelor, EDLevelConst.BachelorShort, EDLevelConst.Speciality};
			//if (dbContext.CompetitiveGroupItem.Where(x => edLevels.Contains(x.EducationLevelID) && x.CompetitiveGroupID == competitiveGroupID
			//	&& (x.NumberBudgetO > 0 || x.NumberBudgetOZ > 0 || x.NumberPaidO > 0 || x.NumberPaidOZ > 0)).Any())
			//	return false;
			//if (dbContext.CompetitiveGroupTargetItem.Where(x => edLevels.Contains(x.CompetitiveGroupItem.EducationLevelID) && x.CompetitiveGroupItem.CompetitiveGroupID == competitiveGroupID
			//	&& (x.NumberTargetO > 0 || x.NumberTargetOZ > 0)).Any())
			//	return false;
			//return true;
		}

		/// <summary>
		/// Сохраняем ВИ
		/// </summary>
		public static AjaxResultModel SaveEntranceTestItem(this EntrantsEntities dbContext, EntranceTestViewModelC model)
		{
			EntranceTestItemC item;
			bool isEdit = false;
			bool allowAnyTestForMain = IsCustomEntranceTestsForMainIsAllowed(dbContext, model.CompetitiveGroupID);

			// GVUZ-323 проверка на минимальный балл для вступительного испытания
			// теперь по предметам, а не по тестам
			int testMinScore = 0;
			//для остальных типов не проверяем баллы
			if (model.EntranceTestID > 0 && model.TestTypeID == EntranceTestType.MainType)
			{
				//testMinScore = dbContext.Subject.Where(x => x.SubjectID == model.EntranceTestID).Select(x => x.MinValue).FirstOrDefault();
				testMinScore = dbContext.SubjectEgeMinValue.Where(x => x.SubjectID == model.EntranceTestID).Select(x => x.MinValue).FirstOrDefault();
			}

			if (model.MinScore < testMinScore)
			{
				return new AjaxResultModel("Минимальный балл, указанный для вступительного испытания, не должен быть меньше {0} (минимального количества баллов  по результатам ЕГЭ по предмету)".FormatWith(testMinScore))
					.SetIsError("MinScore", "");
			}

			if (model.TestItemID.HasValue && model.TestItemID.Value > 0)
			{
				isEdit = true;
				item = dbContext.EntranceTestItemC
					.Include(x => x.Subject)
					.FirstOrDefault(x => x.EntranceTestItemID == model.TestItemID);
				if (item == null)
					return new AjaxResultModel("Не найден элемент");
			}
			else
			{
				item = new EntranceTestItemC();
			}

			item.CompetitiveGroup = dbContext.CompetitiveGroup.First(x => x.CompetitiveGroupID == model.CompetitiveGroupID);

			if (model.TestTypeID == EntranceTestType.ProfileType)
			{
				if (item.CompetitiveGroup.DirectionFilterType == 1 
					&& dbContext.EntranceTestItemC.Any(x => x.EntranceTestTypeID == EntranceTestType.ProfileType 
						&& x.CompetitiveGroupID == model.CompetitiveGroupID 
						&& x.EntranceTestItemID != (model.TestItemID ?? 0)))
				{
					return new AjaxResultModel("В конкурсe разрешено только одно испытание профильной направленности");
				}
			}

			if (dbContext.CompetitiveGroup.Any(x => x.Campaign.StatusID == CampaignStatusType.Finished && x.CampaignID == item.CompetitiveGroup.CampaignID))
				return new AjaxResultModel("Невозможно редактировать данные, так как приемная кампания завершена");

			var count = dbContext.ApplicationEntranceTestDocument.Count(x => x.EntranceTestItemID == item.EntranceTestItemID);
			if (count > 0 && item.EntranceTestItemID > 0 
				&& (item.SubjectID != model.EntranceTestID && item.SubjectID != null))
				return new AjaxResultModel("Невозможно изменить предмет, так как есть заявления со вступительными испытаниями на данный предмет");


			if (model.TestTypeID == EntranceTestType.CreativeType)
			{
				if (String.IsNullOrWhiteSpace(model.EntranceTestName))
					return new AjaxResultModel("Не указано название предмета").SetIsError("EntranceTestID", "");
				item.SubjectName = model.EntranceTestName;
			}
			else
			{
				var foundSubject = dbContext.Subject.FirstOrDefault(x => x.SubjectID == model.EntranceTestID);
				//теперь разрешено любые для профильных
				if (allowAnyTestForMain || model.TestTypeID == EntranceTestType.ProfileType)
				{
					item.Subject = foundSubject;
					if (item.Subject == null && String.IsNullOrWhiteSpace(model.EntranceTestName))
							return new AjaxResultModel("Не указано название предмета").SetIsError("EntranceTestID", "");
					if (item.Subject == null)
						item.SubjectName = model.EntranceTestName;
					else
						item.SubjectName = null;
				}
				else
				{
					item.Subject = foundSubject;
					if (item.Subject == null)
						return new AjaxResultModel("Не найдено испытание");
				}
			}

			item.EntranceTestType = dbContext.EntranceTestType.FirstOrDefault(x => x.EntranceTestTypeID == model.TestTypeID);
			if (model.Form == null || model.Form.Trim() == string.Empty)
				return new AjaxResultModel("Необходимо ввести форму проведения").SetIsError("Form", "");
			//item.Form = model.Form;
			item.MinScore = model.MinScore;
			item.UID = model.UID;

            // TODO: Проверить на существование такого же приоритета - равные приоритеты нельзя (24395 - Сергей Герасимов)

            EntranceTestItemC foundItem = dbContext.EntranceTestItemC.FirstOrDefault(tic => tic.CompetitiveGroupID == model.CompetitiveGroupID && (model.TestItemID == null || tic.EntranceTestItemID != model.TestItemID) && tic.EntranceTestPriority == model.EntranceTestPriority);
            if (model.EntranceTestPriority != null && foundItem != null)
                return new AjaxResultModel(string.Format("Испытание с приоритетом {0} уже существует. Приоритеты испытаний должны различаться", model.EntranceTestPriority));

            item.EntranceTestPriority = model.EntranceTestPriority;
			int testItemID = model.TestItemID ?? 0;

			// проверяем на UID
			if (item.UID != null && dbContext.EntranceTestItemC.Any(x => 
				   x.CompetitiveGroup.InstitutionID == item.CompetitiveGroup.InstitutionID
				&& x.EntranceTestItemID != testItemID
				&& x.UID == item.UID))
				return new AjaxResultModel("Вступительное испытание с данным UID уже существует").SetIsError("UID", "");

			if (!isEdit)
				dbContext.EntranceTestItemC.AddObject(item);

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				SqlException inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("UK_EntranceTestItem"))
					return new AjaxResultModel("Уже существует испытание типа " + (item.Subject == null ? item.SubjectName : item.Subject.Name))
						{
							Data = new[] { new { ControlID = "EntranceTestID", Message = "" } }
						};
				throw;
			}

			bool canRemove = true;
			// можем ли удалить?
			if (item.EntranceTestTypeID == EntranceTestType.MainType)
			{
				//TODO: optimize
				EntranceTestViewModelC modelC = CreateEntranceTestDataByCompetitiveGroup(dbContext, new EntranceTestViewModelC(model.CompetitiveGroupID));
				if (modelC.TestItems.Length > 0)
					canRemove = modelC.TestItems.First(x => x.ItemID == item.EntranceTestItemID).CanRemove;
			}

			int benefitCount = 0;
			if (isEdit)
			{
				using (BenefitsEntities bContext = new BenefitsEntities())
				{
					benefitCount = bContext.GetBenefitItemCount(item.EntranceTestItemID, item.CompetitiveGroupID);
				}
			}

			// возвращаем клиенту сохранённую модель
			return new AjaxResultModel
			{
				Data = new EntranceTestViewModelC.EntranceTestItemData 
				{ 
					ItemID = item.EntranceTestItemID,
					TestName = item.Subject == null ? item.SubjectName : item.Subject.Name,
					UID = item.UID,
					//Form = item.Form,
					Value = item.MinScore,
                    EntranceTestPriority = item.EntranceTestPriority,
					CanRemove = canRemove,
					BenefitCount = benefitCount
				}
			};
		}

		/// <summary>
		/// Количество ВИ для данной КГ
		/// </summary>
		public static int GetEntranceTestCount(this EntrantsEntities dbContext, int groupID)
		{
			return dbContext.EntranceTestItemC.Count(x => x.CompetitiveGroupID == groupID);
		}

		/// <summary>
		/// Количество ВИ для списка групп
		/// </summary>
		public static Dictionary<int, int> GetEntranceTestCount(this EntrantsEntities dbContext, IList<int> groupList)
		{
			return dbContext.EntranceTestItemC.Where(x => groupList.Contains(x.CompetitiveGroupID))
				.GroupBy(x => x.CompetitiveGroupID).Select(x => new { ID = x.Key, Count = x.Count() })
				.ToDictionary(x => x.ID, x => x.Count);
		}
	}
}