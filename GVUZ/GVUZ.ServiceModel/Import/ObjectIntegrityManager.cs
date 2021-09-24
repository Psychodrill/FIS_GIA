using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Core;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections;
using System.Text;
using System.Data.Objects;

namespace GVUZ.ServiceModel.Import
{
	public class ObjectIntegrityManager : StorageConsumer
	{
		public ObjectIntegrityManager(StorageManager storageManager) : base(storageManager)
		{
		}

		/// <summary>
		/// Количество мест импортируемой специальности не должно быть меньше суммарного количества мест по 
		/// данной специальности в конкурсных группах (с учетом совпадения параметров - форма обучения, целевой прием и т.п.).
		/// </summary>
		public void CheckPlacesInDirectionBetweenAdmissionVolumeAndCompetitiveGroupItems(AdmissionInfoDto admissionInfoDto)
		{
			if (admissionInfoDto.CompetitiveGroups == null) return;

			CheckUIDUnique(admissionInfoDto);

			CheckRestrictionOnDirectionsForAdmissionAndCompetitiveGroup(admissionInfoDto);
			CheckRestrictionOnProfileAndCreativeDirection(admissionInfoDto); // только CompetitiveGroup
            CheckRestrictionOnEntranceSubjectInDirectionsForCompetitiveGroup(admissionInfoDto); // только CompetitiveGroup
            CheckIntegrityInCompetitiveGroups(admissionInfoDto); // только CompetitiveGroup
            CompetitiveGroupNameMustBeUnique(admissionInfoDto); // только CompetitiveGroup
            CheckBenefits(admissionInfoDto); // только CompetitiveGroup
            CheckDistributedAdmissionVolume(admissionInfoDto);

			//вытаскиваем все разрешённые варианты и проверяем на них количество
            // 2016 - убираем!
			//var allowedCampaignvariants = DbObjectRepository.Campaigns.SelectMany(x => x.CampaignEducationLevel)
			//	.Select(x => new { x.Campaign.UID, x.Course, x.EducationLevelID }).Distinct().ToArray();
			//foreach (var allowedCampaignvariant in allowedCampaignvariants)
			//{
			//	CheckPlacesInDirectionBetweenAdmissionVolumeAndCompetitiveGroupItems(admissionInfoDto,
			//		allowedCampaignvariant.EducationLevelID,
			//		allowedCampaignvariant.Course,
			//		allowedCampaignvariant.UID);
			//}

			// проверяем минимальные баллы в КГ
            CheckMinSubjectValueInCompetitiveGroups(admissionInfoDto); // только CompetitiveGroup
		}

		#region Проверка справочных значений

	    public void CheckDictionaries(AdmissionInfoDto admissionInfoDto)
		{
			foreach (var cgDto in admissionInfoDto.CompetitiveGroups)
			{
				//проверка перенесена в CheckRestrictionOnDirectionsForAdmissionAndCompetitiveGroup, т.к. там проверяется раньше
				//и данная ошибка уже не проявится, поскольку вышибет по другому условию
				//CheckDictionaryValues(cgDto.EducationLevelID, cgDto, DbObjectRepository.GetEducationalLevel);

				foreach (var entrItemDto in cgDto.EntranceTestItems)
					if (entrItemDto.EntranceTestBenefits != null)
						foreach (var entranceTestBenefitDto in entrItemDto.EntranceTestBenefits)
						{
							// не очень хорошее место присваивать тут UID, но текущий код организован так, что это самое удобное место
							if (entranceTestBenefitDto.ParentUID == null)
								entranceTestBenefitDto.ParentUID = entrItemDto.UID;
							if (entrItemDto.ParentUID == null)
								entrItemDto.ParentUID = cgDto.UID;

							CheckDictionaryValues(entranceTestBenefitDto.OlympicDiplomTypes, "OlympicDiplomTypeID", entrItemDto, DbObjectRepository.GetOlympicDiplomType);
							CheckUniqueValues(entranceTestBenefitDto.OlympicDiplomTypes, "OlympicDiplomTypeID", entrItemDto);

                            if (!entranceTestBenefitDto.IsForAllOlympics.To(false) && entranceTestBenefitDto.OlympicsLevels != null)
                                CheckDictionaryValues(entranceTestBenefitDto.OlympicsLevels.Select(c => c.OlympicID), "OlympicID", entrItemDto, DbObjectRepository.GetOlympicType);
                            if (entranceTestBenefitDto.Olympics != null)
                                CheckDictionaryValues(entranceTestBenefitDto.Olympics, "OlympicID", entrItemDto, DbObjectRepository.GetOlympicType);

							// для всех олимпиад, но передали олимпиады - ошибка.
							if (entranceTestBenefitDto.IsForAllOlympics.To(false))
						    {
						        if (entranceTestBenefitDto.Olympics != null && entranceTestBenefitDto.Olympics.Length > 0)
						            ConflictStorage.AddNotImportedDto(entranceTestBenefitDto, ConflictMessages.BenefitContainsOlympicsAndAllOlympics);
                                if (entranceTestBenefitDto.OlympicsLevels != null && 
                                    entranceTestBenefitDto.OlympicsLevels.Any(c => !string.IsNullOrEmpty(c.OlympicID)))
                                    ConflictStorage.AddNotImportedDto(entranceTestBenefitDto, ConflictMessages.BenefitContainsOlympicsAndAllOlympics);
                            }

						    // не для всех олимпиад и ничего не передали - ошибка
                            if (!entranceTestBenefitDto.IsForAllOlympics.To(false))
                            {
                                if ((entranceTestBenefitDto.Olympics == null || entranceTestBenefitDto.Olympics.Length == 0) &&
                                    (entranceTestBenefitDto.OlympicsLevels == null || entranceTestBenefitDto.OlympicsLevels.Length == 0))
                                    ConflictStorage.AddNotImportedDto(entranceTestBenefitDto, ConflictMessages.BenefitContainsNoOlympicsAndNoAllOlympics);
                            }

							CheckDictionaryValues(entranceTestBenefitDto.BenefitKindID, "BenefitKindID", entrItemDto, DbObjectRepository.GetBenefit);

							//#22769 При указании вида льготы для ВИ можно указать любой ид. Для основных ВИ должен быть доступен вид льготы только "100 баллов по осн ВИ", для профильных только "100 баллов для доп ВИ"
							if (entrItemDto.EntranceTestTypeID == "1" && entranceTestBenefitDto.BenefitKindID != "3")
							{
								ConflictStorage.AddNotImportedDto(entranceTestBenefitDto, ConflictMessages.CompetitiveGroupContainsNotAllowedBenefitType);
							}

							if (entrItemDto.EntranceTestTypeID == "3" && entranceTestBenefitDto.BenefitKindID != "2")
							{
								ConflictStorage.AddNotImportedDto(entranceTestBenefitDto, ConflictMessages.CompetitiveGroupContainsNotAllowedBenefitType);
							}

                            if ((entranceTestBenefitDto.MinEgeMarks != null) && (entranceTestBenefitDto.MinEgeMarks.Any(x => String.IsNullOrEmpty(x.SubjectID) || x.SubjectID == "0")))
                            {
                                ConflictStorage.AddNotImportedDto(entranceTestBenefitDto, ConflictMessages.DictionaryItemAbsent, "SubjectID");
                            }
						}

				// проверяем общую льготу на целостность
				if (cgDto.CommonBenefit != null)
					foreach (var commonBenefitDto in cgDto.CommonBenefit)
					{
						CheckDictionaryValues(commonBenefitDto.OlympicDiplomTypes, "OlympicDiplomTypeID", commonBenefitDto,
							DbObjectRepository.GetOlympicDiplomType);
						CheckUniqueValues(commonBenefitDto.OlympicDiplomTypes, "OlympicDiplomTypeID", commonBenefitDto);

                        if (!commonBenefitDto.IsForAllOlympics.To(false) && commonBenefitDto.OlympicsLevels != null)
                            CheckDictionaryValues(commonBenefitDto.OlympicsLevels.Select(c => c.OlympicID), "OlympicID", commonBenefitDto, DbObjectRepository.GetOlympicType);
                        if (commonBenefitDto.Olympics != null)
                            CheckDictionaryValues(commonBenefitDto.Olympics, "OlympicID", commonBenefitDto, DbObjectRepository.GetOlympicType);

                        if (commonBenefitDto.IsForAllOlympics.To(false))
                        {
                            /* Если выбран признак ВСЕ олимпиады*/
                            if (commonBenefitDto.Olympics != null && commonBenefitDto.Olympics.Length > 0)
                                ConflictStorage.AddNotImportedDto(commonBenefitDto, ConflictMessages.BenefitContainsOlympicsAndAllOlympics);
                            if (commonBenefitDto.OlympicsLevels != null &&
                                commonBenefitDto.OlympicsLevels.Any(c => !string.IsNullOrEmpty(c.OlympicID)))
                                ConflictStorage.AddNotImportedDto(commonBenefitDto, ConflictMessages.BenefitContainsOlympicsAndAllOlympics);
                        }

                        // не для всех олимпиад и ничего не передали - ошибка
                        if (!commonBenefitDto.IsForAllOlympics.To(false))
                        {
                            if ((commonBenefitDto.Olympics == null || commonBenefitDto.Olympics.Length == 0) &&
                                (commonBenefitDto.OlympicsLevels == null || commonBenefitDto.OlympicsLevels.Length == 0))
                                ConflictStorage.AddNotImportedDto(commonBenefitDto, ConflictMessages.BenefitContainsNoOlympicsAndNoAllOlympics);
                        }

						CheckDictionaryValues(commonBenefitDto.BenefitKindID, "BenefitKindID", commonBenefitDto, DbObjectRepository.GetBenefit);
						//#22119 В КГ общая льгота может быть только "Без ВИ"(id=1), сейчас если указать другую льготу сервис не выдаёт ошибок
						if (commonBenefitDto.BenefitKindID != "1")
						{
							ConflictStorage.AddNotImportedDto(commonBenefitDto, ConflictMessages.CompetitiveGroupContainsNotAllowedCommonBenefit);
						}

                        if ((commonBenefitDto.MinEgeMarks != null) && (commonBenefitDto.MinEgeMarks.Any(x => String.IsNullOrEmpty(x.SubjectID) || x.SubjectID == "0")))
                        {
                            ConflictStorage.AddNotImportedDto(commonBenefitDto, ConflictMessages.DictionaryItemAbsent, "SubjectID");
                        }
					}
			}
		}

		/// <summary>
		/// Проверка справочных значений в заявлениях
		/// </summary>
		public void CheckDictionaries(ApplicationDto[] applicationDtos)
		{
			foreach (var applicationDto in applicationDtos)
			{
				foreach (var appCommonBenefitDto in applicationDto.GetCommonBenefits())
				{
					CheckDictionaryValues(appCommonBenefitDto.BenefitKindID, "BenefitKindID", appCommonBenefitDto,
						DbObjectRepository.GetBenefit);
					if (appCommonBenefitDto.DocumentReason != null)
						CheckDictionaryValuesInDocument(appCommonBenefitDto.DocumentReason.GetDocuments(), appCommonBenefitDto);
					if (!applicationDto.SelectedCompetitiveGroups.Contains(appCommonBenefitDto.CompetitiveGroupID))
					{
						ConflictStorage.AddNotImportedDto(applicationDto, ConflictMessages.CommonBenefitContainsInvalidCompetitiveGroup);
						ConflictStorage.AddNotImportedDto(appCommonBenefitDto, ConflictMessages.CommonBenefitContainsInvalidCompetitiveGroup);
					}
				}

				EntranceTestAppItemDto[] entranceTestAppItemDtos = applicationDto.EntranceTestResults;
				if (entranceTestAppItemDtos != null)
					foreach (var entranceTestAppItemDto in entranceTestAppItemDtos)
					{
						if (entranceTestAppItemDto.ResultDocument != null)
						{
							CheckDictionaryValuesInDocument(entranceTestAppItemDto.ResultDocument.GetDocuments(), entranceTestAppItemDto);
							if (entranceTestAppItemDto.ResultDocument.InstitutionDocument != null)
                                CheckDictionaryValues(entranceTestAppItemDto.ResultDocument.InstitutionDocument.DocumentTypeID,
										"InstitutionDocumentID", entranceTestAppItemDto, DbObjectRepository.GetInstitutionDocument);

						    if (entranceTestAppItemDto.IsBroken)
						    {
    					        ConflictStorage.AddNotImportedDto(applicationDto, 
                                    ConflictMessages.EntranceTestResultsError, string.Join(",", entranceTestAppItemDto.ErrorMessages.ToArray())
                                );
						    }
						}
					}

				CheckGender(applicationDto.Entrant.GenderID, applicationDto);

				ApplicationDocumentsDto documentsDto = applicationDto.ApplicationDocuments;
				if (documentsDto != null && documentsDto.IdentityDocument != null)
				{
					CheckDictionaryValuesForAppDocuments(applicationDto, documentsDto.IdentityDocument.NationalityTypeID, "NationalityTypeID",
						documentsDto.IdentityDocument, DbObjectRepository.GetNationality);
					CheckDictionaryValuesForAppDocuments(applicationDto, applicationDto.ApplicationDocuments.IdentityDocument.IdentityDocumentTypeID, "IdentityDocumentTypeID",
						documentsDto.IdentityDocument, DbObjectRepository.GetIdentityDocumentType);

					// проверяем на гражданство 
					if (DbObjectRepository.IsRussianDoc(documentsDto.IdentityDocument.IdentityDocumentTypeID.To(0))
						&& documentsDto.IdentityDocument.NationalityTypeID.To(0) != 1)
					{
						ConflictStorage.AddNotImportedDto(documentsDto.IdentityDocument, ConflictMessages.IdentityDocumentContainsInvalidNationality);
						ConflictStorage.AddNotImportedDto(applicationDto, ConflictMessages.IdentityDocumentContainsInvalidNationality);
					}

					// и на серию
					if (String.IsNullOrEmpty(documentsDto.IdentityDocument.DocumentSeries) && IdentityDocumentViewModel.IsSeriesRequired(documentsDto.IdentityDocument.IdentityDocumentTypeID.To(0)))
					{
						ConflictStorage.AddNotImportedDto(documentsDto.IdentityDocument, ConflictMessages.IdentityDocumentRequireSeries);
						ConflictStorage.AddNotImportedDto(applicationDto, ConflictMessages.IdentityDocumentRequireSeries);
					}
				}
				
				//если есть документы, проверяем их параметры (вначале собираем в список)
				if (documentsDto != null)
				{
					var documentsToCheck = new List<ApplicationDocumentDto>();
                    if (documentsDto.MilitaryCardDocument != null) documentsToCheck.Add(documentsDto.MilitaryCardDocument);
                    if (documentsDto.StudentDocument != null) documentsToCheck.Add(documentsDto.StudentDocument);
					if (documentsDto.CustomDocuments != null) documentsToCheck.AddRange(documentsDto.CustomDocuments);

                    if (documentsDto.EduDocuments != null)
					{
						foreach (var additionalEduDocument in documentsDto.EduDocuments)
						{
							documentsToCheck.Add(additionalEduDocument.AcademicDiplomaDocument);
							documentsToCheck.Add(additionalEduDocument.BasicDiplomaDocument);
							documentsToCheck.Add(additionalEduDocument.HighEduDiplomaDocument);
							documentsToCheck.Add(additionalEduDocument.IncomplHighEduDiplomaDocument);
							documentsToCheck.Add(additionalEduDocument.MiddleEduDiplomaDocument);
							documentsToCheck.Add(additionalEduDocument.SchoolCertificateDocument);
							documentsToCheck.Add(additionalEduDocument.SchoolCertificateBasicDocument);
                            documentsToCheck.Add(additionalEduDocument.EduCustomDocument);
						}
					}

					CheckDictionaryValuesInDocument(documentsToCheck, applicationDto);

#warning arzyanin added - если есть ошибочные документы - не грузим заявление
                    if (documentsToCheck.Any(c => c != null && c.IsBroken))
                        ConflictStorage.AddNotImportedDto(applicationDto, ConflictMessages.DocumentsError);
				}
			}
		}

		private void CheckDictionaryValuesInDocument(IEnumerable<ApplicationDocumentDto> documentsDto, BaseDto parentDto)
		{
			if (documentsDto == null) return;
			foreach (var docDto in documentsDto)
				CheckDictionaryValuesInDocument(docDto, parentDto);
		}

		private void CheckDictionaryValuesInDocument(ApplicationDocumentDto documentDto, BaseDto parentDto)
		{
			//без разбора - есть параметр, проверяем на справочную корректность
			if (documentDto == null) return;
			if (!String.IsNullOrEmpty(documentDto.OlympicID))
				CheckDictionaryValuesForAppDocuments(parentDto, documentDto.OlympicID, "OlympicID", documentDto, DbObjectRepository.GetOlympicType);
			if (!String.IsNullOrEmpty(documentDto.DiplomaTypeID))
				CheckDictionaryValuesForAppDocuments(parentDto, documentDto.DiplomaTypeID, "DiplomaTypeID", documentDto, DbObjectRepository.GetOlympicDiplomType);
			if (!String.IsNullOrEmpty(documentDto.SubjectID))
				CheckDictionaryValuesForAppDocuments(parentDto, documentDto.SubjectID, "SubjectID", documentDto, DbObjectRepository.GetSubject);
			if (!String.IsNullOrEmpty(documentDto.DisabilityTypeID))
				CheckDictionaryValuesForAppDocuments(parentDto, documentDto.DisabilityTypeID, "DisabilityTypeID", documentDto, DbObjectRepository.GetDisabilityType);
		}

		private void CheckDocumentTypeCorrectness(IEnumerable<BaseDocumentDto> documentsDto, ApplicationDto parentDto, EntrantDocumentType docType, List<Tuple<BaseDocumentDto, EntrantDocumentType, string>> parsedDocs)
		{
			if (documentsDto == null) return;

#warning arzyanin added ==>
            /* Ругаемся на дубликаты UID в коллекциях документов */
            var dupUIDs = documentsDto.GroupBy(c => c.UID).Where(c => c.Count() > 1).Select(c => c.Key).ToList();
            dupUIDs.ForEach(uid =>
                documentsDto.Where(c => c.UID == uid).ToList().ForEach(c => SetNonUniqueUIDMessage(c, parentDto)));
// <==

            foreach (var docDto in documentsDto)
				CheckDocumentTypeCorrectness(docDto, parentDto, docType, parsedDocs);
		}

