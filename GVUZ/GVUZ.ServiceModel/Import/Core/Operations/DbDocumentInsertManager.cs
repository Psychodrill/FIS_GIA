using System;
using System.Web.Script.Serialization;
using System.Linq;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections;

namespace GVUZ.ServiceModel.Import.Core.Operations
{
	/// <summary>
	/// Вставка документов абитуриента
	/// </summary>
	public class DbDocumentInsertManager
	{
		private readonly ConflictStorage _conflictStorage;
		private readonly Entrant _entrant;
		private readonly Application _application;
		private EntrantDocument _entrantDocIdentity;

		/// <summary>
		/// Используется для создания документов из dto в объектную модель документа.
		/// </summary>
		public DbDocumentInsertManager()
		{
		}

		/// <summary>
		/// Используется для создания документов из dto в объекты модели с привязкой к абитуриенту и 
		/// вставкой в модель.
		/// </summary>
		public DbDocumentInsertManager(ConflictStorage conflictStorage, Application app)
		{
			if (app == null) throw new ArgumentNullException("app");
			if (app.Entrant == null) throw new ArgumentException("В заявлении должна быть ссылка на абитуриента - свойство Entrant", "app");

			_application = app;
			_entrant = app.Entrant;
			_conflictStorage = conflictStorage;
		}

		/// <summary>
		/// Создаём олимпийский документ
		/// </summary>
		public EntrantDocument CreateOlympicDocument(EntranceTestResultDocumentsDto docContainerDto, ImportEntities dbContext)
		{			
			if (docContainerDto.OlympicTotalDocument != null)
				return AddToContextEntrantDocumentFromDto(docContainerDto.OlympicTotalDocument, EntrantDocumentType.OlympicTotalDocument, dbContext);
			
			if (docContainerDto.OlympicDocument != null)
				return AddToContextEntrantDocumentFromDto(docContainerDto.OlympicDocument, EntrantDocumentType.OlympicDocument, dbContext);

			return null;
		}

		/// <summary>
		/// Создаём документ общей льготы
		/// </summary>
		public EntrantDocument CreateCommonBenefitDocument(ApplicationCommonBenefitDocumentsDto docContainerDto, ImportEntities dbContext)
		{
			if (docContainerDto.OlympicDocument != null)
				return AddToContextEntrantDocumentFromDto(docContainerDto.OlympicDocument, EntrantDocumentType.OlympicDocument, dbContext);
			if (docContainerDto.OlympicTotalDocument != null)
				return AddToContextEntrantDocumentFromDto(docContainerDto.OlympicTotalDocument, EntrantDocumentType.OlympicTotalDocument, dbContext);

			if (docContainerDto.CustomDocument != null)
				return AddToContextEntrantDocumentFromDto(docContainerDto.CustomDocument, EntrantDocumentType.CustomDocument, dbContext);
			MedicalDocumentsDto medicalDocumentsDto = docContainerDto.MedicalDocuments;
			if (medicalDocumentsDto != null)
			{			
    			if (medicalDocumentsDto.BenefitDocument != null)
				{
					if (medicalDocumentsDto.BenefitDocument.DisabilityDocument != null)
						return AddToContextEntrantDocumentFromDto(medicalDocumentsDto.BenefitDocument.DisabilityDocument, EntrantDocumentType.DisabilityDocument, dbContext);
					if (medicalDocumentsDto.BenefitDocument.MedicalDocument != null)
						return AddToContextEntrantDocumentFromDto(medicalDocumentsDto.BenefitDocument.MedicalDocument, EntrantDocumentType.MedicalDocument, dbContext);
				}

                if (medicalDocumentsDto.AllowEducationDocument != null)
                    return AddToContextEntrantDocumentFromDto(medicalDocumentsDto.AllowEducationDocument, EntrantDocumentType.AllowEducationDocument, dbContext);
			}

			return null;
		}

