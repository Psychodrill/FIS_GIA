using System.Globalization;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService;
using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Сообщения об ошибках" (код 17)
    /// </summary>
    public class ErrorMessagesDictionaryDataLoader : IDictionaryDataLoader<DictionaryItemDto>
    {
        public DictionaryItemDto[] Load()
        {
            return ConflictMessages.GetMessagesList().Select(x => new DictionaryItemDto { ID = x.Key.ToString(CultureInfo.InvariantCulture), Name = x.Value }).ToArray();
        }

        public void Dispose()
        {
        }
    }
}