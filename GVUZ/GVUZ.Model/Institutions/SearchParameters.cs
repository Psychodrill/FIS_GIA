using System.Collections.Generic;

namespace GVUZ.Model.Institutions
{
    /// <summary>
    ///     Базовый класс для параметров поиска.
    /// </summary>
    /// <typeparam name="TResult">
    ///     Результат (обычно некоторая структура из <see cref="InstitutionSearchResult" />
    ///     или аналогов).
    /// </typeparam>
    public class SearchParameters<TResult>
    {
        /// <summary>
        ///     Метод для конвертации результата.
        /// </summary>
        public delegate TResult ConvertResults
            (IEnumerable<InstitutionSearchResult> items, SearchParameters<TResult> parameters);

        /// <summary>
        ///     Делегат для конвертации результата.
        /// </summary>
        public virtual ConvertResults Convert { get; set; }
    }
}