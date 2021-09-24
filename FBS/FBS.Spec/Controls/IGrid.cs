namespace FBS.Spec.Controls
{
    using System.Collections.Generic;

    using WatiN.Core;

    /// <summary>
    /// Таблица с данными
    /// </summary>
    public interface IGrid
    {
        /// <summary>
        /// Строки таблицы с данными
        /// </summary>
        IEnumerable<TableRow> DataRows { get; }
    }
}