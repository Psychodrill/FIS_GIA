using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Fbs.Core.CatalogElements
{
    /// <summary>
    /// Обобщенный класс элемента каталога (регион, тип ОУ и т.п.)
    /// </summary>
    public class CatalogElement
    {
        int? Id_;
        public int? Id
        {
            get
            {
                if (Id_ == 0)
                    return null;
                return Id_;
            }
            internal set { Id_ = value; }
        }

        string Name_;
        public string Name
        {
            get { return Name_; }
            set { Name_ = value; }
        }

        internal CatalogElement()
        {
        }

        public CatalogElement(int id)
        {
            Id = id;
        }

        internal CatalogElement(IDataReader reader, string idColumn, string nameColumn)
        {
            if (reader[idColumn] != DBNull.Value)
            {
                Id = Convert.ToInt32(reader[idColumn]);
                Name = reader[nameColumn].ToString();
            }
        }
    }
}
