namespace Ege.Check.Dal.Store.Bulk
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    ///     Дескриптор временной таблицы
    /// </summary>
    public interface IEgeTempTable : IDisposable
    {
        /// <summary>
        ///     Полное имя таблицы в БД
        /// </summary>
        [NotNull]
        string FullTableName { get; }
    }
}