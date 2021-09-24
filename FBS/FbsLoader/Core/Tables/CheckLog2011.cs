using System;
using System.Data.SqlClient;

namespace Core.Tables
{
    public class CheckLog2011 : CheckLog2010
    {
        public CheckLog2011(int year) : base(year) { }

        #region Public Methods

        public override void DoInsert()
        {
            SqlCommand cmd = SourceConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();

            cmd.CommandText =
                string.Format(@"
                select
                    '1' + '{3}' +
                    coalesce(
                        cast(month(A.UpdateDate) as nvarchar(100)) + '/' +
                        cast(day(A.UpdateDate) as nvarchar(100)) + '/' +
                        cast(year(A.UpdateDate) as nvarchar(100)) + ' ' +
                        cast(datepart(hour, A.UpdateDate) as nvarchar(100)) + ':' +
                        cast(datepart(minute, A.UpdateDate) as nvarchar(100)) + ':' +
                        cast(datepart(second, A.UpdateDate) as nvarchar(100)) + '.' +
                        cast(datepart(millisecond, A.UpdateDate) as nvarchar(100)),
                    '') + '{3}' +

                    coalesce(
                        cast(year(A.LogDate) as nvarchar(100)) + '-' +
                        cast(month(A.LogDate) as nvarchar(100)) + '-' +
                        cast(day(A.LogDate) as nvarchar(100)),
                    '') + '{3}' +

                    coalesce(cast(A.OrgId as nvarchar(100)), '') + '{3}' +
                    coalesce(cast(A.OrgType as nvarchar(100)), '') + '{3}' +
                    coalesce(cast(A.MainId as nvarchar(100)), '') + '{3}' +
                    coalesce(A.CheckType, '') + '{3}' +
                    coalesce(cast(A.CertId as nvarchar(100)), '') + '{3}' +
                    coalesce(A.ClientType, '')
                from
                (
                -- Интерактивные проверки (через web-интерфейс)
                select 
	                WL.EventDate UpdateDate,
                    WL.EventDate LogDate,
                    O.Id OrgId,
                    O.TypeId OrgType,
                    O.MainId MainId,
                    case
    	                when WL.CNENumber is not null then 'number'
                        when WL.TypographicNumber is not null then 'typo'
                        when WL.PassportNumber is not null then 'passport'
                        else 'unknown'
                    end CheckType,
                    WL.FoundedCNEId CertId,
                    'interactive' ClientType
                from 
	                CNEWebUICheckLog WL
                    inner join Account A on A.Id = WL.AccountId
                    inner join Organization2010 O on O.Id = A.OrganizationId
                where
                    WL.EventDate < @currentDate and year(WL.EventDate) = @year {0}
                    
                union all

                -- Пакетные проверки и проверки через web-сервис
                -- По номеру и ФИО
                select
	                CCB.UpdateDate,
                    CCB.UpdateDate LogDate,
                    O.Id OrgId,
                    O.TypeId OrgType,
                    O.MainId MainId,
                    'number' CheckType,
                    CC.SourceCertificateId CertId,
                    'csv' ClientType
                from
	                CommonNationalExamCertificateCheck CC
                    inner join CommonNationalExamCertificateCheckBatch CCB on CCB.Id = CC.BatchId
                    inner join Account A on A.Id = CCB.OwnerAccountId
                    inner join Organization2010 O on O.Id = A.OrganizationId
                where
                    CCB.UpdateDate < @currentDate and year(CCB.UpdateDate) = @year {1}

                union all

                -- По паспорту и ФИО + по типографскому номеру и ФИО
                select
	                CRB.UpdateDate,
                    CRB.UpdateDate LogDate,
                    O.Id OrgId,
                    O.TypeId OrgType,
                    O.MainId MainId,
                    case
    	                when CR.TypographicNumber is not null then 'typo'
                        else 'passport'
                    end CheckType,
                    CR.SourceCertificateId CertId,
                    'csv' ClientType
                from
	                CommonNationalExamCertificateRequest CR
                    inner join CommonNationalExamCertificateRequestBatch CRB on CRB.Id = CR.BatchId
                    inner join Account A on A.Id = CRB.OwnerAccountId
                    inner join Organization2010 O on O.Id = A.OrganizationId
                where
                    CRB.UpdateDate < @currentDate and year(CRB.UpdateDate) = @year {2}
                ) A
                order by
	                A.UpdateDate                
                ",
                LastUpdate.HasValue ? "and WL.EventDate >= @lastDate " : string.Empty,

                LastUpdate.HasValue ? "and CCB.UpdateDate >= @lastDate " : string.Empty,

                LastUpdate.HasValue ? "and CRB.UpdateDate >= @lastDate " : string.Empty,

                Config.ValueDelimeter()
                
                );

            cmd.Parameters.AddWithValue("@currentDate", CurrentDate);
            cmd.Parameters.AddWithValue("@year", Year);
            if (LastUpdate.HasValue) cmd.Parameters.AddWithValue("@lastDate", LastUpdate.Value.AddDays(1));

            GetInsertReader(cmd, InsertCalculate);
        }

        #endregion
    }
}



