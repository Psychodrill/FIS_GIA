namespace Ege.Dal.Common.Mappers
{
    using System.Collections.Generic;

    public interface IDataReaderCollectionMapper<T> : IDataReaderMapper<ICollection<T>>
    {
    }

    public interface IDataReaderListMapper<T> : IDataReaderMapper<IList<T>>
    {
    }
}