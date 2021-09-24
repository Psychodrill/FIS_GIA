using System;
using System.Data.SqlClient;

namespace Core.Tables
{
    public class RegistrationLog2010 : CustomTable
    {
        public RegistrationLog2010(int year) : base("RegistrationLog", year) { }

        #region Properties

        private DateTime? LastUpdate { get; set; }
        private DateTime CurrentDate { get; set; }

        #endregion

        #region Public Methods

        public override void Init(SqlConnection sourceConn, SqlConnection destConn, SqlConnection esrpConn)
        {
            base.Init(sourceConn, destConn, esrpConn);

            LastUpdate = GetLastUpdate();
            CurrentDate =
                Year < DateTime.Now.Year
                ? new DateTime(Year + 1, 1, 1)
                : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        public override void DoDelete()
        {
            if (LastUpdate.HasValue)
            {
                Logger.Instance.WriteLog(string.Format("Удаление последнего результата: {0}", LastUpdate.Value.ToString()), 2);

                SqlCommand cmd = DestConnection.CreateCommand();
                cmd.CommandTimeout = Config.CommandTimeout();

                cmd.CommandText = @"delete from RegistrationLog where LogDate = @lastUpdate";
                cmd.Parameters.AddWithValue("@lastUpdate", LastUpdate);

                int deletedCount = cmd.ExecuteNonQuery();
                LastUpdate = GetLastUpdate();
                Logger.Instance.WriteLog(string.Format("Удаление выполнено: {0}", deletedCount), 2);
            }
        }

        public override void DoInsert()
        {
            SqlCommand cmd = SourceConnection.CreateCommand();

            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandText =
                string.Format(@"
                select
                    '1' + '{1}' +
                    coalesce(
                        cast(month(AL.UpdateDate) as nvarchar(100)) + '/' +
                        cast(day(AL.UpdateDate) as nvarchar(100)) + '/' +
                        cast(year(AL.UpdateDate) as nvarchar(100)) + ' ' +
                        cast(datepart(hour, AL.UpdateDate) as nvarchar(100)) + ':' +
                        cast(datepart(minute, AL.UpdateDate) as nvarchar(100)) + ':' +
                        cast(datepart(second, AL.UpdateDate) as nvarchar(100)) + '.' +
                        cast(datepart(millisecond, AL.UpdateDate) as nvarchar(100)),
                    '') + '{1}' +

                    coalesce(
                        cast(year(AL.UpdateDate) as nvarchar(100)) + '-' +
                        cast(month(AL.UpdateDate) as nvarchar(100)) + '-' +
                        cast(day(AL.UpdateDate) as nvarchar(100)),
                    '') + '{1}' +

                    coalesce(cast(O.Id as nvarchar(100)), '') + '{1}' +

                    coalesce(cast(AL.AccountId as nvarchar(100)), '') + '{1}' +
        
                    coalesce(cast(
                        case 
    	                    when AL.VersionId = 1 and AL.[Status] = 'registration' then 'registration'
                            when AL.[Status] = 'activated' then 'activated'
                            when AL.[Status] = 'consideration' then 'consideration'
                            when AL.[Status] = 'deactivated' then 'deactivated'
                            when AL.[Status] = 'revision' then 'revision'
                            else ''
                        end
                    as nvarchar(100)), '')
                from
	                AccountLog AL
                    inner join OrganizationRequest2010 R on R.Id = AL.OrganizationId
                    inner join Organization2010 O on O.Id = R.OrganizationId
                where
                    (
	                (AL.VersionId = 1 and AL.[Status] = 'registration')
                    or 
                    (AL.IsStatusChange = 1 and AL.[Status] in ('activated', 'consideration', 'deactivated', 'revision'))
                    )
                    and AL.UpdateDate < @currentDate {0}
                order by
	                AL.UpdateDate    
                    ",
                LastUpdate.HasValue ? "and AL.UpdateDate >= @lastDate " : string.Empty,
                Config.ValueDelimeter()
                );

            cmd.Parameters.AddWithValue("@currentDate", CurrentDate);
            if (LastUpdate.HasValue) cmd.Parameters.AddWithValue("@lastDate", LastUpdate.Value.AddDays(1));

            GetInsertReader(cmd, InsertCalculate);
        }

        public override void DoUpdate() { }

        #endregion

        #region Private Methods

        private void InsertCalculate(SqlDataReader reader)
        {
            writer.Add(reader[0].ToString());
        }

        private DateTime? GetLastUpdate()
        {
            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandText =
                @"
                select
                    max(RL.LogDate)
                from
                    RegistrationLog RL
                where
                    year(RL.LogDate) = @year
                ";
            cmd.Parameters.AddWithValue("@year", Year);

            var maxDate = cmd.ExecuteScalar();

            if (maxDate != DBNull.Value)
            {
                return Convert.ToDateTime(maxDate);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
