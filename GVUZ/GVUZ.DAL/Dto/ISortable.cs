namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Правило сортировки при выполнении одного запроса к БД
    /// </summary>
    public interface ISortable
    {
        /// <summary>
        /// Наименования сортируемого поля 
        /// </summary>
        string SortExpression { get; }

        /// <summary>
        /// Признак сортировки по-убыванию (true) или по-возрастанию (false)
        /// </summary>
        bool SortDescending { get; }
    }
}
