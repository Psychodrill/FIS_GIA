namespace Ege.Check.Logic.Models.Response
{
    using Ege.Check.Logic.Models.Cache;

    public class ExamListResponse
    {
        /// <summary>
        ///     Информация от РЦОИ
        /// </summary>
        public RegionInfoCacheModel Info { get; set; }

        /// <summary>
        ///     Результаты экзаменов
        /// </summary>
        public ExamCollectionCacheModel Result { get; set; }
    }
}