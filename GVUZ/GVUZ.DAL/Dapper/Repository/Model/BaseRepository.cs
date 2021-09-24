using GVUZ.DAL.Dapper.Repository.Interfaces;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Repository.Model
{
    public class GvuzRepository : BaseRepository
    {
        private static string _currentConnectionString = null;

        public GvuzRepository()
            : base(_currentConnectionString)
        {
            if (!_initialized)
                throw new InvalidOperationException("Не выполнена инициализация"); 
        }

        private static bool _initialized = false;
        public static void Initialize(string connectionString)
        {
            _currentConnectionString = connectionString;
            _initialized = true;
        }
    }

    public abstract class BaseRepository
    {
        protected readonly string _connectionString;
        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected async Task<T> DbConnectionAsync<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }
        protected T DbConnection<T>(Func<IDbConnection, T> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
            }
            catch (SqlException ex)
            {

                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);

            }
        }

        protected SqlConnection OpenConnection()
        {
            var cn = new SqlConnection(_connectionString);
            cn.Open();
            return cn;
        }

        protected T WithTransaction<T>(Func<IDbTransaction, T> getData)
        {
            using (var cn = OpenConnection())
            {
                using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    bool wasErr = false;

                    try
                    {
                        return getData(tx);
                    }
                    catch
                    {
                        wasErr = true;
                        throw;
                    }
                    finally
                    {
                        if (wasErr)
                        {
                            tx.Rollback();
                        }
                        else
                        {
                            tx.Commit();
                        }
                    }
                }
            }
        }

        protected void WithTransaction(Action<IDbTransaction> execute)
        {
            using (var cn = OpenConnection())
            {
                using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        execute(tx);
                        tx.Commit();
                    }
                    catch (Exception e)
                    {
                        tx.Rollback();
                        Debug.WriteLine(">>>>> Transaction Error: " + e.Message);
                        throw;
                    }
                }
            }
        }
    }
}
