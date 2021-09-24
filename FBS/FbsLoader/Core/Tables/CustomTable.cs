using System;
using System.Data.SqlClient;
using System.IO;

namespace Core.Tables
{
    public abstract class CustomTable
    {
        #region Constructors

        public CustomTable(string tableName, int year)
        {
            this.Year = year;
            //this.DateUpdateTo = dateUpdateTo;
            TableName = tableName;
            writer = new BulkWriter(tableName);
            updater = new BulkUpdater();
        }

        public CustomTable(string tableName)
        {
            TableName = tableName;
            writer = new BulkWriter(tableName);
            updater = new BulkUpdater();
        }

        #endregion

        #region Properties

        public string TableName { get; private set; }

        protected int Year { get; set; }

        protected SqlConnection SourceConnection { get; set; }

        protected SqlConnection EsrpConnection { get; set; }

        protected SqlConnection DestConnection { get; set; }

        protected BulkWriter writer { get; set; }

        protected BulkUpdater updater { get; set; }

        #endregion

        #region Public Methods

        public virtual void Init(SqlConnection sourceConn, SqlConnection destConn, SqlConnection esrpConn)
        {
            SourceConnection = sourceConn;
            EsrpConnection = esrpConn;
            DestConnection = destConn;

            writer.Init(destConn);
            updater.Init(destConn);
        }

        public abstract void DoDelete();

        public abstract void DoInsert();

        public abstract void DoUpdate();

        #endregion

        #region Protected Methods

        protected delegate void Calculate(SqlDataReader reader);

        protected void GetInsertReader(SqlCommand cmd, Calculate calc)
        {
            SqlDataReader reader = cmd.ExecuteReader();

            Logger.Instance.WriteLog("Запрос выполнен", 2);

            while (reader.Read())
            {
                calc(reader);
            }
            writer.Flush();
            reader.Close();
        }

        protected void GetUpdateReader(SqlCommand cmd, Calculate calc)
        {
            SqlDataReader reader = cmd.ExecuteReader();

            Logger.Instance.WriteLog("Запрос выполнен", 2);

            while (reader.Read())
            {
                calc(reader);
            }
            updater.Flush();
            reader.Close();
        }

        #endregion

    }
}
