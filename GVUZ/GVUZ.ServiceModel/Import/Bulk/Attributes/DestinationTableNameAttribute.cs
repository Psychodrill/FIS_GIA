using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.ServiceModel.Import.Bulk.Attributes
{
    /// <summary>
    /// Аттрибут SQL таблицы назначения (DestinationTableName) при BULK загрузке данных
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DestinationTableNameAttribute : Attribute
    {
        public string TableName { get; private set; }
        public DestinationTableNameAttribute(string name)
        {
            TableName = name;
        }
    }
}
