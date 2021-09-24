namespace Ege.Dal.Common.Mappers
{
    using System.Data.Common;
    using Ege.Check.Common;

    public interface IDataReaderSyncMapper<out T> : IMapper<DbDataReader, T>
    {
    }
}