		/// <summary>
		/// Проверка на корректность типа документа по UID
		/// </summary>
		private void CheckDocumentTypeCorrectness(BaseDocumentDto documentDto, ApplicationDto parentDto, EntrantDocumentType docType, List<Tuple<BaseDocumentDto, EntrantDocumentType, string>> parsedDocs)
		{
			// в базе есть документ с тем же уидом но другим типом - явная ошибка
			// в остальных случаях, считаем что один и тот же документ, т.к. не проверяем содержимое
			if (documentDto == null) return;
			//у документов могут быть отсутствующие UID'ы
			if (documentDto.UID == null) return;
			string entrantUID = parentDto.Entrant.UID;
			var existingDoc = DbObjectRepository.EntrantDocuments.Where(x => x.UID == documentDto.UID && x.Entrant.UID == entrantUID).FirstOrDefault();
			var existingDocOtherEntrant = DbObjectRepository.EntrantDocuments.Where(x => x.UID == documentDto.UID && x.Entrant.UID != entrantUID).FirstOrDefault();
			if (existingDoc != null && existingDoc.DocumentTypeID != (int)docType)
			{
				ConflictStorage.AddNotImportedDto(documentDto, ConflictMessages.InvalidDocumentTypeRelatedToExisting, documentDto.UID);
				ConflictStorage.AddNotImportedDto(parentDto, ConflictMessages.InvalidDocumentTypeRelatedToExisting, documentDto.UID);
			}
			// документ принадлежит другому энтранту
			if (existingDocOtherEntrant != null)
			{
				SetNonUniqueUIDMessage(documentDto, parentDto);
			}

			var existingParsed = parsedDocs.Where(x => x.Item1.UID == documentDto.UID).FirstOrDefault();

			if (existingParsed != null && existingParsed.Item2 != docType)
			{
				ConflictStorage.AddNotImportedDto(documentDto, ConflictMessages.InvalidDocumentTypeRelatedToExisting, documentDto.UID);
				ConflictStorage.AddNotImportedDto(parentDto, ConflictMessages.InvalidDocumentTypeRelatedToExisting, documentDto.UID);
			}
			//у одного энтранта могут повторяться документы, это не страшно
			if (existingParsed != null && existingParsed.Item3 != parentDto.Entrant.UID)
			{
				SetNonUniqueUIDMessage(documentDto, parentDto);
			}

			if (existingParsed == null)
			{
				parsedDocs.Add(new Tuple<BaseDocumentDto, EntrantDocumentType, string>(documentDto, docType, parentDto.Entrant.UID));
			}
		}

		private void CheckDictionaryValuesForAppDocuments<T>(BaseDto appDto, string value, string propertyName, BaseDto dtoObject,
			Func<int, T> dictGetter) where T : class
		{
			if (String.IsNullOrEmpty(value)) return;
			if (dictGetter(value.To(0)) == null)
			{
				ConflictStorage.AddNotImportedDto(dtoObject, ConflictMessages.DictionaryItemAbsent, propertyName);
				ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.DictionaryItemAbsentForApp, propertyName, dtoObject.UID);
			}
		}

		public void CheckDictionaryValues<T>(string value, string propertyName, BaseDto dtoObject,
			Func<int, T> dictGetter) where T : class
		{
			if (String.IsNullOrEmpty(value)) return;
			if (dictGetter(value.To(0)) == null)
				ConflictStorage.AddNotImportedDto(dtoObject, ConflictMessages.DictionaryItemAbsent, propertyName);
		}

        public void CheckGender(string value, BaseDto dtoObject)
        {
            if (string.IsNullOrEmpty(value)) return;
            int gender = value.To(0);

            if (gender != GenderType.Male && gender != GenderType.Female)
                ConflictStorage.AddNotImportedDto(dtoObject, ConflictMessages.DictionaryItemAbsent, "GenderID");
        }

		/// <summary>
		/// Ищёт элемент в словаре, если не находит, возвращает falce
		/// </summary>
		private bool CheckDictionaryValuesForcedExisting<T>(string value, string propertyName, BaseDto dtoObject,
			Func<int, T> dictGetter) where T : class
		{
			if (dictGetter(value.To(0)) == null)
			{
				ConflictStorage.AddNotImportedDto(dtoObject, ConflictMessages.DictionaryItemAbsent, propertyName);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Ищет элементы в словаре. если один из элементов отсутствует - уже ошибка
		/// </summary>
		public void CheckDictionaryValues<T>(IEnumerable<string> values, string propertyName, BaseDto dtoObject,
			Func<int, T> dictGetter) where T : class
		{
			if (values == null) return;
			if (values.Any(x => dictGetter(x.To(0)) == null))
				ConflictStorage.AddNotImportedDto(dtoObject, ConflictMessages.DictionaryItemAbsent, propertyName);
		}

		#endregion

		#region Проверка уникальности идентификаторов для объектов одного типа

		private void CheckUniqueValues(IList<string> values, string propertyName, BaseDto dtoObject)
		{
			if (values == null) return;
			if (values.Distinct().Count() != values.Count())
				ConflictStorage.AddNotImportedDto(dtoObject, ConflictMessages.DictionaryItemNonUnique, propertyName);
		}

		public void CheckUIDUnique(CampaignDto[] campaigns)
		{
			CheckDtoObjectOnUIDUnique(campaigns);
		}


        private void AddNotImportedDistributedAdmissionVolumes(AdmissionInfoDto admissionInfoDto, string badAdmissionVolumeUID, string levelBudget, bool IsPlan)
        {
            admissionInfoDto.DistributedAdmissionVolume.Where(t => t.AdmissionVolumeUID == badAdmissionVolumeUID)
                .ToList()
                .ForEach(x => ConflictStorage.AddNotImportedDto(x, ConflictMessages.AdmissionVolumeUIDAndBudgetLevelMustBeUniqueInCollection, badAdmissionVolumeUID, levelBudget, IsPlan));
        }

		public void CheckUIDUnique(AdmissionInfoDto admissionInfoDto)
		{
			//изначальная заливка
			var affectedCampaigns = GetAffectedCampaigns(admissionInfoDto);

			//заливаем данные по другим кампаниям, по текущим всё равно апдейтим/удаляем, конфликтов не должно быть
			StorageManager.DbObjectRepository.AdmissionVolumes.Where(x => 
                !string.IsNullOrEmpty(x.UID) && !affectedCampaigns.Contains(x.Campaign.UID)).ToList()
				.ForEach(x => CheckDtoObjectOnUIDUnique(new AdmissionVolumeDto { UID = x.UID }, null));
			//заливаем данные по другим кампаниям, по текущим всё равно апдейтим/удаляем, конфликтов не должно быть
			StorageManager.DbObjectRepository.CompetitiveGroups.Where(x => 
                string.IsNullOrEmpty(x.UID) && !affectedCampaigns.Contains(x.Campaign.UID)).ToList()
				.ForEach(x => CheckDtoObjectOnUIDUnique(new CompetitiveGroupDto { UID = x.UID }, null));

			//проверяем на уникальность уиды объёма и КГ
			CheckDtoObjectOnUIDUnique(admissionInfoDto.AdmissionVolume);
			CheckDtoObjectOnUIDUnique(admissionInfoDto.CompetitiveGroups);

            // проверка, что в admissionInfoDto.DistributedAdmissionVolume нет повторов пар {AdmissionVolumeUID, LevelBudget}
            if (admissionInfoDto.DistributedAdmissionVolume != null)
            {
                foreach (var davGroup in admissionInfoDto.DistributedAdmissionVolume.GroupBy(t => t.Key()))
                    if (davGroup.Count() > 1)
                    {
                        foreach (var davItem in davGroup)
                        {
                            ConflictStorage.AddNotImportedDto(davItem, ConflictMessages.AdmissionVolumeUIDAndBudgetLevelMustBeUniqueInCollection, davItem.AdmissionVolumeUID, davItem.LevelBudget, davItem.IsPlan);
                        }
                    }
            }

			//проверка на уникальность имён организаций ЦП
			var targetOrgs = admissionInfoDto.CompetitiveGroups.Where(x => x.TargetOrganizations != null)
				.SelectMany(x => x.TargetOrganizations).Where(x => x != null).ToArray();
			foreach (var grouped in targetOrgs.GroupBy(x => x.UID))
			{
				if (grouped.Count() > 1 && grouped.Select(x => x.TargetOrganizationName).Distinct().Count() > 1)
				{
					foreach (var groupTargetDto in grouped)
					{
						ConflictStorage.AddNotImportedDto(groupTargetDto, ConflictMessages.TargetOrganizationNameIsNotUnique);
					}

					// до кучи запрещаем импорт всех организаций с данными группами
					admissionInfoDto.CompetitiveGroups.Where(x => x.TargetOrganizations != null && x.TargetOrganizations.Any(y => grouped.Any(z => y == z)))
						.ToList()
						.ForEach(x => ConflictStorage.AddNotImportedDto(x, ConflictMessages.TargetOrganizationNameIsNotUnique));
				}
			}

			//проверка на уникальность уидов/имён организаций цп
			foreach (var grouped in targetOrgs.GroupBy(x => x.TargetOrganizationName))
			{
				if (grouped.Count() > 1 && grouped.Select(x => x.UID).Distinct().Count() > 1)
				{
					foreach (var groupTargetDto in grouped)
					{
						ConflictStorage.AddNotImportedDto(groupTargetDto, ConflictMessages.TargetOrganizationSameNameDifferentUID);
					}
					
					// до кучи запрещаем импорт всех организаций с данными группами
					admissionInfoDto.CompetitiveGroups.Where(x => x.TargetOrganizations != null && x.TargetOrganizations.Any(y => grouped.Any(z => y == z)))
						.ToList()
						.ForEach(x => ConflictStorage.AddNotImportedDto(x, ConflictMessages.TargetOrganizationSameNameDifferentUID));
				}
			}

			foreach (var cgDto in admissionInfoDto.CompetitiveGroups)
			{
				//если не совпадают уиды с родительскими, значит дублируются и надо в конфликты
				CheckDtoObjectOnUIDUnique(cgDto.Items, cgDto);

				CheckDtoObjectOnUIDUnique(cgDto.EntranceTestItems, cgDto);
				
				CheckDtoObjectOnUIDUnique(cgDto.CommonBenefit, cgDto);

				//CheckDtoObjectOnUIDOnRequiredParent<CompetitiveGroupTarget>(cgDto.TargetOrganizations, cgDto);
				//CheckDtoObjectOnUIDUnique(cgDto.TargetOrganizations, cgDto);
				if (cgDto.TargetOrganizations != null)
					foreach (var cgTargetDto in cgDto.TargetOrganizations)
					{
						CheckDtoObjectOnUIDUnique(cgTargetDto.Items, cgTargetDto);
					}

				if (cgDto.EntranceTestItems != null)
					foreach (var entranceTestItemDto in cgDto.EntranceTestItems)
					{
						CheckDtoObjectOnUIDUnique(entranceTestItemDto.EntranceTestBenefits, cgDto);
					}
			}
		}

		/// <summary>
		/// Проверка на то, что у дочерних элементов, должны быть правильные родительские UID'ы
		/// Т.е. в базе нет уже родителей с другим уидом, но тем же уидом элемента, иначе не к тому прикреплять придётся
		/// Т.е. в импорте не от того элемента детишек прислали
		/// </summary>
		/// <param name="admissionInfoDto"></param>
		public void CheckParentUIDUnique(AdmissionInfoDto admissionInfoDto)
		{
			foreach (var cgDto in admissionInfoDto.CompetitiveGroups)
			{
			    //если не совпадают уиды с родительскими, значит дублируются и надо в конфликты
				CheckDtoObjectOnUIDOnRequiredParent<CompetitiveGroupItem, CompetitiveGroup>(cgDto.Items, cgDto);
				CheckDtoObjectOnUIDOnRequiredParent<EntranceTestItemC, CompetitiveGroup>(cgDto.EntranceTestItems, cgDto);
				CheckDtoObjectOnUIDOnRequiredParent<BenefitItemC, EntranceTestItemC>(cgDto.CommonBenefit, cgDto);

				if (cgDto.TargetOrganizations != null)
					foreach (var cgTargetDto in cgDto.TargetOrganizations)
						CheckDtoObjectOnUIDOnRequiredParent<CompetitiveGroupTargetItem, CompetitiveGroupTarget>(cgTargetDto.Items, cgTargetDto);

				if (cgDto.EntranceTestItems != null)
					foreach (var entranceTestItemDto in cgDto.EntranceTestItems)
						CheckDtoObjectOnUIDOnRequiredParent<BenefitItemC, EntranceTestItemC>(entranceTestItemDto.EntranceTestBenefits, entranceTestItemDto);
			}
		}

		private Dictionary<Type, HashSet<string>> _objectsUIDByType = new Dictionary<Type, HashSet<string>>();

		/// <summary>
		/// Проверка на уникльность UID'ов заявлений
		/// </summary>
		/// <param name="applicationDtos"></param>
		public void CheckUIDUnique(ApplicationDto[] applicationDtos)
        {
#warning Очень сильно тормозит в этом месте!!!
            
            if (applicationDtos == null) return;

    		//смотрим зами заявления
			CheckDtoObjectOnUIDUnique(applicationDtos);

#warning arzyanin added ==>
		    applicationDtos.CheckIdentityDocumentsDuplicates(ConflictStorage);
// <==

			List<Tuple<BaseDocumentDto, EntrantDocumentType, string>> typeCheckedDocs = new List<Tuple<BaseDocumentDto, EntrantDocumentType, string>>();
			foreach (var applicationDto in applicationDtos)
			{
				if (String.IsNullOrEmpty(applicationDto.Entrant.UID))
				{
					ConflictStorage.AddNotImportedDto(applicationDto.Entrant, ConflictMessages.EntrantUIDIsMissing);
					ConflictStorage.AddNotImportedDto(applicationDto, ConflictMessages.EntrantUIDIsMissing);
				}
				//если не совпадают уиды с родительскими, значит дублируются и надо в конфликты
				CheckDtoObjectOnUIDOnRequiredParent<ApplicationEntranceTestResult, Application>(applicationDto.EntranceTestResults, applicationDto);
				CheckDtoObjectOnUIDUnique(applicationDto.EntranceTestResults, applicationDto);
				CheckDtoObjectOnUIDOnRequiredParent<ApplicationEntranceTestBenefit, Application>(applicationDto.GetCommonBenefits(), applicationDto);
				CheckDtoObjectOnUIDUnique(applicationDto.GetCommonBenefits(), applicationDto);

				ApplicationDocumentsDto documentsDto = applicationDto.ApplicationDocuments;
				if (documentsDto != null)
				{
					CheckDocumentTypeCorrectness(documentsDto.EgeDocuments, applicationDto, EntrantDocumentType.EgeDocument, typeCheckedDocs);
					CheckDocumentTypeCorrectness(documentsDto.IdentityDocument, applicationDto, EntrantDocumentType.IdentityDocument, typeCheckedDocs);
                    CheckDocumentTypeCorrectness(documentsDto.MilitaryCardDocument, applicationDto, EntrantDocumentType.MilitaryCardDocument, typeCheckedDocs);
					CheckDocumentTypeCorrectness(documentsDto.CustomDocuments, applicationDto, EntrantDocumentType.CustomDocument, typeCheckedDocs);
					if (documentsDto.EduDocuments != null)
					{
						foreach (var additionalEduDocument in documentsDto.EduDocuments)
						{
							CheckDocumentTypeCorrectness(additionalEduDocument.AcademicDiplomaDocument, applicationDto, EntrantDocumentType.AcademicDiplomaDocument, typeCheckedDocs);
							CheckDocumentTypeCorrectness(additionalEduDocument.BasicDiplomaDocument, applicationDto, EntrantDocumentType.BasicDiplomaDocument, typeCheckedDocs);
							CheckDocumentTypeCorrectness(additionalEduDocument.HighEduDiplomaDocument, applicationDto, EntrantDocumentType.HighEduDiplomaDocument, typeCheckedDocs);
							CheckDocumentTypeCorrectness(additionalEduDocument.IncomplHighEduDiplomaDocument, applicationDto, EntrantDocumentType.IncomplHighEduDiplomaDocument, typeCheckedDocs);
							CheckDocumentTypeCorrectness(additionalEduDocument.MiddleEduDiplomaDocument, applicationDto, EntrantDocumentType.MiddleEduDiplomaDocument, typeCheckedDocs);
							CheckDocumentTypeCorrectness(additionalEduDocument.SchoolCertificateDocument, applicationDto, EntrantDocumentType.SchoolCertificateDocument, typeCheckedDocs);
							CheckDocumentTypeCorrectness(additionalEduDocument.SchoolCertificateBasicDocument, applicationDto, EntrantDocumentType.SchoolCertificateBasicDocument, typeCheckedDocs);
                            CheckDocumentTypeCorrectness(additionalEduDocument.EduCustomDocument, applicationDto, EntrantDocumentType.EduCustomDocument, typeCheckedDocs);
						}
					}
				}
			}
		}

		private void CheckDtoObjectOnUIDUnique(IEnumerable<BaseDto> dtoObjects, BaseDto parentObject = null)
		{
			if (dtoObjects == null) return;
			foreach (var dtoObject in dtoObjects)
				CheckDtoObjectOnUIDUnique(dtoObject, parentObject);
		}

		/// <summary>
		/// Проверка объекта на уникальность уида (все уиды одного типа должны быть уникальными)
		/// </summary>
		private void CheckDtoObjectOnUIDUnique(BaseDto dtoObject, BaseDto parentObject = null)
		{
			if (dtoObject == null) return;

			ICollection<string> uidCollection;
			Type type = dtoObject.GetType();
			// all documents should be unique by UID
			if (dtoObject is BaseDocumentDto) type = typeof(BaseDocumentDto);

			if (!_objectsUIDByType.ContainsKey(type))
				uidCollection = _objectsUIDByType[type] = new HashSet<string>();
			else
				uidCollection = _objectsUIDByType[type];

			//если уж пропустили такой UID, то его отсутствие валидно по схеме, и нечего проверять
			if (dtoObject.UID == null) return;

			if (uidCollection.Where(x => x == dtoObject.UID).Any())
			{
				SetNonUniqueUIDMessage(dtoObject, parentObject);
			}
			else
				uidCollection.Add(dtoObject.UID);
		}

		/// <summary>
		/// Формирование текстового сообщения про неуникальность объекта
		/// </summary>
		private void SetNonUniqueUIDMessage(BaseDto dtoObject, BaseDto parentObject)
        {
#warning arzyanin added ==>
            string dtoDisplayType = dtoObject.GetDescription();
// <==

            if (string.IsNullOrEmpty(dtoDisplayType))
            {
                if (dtoObject is ApplicationDto) dtoDisplayType = "Заявление";
                else if (dtoObject is AdmissionVolumeDto) dtoDisplayType = "Объем приема";
                else if (dtoObject is ApplicationCommonBenefitDto) dtoDisplayType = "Общая льгота заявления";
                else if (dtoObject is ApplicationDocumentDto) dtoDisplayType = "Документ";
                else if (dtoObject is BaseDocumentDto) dtoDisplayType = "Документ";
                else if (dtoObject is BenefitItemDto) dtoDisplayType = "Льгота";
                else if (dtoObject is CampaignDateDto) dtoDisplayType = "Дата приемной кампании";
                else if (dtoObject is CampaignDto) dtoDisplayType = "Приемная кампания";
                else if (dtoObject is CompetitiveGroupDto) dtoDisplayType = "Конкурс";
                else if (dtoObject is CompetitiveGroupItemDto) dtoDisplayType = "Направление конкурса";
                else if (dtoObject is CompetitiveGroupTargetDto) dtoDisplayType = "Организация целевого приема";
                else if (dtoObject is CompetitiveGroupTargetItemDto) dtoDisplayType = "Места для целевого приема";
                else if (dtoObject is EntranceTestAppItemDto) dtoDisplayType = "Результаты вступительных испытаний";
                else if (dtoObject is EntranceTestItemDto) dtoDisplayType = "Вступительные испытания конкурса";
                else if (dtoObject is EntrantDto) dtoDisplayType = "Абитуриент";
                else if (dtoObject is OrderOfAdmissionItemDto) dtoDisplayType = "Приказ";
            }

		    ConflictStorage.AddNotImportedDto(dtoObject, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, dtoDisplayType, dtoObject.UID);
			if (parentObject != null)
				ConflictStorage.AddNotImportedDto(parentObject, ConflictMessages.UIDMustBeUniqueForChildrenObjects, dtoDisplayType, dtoObject.UID);
		}

		private void CheckDtoObjectOnUIDOnRequiredParent<TObject, TParent>(IEnumerable<BaseDto> dtoObjects, BaseDto parentObject)
            where TObject : class,IObjectWithUID
            where TParent : class,IObjectWithUID
		{
			if (dtoObjects == null) return;
			foreach (var dtoObject in dtoObjects)
				CheckDtoObjectOnUIDOnRequiredParent<TObject, TParent>(dtoObject, parentObject);
		}

		private void CheckDtoObjectOnUIDOnRequiredParent<TObject, TParent>(BaseDto dtoObject, BaseDto parentObject) 
			where TObject : class, IObjectWithUID
            where TParent : class,IObjectWithUID
		{
			if (dtoObject == null) return;
            string requiredParentUID = StorageManager.DbObjectRepository.GetParentUID<TObject>(dtoObject.UID);
            if (requiredParentUID == null) return;//Юсупов: Не уверен
			if (parentObject.UID != requiredParentUID)
			{
				if (DeleteStorage.ContainsObject<TParent>(requiredParentUID)) //уже помечен на удаление
					return;

				SetNonUniqueUIDMessage(dtoObject, parentObject);
			}
		}

		#endregion

		/// <summary>
		/// Имя конкурсной группы должно быть уникальной в контексте кампании
		/// </summary>
		/// <param name="admissionInfoDto"></param>
		private void CompetitiveGroupNameMustBeUnique(AdmissionInfoDto admissionInfoDto)
		{
            var cgNames = new HashSet<Tuple<string, string>>();
            var affectedCampaigns = GetAffectedCampaigns(admissionInfoDto);
			StorageManager.DbObjectRepository.CompetitiveGroups.Where(x => !affectedCampaigns.Contains(x.Campaign.UID))
				.ToList().ForEach(x => cgNames.Add(new Tuple<string, string>(x.Name, x.Campaign.UID)));

            foreach (var cgDto in admissionInfoDto.CompetitiveGroups)
			{
                if (cgNames.Any(x => cgDto.Name.Equals(x.Item1, StringComparison.InvariantCultureIgnoreCase) 
                    && cgDto.CampaignUID.Equals(x.Item2, StringComparison.InvariantCultureIgnoreCase)))
					ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.CompetitiveGroupNameMustBeUnique, cgDto.Name);
				else
					cgNames.Add(new Tuple<string, string>(cgDto.Name, cgDto.CampaignUID));
			}
		}

		/// <summary>
		/// Получение кампаний, которые затрагивает импорт объёма и КГ (в других ничего не трогаем и не проверям)
		/// </summary>
		private static string[] GetAffectedCampaigns(AdmissionInfoDto admissionInfoDto)
		{
			return admissionInfoDto
				.AdmissionVolume
				.Where(x => !String.IsNullOrEmpty(x.CampaignUID)).Select(x => x.CampaignUID)
				.Distinct()
				.Union(admissionInfoDto.CompetitiveGroups
				       	.Where(x => !String.IsNullOrEmpty(x.CampaignUID)).Select(x => x.CampaignUID)
				       	.Distinct()).ToArray();
		}

		/// <summary>
		/// Проверка целостности данных в КГ
		/// </summary>
		private void CheckIntegrityInCompetitiveGroups(AdmissionInfoDto admissionInfoDto)
		{
			foreach (var cgDto in admissionInfoDto.CompetitiveGroups)
			{
				if (
					// проверка на дублирование DirectionID в перечне направлений
					cgDto.Items.Select(x => new { x.DirectionID, x.EducationLevelID }).Distinct().Count() != cgDto.Items.Count() ||
					(cgDto.TargetOrganizations != null &&
					// проверка на дублирование DirectionID в перечне целевого приема
					cgDto.TargetOrganizations.Any(
						x => x.Items.Select(y => new { y.DirectionID, y.EducationLevelID }).Distinct().Count() != x.Items.Count())))
				{
					ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.DirectionIDDuplicatedInCompetitiveGroup);
					continue;
				}

				var allowedDirectionIDs = cgDto.Items.Select(x => new { x.DirectionID, x.EducationLevelID }).ToArray();
				// если указаны места только для целевого приема, а в разрезе финансирования и формы обучения - нет, то ошибка.
				if (cgDto.TargetOrganizations != null)
				{
					foreach (var cgTargetDto in cgDto.TargetOrganizations)
					{
						foreach (var cgTargetItemDto in cgTargetDto.Items)
						{
							if (!allowedDirectionIDs.Contains(new { cgTargetItemDto.DirectionID, cgTargetItemDto.EducationLevelID }))
							{
								var dir = DbObjectRepository.GetDirection(cgTargetItemDto.DirectionID.To(0));
								ConflictStorage.AddConflictWithCustomMessage(cgDto,
									new ConflictStorage.ConflictMessage
										{
											Code = ConflictMessages.CompetitiveGroupHasDirectionOnlyWithTargetPlaces,
											Message = String.Format("В конкурсе для направления {0} указаны формы обучения и источники финансирования, отсутствующие в объеме приема.",
											 dir == null ? cgTargetItemDto.DirectionID : dir.Name)
											//Message = String.Format(
											//"В целевом приеме конкурсной группы {0} указано направление, отсутствующее в перечне направлений конкурсной группы",
											//	cgDto.UID)
										});
							}
						}
					}
				}

				// проверка на обязательность ВИ: русский язык
				if (cgDto.Course == "1" &&
					!cgDto.EntranceTestItems.Where(x => x.EntranceTestSubject.SubjectID != null && 
				                                       x.EntranceTestSubject.SubjectID == InstitutionStructureHelper.GetRussianLanguageSubject(InstitutionID).ToString()).Any())
				{
					cgDto.UseAnyDirectionsFilter = true;
					//ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.CompetitiveGroupMustHaveRussianLanguangeEntranceTest);
				}

                // Проверим на допустимость мест по квотам в конкурсной группе
                bool isAnyError = false;
                int[] allowedQuotaLevels = new int[] { EDLevelConst.Bachelor, EDLevelConst.Speciality };
                foreach (var item in cgDto.Items)
                {
                    if (item.NumberQuotaO.To(0) > 0 || item.NumberQuotaOZ.To(0) > 0 || item.NumberQuotaZ.To(0) > 0)
                        if (!allowedQuotaLevels.Contains(item.EducationLevelID.To(0)))
                            isAnyError = true;
                }

                if (isAnyError)
                    ConflictStorage.AddConflictWithCustomMessage(cgDto, ConflictMessages.QuotaIncorrect);
			}
		}

