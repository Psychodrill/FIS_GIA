using System.Data.SqlClient;
using GVUZ.ServiceModel.Import.WebService;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base
{
    public abstract class DictionaryItemDtoDataLoader : DictionaryDataLoaderBase<DictionaryItemDto>
    {
        protected override DictionaryItemDto MapDtoFromReader(SqlDataReader reader)
        {
            return new DictionaryItemDto
                {
                    ID = reader.IsDBNull(0) ? null : reader[0].ToString().Trim(),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1).Trim()
                };
        }
    }
}