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
    internal class ApplicationEgeContext
    {
        public ApplicationEgeContext(EntrantsEntities dbContext, Application application)
        {
            FullModels = new List<EGEDocumentViewModel>();
            Application = application;

            LoadData(dbContext);
        }

        private void LoadData(EntrantsEntities dbContext)
        {
            // MISSING: Нет поля Subject у обновленной таблицы
            throw new System.MemberAccessException("Ошибка загрузки данных о ApplicationEge. Отсутствует таблица!");
            //var appGroups = dbContext.ApplicationSelectedCompetitiveGroup.Where(x => x.ApplicationID == Application.ApplicationID).Select(x => x.CompetitiveGroupID).ToArray();//ToArray - Иначе следующий запрос будет очень тяжелый

            //TestItemsWithoutDocument = dbContext.EntranceTestItemC
            //    .Include(x => x.Subject)
            //    .Where(
            //        x => appGroups.Contains(x.CompetitiveGroupID) && (x.EntranceTestTypeID == EntranceTestType.MainType && x.SubjectID != null) //проверка на SubjectID чтобы исключить те, где ЕГЭ не может быть
            //             && x.ApplicationEntranceTestDocument.Count(y => y.ApplicationID == Application.ApplicationID) == 0)
            //    .ToList();

            //AllDocuments = dbContext.ApplicationEntranceTestDocument
            //    .Include(x => x.EntrantDocument)
            //    .Include(x => x.Subject)
            //    .Where(x => x.ApplicationID == Application.ApplicationID)
            //    .ToArray();

            //NoCertificateDocuments = AllDocuments
            //    .Where(x => x.EntrantDocumentID == null && x.ResultValue != null &&
            //                (x.SourceID == EntranceTestSource.EgeDocumentSourceId || x.SourceID == EntranceTestSource.OlympiadSourceId))
            //    .ToArray();

            //DocumentsWithCertificate = AllDocuments
            //    .Where(x => x.EntrantDocumentID != null && x.EntrantDocument.DocumentTypeID == ApplicationValidator.EgeDocumentTypeId &&
            //                x.SourceID == EntranceTestSource.EgeDocumentSourceId)
            //    .ToArray();

            //AllSubjects = dbContext.Subject.ToArray();

            //NotAttachedDocuments = dbContext.EntrantDocument
            //    .Where(x => x.EntrantID == Application.EntrantID && x.DocumentTypeID == ApplicationEgeValidator.EgeDocumentTypeId).ToArray();
        }

		/// <summary>
        /// Есть хотя бы один РВИ
        /// </summary>
        public bool HasAnyEntrantTestDocuments
        {
            get
            {
                return TestItemsWithoutDocument.Any() || NoCertificateDocuments.Any() ||
                         DocumentsWithCertificate.Any();
            }
        }
 
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
        public IEnumerable<EntranceTestItemC> TestItemsWithoutDocument { get; private set; }

        /// <summary>
        /// Документы, для которых нет свидетельств в локальной базе (например, из других заявлений абитуриента).
        /// </summary>
        public IEnumerable<ApplicationEntranceTestDocument> NoCertificateDocuments { get; private set; }

        /// <summary>
        /// Документы, для которых есть свидетельства в локальной базе (например, из других заявлений абитуриента).
        /// </summary>
        public IEnumerable<ApplicationEntranceTestDocument> DocumentsWithCertificate { get; private set; }

        /// <summary>
        /// Все документы заявления
        /// </summary>
        public IEnumerable<ApplicationEntranceTestDocument> AllDocuments { get; private set; }

        /// <summary>
        /// Справочник предметов
        /// </summary>
        public IEnumerable<Subject> AllSubjects { get; private set; }

        /// <summary>
        /// Документы абитуриента, не прикреплённые к заявлению
        /// </summary>
        public IEnumerable<EntrantDocument> NotAttachedDocuments { get; private set; }
    }
}