		/// <summary>
		/// Проверка на достаточность мест в КГ по объёму приёма
		/// </summary>
		private bool CheckPlacesInDirectionBetweenAdmissionVolumeAndCompetitiveGroupItems(AdmissionInfoDto admissionInfoDto, 
			int admissionItemType, int course, string campaignUID)
		{
            FogSoft.Helpers.LogHelper.Log.DebugFormat("Проверка на достаточность мест в КГ по объёму приёма {0} - {1} ({2})", admissionItemType, course, campaignUID);

            bool result = true;
			int budgetO, budgetOZ, budgetZ, paidO, paidOZ, paidZ, targetO, targetOZ, targetZ;
			var cgItemByDirectionsDict = new Dictionary<string, List<CompetitiveGroupItemDto>>();
			var cgTargetItemByDirectionsDict = new Dictionary<string, List<CompetitiveGroupTargetItemDto>>();
			// группируем направления в КГ
			foreach (CompetitiveGroupDto cgDto in admissionInfoDto.CompetitiveGroups.Where(x => x.Course.To(0) == course && x.CampaignUID == campaignUID))
			{
				if (cgDto.Items == null) continue;
				// кг уже в конфликте - не проверяем
				if (ConflictStorage.HasConflictOrNotImported(cgDto)) continue;
				foreach (CompetitiveGroupItemDto cgItemDto in cgDto.Items)
				{
					if (cgItemDto.EducationLevelID.To(0) != admissionItemType) continue;
					cgItemDto.ParentUID = cgDto.UID;

                    List<CompetitiveGroupItemDto> cgItemList;
					if (cgItemByDirectionsDict.ContainsKey(cgItemDto.DirectionID))
						cgItemList = cgItemByDirectionsDict[cgItemDto.DirectionID];
					else
						cgItemList = cgItemByDirectionsDict[cgItemDto.DirectionID] = new List<CompetitiveGroupItemDto>();
					cgItemList.Add(cgItemDto);
				}

				// отдельная проверка для целевого приема
				if (cgDto.TargetOrganizations == null) continue;
				foreach (CompetitiveGroupTargetDto cgTargetDto in cgDto.TargetOrganizations)
				{
                    cgTargetDto.ParentUID = cgDto.UID; // устанавливаем ссылку на родителя для последующей работы с конфликтами
					foreach (CompetitiveGroupTargetItemDto cgTargetItemDto in cgTargetDto.Items)
					{
                        cgTargetItemDto.CompetitiveGroupUID = cgDto.UID;
                        if (cgTargetItemDto.EducationLevelID.To(0) != admissionItemType) continue;
                        //cgTargetItemDto.ParentUID = cgTargetDto.UID;

						List<CompetitiveGroupTargetItemDto> cgTargetItemList;
						if (cgTargetItemByDirectionsDict.ContainsKey(cgTargetItemDto.DirectionID))
							cgTargetItemList = cgTargetItemByDirectionsDict[cgTargetItemDto.DirectionID];
						else
							cgTargetItemList = cgTargetItemByDirectionsDict[cgTargetItemDto.DirectionID] = new List<CompetitiveGroupTargetItemDto>();
						cgTargetItemList.Add(cgTargetItemDto);
					}
				}
			}

			//собрали направления в КГ по Diection, считаем места
			foreach (KeyValuePair<string, List<CompetitiveGroupItemDto>> cgByDirection in cgItemByDirectionsDict)
			{
				var currentDirectionID = cgByDirection.Key;

				// более одного объёма на одно направление - ошибка
				if (admissionInfoDto.AdmissionVolume
					.Where(x => x.DirectionID == currentDirectionID && x.EducationLevelID.To(0) == admissionItemType &&
						x.Course.To(0) == course && x.CampaignUID == campaignUID).Count() > 1)
				{
					foreach (CompetitiveGroupItemDto cgItemDto in cgByDirection.Value)
					{
						ConflictStorage.AddNotImportedDto(cgItemDto, ConflictMessages.AdmissionVolumeNonUniqueDirections);
					}

					continue;
				}

				AdmissionVolumeDto volumeDto = admissionInfoDto.AdmissionVolume
					.SingleOrDefault(x => x.DirectionID == currentDirectionID && x.EducationLevelID.To(0) == admissionItemType
							&& x.Course.To(0) == course && x.CampaignUID == campaignUID);
				budgetO = budgetOZ = budgetZ = paidO = paidOZ = paidZ = targetO = targetOZ = targetZ = 0;
				foreach (CompetitiveGroupItemDto cgItemDto in cgByDirection.Value)
				{
					budgetO += cgItemDto.NumberBudgetO.To(0);
					budgetOZ += cgItemDto.NumberBudgetOZ.To(0);
					budgetZ += cgItemDto.NumberBudgetZ.To(0);
					paidO += cgItemDto.NumberPaidO.To(0);
					paidOZ += cgItemDto.NumberPaidOZ.To(0);
					paidZ += cgItemDto.NumberPaidZ.To(0);
				}

				// если не указан объем приема для направления, то конфликт
				if (volumeDto == null)
				{
					if ((budgetO > 0 || budgetOZ > 0 || budgetZ > 0 ||
					     paidO > 0 || paidOZ > 0 || paidZ > 0))
					{
						result = false;
						foreach (CompetitiveGroupItemDto cgItemDto in cgByDirection.Value)
						{
							ConflictStorage.AddNotImportedDto(cgItemDto, ConflictMessages.AdmissionVolumeIsNotSpecifiedForDirectionInCompetitiveGroupItem);
						}
					}

					continue;
				}

				result = false;
				/*if (volumeDto.NumberBudgetO.To(0) < budgetO)
					ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.PlacesOnDirectionExceeded);
				else if (volumeDto.NumberBudgetOZ.To(0) < budgetOZ)
					ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.PlacesOnDirectionExceeded);
				else if (volumeDto.NumberBudgetZ.To(0) < budgetZ)
					ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.PlacesOnDirectionExceeded);
				else if (volumeDto.NumberBudgetC.To(0) < budgetC)
					ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.PlacesOnDirectionExceeded);
				else if (volumeDto.NumberPaidO.To(0) < paidO)
					ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.PlacesOnDirectionExceeded);
				else if (volumeDto.NumberPaidOZ.To(0) < paidO)
					ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.PlacesOnDirectionExceeded);
				else if (volumeDto.NumberPaidZ.To(0) < paidOZ)
					ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.PlacesOnDirectionExceeded);
				else
					result = true;*/
				//мест меньше чем в объёме - всех в ошибку
				//теперь наоборот
				if (volumeDto.NumberBudgetO.To(0) < budgetO 
					|| volumeDto.NumberBudgetOZ.To(0) < budgetOZ
					|| volumeDto.NumberBudgetZ.To(0) < budgetZ
					|| volumeDto.NumberPaidO.To(0) < paidO
					|| volumeDto.NumberPaidOZ.To(0) < paidOZ
					|| volumeDto.NumberPaidZ.To(0) < paidZ
					/*|| volumeDto.NumberTargetO.To(0) < targetO //не инициализируется заранее, используется ниже
					|| volumeDto.NumberTargetOZ.To(0) < targetOZ
					|| volumeDto.NumberTargetZ.To(0) < targetZ*/)
				{
					foreach (CompetitiveGroupItemDto cgItemDto in cgByDirection.Value)
					{
						ConflictStorage.AddNotImportedDto(cgItemDto, ConflictMessages.CompetitiveGroupPlacesOnDirectionExceeded);
					}
				}
				else
					result = true;
			}

			//теперь проверяем целевой приём
			foreach (KeyValuePair<string, List<CompetitiveGroupTargetItemDto>> cgTargetItemDtoByDirection in cgTargetItemByDirectionsDict)
			{
				var directionID = cgTargetItemDtoByDirection.Key;

				if (admissionInfoDto.AdmissionVolume.Count(x => x.DirectionID == directionID && ParseHelper.To(x.EducationLevelID, 0) == admissionItemType
				                                                && ParseHelper.To(x.Course, 0) == course && x.CampaignUID == campaignUID) > 1)
				{
					foreach (CompetitiveGroupTargetItemDto cgItemDto in cgTargetItemDtoByDirection.Value)
					{
						ConflictStorage.AddNotImportedDto(cgItemDto, ConflictMessages.AdmissionVolumeNonUniqueDirections);
					}

					continue;
				}

				AdmissionVolumeDto volumeDto =
					admissionInfoDto.AdmissionVolume
					.Where(x => x.DirectionID == directionID && x.EducationLevelID.To(0) == admissionItemType
						&& x.Course.To(0) == course && x.CampaignUID == campaignUID)
						.SingleOrDefault();

				targetO = cgTargetItemDtoByDirection.Value.Sum(x => x.NumberTargetO.To(0));
				targetOZ = cgTargetItemDtoByDirection.Value.Sum(x => x.NumberTargetOZ.To(0));
				targetZ = cgTargetItemDtoByDirection.Value.Sum(x => x.NumberTargetZ.To(0));

				// если не указан объем приема для направления, то конфликт
				if (volumeDto == null)
				{
					if (targetO + targetOZ + targetZ > 0)
					{
						result = false;
						cgTargetItemDtoByDirection.Value.ForEach(x => ConflictStorage
						            .AddNotImportedDto(x, ConflictMessages.AdmissionVolumeIsNotSpecifiedForDirectionInCompetitiveGroupTargetItem));

						List<CompetitiveGroupItemDto> competitiveGroupItemDtos = cgItemByDirectionsDict[directionID];
						// если по этому направлению уже добавлены конфликты, то ОК, иначе это ошибка и не указан прием для направления, а целевой прием указан.
						// ошибка целостности, но в конфликты не добавляем, т.к. конфликты по объектам целевого приема не оговаривались
						if (competitiveGroupItemDtos == null)
						{
							LogHelper.Log.ErrorFormat(
								"Институт: {0}. Для направления {1} указан целевой прием, но отсутсвуют данные по бюджетной и платной формам приемов",
								DbObjectRepository.InstitutionId, directionID);
						}
					}

					continue;
				}

				if (volumeDto.NumberTargetO.To(0) < targetO || volumeDto.NumberTargetOZ.To(0) < targetOZ || volumeDto.NumberTargetZ.To(0) < targetZ)
				{
				    result = false;
					//ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.PlacesOnDirectionExceeded);
					//теперь наоборот
					foreach (CompetitiveGroupTargetItemDto cgItemDto in cgTargetItemDtoByDirection.Value)
					{
						ConflictStorage.AddNotImportedDto(cgItemDto, ConflictMessages.AdmissionVolumeIsNotSpecifiedForDirectionInCompetitiveGroupItem);
					}
				}
			}

			return result;
		}

		#region Проверка ограничений по предметам и направлениям в КГ

