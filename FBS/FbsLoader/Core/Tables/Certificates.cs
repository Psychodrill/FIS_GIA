using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Core.Tables
{
    public class Certificates : CustomTable
    {
        #region Constructors

        public Certificates(int year) : base("Certificates", year) { }

        #endregion

        #region Properties

        private HashSet<Int64> InsertedCertIds { get; set; }

        #endregion

        #region Public Methods

        public override void DoDelete()
        {
            // 1. Выбираем за какие года у нас есть сертификаты для импорта
            Logger.Instance.WriteLog("Вычисляем количество итераций", 1);
            int deleteFlushRowCount = Config.DeleteFlushRowsCount();
            List<int> years = new List<int>();
            SqlCommand yearCmd = SourceConnection.CreateCommand();
            yearCmd.CommandTimeout = Config.CommandTimeout();
            yearCmd.CommandText = "select distinct C.Year from CommonNationalExamCertificate C";
            SqlDataReader yearReader = yearCmd.ExecuteReader();
            while (yearReader.Read())
            {
                years.Add(Convert.ToInt32(yearReader[0]));
            }
            yearReader.Close();
            Logger.Instance.WriteLog(string.Format("Выполнено. Количество итераций: {0}. Элементы: {1}", years.Count, string.Join(", ", years.Select(x => x.ToString()).ToArray())), 1);

            // 2. По каждому году выполняем поиск и удаление свидетельств, которые изменились с момента 
            // последней загрузки
            foreach (int year in years)
            {
                Logger.Instance.WriteLog(string.Format("Старт итерации. Элемент: {0}", year), 2);
                Dictionary<Int64, int> source = new Dictionary<Int64, int>();
                GC.Collect();
                source = GetSourceHash(year);
                Logger.Instance.WriteLog(string.Format("Выполнена загрузка свидетельств из исходной БД: {0}", source.Count), 3);

                Dictionary<Int64, int> dest = new Dictionary<Int64, int>();
                GC.Collect();
                dest = GetDestHash(year);
                Logger.Instance.WriteLog(string.Format("Выполнена загрузка свидетельств из аналитической БД: {0}", dest.Count), 3);

                HashSet<Int64> result = new HashSet<Int64>();

                foreach (KeyValuePair<Int64, int> pair in source)
                {
                    if (dest.ContainsKey(pair.Key))
                    {
                        if (dest[pair.Key] != pair.Value)
                        {
                            result.Add(pair.Key);
                        }
                    }
                }
                Logger.Instance.WriteLog(string.Format("Выполнен поиск различий: {0}", result.Count), 3);

                List<Int64> toDelete = new List<Int64>();
                int elementIndex = 0;
                foreach (Int64 id in result)
                {
                    elementIndex++;

                    toDelete.Add(id);
                    if (elementIndex > deleteFlushRowCount)
                    {
                        FlushDeleted(toDelete);
                        toDelete.Clear();
                        elementIndex = 0;
                    }
                }
                FlushDeleted(toDelete);

                Logger.Instance.WriteLog("Итерация завершена", 2);
            }
        }

        public override void DoInsert()
        {
            InsertedCertIds = GetInsertedCertIds();

            SqlCommand cmd = SourceConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();

            cmd.CommandText =
                string.Format(@"
                select
                    EC.Id,
	                coalesce(cast(EC.Id as nvarchar(100)), '') + '{0}' +

                    coalesce(
                        cast(month(EC.UpdateDate) as nvarchar(100)) + '/' +
                        cast(day(EC.UpdateDate) as nvarchar(100)) + '/' +
                        cast(year(EC.UpdateDate) as nvarchar(100)) + ' ' +
                        cast(datepart(hour, EC.UpdateDate) as nvarchar(100)) + ':' +
                        cast(datepart(minute, EC.UpdateDate) as nvarchar(100)) + ':' +
                        cast(datepart(second, EC.UpdateDate) as nvarchar(100)) + '.' +
                        cast(datepart(millisecond, EC.UpdateDate) as nvarchar(100)),
                    '') + '{0}' +

                    coalesce(
                        cast(month(EC.CreateDate) as nvarchar(100)) + '/' +
                        cast(day(EC.CreateDate) as nvarchar(100)) + '/' +
                        cast(year(EC.CreateDate) as nvarchar(100)) + ' ' +
                        cast(datepart(hour, EC.CreateDate) as nvarchar(100)) + ':' +
                        cast(datepart(minute, EC.CreateDate) as nvarchar(100)) + ':' +
                        cast(datepart(second, EC.CreateDate) as nvarchar(100)) + '.' +
                        cast(datepart(millisecond, EC.CreateDate) as nvarchar(100)),
                    '') + '{0}' +

                    coalesce(EC.Number, '') + '{0}' +

                    coalesce(EC.TypographicNumber, '') + '{0}' +

                    coalesce(EC.LastName, '') + '{0}' +

                    coalesce(EC.FirstName, '') + '{0}' +

                    coalesce(EC.PatronymicName, '') + '{0}' +

                    coalesce(EC.PassportSeria, '') + '{0}' +

                    coalesce(EC.PassportNumber, '') + '{0}' +

                    coalesce(cast(EC.RegionId as varchar(100)), '') + '{0}' +

                    coalesce(RG.Name, '') + '{0}' +

                    coalesce(cast(EC.[Year] as varchar(100)), '') + '{0}' +
                    
                    '' + '{0}' +
                    
                    '' + '{0}' +

                    replace(coalesce(cast(cast(A.Russian as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.Mathematics as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.Physics as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.Chemistry as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.Biology as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.RussiaHistory as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.Geography as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.English as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.German as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.Franch as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.SocialScience as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.Literature as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.Spanish as numeric(18,0)) as nvarchar(100)), ''), ',', '.') + '{0}' +
	                replace(coalesce(cast(cast(A.InformationScience as numeric(18,0)) as nvarchar(100)), ''), ',', '.')
                from 
	                CommonNationalExamCertificate EC
                    inner join Region RG on RG.Id = EC.RegionId
                    left join 
	                (
	                select
		                CS.CertificateId,
		                max(case when CS.SubjectId = 1 then CS.Mark else null end) Russian,
		                max(case when CS.SubjectId = 2 then CS.Mark else null end) Mathematics,
		                max(case when CS.SubjectId = 3 then CS.Mark else null end) Physics,
		                max(case when CS.SubjectId = 4 then CS.Mark else null end) Chemistry,
		                max(case when CS.SubjectId = 5 then CS.Mark else null end) Biology,
		                max(case when CS.SubjectId = 6 then CS.Mark else null end) RussiaHistory,
		                max(case when CS.SubjectId = 7 then CS.Mark else null end) Geography,
		                max(case when CS.SubjectId = 8 then CS.Mark else null end) English,
		                max(case when CS.SubjectId = 9 then CS.Mark else null end) German,
		                max(case when CS.SubjectId = 10 then CS.Mark else null end) Franch,
		                max(case when CS.SubjectId = 11 then CS.Mark else null end) SocialScience,
		                max(case when CS.SubjectId = 12 then CS.Mark else null end) Literature,
		                max(case when CS.SubjectId = 13 then CS.Mark else null end) Spanish,
		                max(case when CS.SubjectId = 14 then CS.Mark else null end) InformationScience
	                from
		                CommonNationalExamCertificateSubject CS
	                group by
		                CS.CertificateId
                    ) A on A.CertificateId = EC.Id
                ", Config.ValueDelimeter());

            GetInsertReader(cmd, InsertCalculate);
        }

        public override void DoUpdate()
        {
        }

        #endregion

        #region Private Methods

        private void FlushDeleted(List<Int64> toDelete)
        {
            if (toDelete.Count == 0)
            {
                return;
            }

            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();

            cmd.CommandText = "delete from Certificates where CertId in (" + string.Join(",", toDelete.Select(x => x.ToString()).ToArray()) + ")";
            cmd.ExecuteNonQuery();
        }

        private Dictionary<Int64, int> GetSourceHash(int year)
        {
            Dictionary<Int64, int> sourceHash = new Dictionary<Int64, int>();

            SqlCommand cmd = SourceConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandText =
            @"
                select
                    EC.Id,
                    replace(
	                cast(EC.Id as nvarchar(100)) + '|' +
                    cast(month(EC.CreateDate) as nvarchar(100)) + cast(day(EC.CreateDate) as nvarchar(100)) + '|' +
                    cast(month(EC.UpdateDate) as nvarchar(100)) + cast(day(EC.UpdateDate) as nvarchar(100)) + '|' +
                    cast(EC.Number as nvarchar(100)) + '|' +
                    cast(coalesce(EC.TypographicNumber, '') as nvarchar(100)) + '|' +
                    cast(coalesce(EC.LastName, '') as nvarchar(100)) + '|' +
                    cast(coalesce(EC.FirstName, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(EC.PatronymicName, '') as nvarchar(100)) + '|' +
                    cast(coalesce(EC.PassportSeria, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(EC.PassportNumber, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(EC.RegionId, 0) as nvarchar(100)) + '|' +
                    cast(cast(coalesce(A.Russian, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Mathematics, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Physics, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Chemistry, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Biology, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.RussiaHistory, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Geography, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.English, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.German, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Franch, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.SocialScience, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Literature, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Spanish, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.InformationScience, -1) as numeric(18,0)) as nvarchar(100)),
                    ',',
                    '.')
                from 
	                CommonNationalExamCertificate EC
                    left join Region RG on RG.Id = EC.RegionId
                    left join 
	                (
	                select
		                CS.CertificateId,
		                max(case when CS.SubjectId = 1 then CS.Mark else null end) Russian,
		                max(case when CS.SubjectId = 2 then CS.Mark else null end) Mathematics,
		                max(case when CS.SubjectId = 3 then CS.Mark else null end) Physics,
		                max(case when CS.SubjectId = 4 then CS.Mark else null end) Chemistry,
		                max(case when CS.SubjectId = 5 then CS.Mark else null end) Biology,
		                max(case when CS.SubjectId = 6 then CS.Mark else null end) RussiaHistory,
		                max(case when CS.SubjectId = 7 then CS.Mark else null end) Geography,
		                max(case when CS.SubjectId = 8 then CS.Mark else null end) English,
		                max(case when CS.SubjectId = 9 then CS.Mark else null end) German,
		                max(case when CS.SubjectId = 10 then CS.Mark else null end) Franch,
		                max(case when CS.SubjectId = 11 then CS.Mark else null end) SocialScience,
		                max(case when CS.SubjectId = 12 then CS.Mark else null end) Literature,
		                max(case when CS.SubjectId = 13 then CS.Mark else null end) Spanish,
		                max(case when CS.SubjectId = 14 then CS.Mark else null end) InformationScience
	                from
		                CommonNationalExamCertificateSubject CS
	                group by
		                CS.CertificateId
                    ) A on A.CertificateId = EC.Id
                where
                    EC.Year = @year
                ";
            cmd.Parameters.AddWithValue("@year", year);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sourceHash.Add(Convert.ToInt64(reader[0]), reader[1].ToString().GetHashCode());
            }
            reader.Close();

            return sourceHash;
        }

        private Dictionary<Int64, int> GetDestHash(int year)
        {
            Dictionary<Int64, int> destHash = new Dictionary<Int64, int>();

            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandText =
            @"
                select
                    C.CertId,
                    replace(
	                cast(C.CertId as nvarchar(100)) + '|' +
                    cast(month(C.CreateDate) as nvarchar(100)) + cast(day(C.CreateDate) as nvarchar(100)) + '|' +
                    cast(month(C.UpdateDate) as nvarchar(100)) + cast(day(C.UpdateDate) as nvarchar(100)) + '|' +
                    cast(C.CertNumber as nvarchar(100)) + '|' +
                    cast(coalesce(C.TypoNumber, '') as nvarchar(100)) + '|' +
                    cast(coalesce(C.LastName, '') as nvarchar(100)) + '|' +
                    cast(coalesce(C.FirstName, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(C.PatronymicName, '') as nvarchar(100)) + '|' +
                    cast(coalesce(C.PassportSeria, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(C.PassportNumber, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(C.RegionId, 0) as nvarchar(100)) + '|' +
                    cast(coalesce(C.Russian, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Mathematics, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Physics, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Chemistry, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Biology, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.RussiaHistory, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Geography, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.English, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.German, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Franch, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.SocialScience, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Literature, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Spanish, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.InformationScience, -1) as nvarchar(100)),
                    ',',
                    '.')
                from 
	                Certificates C
	            where
					C.CertYear = @year
            ";
            cmd.Parameters.AddWithValue("@year", year);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                destHash.Add(Convert.ToInt64(reader[0]), reader[1].ToString().GetHashCode());
            }
            reader.Close();

            return destHash;
        }

        private HashSet<Int64> GetInsertedCertIds()
        {
            HashSet<Int64> ids = new HashSet<Int64>();

            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            cmd.CommandText =
                @"
                select 
                    C.CertId
                from 
                    Certificates C
                ";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ids.Add(Convert.ToInt64(reader[0]));
            }
            reader.Close();

            return ids;
        }

        private void InsertCalculate(SqlDataReader reader)
        {
            if (InsertedCertIds.Contains(Convert.ToInt64(reader[0])))
            {
                return;
            }

            writer.Add(reader[1].ToString());
        }

        #endregion
    }
}
