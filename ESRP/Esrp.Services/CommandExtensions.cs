namespace Esrp.Services
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// расширения для System.Data
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// добавить стандартный пейджинг в параметры процедуры
        /// </summary>
        /// <param name="paramsCollection">
        /// параметры процедуры
        /// </param>
        /// <param name="startRow">
        /// начальная запись
        /// </param>
        /// <param name="maxRow">
        /// кол-во
        /// </param>
        public static void AddPaging(this SqlParameterCollection paramsCollection, int startRow, int maxRow)
        {
            paramsCollection.Add("@startRow", SqlDbType.Int).Value = startRow;
            paramsCollection.Add("@maxRow", SqlDbType.Int).Value = maxRow;
        }
    }
}