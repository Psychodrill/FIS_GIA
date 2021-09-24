using System;
using System.Globalization;
using System.Linq;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;
using GVUZ.ServiceModel.Import.Core;
using GVUZ.ServiceModel.Import.Core.Operations;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Operations.Importing;
using GVUZ.ServiceModel.Import.WebService;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections;

namespace GVUZ.ServiceModel.Import
{
	public static class DbDtoComparerExtensions
	{
		/// <summary>
		/// Поменялась ли общая льгота для заявления.
		/// </summary>
		public static int IsChanged(this ObjectImporter objectImporter, ApplicationCommonBenefitDto appCommonBenefitDto,
			Application applicationDb, out ApplicationEntranceTestDocument applicationEntranceTestDocument)
		{			
			var commonDocs = objectImporter.GetApplicationCommonBenefits(applicationDb);
			applicationEntranceTestDocument =
				commonDocs.Where(x => x.CompetitiveGroup.UID == appCommonBenefitDto.CompetitiveGroupID).FirstOrDefault();
			if (appCommonBenefitDto == null && applicationEntranceTestDocument == null) return 0;

			if ((appCommonBenefitDto == null && applicationEntranceTestDocument != null) ||
				(appCommonBenefitDto != null && applicationEntranceTestDocument == null))
				return ConflictMessages.EntranceTestResultChanged;

			if (applicationEntranceTestDocument.UID != appCommonBenefitDto.UID) 
				return ConflictMessages.EntranceTestResultChanged;
			if (applicationEntranceTestDocument != null && appCommonBenefitDto.CompetitiveGroupID != applicationEntranceTestDocument.CompetitiveGroup.UID)
				return ConflictMessages.EntranceTestResultChanged;
			if (applicationEntranceTestDocument.BenefitID != appCommonBenefitDto.BenefitKindID.To(0))
				return ConflictMessages.EntranceTestResultChanged;

			return objectImporter.IsEntrantDocumentChanged(applicationEntranceTestDocument, appCommonBenefitDto) ?
				ConflictMessages.ApplicationCommonBenefitDocumentChanged : 0;
		}

		/// <summary>
		/// Сравнение существующего РВИ в заявлении и импортируемого.
		/// </summary>
		public static int IsChanged(this ObjectImporter objectImporter, EntranceTestAppItemDto appEntranceTestDto,
			ApplicationEntranceTestDocument appEntranceTest, ApplicationDocumentsDto appDocsDto)
		{
            if (appEntranceTestDto.UID != appEntranceTest.UID) return ConflictMessages.EntranceTestResultChanged;

			// сравнение предмета
			int subjectIDDb = appEntranceTest.SubjectID.HasValue ? appEntranceTest.SubjectID.Value : -1;
			int subjectIDDto = appEntranceTestDto.EntranceTestSubject != null &&
				!String.IsNullOrEmpty(appEntranceTestDto.EntranceTestSubject.SubjectID) ? 
				appEntranceTestDto.EntranceTestSubject.SubjectID.To(0) : -1;
			if (subjectIDDb != subjectIDDto) return ConflictMessages.EntranceTestResultChanged;

			// сравнение источника РВИ
			int sourceIDDb = appEntranceTest.SourceID.HasValue ? appEntranceTest.SourceID.Value : -1;
			int sourceIDDto = String.IsNullOrEmpty(appEntranceTestDto.ResultSourceTypeID) ? -1 : appEntranceTestDto.ResultSourceTypeID.To(0);
			if (sourceIDDb != sourceIDDto) return ConflictMessages.EntranceTestResultChanged;

			// сравнение результа ВИ
			decimal resultValueDb = appEntranceTest.ResultValue.HasValue ? appEntranceTest.ResultValue.Value : -1;
			decimal resultValueDto = (String.IsNullOrEmpty(appEntranceTestDto.ResultValue) ? -1 : appEntranceTestDto.ResultValue.To<decimal>(provider: CultureInfo.InvariantCulture));
			if (resultValueDb != resultValueDto) return ConflictMessages.EntranceTestResultChanged;

			EntrantDocument currentEntrantDocument = objectImporter.DbObjectRepository
                .EntrantDocuments.SingleOrDefault(x => x.EntrantDocumentID == appEntranceTest.EntrantDocumentID);
			// в зависимости от источника делаем ветвление: общая льгота для поступления, экзамен в ОУ, ЕГЭ, димплом победителя Олимпиады.
			switch ((EntranceTestResultSourceEnum)(appEntranceTest.SourceID ?? -1))
			{
				case EntranceTestResultSourceEnum.InstitutionEntranceTest:
					// документы проверять не надо
					return 0;
				case EntranceTestResultSourceEnum.EgeDocument:
					//проверка на отсутствие документа
					if (appEntranceTestDto.ResultDocument == null)
						return currentEntrantDocument != null ? ConflictMessages.EntranceTestResultEgeDocumentChanged : 0;

					if (appDocsDto.EgeDocuments == null) //не приложены документы ЕГЭ, но они могут потом подтянуться из проверки. Так что тут всё в порфдке
						return 0;
					
					//по идее некорректная ситуация, и данная ошибка не должна нигде возникнуть, но в любом случае это в целостности должно быть проверено
					//а тут падать не должно
					EgeDocumentWithSubjectsDto egeDocumentDto = appDocsDto.EgeDocuments.SingleOrDefault(x => x.UID == appEntranceTestDto.ResultDocument.EgeDocumentID);
					if (egeDocumentDto == null && currentEntrantDocument != null)
						return ConflictMessages.EntranceTestResultEgeDocumentChanged;
					
				// сравниваем документ ЕГЭ из dto и существующий документ в БД
					EntrantDocument newEgeDocumentDb = Mapper.Map(egeDocumentDto, new EntrantDocument());
					return IsChanged(newEgeDocumentDb, currentEntrantDocument) ? 
						ConflictMessages.EntranceTestResultEgeDocumentChanged : 0;
				case EntranceTestResultSourceEnum.GiaDocument:
					//проверка на отсутствие документа
					if (appEntranceTestDto.ResultDocument == null)
						return currentEntrantDocument != null ? ConflictMessages.EntranceTestResultEgeDocumentChanged : 0;

					GiaDocumentWithSubjectsDto giaDocumentDto = appDocsDto.GiaDocuments.SingleOrDefault(x => x.UID == appEntranceTestDto.ResultDocument.EgeDocumentID);
					
					//по идее некорректная ситуация, и данная ошибка не должна нигде возникнуть, но в любом случае это в целостности должно быть проверено
					//а тут падать не должно
					if (giaDocumentDto == null && currentEntrantDocument != null)
						return ConflictMessages.EntranceTestResultEgeDocumentChanged;
					
				// сравниваем справку ГИА из dto и существующий документ в БД
					EntrantDocument newGiaDocumentDb = Mapper.Map(giaDocumentDto, new EntrantDocument());
					return IsChanged(newGiaDocumentDb, currentEntrantDocument) ? ConflictMessages.EntranceTestResultEgeDocumentChanged : 0;
				case EntranceTestResultSourceEnum.OlympicDocument:
					// сравниваем документ олимпиады из dto и существующий документ в БД
					EntrantDocument newOlympicDocumentDb = new DbDocumentInsertManager().CreateOlympicDocument(appEntranceTestDto.ResultDocument, null);
					return IsChanged(newOlympicDocumentDb, currentEntrantDocument)
					       	? ConflictMessages.EntranceTestResultOlympicDocumentChanged : 0;
				default:
					throw new ImportException(
						String.Format("Некорректный ID {0} основания для оценки для РВИ с UID {1}", appEntranceTest.SourceID, appEntranceTest.UID));
			}
		}

