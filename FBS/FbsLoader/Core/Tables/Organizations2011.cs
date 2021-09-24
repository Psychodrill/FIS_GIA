using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Tables
{
    public class Organizations2011 : Organizations2010
    {
        #region Constructors

        public Organizations2011(int year) : base(year) { }

        #endregion

        #region Public Methods

        public override void Init(SqlConnection sourceConn, SqlConnection destConn, SqlConnection esrpConnection)
        {
            base.Init(sourceConn, destConn, esrpConnection);
        }

        public override void DoInsert()
        {
            InsertedOrgIds = GetDestHash();

            Logger.Instance.WriteLog("Получение информации", 2);
            Dictionary<int, string> commonInfo = GetOrgCommonInfo();
            Dictionary<int, string> extraInfo = GetOrgExtraInfo();

            foreach (KeyValuePair<int, string> pair in commonInfo)
            {
                if (InsertedOrgIds.Contains(pair.Key))
                {
                    continue;
                }

                writer.Add(
                    extraInfo.ContainsKey(pair.Key)
                    ? pair.Value + extraInfo[pair.Key]
                    : pair.Value + string.Empty.PadLeft(7, Config.ValueDelimeter()[0])
                    );
            }
            writer.Flush();
        }

        #endregion

        #region Private Methods

        private Dictionary<int, string> GetOrgCommonInfo()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            SqlCommand cmd = EsrpConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandText =
            string.Format(@"
                select
                    O.Id,
                    cast(O.Id as nvarchar(100)) + '{0}' +

                    coalesce(
                        cast(month(O.UpdateDate) as nvarchar(100)) + '/' +
                        cast(day(O.UpdateDate) as nvarchar(100)) + '/' +
                        cast(year(O.UpdateDate) as nvarchar(100)) + ' ' +
                        cast(datepart(hour, O.UpdateDate) as nvarchar(100)) + ':' +
                        cast(datepart(minute, O.UpdateDate) as nvarchar(100)) + ':' +
                        cast(datepart(second, O.UpdateDate) as nvarchar(100)) + '.' +
                        cast(datepart(millisecond, O.UpdateDate) as nvarchar(100)),
                    '') + '{0}' +

                    coalesce(
                        cast(month(O.CreateDate) as nvarchar(100)) + '/' +
                        cast(day(O.CreateDate) as nvarchar(100)) + '/' +
                        cast(year(O.CreateDate) as nvarchar(100)) + ' ' +
                        cast(datepart(hour, O.CreateDate) as nvarchar(100)) + ':' +
                        cast(datepart(minute, O.CreateDate) as nvarchar(100)) + ':' +
                        cast(datepart(second, O.CreateDate) as nvarchar(100)) + '.' +
                        cast(datepart(millisecond, O.CreateDate) as nvarchar(100)),
                    '') + '{0}' +

	                coalesce(cast(FD.Id as nvarchar(100)), '') + '{0}' +
                    coalesce(FD.[Name], '') + '{0}' +
                    coalesce(cast(R.Id as nvarchar(100)), '') + '{0}' +
                    coalesce(R.[Name], '') + '{0}' +
                    coalesce(cast(OT.Id as nvarchar(100)), '') + '{0}' +
                    coalesce(OT.[Name], '') + '{0}' +
                    coalesce(cast(OK.Id as nvarchar(100)), '') + '{0}' +
                    coalesce(OK.[Name], '') + '{0}' +
                    coalesce(O.ShortName, '') + '{0}' +
                    coalesce(O.FullName, '') + '{0}' +
                    coalesce(cast(O.MainId as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.DepartmentId as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.IsPrivate as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.IsAccredited as nvarchar(100)), '') + '{0}' +
                    coalesce(O.AccreditationSertificate, '') + '{0}' +
                    
                    coalesce(
                        cast(month(T1.RegisterDate) as nvarchar(100)) + '/' +
                        cast(day(T1.RegisterDate) as nvarchar(100)) + '/' +
                        cast(year(T1.RegisterDate) as nvarchar(100)) + ' ' +
                        cast(datepart(hour, T1.RegisterDate) as nvarchar(100)) + ':' +
                        cast(datepart(minute, T1.RegisterDate) as nvarchar(100)) + ':' +
                        cast(datepart(second, T1.RegisterDate) as nvarchar(100)) + '.' +
                        cast(datepart(millisecond, T1.RegisterDate) as nvarchar(100)),
                    '') + '{0}' +

                    coalesce(O.DirectorFullName, '') + '{0}' +
                    coalesce(O.DirectorPosition, '') + '{0}' +
                    coalesce(O.FactAddress, '') + '{0}' +
                    coalesce(O.LawAddress, '') + '{0}' +
                    coalesce(O.PhoneCityCode, '') + '{0}' +
                    coalesce(O.Phone, '') + '{0}' +
                    coalesce(O.EMail, '') + '{0}' +
                    coalesce(O.INN, '') + '{0}' +
                    coalesce(O.OGRN, '')
                from 
                    Organization2010 O
                    left join RecruitmentCampaigns RC on RC.Id = O.RCModel
                    left join Region R on R.Id = O.RegionId
                    left join FederalDistricts FD on FD.Id = R.FederalDistrictId
                    left join OrganizationType2010 OT on OT.Id = O.TypeId
                    left join OrganizationKind OK on OK.Id = O.KindId
                    left join 
                    (
                        select min(ORR.CreateDate) RegisterDate, ORR.OrganizationId 
                        from OrganizationRequest2010 ORR 
                        group by ORR.OrganizationId
                    ) T1 on T1.OrganizationId = O.Id
            ", Config.ValueDelimeter());

            SqlDataReader reader = cmd.ExecuteReader();
            Logger.Instance.WriteLog("Общая информация об организации. Запрос выполнен", 2);
            while (reader.Read())
            {
                result.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
            }
            reader.Close();

            return result;
        }

        private Dictionary<int, string> GetOrgExtraInfo()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            SqlCommand cmd = SourceConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandText =
            string.Format(@"
                select
                    O.Id,
                    '{0}' +
                    coalesce(case when RC.Id <> 999 then RC.ModelName else O.RCDescription end, '') + '{0}' +
                    coalesce(cast(O.CNFBFullTime as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.CNFBEvening as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.CNFBPostal as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.CNPayFullTime as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.CNPayEvening as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.CNPayPostal as nvarchar(100)), '')
                from 
                    Organization2010 O
                    left join RecruitmentCampaigns RC on RC.Id = O.RCModel
            ", Config.ValueDelimeter());

            SqlDataReader reader = cmd.ExecuteReader();
            Logger.Instance.WriteLog("Дополнительная информация об организации. Запрос выполнен", 2);
            while (reader.Read())
            {
                result.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
            }
            reader.Close();

            return result;
        }

        #endregion
    }
}
