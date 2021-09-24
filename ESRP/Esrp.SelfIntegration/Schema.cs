using System;
using System.Collections.Generic;
using System.Data;

namespace Esrp.SelfIntegration
{
    internal class Schema
    {
        public Schema(IDbConnection connection, string tableName)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = String.Format("SELECT * FROM [{0}]", tableName);
            using (IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                PopulateFromReader(reader);
            }
        }

        public Schema(IDataReader dataReader)
        {
            PopulateFromReader(dataReader);
        }

        private void PopulateFromReader(IDataReader dataReader)
        {
            DataTable schemaTable = dataReader.GetSchemaTable();
            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                bool isIdentity = (bool)schemaRow["IsIdentity"];
                if (isIdentity)
                {
                    hasIdentity_ = true;
                }
                SchemaColumn column = new SchemaColumn(schemaRow["ColumnName"].ToString(), schemaRow["DataTypeName"].ToString(), isIdentity);
                columns_.Add(column.Name, column);
            }
        }

        public SchemaColumn GetColumn(string name)
        {
            if (columns_.ContainsKey(name))
                return columns_[name];
            return null;
        }

        public IEnumerable<SchemaColumn> Columns
        {
            get
            {
                return columns_.Values;
            }
        }

        private bool hasIdentity_;
        public bool HasIdentity
        {
            get
            {
                return hasIdentity_;
            }
        }

        private Dictionary<string, SchemaColumn> columns_ = new Dictionary<string, SchemaColumn>();
    }

    internal class SchemaColumn
    {
        public SchemaColumn(string name, string sqlDataType, bool isIdentity)
        {
            Name = name;
            SqlDataType = sqlDataType.ToLower();
        }

        public bool IsIdentity { get; private set; }
        public string Name { get; private set; }
        public string SqlDataType { get; private set; }
    }
}
