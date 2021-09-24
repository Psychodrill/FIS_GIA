using System.Collections.Generic;
using System.Data.SqlClient;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base
{
    public abstract class DictionaryDataLoaderBase<TDto> : SqlDictionaryDataLoaderBase<TDto>
    {
        public override TDto[] Load()
        {
            List<TDto> result = new List<TDto>();

            using (SqlCommand cmd = CreateSelectCommand())
            {
                cmd.Connection = GetConnection();
                cmd.Transaction = GetTransaction();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(MapDtoFromReader(reader));
                    }
                }
            }

            return result.ToArray();
        }

        protected abstract TDto MapDtoFromReader(SqlDataReader reader);

        protected virtual SqlCommand CreateSelectCommand()
        {
            return new SqlCommand(GetQueryText());
        }

        protected abstract string GetQueryText();
    }
}