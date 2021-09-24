namespace Ege.Dal.Common.Mappers
{
    using System.Collections.Generic;

    public abstract class DataReaderCollectionMapper<T> : DataReaderMapper<ICollection<T>>,
                                                          IDataReaderCollectionMapper<T>
    {
    }

    public abstract class DataReaderListMapper<T> : DataReaderMapper<IList<T>>, IDataReaderListMapper<T>
    {
    }
}