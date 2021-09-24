namespace Ege.Dal.Common.Mappers
{
    using System.Collections.Concurrent;
    using System.Data.Common;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public abstract class DataReaderMapper<T> : IDataReaderMapper<T>
    {
        [NotNull] private readonly ConcurrentDictionary<string, int> _ordinal = new ConcurrentDictionary<string, int>();
        public abstract Task<T> Map([NotNull] DbDataReader @from);

        public int GetOrdinal([NotNull] DbDataReader @from, [NotNull] string name)
        {
            return _ordinal.GetOrAdd(name, @from.GetOrdinal);
        }
    }
}