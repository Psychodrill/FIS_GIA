using System;

namespace GVUZ.ServiceModel.Import.Bulk.Attributes
{
    /// <summary>
    /// Аттрибут SQL таблицы назначения (DestinationTableName) при BULK загрузке данных
    /// </summary>
    public class BulkedProcessQueryAttribute : Attribute
    {
        public string Query { get; private set; }
        public BulkedProcessQueryAttribute(string query)
        {
            Query = query;
        }
    }
}
