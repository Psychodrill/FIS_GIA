using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;

using FogSoft.Helpers;

using GVUZ.Helper;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model;
using GVUZ.Model.Applications;
using GVUZ.Model.Benefits;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Web.Portlets.Applications;
using GVUZ.Web.ViewModels;
using EntranceTestType = GVUZ.Model.Entrants.EntranceTestType;
using GVUZ.Model.ApplicationPriorities;
using GVUZ.Model.Institutions;
using GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Вступительные испытания для заявления
	/// </summary>
	public static class ApplicationEntranceTestExtensions
	{
		/// <summary>
		/// Загружаем ВИ для заявления
		/// </summary>
		public static ApplicationEntranceTestViewModelC FillApplicationEntranceTestC(this EntrantsEntities dbContext, ApplicationEntranceTestViewModelC model, bool isView, EntrantKey key)
		{
			int entrantID = key.GetEntrantID(dbContext, !isView);
			if (entrantID == 0 && key.ApplicationID > 0)
				model.ShowDenyMessage = true;
			//List<CompetitiveGroupProperty> StudyPeriod = dbContext.GetCGProperties();
			//// все ви по конкурсным группам
			var entranceTests = (from a in dbContext.Application
								 join app_ in dbContext.ApplicationSelectedCompetitiveGroup
									 on a.ApplicationID equals app_.ApplicationID
								 join g in dbContext.CompetitiveGroup on app_.CompetitiveGroupID equals g.CompetitiveGroupID
								 join e in dbContext.EntranceTestItemC
									 on g.CompetitiveGroupID equals e.CompetitiveGroupID
								 where a.ApplicationID == key.ApplicationID
								 orderby e.EntranceTestTypeID, g.Name, e.SubjectName
								 select new
								 {
									 a.RegistrationDate,
									 a.EntrantID,
									 CompetitiveGroupID = e.CompetitiveGroupID,
									 CompetitiveGroupName = e.CompetitiveGroup.Name,
									 CGEduLevel = e.CompetitiveGroup.CompetitiveGroupItem.FirstOrDefault().CompetitiveGroup.Direction.QUALIFICATIONCODE.Trim(),
									 EntranceTestItemID = e.EntranceTestItemID,
									 EntranceTestType = e.EntranceTestTypeID,
									 SubjectName = e.SubjectID == null ? e.SubjectName : e.Subject.Name,
									 SubjectID = e.SubjectID == null ? 0 : e.SubjectID.Value,
									 IsEgeSubject = e.SubjectID != null && e.Subject.IsEge,

									 Priority = e.EntranceTestPriority,

								 }).ToArray();

			model.EntranceTests = entranceTests.Select(x => new ApplicationEntranceTestViewModelC.EntranceTestData
			{
				CompetitiveGroupID = x.CompetitiveGroupID,
				CompetitiveGroupName = x.CompetitiveGroupName,
				EntranceTestItemID = x.EntranceTestItemID,
				EntranceTestType = x.EntranceTestType,
				SubjectName = x.SubjectName,
				SubjectID = x.SubjectID,
				IsEgeSubject = x.IsEgeSubject,
				Priority = x.Priority,
				CompetitiveGroupEduLevelCode = x.CGEduLevel,
				//StudyPeriod = dbContext.GetCGProperties(x.CompetitiveGroupID).FirstOrDefault().PropertyValue



			}).ToArray();



            var appCompGroups = entranceTests != null ? entranceTests.Select(x => x.CompetitiveGroupID).Distinct().ToArray() : new int[0];
            int origEntrantID = entranceTests.Any()? entranceTests.First().EntrantID: 0;
            if (origEntrantID == 0 && entrantID > 0) origEntrantID = entrantID;

             var registrationDate = entranceTests.Any()? entranceTests.First().RegistrationDate: DateTime.Now;

           
			//льготы
			using (BenefitsEntities bContext = new BenefitsEntities())
			{
				int?[] existingBenefits = bContext.BenefitItemC
					.Where(x => appCompGroups.Contains(x.CompetitiveGroupID))
					.Select(x => x.EntranceTestItemID).Distinct().ToArray();
				foreach (var etID in existingBenefits)
				{
					int? id = etID;
					var etData = model.EntranceTests.FirstOrDefault(x => x.EntranceTestItemID == id);
					if (etData != null) etData.HasBenefits = true;
				}
			}
			
			//проставляем профильные предметы
			foreach (var appCompGroup in appCompGroups)
			{
				int profileSubjectID = dbContext.GetProfileSubjectID(appCompGroup);
				if (profileSubjectID > 0)
					model.EntranceTests.Where(x => x.CompetitiveGroupID == appCompGroup && x.SubjectID == profileSubjectID)	.ToList().ForEach(x => x.IsProfileSubject = true);
			}

			//документы из заявления, прикрёпленные к ви
			model.AttachedDocs = dbContext.ApplicationEntranceTestDocument
                .Where(x => x.ApplicationID == key.ApplicationID && x.EntranceTestItemID != null)
				.Select(x => new ApplicationEntranceTestViewModelC.AttachedDocumentData
				             {
                                 BenefitID = x.BenefitID == null ? (Int16)0 : x.BenefitID.Value,
								EntranceTestItemID = x.EntranceTestItemID == null ? 0 : x.EntranceTestItemID.Value,
								CompetitiveGroupID = x.EntranceTestItemC.CompetitiveGroupID,
								SourceID = x.SourceID == null ? 0 : x.SourceID.Value,
								EntrantDocumentID = x.EntrantDocumentID == null ? 0 : x.EntrantDocumentID.Value,
								ResultValue = x.ResultValue == null ? 0 : x.ResultValue.Value,
                                EgeResultValue = x.EgeResultValue == null ? 0 : x.EgeResultValue.Value,
								DocumentTypeID = x.EntrantDocument == null ? 0 : x.EntrantDocument.DocumentTypeID,
								ID = x.ID,
								SubjectID = x.SubjectID == null ? 0 : x.SubjectID.Value,
								InstitutionDocumentDate = x.InstitutionDocumentDate,
								InstitutionDocumentNumber = x.InstitutionDocumentNumber,
								InstitutionDocumentTypeName = x.InstitutionDocumentTypeID != null ? x.InstitutionDocumentType.Name : null
				             })
				.ToArray();
			//документы, прикрёпленные к льготам
			model.GlobalDocs = dbContext.ApplicationEntranceTestDocument
                            .Where(x => x.ApplicationID == key.ApplicationID && x.EntranceTestItemID == null)
							.Select(x => new ApplicationEntranceTestViewModelC.AttachedDocumentData
							{
                                BenefitID = x.BenefitID == null ? (Int16)0 : x.BenefitID.Value,
								EntrantDocumentID = x.EntrantDocumentID == null ? 0 : x.EntrantDocumentID.Value,
								ID = x.ID,
								CompetitiveGroupID = x.CompetitiveGroupID ?? 0,
							})
							.ToArray();
			var forRemove = new List<int>();

			//по всем прикреплённым документам берём текстовое описание
			foreach (var data in model.AttachedDocs)
			{
				if (data.EntrantDocumentID > 0)
				{
					data.DocumentDescription = GetDocDescription(dbContext, 
                        data.EntrantDocumentID, data.SubjectID, data.EntranceTestItemID, data.CompetitiveGroupID);
				    if (data.DocumentDescription == null)
				        //forRemove.Add(data.ID);
				        data.DocumentDescription = "Для данного предмета нет результата в указанном сертификате ЕГЭ";
				}
				else
				{
					if (data.SourceID == EntranceTestSource.OUTestSourceId)
					{
						if (data.InstitutionDocumentTypeName != null)
							data.DocumentDescription = String.Format("{2} № {0} от {1:dd.MM.yyyy}", data.InstitutionDocumentNumber,
							data.InstitutionDocumentDate, data.InstitutionDocumentTypeName);
						else
							data.DocumentDescription = OUTestName;
					}

					if (data.SourceID == EntranceTestSource.EgeDocumentSourceId)
						data.DocumentDescription = EgeDocName + " (не указано)";
					if (data.SourceID == EntranceTestSource.GiaDocumentSourceId)
						data.DocumentDescription = GiaDocName + " (не указана)";
					if (data.SourceID == EntranceTestSource.OlympiadSourceId)
						data.DocumentDescription = DiplomaDocName + " (не указан)";
				}
			}

			// текстовое описание по льготам
			foreach (var globalDoc in model.GlobalDocs)
			{
				globalDoc.DocumentDescription = GetDocDescription(dbContext, globalDoc.EntrantDocumentID, 0, 0, globalDoc.CompetitiveGroupID);
				string benefitErrorMessage;
				if (ApplicationCountValidator.IsCommonBenefitMoreThanOnce(key.ApplicationID, registrationDate, globalDoc.BenefitID, out benefitErrorMessage))
					globalDoc.BenefitErrorMessage = benefitErrorMessage;
			}

			if (forRemove.Count > 0) //документы невалидны, надо их убрать
			{
				dbContext.ApplicationEntranceTestDocument.Where(x => forRemove.Contains(x.ID))
					.ToList()
					.ForEach(dbContext.ApplicationEntranceTestDocument.DeleteObject);
				//model.AttachedDocs = model.AttachedDocs.Where(x => !forRemove.Contains(x.ID)).ToArray();
			}

            model.ApplicationID = key.ApplicationID;
			model.EntrantID = origEntrantID; //для просмотра/добавления документов
			model.HasEgeDocuments = model.AttachedDocs.Any(x => x.DocumentTypeID == 2);


			model.Course = dbContext.CompetitiveGroup
				.Where(x => appCompGroups.Contains(x.CompetitiveGroupID)).Select(x => x.Course)
				.FirstOrDefault();
			if (model.Course == 0) model.Course = 1;

			// институтские типы документов
			model.InstitutionDocumentTypes = dbContext.InstitutionDocumentType.OrderBy(x => x.DisplayOrder).Select(x => new { ID = x.InstitutionDocumentTypeID, Name = x.Name }).ToArray();

            // Определим, есть ли среди выбранных источников финансирования источник "Квота" (для краткости - так, кто в курсе - поймёт)

            using (ApplicationPrioritiesEntities prioritiesContext = new ApplicationPrioritiesEntities())
            {
                var selectedCompetttiveGroups = model.EntranceTests.Select(x => x.CompetitiveGroupID).Distinct();

                model.IsQuotaBenefitEnabled = prioritiesContext.ApplicationCompetitiveGroupItem
                    .Include("Campaign")
                    .Any(x =>
                    x.ApplicationId == model.ApplicationID &&
                    selectedCompetttiveGroups.Contains(x.CompetitiveGroupId) &&
                    x.EducationSourceId == EDSourceConst.Quota);

                var isAnyCampainAfter2014 = prioritiesContext.CompetitiveGroup.Where(x => selectedCompetttiveGroups.Contains(x.CompetitiveGroupID)).Any(x => x.Campaign.YearStart >= 2014);

                model.IsQuotaBenefitEnabled = (isAnyCampainAfter2014 && model.IsQuotaBenefitEnabled) || (!isAnyCampainAfter2014);
            }

			return model;
		}

		private const string EgeDocName = "Свидетельство ЕГЭ";
		private const string GiaDocName = "Справка ГИА";
		private const string DiplomaDocName = "Диплом победителя/призера олимпиады";
		private const string DiplomaTotalDocName = "Диплом победителя/призера ВОШ";
		private const string OUTestName = "Вступительное испытание ОО";
		private const string OtherDocName = "Иной документ";

		/// <summary>
		/// Получем текстовое описание документа
		/// </summary>
		public static string GetDocDescription(this EntrantsEntities dbContext, int documentID, int subjectID, int entranceTestItemID, int competitiveGroupID)
		{
			BaseDocumentViewModel doc;
			return GetDocDescription(dbContext, documentID, subjectID, entranceTestItemID, competitiveGroupID, out doc);
		}

		/// <summary>
		/// Получем текстовое описание документа и возвращаем модель с документом. Если документ некорректен, возвращаем null
		/// </summary>
		private static string GetDocDescription(this EntrantsEntities dbContext, int documentID, int subjectID, int entranceTestItemID, int competitiveGroupID, out BaseDocumentViewModel relatedDoc)
		{
			BaseDocumentViewModel baseDoc = dbContext.LoadEntrantDocument(documentID);
            var egeDoc = baseDoc as EGEDocumentViewModel;
			relatedDoc = null;
			if (egeDoc != null) //ЕГЭ
			{
                egeDoc.FillData(dbContext, true, null, null);
                if (egeDoc.Subjects.All(x => x.SubjectID != subjectID)) //предмет не подходит
				{
                    if (DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).Any(x => x.Key == subjectID && x.Value == LanguageSubjects.ForeignLanguage))
					{
                        if (!egeDoc.Subjects.Any(x => x.SubjectName.StartsWith(LanguageSubjects.ForeignLanguagePrefix)))
							return null;
					}
                    else return null; //если предмета нет в ЕГЭ, то надо об этом сообщить, иначе его нельзя открепить потом
				}

				relatedDoc = egeDoc;
				return String.Format(EgeDocName + "{0} от {1:yyyy} года", 
                    !string.IsNullOrEmpty(egeDoc.DocumentNumber) ? " № " + egeDoc.DocumentNumber : string.Empty, egeDoc.DocumentDate);
			}

			var giaDoc = baseDoc as GiaDocumentViewModel;
			if (giaDoc != null) //ГИА
			{
                giaDoc.FillData(dbContext, true, null, null);
				if (giaDoc.Subjects.All(x => x.SubjectID != subjectID)) //предмет не подходит
				{
                    if (dbContext.Subject.Any(x => x.SubjectID == subjectID && x.Name == LanguageSubjects.ForeignLanguage))
					{
                        if (!giaDoc.Subjects.Any(x => x.SubjectName.StartsWith(LanguageSubjects.ForeignLanguagePrefix)))
							return null;
					}
					else return null;
				}

				relatedDoc = giaDoc;
                return String.Format(GiaDocName + "{0} от {1:dd.MM.yyyy}",
                    !string.IsNullOrEmpty(giaDoc.DocumentNumber) ? " № " + giaDoc.DocumentNumber : string.Empty, giaDoc.DocumentDate);
			}

			var olDoc = baseDoc as OlympicDocumentViewModel;
			if (olDoc != null) //олимпиада
			{
                olDoc.FillData(dbContext, true, null, null);
				if (subjectID > 0 && !olDoc.OlympicDetails.SubjectIDs.Any(x => x == subjectID)) //предмет не подходит
					return null;
				//Проверка на разрешённые бенефиты
				using (var bContext = new BenefitsEntities())
				{
					if (!olDoc.DiplomaTypeID.HasValue)
						olDoc.DiplomaTypeID = 0;

                    var allowedBenefitItems = new List<BenefitOlympicEntity>();
					foreach (var level in olDoc.OlympicDetails.LevelIDs)
				    {
                        int flagOlympic = 0;
                        if (level == 2) flagOlympic |= 1;
                        if (level == 3) flagOlympic |= 2;
                        if (level == 4) flagOlympic |= 4;

                        if (level == 1) flagOlympic = 7;

                        //вытаскиваем бенефиты для данного диплома
                        allowedBenefitItems.AddRange(
                        bContext.BenefitItemC
                            .Where(x => ((entranceTestItemID > 0 && x.EntranceTestItemID == entranceTestItemID) || (entranceTestItemID == 0 && x.EntranceTestItemID == null))
                                && (x.OlympicDiplomTypeID & olDoc.DiplomaTypeID) != 0 && (x.OlympicLevelFlags & flagOlympic) != 0)
                           .Select(x => new BenefitOlympicEntity
                               {
                                   BenefitItemID = x.BenefitItemID,
                                   IsForAllOlympic = x.IsForAllOlympic 
                               }));
				    }

                    if (allowedBenefitItems.Count(x => x.IsForAllOlympic) == 0) //если есть льгота для всех олимпиад, то отлично
					{
						//иначе тащим олимпиады и ищем совпадения
                        var allowedBenefitItemIDs = allowedBenefitItems.Select(x => x.BenefitItemID).ToArray();
						var allowedOlympics = bContext.BenefitItemCOlympicType.Where(x => x.BenefitItemC.EntranceTestItemID == entranceTestItemID &&                                 allowedBenefitItemIDs.Contains(x.BenefitItemID))
							.Select(x => x.OlympicTypeID).ToArray();
						if (!allowedOlympics.Contains(olDoc.OlympicID))
							return null;
					}
				}

                
				relatedDoc = olDoc;
			    var olympic = dbContext.OlympicType.SingleOrDefault(c => c.OlympicID == olDoc.OlympicID);
			    return String.Format(DiplomaDocName + " \"{1}\" № {0}",
			                         olDoc.DocumentNumber,
			                         olympic != null ? olympic.Name : "");
			    //String.Join(", ", olDoc.OlympicDetails.SubjectNames));
			}

			var disDoc = baseDoc as DisabilityDocumentViewModel;
			if (disDoc != null)
			{
				relatedDoc = disDoc;
				return String.Format("Справка об установлении инвалидности № {2} {0} от {1:dd.MM.yyyy}", disDoc.DocumentNumber, disDoc.DocumentDate, disDoc.DocumentSeries);
			}

			var cusDoc = baseDoc as CustomDocumentViewModel;
			if (cusDoc != null)
			{
				relatedDoc = cusDoc;
				return String.Format("{2} № {3} {0} от {1:dd.MM.yyyy}", cusDoc.DocumentNumber, cusDoc.DocumentDate, cusDoc.DocumentTypeNameText, cusDoc.DocumentSeries);
			}

			var totDoc = baseDoc as OlympicTotalDocumentViewModel;
			if (totDoc != null) //ВОШ
            {
#warning https://redmine.armd.ru/issues/18408
                //if (entranceTestItemID > 0) //даём 100 баллов, если есть предмет
                //{
                //    if (!totDoc.Subjects.Select(x => x.SubjectID).Contains(subjectID))
                //        return null;
                //}
                //else //даём льготу, если совпадает с профилем
                //{
                //    int profileSubjectID = dbContext.GetProfileSubjectID(competitiveGroupID);
                //    if (!totDoc.Subjects.Select(x => x.SubjectID).Contains(profileSubjectID))
                //        return null;
                //}

				relatedDoc = totDoc;
				return String.Format(DiplomaTotalDocName + " {3} {2} № {0} от {1:dd.MM.yyyy}", totDoc.DocumentNumber, totDoc.DocumentDate, totDoc.OlympicPlace, totDoc.DocumentSeries);
			}

			var psyDoc = baseDoc as PsychoDocumentViewModel;
			if (psyDoc != null)
			{
				return String.Format("Заключение психолого-медико-педагогической комиссии № {0} от {1:dd.MM.yyyy}", psyDoc.DocumentNumber, psyDoc.DocumentDate);
			}

			return null;
		}

		/// <summary>
		/// Возвращаем разрешённые типы документов (и сами документы, если есть) для льгот
		/// </summary>
		public static AjaxResultModel GetAllowedDocumentDocumentsForEntranceTestGlobal(this EntrantsEntities dbContext, int applicationID, int docSourceID, int groupID)
		{
			Application app = dbContext.GetApplication(applicationID);
			int entrantID = app.EntrantID;
			var model = new ApplicationEntranceTestViewModelC.SelectionDocumentsForEntranceTest { EntranceTestItemID = 0 };
			ApplicationEntranceTestDocument doc = dbContext.ApplicationEntranceTestDocument
				.FirstOrDefault(x => x.ApplicationID == applicationID && x.EntranceTestItemID == null);

			if (doc != null && doc.EntrantDocumentID.HasValue)
			{
				model.DocCurrent = new[]
					{
						new ApplicationEntranceTestViewModelC.SelectionDocumentData
							{
								DocumentID = doc.EntrantDocumentID.Value,
								Description = GetDocDescription(dbContext, doc.EntrantDocumentID.Value, 0, 0, groupID)
							}
					};

			    model.DocCurrent = model.DocCurrent.Where(c => !string.IsNullOrEmpty(c.Description)).ToArray();
			}

			int[] reqDocTypeID = docSourceID == 102 ? new[] { 9, 10, 15 } : (docSourceID == 104 ? new[] { 11, 12, 15 } : new[] { 15 });
			var docIDs = dbContext.EntrantDocument.Where(x => x.EntrantID == entrantID && reqDocTypeID.Contains(x.DocumentTypeID)).Select(x => new { x.EntrantDocumentID, x.DocumentTypeID }).ToArray();
			var l = new List<ApplicationEntranceTestViewModelC.SelectionDocumentData>();
			foreach (var docID in docIDs)
			{
                string egeDescription = GetDocDescription(dbContext, docID.EntrantDocumentID, 0, 0, groupID);
				if (egeDescription != null)
					l.Add(new ApplicationEntranceTestViewModelC.SelectionDocumentData
					{
                        DocumentID = docID.EntrantDocumentID,
						Description = egeDescription,
                        TypeID = docID.DocumentTypeID
					});
			}
            //Ещё надо проверить документы (льготы), поданые в другие ВУЗы 

		    var otherEntrantDocuments =
                dbContext.EntrantDocument.Where(
                    x =>
                    x.Entrant.EntrantDocument_Identity.DocumentSeries.Equals(
                        app.Entrant.EntrantDocument_Identity.DocumentSeries) &&
                        x.Entrant.EntrantDocument_Identity.DocumentNumber.Equals(
                        app.Entrant.EntrantDocument_Identity.DocumentNumber) &&
                        x.Entrant.Application.Select(y => y.ApplicationID != app.ApplicationID).FirstOrDefault()
                        ).Select(x => new { x.EntrantDocumentID, x.DocumentTypeID }).ToArray();
            foreach (var otherDocument in otherEntrantDocuments)
            {
                string egeDescription = GetDocDescription(dbContext, otherDocument.EntrantDocumentID, 0, 0, groupID);
                if (egeDescription != null && !l.Any(x => x.DocumentID == otherDocument.EntrantDocumentID))
                    l.Add(new ApplicationEntranceTestViewModelC.SelectionDocumentData
                    {
                        DocumentID = otherDocument.EntrantDocumentID,
                        Description = egeDescription,
                        TypeID = otherDocument.DocumentTypeID
                    });
            }

			model.DocExisting = l.ToArray();

			if (docSourceID == 102)
			{
				var al = new List<ApplicationEntranceTestViewModelC.SelectionDocumentData> 
				{ 
					new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 9, Description = DiplomaDocName },
					new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 10, Description = DiplomaTotalDocName },
					new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 15, Description = OtherDocName }
				};
				using (BenefitsEntities bContext = new BenefitsEntities())
				{
					int cnt =
						bContext.BenefitItemC.Count(x => x.CompetitiveGroupID == groupID && x.EntranceTestItemID == null);
					if (cnt == 0) //не даём выбрать обычный диплом, если нет льготы
						al.RemoveAt(0);
				}

				model.DocAdd = al.ToArray();
			}

			if (docSourceID == 104)
			{
				model.DocAdd = new[] 
				{ 
							new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 11, Description = "Справка об установлении инвалидности" },
							new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 12, Description = "Заключение психолого-медико-педагогической комиссии" },
							new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 15, Description = OtherDocName }
				};
			}

			if (docSourceID == 105)
			{
				model.DocAdd = new[] 
				{ 
							new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 15, Description = OtherDocName }
				};
			}

			CheckDiplomaBenefitUsed(dbContext, applicationID, model);
			return new AjaxResultModel { Data = model };
		}

		/// <summary>
		/// Возвращаем разрешённые типы документов (и сами документы, если есть) для РВИ
		/// </summary>
		public static AjaxResultModel GetAllowedDocumentDocumentsForEntranceTest(this EntrantsEntities dbContext, int applicationID, int entranceTestItemID, int docSourceID, int groupID)
		{
			if (entranceTestItemID == 0)
			{
				return GetAllowedDocumentDocumentsForEntranceTestGlobal(dbContext, applicationID, docSourceID, groupID);
			}

			Application app = dbContext.GetApplication(applicationID);
			int entrantID = app.EntrantID > 0 ? app.EntrantID : app.EntrantID;
			var model = new ApplicationEntranceTestViewModelC.SelectionDocumentsForEntranceTest { EntranceTestItemID = entranceTestItemID };
			var testData = dbContext.EntranceTestItemC.Where(x => x.EntranceTestItemID == entranceTestItemID)
				.Select(x => new { x.EntranceTestTypeID, SubjectID = x.SubjectID.HasValue ? x.SubjectID.Value : 0, x.CompetitiveGroupID }).Single();

			ApplicationEntranceTestDocument doc = dbContext.ApplicationEntranceTestDocument
				.FirstOrDefault(x => x.ApplicationID == applicationID && x.EntranceTestItemID == entranceTestItemID);

			if (doc != null && doc.EntrantDocumentID.HasValue)
			{
				model.DocCurrent = new[] 
				{ 
					new ApplicationEntranceTestViewModelC.SelectionDocumentData
										{
											DocumentID = doc.EntrantDocumentID.Value,
											Description = GetDocDescription(dbContext, doc.EntrantDocumentID.Value, 
                                            testData.SubjectID, entranceTestItemID, testData.CompetitiveGroupID)
										}
				};
                model.DocCurrent = model.DocCurrent.Where(c => !string.IsNullOrEmpty(c.Description)).ToArray();
			}

			int[] reqDocTypeID = new[] { 9, 10 };
			if (docSourceID == EntranceTestSource.EgeDocumentSourceId)
				reqDocTypeID = new[] { 2 };
			if (docSourceID == EntranceTestSource.GiaDocumentSourceId)
				reqDocTypeID = new[] { 17 };
			var docIDs = dbContext.EntrantDocument.Where(x => x.EntrantID == entrantID && reqDocTypeID.Contains(x.DocumentTypeID)).Select(x => new { x.EntrantDocumentID, x.DocumentTypeID }).ToArray();
			var l = new List<ApplicationEntranceTestViewModelC.SelectionDocumentData>();
			for (int i = 0; i < docIDs.Length; i++)
			{
                string egeDescription = GetDocDescription(dbContext, docIDs[i].EntrantDocumentID, testData.SubjectID, entranceTestItemID, testData.CompetitiveGroupID);
                if (egeDescription != null)
                    l.Add(new ApplicationEntranceTestViewModelC.SelectionDocumentData
                    {
                        DocumentID = docIDs[i].EntrantDocumentID,
                        Description = egeDescription,
                        TypeID = docIDs[i].DocumentTypeID
                    });
			}

			model.DocExisting = l.ToArray();
			if (docSourceID == EntranceTestSource.EgeDocumentSourceId)
			{
				model.DocAdd = new[] { new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 2, Description = EgeDocName } };
			}
			else if (docSourceID == EntranceTestSource.GiaDocumentSourceId)
			{
				model.DocAdd = new[] { new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 17, Description = GiaDocName } };
			}
			else
			{
				var al = new List<ApplicationEntranceTestViewModelC.SelectionDocumentData>
				         {
				         	new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 9, Description = DiplomaDocName },
				         	new ApplicationEntranceTestViewModelC.SelectionDocumentData { TypeID = 10, Description = DiplomaTotalDocName }
				         };
				using (var bContext = new BenefitsEntities())
				{
					int cnt =
						bContext.BenefitItemC.Count(x => x.CompetitiveGroupID == testData.CompetitiveGroupID && x.EntranceTestItemID == entranceTestItemID);
					if (cnt == 0) //не даём выбрать обычный диплом, если нет льготы
						al.RemoveAt(0);
				}
				//если предмет введён вручную, то не даём выбрать Диплом Всероссийской Олимпиады (там предметы указываются)
				if (dbContext.EntranceTestItemC.Where(x => x.EntranceTestItemID == entranceTestItemID && x.SubjectID == null).Any())
				{
					al = al.Where(x => x.TypeID != 10).ToList();
				}

				model.DocAdd = al.ToArray();

				//исключено из постановок
				//CheckDiplomaBenefitUsed(dbContext, applicationID, model);
			}

			return new AjaxResultModel { Data = model };
		}

		/// <summary>
		/// Можно ли вводить институтские испытания
		/// </summary>
		public static AjaxResultModel GetAbilityToEnterManualValue(this EntrantsEntities dbContext, int applicationID, int entranceTestItemID)
		{
			var m = GetAbilityToEnterManualValueInternal(dbContext, applicationID, entranceTestItemID);
			return new AjaxResultModel { Data = m };
		}

		/// <summary>
		/// Можно ли вводить институтские испытания
		/// </summary>
		public static ApplicationEntranceTestViewModelC.SelectionDocumentsForEntranceTestManual 
			GetAbilityToEnterManualValueInternal(this EntrantsEntities dbContext, int applicationID, int entranceTestItemID)
		{
			Application app = dbContext.GetApplication(applicationID);
			int entrantID = app.EntrantID > 0 ? app.EntrantID : app.EntrantID;
			var model = new ApplicationEntranceTestViewModelC.SelectionDocumentsForEntranceTestManual
				{
					EntranceTestItemID = entranceTestItemID 
				};
			model.CanAddManualValue = false;

			var testData =
				dbContext.EntranceTestItemC.Where(x => x.EntranceTestItemID == entranceTestItemID)
				.Select(x => new
						{
							x.EntranceTestTypeID,
							SubjectID = x.SubjectID.HasValue ? x.SubjectID.Value : 0,
							SubjectName = x.SubjectID.HasValue ? x.Subject.Name : x.SubjectName,
							CompetitiveGroupID = x.CompetitiveGroupID
						}).Single();

			model.SubjectName = testData.SubjectName;

			// можем для неосновных испытаний
			if (testData.EntranceTestTypeID != EntranceTestType.MainType)
			{
				model.CanAddManualValue = true;
				return model;
			}

		    // не можем если не проверяли ещё ЕГЭ
			if (app.LastEgeDocumentsCheckDate == null && testData.SubjectID > 0)
			{
				model.CanAddManualValue = false;
				model.ShouldCheckEgeBefore = true;
				return model;
			}

			ApplicationEntranceTestDocument doc =
				dbContext.ApplicationEntranceTestDocument.FirstOrDefault(
					x => x.ApplicationID == applicationID && x.EntranceTestItemID == entranceTestItemID);

			// не можем, если уже есть документ
			if (doc != null && doc.EntrantDocumentID.HasValue)
			{
				model.CanAddManualValue = false;
				return model;
			}

			// вытаскиваем список свидетельств ЕГЭ
			var docIDs =
				dbContext.EntrantDocument.Where(x => x.EntrantID == entrantID && x.DocumentTypeID == 2)
				.Select(x => new { x.EntrantDocumentID, x.DocumentTypeID }).ToArray();
			var l = new List<ApplicationEntranceTestViewModelC.SelectionDocumentData>();
			for (int i = 0; i < docIDs.Length; i++)
			{
				BaseDocumentViewModel relatedDoc;
				string egeDescription = GetDocDescription(
					dbContext,
					docIDs[i].EntrantDocumentID,
					testData.SubjectID,
					entranceTestItemID,
					testData.CompetitiveGroupID,
					out relatedDoc);
				
                // если есть свидетельство подходящее, то добавляем в список
				if (egeDescription != null &&
				    ((relatedDoc is EGEDocumentViewModel && relatedDoc.DocumentDate.HasValue
				      && relatedDoc.DocumentDate.Value.Year == DateTime.Today.Year) || !(relatedDoc is EGEDocumentViewModel)))
				{
					l.Add(new ApplicationEntranceTestViewModelC.SelectionDocumentData
							{
								DocumentID = docIDs[i].EntrantDocumentID,
								Description = egeDescription,
								TypeID = docIDs[i].DocumentTypeID 
							});
				}
			}

			// возвращаем список существующих документов, и не разрешам вводить, если они уже есть
			model.DocExisting = l.ToArray();
			model.CanAddManualValue = l.Count == 0;

            /* Нет документов - надо будет проверить в ФБС еще раз */
            if (l.Count == 0)
                model.ShouldCheckEgeBefore = true;

			return model;
		}

		/// <summary>
		/// Используется ли уже льгота
		/// </summary>
		private static void CheckDiplomaBenefitUsed(EntrantsEntities dbContext, int applicationID, ApplicationEntranceTestViewModelC.SelectionDocumentsForEntranceTest model) 
		{
			bool isDiplomaUsed = dbContext.ApplicationEntranceTestDocument
				.Any(x => x.ApplicationID == applicationID && x.EntrantDocument.DocumentTypeID == 9);
			if (isDiplomaUsed)
			{
				var sdoc = model.DocAdd.Where(x => x.TypeID == 9).ToArray();
				foreach (var doc in sdoc)
					doc.DenyMessage = "Льгота на основе диплома победителя/призера олимпиады школьников уже используется";
					
				sdoc = model.DocExisting.Where(x => x.TypeID == 9).ToArray();
				foreach (var doc in sdoc)
					doc.DenyMessage = "Льгота на основе диплома победителя/призера олимпиады школьников уже используется";
			}
		}

		/// <summary>
		/// Проверяем ЕГЭ
		/// </summary>
		public static AjaxResultModel CheckEgeResults(this EntrantsEntities dbContext, int applicationID, 
            bool findCertificatesOfCurrentYearOnly, int? etId)
		{
			Dictionary<int, ApplicationValidationErrorInfo> res =
                dbContext.ValidateEgeDocuments(dbContext.GetApplication(applicationID), findCertificatesOfCurrentYearOnly, etId);
			var answ = res.Select(x => new { EntranceTestItemID = x.Key, Result = x.Value.Message/*, DocData = getDocData(x.Key, x.Value)*/ }).ToArray();
			
			return new AjaxResultModel { Data = answ };
		}

		/// <summary>
		/// Получаем результаты проверки ЕГЭ
		/// </summary>
		public static AjaxResultModel GetEgeResults(this EntrantsEntities dbContext, int applicationID, string certificateNumber)
		{
			List<ApplicationValidationErrorInfo> errors;
			var model = dbContext.GetEgeDocumentByFIO(dbContext.GetApplication(applicationID), certificateNumber, null, out errors);
			if (model == null)
				return new AjaxResultModel(String.Join(", ", errors.Select(x => x.Message)));

			return new AjaxResultModel { Data = model };
		}
	}

    public class BenefitOlympicEntity
    {
        public int BenefitItemID { get; set; }
        public bool IsForAllOlympic { get; set; }
    }
}