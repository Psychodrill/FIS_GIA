-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (45, '045_2012_06_21_ChangeOrg.sql')
-- =========================================================================
GO
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [FullName]                      NVARCHAR (1000) NOT NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [OwnerDepartment]               NVARCHAR (1000) NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [DirectorPosition]              NVARCHAR (1000) NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [DirectorFullName]              NVARCHAR (1000) NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [AccreditationSertificate]      NVARCHAR (1000) NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [LawAddress]                    NVARCHAR (1000) NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [FactAddress]                   NVARCHAR (1000) NOT NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN  [PhoneCityCode]                 NVARCHAR (1000) NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [Phone]                         NVARCHAR (1000) NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [Fax]                           NVARCHAR (1000) NULL
 
 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN  [EMail]                         NVARCHAR (1000) NULL

 ALTER TABLE dbo.Organization2010 
 ALTER COLUMN [Site]                          NVARCHAR (1000) NULL
 