		public void CheckRestrictionOnEntranceSubjectInDirectionsForCompetitiveGroup(AdmissionInfoDto admissionInfoDto)
		{
			foreach (var cgDto in admissionInfoDto.CompetitiveGroups)
			{
				int[] entranceTestSubjects = cgDto.EntranceTestItems.Where(x => x.EntranceTestTypeID.To(0) == EntranceTestType.MainType)
					.Select(x => x.EntranceTestSubject.SubjectID).Distinct().Select(x => x.To(0))
					.ToArray();

				//проверяем, можно ли предметы без SubjectID
				var edLevels = new[]
				               {
				               	EDLevelConst.Bachelor.To(0).ToString(), 
				               	EDLevelConst.Speciality.To(0).ToString()
				               };

				bool isAllowCustomSubjects = true;

				if (cgDto.Course != "1")
					isAllowCustomSubjects = true;
				else if (!cgDto.Items.Where(x => edLevels.Contains(x.EducationLevelID)).Any())
					isAllowCustomSubjects = true;
				else
				{
					if (cgDto.Items != null && cgDto.Items.Where(x => x.NumberBudgetO.To(0) > 0 || x.NumberBudgetOZ.To(0) > 0 || x.NumberPaidO.To(0) > 0 || x.NumberPaidOZ.To(0) > 0).Any())
						isAllowCustomSubjects = false;
					if (cgDto.TargetOrganizations != null && cgDto.TargetOrganizations
						.Where(x => x != null && x.Items != null)
						.SelectMany(x => x.Items)
						.Where(x => x != null && x.NumberTargetO.To(0) > 0 && x.NumberTargetOZ.To(0) > 0)
						.Any())
						isAllowCustomSubjects = false;
				}

				//#35030 Для очной и очно-заочной формы тоже добавить возможность ввода испытаний с клавиатуры.
				isAllowCustomSubjects = true;

				// в общем, сейчас всегда допустимы кастомные предметы, но логика осталась, на случай возврата назад
				if (isAllowCustomSubjects)
				{
					//выкидываем эти предметы из списка
					entranceTestSubjects = entranceTestSubjects.Where(x => x != 0).ToArray();
				}

				HashSet<int> allowedEntranceTestSubjects = new HashSet<int>();
				//если не разрешены любые предметы, берём все доступные
				if (!isAllowCustomSubjects)
				{
					allowedEntranceTestSubjects = GetAllowedEntranceTestSubject(cgDto) ?? new HashSet<int>();
					if (allowedEntranceTestSubjects.Count == 0)
					{
						cgDto.UseAnyDirectionsFilter = true;
						//проверка отключена, т.к. в интерфейсе в логике можно и так всё делать.
						//ConflictStorage.AddConflictWithCustomMessage(cgDto,
						//	new ConflictStorage.ConflictMessage
						//		{
						//			Code = ConflictMessages.CompetitiveGroupHasNotAllowedEntranceTestSubjects,
						//			Message = String.Format("В конкурсной группе {0} не указаны допустимые предметы в институте.",
						//			cgDto.UID)
						//		}
						//	);
						//continue;
					}
				}

				//предметы с невалидными ID
				var invalidSubjects = entranceTestSubjects.Where(x => DbObjectRepository.GetSubject(x) == null).ToArray();
				if (invalidSubjects.Length > 0)
				{
					ConflictStorage.AddConflictWithCustomMessage(cgDto,
						new ConflictStorage.ConflictMessage
						{
							Code = ConflictMessages.CompetitiveGroupContainsNotAllowedEntranceTestSubjects,
							Message = String.Format(
								"В конкурсе указаны предметы для основных вступительных испытаний, c несуществующими ID: {0}.",
								String.Join(",", invalidSubjects))
						});
				}

				if (allowedEntranceTestSubjects.Count == 0) continue;

				//при флаге любые ВИ, можно использовать любые предметы
				//проверяем доступные и то что передали
				int[] notAllowedSubjects = entranceTestSubjects
					.Where(entranceTestSubject => !allowedEntranceTestSubjects.Contains(entranceTestSubject)).ToArray();
				if (notAllowedSubjects.Length > 0)
					cgDto.UseAnyDirectionsFilter = true; //вместо ошибки, ставим что в КГ можно любые предметы использовать
				//	ConflictStorage.AddConflictWithCustomMessage(cgDto,
				//		new ConflictStorage.ConflictMessage
				//			{
				//				Code = ConflictMessages.CompetitiveGroupContainsNotAllowedEntranceTestSubjects,
				//				Message = String.Format(
				//					"В Конкурсной группе указаны предметы c ID: {0} для основных вступительных испытаний, которые не допустимы для направлений.",
				//					String.Join(",", notAllowedSubjects))
				//			});
			}
		}

		/// <summary>
		/// Получаем разрешённые предметы для КГ
		/// </summary>
		/// <param name="cgDto"></param>
		/// <returns></returns>
		private HashSet<int> GetAllowedEntranceTestSubject(CompetitiveGroupDto cgDto)
		{
			int[] directionIDs = cgDto.Items.Select(x => x.DirectionID.To(0)).ToArray();
			var link = DbObjectRepository.DirectionSubjectLinkDirections
					.Where(x => directionIDs.Contains(x.DirectionID))
					.Select(x => new { x.LinkID, x.DirectionID }).ToArray();
			//если есть хотя бы одно отсутствующее — ставим любой фильтр
			if (link.Select(x => x.DirectionID).Except(directionIDs).Any())
				cgDto.UseAnyDirectionsFilter = true;
			if (link.Length == 0)
			{
				cgDto.UseAnyDirectionsFilter = true;
				//проверка отключена. 
				//ConflictStorage.AddConflictWithCustomMessage(cgDto,
				//	new ConflictStorage.ConflictMessage
				//		{
				//			Code = ConflictMessages.CompetitiveGroupContainsDirectionNotDefinedInAllowed,
				//			Message = String.Format(
				//			"В конкурсной группе ни одно из направлений не является допустимым.")
				//		});
				return null;
			}

			var linkIDs = link.Select(x => x.LinkID).ToArray();
			int[] allowedSubjects = DbObjectRepository.DirectionSubjectLinkSubjects
				.Where(x => linkIDs.Contains(x.LinkID)).Select(x => x.SubjectID).ToArray();
			if (allowedSubjects.Length == 0)
			{
				Log.DebugFormat("В КГ {0} не указаны допустимые предметы", cgDto.UID);
				return new HashSet<int>();
			}

			return new HashSet<int>(allowedSubjects);
		}

		/// <summary>
		/// Проверяем допустимый объём приёма по датам ПК
		/// </summary>
		/// <param name="dto"></param>
		private void CheckAllowedPlacesForAdmissionVolumeByCampaignDate(AdmissionVolumeDto dto)
		{
			var dbCampaign =
				DbObjectRepository.Campaigns.Single(x => x.InstitutionID == InstitutionID && x.UID == dto.CampaignUID);
			var intCourse = dto.Course.To(0);
			var intEdLevelID = dto.EducationLevelID.To(0);
			var allowedDates = dbCampaign.CampaignDate
				.Where(x => x.IsActive && x.Course == intCourse && x.EducationLevelID == intEdLevelID).Select(x => new { x.EducationFormID, x.EducationSourceID })
				.Distinct().ToArray();
			Func<int, int, bool> isFormAvail = (formID, sourceID) => allowedDates
				.Any(x => x.EducationFormID == formID && x.EducationSourceID == sourceID);
			bool isFail = false;
			Action<string, int, int> checkIsAvail = (number, formID, sourceID) =>
			                                        	{
			                                        		if (number.To(0) > 0 && !isFormAvail(formID, sourceID)) isFail = true;
			                                        	};
			//если есть нужная дата - хорошо. Нет, ошибка.
			checkIsAvail(dto.NumberBudgetO, EDFormsConst.O, EDSourceConst.Budget);
			checkIsAvail(dto.NumberBudgetOZ, EDFormsConst.OZ, EDSourceConst.Budget);
			checkIsAvail(dto.NumberBudgetZ, EDFormsConst.Z, EDSourceConst.Budget);
            checkIsAvail(dto.NumberQuotaO, EDFormsConst.O, EDSourceConst.Budget);
            checkIsAvail(dto.NumberQuotaOZ, EDFormsConst.OZ, EDSourceConst.Budget);
            checkIsAvail(dto.NumberQuotaZ, EDFormsConst.Z, EDSourceConst.Budget);
            checkIsAvail(dto.NumberPaidO, EDFormsConst.O, EDSourceConst.Paid);
			checkIsAvail(dto.NumberPaidOZ, EDFormsConst.OZ, EDSourceConst.Paid);
			checkIsAvail(dto.NumberPaidZ, EDFormsConst.Z, EDSourceConst.Paid);
			checkIsAvail(dto.NumberTargetO, EDFormsConst.O, EDSourceConst.Target);
			checkIsAvail(dto.NumberTargetOZ, EDFormsConst.OZ, EDSourceConst.Target);
			checkIsAvail(dto.NumberTargetZ, EDFormsConst.Z, EDSourceConst.Target);
			//есть хоть одна ошибка - пишем про это
			if (isFail)
			{
				ConflictStorage.AddConflictWithCustomMessage(dto,
						ConflictMessages.AdmissionVolumeContainsNotAllowedNumbers);
			}

            if ((dto.NumberQuotaO.To(0) > 0 || dto.NumberQuotaOZ.To(0) > 0 || dto.NumberQuotaZ.To(0) > 0))
            {
                int[] allowedQuotaLevels = new int[] { EDLevelConst.Bachelor, EDLevelConst.Speciality };

                if (!allowedQuotaLevels.Contains(dto.EducationLevelID.To(0)))
                {
                    ConflictStorage.AddConflictWithCustomMessage(dto, ConflictMessages.QuotaIncorrect);
                }
            }
		}

		/// <summary>
		/// Проверка ограничений по направлениям в КГ.
		/// </summary>
		public void CheckRestrictionOnDirectionsForAdmissionAndCompetitiveGroup(AdmissionInfoDto admissionInfoDto)
		{
			List<string> notAvailableDirectionIDs = new List<string>();
			List<string> notAllowedDirectionIDs = new List<string>();

			var availableDirectionsForInstitute = DbObjectRepository
				.AllowedDirections
				.Where(x => x.InstitutionID == InstitutionID)
				.Select(x => new { EducationLevelID = x.AdmissionItemTypeID, x.DirectionID }).Distinct().ToList();
			foreach (var admissionVolume in admissionInfoDto.AdmissionVolume)
			{
				bool isFail = false;
				//не разрешены для института направления
				if (!availableDirectionsForInstitute.Any(x => x.DirectionID == admissionVolume.DirectionID.To(0) && x.EducationLevelID == admissionVolume.EducationLevelID.To(0)))
				{
					ConflictStorage.AddConflictWithCustomMessage(admissionVolume, ConflictMessages.AdmissionVolumeContainsNotAllowedDirections);
					isFail = true;
				}

				//не та кампания
				if (DbObjectRepository.GetObject<Campaign>(admissionVolume.CampaignUID) == null)
				{
					ConflictStorage.AddConflictWithCustomMessage(admissionVolume, ConflictMessages.AdmissionVolumeContainsNotAllowedCampaign);
					isFail = true;
				}

				//не тот уровень образования/курс
				//if (!DbObjectRepository.Campaigns.Where(x => x.InstitutionID == InstitutionID && x.UID == admissionVolume.CampaignUID)
				//	.SelectMany(x => x.CampaignEducationLevel).Where(x => x.Course == admissionVolume.Course.To(0)).Any())
				//{
				//	ConflictStorage.AddConflictWithCustomMessage(admissionVolume, ConflictMessages.AdmissionVolumeContainsNotAllowedCampaignCourse);
				//	isFail = true;
				//}

				//если нет ошибки, смотрим места, иначе нечего смотреть
				if (!isFail)
					CheckAllowedPlacesForAdmissionVolumeByCampaignDate(admissionVolume);
			}

			// проверяем на повторяющиеся значения в объёме приёма
			var failGroupedVolumes = admissionInfoDto.AdmissionVolume
				.GroupBy(x => new { x.DirectionID, x.CampaignUID, x.Course, x.EducationLevelID })
				.Where(x => x.Count() > 1)
				.ToArray();
			if (failGroupedVolumes.Any())
			{
				foreach (var admissionVolumeDto in failGroupedVolumes)
				{
					foreach (var volumeDto in admissionVolumeDto)
					{
						ConflictStorage.AddNotImportedDto(volumeDto, ConflictMessages.AdmissionVolumeNonUniqueDirections);
					}
				}
			}

			//а теперь смотрим КГ
			foreach (var cgDto in admissionInfoDto.CompetitiveGroups)
			{
				bool isFailed = false;
				foreach (var cgItemDto in cgDto.Items)
				{
					if (!CheckDictionaryValuesForcedExisting(cgItemDto.EducationLevelID, "EducationLevelID", cgItemDto, DbObjectRepository.GetEducationalLevel))
						isFailed = true;
				}

				if (isFailed) continue;

				//нет кампании - ошибка
				if (DbObjectRepository.GetObject<Campaign>(cgDto.CampaignUID) == null)
				{
					ConflictStorage.AddConflictWithCustomMessage(cgDto,
						ConflictMessages.AdmissionVolumeContainsNotAllowedCampaign);
				}

				//не тот курс - ошибка
				//if (!DbObjectRepository.Campaigns.Where(x => x.InstitutionID == InstitutionID && x.UID == cgDto.CampaignUID)
				//	.SelectMany(x => x.CampaignEducationLevel).Where(x => x.Course == cgDto.Course.To(0)).Any())
				//{
				//	ConflictStorage.AddConflictWithCustomMessage(cgDto, ConflictMessages.AdmissionVolumeContainsNotAllowedCampaignCourse);
				//}

				var availableDirections = GetAvailableDirectionsForCompetitiveGroup(cgDto);
				var allowedDirections = GetAllowedDirectionsForCompetitiveGroup(cgDto);
				//если ничего нет, переключаемся в режим "любые направления"
				if (availableDirections.Count == 0)
				{
					cgDto.UseAnyDirectionsFilter = true;
					availableDirections = new List<Tuple<int, int>>(allowedDirections);
				}

				foreach (var cgItemDto in cgDto.Items)
				{
					if (!availableDirections.Any(x => x.Item1 == cgItemDto.EducationLevelID.To(0) && x.Item2 == cgItemDto.DirectionID.To(0)))
						notAvailableDirectionIDs.Add(cgItemDto.UID);
					if (!allowedDirections.Any(x => x.Item1 == cgItemDto.EducationLevelID.To(0) && x.Item2 == cgItemDto.DirectionID.To(0)))
						notAllowedDirectionIDs.Add(cgItemDto.UID);
				}

				//разрешённые по вузу
				if (notAllowedDirectionIDs.Count > 0)
				{
					string conflictMessage = ConflictMessages.GetMessage(ConflictMessages.CompetitiveGroupContainsNotAllowedByInstituteDirections);
					conflictMessage += String.Format(" Недопустимые направления с UID: {0} в КГ {1}",
						String.Join(",", notAllowedDirectionIDs), cgDto.UID);
					Log.DebugFormat(conflictMessage);

					ConflictStorage.AddConflictWithCustomMessage(cgDto, new ConflictStorage.ConflictMessage
					{
						Code = ConflictMessages.CompetitiveGroupContainsNotAllowedByInstituteDirections,
						Message = conflictMessage
					});
				}
				
				//допустимые вообще
				if (notAvailableDirectionIDs.Count > 0)
				{
					cgDto.UseAnyDirectionsFilter = true;
					//string conflictMessage = ConflictMessages.GetMessage(ConflictMessages.CompetitiveGroupContainsNotAvailableDirections);
					//conflictMessage += String.Format(" Недопустимые направления с UID: {0} в КГ {1}",
					//	String.Join(",", notAvailableDirectionIDs), cgDto.UID);
					//Log.DebugFormat(conflictMessage);

					//ConflictStorage.AddConflictWithCustomMessage(cgDto, new ConflictStorage.ConflictMessage
					//{
					//	Code = ConflictMessages.CompetitiveGroupContainsNotAvailableDirections,
					//	Message = conflictMessage
					//});
				}

				notAvailableDirectionIDs.Clear();
                notAllowedDirectionIDs.Clear();
			}			
		}

        
        /// <summary>
        /// Все проверки по DistributedAdmissionVolume
        /// </summary>
        /// <param name="admissionInfoDto"></param>
        public void CheckDistributedAdmissionVolume(AdmissionInfoDto admissionInfoDto)
        {
            // все проверки по DistributedAdmissionVolume!
            if (admissionInfoDto.DistributedAdmissionVolume == null) return;

            foreach (var dav in admissionInfoDto.DistributedAdmissionVolume)
            {
                // 0. Уже сломан?
                if (dav.IsBroken)
                    continue;
                
                // 1. в пакете есть такой AV.UID и AV не сломан
                var av = admissionInfoDto.AdmissionVolume.Where(t => t.UID == dav.AdmissionVolumeUID).FirstOrDefault();
                if (av == null || av.IsBroken)
                {
                    ConflictStorage.AddNotImportedDto(dav, ConflictMessages.AdmissionVolumeIsNotImportedForDistributedAdmissionVolume,  dav.AdmissionVolumeUID, dav.IsPlan.ToString());
                    continue;
                }
                // 2. LevelBudget есть в справочнике
                int intLevelBudget = dav.LevelBudget.To(0);
                using (GVUZ.Model.Entrants.EntrantsEntities _entrantEntities = new EntrantsEntities())
                {
                    if (!_entrantEntities.LevelBudget.Any(t => t.IdLevelBudget == intLevelBudget))
                    {
                        // Если бы помечать сломанным только этот объект
                        // ConflictStorage.AddNotImportedDto(dav, ConflictMessages.DictionaryItemAbsent, "LevelBudget=" + dav.LevelBudget);
                        // Но надо помечать сломанными все объекты c данным AdmissionVolumeUID
                        foreach(var item in admissionInfoDto.DistributedAdmissionVolume.Where(t=> t.AdmissionVolumeUID == dav.AdmissionVolumeUID))
                        {
                            ConflictStorage.AddNotImportedDto(item, ConflictMessages.DictionaryItemAbsent, "LevelBudget=" + dav.LevelBudget);
                        }
                        continue;
                    }
                }

                // 3. Все числа не меньше нуля - не нужно, они и так uint!
                //if (dav.NumberBudgetO.To(0) < 0 || dav.NumberBudgetOZ.To(0) < 0 || dav.NumberBudgetZ.To(0) < 0 ||
                //    dav.NumberQuotaO.To(0) < 0 || dav.NumberQuotaOZ.To(0) < 0 || dav.NumberQuotaZ.To(0) < 0 ||
                //    dav.NumberTargetO.To(0) < 0 || dav.NumberTargetOZ.To(0) < 0 || dav.NumberTargetZ.To(0) < 0)
                //{
                //    ConflictStorage.AddNotImportedDto(dav, ConflictMessages.DistributedAdmissionVolumeInvalidNumbers, dav.AdmissionVolumeUID);
                //}
            }

            // 4. Проверка того, что сумма распределенных К-Ц-П не превышает исходных К-Ц-П.
            foreach (var group in admissionInfoDto.DistributedAdmissionVolume.GroupBy(t=> t.AdmissionVolumeUID))
            {
                var av = admissionInfoDto.AdmissionVolume.Where(t => t.UID == group.Key).FirstOrDefault();
                if (av == null || av.IsBroken)
                    continue;

                if (av.NumberBudgetO.To(0) < group.Sum(t => t.NumberBudgetO.To(0)) 
                    || av.NumberBudgetOZ.To(0) < group.Sum(t => t.NumberBudgetOZ.To(0)) 
                    || av.NumberBudgetZ.To(0) < group.Sum(t => t.NumberBudgetZ.To(0))
                    || av.NumberQuotaO.To(0) < group.Sum(t => t.NumberQuotaO.To(0))
                    || av.NumberQuotaOZ.To(0) < group.Sum(t => t.NumberQuotaOZ.To(0))
                    || av.NumberQuotaZ.To(0) < group.Sum(t => t.NumberQuotaZ.To(0))

                    || av.NumberTargetO.To(0) < group.Sum(t => t.NumberTargetO.To(0))
                    || av.NumberTargetOZ.To(0) < group.Sum(t => t.NumberTargetOZ.To(0))
                    || av.NumberTargetZ.To(0) < group.Sum(t => t.NumberTargetZ.To(0)) 
                   )
                {
                    foreach (var item in group)
                    {
                        ConflictStorage.AddNotImportedDto(item, ConflictMessages.DistributedAdmissionVolumeNumbersSumBiggerAV, av.UID, av.IsPlan.ToString());
                    }
                }
            }
        }

