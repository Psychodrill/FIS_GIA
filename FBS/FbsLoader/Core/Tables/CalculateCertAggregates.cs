using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Core.Tables
{
    public class CalculateCertAggregates : CustomTable
    {
        public CalculateCertAggregates() : base("CalculateCertAggregates") { }

        #region Public Methods

        public override void Init(SqlConnection sourceConn, SqlConnection destConn, SqlConnection esrpConn)
        {
            base.Init(sourceConn, destConn, esrpConn);
        }

        public override void DoDelete()
        {
        }

        public override void DoInsert()
        {
            var aaa = GetStartDate();

            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "CalculateCertAggregates";
            cmd.ExecuteNonQuery();

            var a = aaa;
        }

        public override void DoUpdate() { }

        #endregion

        #region Private Methods

        private DateTime? GetStartDate()
        {
            // Определяем дату начала расчетов
            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "GetCertAggregatesStartDate";

            SqlParameter startDate = new SqlParameter();
            startDate.ParameterName = "StartDate";
            startDate.DbType = DbType.Date;
            startDate.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(startDate);
            cmd.ExecuteNonQuery();

            return cmd.Parameters["StartDate"].Value != DBNull.Value ? Convert.ToDateTime(cmd.Parameters["StartDate"].Value) : (DateTime?) null;
        }

        #endregion
    }
}