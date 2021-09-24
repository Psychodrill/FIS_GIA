using System;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Сведения о заявках на добавление или исключение из списка разрешенных направлений или списка направлений с профильными ВИ
    /// </summary>
    public class InstitutionDirectionRequestSummaryDto
    {
        /// <summary>
        /// Id ОО
        /// </summary>
        public int InstitutionId { get; set; }

        /// <summary>
        /// Наименование ОО
        /// </summary>
        public string InstitutionName { get; set; }

        /// <summary>
        /// Общее количество текущих (нерассмотренных) заявок
        /// </summary>
        public int NumRequests { get; set; }

        /// <summary>
        /// Дата последней нерассмотренной заявки
        /// </summary>
        public DateTime LastRequestDate { get; set; }
    }
}
