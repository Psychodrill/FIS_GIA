using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Esrp.DB.Common
{
    public abstract class RepositoryBase : IDisposable
    {
        protected SqlConnection Connection { get; private set; }

        protected RepositoryBase(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : EntityBase, IIdable, new()
        {
            List<TEntity> result = new List<TEntity>();

            SqlCommand cmd = Connection.CreateCommand();
            cmd.CommandTimeout = 600;
            cmd.CommandText = GetSelectCommand<TEntity>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    TEntity entity = new TEntity();
                    PopulateEntity(entity, reader);
                    result.Add(entity);
                }
            }

            return result;
        }

        public TEntity Get<TEntity>(int id) where TEntity : EntityBase, IIdable, new()
        {
            SqlCommand cmd = Connection.CreateCommand();
            cmd.CommandTimeout = 600;
            cmd.CommandText = GetSelectCommand<TEntity>();
            cmd.CommandText += String.Format(" WHERE [{0}]=@id", GetIdField<TEntity>());
            cmd.Parameters.AddWithValue("id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    TEntity result = new TEntity();
                    PopulateEntity(result, reader);
                    return result;
                }
                else
                    return default(TEntity);
            }
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : EntityBase, IIdable
        {
            SqlCommand cmd = Connection.CreateCommand();
            cmd.CommandTimeout = 600;
            cmd.CommandText = GetInsertCommand<TEntity>();
            PopulateSqlParameters<TEntity>(cmd, entity, true);
            cmd.ExecuteNonQuery();

            if (GetIdIsDbGenerated<TEntity>())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT @@IDENTITY";
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                entity.Id = id;
            }

            entity.SetHasChanges(false);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : EntityBase, IIdable
        {
            if (!entity.HasChanges)
                return;

            SqlCommand cmd = Connection.CreateCommand();
            cmd.CommandTimeout = 600;
            cmd.CommandText = GetUpdateCommand<TEntity>();
            PopulateSqlParameters<TEntity>(cmd, entity, false);
            cmd.ExecuteNonQuery();

            entity.SetHasChanges(false);
        }

        public void Delete<TEntity>(int id) where TEntity : EntityBase, IIdable
        {
            SqlCommand cmd = Connection.CreateCommand();
            cmd.CommandTimeout = 600;
            cmd.CommandText = String.Format("DELETE FROM [{0}] WHERE [{0}]=@id", GetTableName<TEntity>(), GetIdField<TEntity>());
            cmd.Parameters.AddWithValue("id", id);
        }

        protected void PopulateSqlParameters<TEntity>(SqlCommand cmd, TEntity entity, bool forInsert) where TEntity : EntityBase, IIdable
        {
            foreach (PropertyInfo property in GetMappedProperties<TEntity>())
            {
                if ((forInsert) && (GetIdIsDbGenerated<TEntity>()) && (property.Name == GetIdField<TEntity>()))
                    continue;

                object value = property.GetValue(entity, null);
                if (value == null)
                {
                    value = DBNull.Value;
                }
                cmd.Parameters.AddWithValue(property.Name.ToLower(), value);
            }
        }

        protected void PopulateEntity<TEntity>(TEntity entity, SqlDataReader reader) where TEntity : EntityBase, IIdable
        {
            foreach (PropertyInfo property in GetMappedProperties<TEntity>())
            {
                object value = reader[property.Name];
                if (value == DBNull.Value)
                {
                    value = null;
                }
                property.SetValue(entity, value, null);
            }
            entity.SetHasChanges(false);
        }

        protected string GetInsertCommand<TEntity>() where TEntity : EntityBase, IIdable
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("INSERT INTO [{0}]", GetTableName<TEntity>()).AppendLine();
            result.AppendLine("(");

            bool comma = false;
            foreach (PropertyInfo property in GetMappedProperties<TEntity>())
            {
                if ((GetIdIsDbGenerated<TEntity>()) && (property.Name == GetIdField<TEntity>()))
                    continue;

                result.AppendFormat("{0}{1}", comma ? "," : String.Empty, property.Name).AppendLine();
                comma = true;
            }
            result.AppendLine(")");
            result.AppendLine("VALUES");
            result.AppendLine("(");

            comma = false;
            foreach (PropertyInfo property in GetMappedProperties<TEntity>())
            {
                if ((GetIdIsDbGenerated<TEntity>()) && (property.Name == GetIdField<TEntity>()))
                    continue;

                result.AppendFormat("{0}@{1}", comma ? "," : String.Empty, property.Name.ToLower()).AppendLine();
                comma = true;
            }
            result.AppendLine(")");
            return result.ToString();
        }

        protected string GetUpdateCommand<TEntity>() where TEntity : EntityBase, IIdable
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("UPDATE [{0}] SET", GetTableName<TEntity>()).AppendLine();

            bool comma = false;
            foreach (PropertyInfo property in GetMappedProperties<TEntity>())
            {
                if ((GetIdIsDbGenerated<TEntity>()) && (property.Name == GetIdField<TEntity>()))
                    continue;

                result.AppendFormat("{0}{1} = @{2}", comma ? "," : String.Empty, property.Name, property.Name.ToLower()).AppendLine();
                comma = true;
            }
            result.AppendFormat("WHERE [{0}]=@id", GetIdField<TEntity>()).AppendLine();

            return result.ToString();
        }

        protected string GetSelectCommand<TEntity>() where TEntity : EntityBase, IIdable
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("SELECT");

            bool comma = false;
            foreach (PropertyInfo property in GetMappedProperties<TEntity>())
            {
                result.AppendFormat("{0}{1}", comma ? "," : String.Empty, property.Name).AppendLine();
                comma = true;
            }

            result.AppendFormat("FROM {0}", GetTableName<TEntity>()).AppendLine();

            return result.ToString();
        }

        public abstract string GetTableName<TEntity>() where TEntity : EntityBase, IIdable;

        protected abstract string GetIdField<TEntity>() where TEntity : EntityBase, IIdable;

        protected abstract bool GetIdIsDbGenerated<TEntity>() where TEntity : EntityBase, IIdable;

        private IEnumerable<PropertyInfo> GetMappedProperties<TEntity>() where TEntity : EntityBase, IIdable
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            foreach (PropertyInfo property in typeof(TEntity).GetProperties())
            {
                if (property.GetSetMethod() == null)
                    continue;

                result.Add(property);
            }
            return result;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