		/// <summary>
		/// Направления, разрешённые по вузу и с теми же ВИ (505-ый приказ) что и в направлениях КГ
		/// </summary>
		/// <param name="cgDto"></param>
		/// <returns></returns>
		private ICollection<Tuple<int, int>> GetAvailableDirectionsForCompetitiveGroup(CompetitiveGroupDto cgDto)
		{
			int[] directionIDs = cgDto.Items.Select(x => x.DirectionID.To(0)).ToArray();
			var allowedDirections = DbObjectRepository.AllowedDirections
				.Where(x => /*x.AdmissionItemTypeID == eduLevelID && */x.InstitutionID == InstitutionID)
				.OrderBy(x => x.Direction.Name)
                .ToArray()
				.Select(x => new Tuple<int, int>(x.AdmissionItemTypeID, x.DirectionID)).Distinct().ToArray();

			int[] availableByET = GetDirectionsWithSameEntranceTests(directionIDs, false);
			return allowedDirections.Where(x => Array.IndexOf(availableByET, x.Item2) >= 0).Select(x => x).ToArray();
		}

		/// <summary>
		/// Направления в принципе разрешённые по вузу
		/// </summary>
		private ICollection<Tuple<int, int>> GetAllowedDirectionsForCompetitiveGroup(CompetitiveGroupDto cgDto)
		{
			var allowedDirections = DbObjectRepository.AllowedDirections
				.Where(x => /*x.AdmissionItemTypeID == eduLevelID &&*/ x.InstitutionID == InstitutionID)
				.OrderBy(x => x.Direction.Name)
                .ToArray()
				.Select(x => new Tuple<int, int>(x.AdmissionItemTypeID, x.DirectionID)).Distinct().ToArray();

			return allowedDirections;
		}

		/// <summary>
		/// Профильный предмет для КГ
		/// </summary>
		private int GetProfileSubject(CompetitiveGroupDto cgDto)
		{
			int directionID = cgDto.Items[0].DirectionID.To(0);
			int linkID = DbObjectRepository.DirectionSubjectLinkDirections
				.Where(x => x.DirectionID == directionID)
				.Select(x => x.LinkID).FirstOrDefault();
			if (linkID == 0) return 0;
			return DbObjectRepository.DirectionSubjectLinks.Where(x => x.ID == linkID).Select(x => x.ProfileSubjectID ?? 0).First();
		}

		/// <summary>
		/// Направления с теми же ВИ
		/// </summary>
		/// <param name="isExact">false - 505 приказ, иначе точное совпадение ищем</param>
		public int[] GetDirectionsWithSameEntranceTests(int[] directionIDs, bool isExact)
		{
		    var links = DbObjectRepository.DirectionSubjectLinkDirections.Where(x => 
                x.DirectionSubjectLink != null && 
                x.DirectionSubjectLink.ProfileSubjectID != null &&
                directionIDs.Contains(x.DirectionID)).Select(x => new { x.LinkID, x.DirectionSubjectLink.ProfileSubjectID }).Distinct().ToArray();
            if (links.Length == 0)
				return new int[0];

			//профильные должны быть у всех одинаковыми
			if (links.Select(x => x.ProfileSubjectID).Distinct().Count() > 1)
				return new int[0];

			var firstLinkID = links[0].LinkID;
			var linkIDs = links.Select(x => x.LinkID).ToArray();

			int? profileSubjectID = DbObjectRepository.DirectionSubjectLinks.Where(x => x.ID == firstLinkID).Select(x => x.ProfileSubjectID).First();
			var subjects = DbObjectRepository.DirectionSubjectLinkSubjects.Where(x => linkIDs.Contains(x.LinkID)).Select(x => new { x.SubjectID, x.LinkID }).ToArray();

			//берём общие для всех испытания
			List<int> intersectedSubjects = subjects.Select(x => x.SubjectID).Distinct().ToList();
			intersectedSubjects = subjects.GroupBy(x => x.LinkID)
				.Aggregate(intersectedSubjects, (current, sub) => current.Intersect(sub.Select(x => x.SubjectID)).ToList());

			//если не точное совпадение то требуем, чтобы у остальных испытаний было совпадание по трём (или меньше)
			int subLength = intersectedSubjects.Count;
			if (!isExact)
				subLength = Math.Min(subLength, 3);

			var possibleLinks = DbObjectRepository.DirectionSubjectLinkSubjects
				.Where(x =>
                    ((x.DirectionSubjectLink != null && x.DirectionSubjectLink.ProfileSubjectID == profileSubjectID) || x.DirectionSubjectLink == null)
                    && intersectedSubjects.Contains(x.SubjectID))
				.Select(x => new { x.LinkID, x.SubjectID }).ToArray().GroupBy(x => x.LinkID);

			List<int> correctLinks = (from possibleLink in possibleLinks
									  where possibleLink.Select(x => x.SubjectID).Intersect(intersectedSubjects).Count() >= subLength
									  select possibleLink.Key).ToList();

			return DbObjectRepository.DirectionSubjectLinkDirections.Where(x => correctLinks.Contains(x.LinkID)).Select(x => x.DirectionID).ToArray();
		}

		/// <summary>
		/// Проверяем на корректность выставления минимальных баллов в КГ
		/// </summary>
		/// <param name="admissionInfoDto"></param>
		private void CheckMinSubjectValueInCompetitiveGroups(AdmissionInfoDto admissionInfoDto)
		{
			if (admissionInfoDto.CompetitiveGroups != null)
			{
				foreach (CompetitiveGroupDto competitiveGroupDto in admissionInfoDto.CompetitiveGroups)
				{
					if (competitiveGroupDto.EntranceTestItems != null)
					{
						foreach (EntranceTestItemDto etDto in competitiveGroupDto.EntranceTestItems)
						{
							if (etDto.EntranceTestTypeID == "1") //обычные ВИ
							{
								int subjectID = etDto.EntranceTestSubject.SubjectID.To(0);
								
								if (subjectID > 0)
								{
									int dbMinValue = DbObjectRepository.GetSubjectEgeMinValue(subjectID);
									decimal minScore = etDto.MinScore.To(0m, CultureInfo.InvariantCulture);
									if (string.IsNullOrEmpty(etDto.MinScore))
										minScore = dbMinValue;

									if (dbMinValue > minScore)
									{
										ConflictStorage.AddNotImportedDto(etDto, ConflictMessages.EntranceTestContainsScoreLowerThanRequired);
									}
								}
							}
						}
					}
				}
			}
		}

		#endregion

		#region Проверка на ограничения по профильным и творческим вступительным испытаниям для направлений в КГ