		/// <summary>
		/// Добавляем документ в датабазный контекст
		/// </summary>
		public EntrantDocument AddToContextEntrantDocumentFromDto<T>(T documentDto, EntrantDocumentType documentTypeID, ImportEntities dbContext) where T : BaseDocumentDto
		{
			if (documentDto == null) return null;
			if (_conflictStorage != null && 
				_conflictStorage.HasConflictOrNotImported(documentDto)) return null;

			EntrantDocument entrantDocumentDb;
			if (dbContext != null)
			{
				if (documentDto.UID != null)
					entrantDocumentDb = dbContext.EntrantDocument.FirstOrDefault(x => x.UID == documentDto.UID && x.EntrantID == _entrant.EntrantID);
				else entrantDocumentDb = null;

				if (entrantDocumentDb == null)
				{
					entrantDocumentDb = new EntrantDocument();
					dbContext.EntrantDocument.AddObject(entrantDocumentDb);
					_entrant.EntrantDocuments.Add(entrantDocumentDb);
				}
			}
			else
			{
				entrantDocumentDb = new EntrantDocument();
			}

			var appEntrantDocumentDb = new ApplicationEntrantDocument();
			
			entrantDocumentDb = Mapper.Map(documentDto, entrantDocumentDb);
			entrantDocumentDb.DocumentTypeID = (int)documentTypeID;
			// мапим dto объект во ViewModel документа для сохранения специфических данных
			var dtoMap = Mapper.Map(documentDto, EntrantDocumentExtensions.InstantiateDocumentByType((int)documentTypeID),
			                     typeof(T), EntrantDocumentExtensions.GetDocumentViewModelType((int)documentTypeID));
			entrantDocumentDb.DocumentSpecificData = new JavaScriptSerializer().Serialize(dtoMap);

			if (dbContext != null)
			{
				if (documentTypeID == EntrantDocumentType.IdentityDocument)
					_entrantDocIdentity = entrantDocumentDb;

				dbContext.ApplicationEntrantDocument.AddObject(appEntrantDocumentDb);
				appEntrantDocumentDb.EntrantDocument = entrantDocumentDb;
				appEntrantDocumentDb.Application = _application;

                if (documentTypeID != EntrantDocumentType.IdentityDocument)
				    appEntrantDocumentDb.OriginalReceivedDate = !documentDto.OriginalReceived.To(false)
				                                            	? (DateTime?)null
				                                            	: (documentDto.OriginalReceivedDate.GetStringOrEmptyAsDate() ?? DateTime.Now);
			}

			return entrantDocumentDb;
		}

		/// <summary>
		/// Добавляем в контекст образовательные документы
		/// </summary>
		public void AddToContextEduDocumentsDto(EduDocumentsDto eduDocumentsDto, ImportEntities dbContext)
		{
			if (eduDocumentsDto == null) return;
			if (eduDocumentsDto.AcademicDiplomaDocument != null)
				AddToContextEntrantDocumentFromDto(eduDocumentsDto.AcademicDiplomaDocument, EntrantDocumentType.AcademicDiplomaDocument, dbContext);
			if (eduDocumentsDto.BasicDiplomaDocument != null)
				AddToContextEntrantDocumentFromDto(eduDocumentsDto.BasicDiplomaDocument, EntrantDocumentType.BasicDiplomaDocument, dbContext);
			if (eduDocumentsDto.HighEduDiplomaDocument != null)
				AddToContextEntrantDocumentFromDto(eduDocumentsDto.HighEduDiplomaDocument, EntrantDocumentType.HighEduDiplomaDocument, dbContext);
			if (eduDocumentsDto.IncomplHighEduDiplomaDocument != null)
				AddToContextEntrantDocumentFromDto(eduDocumentsDto.IncomplHighEduDiplomaDocument, EntrantDocumentType.IncomplHighEduDiplomaDocument, dbContext);
			if (eduDocumentsDto.MiddleEduDiplomaDocument != null)
				AddToContextEntrantDocumentFromDto(eduDocumentsDto.MiddleEduDiplomaDocument, EntrantDocumentType.MiddleEduDiplomaDocument, dbContext);
			if (eduDocumentsDto.SchoolCertificateDocument != null)
				AddToContextEntrantDocumentFromDto(eduDocumentsDto.SchoolCertificateDocument, EntrantDocumentType.SchoolCertificateDocument, dbContext);
			if (eduDocumentsDto.SchoolCertificateBasicDocument != null)
				AddToContextEntrantDocumentFromDto(eduDocumentsDto.SchoolCertificateBasicDocument, EntrantDocumentType.SchoolCertificateBasicDocument, dbContext);
            if (eduDocumentsDto.EduCustomDocument != null)
                AddToContextEntrantDocumentFromDto(eduDocumentsDto.EduCustomDocument, EntrantDocumentType.EduCustomDocument, dbContext);
		}

		/// <summary>
		/// Проставляем ДУЛ для абитуриента
		/// </summary>
		public void SetIdentityDocumentForEntrant()
		{
			if (_entrantDocIdentity != null)
				_entrant.EntrantDocument_Identity = _entrantDocIdentity;
		}

		/// <summary>
		/// Обновляем внутренние данные документов (запись во внешнюю таблицу, инициализация внутренних структур
		/// </summary>
		/// <param name="dbContext"></param>
		public void UpdateInternalDocumentData(ImportEntities dbContext)
		{
			var docIDs = dbContext.EntrantDocument.Where(x => x.EntrantID == _entrant.EntrantID).Select(x => x.EntrantDocumentID).ToArray();
			using (var eContext = new EntrantsEntities())
			foreach (int docID in docIDs)
			{
				var baseDocumentViewModel = eContext.LoadEntrantDocument(docID, false);
				if (baseDocumentViewModel != null)
				{
					baseDocumentViewModel.FillDataImportLoadSave(eContext);
					baseDocumentViewModel.PrepareForSave(eContext);
					baseDocumentViewModel.SaveToAdditionalTable(eContext);
					//сами документы не сохраняем, у них уже корректные данные
				}
			}
		}
	}
}
