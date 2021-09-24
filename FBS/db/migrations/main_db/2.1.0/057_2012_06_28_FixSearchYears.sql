-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (57, '057_2012_06_28_FixSearchYears.sql')
-- =========================================================================

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

--------------------------------------------------
-- Получить годы актуальности сертификатов ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 27.05.2008
--------------------------------------------------
ALTER function dbo.GetCommonNationalExamCertificateActuality
 (
 )
returns @Actuality table (YearFrom int, YearTo int)
as 
begin
 insert into @Actuality
 select
  Year(GetDate()) - 5
  , Year(GetDate())

 return
end
GO