		/// <summary>
		/// Проверка на допустимость испытаний профильной направленности и творческих
		/// </summary>
		/// <param name="admissionInfoDto"></param>
		public void CheckRestrictionOnProfileAndCreativeDirection(AdmissionInfoDto admissionInfoDto)
		{
			foreach (var cgDto in admissionInfoDto.CompetitiveGroups)
			{
				if (!AllowedCreativeEntranceTests(cgDto))
				{
					// проверить есть ли ВИ для КГ творческой направленности, если есть - ошибка.
					if (cgDto.EntranceTestItems
						.Where(x => x.EntranceTestTypeID.To(0) == EntranceTestType.CreativeType).Any())
						ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.CompetitiveGroupNotAllowedCreativeEntranceTests);
				}

				int profileSubjectID = GetProfileSubject(cgDto);
				//может быть любое
				//int extraProfileSubjectID = GetExtraProfileSubjectForCompetitiveGroup(cgDto);

				//нет ни профильных, ни профильной направленности
				if (cgDto.UseAnyDirectionsFilter)
				{
				    profileSubjectID = 0;
				//    extraProfileSubjectID = 0;
				}

				// проверка на обязательное профильное ВИ в КГ.
				if (!cgDto.UseAnyDirectionsFilter && cgDto.EntranceTestItems
					.Where(x => x.EntranceTestTypeID.To(0) == EntranceTestType.MainType &&
						!String.IsNullOrWhiteSpace(x.EntranceTestSubject.SubjectID) &&
						x.EntranceTestSubject.SubjectID.To(0) == profileSubjectID).Count() != 1)
				{
					//ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.CompetitiveGroupMustHaveOneProfileEntranceTest);
					//continue;
					cgDto.UseAnyDirectionsFilter = true;
				}

				// проверить ВИ, если есть профильный предмет, то он должен быть только один и совпадать с profileSubjectID
				EntranceTestItemDto[] profileEntrTestItem = cgDto.EntranceTestItems
					.Where(x => x.EntranceTestTypeID.To(0) == EntranceTestType.ProfileType).ToArray();
				
				if (profileEntrTestItem.Length > 1)
				{
					//теперь разрешено
					cgDto.UseAnyDirectionsFilter = true;
					//ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.CompetitiveGroupCantHaveMoreThanOneExtraProfileSubject);
					//continue;
				}

				if (!IsProfileSubjectsAllowed(cgDto) && profileEntrTestItem.Length > 0)
				{
					ConflictStorage.AddNotImportedDto(cgDto,
						ConflictMessages.CompetitiveGroupContainsNotAllowedExtraProfileSubjectInEntranceTest);
					continue;
				}

				//теперь не нужна проверка
				//if (extraProfileSubjectID == 0 && profileEntrTestItem.Length == 1)
				//{
				//    ConflictStorage.AddNotImportedDto(cgDto,
				//        ConflictMessages.CompetitiveGroupContainsNotAllowedExtraProfileSubjectInEntranceTest);
				//    continue;
				//}

				//теперь не нужна проверка
				//if (extraProfileSubjectID > 0 && profileEntrTestItem.Length == 1)
				//{
				//    if (extraProfileSubjectID != profileEntrTestItem[0].EntranceTestSubject.SubjectID.To(0))
				//    {
				//        ConflictStorage.AddConflictWithCustomMessage(cgDto,
				//            new ConflictStorage.ConflictMessage
				//                {
				//                    Code = ConflictMessages.CompetitiveGroupAllowedExtraProfileSubjectEqualToProfileSubject,
				//                    Message = String.Format(
				//                    "В Конкурсной группе испытание профильной направленности должно совпадать с профильным предметом (ID предмета: {0}).",
				//                        extraProfileSubjectID)
				//                }
				//        );
				//        continue;
				//    }
				//}
			}
		}

		/// <summary>
		/// Разрешены ли творческие испытания
		/// </summary>
		/// <param name="cgDto"></param>
		/// <returns></returns>
		private bool AllowedCreativeEntranceTests(CompetitiveGroupDto cgDto)
		{
			return cgDto.Items.Any(x => DbObjectRepository.EntranceTestCreativeDirections.Any(
				creativeDir => creativeDir.DirectionID == x.DirectionID.To(0)));
		}

		/// <summary>
		/// Получить вступительное испытание профильной направленности (внимание: не профильное испытание!) для конкурсной группы.
		/// </summary>
		private int GetExtraProfileSubjectForCompetitiveGroup(CompetitiveGroupDto cgDto)
		{
			List<int> cgDirectionIDs = cgDto.Items.Select(x => x.DirectionID.To(0)).Distinct().ToList();
			List<int> profileDirectionIDs = DbObjectRepository.EntranceTestProfileDirections
				.Where(x => x.InstitutionID == InstitutionID)
				.Select(x => x.DirectionID).ToList();
			int[] cgProfileDirections = cgDirectionIDs.Intersect(profileDirectionIDs).ToArray();
			if (cgProfileDirections.Length == 0) return 0;

			// Если в массиве несколько направлений и у них разные предметы, то появится конфликт на стадии проверки допустимости направлений в КГ
			int cgProfileDirection = cgProfileDirections[0];

			int[] profileSubjectsID = (from linkDir in DbObjectRepository.DirectionSubjectLinkDirections
			                           from linkSubj in DbObjectRepository.DirectionSubjectLinks
			                           where linkDir.LinkID == linkSubj.ID &&
                                            linkDir.DirectionID == cgProfileDirection && linkSubj.ProfileSubjectID.HasValue
			                           select linkSubj.ProfileSubjectID.Value).ToArray();
			return profileSubjectsID.Length == 0 ? 0 : profileSubjectsID[0];
		}

		/// <summary>
		/// Разрешено ли испытание профильной направленности
		/// </summary>
		private bool IsProfileSubjectsAllowed(CompetitiveGroupDto cgDto)
		{
			List<int> cgDirectionIDs = cgDto.Items.Select(x => x.DirectionID.To(0)).Distinct().ToList();
			List<int> profileDirectionIDs = DbObjectRepository.EntranceTestProfileDirections
				.Where(x => x.InstitutionID == InstitutionID)
				.Select(x => x.DirectionID).ToList();
			int[] cgProfileDirections = cgDirectionIDs.Intersect(profileDirectionIDs).ToArray();
			return cgProfileDirections.Length != 0;
		}

        /// <summary>
        /// Проверка на минимальные баллы ЕГЭ для льгот
        /// </summary>
        /// <param name="cgDto"></param>
        private void CheckMinEgeMarksForCommonBenefits(CompetitiveGroupDto cgDto)
        {
            if (cgDto.CommonBenefit == null && (cgDto.EntranceTestItems == null || cgDto.EntranceTestItems.Count(x => x.EntranceTestBenefits != null) == 0)) return;

            using (var dbContext = new ImportEntities())
            {
                var campaign = dbContext.Campaign.FirstOrDefault(x => x.UID == cgDto.CampaignUID && x.InstitutionID == InstitutionID);
                    
                int campaignYear = DateTime.Today.Year;

                if (campaign != null)
                {
                    campaignYear = campaign.YearStart;
                }

                var systemMinEge = dbContext.GlobalMinEge.FirstOrDefault(x => x.EgeYear == campaignYear);

                if (cgDto.CommonBenefit != null)
                {
                    foreach (var benefit in cgDto.CommonBenefit)
                    {
                        // Проверим на соответствие общесистемному минимальному баллу
                        // Но только если указан год олимпиады больше или равный 2014 - это вводится в 2014 году


                        // Если выбраны олимпиады только с творческими дисциплинами, то проверка не нужна

                        bool isCheckMinMarkNecessary = false;

                        if (benefit.IsForAllOlympics.To(true))
                            isCheckMinMarkNecessary = true;
                        else if (benefit.Olympics != null && benefit.Olympics.Length > 0)
                        { 
                            var attachedOlympics = benefit.Olympics.Select(x => x.To(0));
                            var olympics = dbContext.OlympicType.Where(x => attachedOlympics.Contains(x.OlympicID));

                            // Массив ID предметов не ЕГЭ
                            var subjects = dbContext.Subject.Where(x => !x.IsEge).Select(x => x.SubjectID).ToArray();

                            var allExcludedOlympics = dbContext.OlympicTypeSubjectLink.Where(x => subjects.Contains(x.SubjectID)).Select(x => x.OlympicID).Distinct().ToArray();

                            int allOlympicsCount = olympics.Count();
                            int excludedOlympicsCount = olympics.Count(x => allExcludedOlympics.Contains(x.OlympicID));

                            isCheckMinMarkNecessary = (allOlympicsCount != excludedOlympicsCount);
                        }

                        if (isCheckMinMarkNecessary)
                        {
#warning ХАРДКОД!!!!! ХАРДКОД!!!!! ХАРДКОД!!!!! ХАРДКОД!!!!! ХАРДКОД!!!!! ХАРДКОД!!!!!
                            if (campaignYear >= 2014)
                            {
                                if (benefit.MinEgeMarks == null)
                                {
                                    ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.NoMinEGEMarks);
                                    continue;
                                }

                                var lowSubjects = benefit.MinEgeMarks.Where(x => x.MinMark.To(0) < systemMinEge.MinEgeScore).ToArray();
                                if (lowSubjects.Count() > 0)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    foreach (var lowSubject in lowSubjects)
                                        sb.AppendFormat("Для предмета {0} задан балл ЕГЭ {1}, что меньше установленного минимального балла ({2}) на {3} год\r\n", lowSubject.SubjectID, lowSubject.MinMark, systemMinEge.MinEgeScore, benefit.OlympicYear);

                                    ConflictStorage.ConflictMessage message = new Core.Storages.ConflictStorage.ConflictMessage();
                                    message.Message = sb.ToString();
                                    message.Code = 1019;
                                    ConflictStorage.AddConflictWithCustomMessage(cgDto, message);
                                }
                            }
                        }
                    }
                }

                foreach (EntranceTestItemDto etiDto in cgDto.EntranceTestItems)
                {
                    if (etiDto.EntranceTestBenefits == null) continue;

                    foreach (var benefit in etiDto.EntranceTestBenefits)
                    {
                        // Если выбраны олимпиады только с творческими дисциплинами, то проверка не нужна

                        bool isCheckMinMarkNecessary = false;
                        if (benefit.IsForAllOlympics.To(true))
                            isCheckMinMarkNecessary = true;

                        else if (benefit.Olympics != null && benefit.Olympics.Length > 0)
                        {
                            var attachedOlympics = benefit.Olympics.Select(x => x.To(0));
                            var olympics = dbContext.OlympicType.Where(x => attachedOlympics.Contains(x.OlympicID));

                            // Массив ID предметов не ЕГЭ
                            var subjects = dbContext.Subject.Where(x => !x.IsEge).Select(x => x.SubjectID).ToArray();

                            var allExcludedOlympics = dbContext.OlympicTypeSubjectLink.Where(x => subjects.Contains(x.SubjectID)).Select(x => x.OlympicID).Distinct().ToArray();

                            int allOlympicsCount = olympics.Count();
                            int excludedOlympicsCount = olympics.Count(x => allExcludedOlympics.Contains(x.OlympicID));

                            isCheckMinMarkNecessary = (allOlympicsCount != excludedOlympicsCount);
                        }

                        if (!isCheckMinMarkNecessary) continue;
                        if (systemMinEge == null && campaignYear >= 2014)
                        {
                            ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.NoMinSystemEGE);
                            continue;
                        }

                        if (benefit.MinEgeMark == null && campaignYear >= 2014)
                        {
                            ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.NoMinEgeForOlympics);
                            continue;
                        }

                        if (benefit.MinEgeMark != null && campaignYear >= 2014 && systemMinEge != null && systemMinEge.MinEgeScore > benefit.MinEgeMark.To(0))
                        {
                            ConflictStorage.AddNotImportedDto(cgDto, ConflictMessages.BenefitEGELessSystemMinEGE, benefit.MinEgeMark, systemMinEge.MinEgeScore.ToString());
                            continue;
                        }
                    }
                }
            }
        }

        public void CheckBenefits(AdmissionInfoDto admDto)
        {
            foreach (CompetitiveGroupDto cgDto in admDto.CompetitiveGroups)
                CheckMinEgeMarksForCommonBenefits(cgDto);
        }
		#endregion

		#region Проверка целостности для заявлений

		public void CheckIntegrity(ApplicationDto appDto)
		{
            //Сначала проверка статуса иначе дальше не проходит на проверку и не понятно почему, например, результат ЕГЭ обязателен
            CheckAppDatesAndStatuses(appDto);
			CheckEgeDocumentExistanceForEntranceTestResult(appDto);
			//адресов нет
			//CheckCountryAndRegionIntegrity(appDto.Entrant.RegistrationAddress, appDto);
			//CheckCountryAndRegionIntegrity(appDto.Entrant.FactAddress, appDto);
			CheckApplicationEntranceTestResult(appDto);
			CheckApplicationEntranceTestResultUniqueness(appDto);
			CheckEgeDocumentYearCorrectness(appDto);
			CheckEgeDocumentSubjectCorrectness(appDto);
			CheckRequiredDocuments(appDto);
			CheckApplicationCampaignStatus(appDto);
			CheckApplicationCompetitiveGroupEntranceTests(appDto);
		    CheckEntranceTestExistance(appDto);
            CheckIndividualAchivements(appDto);
            CheckApplicationIsInRecList(appDto);
		}

        private void CheckApplicationIsInRecList(ApplicationDto appDto)
        {

        }

        /// <summary>
        /// Проверка индивидуальных достижений
        /// </summary>
        /// <param name="appDto">Заявление из пакета</param>
        private void CheckIndividualAchivements(ApplicationDto appDto)
        {
            if (appDto.IndividualAchievements == null)
                return;

            foreach (var indAchivement in appDto.IndividualAchievements)
            {
                // Проверка задания имени индивидуального сдостижения
                if (string.IsNullOrEmpty(indAchivement.IAName))
                {
                    ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.IndividualAchivementNameIsEmpty);
                    return;
                }

                // Проверка указания подтверждающего документа
                if (string.IsNullOrEmpty(indAchivement.IADocumentUID))
                {
                    ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.IndividualAchivementDocumentUIDIsEmpty);
                    return;
                }

                // Проверка существования указанного подтверждающего документа
                using (var dbContext = new ImportEntities())
                {
                    bool isDocInDB = dbContext.EntrantDocument.Any(x => x.UID == indAchivement.IADocumentUID && x.DocumentTypeID == 15);
                    bool isDocInPackage = (appDto.ApplicationDocuments.CustomDocuments != null) && appDto.ApplicationDocuments.CustomDocuments.Any(x => x.UID == indAchivement.IADocumentUID);

                    if (!(isDocInDB || isDocInPackage))
                    {
                        ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.IndividualAchivementDocumentNotFound);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Есть ли для данных резуьлтатов ВИ в соответсвующей КГ
        /// </summary>
        /// <param name="dto"></param>
        public void CheckEntranceTestExistance(ApplicationDto dto)
        {
            if (dto.EntranceTestResults == null) return;
            foreach (var enTestResult in dto.EntranceTestResults)
            {
                if (dto.SelectedCompetitiveGroups.Contains(enTestResult.CompetitiveGroupID))
                    continue;
                ConflictStorage.AddNotImportedDto(dto, ConflictMessages.EntranceTestIsNotPartOfCompetitiveGroup, enTestResult.UID);
                return;
            }

        }

	    /// <summary>
		/// Корректный ли статус у кампании, в котороую импортируется заявление
		/// </summary>
		/// <param name="dto"></param>
		public void CheckApplicationCampaignStatus(ApplicationDto dto)
		{
			bool hasInvalidCampaigns = DbObjectRepository.CompetitiveGroups.Where(x => dto.SelectedCompetitiveGroups.Contains(x.UID))
				.Select(x => x.Campaign.StatusID).Where(x => x != 1).Any(); //идёт набор
			if (hasInvalidCampaigns)
			{
				ConflictStorage.AddNotImportedDto(dto, ConflictMessages.ApplicationCannotImportedForCampaignStatus);
			}
		}

		/// <summary>
		/// Не импортируется ли заявление в КГ без ВИ
		/// </summary>
		/// <param name="dto"></param>
		public void CheckApplicationCompetitiveGroupEntranceTests(ApplicationDto dto)
		{
			foreach (var compGroupUID in dto.SelectedCompetitiveGroups)
			{
				var dbCG = DbObjectRepository.CompetitiveGroups.Where(x => x.InstitutionID == InstitutionID && x.UID == compGroupUID).FirstOrDefault();
				if (dbCG != null)
				{
                    /* Если все назначения только на СПО, то вступительных испытаний может не быть */
				    var SPOOnlyExists = dbCG.CompetitiveGroupItem.All(c => c.EducationLevelID == 17);
                    if (!SPOOnlyExists && !DbObjectRepository.CompetitiveGroupEntranceTestItems.Where(x => 
                        x.CompetitiveGroupID == dbCG.CompetitiveGroupID).Any())
					{
						ConflictStorage.AddNotImportedDto(dto, ConflictMessages.ApplicationCannotImportedForCGWithoutEntranceTests);
					}
				}
			}
		}


#warning added Решение по доработке добавления уровня образования - часть доделать здесь.
        // скопирован из EntrantApplicationExtensions
		/// <summary>
		/// Проверка на наличие необходимых документов
		/// </summary>
		/// <param name="dto"></param>
		public void CheckRequiredDocuments(ApplicationDto dto)
		{
			var res = DbObjectRepository.CompetitiveGroupItems.Where(x => dto.SelectedCompetitiveGroupItems.Contains(x.UID))
					.Select(x => new { x.CompetitiveGroup.Course, x.EducationLevelID }).ToArray();
			bool hasSpo = res.Any(x => x.EducationLevelID == EDLevelConst.SPO);
			bool hasVpo1Course =
				res.Any(x => x.Course == 1 &&
					(x.EducationLevelID == EDLevelConst.Bachelor || 
					 x.EducationLevelID == EDLevelConst.Speciality));
			bool hasVpo2Course = res.Any(x => x.Course > 1 &&
					(x.EducationLevelID == EDLevelConst.Bachelor || 
					 x.EducationLevelID == EDLevelConst.Speciality));
			bool hasMag = res.Any(x => x.EducationLevelID == EDLevelConst.Magistracy);
            bool hasHiQual = res.Any(x => x.EducationLevelID == EDLevelConst.HighQualification);

			var attachedDocs = new List<EntrantDocumentType>();
			if (dto.ApplicationDocuments != null && dto.ApplicationDocuments.EduDocuments != null)
			{
				foreach (var eduDocDto in dto.ApplicationDocuments.EduDocuments)
				{
					if (eduDocDto.AcademicDiplomaDocument != null) attachedDocs.Add(EntrantDocumentType.AcademicDiplomaDocument);
					if (eduDocDto.BasicDiplomaDocument != null) attachedDocs.Add(EntrantDocumentType.BasicDiplomaDocument);
					if (eduDocDto.HighEduDiplomaDocument != null) attachedDocs.Add(EntrantDocumentType.HighEduDiplomaDocument);
					if (eduDocDto.IncomplHighEduDiplomaDocument != null) attachedDocs.Add(EntrantDocumentType.IncomplHighEduDiplomaDocument);
					if (eduDocDto.MiddleEduDiplomaDocument != null) attachedDocs.Add(EntrantDocumentType.MiddleEduDiplomaDocument);
					if (eduDocDto.SchoolCertificateBasicDocument != null) attachedDocs.Add(EntrantDocumentType.SchoolCertificateBasicDocument);
					if (eduDocDto.SchoolCertificateDocument != null) attachedDocs.Add(EntrantDocumentType.SchoolCertificateDocument);
                    if (eduDocDto.EduCustomDocument != null) attachedDocs.Add(EntrantDocumentType.EduCustomDocument);
                    if (eduDocDto.PostGraduateDiplomaDocument != null) attachedDocs.Add(EntrantDocumentType.PostGraduateDiplomaDocument);
                    if (eduDocDto.PhDDiplomaDocument != null) attachedDocs.Add(EntrantDocumentType.PhDDiplomaDocument);
				}
			}

			var loadedTypes = attachedDocs.Select(x => (int)x).ToArray();

			Func<int[], string> checkDoc2 = arr =>
			{
				if (!loadedTypes.Intersect(arr).Any())
				{
					string docTypes = String.Join(", ", arr.Select(DbObjectRepository.GetDocumentTypeName));
					return docTypes;
				}

				return null;
			};

            // Сергей Герасимов - место для связи документов с уровнем обр-я
			List<string> errors = new List<string>();
			if (hasSpo)
				errors.Add(checkDoc2(new[] { 16, 3, 4, 5, 6, 19 }));
			if (hasVpo1Course)
                errors.Add(checkDoc2(new[] { 3, 4, 5, 6, 7, 19 }));
			if (hasVpo2Course)
                errors.Add(checkDoc2(new[] { 4, 7, 8, 19 }));
			if (hasMag)
                errors.Add(checkDoc2(new[] { 4, 19 }));
            if (hasHiQual)
                errors.Add(checkDoc2(new[] { 25, 26, 4 }));
            errors = errors.Where(x => x != null).ToList();
			if (errors.Count == 0) return;
			ConflictStorage.AddNotImportedDto(dto, ConflictMessages.ApplicationDoesNotContainRequiredEduDocument,
				String.Join(". А также один из следующих: ", errors));
		}

		/// <summary>
		/// Проверка на уникальность РВИ
		/// </summary>
		/// <param name="appDto"></param>
		private void CheckApplicationEntranceTestResultUniqueness(ApplicationDto appDto)
		{
			if (appDto.EntranceTestResults != null)
			{
				var entrTestWithAppEntrTestIds = new Dictionary<int, string>();
				foreach (var etDto in appDto.EntranceTestResults)
				{
					var sourceID = etDto.ResultSourceTypeID.To((int?)null);
					int entranceTestTypeID = etDto.EntranceTestTypeID.To(0);
					var subjectID = etDto.EntranceTestSubject.SubjectID.To((int?)null);

				    //ищем в базе нужное испытание
					var et = DbObjectRepository.CompetitiveGroupEntranceTestItems.SingleOrDefault(x => 
                        x.CompetitiveGroup.UID == etDto.CompetitiveGroupID 
				        && sourceID.HasValue 
					    && x.EntranceTestTypeID == entranceTestTypeID 
					    && ((!subjectID.HasValue && (x.SubjectName ?? "").ToLower() == etDto.EntranceTestSubject.SubjectName.ToLower()) 
					    || (subjectID.HasValue && x.SubjectID == subjectID)));
					if (et == null)
					{
						etDto.ParentUID = appDto.UID;
						ConflictStorage.AddNotImportedDto(etDto, ConflictMessages.CompetitiveGroupEntranceTestNotFoundForEntranceTestResult);
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.CompetitiveGroupEntranceTestNotFoundForEntranceTestResult);
						continue;
					}

					//проверяем на дубли
					if (entrTestWithAppEntrTestIds.ContainsKey(et.EntranceTestItemID))
					{
						etDto.ParentUID = appDto.UID;
						ConflictStorage.ConflictMessage conflictMessage = new ConflictStorage.ConflictMessage
						{
							Code = ConflictMessages.EntranceTestResultAlreadyImportedForEntranceTestItem,
							Message = String.Format(
								ConflictMessages.GetMessage(ConflictMessages.EntranceTestResultAlreadyImportedForEntranceTestItem),
								et.UID, entrTestWithAppEntrTestIds[et.EntranceTestItemID])
						};
						ConflictStorage.AddConflictWithCustomMessage(etDto, conflictMessage);
						ConflictStorage.AddConflictWithCustomMessage(appDto, conflictMessage);
						continue;
					}

					//проверяем на источник
					if (!sourceID.HasValue)
					{
						ConflictStorage.AddNotImportedDto(etDto, ConflictMessages.EntranceTestSourceIDNotFound);
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.EntranceTestSourceIDNotFound);
						continue;
					}

				    var appDb = DbObjectRepository.FindApplicationByUID(appDto.UID, false);
                    //var appDb = DbObjectRepository.GetObject<Application>(appDto.UID);

					//проверяем в зависимости от типа, всё ли хорошо
					switch ((EntranceTestResultSourceEnum)sourceID.Value)
					{
						case EntranceTestResultSourceEnum.EgeDocument:
							if (etDto.ResultDocument == null || String.IsNullOrWhiteSpace(etDto.ResultDocument.EgeDocumentID))
							{
								bool isInNotOrderDb = appDb == null || appDb.OrderOfAdmissionID == null;
								if (isInNotOrderDb) //если не включено в приказ, тогда разрешено не иметь источника. #22904
									break;

								ConflictStorage.AddNotImportedDto(etDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								continue;
							}

							string egeDocumentID = etDto.ResultDocument.EgeDocumentID;

							//указано свидетельство ЕГЭ, а самого свидетельства нет
							if (appDto.ApplicationDocuments == null || appDto.ApplicationDocuments.EgeDocuments == null ||
								!appDto.ApplicationDocuments.EgeDocuments.Where(x => x.UID == egeDocumentID).Any())
							{
								ConflictStorage.AddNotImportedDto(etDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								continue;
							}

							break;

						case EntranceTestResultSourceEnum.GiaDocument:
							if (etDto.ResultDocument == null || String.IsNullOrWhiteSpace(etDto.ResultDocument.EgeDocumentID))
							{
                                bool isInNotOrderDb = appDb == null || appDb.OrderOfAdmissionID == null;
								if (isInNotOrderDb) //если не включено в приказ, тогда разрешено не иметь источника. #22904
									break;

								ConflictStorage.AddNotImportedDto(etDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								continue;
							}

							string giaDocumentID = etDto.ResultDocument.EgeDocumentID;

							//указана справка ГИА, а самой справки нет
							if (appDto.ApplicationDocuments == null || appDto.ApplicationDocuments.GiaDocuments == null ||
								!appDto.ApplicationDocuments.GiaDocuments.Where(x => x.UID == giaDocumentID).Any())
							{
								ConflictStorage.AddNotImportedDto(etDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								continue;
							}

							break;

						case EntranceTestResultSourceEnum.OlympicDocument:
							//указан олимпийский документ, а документа нет
							if (etDto.ResultDocument == null ||
								(etDto.ResultDocument.OlympicDocument == null && etDto.ResultDocument.OlympicTotalDocument == null))
							{
								ConflictStorage.AddNotImportedDto(etDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.DocumentNotAttachedToEntranceTestResult);
								continue;
							}

							break;
					}

					entrTestWithAppEntrTestIds.Add(et.EntranceTestItemID, etDto.UID);
				}
			}
		}

		/// <summary>
		/// Проверяем РВИ у заявления
		/// </summary>
		/// <param name="appDto"></param>
		private void CheckApplicationEntranceTestResult(ApplicationDto appDto)
		{			
			if (appDto.EntranceTestResults != null)
				foreach (var entrTestDto in appDto.EntranceTestResults)
				{
					// проверка на существование предмета
					if (entrTestDto.EntranceTestSubject.SubjectID != null)
						CheckIsSubjectExists(entrTestDto.EntranceTestSubject.SubjectID.To(0), appDto);

					//проверка на корректность типа ВИ
					CheckEntranceTestSourceID(entrTestDto.ResultSourceTypeID.To(0), appDto);

					//проверка на ЕГЭ предмет
					if (entrTestDto.ResultDocument != null && !String.IsNullOrEmpty(entrTestDto.ResultDocument.EgeDocumentID))
					{
						var subjectID = entrTestDto.EntranceTestSubject.SubjectID.To(0);
						bool isEge = false;
						if (subjectID > 0)
						{
							var subject = DbObjectRepository.GetSubject(subjectID);
							if (subject != null) isEge = subject.IsEge;
						}

						//кастомный предмет, а источник свидетельство
						if (subjectID == 0 && entrTestDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.EgeDocument)
						{
							ConflictStorage.AddNotImportedDto(entrTestDto, ConflictMessages.NotAllowedSubjectForEgeDocument);
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.NotAllowedSubjectForEgeDocument);
						}

						//не ЕГЭ предмет со свидетельством
						if (subjectID > 0 && !isEge && entrTestDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.EgeDocument)
						{
							ConflictStorage.AddNotImportedDto(entrTestDto, ConflictMessages.NotAllowedSubjectForEgeDocument);
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.NotAllowedSubjectForEgeDocument);
						}

						//для ГИА более серьёзная проверка
						//кастомный предмет со справкой
						if (subjectID == 0 && entrTestDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.GiaDocument)
						{
							ConflictStorage.AddNotImportedDto(entrTestDto, ConflictMessages.NotAllowedSubjectForGiaDocument);
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.NotAllowedSubjectForGiaDocument);
						}

						//указан ли предмет в справке ГИА
						if (subjectID > 0 && entrTestDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.GiaDocument)
						{
							//проверка на корректность документа в другом месте. Тут именно предмет проверяем
							if (appDto.ApplicationDocuments != null && appDto.ApplicationDocuments.GiaDocuments != null)
							{
								var giaDoc = appDto.ApplicationDocuments.GiaDocuments.Where(x => x.UID == entrTestDto.ResultDocument.EgeDocumentID).FirstOrDefault();
								if (giaDoc != null)
								{
									if (giaDoc.Subjects.All(x => x.SubjectID.To(0) != subjectID))
									{
										ConflictStorage.AddNotImportedDto(entrTestDto, ConflictMessages.NotAllowedSubjectForGiaDocument);
										ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.NotAllowedSubjectForGiaDocument);
									}
								}
							}
						}
					}

					//ещё раз проверяем предмет, если документ основание не приложен. Но ЕГЭ
					if (entrTestDto.ResultDocument == null && entrTestDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.EgeDocument)
					{
						var subjectID = entrTestDto.EntranceTestSubject.SubjectID.To(0);
						if (subjectID == 0)
						{
							ConflictStorage.AddNotImportedDto(entrTestDto, ConflictMessages.NotAllowedSubjectForEgeDocument);
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.NotAllowedSubjectForEgeDocument);
						}
					}

					// проверка на ResultValue для испытаний где источник - ЕГЭ или само вступительное испытание ОУ
					switch ((EntranceTestResultSourceEnum)entrTestDto.ResultSourceTypeID.To(0))
					{
						case EntranceTestResultSourceEnum.EgeDocument:
						case EntranceTestResultSourceEnum.GiaDocument:
						case EntranceTestResultSourceEnum.InstitutionEntranceTest:
							if (entrTestDto.ResultValue != null) //есть результат, проверям, подходит ли он
                                CheckResultValue(entrTestDto.ResultValue.To<decimal>(provider: CultureInfo.InvariantCulture), appDto, true);
							break;
						case EntranceTestResultSourceEnum.OlympicDocument:
                            break;
					}
				}

			// проверить целостность данных для общей льготы
			foreach (var appCommonBenefitDto in appDto.GetCommonBenefits())
			{
				short? benefitID = String.IsNullOrEmpty(appCommonBenefitDto.BenefitKindID) ? null :
					appCommonBenefitDto.BenefitKindID.To((short?)0);
				if (benefitID == null || (benefitID != 1 && benefitID != 4 && benefitID != 5))
				{
					ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.NotAllowedCommonBenefitTypeForApplication);
				}

				if (appCommonBenefitDto.DocumentReason == null)
				{
					//#40117 Сделать документ-основание общей льготы необязательной
					// ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.DocumentNotSpecifiedForBenefit);
					// return;
					continue;
				}
				//#22899 Если документ-основание (DocumentReason) не соответствует виду льготы (BenefitKindID), то ошибки не возникает
				if (benefitID != null && benefitID == 1) //Зачисление без вступительных испытаний
				{
					//DocumentTypeID судя по всему не используется
					if (/*!new [] {0, 9, 10, 15}.Contains(appCommonBenefitDto.DocumentTypeID.To(0)) ||*/
						(appCommonBenefitDto.DocumentReason.CustomDocument == null 
						&& appCommonBenefitDto.DocumentReason.OlympicDocument == null
						&& appCommonBenefitDto.DocumentReason.OlympicTotalDocument == null))
					{
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
					}
				}

				if (benefitID != null && benefitID == 4) //Вне конкурса
				{
					if (/*!new[] {0, 11, 15 }.Contains(appCommonBenefitDto.DocumentTypeID.To(0)) ||*/
						(appCommonBenefitDto.DocumentReason.CustomDocument == null
						&& appCommonBenefitDto.DocumentReason.MedicalDocuments == null))
					{
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
					}
				}

				if (benefitID != null && benefitID == 5) //Преимущественное право
				{
					if (appCommonBenefitDto.DocumentReason.CustomDocument == null)
					{
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.InvalidDocumentSpecifiedForBenefit);
					}
				}

                // Проверим баллы в документе на соответствие баллам льготы

                if (benefitID != null && benefitID == 1) // Льгота - поступление без ВИ
                {
                    if (appCommonBenefitDto.DocumentReason.OlympicDocument != null)
                    {
                        using (var dbContext = new ImportEntities())
                        {
                            var cgId = appCommonBenefitDto.CompetitiveGroupID;
                            var groupBenefits = dbContext.BenefitItemC.Where(x => x.CompetitiveGroup.UID == cgId && !x.EntranceTestItemID.HasValue).ToList();

                            var olympicsId = appCommonBenefitDto.DocumentReason.OlympicDocument.OlympicID.To(0);
                            var olympicsBenefitItemIds = dbContext.BenefitItemCOlympicType.Where(x => x.OlympicTypeID == olympicsId).Select(x => x.BenefitItemID);

/*                            var AllEgeResults = appDto.ApplicationDocuments.EgeDocuments.Where(x => !string.IsNullOrEmpty(x..ResultValue) && x.CompetitiveGroupID == cgId)
                                .Select(x => new
                                {
                                    egeResult = x.ResultValue,
                                    SubjectID = x.EntranceTestSubject.SubjectID,
                                    SubjectName = x.EntranceTestSubject.SubjectName
                                }).ToList();*/
                        }
                    }
                }
			}
		}

		//метод пока не используется, но оставлен на случай возвращения стран
		/*private void CheckCountryAndRegionIntegrity(AddressDto addressDto, ApplicationDto appDto)
		{
			if (addressDto == null) return;
			//DbObjectRepository.Coun
			RegionType region = DbObjectRepository.GetRegion(addressDto.RegionID.To(0));
			if(region == null)
			{
				ConflictStorage.AddConflictWithCustomMessage(appDto,
					new ConflictStorage.ConflictMessage
						{
							Code = ConflictMessages.RegionIsNotFounded,
							Message = String.Format("Регион {0} отсутствует для указанного идентификатора.", addressDto.RegionID)
						});			
			}

			CountryType country = DbObjectRepository.GetCountry(addressDto.CountryID.To(0));
			if (country == null)
			{
				ConflictStorage.AddConflictWithCustomMessage(appDto,
					new ConflictStorage.ConflictMessage
					{
						Code = ConflictMessages.CountryIsNotFounded,
						Message = String.Format("Страна {0} отсутствует для указанного идентификатора.", addressDto.CountryID)
					});				
			}

			if(region != null && country != null)
			{
				if(region.CountryID != country.CountryID)
					ConflictStorage.AddConflictWithCustomMessage(appDto,
						new ConflictStorage.ConflictMessage
						{
							Code = ConflictMessages.RegionIsNotFoundedForCountry,
							Message = String.Format("Страна {0} не содержит указанный регион {1}.", addressDto.CountryID, addressDto.RegionID)
						});
			}
		}*/

		/// <summary>
		/// Проверка наличия документа ЕГЭ для РВИ
		/// </summary>
		public void CheckEgeDocumentExistanceForEntranceTestResult(ApplicationDto appDto)
		{
			if (appDto.EntranceTestResults == null) return;

			foreach (var entranceTestResultDto in appDto.EntranceTestResults)
			{
                if (entranceTestResultDto.ResultDocument == null) continue;
                var egeDocumentID = entranceTestResultDto.ResultDocument.EgeDocumentID;
                
                /* Указан неверный тип документа-основания */
			    if (entranceTestResultDto.ResultDocument.InstitutionDocument != null &&
			        entranceTestResultDto.ResultSourceTypeID.To(0) != (int) EntranceTestResultSourceEnum.InstitutionEntranceTest)
			    {
                    ConflictStorage.AddConflictWithCustomMessage(appDto,
                    new ConflictStorage.ConflictMessage
                    {
                        Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                        Message = String.Format("Указан неверный тип документа-основания для документа о внутреннем испытании ОУ")
                    });
                    continue;
			    }
                if ((entranceTestResultDto.ResultDocument.OlympicDocument != null || 
                    entranceTestResultDto.ResultDocument.OlympicTotalDocument != null) &&
                    entranceTestResultDto.ResultSourceTypeID.To(0) != (int)EntranceTestResultSourceEnum.OlympicDocument)
                {
                    ConflictStorage.AddConflictWithCustomMessage(appDto,
                    new ConflictStorage.ConflictMessage
                    {
                        Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                        Message = String.Format("Указан неверный тип документа-основания для диплома победителя/призера олимпиады")
                    });
                    continue;
                }
                if (String.IsNullOrWhiteSpace(egeDocumentID) && (
                    entranceTestResultDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.EgeDocument ||
                    entranceTestResultDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.GiaDocument))
                {
                    ConflictStorage.AddConflictWithCustomMessage(appDto,
                    new ConflictStorage.ConflictMessage
                    {
                        Code = ConflictMessages.DocumentNotAttachedToEntranceTestResult,
                        Message = String.Format("Указан неверный тип документа-основания")
                    });
                    continue;
                }

                if (string.IsNullOrEmpty(egeDocumentID)) continue;

                // ищем ЕГЭ свидетельство в данных пользователя
                if (appDto.ApplicationDocuments != null && appDto.ApplicationDocuments.EgeDocuments != null
                    && entranceTestResultDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.EgeDocument)
                {
                    if (appDto.ApplicationDocuments.EgeDocuments.Count(x => !String.IsNullOrWhiteSpace(x.UID) && x.UID == egeDocumentID) == 1)
                        continue;
                }

				//ищем справку ГИА в данных
				if (appDto.ApplicationDocuments != null && appDto.ApplicationDocuments.GiaDocuments != null
					&& entranceTestResultDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.GiaDocument)
				{
					if (appDto.ApplicationDocuments.GiaDocuments.Count(x => !String.IsNullOrWhiteSpace(x.UID) && x.UID == egeDocumentID) == 1)
						continue;
				}

				//мы тут, если правильные случаи кончились
				if (appDto.ApplicationDocuments != null && appDto.ApplicationDocuments.EgeDocuments != null)
				{
					//приложен документ к не тому типу РВИ
					if (appDto.ApplicationDocuments.EgeDocuments.Count(x => !String.IsNullOrWhiteSpace(x.UID) && x.UID == egeDocumentID) == 1)
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.NotAllowedEgeDocumentForEntranceTestResult);
				}
				else
				{
					//нет нужных документов
					if (entranceTestResultDto.ResultSourceTypeID.To(0) == (int)EntranceTestResultSourceEnum.GiaDocument)
					{
						ConflictStorage.AddConflictWithCustomMessage(appDto,
						new ConflictStorage.ConflictMessage
						{
							Code = ConflictMessages.ReferencedEgeDocumentIsAbsent,
							Message = String.Format("В списке документов заявления не найдено указанной справки ГИА {0} для вступительного испытания {1}.",
													egeDocumentID, entranceTestResultDto.UID)
						});
					}
					else
					{
						ConflictStorage.AddConflictWithCustomMessage(appDto,
							new ConflictStorage.ConflictMessage
							{
								Code = ConflictMessages.ReferencedEgeDocumentIsAbsent,
								Message = String.Format("В списке документов заявления не найдено указанного ЕГЭ документа {0} для вступительного испытания {1}.",
													egeDocumentID, entranceTestResultDto.UID)
							});
					}
				}
			}
		}

		/// <summary>
		/// Проверка на корректность даты свидетельства ЕГЭ
		/// </summary>
		/// <param name="appDto"></param>
		public void CheckEgeDocumentYearCorrectness(ApplicationDto appDto)
		{
			if (appDto.ApplicationDocuments != null && appDto.ApplicationDocuments.EgeDocuments != null)
			{
				foreach (EgeDocumentWithSubjectsDto egeDto in appDto.ApplicationDocuments.EgeDocuments)
				{
					if (!String.IsNullOrEmpty(egeDto.RawDocumentYear) && !String.IsNullOrEmpty(egeDto.DocumentDate))
					{
						if (egeDto.RawDocumentYear.To(0) != egeDto.DocumentDate.GetStringAsDate().Year)
						{
							ConflictStorage.AddNotImportedDto(egeDto, ConflictMessages.EgeDocumentYearDateInvalid);
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.EgeDocumentYearDateInvalid);
						}
					}

					if (String.IsNullOrEmpty(egeDto.DocumentYear))
					{
						ConflictStorage.AddNotImportedDto(egeDto, ConflictMessages.EgeDocumentDateMissing);
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.EgeDocumentDateMissing);
					}
				}
			}
		}

		/// <summary>
		/// Проверка на корректность предметов в приложенном свидетельстве ЕГЭ
		/// </summary>
		/// <param name="appDto"></param>
		public void CheckEgeDocumentSubjectCorrectness(ApplicationDto appDto)
		{
			// ищем ЕГЭ свидетельство в данных пользователя
			if (appDto.ApplicationDocuments != null && appDto.ApplicationDocuments.EgeDocuments != null)
			{
				foreach (EgeDocumentWithSubjectsDto egeDocumentWithSubjectsDto in appDto.ApplicationDocuments.EgeDocuments)
				{
					//если указали предметы
					if (egeDocumentWithSubjectsDto.Subjects != null)
					{
						//проверяем на дубли
						if (egeDocumentWithSubjectsDto
							.Subjects
							.Select(x => x.SubjectID)
							.Distinct().Count() < egeDocumentWithSubjectsDto.Subjects.Length)
						{
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.SubjectsInEgeDocumentIsDupllicated);
						}

						//проверям по справочнику
						CheckDictionaryValues(egeDocumentWithSubjectsDto
							.Subjects
							.Select(x => x.SubjectID), "SubjectID", appDto, DbObjectRepository.GetSubject);
					}
				}
			}
			//та же проверка для ГИА
			// ищем ГИА свидетельство в данных пользователя
			if (appDto.ApplicationDocuments != null && appDto.ApplicationDocuments.GiaDocuments != null)
			{
				foreach (GiaDocumentWithSubjectsDto giaDocumentWithSubjectsDto in appDto.ApplicationDocuments.GiaDocuments)
				{
    			    if (giaDocumentWithSubjectsDto.Subjects != null)
					{
						if (giaDocumentWithSubjectsDto
							.Subjects
							.Select(x => x.SubjectID)
							.Distinct().Count() < giaDocumentWithSubjectsDto.Subjects.Length)
						{
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.SubjectsInGiaDocumentIsDupllicated);
						}

						CheckDictionaryValues(giaDocumentWithSubjectsDto
							.Subjects
							.Select(x => x.SubjectID), "SubjectID", appDto, DbObjectRepository.GetSubject);
					}
				}
			}
		}

		/// <summary>
		/// Проверка на уникальность номера заявления
		/// </summary>
		public void CheckAppNumberUnique(ApplicationDto[] applicationsDto)
		{
			List<string> appNumbers = new List<string>(applicationsDto.Length);
			foreach (var appDto in applicationsDto)
			{
				if (appNumbers.Contains(appDto.ApplicationNumber))
					ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationNumberIsNotUnique);
				else
					appNumbers.Add(appDto.ApplicationNumber);
			}
		}

		/// <summary>
		/// Проверка на соответствие номера заявления и уида
		/// </summary>
		/// <param name="applicationsDto"></param>
		public void CheckAppNumberAndUIDRelated(ApplicationDto[] applicationsDto)
		{
			foreach (var appDto in applicationsDto)
			{
                var dbApp = DbObjectRepository.FindApplicationByNumber(appDto.ApplicationNumber, false);
				//var dbApp = DbObjectRepository.Applications.Where(x => x.ApplicationNumber == appDto.ApplicationNumber).FirstOrDefault();
				if (dbApp != null)
				{
                    if (dbApp.UID != appDto.UID)
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationNumberIsNotCorrelateWithUID);
				}

                var dbApp2 = DbObjectRepository.FindApplicationByUID(appDto.UID, false);
				//var dbApp2 = DbObjectRepository.Applications.Where(x => x.UID == appDto.UID).FirstOrDefault();
				if (dbApp2 != null)
				{
					if (dbApp2.ApplicationNumber != appDto.ApplicationNumber)
						ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationUIDIsNotCorrelateWithNumber);
					else
					{
						if (dbApp2.RegistrationDate != appDto.RegistrationDateDate)
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationUIDNumberIsNotCorrelateWithDate);
					}
				}
			}
		}

		/// <summary>
		/// Проверка на даты и статусы заявления
		/// </summary>
		/// <param name="appDto"></param>
		private void CheckAppDatesAndStatuses(ApplicationDto appDto)
		{
			var regDate = appDto.RegistrationDateDate;
			var denyDate = appDto.LastDenyDate.GetStringOrEmptyAsDate();
			//отклонили раньше чем подали
			if (denyDate.HasValue && regDate > denyDate.Value)
			{
				ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationLastDenyDateShouldBeGreaterRegistrationDate);
			}
            //Проверка текущего статуса, если заявление в приказе, то нельзя его обновлять
            if(DbObjectRepository.Applications.Any(x => x.ApplicationNumber == appDto.ApplicationNumber && x.OrderOfAdmissionId != null))
                ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationsInOrderIsNotAllowedToUpdate, appDto.ApplicationNumber);
			//есть ли статус в справочнике
			CheckDictionaryValues(appDto.StatusID, "StatusID", appDto, DbObjectRepository.GetApplicationStatus);
			if (appDto.StatusID.To(0) == ApplicationStatusType.InOrder) //в приказе
			{
				ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationInOrderCannotBeImported);
			}
			//1.	В Систему передаваться должны только заявления в статусе «Принятые» и «Отозванные». 
			if (appDto.StatusID.To(0) != ApplicationStatusType.Accepted && appDto.StatusID.To(0) != ApplicationStatusType.Denied)
			{
				ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationCannotBeImportedExceptAcceptedDenied);
			}
			
			//проверяем на наличие конкурсной группы
			foreach (var compGroupUID in appDto.SelectedCompetitiveGroups)
			{
				var cg = DbObjectRepository.GetObject<CompetitiveGroup>(compGroupUID);
				if (cg == null)
				{
					ConflictStorage.AddConflictWithCustomMessage(appDto, new ConflictStorage.ConflictMessage
					{
						Code = ConflictMessages.ApplicationContainsInvalidCompetitiveGroupID,
						Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.ApplicationContainsInvalidCompetitiveGroupID), compGroupUID)
					});
				}
			}

			//проверяем на наличие элементов конкурсной группы
			foreach (var compGroupItemUID in (appDto.SelectedCompetitiveGroupItems ?? new string[0]))
			{
				bool hasCGI = DbObjectRepository.CompetitiveGroupItems.Where(x => x.UID == compGroupItemUID).Any();
				if (!hasCGI)
				{
					ConflictStorage.AddConflictWithCustomMessage(appDto, new ConflictStorage.ConflictMessage
					{
						Code = ConflictMessages.ApplicationContainsInvalidCompetitiveGroupItemID,
						Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.ApplicationContainsInvalidCompetitiveGroupItemID), compGroupItemUID)
					});
				}
			}

			//проверяем на корректность форм обучения и источников финансирования
			if (appDto.FinSourceAndEduForms != null)
			{
				CheckDictionaryValues(appDto.FinSourceAndEduForms.Select(x => x.FinanceSourceID), "FinanceSourceID", appDto, DbObjectRepository.GetFinanceSource);
				CheckDictionaryValues(appDto.FinSourceAndEduForms.Select(x => x.EducationFormID), "EducationFormID", appDto, DbObjectRepository.GetEducationForm);
				var avForms = GetAvailableEducationFormsForCompetitiveGroup(appDto.SelectedCompetitiveGroupItems);
				int cnt = appDto.FinSourceAndEduForms
					.Select(x => new Tuple<int, int>(x.EducationFormID.To(0), x.FinanceSourceID.To(0)))
					.Except(avForms).Count();
				if (cnt > 0)
				{
					ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationContainsNotAllowedFinSourceEducationForm);
				}

				var targetReceptions = appDto.FinSourceAndEduForms.Where(x => x.FinanceSourceID.To(0) == AdmissionItemTypeConstants.TargetReception).ToArray();
				if (targetReceptions.Length > 0)
				{
					foreach (var targetReception in targetReceptions)
					{
						if (targetReception.TargetOrganizationUID == null
							|| !DbObjectRepository.CompetitiveGroupTargets
								.Where(x => x.UID == targetReception.TargetOrganizationUID && x.InstitutionID == InstitutionID).Any())
						{
							ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.ApplicationContainsIncorrectTargetOrganizationUID);
						}
					}
				}
			}

            /* Проверяем на правильность заполнения приоритетов
             В постановке для сервиса проверок на одинаковость и нули в приоритетах нет - значит и не делаем
             Из постановки: 
             "Если указано только одно или 2 поля, то необходимо выдавать сообщение об ошибке" */

            if (appDto.NewSourcesAndForms != null && appDto.NewSourcesAndForms.Count > 0)
            {
                var correct = appDto.NewSourcesAndForms.Where(x => x.Priority.HasValue && !string.IsNullOrEmpty(x.CompetitiveGroupUID) && !string.IsNullOrEmpty(x.CompetitiveGroupItemUID)).Count();
                if (correct != appDto.NewSourcesAndForms.Count)
                    ConflictStorage.AddNotImportedDto(appDto, ConflictMessages.CompetitiveGroupItemPriorityIncorrect);
            }
		}

		/// <summary>
		/// Получаем разрешённые формы обучения (по наличию мест) в выбранных конкурсных группах (направлениях)
		/// </summary>
		public Tuple<int, int>[] GetAvailableEducationFormsForCompetitiveGroup(string[] competitiveGroupItemUIDs)
		{
			//ImportEntities _importEntities = DbObjectRepository.ImportEntities;
			//var institutionID = DbObjectRepository.InstitutionID;
			if (competitiveGroupItemUIDs == null) return new Tuple<int, int>[0];
			var groupItems = DbObjectRepository.CompetitiveGroupItems
						.Where(x => competitiveGroupItemUIDs.Contains(x.UID))
						.Select(x => new
							  {
								  x.NumberBudgetO,
								  x.NumberBudgetOZ,
								  x.NumberBudgetZ,
								  x.NumberPaidO,
								  x.NumberPaidOZ,
								  x.NumberPaidZ,
                                  x.NumberQuotaO,
                                  x.NumberQuotaOZ,
                                  x.NumberQuotaZ
							  }).ToArray();
			List<Tuple<int, int>> availableForms = new List<Tuple<int, int>>();
			if (groupItems.Sum(x => x.NumberBudgetO) > 0)
				availableForms.Add(new Tuple<int, int>(11, 14));
			if (groupItems.Sum(x => x.NumberBudgetOZ) > 0)
				availableForms.Add(new Tuple<int, int>(12, 14));
			if (groupItems.Sum(x => x.NumberBudgetZ) > 0)
				availableForms.Add(new Tuple<int, int>(10, 14));
			if (groupItems.Sum(x => x.NumberPaidO) > 0)
				availableForms.Add(new Tuple<int, int>(11, 15));
			if (groupItems.Sum(x => x.NumberPaidOZ) > 0)
				availableForms.Add(new Tuple<int, int>(12, 15));
			if (groupItems.Sum(x => x.NumberPaidZ) > 0)
				availableForms.Add(new Tuple<int, int>(10, 15));
            if (groupItems.Sum(x => x.NumberQuotaO) > 0)
                availableForms.Add(new Tuple<int, int>(11, 20));
            if (groupItems.Sum(x => x.NumberQuotaOZ) > 0)
                availableForms.Add(new Tuple<int, int>(12, 20));
            if (groupItems.Sum(x => x.NumberQuotaZ) > 0)
                availableForms.Add(new Tuple<int, int>(10, 20));

			var target = (from x in DbObjectRepository.CompetitiveGroupTargetItems
						  where competitiveGroupItemUIDs.Contains(x.CompetitiveGroupItem.UID)
						  select new { x.NumberTargetO, x.NumberTargetOZ, x.NumberTargetZ }).ToArray();
			if (target.Sum(x => x.NumberTargetO) > 0)
				availableForms.Add(new Tuple<int, int>(11, 16));
			if (target.Sum(x => x.NumberTargetOZ) > 0)
				availableForms.Add(new Tuple<int, int>(12, 16));
			if (target.Sum(x => x.NumberTargetZ) > 0)
				availableForms.Add(new Tuple<int, int>(10, 16));
			return availableForms.ToArray();
		}

		#endregion

		#region Проверка целостности испытаний

		/// <summary>
		/// Есть ли указанный предмет в справочнике
		/// </summary>
		public void CheckIsSubjectExists(int subjectID, BaseDto dto)
		{
			if (DbObjectRepository.GetSubject(subjectID) == null)
			{
				ConflictStorage.AddConflictWithCustomMessage(
					dto, new ConflictStorage.ConflictMessage
					{
						Code = ConflictMessages.SubjectIsNotFounded,
						Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.SubjectIsNotFounded), subjectID)
					});
			}
		}

		/// <summary>
		/// Корректен ли результат РВИ (для ОУ можно 0 баллов)
		/// </summary>
		public void CheckResultValue(decimal resultValue, BaseDto dto, bool isOUResult)
		{
			if (resultValue < (isOUResult ? 0 : 1) || resultValue > 100)
			{
				ConflictStorage.AddConflictWithCustomMessage(
					dto, new ConflictStorage.ConflictMessage
					{
						Code = ConflictMessages.ResultValueRange,
						Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.ResultValueRange), resultValue)
					});
			}
		}

		/// <summary>
		/// Проверка на корректность источника РВИ
		/// </summary>
		/// <param name="entranceTestSourceID"></param>
		/// <param name="dto"></param>
		public void CheckEntranceTestSourceID(int entranceTestSourceID, BaseDto dto)
		{
			if (!Enum.IsDefined(typeof(EntranceTestResultSourceEnum), entranceTestSourceID))
			{
				ConflictStorage.AddConflictWithCustomMessage(
					dto, new ConflictStorage.ConflictMessage
					{
						Code = ConflictMessages.EntranceTestSourceIDNotFound,
						Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.EntranceTestSourceIDNotFound), entranceTestSourceID)
					});
			}
		}

		#endregion

		#region Проверки целостности для кампаний

		public void CheckIntegrity(CampaignDto dto)
		{
		    //всё ли в порядке с годами
			if (dto.YearEnd.To(0) < dto.YearStart.To(0) 
				|| dto.YearStart.To(0) < 1900 
				|| String.IsNullOrWhiteSpace(dto.Name)
				|| dto.EducationLevels == null)
			{
				ConflictStorage.AddNotImportedDto(dto, ConflictMessages.CampaignWithInvalidData);
				return;
			}

			//всё ли в порядке со статусами
			if (dto.StatusID.To(0) < 0 || dto.StatusID.To(0) > 2)
			{
				ConflictStorage.AddNotImportedDto(dto, ConflictMessages.CampaignWithInvalidData);
				return;
			}

//#warning https://redmine.armd.ru/issues/17866
//            /* Если указаны значения AdditionalSet и Stage одновременно */
//            if ((dto.AdditionalSet ?? false) && dto.CampaignDates.Any(c => !string.IsNullOrEmpty(c.Stage)))
//            {
//                ConflictStorage.AddNotImportedDto(dto, ConflictMessages.CampaignStageAndAdditionalExists);
//                return;
//            }

			//корректные ли формы
			CheckDictionaryValues(dto.EducationForms, "EducationFormID", dto, DbObjectRepository.GetEducationForm);

			//не лишние ли уровни образования
			var allowedDirectionsByInstition =
				StorageManager.DbObjectRepository.AllowedDirections.Where(y => y.InstitutionID == InstitutionID)
					.Select(x => x.AdmissionItemTypeID).Distinct();
			var exceedLevels = dto.EducationLevels.Select(x => x.EducationLevelID.To((short)0)).Except(allowedDirectionsByInstition);
			if (exceedLevels.Any())
			{
				ConflictStorage.AddConflictWithCustomMessage(dto,
						new ConflictStorage.ConflictMessage
						{
							Code = ConflictMessages.CampaignWithInvalidEducationLevelsByInstitution,
							Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.CampaignWithInvalidEducationLevelsByInstitution),
								String.Join("; ", exceedLevels.Select(x => DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, x)))),
						});
			}

			List<CampaignDateDto> allowedDates = new List<CampaignDateDto>();
			Action<string, int, string, int, int> addCampaignDateDetails = (course, form, level, source, stage) => allowedDates.Add(new CampaignDateDto
			                                                                                                                  {
			                                                                                                                  	Course = course.ToString(),
			                                                                                                                  	EducationFormID = form.ToString(),
			                                                                                                                  	EducationLevelID = level.ToString(),
			                                                                                                                  	EducationSourceID = source.ToString(),
			                                                                                                                  	Stage = stage > 0 ? stage.ToString() : null
			                                                                                                                  });
			//собираем допустимые даты по параметрам
			foreach (var edFormID in dto.EducationForms.Select(x => x.To(0)))
			{
				foreach (var campaignEducationLevel in dto.EducationLevels)
				{
					//первый и второй этапы для бакалавриата и специалитета первого курса
					//#40897 - Для сокращенного бакалавриата не должно существовать этапов приема
					int[] allowedStages = campaignEducationLevel.Course.To(0) == 1 && (edFormID == EDFormsConst.O || edFormID == EDFormsConst.OZ)
						&& new int[]
						   {
						   		EDLevelConst.Bachelor, EDLevelConst.Speciality
						   }.Contains(campaignEducationLevel.EducationLevelID.To(0)) ? new[] { 1, 2 } : new[] { 0 };
					foreach (var allowedStage in allowedStages)
					{
						addCampaignDateDetails(campaignEducationLevel.Course, edFormID, campaignEducationLevel.EducationLevelID, EDSourceConst.Budget, allowedStage);
					}

					addCampaignDateDetails(campaignEducationLevel.Course, edFormID, campaignEducationLevel.EducationLevelID, EDSourceConst.Paid, 0);
					//if (edFormID == AdmissionItemTypeConstants.FullTimeTuition)
					addCampaignDateDetails(campaignEducationLevel.Course, edFormID, campaignEducationLevel.EducationLevelID, EDSourceConst.Target, 0);
				}
			}

			if (dto.CampaignDates != null)
			{
				var processedDates = new List<CampaignDateDto>();
				foreach (var cdDto in dto.CampaignDates)
				{
					cdDto.ParentUID = dto.UID;
					//проверяем на корректность справочных данных
					CheckDictionaryValues(cdDto.EducationFormID, "EducationFormID", cdDto, DbObjectRepository.GetEducationForm);
					CheckDictionaryValues(cdDto.EducationSourceID, "EducationSourceID", cdDto, DbObjectRepository.GetFinanceSource);
					CheckDictionaryValues(cdDto.EducationLevelID, "EducationLevelID", cdDto, DbObjectRepository.GetEducationalLevel);
					if (cdDto.Course.To(0) < 0 || cdDto.Course.To(0) > 6)
						ConflictStorage.AddNotImportedDto(cdDto, ConflictMessages.DictionaryItemAbsent, "Course");
					if (cdDto.Stage != null && (cdDto.Stage.To(0) < 1 || cdDto.Stage.To(0) > 2))
						ConflictStorage.AddNotImportedDto(cdDto, ConflictMessages.DictionaryItemAbsent, "Stage");
					//нормальные ли даты
					if (cdDto.DateStart.GetStringOrEmptyAsDate() > cdDto.DateEnd.GetStringOrEmptyAsDate())
						ConflictStorage.AddNotImportedDto(cdDto, ConflictMessages.CampaignWithInvalidDate);
					if (cdDto.DateEnd.GetStringOrEmptyAsDate() > cdDto.DateOrder.GetStringOrEmptyAsDate())
						ConflictStorage.AddNotImportedDto(cdDto, ConflictMessages.CampaignWithInvalidDate);
					if (cdDto.DateStart.GetStringOrEmptyAsDate() < new DateTime(dto.YearStart.To(0), 1, 1))
						ConflictStorage.AddNotImportedDto(cdDto, ConflictMessages.CampaignWithInvalidDate);
					if (cdDto.DateOrder.GetStringOrEmptyAsDate() > new DateTime(dto.YearEnd.To(0), 12, 31))
						ConflictStorage.AddNotImportedDto(cdDto, ConflictMessages.CampaignWithInvalidDate);

					// не лишние ли этапы
                    // Не знаю, насколько это правильно - нормального ответа от аналитиков НЕ ПОЛУЧЕНО - Сергей Герасимов
                    // Посему - подмена источника финансирования "Квота" ни бюджет, иначе будет беда дальше.
                    if (cdDto.EducationSourceID.To(0) == 20) cdDto.EducationSourceID = "14"; // В датах нет отдельных дат на квоты - поэтому заменяем их на бюджет
					var allowedDateStages =
						allowedDates.Where(x => x.Course == cdDto.Course.To(0).ToString() && x.EducationFormID == cdDto.EducationFormID.To(0).ToString()
												&& x.EducationLevelID == cdDto.EducationLevelID.To(0).ToString()
                                                && x.EducationSourceID == cdDto.EducationSourceID.To(0).ToString()).Select(x => x.Stage).ToArray();

					if (allowedDateStages.Length == 0)
                        ConflictStorage.AddNotImportedDto(cdDto, ConflictMessages.CampaignWithInvalidData);
					if (!allowedDateStages.Any(x => x == cdDto.Stage))
						ConflictStorage.AddNotImportedDto(cdDto, ConflictMessages.CampaignDateWithInvalidStage);

					//если есть ошибка - всю кампанию в неё
					if (ConflictStorage.HasConflictOrNotImported(cdDto))
						ConflictStorage.AddNotImportedDto(dto, ConflictMessages.CampaignWithInvalidData);
					else
						processedDates.Add(cdDto);
				}

				bool isFailedStages = false;
				//проверяем даты на дубли и недостающие этапы
				var groupedDates = processedDates.GroupBy(x => x.Course.To(0).ToString() 
					+ "@" + x.EducationFormID.To(0).ToString() 
					+ "@" + x.EducationLevelID.To(0).ToString() 
					+ "@" + (x.EducationSourceID == "20" ? "14" : x.EducationSourceID.To(0).ToString())).ToArray();
				foreach (var groupedDatesItem in groupedDates)
				{
					var key = groupedDatesItem.Key;
					var stages = allowedDates.Where(x => x.Course + "@" + x.EducationFormID + "@" + x.EducationLevelID + "@" + x.EducationSourceID == key)
						.Select(x => x.Stage).ToArray();
					//есть дубли
					if (groupedDatesItem.Select(x => x.Stage).Distinct().Count() != groupedDatesItem.Select(x => x.Stage).Count())
					{
						foreach (var dateDto in groupedDatesItem)
						{
							ConflictStorage.AddNotImportedDto(dateDto, ConflictMessages.CampaignDateDuplicateData);
							isFailedStages = true;
						}
					}
					//не хватает обязательных этапов
					else if (!groupedDatesItem.Select(x => x.Stage).OrderBy(x => x).ToArray().SequenceEqual(stages))
					{
						foreach (var dateDto in groupedDatesItem)
						{
							ConflictStorage.AddNotImportedDto(dateDto, ConflictMessages.CampaignDateWithMissingStage);
							isFailedStages = true;
						}
					}
				}

				//неправильные этапы - всю кампанию в ошибку
				if (isFailedStages)
					ConflictStorage.AddNotImportedDto(dto, ConflictMessages.CampaignWithInvalidData);
			}
			//проверка комбинаций уровней образования
			{
				//•	Прием на СПО возможен только для 1 курса (отключено)
			//•	Прием на бакалавриат (полн.) может проходить на 4 курса
			//•	Прием на бакалавриат (сокращ.) возможен только на 1 курс (теперь на 3)
			//•	Прием на специалитет может проходить на 6 курсов
			//•	Прием в магистратуру возможен только на 1 курс
            //•	Прием кадров высшей квалификации возможен только на 1 курс
                var edLevels = dto.EducationLevels.Where(x => /*(x.EducationLevelID.To(0) == 17 && x.Course.To(0) > 1)
				                                              || */(x.EducationLevelID.To(0) == 2 && x.Course.To(0) > 4)
				                                              || (x.EducationLevelID.To(0) == 3 && x.Course.To(0) > 3)
				                                              || (x.EducationLevelID.To(0) == 4 && x.Course.To(0) > 1)
				                                              || (x.EducationLevelID.To(0) == 5 && x.Course.To(0) > 6)
                                                              || (x.EducationLevelID.To(0) == 18 && x.Course.To(0) > 1)).ToArray();
				if (edLevels.Length > 0)
				{
					ConflictStorage.AddConflictWithCustomMessage(dto, 
						new ConflictStorage.ConflictMessage
						{
							Code = ConflictMessages.CampaignWithInvalidEducationLevelsCombination,
							Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.CampaignWithInvalidEducationLevelsCombination),
								String.Join("; ", edLevels.Select(x => x.Course + " курс, " + DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, x.EducationLevelID.To(0))))),
						});
				}
    		}
		}
		#endregion

        #region Проверки целостности для списков рекомендованных
        private bool CheckStage(RecommendedListDto recList)
        {
            return (recList.Stage == 1 || recList.Stage == 2);
        }


        private class ApplicationInRecLIstCheckResult
        {
            public int Result;
            public string ApplicationNumber;
        }

        private short CheckApplicationsForRecList(RecommendedListDto recList, out List<ApplicationInRecLIstCheckResult> apps)
        {
            recList.UID = Guid.NewGuid().ToString();
            apps = new List<ApplicationInRecLIstCheckResult>();

            using (ImportEntities dbContext = new ImportEntities())
            {
                foreach (var item in recList.RecLists)
                {
                    var Application = dbContext.Application.FirstOrDefault(x => x.ApplicationNumber == item.Application.ApplicationNumber && x.RegistrationDate == item.Application.RegistrationDate);

                    // Проверим наличие и статус заявления
                    if (Application == null || (Application.StatusID != (int)ApplicationStatusType.Accepted && Application.StatusID != (int)ApplicationStatusType.InOrder))
                    {
                        apps.Add(new ApplicationInRecLIstCheckResult() { Result = -1, ApplicationNumber = item.Application.ApplicationNumber });
                        continue;
                    }

                    // Проверим наличие в заявлении указанного уровня образования, формы обучения, конкурсной группы, направления и источника финансирования
                    foreach (var listItem in item.FinSourceAndEduForms)
                    {
                        var existingItem =  Application.ApplicationCompetitiveGroupItem.FirstOrDefault(x => x.CompetitiveGroup.UID == listItem.CompetitiveGroupID && listItem.EducationFormID == x.EducationFormId && x.EducationSourceId == EDSourceConst.Budget);// && x.Priority != 0);

                        if (existingItem == null)
                        {
                            apps.Add(new ApplicationInRecLIstCheckResult() { Result = -2, ApplicationNumber = item.Application.ApplicationNumber });
                            continue;
                        }

                        var competitiveGroup = dbContext.CompetitiveGroup.First(x => x.CompetitiveGroupID == existingItem.CompetitiveGroupId);

                        if (!competitiveGroup.CompetitiveGroupItem.Any(x => x.EducationLevelID == listItem.EducationLevelID && x.DirectionID == listItem.DirectionID))
                        {
                            apps.Add(new ApplicationInRecLIstCheckResult() { Result = -3, ApplicationNumber = item.Application.ApplicationNumber });
                            continue;
                        }

                        ObjectParameter rating = new ObjectParameter("rating", typeof(decimal));
                        dbContext.CalculateAppRagting(Application.ApplicationID, competitiveGroup.CompetitiveGroupID, rating);

                        if (rating.Value == DBNull.Value)
                        {
                            apps.Add(new ApplicationInRecLIstCheckResult() { Result = -4, ApplicationNumber = item.Application.ApplicationNumber });
                            continue;
                        }
                    }
                }
            }
            return 0;
        }

        public void CheckRecommendedList(RecommendedListDto list)
        {
            if (!CheckStage(list))
            {
                ConflictStorage.AddNotImportedDto(list, ConflictMessages.IncorrectStageInRecommendedList, new string[1] { list.Stage.ToString() });
                return;
            }

            List<ApplicationInRecLIstCheckResult> checkResult;

            CheckApplicationsForRecList(list, out checkResult);
            if (checkResult.Count > 0)
            {
                StringBuilder messageBuilder = new StringBuilder();

                foreach (var item in checkResult)
                {
                    switch (item.Result)
                    {
                        case -1: // Заявление вообще не найдено
                            messageBuilder.AppendFormat(ConflictMessages.GetMessage(ConflictMessages.IncorrectApplicationForRecommendedList), item.ApplicationNumber);
                            break;

                        case -2: // Указанная КГ с указанной формой обучения и источником финансирования - бюджет не входит в список выбранных абитуриентом 
                            messageBuilder.AppendFormat(ConflictMessages.GetMessage(ConflictMessages.IncorrectGroupFormSource), item.ApplicationNumber);
                            break;

                        case -3: // Указанная КГ не содержит указанного направления подготовки для указанного уровня образования
                            messageBuilder.AppendFormat(ConflictMessages.GetMessage(ConflictMessages.IncorrectLevelDirection), item.ApplicationNumber);
                            break;

                        case -4:
                            messageBuilder.AppendFormat(ConflictMessages.GetMessage(ConflictMessages.IncorrectRating), item.ApplicationNumber);
                            break;
                    }
                }
                ConflictStorage.AddConflictWithCustomMessage(list, new ConflictStorage.ConflictMessage() { Message = messageBuilder.ToString() });
            }
        }
        #endregion
    }
}
