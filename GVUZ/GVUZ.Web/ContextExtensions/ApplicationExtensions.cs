using System;
using System.Data.Entity;
using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Web.ViewModels;
using GVUZ.Model.Helpers;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с заявлениями
	/// </summary>
	public static class ApplicationExtensions
	{
		/// <summary>
		/// Возращаем модель для печати документов
		/// </summary>
		public static PrintTemplateInfoViewModel GetPrintTemplateInfoModel(this EntrantsEntities dbContext, EntrantKey key)
		{
			var application =
				dbContext.Application
				         .Include(x => x.Institution)
				         .Include(x => x.CompetitiveGroup).FirstOrDefault(x => x.ApplicationID == key.ApplicationID);
            //.Include(x => x.ApplicationSelectedCompetitiveGroup)
            //.Include(x => x.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup)).FirstOrDefault(x => x.ApplicationID == key.ApplicationID);

            if (application == null)
				return new PrintTemplateInfoViewModel();

			var model = new PrintTemplateInfoViewModel
			            	{
			            		ApplicationID = application.ApplicationID,
			            		ApplicationNumber = application.ApplicationNumber,
			            		ApplicationRegistrationDateTime = application.RegistrationDate,
			            		EntrantID = application.EntrantID,
			            		EntrantFistName = application.Entrant.FirstName,
			            		EntrantLastName = application.Entrant.LastName,
			            		EntrantMiddleName = application.Entrant.MiddleName,
								CompetitiveGroupName = application.StatusID == ApplicationStatusType.InOrder ? application.CompetitiveGroup.Name
									: null,
                                    //String.Join(", ", application.ApplicationSelectedCompetitiveGroup.Select(x => x.CompetitiveGroup.Name)),
			            		InstitutionFullName = application.Institution.FullName,
			            		FormList = dbContext.GetEducationalFormList(application),
			            		OriginalDocumentsReceived = application.OriginalDocumentsReceived
			            	};

			SetOnlySchoolCertificateDocument(dbContext, model);

			if (model.OriginalDocumentsReceived)
				SetAllDocuments(dbContext, model);					
			dbContext.AddApplicationAccessToLog(application, "PrintTemplate");
			return model;
		}		

		/// <summary>
		/// Проставляем правильный школьный документ, по приоритету (в документах один требуется)
		/// </summary>
		private static void SetOnlySchoolCertificateDocument(EntrantsEntities dbContext, PrintTemplateInfoViewModel model)
		{
			var documents =
				dbContext.ApplicationEntrantDocument.Where(
					x =>
						(x.EntrantDocument.DocumentTypeID == 3
							// сначала аттестат, если его нет, то далее по порядку: диплом НПО, СПО и ВПО
							|| x.EntrantDocument.DocumentTypeID == 6
							|| x.EntrantDocument.DocumentTypeID == 5
							|| x.EntrantDocument.DocumentTypeID == 4
							|| x.EntrantDocument.DocumentTypeID == 16
							|| x.EntrantDocument.DocumentTypeID == 7
							|| x.EntrantDocument.DocumentTypeID == 8)
							&& x.ApplicationID == model.ApplicationID)
					.OrderBy(x => x.EntrantDocument.DocumentTypeID)
					.ThenByDescending(x => x.EntrantDocument.DocumentDate)
					.Select(x => new { x.EntrantDocumentID, x.EntrantDocument.DocumentTypeID })
					.ToArray();
			if (documents.Length == 0) return;
			int documentId = (documents.FirstOrDefault(x => x.DocumentTypeID == 3) ??
				documents.FirstOrDefault(x => x.DocumentTypeID == 6) ??
				documents.FirstOrDefault(x => x.DocumentTypeID == 5) ??
				documents.FirstOrDefault(x => x.DocumentTypeID == 4) ??
				documents.First()).EntrantDocumentID;

			if (documentId == 0) return;
			
			var document = dbContext.LoadEntrantDocument(documentId);
			
			if (document == null) return;
			
			model.SchoolCertificateDocument = new PrintTemplateInfoViewModel.DocumentData
			                                  	{
			                                  		TypeName = document.DocumentTypeName,
			                                  		Series = document.DocumentSeries,
			                                  		Number = document.DocumentNumber
			                                  	};
		}

		/// <summary>
		/// Загружаем все документы
		/// </summary>
		private static void SetAllDocuments(EntrantsEntities dbContext, PrintTemplateInfoViewModel model)
		{			
			var documents = dbContext.LoadApplicationDocuments(model.ApplicationID);			

			model.AttachedDocuments = documents.Select(x => new PrintTemplateInfoViewModel.DocumentData
			                                                	{
			                                                		TypeName = x.DocumentTypeName,
			                                                		Series = x.DocumentSeries,
			                                                		Number = x.DocumentNumber
			                                                	}).ToArray();			
		}

		/// <summary>
		/// Загружаем заявление
		/// </summary>
		public static Application GetApplication(int appID)
		{
			using (var dbContext = new EntrantsEntities())
			{
				return dbContext.Application.SingleOrDefault(x => x.ApplicationID == appID);
			}
		}

		/// <summary>
		/// Проверка на уникальность номера заявления
		/// </summary>
		internal static bool CheckApplicationNumberIsUnique(this EntrantsEntities dbContext, Application app, string applicationNumber)
		{
            return !dbContext.Application.Any(x => x.InstitutionID == app.InstitutionID
													&& x.ApplicationNumber == applicationNumber
													&& x.ApplicationID != app.ApplicationID
													&& x.ApplicationNumber != null);
		}

		/// <summary>
		/// Проверка на уникальность номера заявления
		/// </summary>
		internal static bool CheckApplicationNumberIsUnique(this EntrantsEntities dbContext, int institutionID, string applicationNumber)
		{
			return !dbContext.Application.Any(x => x.InstitutionID == institutionID
													&& x.ApplicationNumber == applicationNumber
													&& x.ApplicationNumber != null);
		}

        /// <summary>
        /// Возвращает модель для печати справки о результатах ЕГЭ
        /// </summary>
        public static PrintExaminationResultReferenceViewModel GetPrintExaminationReferenceViewModel(this EntrantsEntities dbContext, EntrantKey key)
        {
            var app = dbContext.Application
                .Include(x => x.Entrant)
                .Include(x => x.Entrant.EntrantDocument_Identity)
                .Include(x => x.Institution)
                .Include(x => x.ApplicationEntranceTestDocument)
                .Single(x => x.ApplicationID == key.ApplicationID);

            var entrant = app.Entrant;
            
            var model = new PrintExaminationResultReferenceViewModel
                {
                    EntrantLastName = entrant.LastName,
                    EntrantFistName = entrant.FirstName,
                    EntrantMiddleName = entrant.MiddleName,
                    DocumentNumber =
                        entrant.EntrantDocument_Identity == null
                            ? null
                            : entrant.EntrantDocument_Identity.DocumentNumber,
                    DocumentSeries =
                        entrant.EntrantDocument_Identity == null
                            ? null
                            : entrant.EntrantDocument_Identity.DocumentSeries,
                    InstitutionFullName = app.Institution == null ? null : app.Institution.FullName,
                    Marks = app.ApplicationEntranceTestDocument.Select(x => new PrintExaminationResultCertificateMark
                        {
                            Mark = Convert.ToInt32(Math.Floor(x.ResultValue.GetValueOrDefault())),
                            SubjectName = x.Subject == null ? null : x.Subject.Name,
                            Year = x.EntrantDocument == null ? 0 : x.EntrantDocument.DocumentDate.GetValueOrDefault().Year,
                            Status = x.EntrantDocument == null ? null : x.EntrantDocument.DocumentDate.GetValueOrDefault().Year == 2011 ? "Истек срок" : "Действующий",
                            CertificateNumber = x.EntrantDocument == null ? null : x.EntrantDocument.DocumentNumber
                            
                        }).ToArray()

                };

            return model;
            //return new PrintExaminationResultReferenceViewModel
            //    {
            //        DocumentNumber = "567890",
            //        DocumentSeries = "1234",
            //        EntrantFistName = "Иван",
            //        EntrantMiddleName = "Иванович",
            //        EntrantLastName = "Иванов",
            //        InstitutionFullName = "Полное наименование образовательной организации",
            //        Marks = new[]
            //            {
            //                new PrintExaminationResultCertificateMark
            //                    {
            //                        CertificateNumber = "000001",
            //                        Mark = 99,
            //                        Status = "Действующий",
            //                        SubjectName = "Математика",
            //                        Year = 2014
            //                    },
            //                new PrintExaminationResultCertificateMark
            //                    {
            //                        CertificateNumber = "000001",
            //                        Mark = 75,
            //                        Status = "Действующий",
            //                        SubjectName = "Русский язык",
            //                        Year = 2014
            //                    },
            //                new PrintExaminationResultCertificateMark
            //                    {
            //                        CertificateNumber = "000001",
            //                        Mark = 81,
            //                        Status = "Действующий",
            //                        SubjectName = "Литература",
            //                        Year = 2014
            //                    },
            //                new PrintExaminationResultCertificateMark
            //                    {
            //                        CertificateNumber = "000001",
            //                        Mark = 42,
            //                        Status = "Действующий",
            //                        SubjectName = "Физика",
            //                        Year = 2014
            //                    }
            //            }
            //    };
        }

	    public const string RussiaDigitCode = "643";

	    public static bool IsForeignCitizen(this Application app)
	    {
            if (app != null && app.Entrant != null && app.Entrant.EntrantDocument_Identity != null && app.Entrant.EntrantDocument_Identity.EntrantDocumentIdentity != null && app.Entrant.EntrantDocument_Identity.EntrantDocumentIdentity.CountryType != null)
            {
                return app.Entrant.EntrantDocument_Identity.EntrantDocumentIdentity.CountryType.DigitCode != "643";
            }

	        return false;
	    }
	}
}