		/// <summary>
		/// Изменился ли документ-основание для общей льготы
		/// </summary>
		public static bool IsEntrantDocumentChanged(this ObjectImporter objectImporter,
			ApplicationEntranceTestDocument appEntranceTestDocument, ApplicationCommonBenefitDto applicationCommonBenefitDto)
		{			
			EntrantDocument documentDb = objectImporter.DbObjectRepository.EntrantDocuments.SingleOrDefault(x => x.EntrantDocumentID == appEntranceTestDocument.EntrantDocumentID);

			ApplicationCommonBenefitDocumentsDto docContainerDto = applicationCommonBenefitDto.DocumentReason;

			// нет документа основания
			if (documentDb == null && docContainerDto == null) return false;			
			if (documentDb != null && docContainerDto == null) return true;

			EntrantDocument newDocumentDb = new DbDocumentInsertManager().CreateCommonBenefitDocument(docContainerDto, null);
			return IsChanged(documentDb, newDocumentDb);
		}

		public static bool IsChanged(EntrantDocument entrantDoc1, EntrantDocument entrantDoc2)
		{
			if (entrantDoc1 == null && entrantDoc2 == null) return false;
			if ((entrantDoc1 == null && entrantDoc2 != null) || (entrantDoc1 != null && entrantDoc2 == null)) return true;

			if (entrantDoc1.UID != entrantDoc2.UID) return true;
			if (entrantDoc1.DocumentDate != entrantDoc2.DocumentDate) return true;
			if (entrantDoc1.DocumentNumber != null || entrantDoc2.DocumentNumber != null)
			{
				if (entrantDoc1.DocumentNumber == null || entrantDoc2.DocumentNumber == null ||
					entrantDoc1.DocumentNumber.ToLower() != entrantDoc2.DocumentNumber.ToLower()) return true;
			}

			if (entrantDoc1.DocumentSeries != null || entrantDoc2.DocumentSeries != null)
			{
				if (entrantDoc1.DocumentSeries == null || entrantDoc2.DocumentSeries == null ||
					entrantDoc1.DocumentSeries.ToLower() != entrantDoc2.DocumentSeries.ToLower()) return true;
			}

			if (entrantDoc1.DocumentOrganization != null || entrantDoc2.DocumentOrganization != null)
			{
				if (entrantDoc1.DocumentOrganization == null || entrantDoc2.DocumentOrganization == null ||
					entrantDoc1.DocumentOrganization.ToLower() != entrantDoc2.DocumentOrganization.ToLower()) return true;
			}

			return false;
		}
	}
}
