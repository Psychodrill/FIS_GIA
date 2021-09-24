namespace Ege.Dal.Common.Mappers
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Common;

    public interface IDataReaderMapper<T> : IMapper<DbDataReader, Task<T>>
    {
    }
}