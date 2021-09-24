namespace Ege.Check.Dal.Store.Bulk
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    ///     Управляет временными таблицами
    /// </summary>
    public interface IEgeTempTableOperator
    {
        /// <summary>
        ///     Создает временную таблицу и возвращает ее дескриптор
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dt"></param>
        /// <param name="connection">Коннекшен к бд</param>
        /// <param name="transaction">Транзакция, в рамках которой происходиит выполнение</param>
        /// <returns></returns>
        [NotNull]
        Task<IEgeTempTable> CreateAsync(Guid id, [NotNull] DataTable dt, [NotNull] DbConnection connection,
                                        DbTransaction transaction = null);
    }
}