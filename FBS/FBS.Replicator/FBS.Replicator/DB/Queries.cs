using System;
using System.Data.SqlClient;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;

namespace FBS.Replicator.DB
{
    public static class Queries
    {
        public static class Indexes_FBS
        {
            public static void CreateAllConstraintsQuery(Tables tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='PK_Participants?' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    ALTER TABLE [rbd].[Participants?] ADD  CONSTRAINT [PK_Participants?] PRIMARY KEY CLUSTERED 
    (
	    [ParticipantID] ASC,
	    [UseYear] ASC,
	    [REGION] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='PK_prn_Certificates?' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    ALTER TABLE [prn].[Certificates?] ADD  CONSTRAINT [PK_prn_Certificates?] PRIMARY KEY CLUSTERED 
    (
	    [CertificateID] ASC,
	    [REGION] ASC,
	    [UseYear] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='PK_prn_CertificatesMarks?' AND object_id = OBJECT_ID('[prn].[CertificatesMarks?]'))
BEGIN
    ALTER TABLE [prn].[CertificatesMarks?] ADD  CONSTRAINT [PK_prn_CertificatesMarks?] PRIMARY KEY CLUSTERED 
    (
	    [CertificateMarkID] ASC,
	    [UseYear] ASC,
	    [REGION] ASC,
	    [CertificateFK] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='PK_prn_CancelledCertificates?' AND object_id = OBJECT_ID('[prn].[CancelledCertificates?]'))
BEGIN
    ALTER TABLE [prn].[CancelledCertificates?] ADD  CONSTRAINT [PK_prn_CancelledCertificates?] PRIMARY KEY CLUSTERED 
    (
	    [CertificateFK] ASC,
	    [REGION] ASC,
	    [UseYear] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;
".Replace("?", TablesHelper.ToString(tables));
            }

            public static void DropAllConstraintsQuery(Tables tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
IF EXISTS (SELECT * FROM sys.indexes WHERE name='PK_Participants?' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    ALTER TABLE [rbd].[Participants?] DROP CONSTRAINT [PK_Participants?]
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='PK_prn_CertificatesMarks?' AND object_id = OBJECT_ID('[prn].[CertificatesMarks?]'))
BEGIN
    ALTER TABLE [prn].[CertificatesMarks?] DROP CONSTRAINT [PK_prn_CertificatesMarks?]
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='PK_prn_Certificates?' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    ALTER TABLE [prn].[Certificates?] DROP CONSTRAINT [PK_prn_Certificates?]
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='PK_prn_CancelledCertificates?' AND object_id = OBJECT_ID('[prn].[CancelledCertificates?]'))
BEGIN
    ALTER TABLE [prn].[CancelledCertificates?] DROP CONSTRAINT [PK_prn_CancelledCertificates?]
END
;
".Replace("?", TablesHelper.ToString(tables));
            }

            public static void CreateAllIndexesQuery(Tables tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_UseYear_other_fields' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Participants?_UseYear_other_fields] ON [rbd].[Participants?] 
    (
	    [UseYear] ASC
    )
    INCLUDE ( [ParticipantID],
    [REGION],
    [Surname],
    [Name],
    [SecondName],
    [DocumentSeries],
    [DocumentNumber]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_DocumentSeries_DocumentNumber_Surname_Name' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Participants?_DocumentSeries_DocumentNumber_Surname_Name] ON [rbd].[Participants?] 
    (
	    [DocumentSeries] ASC,
	    [DocumentNumber] ASC
    )
    INCLUDE ( [ParticipantID],
    [Surname],
    [Name]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_NameTrimmed' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Participants?_NameTrimmed] ON [rbd].[Participants?] 
    (
	    [NameTrimmed] ASC,
	    [SecondNameTrimmed] ASC
    )
    INCLUDE ( [ParticipantID],
    [UseYear],
    [REGION],
    [Surname],
    [Name],
    [SecondName],
    [DocumentSeries],
    [DocumentNumber],
    [SurnameTrimmed]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_FioTrimmed' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Participants?_FioTrimmed] ON [rbd].[Participants?] 
    (
	    [SurnameTrimmed] ASC,
	    [NameTrimmed] ASC,
	    [SecondNameTrimmed] ASC
    )
    INCLUDE ( [ParticipantID],
    [UseYear],
    [REGION],
    [Surname],
    [Name],
    [SecondName],
    [DocumentSeries],
    [DocumentNumber]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_DocumentNumber_NameSecondNameTrimmed' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Participants?_DocumentNumber_NameSecondNameTrimmed] ON [rbd].[Participants?] 
    (
	    [DocumentNumber] ASC,
	    [SurnameTrimmed] ASC
    )
    INCLUDE ( [Surname],
    [Name],
    [SecondName],
    [DocumentSeries],
    [NameTrimmed],
    [SecondNameTrimmed]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_PersonId' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Participants?_PersonId] ON [rbd].[Participants?] 
    (
	    PersonId ASC
    ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;


IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Certificates?_UseYear_CertificateID_ParticipantFK' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Certificates?_UseYear_CertificateID_ParticipantFK] ON [prn].[Certificates?] 
    (
	    [UseYear] ASC
    )
    INCLUDE ( [CertificateID],
    [ParticipantFK]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Certificates?_ParticipantFK' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Certificates?_ParticipantFK] ON [prn].[Certificates?] 
    (
	    [ParticipantFK] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Certificates?_LicenseNumber_UseYear' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Certificates?_LicenseNumber_UseYear] ON [prn].[Certificates?] 
    (
	    [LicenseNumber] ASC,
	    [UseYear] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Certificates?_TypographicNumber' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Certificates?_TypographicNumber] ON [prn].[Certificates?] 
    (
	    [TypographicNumber] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;


IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_CertificatesMarks?_CertificateFK' AND object_id = OBJECT_ID('[prn].[CertificatesMarks?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_CertificatesMarks?_CertificateFK] ON [prn].[CertificatesMarks?] 
    (
	    [CertificateFK] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_CertificatesMarks?_ParticipantFK' AND object_id = OBJECT_ID('[prn].[CertificatesMarks?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_CertificatesMarks?_ParticipantFK] ON [prn].[CertificatesMarks?] 
    (
	    [ParticipantFK] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
END
;
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_CertificatesMarks?_SubjectCode_Mark_UseYear_ParticipantFK' AND object_id = OBJECT_ID('[prn].[CertificatesMarks?]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_CertificatesMarks?_SubjectCode_Mark_UseYear_ParticipantFK] ON [prn].[CertificatesMarks?] 
    (
	    [SubjectCode] ASC,
	    [Mark] ASC,
	    [UseYear] ASC
    )
    INCLUDE ( [ParticipantFK]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
END
;
".Replace("?", TablesHelper.ToString(tables));
            }

            public static void DropAllIndexesQuery(Tables tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_UseYear_other_fields' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    DROP INDEX [IX_Participants?_UseYear_other_fields] ON [rbd].[Participants?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_NameTrimmed' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    DROP INDEX [IX_Participants?_NameTrimmed] ON [rbd].[Participants?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_FioTrimmed' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    DROP INDEX [IX_Participants?_FioTrimmed] ON [rbd].[Participants?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_DocumentSeries_DocumentNumber_Surname_Name' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    DROP INDEX [IX_Participants?_DocumentSeries_DocumentNumber_Surname_Name] ON [rbd].[Participants?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_DocumentNumber_NameSecondNameTrimmed' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    DROP INDEX [IX_Participants?_DocumentNumber_NameSecondNameTrimmed] ON [rbd].[Participants?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Participants?_PersonId' AND object_id = OBJECT_ID('[rbd].[Participants?]'))
BEGIN
    DROP INDEX [IX_Participants?_PersonId] ON [rbd].[Participants?] WITH ( ONLINE = OFF )
END
;

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Certificates?_UseYear_CertificateID_ParticipantFK' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    DROP INDEX [IX_Certificates?_UseYear_CertificateID_ParticipantFK] ON [prn].[Certificates?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Certificates?_TypographicNumber' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    DROP INDEX [IX_Certificates?_TypographicNumber] ON [prn].[Certificates?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Certificates?_ParticipantFK' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    DROP INDEX [IX_Certificates?_ParticipantFK] ON [prn].[Certificates?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Certificates?_LicenseNumber_UseYear' AND object_id = OBJECT_ID('[prn].[Certificates?]'))
BEGIN
    DROP INDEX [IX_Certificates?_LicenseNumber_UseYear] ON [prn].[Certificates?] WITH ( ONLINE = OFF )
END
;

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_CertificatesMarks?_SubjectCode_Mark_UseYear_ParticipantFK' AND object_id = OBJECT_ID('[prn].[CertificatesMarks?]'))
BEGIN
    DROP INDEX [IX_CertificatesMarks?_SubjectCode_Mark_UseYear_ParticipantFK] ON [prn].[CertificatesMarks?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_CertificatesMarks?_ParticipantFK' AND object_id = OBJECT_ID('[prn].[CertificatesMarks?]'))
BEGIN
    DROP INDEX [IX_CertificatesMarks?_ParticipantFK] ON [prn].[CertificatesMarks?] WITH ( ONLINE = OFF )
END
;
IF EXISTS (SELECT * FROM sys.indexes WHERE name='IX_CertificatesMarks?_CertificateFK' AND object_id = OBJECT_ID('[prn].[CertificatesMarks?]'))
BEGIN
    DROP INDEX [IX_CertificatesMarks?_CertificateFK] ON [prn].[CertificatesMarks?] WITH ( ONLINE = OFF )
END
;
".Replace("?", TablesHelper.ToString(tables));
            }
        }

        public static class TablesModes_FBS
        {
            public const string DropUsingTables = @"
DELETE FROM [dbo].[UsingTables]
";

            public const string SetUsingTablesA = @"
INSERT INTO [dbo].[UsingTables] (TableGroup) VALUES ('A')
";

            public const string SetUsingTablesB = @"
INSERT INTO [dbo].[UsingTables] (TableGroup) VALUES ('B')
";

            public const string GetUsingTables = @"
SELECT TOP 1 TableGroup FROM [dbo].[UsingTables]
";
        }

        public static class DBModes
        {
            public static void SetSingleUserQuery(SqlCommand cmd)
            {
                cmd.CommandText = @"
ALTER DATABASE ?
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE
".Replace("?", cmd.Connection.Database);
            }

            public static void SetMultipleUserQuery(SqlCommand cmd)
            {
                cmd.CommandText = @"
ALTER DATABASE ?
SET MULTI_USER
".Replace("?", cmd.Connection.Database);
            }
        }

        public static class Views_FBS
        {
            public static void AlterParticipantsViewQuery(Tables tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
ALTER VIEW [rbd].[Participants] 
AS
SELECT [ParticipantID]
      ,[UseYear]
      ,[REGION]
      ,[ParticipantCode]
      ,[Surname]
      ,[Name]
      ,[SecondName]
      ,[DocumentSeries]
      ,[DocumentNumber]
      ,[DocumentTypeCode]
      ,[Sex]
      ,[BirthDay]
      ,[FinishRegion]
      ,[ParticipantCategoryFK]
      ,[CreateDate]
      ,[UpdateDate]
      ,[ImportCreateDate]
      ,[ImportUpdateDate]
      ,[TestTypeID]
      ,[SurnameTrimmed]
      ,[NameTrimmed]
      ,[SecondNameTrimmed]
      ,[PersonId]
      ,[PersonLinkDate]
FROM [rbd].[Participants?]
".Replace("?", TablesHelper.ToString(tables));
            }

            public static void AlterCertificatesViewQuery(Tables tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
ALTER VIEW [prn].[Certificates] 
AS
SELECT [CertificateID]
      ,[UseYear]
      ,[REGION]
      ,[Wave]
      ,[LicenseNumber]
      ,[TypographicNumber]
      ,[ParticipantFK]
      ,[Cancelled]
      ,[CreateDate]
      ,[UpdateDate]
      ,[ImportCreateDate]
      ,[ImportUpdateDate]
FROM [prn].[Certificates?]
".Replace("?", TablesHelper.ToString(tables));
            }

            public static void AlterCertificateMarksViewQuery(Tables tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
ALTER VIEW [prn].[CertificatesMarks] 
AS
SELECT [CertificateMarkID]
      ,cm.[UseYear]
      ,cm.[REGION]
      ,[CertificateFK]
      ,[ParticipantFK]
      ,[SubjectCode]
      ,[Mark]
      ,[HasAppeal]
      ,[PrintedMarkID]
      ,cm.[TestTypeID]
      ,[ProcessCondition]
      ,[VariantCode]
      ,[AppealStatusID] 
      ,[ExamDate] 
      ,[CompositionBarcode]
      ,[CompositionPagesCount]
      ,[CompositionStatus]
      ,[CompositionPaths] 
FROM [prn].[CertificatesMarks?] cm
INNER JOIN [fbs].[rbd].[Participants?] p ON cm.ParticipantFK = p.ParticipantID AND cm.UseYear = p.UseYear AND cm.REGION = p.REGION
LEFT JOIN fbs.dbo.unvip uv ON p.PersonId = uv.PersonID AND cm.SubjectCode = uv.SubjectID AND (uv.[year] IS NULL OR uv.[Year] = cm.[UseYear])
WHERE uv.PersonID IS NULL
".Replace("?", TablesHelper.ToString(tables));
            }

            public static void AlterCancelledCertificatesViewQuery(Tables tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
ALTER VIEW [prn].[CancelledCertificates] 
AS
SELECT [UseYear]
      ,[REGION]
      ,[CertificateFK]
      ,[Reason]
FROM [prn].[CancelledCertificates?]
".Replace("?", TablesHelper.ToString(tables));
            }
        }

        public static class TableNames_FBS
        {
            public static string Participants(Tables tables)
            {
                return "[rbd].[Participants?]".Replace("?", TablesHelper.ToString(tables));
            }
            public static string Certificates(Tables tables)
            {
                return "[prn].[Certificates?]".Replace("?", TablesHelper.ToString(tables));
            }
            public static string CertificateMarks(Tables tables)
            {
                return "[prn].[CertificatesMarks?]".Replace("?", TablesHelper.ToString(tables));
            }
            public static string CancelledCertificates(Tables tables)
            {
                return "[prn].[CancelledCertificates?]".Replace("?", TablesHelper.ToString(tables));
            }

            public const string ParticipantsHidden = "#ParticipantsHidden";
            public const string ParticipantsTemp = "#Participants";
            public const string ParticipantIdsTemp = "#ParticipantIds";

            public const string CertificatesTemp = "#Certificates";
            public const string CertificateIdsTemp = "#CertificateIds";

            public const string CertificateMarksTemp = "#CertificatesMarks";
            public const string CertificateMarkIdsTemp = "#CertificatesMarkIds";
            public const string CancelledCertificatesTemp = "#CancelledCertificates";
            public const string CancelledCertificateIdsTemp = "#CancelledCertificateIds";

            public const string PersonsTemp = "#Persons";
        }

        public static class TableNames_GVUZ
        {
            public const string Persons = "[dbo].[RVIPersons]";
            public const string Documents = "[dbo].[RVIPersonIdentDocs]";

            public const string PersonsTemp = "#RVIPersons";
        }

        public static class TempTables_FBS
        {
            public const string CreateParticipantsHidden = @"
IF OBJECT_ID('tempdb..#ParticipantsHidden') IS NULL
CREATE TABLE #ParticipantsHidden (
    [ParticipantID] [uniqueidentifier] NOT NULL,
	[UseYear] [int] NOT NULL,
	[REGION] [int] NOT NULL, 
	[Surname] [varchar](80) NOT NULL,
	[Name] [varchar](80) NOT NULL,
	[SecondName] [varchar](80) NULL, 
	[SurnameTrimmed] [varchar](80) NULL,
	[NameTrimmed] [varchar](80) NULL,
	[SecondNameTrimmed] [varchar](80) NULL
)
";

            public static void MoveParticipantsHiddenQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
UPDATE [rbd].[Participants?] 
SET 
    [rbd].[Participants?].[Surname] = Temp.[Surname]
    ,[rbd].[Participants?].[Name] = Temp.[Name] 
    ,[rbd].[Participants?].[SecondName] = Temp.[SecondName] 
    ,[rbd].[Participants?].[SurnameTrimmed] = Temp.[SurnameTrimmed] 
    ,[rbd].[Participants?].[NameTrimmed] = Temp.[NameTrimmed]
    ,[rbd].[Participants?].[SecondNameTrimmed] = Temp.[SecondNameTrimmed]
FROM [rbd].[Participants?]
INNER JOIN #ParticipantsHidden Temp 
    ON [rbd].[Participants?].[ParticipantID] = Temp.[ParticipantID]
    AND [rbd].[Participants?].[UseYear] = Temp.[UseYear]
    AND [rbd].[Participants?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreateParticipantIdsTemp = @"
IF OBJECT_ID('tempdb..#ParticipantIds') IS NULL
CREATE TABLE #ParticipantIds (
    [ParticipantID] [uniqueidentifier] NOT NULL,
	[UseYear] [int] NOT NULL,
	[REGION] [int] NOT NULL
)
";

            public static void DeleteByParticipantIdsQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
DELETE [rbd].[Participants?] FROM [rbd].[Participants?]
INNER JOIN #ParticipantIds Temp 
    ON [rbd].[Participants?].[ParticipantID] = Temp.[ParticipantID]
    AND [rbd].[Participants?].[UseYear] = Temp.[UseYear]
    AND [rbd].[Participants?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreateParticipantsTemp = @"
IF OBJECT_ID('tempdb..#Participants') IS NULL
    CREATE TABLE #Participants (
        [ParticipantID] [uniqueidentifier] NOT NULL,
	    [UseYear] [int] NOT NULL,
	    [REGION] [int] NOT NULL,
	    [ParticipantCode] [varchar](16) NULL,
	    [Surname] [varchar](80) NOT NULL,
	    [Name] [varchar](80) NOT NULL,
	    [SecondName] [varchar](80) NULL,
	    [DocumentSeries] [varchar](9) NULL,
	    [DocumentNumber] [varchar](12) NOT NULL,
	    [DocumentTypeCode] [int] NOT NULL,
	    [Sex] [bit] NOT NULL,
	    [BirthDay] [datetime] NOT NULL,
	    [FinishRegion] [int] NULL,
	    [ParticipantCategoryFK] [int] NOT NULL,
	    [CreateDate] [datetime] NOT NULL,
	    [UpdateDate] [datetime] NOT NULL,
	    [ImportCreateDate] [datetime] NULL,
	    [ImportUpdateDate] [datetime] NULL,
	    [TestTypeID] [int] NOT NULL,
	    [SurnameTrimmed] [varchar](80) NULL,
	    [NameTrimmed] [varchar](80) NULL,
	    [SecondNameTrimmed] [varchar](80) NULL
)
";

            public static void MoveParticipantsTempQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
UPDATE [rbd].[Participants?] 
SET 
       [rbd].[Participants?].[ParticipantCode] = Temp.[ParticipantCode]
      ,[rbd].[Participants?].[Surname] = Temp.[Surname]
      ,[rbd].[Participants?].[Name] = Temp.[Name]
      ,[rbd].[Participants?].[SecondName] = Temp.[SecondName]
      ,[rbd].[Participants?].[DocumentSeries] = Temp.[DocumentSeries]
      ,[rbd].[Participants?].[DocumentNumber] = Temp.[DocumentNumber] 
      ,[rbd].[Participants?].[DocumentTypeCode] = Temp.[DocumentTypeCode]
      ,[rbd].[Participants?].[Sex] = Temp.[Sex]
      ,[rbd].[Participants?].[BirthDay] = Temp.[BirthDay]
      ,[rbd].[Participants?].[FinishRegion] = Temp.[FinishRegion]
      ,[rbd].[Participants?].[ParticipantCategoryFK] = Temp.[ParticipantCategoryFK]
      ,[rbd].[Participants?].[CreateDate] = Temp.[CreateDate]
      ,[rbd].[Participants?].[UpdateDate] = Temp.[UpdateDate]
      ,[rbd].[Participants?].[ImportCreateDate] = Temp.[ImportCreateDate]
      ,[rbd].[Participants?].[ImportUpdateDate] = Temp.[ImportUpdateDate] 
      ,[rbd].[Participants?].[TestTypeID] = Temp.[TestTypeID]
      ,[rbd].[Participants?].[SurnameTrimmed] = Temp.[SurnameTrimmed]
      ,[rbd].[Participants?].[NameTrimmed] = Temp.[NameTrimmed]
      ,[rbd].[Participants?].[SecondNameTrimmed] = Temp.[SecondNameTrimmed]
FROM [rbd].[Participants?]
INNER JOIN #Participants Temp 
    ON [rbd].[Participants?].[ParticipantID] = Temp.[ParticipantID]
    AND [rbd].[Participants?].[UseYear] = Temp.[UseYear]
    AND [rbd].[Participants?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreateCertificateIdsTemp = @"
IF OBJECT_ID('tempdb..#CertificateIds') IS NULL
    CREATE TABLE #CertificateIds (
        [CertificateID] [uniqueidentifier] NOT NULL,
        [UseYear] [int] NOT NULL,
        [REGION] [int] NOT NULL
)";

            public static void DeleteByCertificateIdsQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
DELETE [prn].[Certificates?] FROM [prn].[Certificates?]
INNER JOIN #CertificateIds Temp 
    ON [prn].[Certificates?].[CertificateID] = Temp.[CertificateID]
    AND [prn].[Certificates?].[UseYear] = Temp.[UseYear]
    AND [prn].[Certificates?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreateCertificatesTemp = @"
IF OBJECT_ID('tempdb..#Certificates') IS NULL
    CREATE TABLE #Certificates (
        [CertificateID] [uniqueidentifier] NOT NULL,
        [UseYear] [int] NOT NULL,
        [REGION] [int] NOT NULL,
        [Wave] [int] NOT NULL,
        [LicenseNumber] [nvarchar](18) NOT NULL,
        [TypographicNumber] [nvarchar](12) NULL,
        [ParticipantFK] [uniqueidentifier] NULL,
        [Cancelled] [bit] NOT NULL,
        [CreateDate] [datetime] NOT NULL,
        [UpdateDate] [datetime] NULL,
        [ImportCreateDate] [datetime] NULL,
        [ImportUpdateDate] [datetime] NULL
)
";

            public static void MoveCertificatesTempQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
UPDATE [prn].[Certificates?] 
SET 
       [prn].[Certificates?].[Wave] = Temp.[Wave]
      ,[prn].[Certificates?].[LicenseNumber] = Temp.[LicenseNumber]
      ,[prn].[Certificates?].[TypographicNumber] = Temp.[TypographicNumber]
      ,[prn].[Certificates?].[ParticipantFK] = Temp.[ParticipantFK]
      ,[prn].[Certificates?].[Cancelled] = Temp.[Cancelled]
      ,[prn].[Certificates?].[CreateDate] = Temp.[CreateDate]
      ,[prn].[Certificates?].[UpdateDate] = Temp.[UpdateDate] 
      ,[prn].[Certificates?].[ImportCreateDate] = Temp.[ImportCreateDate] 
      ,[prn].[Certificates?].[ImportUpdateDate] = Temp.[ImportUpdateDate]
FROM [prn].[Certificates?]
INNER JOIN #Certificates Temp 
    ON [prn].[Certificates?].[CertificateID] = Temp.[CertificateID]
    AND [prn].[Certificates?].[UseYear] = Temp.[UseYear]
    AND [prn].[Certificates?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreateCertificateMarkIdsTemp = @"
IF OBJECT_ID('tempdb..#CertificatesMarkIds') IS NULL
    CREATE TABLE #CertificatesMarkIds (
        [CertificateMarkID] [uniqueidentifier] NOT NULL,
        [UseYear] [int] NOT NULL,
        [REGION] [int] NOT NULL,
        [CertificateFK] [uniqueidentifier] NOT NULL
)";

            public static void DeleteByCertificateMarkIdsQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
DELETE [prn].[CertificatesMarks?] FROM [prn].[CertificatesMarks?]
INNER JOIN #CertificatesMarkIds Temp 
    ON [prn].[CertificatesMarks?].[CertificateMarkID] = Temp.[CertificateMarkID]
    AND [prn].[CertificatesMarks?].[CertificateFK] = Temp.[CertificateFK]
    AND [prn].[CertificatesMarks?].[UseYear] = Temp.[UseYear]
    AND [prn].[CertificatesMarks?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreateCertificateMarksTemp = @"
CREATE TABLE #CertificatesMarks (
	[CertificateMarkID] [uniqueidentifier] NOT NULL,
	[UseYear] [int] NOT NULL,
	[REGION] [int] NOT NULL,
	[CertificateFK] [uniqueidentifier] NOT NULL,
	[ParticipantFK] [uniqueidentifier] NOT NULL,
	[SubjectCode] [int] NOT NULL,
	[Mark] [int] NOT NULL,
	[HasAppeal] [bit] NOT NULL,
	[PrintedMarkID] [uniqueidentifier] NULL,
	[TestTypeID] [int] NOT NULL,
	[ProcessCondition] [int] NOT NULL,
    [VariantCode] [int] NULL,
    [AppealStatusID] [int] NULL,
    [ExamDate] [datetime] NULL,
    [CompositionBarcode] varchar(100) NULL,
    [CompositionPagesCount] [int] NULL,
    [CompositionStatus] [int] NULL,
    [CompositionPaths] varchar(max) NULL
)
";

            public static void MoveCertificateMarksTempQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
UPDATE [prn].[CertificatesMarks?] 
SET 
      [prn].[CertificatesMarks?].[ParticipantFK] = Temp.[ParticipantFK]
      ,[prn].[CertificatesMarks?].[SubjectCode] = Temp.[SubjectCode]
      ,[prn].[CertificatesMarks?].[Mark] = Temp.[Mark]
      ,[prn].[CertificatesMarks?].[HasAppeal] = Temp.[HasAppeal]
      ,[prn].[CertificatesMarks?].[PrintedMarkID] = Temp.[PrintedMarkID]
      ,[prn].[CertificatesMarks?].[TestTypeID] = Temp.[TestTypeID]
      ,[prn].[CertificatesMarks?].[ProcessCondition] = Temp.[ProcessCondition]
      ,[prn].[CertificatesMarks?].[VariantCode] = Temp.[VariantCode]
      ,[prn].[CertificatesMarks?].[AppealStatusID] = Temp.[AppealStatusID] 
      ,[prn].[CertificatesMarks?].[ExamDate] = Temp.[ExamDate]
      ,[prn].[CertificatesMarks?].[CompositionBarcode] = Temp.[CompositionBarcode]
      ,[prn].[CertificatesMarks?].[CompositionPagesCount] = Temp.[CompositionPagesCount]
      ,[prn].[CertificatesMarks?].[CompositionStatus] = Temp.[CompositionStatus]
      ,[prn].[CertificatesMarks?].[CompositionPaths] = Temp.[CompositionPaths]
FROM [prn].[CertificatesMarks?]
INNER JOIN #CertificatesMarks Temp 
    ON [prn].[CertificatesMarks?].[CertificateMarkID] = Temp.[CertificateMarkID]
    AND [prn].[CertificatesMarks?].[CertificateFK] = Temp.[CertificateFK]
    AND [prn].[CertificatesMarks?].[UseYear] = Temp.[UseYear]
    AND [prn].[CertificatesMarks?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreateCancelledCertificateIdsTemp = @"
CREATE TABLE #CancelledCertificateIds (
	[UseYear] [int] NOT NULL,
	[REGION] [int] NOT NULL,
	[CertificateID] [uniqueidentifier] NOT NULL
)";

            public static void DeleteByCancelledCertificateIdsQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
DELETE [prn].[CancelledCertificates?] FROM [prn].[CancelledCertificates?]
INNER JOIN #CancelledCertificateIds Temp 
    ON [prn].[CancelledCertificates?].[CertificateFK] = Temp.[CertificateID]
    AND [prn].[CancelledCertificates?].[UseYear] = Temp.[UseYear]
    AND [prn].[CancelledCertificates?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreateCancelledCertificatesTemp = @"
CREATE TABLE #CancelledCertificates (
	[UseYear] [int] NOT NULL,
	[REGION] [int] NOT NULL,
	[CertificateFK] [uniqueidentifier] NOT NULL,
	[Reason] [nvarchar](255) NULL
)
";

            public static void MoveCancelledCertificatesTempQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
UPDATE [prn].[CancelledCertificates?] 
SET 
       [prn].[CancelledCertificates?].[Reason] = Temp.[Reason] 
FROM [prn].[CancelledCertificates?]
INNER JOIN #CancelledCertificates Temp 
    ON [prn].[CancelledCertificates?].[CertificateFK] = Temp.[CertificateFK]
    AND [prn].[CancelledCertificates?].[UseYear] = Temp.[UseYear]
    AND [prn].[CancelledCertificates?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }

            public const string CreatePersonsTemp = @"
CREATE TABLE #Persons (
    [ParticipantID] [uniqueidentifier] NOT NULL,
	[UseYear] [int] NOT NULL,
	[REGION] [int] NOT NULL,
    [PersonId] INT NOT NULL, 
    [PersonLinkDate] DATETIME NOT NULL
)
";
            public static void LinkPersonsTempQuery(Tables? tables, SqlCommand cmd)
            {
                cmd.CommandText = @"
UPDATE [rbd].[Participants?] 
SET 
       [rbd].[Participants?].[PersonId] = Temp.[PersonId]
      ,[rbd].[Participants?].[PersonLinkDate] = Temp.[PersonLinkDate]
FROM [rbd].[Participants?]
INNER JOIN #Persons Temp 
    ON [rbd].[Participants?].[ParticipantID] = Temp.[ParticipantID]
    AND [rbd].[Participants?].[UseYear] = Temp.[UseYear]
    AND [rbd].[Participants?].[REGION] = Temp.[REGION] 
".Replace("?", TablesHelper.ToString(tables));
            }
        }

        public static class TempTables_GVUZ
        {
            public const string CreatePersonsTemp = @"
CREATE TABLE #RVIPersons (
	[PersonId] [int] NOT NULL,
	[IsRecordDeleted] [bit] NOT NULL,
	[NormSurname] [varchar](255) NOT NULL,
	[NormName] [varchar](255) NULL,
	[NormSecondName] [varchar](255) NULL,
	[BirthDay] [datetime] NULL,
	[Sex] [bit] NULL,
	[Email] varchar(150) NULL,
	[MobilePhone]	varchar(20) NULL,
	[SNILS] char(14) NULL,
	[INN]	char(12) NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[IntegralUpdateDate] [datetime] NOT NULL
);
";

            public static void MovePersonsTempQuery(SqlCommand cmd)
            {
                cmd.CommandText = @"
UPDATE [dbo].[RVIPersons] 
SET 
       [dbo].[RVIPersons].[BirthDay] = Temp.[BirthDay] 
       ,[dbo].[RVIPersons].[Sex] = Temp.[Sex] 
       ,[dbo].[RVIPersons].[UpdateDate] = Temp.[UpdateDate]
       ,[dbo].[RVIPersons].[IntegralUpdateDate] = Temp.[IntegralUpdateDate]
FROM [dbo].[RVIPersons] 
INNER JOIN #RVIPersons Temp 
    ON [dbo].[RVIPersons].[PersonId] = Temp.[PersonId] 
";
            }
        }

        public static class Read_FBS_ERBD
        {
            public static void CertificatesQuery(Tables? tables, SqlCommand cmd, int year)
            {
                cmd.CommandText = @"
SELECT
      [CertificateID]
      ,[UseYear]
      ,[REGION]
      ,[Wave]
      ,[LicenseNumber]
      ,[TypographicNumber]
      ,[ParticipantFK]
      ,[Cancelled]
      ,[CreateDate]
      ,[UpdateDate]
      ,[ImportCreateDate]
      ,[ImportUpdateDate]
FROM [prn].[Certificates?]
WHERE [UseYear] = @year
".Replace("?", TablesHelper.ToString(tables));
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("year", year);
            }

            public static void CancelledCertificatesQuery(Tables? tables, SqlCommand cmd, int year)
            {
                cmd.CommandText = @"
SELECT
      [UseYear]
      ,[REGION]
      ,[CertificateFK] AS CertificateID
      ,[Reason]
FROM [prn].[CancelledCertificates?]
WHERE [UseYear] = @year
".Replace("?", TablesHelper.ToString(tables));
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("year", year);
            }
        }

        public static class Read_ERBD
        {
            public static void HumanTestsQuery(SqlCommand cmd, int year, bool withCompositions)
            {
                cmd.CommandText = @"
SELECT 
      [res].[HumanTests].[HumanTestID]
      ,[res].[HumanTests].[ParticipantFK]
      ,[res].[HumanTests].[RegionCode]
      ,[res].[HumanTests].[TestTypeID]
      ,[res].[HumanTests].[SubjectCode]
      ,[res].[HumanTests].[ExamDate]
      ,[res].[HumanTests].[StationCode]
      ,[res].[HumanTests].[AuditoriumCode]
      ,[res].[HumanTests].[ProcessCondition]
      ,[res].[HumanTests].[Mark]
      ,[res].[HumanTests].[UseYear]
      ,[res].[HumanTests].[ImportCreateDate]
      ,[res].[HumanTests].[ImportUpdateDate]
      ,[res].[HumanTests].[HasAppeal]
      ,[res].[HumanTests].[VariantCode]
      ,[res].[HumanTests].[AppealStatusID]";
                if (withCompositions)
                {
                    cmd.CommandText += @"
      ,[sht].[Sheets_R].[Barcode]
      ,[sht].[FormPagesRequest].[Status]
      ,[sht].[Sheets_R].[ProjectBatchID]
      ,[sht].[Packages].[ProjectName]
";
                }
                else
                {
                    cmd.CommandText += @"
      ,NULL AS [Barcode]
      ,NULL AS [Status]
      ,NULL AS [ProjectBatchID]
      ,NULL AS [ProjectName]
";
                }
                cmd.CommandText += @"
FROM [res].[HumanTests]
";
                if (withCompositions)
                {
                    cmd.CommandText += @"
LEFT JOIN [res].[Complects]
    ON [res].[HumanTests].[HumanTestID] = [res].[Complects].[ComplectID] 
LEFT JOIN [sht].[Sheets_R]
    ON [res].[Complects].[SheetFK_R] = [sht].[Sheets_R].[SheetID]
LEFT JOIN [sht].[FormPagesRequest]
    ON [sht].[Sheets_R].[SheetID] = [sht].[FormPagesRequest].[SheetID] 
LEFT JOIN [sht].[Packages] 
    ON [sht].[Sheets_R].[PackageFK] = [sht].[Packages].[PackageID]
";
                }
                cmd.CommandText += @"
WHERE [res].[HumanTests].[UseYear] = @year
";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("year", year);
            }

            public static void QueryForCertificatesByMarks(SqlCommand cmd, int year)
            {
                cmd.CommandText = @"
SELECT DISTINCT
      [CertificateMarkID]
      ,[UseYear]
      ,[REGION]
      ,[CertificateFK]
      ,[ParticipantFK]
      ,[SubjectCode]
      ,[Mark]
      ,[HasAppeal]
      ,[PrintedMarkID]      
      ,[TestTypeID]
      ,[ProcessCondition]      
FROM [prn].[CertificatesMarks]
WHERE [UseYear] = @year
AND [CertificateFK] NOT IN
(SELECT [CertificateID] FROM [prn].[Certificates])
";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("year", year);
            }

            public static void ParticipantsQuery(SqlCommand cmd, int? year)
            {
                cmd.CommandText = @"
SELECT
      [ParticipantID]
      ,[UseYear]
      ,[REGION]
      ,[ParticipantCode]
      ,[Surname]
      ,[Name]
      ,[SecondName]
      ,[DocumentSeries]
      ,[DocumentNumber]
      ,[DocumentTypeCode]
      ,[Sex]
      ,[BirthDay]
      ,[FinishRegion]
      ,[ParticipantCategoryFK]
      ,[CreateDate]
      ,[UpdateDate]
      ,[ImportCreateDate]
      ,[ImportUpdateDate]
      ,[TestTypeID] 
FROM [rbd].[Participants]
";
                if (year.HasValue)
                {
                    cmd.CommandText += @"WHERE [UseYear] = @year
";
                }
                cmd.Parameters.Clear();
                if (year.HasValue)
                {
                    cmd.Parameters.AddWithValue("year", year);
                }
            }

            public static void QueryForCertificatesByHumanTests(SqlCommand cmd, int year)
            {
                cmd.CommandText = @"
SELECT DISTINCT
      '00000000-0000-0000-0000-000000000000' AS [HumanTestID] 
      ,'00000000-0000-0000-0000-000000000000' AS [ParticipantFK]
      ,[RegionCode]
      ,0 AS [TestTypeID]
      ,0 AS [SubjectCode]
      ,NULL AS [ExamDate]
      ,0 AS [StationCode]
      ,'' AS [AuditoriumCode]      
      ,0 AS [ProcessCondition]
      ,0 AS [Mark]
      ,[UseYear]
      ,[ImportCreateDate]
      ,[ImportUpdateDate]
      ,0 AS [HasAppeal]
      ,0 AS [VariantCode]
      ,NULL AS [AppealStatusID]
      ,NULL AS [Barcode]
      ,NULL AS [Status]
      ,NULL AS [ProjectBatchID]
      ,NULL AS [ProjectName]
FROM [res].[HumanTests]
WHERE [UseYear] = @year
";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("year", year);
            }

            public static void CertificateMarksQuery(SqlCommand cmd, int year)
            {
                cmd.CommandText = @"
SELECT
      [CertificateMarkID]
      ,cm.[UseYear]
      ,cm.[REGION]
      ,cm.[CertificateFK]
      ,[ParticipantFK]
      ,[SubjectCode]
      ,[Mark]
      ,[HasAppeal]
      ,[PrintedMarkID]
      ,[TestTypeID]
      ,CASE WHEN cc.CertificateFK IS NULL THEN [ProcessCondition] ELSE 121 END AS ProcessCondition
FROM [prn].[CertificatesMarks] cm
LEFT JOIN [prn].[CancelledCertificates] cc ON 
cm.CertificateFK = cc.CertificateFK AND cm.UseYear = cc.UseYear AND cm.REGION = cc.REGION
WHERE cm.[UseYear] = @year
";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("year", year);
            }
        }

        public static class Read_FBS
        {
            public static void ParticipantsQuery(Tables? tables, SqlCommand cmd, int? year)
            {
                cmd.CommandText = @"
SELECT
      [ParticipantID]
      ,[UseYear]
      ,[REGION]
      ,[ParticipantCode]
      ,[Surname]
      ,[Name]
      ,[SecondName]
      ,[DocumentSeries]
      ,[DocumentNumber]
      ,[DocumentTypeCode]
      ,[Sex]
      ,[BirthDay]
      ,[FinishRegion]
      ,[ParticipantCategoryFK]
      ,[CreateDate]
      ,[UpdateDate]
      ,[ImportCreateDate]
      ,[ImportUpdateDate]
      ,[TestTypeID] 
      ,[PersonId]
      ,[PersonLinkDate]
FROM [rbd].[Participants?]
".Replace("?", TablesHelper.ToString(tables));
                if (year.HasValue)
                {
                    cmd.CommandText += @"WHERE [UseYear] = @year
";
                }
                cmd.Parameters.Clear();
                if (year.HasValue)
                {
                    cmd.Parameters.AddWithValue("year", year);
                }
            }

            public static void CertificateMarksQuery(Tables? tables, SqlCommand cmd, int year)
            {
                cmd.CommandText = @"
SELECT
      [CertificateMarkID]
      ,[UseYear]
      ,[REGION]
      ,[CertificateFK]
      ,[ParticipantFK]
      ,[SubjectCode]
      ,[Mark]
      ,[HasAppeal]
      ,[PrintedMarkID]
      ,[TestTypeID]
      ,[ProcessCondition]
      ,[VariantCode]
      ,[AppealStatusID]
      ,[ExamDate]
      ,[CompositionBarcode]
      ,[CompositionPagesCount]
      ,[CompositionStatus]
      ,[CompositionPaths]
FROM [prn].[CertificatesMarks?]
WHERE [UseYear] = @year
".Replace("?", TablesHelper.ToString(tables));
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("year", year);
            }

            public static void ExpireDatesQuery(SqlCommand cmd)
            {
                cmd.CommandText = @"
SELECT 
      [Id]
      ,[Year]
      ,[ExpireDate]
FROM [dbo].[ExpireDate]
";
            }

            public static void MinimalMarksQuery(SqlCommand cmd)
            {
                cmd.CommandText = @"
SELECT 
      [Id]
      ,[SubjectId]
      ,[Year]
      ,[MinimalMark]
FROM [dbo].[MinimalMark]
";
            }

            public static void DocumentTypesMappingQuery(SqlCommand cmd)
            {
                cmd.CommandText = @"
SELECT 
      [ID]
      ,[RVIDocumentTypeCode]
      ,[ParticipantDocumentTypeCode]
  FROM [rbdc].[Translation_RVIDT_ParticipantDT]
";
            }
        }

        public static class Read_GVUZ
        {
            public const string MaxPersonId = @"SELECT ISNULL(MAX([PersonId]),0) FROM [dbo].[RVIPersons]";
            public const string MaxDocumentId = @"SELECT ISNULL(MAX([PersonIdentDocID]),0) FROM [dbo].[RVIPersonIdentDocs]";

            public static void PersonsQuery(SqlCommand cmd)
            {
                cmd.CommandText = @"
SELECT 
      [PersonId]
      ,[IsRecordDeleted]
      ,[NormSurname]
      ,[NormName]
      ,[NormSecondName]
      ,[BirthDay]
      ,[Sex]
      ,[Email]
      ,[MobilePhone]
      ,[SNILS]
      ,[INN]
      ,[CreateDate]
      ,[UpdateDate]
      ,[IntegralUpdateDate]
FROM [dbo].[RVIPersons]
WHERE
    [IsRecordDeleted] = 0
";
            }

            public static void IdentityDocumentsQuery(SqlCommand cmd)
            {
                cmd.CommandText = @"
SELECT 
      [PersonIdentDocID]
      ,[PersonId]
      ,[DocumentTypeCode]
      ,[DocumentSeries]
      ,[DocumentNumber]
      ,[DocumentDate]
      ,[DocumentOrganization]
      ,[DocumentName]
      ,[CreateDate]
      ,[UpdateDate]
FROM [dbo].[RVIPersonIdentDocs]
";
            }
        }

        public static class Write_FBS
        {
            public static void DeleteParticipantQuery(Tables tables, SqlCommand cmd, ParticipantId id)
            {
                cmd.CommandText = @"
DELETE FROM [rbd].[Participants?]
WHERE 
      [ParticipantID] = @ParticipantID
      and [UseYear] = @UseYear
      and [REGION] = @REGION
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantID", id.ParticipantID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", id.REGION);
            }

            public static void DeleteCertificateQuery(Tables tables, SqlCommand cmd, CertificateId id)
            {
                cmd.CommandText = @"
DELETE FROM [prn].[Certificates?]
WHERE
      [CertificateID] = @CertificateID
      and [UseYear] = @UseYear
      and [REGION] = @REGION
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateID", id.CertificateID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", id.REGION);
            }

            public static void DeleteCertificateMarkQuery(Tables tables, SqlCommand cmd, CertificateMarkId id)
            {
                cmd.CommandText = @"
DELETE FROM [prn].[CertificatesMarks?]
WHERE
      [CertificateMarkID] = @CertificateMarkID
      and [UseYear] = @UseYear
      and [REGION] = @REGION
      and [CertificateFK] = @CertificateFK
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateMarkID", id.CertificateMarkID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateFK", id.CertificateFK);
            }

            public static void DeleteCancelledCertificateQuery(Tables tables, SqlCommand cmd, CertificateId id)
            {
                cmd.CommandText = @"
DELETE FROM [prn].[CancelledCertificates?]
WHERE
      [UseYear] = @UseYear
      and [REGION] = @REGION
      and [CertificateFK] = @CertificateFK
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateFK", id.CertificateID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", id.REGION);
            }

            public static void InsertHiddenParticipantQuery(Tables tables, SqlCommand cmd, ERBDParticipant participant)
            {
                cmd.CommandText = @"
INSERT INTO [rbd].[Participants?]
           ([ParticipantID]
           ,[UseYear]
           ,[REGION]
           ,[ParticipantCode]
           ,[Surname]
           ,[Name]
           ,[SecondName]
           ,[DocumentSeries]
           ,[DocumentNumber]
           ,[DocumentTypeCode]
           ,[Sex]
           ,[BirthDay]
           ,[FinishRegion]
           ,[ParticipantCategoryFK]
           ,[CreateDate]
           ,[UpdateDate]
           ,[ImportCreateDate]
           ,[ImportUpdateDate]
           ,[TestTypeID]
           ,[SurnameTrimmed]
           ,[NameTrimmed]
           ,[SecondNameTrimmed])
     VALUES
           (@ParticipantID
           ,@UseYear
           ,@REGION
           ,@ParticipantCode
           ,@Surname
           ,@Name
           ,@SecondName
           ,@DocumentSeries
           ,@DocumentNumber
           ,@DocumentTypeCode
           ,@Sex
           ,@BirthDay
           ,@FinishRegion
           ,@ParticipantCategoryFK
           ,@CreateDate
           ,@UpdateDate
           ,@ImportCreateDate
           ,@ImportUpdateDate
           ,@TestTypeID
           ,@SurnameTrimmed
           ,@NameTrimmed
           ,@SecondNameTrimmed) 
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantID", participant.Id.ParticipantID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", participant.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", participant.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantCode", participant.ParticipantCodeStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "Surname", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "Name", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "SecondName", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentSeries", participant.DocumentSeriesStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentNumber", participant.DocumentNumberStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentTypeCode", participant.DocumentTypeCode);
                QueryHelper.AddParameterWithNullableValue(cmd, "Sex", participant.Sex);
                QueryHelper.AddParameterWithNullableValue(cmd, "BirthDay", participant.BirthDay);
                QueryHelper.AddParameterWithNullableValue(cmd, "FinishRegion", participant.FinishRegion);
                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantCategoryFK", participant.ParticipantCategoryFK);
                QueryHelper.AddParameterWithNullableValue(cmd, "CreateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "UpdateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "ImportCreateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "ImportUpdateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "TestTypeID", participant.TestTypeID);
                QueryHelper.AddParameterWithNullableValue(cmd, "SurnameTrimmed", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "NameTrimmed", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "SecondNameTrimmed", Guid.Empty.ToString());
            }

            public static void InsertCertificateQuery(Tables tables, SqlCommand cmd, ERBDCertificate certificate)
            {
                cmd.CommandText = @"
INSERT INTO [prn].[Certificates?]
           ([CertificateID]
           ,[UseYear]
           ,[REGION]
           ,[Wave]
           ,[LicenseNumber]
           ,[TypographicNumber]
           ,[ParticipantFK]
           ,[Cancelled]
           ,[CreateDate]
           ,[UpdateDate]
           ,[ImportCreateDate]
           ,[ImportUpdateDate])
     VALUES
           (@CertificateID
           ,@UseYear
           ,@REGION
           ,@Wave
           ,@LicenseNumber
           ,@TypographicNumber
           ,@ParticipantFK
           ,@Cancelled
           ,@CreateDate
           ,@UpdateDate
           ,@ImportCreateDate
           ,@ImportUpdateDate)
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateID", certificate.Id.CertificateID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", certificate.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", certificate.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "Wave", certificate.Wave);
                QueryHelper.AddParameterWithNullableValue(cmd, "LicenseNumber", certificate.LicenseNumberStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "TypographicNumber", certificate.TypographicNumberStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantFK", certificate.ParticipantFK);
                QueryHelper.AddParameterWithNullableValue(cmd, "Cancelled", certificate.Cancelled);
                QueryHelper.AddParameterWithNullableValue(cmd, "CreateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "UpdateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "ImportCreateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "ImportUpdateDate", DateTime.Now);
            }

            public static void InsertCertificateMarkQuery(Tables tables, SqlCommand cmd, ERBDCertificateMark certificateMark)
            {
                cmd.CommandText = @"
INSERT INTO [prn].[CertificatesMarks?]
           ([CertificateMarkID]
           ,[UseYear]
           ,[REGION]
           ,[CertificateFK]
           ,[ParticipantFK]
           ,[SubjectCode]
           ,[Mark]
           ,[HasAppeal]
           ,[PrintedMarkID]
           ,[TestTypeID]
           ,[ProcessCondition]
           ,[VariantCode]
           ,[AppealStatusID]
           ,[ExamDate]
           ,[CompositionBarcode]
           ,[CompositionPagesCount]
           ,[CompositionStatus]
           ,[CompositionPaths]
)
     VALUES
           (@CertificateMarkID
           ,@UseYear
           ,@REGION
           ,@CertificateFK
           ,@ParticipantFK
           ,@SubjectCode
           ,@Mark
           ,@HasAppeal
           ,@PrintedMarkID
           ,@TestTypeID
           ,@ProcessCondition
           ,@VariantCode
           ,@AppealStatusID
           ,@ExamDate
           ,@CompositionBarcode
           ,@CompositionPagesCount
           ,@CompositionStatus
           ,@CompositionPaths)
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateMarkID", certificateMark.Id.CertificateMarkID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", certificateMark.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", certificateMark.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateFK", certificateMark.Id.CertificateFK);
                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantFK", certificateMark.ParticipantFK);
                QueryHelper.AddParameterWithNullableValue(cmd, "SubjectCode", certificateMark.SubjectCode);
                QueryHelper.AddParameterWithNullableValue(cmd, "Mark", certificateMark.Mark);
                QueryHelper.AddParameterWithNullableValue(cmd, "HasAppeal", certificateMark.HasAppeal);
                QueryHelper.AddParameterWithNullableValue(cmd, "PrintedMarkID", certificateMark.PrintedMarkID);
                QueryHelper.AddParameterWithNullableValue(cmd, "TestTypeID", certificateMark.TestTypeID);
                QueryHelper.AddParameterWithNullableValue(cmd, "ProcessCondition", certificateMark.ProcessCondition);
                QueryHelper.AddParameterWithNullableValue(cmd, "VariantCode", certificateMark.VariantCode);
                QueryHelper.AddParameterWithNullableValue(cmd, "AppealStatusID", certificateMark.AppealStatusID);
                QueryHelper.AddParameterWithNullableValue(cmd, "ExamDate", certificateMark.ExamDate);
                QueryHelper.AddParameterWithNullableValue(cmd, "CompositionBarcode", certificateMark.CompositionBarcodeStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "CompositionPagesCount", certificateMark.CompositionPagesCount);
                QueryHelper.AddParameterWithNullableValue(cmd, "CompositionStatus", certificateMark.CompositionStatus);
                QueryHelper.AddParameterWithNullableValue(cmd, "CompositionPaths", certificateMark.CompositionPathsStr);
            }

            public static void InsertCancelledCertificateQuery(Tables tables, SqlCommand cmd, ERBDCancelledCertificate cancelledCertificate)
            {
                cmd.CommandText = @"
INSERT INTO [prn].[CancelledCertificates?]
           ([UseYear]
           ,[REGION]
           ,[CertificateFK]
           ,[Reason])
     VALUES
           (@UseYear
           ,@REGION
           ,@CertificateFK
           ,@Reason)
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", cancelledCertificate.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", cancelledCertificate.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateFK", cancelledCertificate.Id.CertificateID);
                QueryHelper.AddParameterWithNullableValue(cmd, "Reason", cancelledCertificate.ReasonStr);
            }

            public static void UpdateParticipantQuery(Tables tables, SqlCommand cmd, ERBDParticipant participant)
            {
                cmd.CommandText = @"
UPDATE [rbd].[Participants?]
   SET [ParticipantCode] = @ParticipantCode
      ,[Surname] = @Surname
      ,[Name] = @Name
      ,[SecondName] = @SecondName
      ,[DocumentSeries] = @DocumentSeries
      ,[DocumentNumber] = @DocumentNumber
      ,[DocumentTypeCode] = @DocumentTypeCode
      ,[Sex] = @Sex
      ,[BirthDay] = @BirthDay
      ,[FinishRegion] = @FinishRegion
      ,[ParticipantCategoryFK] = @ParticipantCategoryFK 
      ,[UpdateDate] = @UpdateDate 
      ,[ImportUpdateDate] = @ImportUpdateDate
      ,[TestTypeID] = @TestTypeID
      ,[SurnameTrimmed] = @SurnameTrimmed
      ,[NameTrimmed] = @NameTrimmed
      ,[SecondNameTrimmed] = @SecondNameTrimmed
WHERE 
      [ParticipantID] = @ParticipantID
      and [UseYear] = @UseYear
      and [REGION] = @REGION
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantID", participant.Id.ParticipantID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", participant.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", participant.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantCode", participant.ParticipantCodeStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "Surname", participant.SurnameStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "Name", participant.NameStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "SecondName", participant.SecondNameStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentSeries", participant.DocumentSeriesStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentNumber", participant.DocumentNumberStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentTypeCode", participant.DocumentTypeCode);
                QueryHelper.AddParameterWithNullableValue(cmd, "Sex", participant.Sex);
                QueryHelper.AddParameterWithNullableValue(cmd, "BirthDay", participant.BirthDay);
                QueryHelper.AddParameterWithNullableValue(cmd, "FinishRegion", participant.FinishRegion);
                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantCategoryFK", participant.ParticipantCategoryFK);
                QueryHelper.AddParameterWithNullableValue(cmd, "UpdateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "ImportUpdateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "TestTypeID", participant.TestTypeID);
                QueryHelper.AddParameterWithNullableValue(cmd, "SurnameTrimmed", participant.SurnameTrimmedStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "NameTrimmed", participant.NameTrimmedStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "SecondNameTrimmed", participant.SecondNameTrimmedStr);
            }

            public static void UpdateToHiddenParticipantQuery(Tables tables, SqlCommand cmd, ParticipantId id)
            {
                cmd.CommandText = @"
UPDATE [rbd].[Participants?]
   SET [Surname] = @Surname
      ,[Name] = @Name
      ,[SecondName] = @SecondName
      ,[SurnameTrimmed] = @SurnameTrimmed
      ,[NameTrimmed] = @NameTrimmed
      ,[SecondNameTrimmed] = @SecondNameTrimmed
WHERE 
      [ParticipantID] = @ParticipantID
      and [UseYear] = @UseYear
      and [REGION] = @REGION
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantID", id.ParticipantID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "Surname", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "Name", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "SecondName", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "SurnameTrimmed", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "NameTrimmed", Guid.Empty.ToString());
                QueryHelper.AddParameterWithNullableValue(cmd, "SecondNameTrimmed", Guid.Empty.ToString());
            }

            public static void UpdateCertificateQuery(Tables tables, SqlCommand cmd, ERBDCertificate certificate)
            {
                cmd.CommandText = @"
UPDATE [prn].[Certificates?]
   SET [Wave] = @Wave
      ,[LicenseNumber] = @LicenseNumber
      ,[TypographicNumber] = @TypographicNumber
      ,[ParticipantFK] = @ParticipantFK
      ,[Cancelled] = @Cancelled 
      ,[UpdateDate] = @UpdateDate 
      ,[ImportUpdateDate] = @ImportUpdateDate
 WHERE
      [CertificateID] = @CertificateID
      and [UseYear] = @UseYear
      and [REGION] = @REGION
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateID", certificate.Id.CertificateID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", certificate.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", certificate.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "Wave", certificate.Wave);
                QueryHelper.AddParameterWithNullableValue(cmd, "LicenseNumber", certificate.LicenseNumberStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "TypographicNumber", certificate.TypographicNumberStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantFK", certificate.ParticipantFK);
                QueryHelper.AddParameterWithNullableValue(cmd, "Cancelled", certificate.Cancelled);
                QueryHelper.AddParameterWithNullableValue(cmd, "UpdateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "ImportUpdateDate", DateTime.Now);
            }

            public static void UpdateCertificateMarkQuery(Tables tables, SqlCommand cmd, ERBDCertificateMark certificateMark)
            {
                cmd.CommandText = @"
UPDATE [prn].[CertificatesMarks?]
   SET 
      [ParticipantFK] = @ParticipantFK
      ,[SubjectCode] = @SubjectCode
      ,[Mark] = @Mark
      ,[HasAppeal] = @HasAppeal
      ,[PrintedMarkID] = @PrintedMarkID
      ,[TestTypeID] = @TestTypeID
      ,[ProcessCondition] = @ProcessCondition
      ,[ExamDate] = @ExamDate
      ,[CompositionBarcode] = @CompositionBarcode
      ,[CompositionPagesCount] = @CompositionPagesCount
      ,[CompositionStatus] = @CompositionStatus
      ,[CompositionPaths] = @CompositionPaths
 WHERE 
	  [CertificateMarkID] = @CertificateMarkID
      and [UseYear] = @UseYear
      and [REGION] = @REGION
      and [CertificateFK] = @CertificateFK
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateMarkID", certificateMark.Id.CertificateMarkID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", certificateMark.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", certificateMark.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateFK", certificateMark.Id.CertificateFK);
                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantFK", certificateMark.ParticipantFK);
                QueryHelper.AddParameterWithNullableValue(cmd, "SubjectCode", certificateMark.SubjectCode);
                QueryHelper.AddParameterWithNullableValue(cmd, "Mark", certificateMark.Mark);
                QueryHelper.AddParameterWithNullableValue(cmd, "HasAppeal", certificateMark.HasAppeal);
                QueryHelper.AddParameterWithNullableValue(cmd, "PrintedMarkID", certificateMark.PrintedMarkID);
                QueryHelper.AddParameterWithNullableValue(cmd, "TestTypeID", certificateMark.TestTypeID);
                QueryHelper.AddParameterWithNullableValue(cmd, "ProcessCondition", certificateMark.ProcessCondition);
                QueryHelper.AddParameterWithNullableValue(cmd, "ExamDate", certificateMark.ExamDate);
                QueryHelper.AddParameterWithNullableValue(cmd, "CompositionBarcode", certificateMark.CompositionBarcodeStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "CompositionPagesCount", certificateMark.CompositionPagesCount);
                QueryHelper.AddParameterWithNullableValue(cmd, "CompositionStatus", certificateMark.CompositionStatus);
                QueryHelper.AddParameterWithNullableValue(cmd, "CompositionPaths", certificateMark.CompositionPathsStr);
            }

            public static void UpdateCancelledCertificateQuery(Tables tables, SqlCommand cmd, ERBDCancelledCertificate cancelledCertificate)
            {
                cmd.CommandText = @"
UPDATE [prn].[CancelledCertificates?]
   SET [Reason] = @Reason
 WHERE
      [UseYear] = @UseYear
      and [REGION] = @REGION
      and [CertificateFK] = @CertificateFK
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", cancelledCertificate.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", cancelledCertificate.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "CertificateFK", cancelledCertificate.Id.CertificateID);
                QueryHelper.AddParameterWithNullableValue(cmd, "Reason", cancelledCertificate.ReasonStr);
            }

            public static void LinkPersonQuery(Tables tables, SqlCommand cmd, FBSPerson person)
            {
                cmd.CommandText = @"
UPDATE [rbd].[Participants?]
   SET [PersonId] = @PersonId
      ,[PersonLinkDate] = @PersonLinkDate
WHERE 
      [ParticipantID] = @ParticipantID
      and [UseYear] = @UseYear
      and [REGION] = @REGION
".Replace("?", TablesHelper.ToString(tables));

                QueryHelper.AddParameterWithNullableValue(cmd, "ParticipantID", person.Id.ParticipantID);
                QueryHelper.AddParameterWithNullableValue(cmd, "UseYear", person.Id.UseYear);
                QueryHelper.AddParameterWithNullableValue(cmd, "REGION", person.Id.REGION);
                QueryHelper.AddParameterWithNullableValue(cmd, "PersonId", person.PersonId.Value);
                QueryHelper.AddParameterWithNullableValue(cmd, "PersonLinkDate", DateTime.Now);
            }
        }

        public static class Write_GVUZ
        {
            public static void InsertPersonQuery(SqlCommand cmd, FBSPerson person)
            {
                cmd.CommandText = @"
    INSERT INTO [dbo].[RVIPersons]
           ([PersonId]
           ,[IsRecordDeleted]
           ,[NormSurname]
           ,[NormName]
           ,[NormSecondName]
           ,[BirthDay]
           ,[Sex]
           ,[CreateDate]
           ,[UpdateDate]
           ,[IntegralUpdateDate])
     VALUES
           (@PersonId
           ,@IsRecordDeleted
           ,@NormSurname
           ,@NormName
           ,@NormSecondName
           ,@BirthDay
           ,@Sex
           ,@CreateDate
           ,@UpdateDate
           ,@IntegralUpdateDate)
";

                QueryHelper.AddParameterWithNullableValue(cmd, "PersonId", person.PersonId);
                QueryHelper.AddParameterWithNullableValue(cmd, "IsRecordDeleted", false);
                QueryHelper.AddParameterWithNullableValue(cmd, "NormSurname", person.NormSurnameStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "NormName", person.NormNameStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "NormSecondName", person.NormSecondNameStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "BirthDay", person.BirthDay);
                QueryHelper.AddParameterWithNullableValue(cmd, "Sex", person.Sex);
                QueryHelper.AddParameterWithNullableValue(cmd, "CreateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "UpdateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "IntegralUpdateDate", DateTime.Now);
            }

            public static void UpdatePersonQuery(SqlCommand cmd, FBSPerson person)
            {
                cmd.CommandText = @"
UPDATE [dbo].[RVIPersons]
   SET [BirthDay] = @BirthDay
        ,[Sex] = @Sex
        ,[UpdateDate] = @UpdateDate
        ,[IntegralUpdateDate] = @IntegralUpdateDate
 WHERE
      [PersonId] = @PersonId 
";

                QueryHelper.AddParameterWithNullableValue(cmd, "PersonId", person.PersonId);
                QueryHelper.AddParameterWithNullableValue(cmd, "BirthDay", person.BirthDay);
                QueryHelper.AddParameterWithNullableValue(cmd, "Sex", person.Sex);
                QueryHelper.AddParameterWithNullableValue(cmd, "UpdateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "IntegralUpdateDate", DateTime.Now);
            }

            public static void InsertDocumentQuery(SqlCommand cmd, FBSIdentityDocument document, int documentId)
            {
                cmd.CommandText = @"
INSERT INTO [dbo].[RVIPersonIdentDocs]
           ([PersonIdentDocID]
           ,[PersonId]
           ,[DocumentTypeCode]
           ,[DocumentSeries]
           ,[DocumentNumber]
           ,[CreateDate]
           ,[UpdateDate])
     VALUES
           (@PersonIdentDocID 
           ,@PersonId 
           ,@DocumentTypeCode 
           ,@DocumentSeries 
           ,@DocumentNumber 
           ,@CreateDate 
           ,@UpdateDate)
";

                QueryHelper.AddParameterWithNullableValue(cmd, "PersonIdentDocID", documentId);
                QueryHelper.AddParameterWithNullableValue(cmd, "PersonId", document.Person.PersonId);
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentTypeCode", document.RVIDocumentTypeCode);
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentSeries", document.DocumentSeriesStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "DocumentNumber", document.DocumentNumberStr);
                QueryHelper.AddParameterWithNullableValue(cmd, "CreateDate", DateTime.Now);
                QueryHelper.AddParameterWithNullableValue(cmd, "UpdateDate", DateTime.Now);
            }
        }

        public static class QueryHelper
        {
            public static void AddParameterWithNullableValue(SqlCommand cmd, string paramenterName, object value)
            {
                if (value == null)
                {
                    cmd.Parameters.AddWithValue(paramenterName, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(paramenterName, value);
                }
            }
        }
    }
}