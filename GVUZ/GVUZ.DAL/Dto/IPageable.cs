namespace GVUZ.DAL.Dto
{
    public interface IPageable
    {
        /// <summary>
        /// Текущая страница
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        /// Размер текущей страницы
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Общее количество найденных записей (в т.ч. удовлетворяющих текущим критериям поиска)
        /// </summary>
        int TotalRecords { get; set; }
    }
}
