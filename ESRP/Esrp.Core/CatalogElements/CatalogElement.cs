namespace Esrp.Core.CatalogElements
{
    using System;
    using System.Data;

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
            get { return this.Name_; }
            set { this.Name_ = value; }
        }

		public string Code { get; set; }

        internal CatalogElement()
        {
        }

        public CatalogElement(int id)
        {
            this.Id = id;
        }

        internal CatalogElement(IDataReader reader, string idColumn, string nameColumn)
        {
            if (reader[idColumn] != DBNull.Value)
            {
                this.Id = Convert.ToInt32(reader[idColumn]);
                this.Name = reader[nameColumn].ToString();
            }
        }
    }
}
