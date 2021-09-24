using System.Collections.Generic;

namespace GVUZ.ServiceModel.Import.Bulk.Model.Results
{
    public class ConsideredApplicationsResult : IEmptyResult
    {
        /// <summary>
        ///     Не хватает мест
        /// </summary>
        public List<ApplicationShortRefResult> AdmissionVolumeOverflow = new List<ApplicationShortRefResult>();

        /// <summary>
        ///     Нельзя включить в список рассматриваемых заявление которое не принято
        /// </summary>
        public List<ApplicationShortRefResult> ApplicationNotAccepted = new List<ApplicationShortRefResult>();

        /// <summary>
        ///     Не найденные заявления в БД
        /// </summary>
        public List<ApplicationShortRefResult> ApplicationNotFound = new List<ApplicationShortRefResult>();

        /// <summary>
        ///     Не найдено направление уровень образования разрешенное для заявления
        /// </summary>
        public List<ApplicationShortRefResult> DirectionLevelNotFound = new List<ApplicationShortRefResult>();

        /// <summary>
        ///     2.	Балл по предметам должен быть > минимального в КГ
        /// </summary>
        public List<ApplicationShortRefResult> EntrantRatingIsLessThanMinimal = new List<ApplicationShortRefResult>();

        /// <summary>
        ///     Не найдены FinsourceID FinFormID для заявления
        /// </summary>
        public List<ApplicationShortRefResult> FinSourceFormNotFound = new List<ApplicationShortRefResult>();

        /// <summary>
        ///     Успешно добавленные
        /// </summary>
        public List<ApplicationShortRefResult> Successful = new List<ApplicationShortRefResult>();


        public bool IsEmpty
        {
            get
            {
                return
                    ApplicationNotFound.Count == 0 &&
                    Successful.Count == 0 &&
                    FinSourceFormNotFound.Count == 0 &&
                    DirectionLevelNotFound.Count == 0 &&
                    ApplicationNotAccepted.Count == 0 &&
                    EntrantRatingIsLessThanMinimal.Count == 0 &&
                    AdmissionVolumeOverflow.Count == 0;
            }
        }
    }
}