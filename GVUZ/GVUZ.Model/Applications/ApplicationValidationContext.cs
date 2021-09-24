using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.Documents;
using Application = GVUZ.Model.Entrants.Application;

namespace GVUZ.Model.Applications
{
	/// <summary>
	/// Модель для проверки заявлений
	/// </summary>
	internal class ApplicationValidationContext
	{
		public ApplicationValidationContext(EntrantsEntities entities, Application application)
		{
			Entities = entities;
			Application = application;

			TestItemsWithoutDocument = GetTestItemsWithoutDocument();
			NoCertificateDocuments = GetNoCertificateDocuments();
			DocumentsWithCertificate = GetDocumentsWithCertificate();
			FullModels = new List<EGEDocumentViewModel>();
		}

		/// <summary>
		/// Есть хотя бы один РВИ
		/// </summary>
		public bool HasAnyEntrantTestDocuments
		{
			get
			{
				return !TestItemsWithoutDocument.IsNullOrEmpty() || !NoCertificateDocuments.IsNullOrEmpty() ||
				         !DocumentsWithCertificate.IsNullOrEmpty();
			}
		}

		/// <summary>
		/// Текущий контекст для запросов к БД.</summary>
		public EntrantsEntities Entities { get; private set; }

		/// <summary>
		/// Поданное заявление.</summary>
		public Application Application { get; private set; }

		/// <summary>
		/// Пополняемый список "полных" по баллам моделей (чтобы не плодить лишних). </summary>
		/// <remarks>"Полные" означает, что они соответствуют сертификатам ЕГЭ по количеству отметок (и самим отметкам).
		/// В будущем можно рассмотреть возможность сделать соответствие ВИ из конкурсных групп 
		/// (останавливает то, что один сертификат может исопльзоваться несколькими заявлениями, с разными конкурсными группами).</remarks>
		public List<EGEDocumentViewModel> FullModels { get; private set; }
		
		/// <summary>
		/// Результаты ВИ по конкурсной группе из заявления, но документы по ВИ не привязаны к переданному заявлению.
		/// </summary>
		public List<EntranceTestItemC> TestItemsWithoutDocument { get; private set; }
		
		/// <summary>
		/// Документы, для которых нет свидетельств в локальной базе (например, из других заявлений абитуриента).
		/// </summary>
		public List<ApplicationEntranceTestDocument> NoCertificateDocuments { get; private set; }
		
		/// <summary>
		/// Документы, для которых есть свидетельства в локальной базе (например, из других заявлений абитуриента).
		/// </summary>
		public List<ApplicationEntranceTestDocument> DocumentsWithCertificate { get; private set; }

		private List<ApplicationEntranceTestDocument> GetDocumentsWithCertificate()
		{
			return Entities.ApplicationEntranceTestDocument
				.Include(x => x.EntrantDocument)
				.Include(x => x.Subject)
				.Where(x => x.ApplicationID == Application.ApplicationID &&
							x.EntrantDocument.DocumentTypeID == ApplicationValidator.EgeDocumentTypeId &&
							x.SourceID == EntranceTestSource.EgeDocumentSourceId)
				.Select(x => x).ToList();
		}

		private List<ApplicationEntranceTestDocument> GetNoCertificateDocuments()
		{
			return Entities.ApplicationEntranceTestDocument
				.Include(x => x.EntrantDocument)
				.Include(x => x.Subject)
				.Where(x => x.ApplicationID == Application.ApplicationID &&
							x.EntrantDocumentID == null && x.ResultValue != null &&
							(x.SourceID == EntranceTestSource.EgeDocumentSourceId || x.SourceID == EntranceTestSource.OlympiadSourceId))
				.Select(x => x).ToList();
		}

		private List<EntranceTestItemC> GetTestItemsWithoutDocument()
		{
			Application.ApplicationSelectedCompetitiveGroup.Load();
			var appGroups = Application.ApplicationSelectedCompetitiveGroup.Select(x => x.CompetitiveGroupID).ToArray();
			return Entities.EntranceTestItemC
				.Include(x => x.Subject)
				.Where(
					x => appGroups.Contains(x.CompetitiveGroupID) && (x.EntranceTestTypeID == EntranceTestType.MainType && x.SubjectID != null) //проверка на SubjectID чтобы исключить те, где ЕГЭ не может быть
						 && x.ApplicationEntranceTestDocument.Count(y => y.ApplicationID == Application.ApplicationID) == 0)
				.Select(x => x).ToList();
		}
	}
}