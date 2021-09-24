using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Core.Tables
{
    public class CalculateOrgAggregates : CustomTable
    {
        public CalculateOrgAggregates() : base("CalculateOrgAggregates") { }

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
            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "CalculateOrgAggregates";
            cmd.ExecuteNonQuery();
        }

        public override void DoUpdate() { }

        #endregion
    }
}
