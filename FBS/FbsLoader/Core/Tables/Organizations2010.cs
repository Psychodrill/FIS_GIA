using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Tables
{
    public class Organizations2010 : CustomTable
    {
        #region Constructors

        public Organizations2010(int year) : base("Organizations", year) { }

        #endregion

        #region Properties

        protected HashSet<int> InsertedOrgIds { get; set; }

        #endregion

        #region Public Methods

        public override void DoDelete()
        {
            int deleteFlushRowCount = Config.DeleteFlushRowsCount();

            // 1. Извлекаем коды организаций в БД источника и приемника
            HashSet<int> source = GetSourceHash();
            Logger.Instance.WriteLog(string.Format("Выполнена загрузка организаций из исходной БД: {0}", source.Count), 2);

            HashSet<int> dest = GetDestHash();
            Logger.Instance.WriteLog(string.Format("Выполнена загрузка организаций из аналитической БД: {0}", dest.Count), 2);

            // 2. Поиск совпадений и их удаление
            List<int> toDelete = new List<int>();
            int elementIndex = 0;
            int total = 0;
            foreach (int key in source)
            {
                if (dest.Contains(key))
                {
                    elementIndex++;
                    total++;
                    toDelete.Add(key);
                    if (elementIndex > deleteFlushRowCount)
                    {
                        FlushDeleted(toDelete);
                        elementIndex = 0;
                        toDelete.Clear();
                    }
                }
            }
            FlushDeleted(toDelete);
            Logger.Instance.WriteLog(string.Format("Выполнено удаление найденных организаций: {0}", total), 2);
        }

        public override void DoInsert()
        {
            InsertedOrgIds = GetDestHash();

            SqlCommand cmd = SourceConnection.CreateCommand();
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
                    coalesce(O.OGRN, '') + '{0}' +
                    coalesce(case when RC.Id <> 999 then RC.ModelName else O.RCDescription end, '') + '{0}' +
                    coalesce(cast(O.CNFullTime as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.CNEvening as nvarchar(100)), '') + '{0}' +
                    coalesce(cast(O.CNPostal as nvarchar(100)), '') + '{0}{0}{0}'
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

            GetInsertReader(cmd, InsertCalculate);
        }

        public override void DoUpdate()
        {
        }

        #endregion

        #region Protected Methods

        protected void FlushDeleted(List<int> toDelete)
        {
            if (toDelete.Count == 0)
            {
                return;
            }

            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();

            cmd.CommandText = "delete from Organizations where OrgId in (" + string.Join(",", toDelete.Select(x => x.ToString()).ToArray()) + ")";
            cmd.ExecuteNonQuery();
        }

        protected HashSet<int> GetSourceHash()
        {
            HashSet<int> sourceHash = new HashSet<int>();

            SqlCommand cmd = SourceConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandText = "select O.Id from Organization2010 O";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sourceHash.Add(Convert.ToInt32(reader[0]));
            }
            reader.Close();

            return sourceHash;
        }

        protected HashSet<int> GetDestHash()
        {
            HashSet<int> destHash = new HashSet<int>();

            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandText = "select O.OrgId from Organizations O";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                destHash.Add(Convert.ToInt32(reader[0]));
            }
            reader.Close();

            return destHash;
        }

        protected void InsertCalculate(SqlDataReader reader)
        {
            if (InsertedOrgIds.Contains(Convert.ToInt32(reader[0])))
            {
                return;
            }

            writer.Add(reader[1].ToString());
        }

        #endregion
    }
}
