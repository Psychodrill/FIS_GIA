namespace Ege.Dal.Common.Mappers
{
    using System.Data;
    using Ege.Check.Common;

    internal interface IDataTableMapper<in T> : IMapper<T, DataTable>
    {
    }
}