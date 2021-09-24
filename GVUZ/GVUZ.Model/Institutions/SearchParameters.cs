using System.Collections.Generic;

namespace GVUZ.Model.Institutions
{
    /// <summary>
    ///     ������� ����� ��� ���������� ������.
    /// </summary>
    /// <typeparam name="TResult">
    ///     ��������� (������ ��������� ��������� �� <see cref="InstitutionSearchResult" />
    ///     ��� ��������).
    /// </typeparam>
    public class SearchParameters<TResult>
    {
        /// <summary>
        ///     ����� ��� ����������� ����������.
        /// </summary>
        public delegate TResult ConvertResults
            (IEnumerable<InstitutionSearchResult> items, SearchParameters<TResult> parameters);

        /// <summary>
        ///     ������� ��� ����������� ����������.
        /// </summary>
        public virtual ConvertResults Convert { get; set; }
    }
}