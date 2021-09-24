using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esrp.DB.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace Esrp.DB.EsrpADODB
{
    public class EsrpADORepository : RepositoryBase
    {
        private static string connectionString_;
        public static void Init(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString");
            connectionString_ = connectionString;
        }

        public static EsrpADORepository Create()
        {
            if (String.IsNullOrEmpty(connectionString_))
                throw new InvalidOperationException("Класс не был инициализирован. Вызовите метод Init");
            return new EsrpADORepository(connectionString_);
        }

        protected EsrpADORepository(string connectionString) : base(connectionString) { }

        public override string GetTableName<TEntity>()
        {
            return typeof(TEntity).Name;
        }

        protected override string GetIdField<TEntity>()
        {
            return "Id";
        }

        protected override bool GetIdIsDbGenerated<TEntity>()
        {
            if (GetTableName<TEntity>() == "Region")
                return false;
            return true;
        }

        public IEnumerable<TEntity> GetWithNotEmptyEiisId<TEntity>() where TEntity : EntityBase, IEIISIdable, new()
        {
            Dictionary<string, TEntity> result = new Dictionary<string, TEntity>();

            SqlCommand cmd = Connection.CreateCommand();
            cmd.CommandText = GetSelectCommand<TEntity>();
            cmd.CommandText += " WHERE ISNULL([Eiis_Id],'')<>'' ";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    TEntity entity = new TEntity();
                    PopulateEntity(entity, reader);
                    if (!result.ContainsKey(entity.Eiis_Id))
                    {
                        result.Add(entity.Eiis_Id, entity);
                    }
                }
            }

            return result.Values;
        }

        public IEnumerable<Organization2010> GetOrganizationWithNotEmptyIslodId()
        {
            Dictionary<string, Organization2010> result = new Dictionary<string, Organization2010>();

            SqlCommand cmd = Connection.CreateCommand();
            cmd.CommandText = GetSelectCommand<Organization2010>();
            cmd.CommandText += " WHERE ISNULL([ISLOD_GUID],'')<>'' ";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Organization2010 entity = new Organization2010();
                    PopulateEntity(entity, reader);
                    if (!result.ContainsKey(entity.ISLOD_GUID))
                    {
                        result.Add(entity.ISLOD_GUID, entity);
                    }
                }
            }

            return result.Values;
        }
    }
}
