using System;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base
{
    public interface IDictionaryDataLoader<out TDto> : IDisposable
    {
        TDto[] Load();
    }
}