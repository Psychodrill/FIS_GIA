using System;
using System.Data;
using System.Data.SqlClient;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base
{
    public abstract class SqlDictionaryDataLoaderBase<TDto> : IDictionaryDataLoader<TDto>
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public abstract TDto[] Load();
        
        protected SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
                _connection = new SqlConnection(connectionString);
                _connection.Open();
            }

            return _connection;
        }

        protected SqlTransaction GetTransaction()
        {
            if (_transaction == null)
            {
                _transaction = GetConnection().BeginTransaction(IsolationLevel.ReadCommitted);
            }

            return _transaction;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_transaction != null)
                {
                    try
                    {
                        _transaction.Commit();
                    }
                    finally
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                }

                if (_connection != null)
                {
                    try
                    {
                        _connection.Close();
                    }
                    finally
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
            }
        }

        ~SqlDictionaryDataLoaderBase()
        {
            Dispose(false);
        }

    }
}