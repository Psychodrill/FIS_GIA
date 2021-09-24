using System;
using System.Data.SqlClient;

namespace Core.Tables
{
    public class RegistrationLog2011 : RegistrationLog2010
    {
        public RegistrationLog2011(int year) : base(year) { }

        public override void Init(SqlConnection sourceConn, SqlConnection destConn, SqlConnection esrpConn)
        {
            base.Init(sourceConn, destConn, esrpConn);

            SourceConnection = EsrpConnection;
        }
    }
}
