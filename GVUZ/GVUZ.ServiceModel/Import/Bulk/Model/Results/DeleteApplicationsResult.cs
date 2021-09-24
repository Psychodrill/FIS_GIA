using System.Collections.Generic;

namespace GVUZ.ServiceModel.Import.Bulk.Model.Results
{
    /// <summary>
    ///     Результат удаления заявлений
    /// </summary>
    public class DeleteApplicationsResult : IEmptyResult
    {
        /// <summary>
        ///     Заявления со статусоь InOrder
        /// </summary>
        public List<ApplicationShortRefResult> ApplicationIsInOrder = new List<ApplicationShortRefResult>();

        /// <summary>
        ///     Не найденные заявления в БД
        /// </summary>
        public List<ApplicationShortRefResult> ApplicationIsNotFound = new List<ApplicationShortRefResult>();

        /// <summary>
        ///     Удаленные заявления
        /// </summary>
        public List<int> ApplicationsDeleted = new List<int>();


        public bool IsEmpty
        {
            get
            {
                return ApplicationIsInOrder.Count == 0
                       && ApplicationIsNotFound.Count == 0 && ApplicationsDeleted.Count == 0;
            }
        }
    }
}