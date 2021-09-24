-- =========================================================================
-- Запись информации о текущей миграции в лог
INSERT  INTO Migrations
        (
          MigrationVersion,
          MigrationName
        )
VALUES  (
          42,
          '042__2012_06_07_AddReplication'
        )
-- =========================================================================
GO




GO
PRINT N'Altering [dbo].[OrganizationRequestAccount]...' ;


GO
ALTER TABLE [dbo].[OrganizationRequestAccount]
ADD [Id] BIGINT IDENTITY(1, 1) NOT FOR REPLICATION
                NOT NULL ;


GO
PRINT N'Creating [dbo].[OrganizationRequestAccount].[IX_OrganizationRequestAccount]...' ;


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationRequestAccount] ON [dbo].[OrganizationRequestAccount] ( [Id] ASC )
    WITH (
         ALLOW_PAGE_LOCKS = ON,
         ALLOW_ROW_LOCKS = ON,
         PAD_INDEX = OFF,
         SORT_IN_TEMPDB = OFF,
         DROP_EXISTING = OFF,
         IGNORE_DUP_KEY = OFF,
         STATISTICS_NORECOMPUTE = OFF,
         ONLINE = OFF,
         MAXDOP = 0)
ON  [PRIMARY] ;


GO
PRINT N'Altering [dbo].[UserAccountPassword]...' ;


GO
ALTER TABLE [dbo].[UserAccountPassword]
ADD [Id] BIGINT IDENTITY(1, 1) NOT FOR REPLICATION
                NOT NULL ;


GO
PRINT N'Creating [dbo].[UserAccountPassword].[IX_UserAccountPassword]...' ;


GO
CREATE NONCLUSTERED INDEX [IX_UserAccountPassword] ON [dbo].[UserAccountPassword] ( [Id] ASC )
    WITH (
         ALLOW_PAGE_LOCKS = ON,
         ALLOW_ROW_LOCKS = ON,
         PAD_INDEX = OFF,
         SORT_IN_TEMPDB = OFF,
         DROP_EXISTING = OFF,
         IGNORE_DUP_KEY = OFF,
         STATISTICS_NORECOMPUTE = OFF,
         ONLINE = OFF,
         MAXDOP = 0)
ON  [PRIMARY] ;


GO
PRINT N'Creating [dbo].[Account_repl]...' ;


GO
CREATE TABLE [dbo].[Account_repl]
    (
      [Id] BIGINT NOT NULL,
      [Done] BIT NOT NULL,
      [Type] SMALLINT NOT NULL,
      [Updated] TIMESTAMP NOT NULL,
      [Date] DATETIME NULL
    ) ;


GO
PRINT N'Creating PK_Account_repl...' ;


GO
ALTER TABLE [dbo].[Account_repl]
ADD CONSTRAINT [PK_Account_repl] PRIMARY KEY CLUSTERED ( [Id] ASC, [Updated] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating [dbo].[Group_repl]...' ;


GO
CREATE TABLE [dbo].[Group_repl]
    (
      [Id] BIGINT NOT NULL,
      [Done] BIT NOT NULL,
      [Type] SMALLINT NOT NULL,
      [Updated] TIMESTAMP NOT NULL,
      [Date] DATETIME NULL
    ) ;


GO
PRINT N'Creating PK_Group_repl...' ;


GO
ALTER TABLE [dbo].[Group_repl]
ADD CONSTRAINT [PK_Group_repl] PRIMARY KEY CLUSTERED ( [Id] ASC, [Updated] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating [dbo].[GroupAccount_repl]...' ;


GO
CREATE TABLE [dbo].[GroupAccount_repl]
    (
      [Id] BIGINT NOT NULL,
      [Done] BIT NOT NULL,
      [Type] SMALLINT NOT NULL,
      [Updated] TIMESTAMP NOT NULL,
      [Date] DATETIME NULL
    ) ;


GO
PRINT N'Creating PK_GroupAccount_repl...' ;


GO
ALTER TABLE [dbo].[GroupAccount_repl]
ADD CONSTRAINT [PK_GroupAccount_repl] PRIMARY KEY CLUSTERED ( [Id] ASC, [Updated] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating [dbo].[GroupRole_repl]...' ;


GO
CREATE TABLE [dbo].[GroupRole_repl]
    (
      [Id] BIGINT NOT NULL,
      [Done] BIT NOT NULL,
      [Type] SMALLINT NOT NULL,
      [Updated] TIMESTAMP NOT NULL,
      [Date] DATETIME NULL
    ) ;


GO
PRINT N'Creating PK_GroupRole_repl...' ;


GO
ALTER TABLE [dbo].[GroupRole_repl]
ADD CONSTRAINT [PK_GroupRole_repl] PRIMARY KEY CLUSTERED ( [Id] ASC, [Updated] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating [dbo].[Organization2010_repl]...' ;


GO
CREATE TABLE [dbo].[Organization2010_repl]
    (
      [Id] BIGINT NOT NULL,
      [Done] BIT NOT NULL,
      [Type] SMALLINT NOT NULL,
      [Updated] TIMESTAMP NOT NULL,
      [Date] DATETIME NULL
    ) ;


GO
PRINT N'Creating PK_Organization2010_repl...' ;


GO
ALTER TABLE [dbo].[Organization2010_repl]
ADD CONSTRAINT [PK_Organization2010_repl] PRIMARY KEY CLUSTERED ( [Id] ASC, [Updated] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating [dbo].[OrganizationRequest2010_repl]...' ;


GO
CREATE TABLE [dbo].[OrganizationRequest2010_repl]
    (
      [Id] BIGINT NOT NULL,
      [Done] BIT NOT NULL,
      [Type] SMALLINT NOT NULL,
      [Updated] TIMESTAMP NOT NULL,
      [Date] DATETIME NULL
    ) ;


GO
PRINT N'Creating PK_OrganizationRequest2010_repl...' ;


GO
ALTER TABLE [dbo].[OrganizationRequest2010_repl]
ADD CONSTRAINT [PK_OrganizationRequest2010_repl] PRIMARY KEY CLUSTERED ( [Id] ASC, [Updated] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating [dbo].[OrganizationRequestAccount_repl]...' ;


GO
CREATE TABLE [dbo].[OrganizationRequestAccount_repl]
    (
      [Id] BIGINT NOT NULL,
      [Done] BIT NOT NULL,
      [Type] SMALLINT NOT NULL,
      [Updated] TIMESTAMP NOT NULL,
      [Date] DATETIME NULL
    ) ;


GO
PRINT N'Creating PK_OrganizationRequestAccount_repl...' ;


GO
ALTER TABLE [dbo].[OrganizationRequestAccount_repl]
ADD CONSTRAINT [PK_OrganizationRequestAccount_repl] PRIMARY KEY CLUSTERED ( [Id] ASC, [Updated] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating [dbo].[repl_Tables]...' ;


GO
CREATE TABLE [dbo].[repl_Tables]
    (
      [tablename] VARCHAR(150) NULL,
      [getbatch] VARCHAR(150) NULL,
      [repldone] VARCHAR(150) NULL,
      [so] INT NULL
    ) ;


GO
PRINT N'Creating [dbo].[Settings]...' ;


GO
CREATE TABLE [dbo].[Settings]
    (
      [name] NVARCHAR(50) NOT NULL,
      [value] NVARCHAR(250) NULL
    ) ;


GO
PRINT N'Creating PK_Settings...' ;


GO
ALTER TABLE [dbo].[Settings]
ADD CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED ( [name] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating [dbo].[UserAccountPassword_repl]...' ;


GO
CREATE TABLE [dbo].[UserAccountPassword_repl]
    (
      [Id] BIGINT NOT NULL,
      [Done] BIT NOT NULL,
      [Type] SMALLINT NOT NULL,
      [Updated] TIMESTAMP NOT NULL,
      [Date] DATETIME NULL
    ) ;


GO
PRINT N'Creating PK_UserAccountPassword_repl...' ;


GO
ALTER TABLE [dbo].[UserAccountPassword_repl]
ADD CONSTRAINT [PK_UserAccountPassword_repl] PRIMARY KEY CLUSTERED ( [Id] ASC, [Updated] ASC )
        WITH ( ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF,
               IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF ) ;


GO
PRINT N'Creating DF__Account_r__replD__060B4A8C...' ;


GO
ALTER TABLE [dbo].[Account_repl]
ADD CONSTRAINT [DF__Account_r__replD__060B4A8C] DEFAULT ( (0) ) FOR [Done] ;


GO
PRINT N'Creating DF_Account_repl_Date...' ;


GO
ALTER TABLE [dbo].[Account_repl]
ADD CONSTRAINT [DF_Account_repl_Date] DEFAULT ( GETDATE() ) FOR [Date] ;


GO
PRINT N'Creating DF__Group_r__replD__060B4A8C...' ;


GO
ALTER TABLE [dbo].[Group_repl]
ADD CONSTRAINT [DF__Group_r__replD__060B4A8C] DEFAULT ( (0) ) FOR [Done] ;


GO
PRINT N'Creating DF_Group_repl_Date...' ;


GO
ALTER TABLE [dbo].[Group_repl]
ADD CONSTRAINT [DF_Group_repl_Date] DEFAULT ( GETDATE() ) FOR [Date] ;


GO
PRINT N'Creating DF__GroupAccount_r__replD__060B4A8C...' ;


GO
ALTER TABLE [dbo].[GroupAccount_repl]
ADD CONSTRAINT [DF__GroupAccount_r__replD__060B4A8C] DEFAULT ( (0) ) FOR [Done] ;


GO
PRINT N'Creating DF_GroupAccount_repl_Date...' ;


GO
ALTER TABLE [dbo].[GroupAccount_repl]
ADD CONSTRAINT [DF_GroupAccount_repl_Date] DEFAULT ( GETDATE() ) FOR [Date] ;


GO
PRINT N'Creating DF__GroupRole_r__replD__060B4A8C...' ;


GO
ALTER TABLE [dbo].[GroupRole_repl]
ADD CONSTRAINT [DF__GroupRole_r__replD__060B4A8C] DEFAULT ( (0) ) FOR [Done] ;


GO
PRINT N'Creating DF_GroupRole_repl_Date...' ;


GO
ALTER TABLE [dbo].[GroupRole_repl]
ADD CONSTRAINT [DF_GroupRole_repl_Date] DEFAULT ( GETDATE() ) FOR [Date] ;


GO
PRINT N'Creating DF__Organization2010_r__replD__060B4A8C...' ;


GO
ALTER TABLE [dbo].[Organization2010_repl]
ADD CONSTRAINT [DF__Organization2010_r__replD__060B4A8C] DEFAULT ( (0) ) FOR [Done] ;


GO
PRINT N'Creating DF_Organization2010_repl_Date...' ;


GO
ALTER TABLE [dbo].[Organization2010_repl]
ADD CONSTRAINT [DF_Organization2010_repl_Date] DEFAULT ( GETDATE() ) FOR [Date] ;


GO
PRINT N'Creating DF__OrganizationRequest2010_r__replD__060B4A8C...' ;


GO
ALTER TABLE [dbo].[OrganizationRequest2010_repl]
ADD CONSTRAINT [DF__OrganizationRequest2010_r__replD__060B4A8C] DEFAULT ( (0) )
        FOR [Done] ;


GO
PRINT N'Creating DF_OrganizationRequest2010_repl_Date...' ;


GO
ALTER TABLE [dbo].[OrganizationRequest2010_repl]
ADD CONSTRAINT [DF_OrganizationRequest2010_repl_Date] DEFAULT ( GETDATE() )
        FOR [Date] ;


GO
PRINT N'Creating DF__OrganizationRequestAccount_r__replD__060B4A8C...' ;


GO
ALTER TABLE [dbo].[OrganizationRequestAccount_repl]
ADD CONSTRAINT [DF__OrganizationRequestAccount_r__replD__060B4A8C] DEFAULT ( (0) )
        FOR [Done] ;


GO
PRINT N'Creating DF_OrganizationRequestAccount_repl_Date...' ;


GO
ALTER TABLE [dbo].[OrganizationRequestAccount_repl]
ADD CONSTRAINT [DF_OrganizationRequestAccount_repl_Date] DEFAULT ( GETDATE() )
        FOR [Date] ;


GO
PRINT N'Creating DF__UserAccountPassword_r__replD__060B4A8C...' ;


GO
ALTER TABLE [dbo].[UserAccountPassword_repl]
ADD CONSTRAINT [DF__UserAccountPassword_r__replD__060B4A8C] DEFAULT ( (0) )
        FOR [Done] ;


GO
PRINT N'Creating DF_UserAccountPassword_repl_Date...' ;


GO
ALTER TABLE [dbo].[UserAccountPassword_repl]
ADD CONSTRAINT [DF_UserAccountPassword_repl_Date] DEFAULT ( GETDATE() ) FOR [Date] ;


GO
PRINT N'Creating [dbo].[tg_AccountReplication_del]...' ;


GO

CREATE TRIGGER [dbo].[tg_AccountReplication_del] ON [dbo].[Account]
    FOR DELETE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Account_repl ( Id, Type )
            SELECT  Id,
                    3
            FROM    deleted
GO
PRINT N'Creating [dbo].[tg_AccountReplication_ins]...' ;


GO

CREATE TRIGGER [dbo].[tg_AccountReplication_ins] ON [dbo].[Account]
    FOR INSERT
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Account_repl ( Id, Type )
            SELECT  Id,
                    1
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_AccountReplication_upd]...' ;


GO

CREATE TRIGGER [dbo].[tg_AccountReplication_upd] ON [dbo].[Account]
    FOR UPDATE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Account_repl ( Id, Type )
            SELECT  Id,
                    2
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_GroupReplication_del]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupReplication_del] ON [dbo].[Group]
    FOR DELETE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Group_repl ( Id, Type )
            SELECT  Id,
                    3
            FROM    deleted
GO
PRINT N'Creating [dbo].[tg_GroupReplication_ins]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupReplication_ins] ON [dbo].[Group]
    FOR INSERT
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Group_repl ( Id, Type )
            SELECT  Id,
                    1
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_GroupReplication_upd]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupReplication_upd] ON [dbo].[Group]
    FOR UPDATE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Group_repl ( Id, Type )
            SELECT  Id,
                    2
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_GroupAccountReplication_del]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupAccountReplication_del] ON [dbo].[GroupAccount]
    FOR DELETE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  GroupAccount_repl ( Id, Type )
            SELECT  Id,
                    3
            FROM    deleted
GO
PRINT N'Creating [dbo].[tg_GroupAccountReplication_ins]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupAccountReplication_ins] ON [dbo].[GroupAccount]
    FOR INSERT
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  GroupAccount_repl ( Id, Type )
            SELECT  Id,
                    1
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_GroupAccountReplication_upd]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupAccountReplication_upd] ON [dbo].[GroupAccount]
    FOR UPDATE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  GroupAccount_repl ( Id, Type )
            SELECT  Id,
                    2
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_GroupRoleReplication_del]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupRoleReplication_del] ON [dbo].[GroupRole]
    FOR DELETE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  GroupRole_repl ( Id, Type )
            SELECT  Id,
                    3
            FROM    deleted
GO
PRINT N'Creating [dbo].[tg_GroupRoleReplication_ins]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupRoleReplication_ins] ON [dbo].[GroupRole]
    FOR INSERT
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  GroupRole_repl ( Id, Type )
            SELECT  Id,
                    1
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_GroupRoleReplication_upd]...' ;


GO


CREATE TRIGGER [dbo].[tg_GroupRoleReplication_upd] ON [dbo].[GroupRole]
    FOR UPDATE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  GroupRole_repl ( Id, Type )
            SELECT  Id,
                    2
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_Organization2010Replication_del]...' ;


GO

CREATE TRIGGER [dbo].[tg_Organization2010Replication_del] ON [dbo].[Organization2010]
    FOR DELETE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Organization2010_repl ( Id, Type )
            SELECT  Id,
                    3
            FROM    deleted
GO
PRINT N'Creating [dbo].[tg_Organization2010Replication_ins]...' ;


GO

CREATE TRIGGER [dbo].[tg_Organization2010Replication_ins] ON [dbo].[Organization2010]
    FOR INSERT
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Organization2010_repl ( Id, Type )
            SELECT  Id,
                    1
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_Organization2010Replication_upd]...' ;


GO

CREATE TRIGGER [dbo].[tg_Organization2010Replication_upd] ON [dbo].[Organization2010]
    FOR UPDATE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  Organization2010_repl ( Id, Type )
            SELECT  Id,
                    2
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_OrganizationRequest2010Replication_del]...' ;


GO

CREATE TRIGGER [dbo].[tg_OrganizationRequest2010Replication_del] ON [dbo].[OrganizationRequest2010]
    FOR DELETE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  OrganizationRequest2010_repl ( Id, Type )
            SELECT  Id,
                    3
            FROM    deleted
GO
PRINT N'Creating [dbo].[tg_OrganizationRequest2010Replication_ins]...' ;


GO

CREATE TRIGGER [dbo].[tg_OrganizationRequest2010Replication_ins] ON [dbo].[OrganizationRequest2010]
    FOR INSERT
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  OrganizationRequest2010_repl ( Id, Type )
            SELECT  Id,
                    1
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_OrganizationRequest2010Replication_upd]...' ;


GO

CREATE TRIGGER [dbo].[tg_OrganizationRequest2010Replication_upd] ON [dbo].[OrganizationRequest2010]
    FOR UPDATE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  OrganizationRequest2010_repl ( Id, Type )
            SELECT  Id,
                    2
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_OrganizationRequestAccountReplication_del]...' ;


GO

CREATE TRIGGER [dbo].[tg_OrganizationRequestAccountReplication_del] ON [dbo].[OrganizationRequestAccount]
    FOR DELETE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  OrganizationRequestAccount_repl ( Id, Type )
            SELECT  Id,
                    3
            FROM    deleted
GO
PRINT N'Creating [dbo].[tg_OrganizationRequestAccountReplication_ins]...' ;


GO

CREATE TRIGGER [dbo].[tg_OrganizationRequestAccountReplication_ins] ON [dbo].[OrganizationRequestAccount]
    FOR INSERT
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  OrganizationRequestAccount_repl ( Id, Type )
            SELECT  Id,
                    1
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_OrganizationRequestAccountReplication_upd]...' ;


GO

CREATE TRIGGER [dbo].[tg_OrganizationRequestAccountReplication_upd] ON [dbo].[OrganizationRequestAccount]
    FOR UPDATE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  OrganizationRequestAccount_repl ( Id, Type )
            SELECT  Id,
                    2
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_UserAccountPasswordReplication_del]...' ;


GO

CREATE TRIGGER [dbo].[tg_UserAccountPasswordReplication_del] ON [dbo].[UserAccountPassword]
    FOR DELETE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  dbo.UserAccountPassword_repl ( Id, Type )
            SELECT  Id,
                    3
            FROM    deleted
GO
PRINT N'Creating [dbo].[tg_UserAccountPasswordReplication_ins]...' ;


GO

CREATE TRIGGER [dbo].[tg_UserAccountPasswordReplication_ins] ON [dbo].[UserAccountPassword]
    FOR INSERT
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  dbo.UserAccountPassword_repl ( Id, Type )
            SELECT  Id,
                    1
            FROM    inserted
GO
PRINT N'Creating [dbo].[tg_UserAccountPasswordReplication_upd]...' ;


GO
CREATE TRIGGER [dbo].[tg_UserAccountPasswordReplication_upd] ON [dbo].[UserAccountPassword]
    FOR UPDATE
AS
    SET nocount ON
    IF ISNULL(( SELECT  value
                FROM    settings
                WHERE   name = 'IsOpenSystem'
              ), '') <> 'yes' 
        RETURN
    INSERT  dbo.UserAccountPassword_repl ( Id, Type )
            SELECT  Id,
                    2
            FROM    inserted
GO
PRINT N'Creating [dbo].[usp_repl_AccountApplyBatch]...' ;
GO
/*

*/
CREATE PROC [dbo].[usp_repl_AccountApplyBatch] @xml XML, @type INT
AS 
    SET nocount ON
    IF @type = 1 
        BEGIN
            SET IDENTITY_INSERT dbo.Account ON 
            INSERT  dbo.Account
                    (
                      Id,
                      CreateDate,
                      UpdateDate,
                      UpdateId,
                      EditorAccountId,
                      EditorIp,
                      Login,
                      PasswordHash,
                      LastName,
                      FirstName,
                      PatronymicName,
                      OrganizationId,
                      IsOrganizationOwner,
                      ConfirmYear,
                      Phone,
                      Email,
--RegistrationDocument,
                      RegistrationDocumentContentType,
                      AdminComment,
                      IsActive,
                      Status,
                      IpAddresses,
                      HasFixedIp,
                      Position
                    )
                    SELECT  item.ref.value('@Id', 'bigint') AS Id,
                            item.ref.value('@CreateDate', 'datetime') AS CreateDate,
                            item.ref.value('@UpdateDate', 'datetime') AS UpdateDate,
                            item.ref.value('@UpdateId', 'uniqueidentifier') AS UpdateId,
                            item.ref.value('@EditorAccountId', 'bigint') AS EditorAccountId,
                            item.ref.value('@EditorIp', 'nvarchar(255)') AS EditorIp,
                            item.ref.value('@Login', 'nvarchar(255)') AS Login,
                            item.ref.value('@PasswordHash', 'nvarchar(255)') AS PasswordHash,
                            item.ref.value('@LastName', 'nvarchar(255)') AS LastName,
                            item.ref.value('@FirstName', 'nvarchar(255)') AS FirstName,
                            item.ref.value('@PatronymicName', 'nvarchar(255)') AS PatronymicName,
                            item.ref.value('@OrganizationId', 'bigint') AS OrganizationId,
                            item.ref.value('@IsOrganizationOwner', 'bit') AS IsOrganizationOwner,
                            item.ref.value('@ConfirmYear', 'int') AS ConfirmYear,
                            item.ref.value('@Phone', 'nvarchar(255)') AS Phone,
                            item.ref.value('@Email', 'nvarchar(255)') AS Email,
--item.ref.value('@RegistrationDocument', 'nvarchar(255)') as RegistrationDocument,
                            item.ref.value('@RegistrationDocumentContentType',
                                           'nvarchar(255)') AS RegistrationDocumentContentType,
                            item.ref.value('@AdminComment', 'nvarchar(max)') AS AdminComment,
                            item.ref.value('@IsActive', 'nvarchar(255)') AS IsActive,
                            item.ref.value('@Status', 'nvarchar(255)') AS Status,
                            item.ref.value('@IpAddresses', 'nvarchar(4000)') AS IpAddresses,
                            item.ref.value('@HasFixedIp', 'bit') AS HasFixedIp,
                            item.ref.value('@Position', 'nvarchar(510)') AS Position
                    FROM    ( SELECT    @xml
                            ) feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
            SET IDENTITY_INSERT dbo.Account OFF
            RETURN
        END
    IF @type = 2 
        BEGIN
            UPDATE  a
            SET     CreateDate = tab.CreateDate,
                    UpdateDate = tab.UpdateDate,
                    UpdateId = tab.UpdateId,
                    EditorAccountId = tab.EditorAccountId,
                    EditorIp = tab.EditorIp,
                    Login = tab.Login,
                    PasswordHash = tab.PasswordHash,
                    LastName = tab.LastName,
                    FirstName = tab.FirstName,
                    PatronymicName = tab.PatronymicName,
                    OrganizationId = tab.OrganizationId,
                    IsOrganizationOwner = tab.IsOrganizationOwner,
                    ConfirmYear = tab.ConfirmYear,
                    Phone = tab.Phone,
                    Email = tab.Email,
--RegistrationDocument=tab.RegistrationDocument,
                    RegistrationDocumentContentType = tab.RegistrationDocumentContentType,
                    AdminComment = tab.AdminComment,
                    IsActive = tab.IsActive,
                    Status = tab.Status,
                    IpAddresses = tab.IpAddresses,
                    HasFixedIp = tab.HasFixedIp,
                    Position = tab.Position
            FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                                item.ref.value('@CreateDate', 'datetime') AS CreateDate,
                                item.ref.value('@UpdateDate', 'datetime') AS UpdateDate,
                                item.ref.value('@UpdateId', 'uniqueidentifier') AS UpdateId,
                                item.ref.value('@EditorAccountId', 'bigint') AS EditorAccountId,
                                item.ref.value('@EditorIp', 'nvarchar(255)') AS EditorIp,
                                item.ref.value('@Login', 'nvarchar(255)') AS Login,
                                item.ref.value('@PasswordHash',
                                               'nvarchar(255)') AS PasswordHash,
                                item.ref.value('@LastName', 'nvarchar(255)') AS LastName,
                                item.ref.value('@FirstName', 'nvarchar(255)') AS FirstName,
                                item.ref.value('@PatronymicName',
                                               'nvarchar(255)') AS PatronymicName,
                                item.ref.value('@OrganizationId', 'bigint') AS OrganizationId,
                                item.ref.value('@IsOrganizationOwner', 'bit') AS IsOrganizationOwner,
                                item.ref.value('@ConfirmYear', 'int') AS ConfirmYear,
                                item.ref.value('@Phone', 'nvarchar(255)') AS Phone,
                                item.ref.value('@Email', 'nvarchar(255)') AS Email,
--item.ref.value('@RegistrationDocument', 'nvarchar(255)') as RegistrationDocument,
                                item.ref.value('@RegistrationDocumentContentType',
                                               'nvarchar(255)') AS RegistrationDocumentContentType,
                                item.ref.value('@AdminComment',
                                               'nvarchar(max)') AS AdminComment,
                                item.ref.value('@IsActive', 'nvarchar(255)') AS IsActive,
                                item.ref.value('@Status', 'nvarchar(255)') AS Status,
                                item.ref.value('@IpAddresses',
                                               'nvarchar(4000)') AS IpAddresses,
                                item.ref.value('@HasFixedIp', 'bit') AS HasFixedIp,
                                item.ref.value('@Position', 'nvarchar(510)') AS Position
                      FROM      ( SELECT    @xml
                                ) feeds ( feedXml )
                                CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
                    ) tab
                    JOIN dbo.Account a ON a.id = tab.Id

            RETURN
        END

    IF @type = 3 
        BEGIN
            BEGIN TRY
                BEGIN TRAN
                DECLARE @t TABLE ( id BIGINT )
                INSERT  @t ( id )
                        SELECT  id
                        FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id
                                  FROM      ( SELECT    @xml
                                            ) feeds ( feedXml )
                                            CROSS APPLY feedXml.nodes('/rows/row')
                                            AS item ( ref )
                                ) tab

                DELETE  a
                FROM    @t tab
                        JOIN dbo.Account a ON a.id = tab.Id 
                IF @@TRANCOUNT > 0 
                    COMMIT
            END TRY
            BEGIN CATCH
                IF @@trancount > 0 
                    ROLLBACK
                DECLARE @msg NVARCHAR(4000)
                SET @msg = ERROR_MESSAGE()
                RAISERROR ( @msg, 16, 1 )
                RETURN -1
            END CATCH
        END


GO
/*

*/
PRINT N'Creating [dbo].[usp_repl_AccountGetBatch]...' ;
GO

CREATE PROC [dbo].[usp_repl_AccountGetBatch]
    @type INT = 3,
    @rowcount INT = 2,
    @xmlrange XML = NULL OUT,
    @xmlresult XML = NULL OUT
AS 
    SET nocount ON
    DECLARE @min TIMESTAMP,
        @max TIMESTAMP
    DECLARE @range TABLE
        (
          id BIGINT,
          umin VARBINARY(8),
          umax VARBINARY(8)
        )
    INSERT  @range
            SELECT TOP ( @rowcount )
                    id,
                    MIN(Updated) umin,
                    MAX(Updated) umax
            FROM    dbo.Account_repl
            WHERE   type = @type
                    AND done = 0
            GROUP BY id
    IF @@rowcount = 0 
        GOTO endp
    IF @type <> 3 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    b.Id AS '@Id',
                                                    b.CreateDate AS '@CreateDate',
                                                    b.UpdateDate AS '@UpdateDate',
                                                    b.UpdateId AS '@UpdateId',
                                                    b.EditorAccountId AS '@EditorAccountId',
                                                    b.EditorIp AS '@EditorIp',
                                                    b.Login AS '@Login',
                                                    b.PasswordHash AS '@PasswordHash',
                                                    b.LastName AS '@LastName',
                                                    b.FirstName AS '@FirstName',
                                                    b.PatronymicName AS '@PatronymicName',
                                                    b.OrganizationId AS '@OrganizationId',
                                                    b.IsOrganizationOwner AS '@IsOrganizationOwner',
                                                    b.ConfirmYear AS '@ConfirmYear',
                                                    b.Phone AS '@Phone',
                                                    b.Email AS '@Email',
--b.RegistrationDocument as '@RegistrationDocument',
                                                    b.RegistrationDocumentContentType AS '@RegistrationDocumentContentType',
                                                    b.AdminComment AS '@AdminComment',
                                                    b.IsActive AS '@IsActive',
                                                    b.Status AS '@Status',
                                                    b.IpAddresses AS '@IpAddresses',
                                                    b.HasFixedIp AS '@HasFixedIp',
                                                    b.Position AS '@Position'
                                          FROM      @range a
                                                    JOIN [dbo].[Account] b ON a.id = b.id
                                        FOR
                                          XML PATH('row'),
                                              ELEMENTS XSINIL,
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )

        END
    ELSE 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    ISNULL(a.Id, 0) AS '@Id'
                                          FROM      @range a
                                        FOR
                                          XML PATH('row'),
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )
        END

    SELECT  @xmlrange = ( SELECT    ( SELECT    Id AS '@Id',
                                                umin AS '@umin',
                                                umax AS '@umax'
                                      FROM      @range
                                    FOR
                                      XML PATH('range'),
                                          TYPE
                                    )
                        FOR
                          XML PATH('ranges'),
                              TYPE
                        )
    endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_AccountApplyBatch @xmlresult,3
--exec usp_repl_AccountApplyBatch @xmlresult,1
--exec dbo.usp_repl_AccountReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_AccountGetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/

GO
PRINT N'Creating [dbo].[usp_repl_AccountReplDone]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_AccountReplDone] @xmlrange XML
AS 
    SET nocount ON
--select a.*
    UPDATE  a
    SET     Done = 1
    FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                        item.ref.value('@umin', 'varbinary(8)') AS umin,
                        item.ref.value('@umax', 'varbinary(8)') AS umax
              FROM      ( SELECT    @xmlrange
                        ) feeds ( feedXml )
                        CROSS APPLY feedXml.nodes('/ranges/range') AS item ( ref )
            ) tab
            JOIN dbo.Account_repl a ON a.id = tab.Id
                                       AND a.Updated BETWEEN tab.umin AND tab.umax
GO
PRINT N'Creating [dbo].[usp_repl_GroupAccountApplyBatch]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_GroupAccountApplyBatch] @xml XML, @type INT
AS 
    SET nocount ON
    IF @type = 1 
        BEGIN
            SET IDENTITY_INSERT dbo.GroupAccount ON 
            INSERT  dbo.GroupAccount ( Id, GroupId, AccountId )
                    SELECT  item.ref.value('@Id', 'bigint') AS Id,
                            item.ref.value('@GroupId', 'int') AS GroupId,
                            item.ref.value('@AccountId', 'bigint') AS AccountId
                    FROM    ( SELECT    @xml
                            ) feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
            SET IDENTITY_INSERT dbo.GroupAccount OFF
            RETURN
        END
    IF @type = 2 
        BEGIN
            UPDATE  a
            SET     GroupId = tab.GroupId,
                    AccountId = tab.AccountId
            FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                                item.ref.value('@GroupId', 'int') AS GroupId,
                                item.ref.value('@AccountId', 'bigint') AS AccountId
                      FROM      ( SELECT    @xml
                                ) feeds ( feedXml )
                                CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
                    ) tab
                    JOIN dbo.GroupAccount a ON a.id = tab.Id

            RETURN
        END

    IF @type = 3 
        BEGIN
            BEGIN TRY
                BEGIN TRAN
                DECLARE @t TABLE ( id BIGINT )
                INSERT  @t ( id )
                        SELECT  id
                        FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id
                                  FROM      ( SELECT    @xml
                                            ) feeds ( feedXml )
                                            CROSS APPLY feedXml.nodes('/rows/row')
                                            AS item ( ref )
                                ) tab

                DELETE  a
                FROM    @t tab
                        JOIN dbo.GroupAccount a ON a.id = tab.Id 
                IF @@TRANCOUNT > 0 
                    COMMIT
            END TRY
            BEGIN CATCH
                IF @@trancount > 0 
                    ROLLBACK
                DECLARE @msg NVARCHAR(4000)
                SET @msg = ERROR_MESSAGE()
                RAISERROR ( @msg, 16, 1 )
                RETURN -1
            END CATCH
            RETURN
        END
GO
PRINT N'Creating [dbo].[usp_repl_GroupAccountGetBatch]...' ;


GO

CREATE PROC [dbo].[usp_repl_GroupAccountGetBatch]
    @type INT = 3,
    @rowcount INT = 2,
    @xmlrange XML = NULL OUT,
    @xmlresult XML = NULL OUT
AS 
    SET nocount ON
    DECLARE @min TIMESTAMP,
        @max TIMESTAMP
    DECLARE @range TABLE
        (
          id BIGINT,
          umin VARBINARY(8),
          umax VARBINARY(8)
        )
    INSERT  @range
            SELECT TOP ( @rowcount )
                    id,
                    MIN(Updated) umin,
                    MAX(Updated) umax
            FROM    dbo.GroupAccount_repl
            WHERE   type = @type
                    AND done = 0
            GROUP BY id

    IF @@rowcount = 0 
        GOTO endp
    IF @type <> 3 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    b.Id AS '@Id',
                                                    b.GroupId AS '@GroupId',
                                                    b.AccountId AS '@AccountId'
                                          FROM      @range a
                                                    JOIN [dbo].[GroupAccount] b ON a.id = b.id
                                        FOR
                                          XML PATH('row'),
                                              ELEMENTS XSINIL,
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )

        END
    ELSE 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    ISNULL(a.Id, 0) AS '@Id'
                                          FROM      @range a
                                        FOR
                                          XML PATH('row'),
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )
        END

    SELECT  @xmlrange = ( SELECT    ( SELECT    Id AS '@Id',
                                                umin AS '@umin',
                                                umax AS '@umax'
                                      FROM      @range
                                    FOR
                                      XML PATH('range'),
                                          TYPE
                                    )
                        FOR
                          XML PATH('ranges'),
                              TYPE
                        )
    endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_GroupAccountApplyBatch @xmlresult,3
--exec usp_repl_GroupAccountApplyBatch @xmlresult,2
--exec dbo.usp_repl_GroupAccountReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_GroupAccountGetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/
GO
PRINT N'Creating [dbo].[usp_repl_GroupAccountReplDone]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_GroupAccountReplDone] @xmlrange XML
AS 
    SET nocount ON
--select a.*
    UPDATE  a
    SET     Done = 1
    FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                        item.ref.value('@umin', 'varbinary(8)') AS umin,
                        item.ref.value('@umax', 'varbinary(8)') AS umax
              FROM      ( SELECT    @xmlrange
                        ) feeds ( feedXml )
                        CROSS APPLY feedXml.nodes('/ranges/range') AS item ( ref )
            ) tab
            JOIN dbo.GroupAccount_repl a ON a.id = tab.Id
                                            AND a.Updated BETWEEN tab.umin AND tab.umax
GO
PRINT N'Creating [dbo].[usp_repl_GroupApplyBatch]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_GroupApplyBatch] @xml XML, @type INT
AS 
    SET nocount ON
    IF @type = 1 
        BEGIN
            SET IDENTITY_INSERT dbo.[Group] ON 
            INSERT  dbo.[Group]
                    (
                      Id,
                      Code,
                      Name,
                      SystemID,
                      [Default],
                      IsUserIS
                    )
                    SELECT  item.ref.value('@Id', 'int') AS Id,
                            item.ref.value('@Code', 'nvarchar(255)') AS Code,
                            item.ref.value('@Name', 'nvarchar(255)') AS Name,
                            item.ref.value('@SystemID', 'int') AS SystemID,
                            item.ref.value('@Default', 'bit') AS [Default],
                            item.ref.value('@IsUserIS', 'bit') AS IsUserIS
                    FROM    ( SELECT    @xml
                            ) feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
            SET IDENTITY_INSERT dbo.[Group] OFF
            RETURN
        END
    IF @type = 2 
        BEGIN
            UPDATE  a
            SET     Code = tab.Code,
                    Name = tab.Name,
                    SystemID = tab.SystemID,
                    [Default] = tab.[Default],
                    IsUserIS = tab.IsUserIS
            FROM    ( SELECT    item.ref.value('@Id', 'int') AS Id,
                                item.ref.value('@Code', 'nvarchar(255)') AS Code,
                                item.ref.value('@Name', 'nvarchar(255)') AS Name,
                                item.ref.value('@SystemID', 'int') AS SystemID,
                                item.ref.value('@Default', 'bit') AS [Default],
                                item.ref.value('@IsUserIS', 'bit') AS IsUserIS
                      FROM      ( SELECT    @xml
                                ) feeds ( feedXml )
                                CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
                    ) tab
                    JOIN dbo.[Group] a ON a.id = tab.Id


            RETURN
        END

    IF @type = 3 
        BEGIN
            BEGIN TRY
                BEGIN TRAN
                DECLARE @t TABLE ( id BIGINT )
                INSERT  @t ( id )
                        SELECT  id
                        FROM    ( SELECT    item.ref.value('@Id', 'int') AS Id
                                  FROM      ( SELECT    @xml
                                            ) feeds ( feedXml )
                                            CROSS APPLY feedXml.nodes('/rows/row')
                                            AS item ( ref )
                                ) tab

                DELETE  a
                FROM    @t tab
                        JOIN dbo.[Group] a ON a.id = tab.Id 
                IF @@TRANCOUNT > 0 
                    COMMIT
            END TRY
            BEGIN CATCH
                IF @@trancount > 0 
                    ROLLBACK
                DECLARE @msg NVARCHAR(4000)
                SET @msg = ERROR_MESSAGE()
                RAISERROR ( @msg, 16, 1 )
                RETURN -1
            END CATCH
        END
GO
PRINT N'Creating [dbo].[usp_repl_GroupGetBatch]...' ;


GO

CREATE PROC [dbo].[usp_repl_GroupGetBatch]
    @type INT = 3,
    @rowcount INT = 2,
    @xmlrange XML = NULL OUT,
    @xmlresult XML = NULL OUT
AS 
    SET nocount ON
    DECLARE @min TIMESTAMP,
        @max TIMESTAMP
    DECLARE @range TABLE
        (
          id BIGINT,
          umin VARBINARY(8),
          umax VARBINARY(8)
        )
    INSERT  @range
            SELECT TOP ( @rowcount )
                    id,
                    MIN(Updated) umin,
                    MAX(Updated) umax
            FROM    dbo.Group_repl
            WHERE   type = @type
                    AND done = 0
            GROUP BY id
    IF @@rowcount = 0 
        GOTO endp
    IF @type <> 3 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    b.Id AS '@Id',
                                                    b.Code AS '@Code',
                                                    b.Name AS '@Name',
                                                    b.SystemID AS '@SystemID',
                                                    b.[Default] AS '@Default',
                                                    b.IsUserIS AS '@IsUserIS'
                                          FROM      @range a
                                                    JOIN [dbo].[Group] b ON a.id = b.id
                                        FOR
                                          XML PATH('row'),
                                              ELEMENTS XSINIL,
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )

        END
    ELSE 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    ISNULL(a.Id, 0) AS '@Id'
                                          FROM      @range a
                                        FOR
                                          XML PATH('row'),
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )
        END

    SELECT  @xmlrange = ( SELECT    ( SELECT    Id AS '@Id',
                                                umin AS '@umin',
                                                umax AS '@umax'
                                      FROM      @range
                                    FOR
                                      XML PATH('range'),
                                          TYPE
                                    )
                        FOR
                          XML PATH('ranges'),
                              TYPE
                        )
    endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_GroupApplyBatch @xmlresult,3
--exec usp_repl_GroupApplyBatch @xmlresult,2
--exec dbo.usp_repl_GroupReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_GroupGetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/
GO
PRINT N'Creating [dbo].[usp_repl_GroupReplDone]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_GroupReplDone] @xmlrange XML
AS 
    SET nocount ON
--select a.*
    UPDATE  a
    SET     Done = 1
    FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                        item.ref.value('@umin', 'varbinary(8)') AS umin,
                        item.ref.value('@umax', 'varbinary(8)') AS umax
              FROM      ( SELECT    @xmlrange
                        ) feeds ( feedXml )
                        CROSS APPLY feedXml.nodes('/ranges/range') AS item ( ref )
            ) tab
            JOIN dbo.Group_repl a ON a.id = tab.Id
                                     AND a.Updated BETWEEN tab.umin AND tab.umax
GO
PRINT N'Creating [dbo].[usp_repl_GroupRoleApplyBatch]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_GroupRoleApplyBatch] @xml XML, @type INT
AS 
    SET nocount ON
    IF @type = 1 
        BEGIN
            SET IDENTITY_INSERT dbo.GroupRole ON 
            INSERT  dbo.GroupRole
                    (
                      Id,
                      RoleId,
                      GroupId,
                      IsActive,
                      IsActiveCondition
                    )
                    SELECT  item.ref.value('@Id', 'int') AS Id,
                            item.ref.value('@RoleId', 'int') AS RoleId,
                            item.ref.value('@GroupId', 'int') AS GroupId,
                            item.ref.value('@IsActive', 'bit') AS IsActive,
                            item.ref.value('@IsActiveCondition',
                                           'nvarchar(max)') AS IsActiveCondition
                    FROM    ( SELECT    @xml
                            ) feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
            SET IDENTITY_INSERT dbo.GroupRole OFF
            RETURN
        END
    IF @type = 2 
        BEGIN
            UPDATE  a
            SET     RoleId = tab.RoleId,
                    GroupId = tab.GroupId,
                    IsActive = tab.IsActive,
                    IsActiveCondition = tab.IsActiveCondition
            FROM    ( SELECT    item.ref.value('@Id', 'int') AS Id,
                                item.ref.value('@RoleId', 'int') AS RoleId,
                                item.ref.value('@GroupId', 'int') AS GroupId,
                                item.ref.value('@IsActive', 'bit') AS IsActive,
                                item.ref.value('@IsActiveCondition',
                                               'nvarchar(max)') AS IsActiveCondition
                      FROM      ( SELECT    @xml
                                ) feeds ( feedXml )
                                CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
                    ) tab
                    JOIN dbo.GroupRole a ON a.id = tab.Id


            RETURN
        END

    IF @type = 3 
        BEGIN
            BEGIN TRY
                BEGIN TRAN
                DECLARE @t TABLE ( id BIGINT )
                INSERT  @t ( id )
                        SELECT  id
                        FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id
                                  FROM      ( SELECT    @xml
                                            ) feeds ( feedXml )
                                            CROSS APPLY feedXml.nodes('/rows/row')
                                            AS item ( ref )
                                ) tab

                DELETE  a
                FROM    @t tab
                        JOIN dbo.GroupRole a ON a.id = tab.Id 
                IF @@TRANCOUNT > 0 
                    COMMIT
            END TRY
            BEGIN CATCH
                IF @@trancount > 0 
                    ROLLBACK
                DECLARE @msg NVARCHAR(4000)
                SET @msg = ERROR_MESSAGE()
                RAISERROR ( @msg, 16, 1 )
                RETURN -1
            END CATCH
            RETURN
        END
GO
PRINT N'Creating [dbo].[usp_repl_GroupRoleGetBatch]...' ;


GO

CREATE PROC [dbo].[usp_repl_GroupRoleGetBatch]
    @type INT = 3,
    @rowcount INT = 2,
    @xmlrange XML = NULL OUT,
    @xmlresult XML = NULL OUT
AS 
    SET nocount ON
    DECLARE @min TIMESTAMP,
        @max TIMESTAMP
    DECLARE @range TABLE
        (
          id BIGINT,
          umin VARBINARY(8),
          umax VARBINARY(8)
        )
    INSERT  @range
            SELECT TOP ( @rowcount )
                    id,
                    MIN(Updated) umin,
                    MAX(Updated) umax
            FROM    dbo.GroupRole_repl
            WHERE   type = @type
                    AND done = 0
            GROUP BY id

    IF @@rowcount = 0 
        GOTO endp
    IF @type <> 3 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    b.Id AS '@Id',
                                                    b.RoleId AS '@RoleId',
                                                    b.GroupId AS '@GroupId',
                                                    b.IsActive AS '@IsActive',
                                                    b.IsActiveCondition AS '@IsActiveCondition'
                                          FROM      @range a
                                                    JOIN [dbo].[GroupRole] b ON a.id = b.id
                                        FOR
                                          XML PATH('row'),
                                              ELEMENTS XSINIL,
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )

        END
    ELSE 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    ISNULL(a.Id, 0) AS '@Id'
                                          FROM      @range a
                                        FOR
                                          XML PATH('row'),
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )
        END

    SELECT  @xmlrange = ( SELECT    ( SELECT    Id AS '@Id',
                                                umin AS '@umin',
                                                umax AS '@umax'
                                      FROM      @range
                                    FOR
                                      XML PATH('range'),
                                          TYPE
                                    )
                        FOR
                          XML PATH('ranges'),
                              TYPE
                        )
    endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_GroupRoleApplyBatch @xmlresult,3
--exec usp_repl_GroupRoleApplyBatch @xmlresult,2
--exec dbo.usp_repl_GroupRoleReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_GroupRoleGetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/
GO
PRINT N'Creating [dbo].[usp_repl_GroupRoleReplDone]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_GroupRoleReplDone] @xmlrange XML
AS 
    SET nocount ON
--select a.*
    UPDATE  a
    SET     Done = 1
    FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                        item.ref.value('@umin', 'varbinary(8)') AS umin,
                        item.ref.value('@umax', 'varbinary(8)') AS umax
              FROM      ( SELECT    @xmlrange
                        ) feeds ( feedXml )
                        CROSS APPLY feedXml.nodes('/ranges/range') AS item ( ref )
            ) tab
            JOIN dbo.GroupRole_repl a ON a.id = tab.Id
                                         AND a.Updated BETWEEN tab.umin AND tab.umax
GO
PRINT N'Creating [dbo].[usp_repl_Organization2010ApplyBatch]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_Organization2010ApplyBatch] @xml XML, @type INT
AS 
    SET nocount ON
    IF @type = 1 
        BEGIN
            SET IDENTITY_INSERT dbo.Organization2010 ON 
            INSERT  dbo.Organization2010
                    (
                      Id,
                      CreateDate,
                      UpdateDate,
                      FullName,
                      ShortName,
                      RegionId,
                      TypeId,
                      KindId,
                      INN,
                      OGRN,
                      OwnerDepartment,
                      IsPrivate,
                      IsFilial,
                      DirectorPosition,
                      DirectorFullName,
                      IsAccredited,
                      AccreditationSertificate,
                      LawAddress,
                      FactAddress,
                      PhoneCityCode,
                      Phone,
                      Fax,
                      EMail,
                      Site,
                      WasImportedAtStart,
                      CNFederalBudget,
                      CNTargeted,
                      CNLocalBudget,
                      CNPaying,
                      CNFullTime,
                      CNEvening,
                      CNPostal,
                      RCModel,
                      RCDescription,
                      MainId,
                      DepartmentId,
                      StatusId,
                      NewOrgId,
                      Version,
                      DateChangeStatus,
                      Reason,
                      ReceptionOnResultsCNE,
                      KPP
                    )
                    SELECT  item.ref.value('@Id', 'int') AS Id,
                            item.ref.value('@CreateDate', 'datetime') AS CreateDate,
                            item.ref.value('@UpdateDate', 'datetime') AS UpdateDate,
                            item.ref.value('@FullName', 'nvarchar(1000)') AS FullName,
                            item.ref.value('@ShortName', 'nvarchar(500)') AS ShortName,
                            item.ref.value('@RegionId', 'int') AS RegionId,
                            item.ref.value('@TypeId', 'int') AS TypeId,
                            item.ref.value('@KindId', 'int') AS KindId,
                            item.ref.value('@INN', 'nvarchar(10)') AS INN,
                            item.ref.value('@OGRN', 'nvarchar(13)') AS OGRN,
                            item.ref.value('@OwnerDepartment', 'nvarchar(500)') AS OwnerDepartment,
                            item.ref.value('@IsPrivate', 'bit') AS IsPrivate,
                            item.ref.value('@IsFilial', 'bit') AS IsFilial,
                            item.ref.value('@DirectorPosition',
                                           'nvarchar(255)') AS DirectorPosition,
                            item.ref.value('@DirectorFullName',
                                           'nvarchar(255)') AS DirectorFullName,
                            item.ref.value('@IsAccredited', 'bit') AS IsAccredited,
                            item.ref.value('@AccreditationSertificate',
                                           'nvarchar(255)') AS AccreditationSertificate,
                            item.ref.value('@LawAddress', 'nvarchar(255)') AS LawAddress,
                            item.ref.value('@FactAddress', 'nvarchar(255)') AS FactAddress,
                            item.ref.value('@PhoneCityCode', 'nvarchar(255)') AS PhoneCityCode,
                            item.ref.value('@Phone', 'nvarchar(10)') AS Phone,
                            item.ref.value('@Fax', 'nvarchar(100)') AS Fax,
                            item.ref.value('@EMail', 'nvarchar(100)') AS EMail,
                            item.ref.value('@Site', 'nvarchar(100)') AS Site,
                            item.ref.value('@WasImportedAtStart',
                                           'nvarchar(40)') AS WasImportedAtStart,
                            item.ref.value('@CNFederalBudget', 'bit') AS CNFederalBudget,
                            item.ref.value('@CNTargeted', 'int') AS CNTargeted,
                            item.ref.value('@CNLocalBudget', 'int') AS CNLocalBudget,
                            item.ref.value('@CNPaying', 'int') AS CNPaying,
                            item.ref.value('@CNFullTime', 'int') AS CNFullTime,
                            item.ref.value('@CNEvening', 'int') AS CNEvening,
                            item.ref.value('@CNPostal', 'int') AS CNPostal,
                            item.ref.value('@RCModel', 'int') AS RCModel,
                            item.ref.value('@RCDescription', 'nvarchar(400)') AS RCDescription,
                            item.ref.value('@MainId', 'nvarchar(255)') AS MainId,
                            item.ref.value('@DepartmentId', 'int') AS DepartmentId,
                            item.ref.value('@StatusId', 'int') AS StatusId,
                            item.ref.value('@NewOrgId', 'int') AS NewOrgId,
                            item.ref.value('@Version', 'int') AS Version,
                            item.ref.value('@DateChangeStatus', 'datetime') AS DateChangeStatus,
                            item.ref.value('@Reason', 'nvarchar(100)') AS Reason,
                            item.ref.value('@ReceptionOnResultsCNE', 'bit') AS ReceptionOnResultsCNE,
                            item.ref.value('@KPP', 'nvarchar(9)') AS KPP
                    FROM    ( SELECT    @xml
                            ) feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
            SET IDENTITY_INSERT dbo.Organization2010 OFF
            RETURN
        END
    IF @type = 2 
        BEGIN
            UPDATE  a
            SET     CreateDate = tab.CreateDate,
                    UpdateDate = tab.UpdateDate,
                    FullName = tab.FullName,
                    ShortName = tab.ShortName,
                    RegionId = tab.RegionId,
                    TypeId = tab.TypeId,
                    KindId = tab.KindId,
                    INN = tab.INN,
                    OGRN = tab.OGRN,
                    OwnerDepartment = tab.OwnerDepartment,
                    IsPrivate = tab.IsPrivate,
                    IsFilial = tab.IsFilial,
                    DirectorPosition = tab.DirectorPosition,
                    DirectorFullName = tab.DirectorFullName,
                    IsAccredited = tab.IsAccredited,
                    AccreditationSertificate = tab.AccreditationSertificate,
                    LawAddress = tab.LawAddress,
                    FactAddress = tab.FactAddress,
                    PhoneCityCode = tab.PhoneCityCode,
                    Phone = tab.Phone,
                    Fax = tab.Fax,
                    EMail = tab.EMail,
                    Site = tab.Site,
                    WasImportedAtStart = tab.WasImportedAtStart,
                    CNFederalBudget = tab.CNFederalBudget,
                    CNTargeted = tab.CNTargeted,
                    CNLocalBudget = tab.CNLocalBudget,
                    CNPaying = tab.CNPaying,
                    CNFullTime = tab.CNFullTime,
                    CNEvening = tab.CNEvening,
                    CNPostal = tab.CNPostal,
                    RCModel = tab.RCModel,
                    RCDescription = tab.RCDescription,
                    MainId = tab.MainId,
                    DepartmentId = tab.DepartmentId,
                    StatusId = tab.StatusId,
                    NewOrgId = tab.NewOrgId,
                    Version = tab.Version,
                    DateChangeStatus = tab.DateChangeStatus,
                    Reason = tab.Reason,
                    ReceptionOnResultsCNE = tab.ReceptionOnResultsCNE,
                    KPP = tab.KPP
            FROM    ( SELECT    item.ref.value('@Id', 'int') AS Id,
                                item.ref.value('@CreateDate', 'datetime') AS CreateDate,
                                item.ref.value('@UpdateDate', 'datetime') AS UpdateDate,
                                item.ref.value('@FullName', 'nvarchar(1000)') AS FullName,
                                item.ref.value('@ShortName', 'nvarchar(500)') AS ShortName,
                                item.ref.value('@RegionId', 'int') AS RegionId,
                                item.ref.value('@TypeId', 'int') AS TypeId,
                                item.ref.value('@KindId', 'int') AS KindId,
                                item.ref.value('@INN', 'nvarchar(10)') AS INN,
                                item.ref.value('@OGRN', 'nvarchar(13)') AS OGRN,
                                item.ref.value('@OwnerDepartment',
                                               'nvarchar(500)') AS OwnerDepartment,
                                item.ref.value('@IsPrivate', 'bit') AS IsPrivate,
                                item.ref.value('@IsFilial', 'bit') AS IsFilial,
                                item.ref.value('@DirectorPosition',
                                               'nvarchar(255)') AS DirectorPosition,
                                item.ref.value('@DirectorFullName',
                                               'nvarchar(255)') AS DirectorFullName,
                                item.ref.value('@IsAccredited', 'bit') AS IsAccredited,
                                item.ref.value('@AccreditationSertificate',
                                               'nvarchar(255)') AS AccreditationSertificate,
                                item.ref.value('@LawAddress', 'nvarchar(255)') AS LawAddress,
                                item.ref.value('@FactAddress', 'nvarchar(255)') AS FactAddress,
                                item.ref.value('@PhoneCityCode',
                                               'nvarchar(255)') AS PhoneCityCode,
                                item.ref.value('@Phone', 'nvarchar(10)') AS Phone,
                                item.ref.value('@Fax', 'nvarchar(100)') AS Fax,
                                item.ref.value('@EMail', 'nvarchar(100)') AS EMail,
                                item.ref.value('@Site', 'nvarchar(100)') AS Site,
                                item.ref.value('@WasImportedAtStart',
                                               'nvarchar(40)') AS WasImportedAtStart,
                                item.ref.value('@CNFederalBudget', 'bit') AS CNFederalBudget,
                                item.ref.value('@CNTargeted', 'int') AS CNTargeted,
                                item.ref.value('@CNLocalBudget', 'int') AS CNLocalBudget,
                                item.ref.value('@CNPaying', 'int') AS CNPaying,
                                item.ref.value('@CNFullTime', 'int') AS CNFullTime,
                                item.ref.value('@CNEvening', 'int') AS CNEvening,
                                item.ref.value('@CNPostal', 'int') AS CNPostal,
                                item.ref.value('@RCModel', 'int') AS RCModel,
                                item.ref.value('@RCDescription',
                                               'nvarchar(400)') AS RCDescription,
                                item.ref.value('@MainId', 'nvarchar(255)') AS MainId,
                                item.ref.value('@DepartmentId', 'int') AS DepartmentId,
                                item.ref.value('@StatusId', 'int') AS StatusId,
                                item.ref.value('@NewOrgId', 'int') AS NewOrgId,
                                item.ref.value('@Version', 'int') AS Version,
                                item.ref.value('@DateChangeStatus', 'datetime') AS DateChangeStatus,
                                item.ref.value('@Reason', 'nvarchar(100)') AS Reason,
                                item.ref.value('@ReceptionOnResultsCNE', 'bit') AS ReceptionOnResultsCNE,
                                item.ref.value('@KPP', 'nvarchar(9)') AS KPP
                      FROM      ( SELECT    @xml
                                ) feeds ( feedXml )
                                CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
                    ) tab
                    JOIN dbo.Organization2010 a ON a.id = tab.Id


            RETURN
        END

    IF @type = 3 
        BEGIN
            BEGIN TRY
                BEGIN TRAN
                DECLARE @t TABLE ( id BIGINT )
                INSERT  @t ( id )
                        SELECT  id
                        FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id
                                  FROM      ( SELECT    @xml
                                            ) feeds ( feedXml )
                                            CROSS APPLY feedXml.nodes('/rows/row')
                                            AS item ( ref )
                                ) tab

                DELETE  a
                FROM    @t tab
                        JOIN dbo.Organization2010 a ON a.id = tab.Id 
                IF @@TRANCOUNT > 0 
                    COMMIT
            END TRY
            BEGIN CATCH
                IF @@trancount > 0 
                    ROLLBACK
                DECLARE @msg NVARCHAR(4000)
                SET @msg = ERROR_MESSAGE()
                RAISERROR ( @msg, 16, 1 )
                RETURN -1
            END CATCH
        END
GO
PRINT N'Creating [dbo].[usp_repl_Organization2010GetBatch]...' ;


GO

CREATE PROC [dbo].[usp_repl_Organization2010GetBatch]
    @type INT = 3,
    @rowcount INT = 2,
    @xmlrange XML = NULL OUT,
    @xmlresult XML = NULL OUT
AS 
    SET nocount ON
    DECLARE @min TIMESTAMP,
        @max TIMESTAMP
    DECLARE @range TABLE
        (
          id BIGINT,
          umin VARBINARY(8),
          umax VARBINARY(8)
        )
    INSERT  @range
            SELECT TOP ( @rowcount )
                    id,
                    MIN(Updated) umin,
                    MAX(Updated) umax
            FROM    dbo.Organization2010_repl
            WHERE   type = @type
                    AND done = 0
            GROUP BY id
    IF @@rowcount = 0 
        GOTO endp
    IF @type <> 3 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    b.Id AS '@Id',
                                                    b.CreateDate AS '@CreateDate',
                                                    b.UpdateDate AS '@UpdateDate',
                                                    b.FullName AS '@FullName',
                                                    b.ShortName AS '@ShortName',
                                                    b.RegionId AS '@RegionId',
                                                    b.TypeId AS '@TypeId',
                                                    b.KindId AS '@KindId',
                                                    b.INN AS '@INN',
                                                    b.OGRN AS '@OGRN',
                                                    b.OwnerDepartment AS '@OwnerDepartment',
                                                    b.IsPrivate AS '@IsPrivate',
                                                    b.IsFilial AS '@IsFilial',
                                                    b.DirectorPosition AS '@DirectorPosition',
                                                    b.DirectorFullName AS '@DirectorFullName',
                                                    b.IsAccredited AS '@IsAccredited',
                                                    b.AccreditationSertificate AS '@AccreditationSertificate',
                                                    b.LawAddress AS '@LawAddress',
                                                    b.FactAddress AS '@FactAddress',
                                                    b.PhoneCityCode AS '@PhoneCityCode',
                                                    b.Phone AS '@Phone',
                                                    b.Fax AS '@Fax',
                                                    b.EMail AS '@EMail',
                                                    b.Site AS '@Site',
                                                    b.WasImportedAtStart AS '@WasImportedAtStart',
                                                    b.CNFederalBudget AS '@CNFederalBudget',
                                                    b.CNTargeted AS '@CNTargeted',
                                                    b.CNLocalBudget AS '@CNLocalBudget',
                                                    b.CNPaying AS '@CNPaying',
                                                    b.CNFullTime AS '@CNFullTime',
                                                    b.CNEvening AS '@CNEvening',
                                                    b.CNPostal AS '@CNPostal',
                                                    b.RCModel AS '@RCModel',
                                                    b.RCDescription AS '@RCDescription',
                                                    b.MainId AS '@MainId',
                                                    b.DepartmentId AS '@DepartmentId',
                                                    b.StatusId AS '@StatusId',
                                                    b.NewOrgId AS '@NewOrgId',
                                                    b.Version AS '@Version',
                                                    b.DateChangeStatus AS '@DateChangeStatus',
                                                    b.Reason AS '@Reason',
                                                    b.ReceptionOnResultsCNE AS '@ReceptionOnResultsCNE',
                                                    b.KPP AS '@KPP'
                                          FROM      @range a
                                                    JOIN [dbo].[Organization2010] b ON a.id = b.id
                                        FOR
                                          XML PATH('row'),
                                              ELEMENTS XSINIL,
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )

        END
    ELSE 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    ISNULL(a.Id, 0) AS '@Id'
                                          FROM      @range a
                                        FOR
                                          XML PATH('row'),
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )
        END

    SELECT  @xmlrange = ( SELECT    ( SELECT    Id AS '@Id',
                                                umin AS '@umin',
                                                umax AS '@umax'
                                      FROM      @range
                                    FOR
                                      XML PATH('range'),
                                          TYPE
                                    )
                        FOR
                          XML PATH('ranges'),
                              TYPE
                        )
    endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_Organization2010ApplyBatch @xmlresult,3
--exec usp_repl_Organization2010ApplyBatch @xmlresult,2
--exec dbo.usp_repl_Organization2010ReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_Organization2010GetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/
GO
PRINT N'Creating [dbo].[usp_repl_Organization2010ReplDone]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_Organization2010ReplDone] @xmlrange XML
AS 
    SET nocount ON
--select a.*
    UPDATE  a
    SET     Done = 1
    FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                        item.ref.value('@umin', 'varbinary(8)') AS umin,
                        item.ref.value('@umax', 'varbinary(8)') AS umax
              FROM      ( SELECT    @xmlrange
                        ) feeds ( feedXml )
                        CROSS APPLY feedXml.nodes('/ranges/range') AS item ( ref )
            ) tab
            JOIN dbo.Organization2010_repl a ON a.id = tab.Id
                                                AND a.Updated BETWEEN tab.umin AND tab.umax
GO
PRINT N'Creating [dbo].[usp_repl_OrganizationRequest2010ApplyBatch]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_OrganizationRequest2010ApplyBatch] @xml XML,
    @type INT
AS 
    SET nocount ON
    IF @type = 1 
        BEGIN
            SET IDENTITY_INSERT dbo.OrganizationRequest2010 ON 
            INSERT  dbo.OrganizationRequest2010
                    (
                      Id,
                      CreateDate,
                      UpdateDate,
                      FullName,
                      ShortName,
                      RegionId,
                      TypeId,
                      KindId,
                      INN,
                      OGRN,
                      OwnerDepartment,
                      IsPrivate,
                      IsFilial,
                      DirectorPosition,
                      DirectorFullName,
                      IsAccredited,
                      AccreditationSertificate,
                      LawAddress,
                      FactAddress,
                      PhoneCityCode,
                      Phone,
                      Fax,
                      EMail,
                      Site,
                      OrganizationId,
                      StatusID,
--RegistrationDocument,
                      RegistrationDocumentContentType,
                      IsForActivation,
                      RCModelID,
                      RCDescription,
                      ReceptionOnResultsCNE,
                      KPP
                    )
                    SELECT  item.ref.value('@Id', 'int') AS Id,
                            item.ref.value('@CreateDate', 'datetime') AS CreateDate,
                            item.ref.value('@UpdateDate', 'datetime') AS UpdateDate,
                            item.ref.value('@FullName', 'nvarchar(1000)') AS FullName,
                            item.ref.value('@ShortName', 'nvarchar(500)') AS ShortName,
                            item.ref.value('@RegionId', 'int') AS RegionId,
                            item.ref.value('@TypeId', 'int') AS TypeId,
                            item.ref.value('@KindId', 'int') AS KindId,
                            item.ref.value('@INN', 'nvarchar(10)') AS INN,
                            item.ref.value('@OGRN', 'nvarchar(13)') AS OGRN,
                            item.ref.value('@OwnerDepartment', 'nvarchar(500)') AS OwnerDepartment,
                            item.ref.value('@IsPrivate', 'bit') AS IsPrivate,
                            item.ref.value('@IsFilial', 'bit') AS IsFilial,
                            item.ref.value('@DirectorPosition',
                                           'nvarchar(255)') AS DirectorPosition,
                            item.ref.value('@DirectorFullName',
                                           'nvarchar(255)') AS DirectorFullName,
                            item.ref.value('@IsAccredited', 'bit') AS IsAccredited,
                            item.ref.value('@AccreditationSertificate',
                                           'nvarchar(255)') AS AccreditationSertificate,
                            item.ref.value('@LawAddress', 'nvarchar(255)') AS LawAddress,
                            item.ref.value('@FactAddress', 'nvarchar(255)') AS FactAddress,
                            item.ref.value('@PhoneCityCode', 'nvarchar(255)') AS PhoneCityCode,
                            item.ref.value('@Phone', 'nvarchar(10)') AS Phone,
                            item.ref.value('@Fax', 'nvarchar(100)') AS Fax,
                            item.ref.value('@EMail', 'nvarchar(100)') AS EMail,
                            item.ref.value('@Site', 'nvarchar(40)') AS Site,
                            item.ref.value('@OrganizationId', 'int') AS OrganizationId,
                            item.ref.value('@StatusID', 'int') AS StatusID,
--item.ref.value('@RegistrationDocument', 'nvarchar(255)') as RegistrationDocument,
                            item.ref.value('@RegistrationDocumentContentType',
                                           'nvarchar(255)') AS RegistrationDocumentContentType,
                            item.ref.value('@IsForActivation', 'bit') AS IsForActivation,
                            item.ref.value('@RCModelID', 'int') AS RCModelID,
                            item.ref.value('@RCDescription', 'nvarchar(400)') AS RCDescription,
                            item.ref.value('@ReceptionOnResultsCNE', 'bit') AS ReceptionOnResultsCNE,
                            item.ref.value('@KPP', 'nvarchar(9)') AS KPP
                    FROM    ( SELECT    @xml
                            ) feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
            SET IDENTITY_INSERT dbo.OrganizationRequest2010 OFF
            RETURN
        END
    IF @type = 2 
        BEGIN
            UPDATE  a
            SET     CreateDate = tab.CreateDate,
                    UpdateDate = tab.UpdateDate,
                    FullName = tab.FullName,
                    ShortName = tab.ShortName,
                    RegionId = tab.RegionId,
                    TypeId = tab.TypeId,
                    KindId = tab.KindId,
                    INN = tab.INN,
                    OGRN = tab.OGRN,
                    OwnerDepartment = tab.OwnerDepartment,
                    IsPrivate = tab.IsPrivate,
                    IsFilial = tab.IsFilial,
                    DirectorPosition = tab.DirectorPosition,
                    DirectorFullName = tab.DirectorFullName,
                    IsAccredited = tab.IsAccredited,
                    AccreditationSertificate = tab.AccreditationSertificate,
                    LawAddress = tab.LawAddress,
                    FactAddress = tab.FactAddress,
                    PhoneCityCode = tab.PhoneCityCode,
                    Phone = tab.Phone,
                    Fax = tab.Fax,
                    EMail = tab.EMail,
                    Site = tab.Site,
                    OrganizationId = tab.OrganizationId,
                    StatusID = tab.StatusID,
--RegistrationDocument=tab.RegistrationDocument,
                    RegistrationDocumentContentType = tab.RegistrationDocumentContentType,
                    IsForActivation = tab.IsForActivation,
                    RCModelID = tab.RCModelID,
                    RCDescription = tab.RCDescription,
                    ReceptionOnResultsCNE = tab.ReceptionOnResultsCNE,
                    KPP = tab.KPP
            FROM    ( SELECT    item.ref.value('@Id', 'int') AS Id,
                                item.ref.value('@CreateDate', 'datetime') AS CreateDate,
                                item.ref.value('@UpdateDate', 'datetime') AS UpdateDate,
                                item.ref.value('@FullName', 'nvarchar(1000)') AS FullName,
                                item.ref.value('@ShortName', 'nvarchar(500)') AS ShortName,
                                item.ref.value('@RegionId', 'int') AS RegionId,
                                item.ref.value('@TypeId', 'int') AS TypeId,
                                item.ref.value('@KindId', 'int') AS KindId,
                                item.ref.value('@INN', 'nvarchar(10)') AS INN,
                                item.ref.value('@OGRN', 'nvarchar(13)') AS OGRN,
                                item.ref.value('@OwnerDepartment',
                                               'nvarchar(500)') AS OwnerDepartment,
                                item.ref.value('@IsPrivate', 'bit') AS IsPrivate,
                                item.ref.value('@IsFilial', 'bit') AS IsFilial,
                                item.ref.value('@DirectorPosition',
                                               'nvarchar(255)') AS DirectorPosition,
                                item.ref.value('@DirectorFullName',
                                               'nvarchar(255)') AS DirectorFullName,
                                item.ref.value('@IsAccredited', 'bit') AS IsAccredited,
                                item.ref.value('@AccreditationSertificate',
                                               'nvarchar(255)') AS AccreditationSertificate,
                                item.ref.value('@LawAddress', 'nvarchar(255)') AS LawAddress,
                                item.ref.value('@FactAddress', 'nvarchar(255)') AS FactAddress,
                                item.ref.value('@PhoneCityCode',
                                               'nvarchar(255)') AS PhoneCityCode,
                                item.ref.value('@Phone', 'nvarchar(10)') AS Phone,
                                item.ref.value('@Fax', 'nvarchar(100)') AS Fax,
                                item.ref.value('@EMail', 'nvarchar(100)') AS EMail,
                                item.ref.value('@Site', 'nvarchar(40)') AS Site,
                                item.ref.value('@OrganizationId', 'int') AS OrganizationId,
                                item.ref.value('@StatusID', 'int') AS StatusID,
--item.ref.value('@RegistrationDocument', 'nvarchar(255)') as RegistrationDocument,
                                item.ref.value('@RegistrationDocumentContentType',
                                               'nvarchar(255)') AS RegistrationDocumentContentType,
                                item.ref.value('@IsForActivation', 'bit') AS IsForActivation,
                                item.ref.value('@RCModelID', 'int') AS RCModelID,
                                item.ref.value('@RCDescription',
                                               'nvarchar(400)') AS RCDescription,
                                item.ref.value('@ReceptionOnResultsCNE', 'bit') AS ReceptionOnResultsCNE,
                                item.ref.value('@KPP', 'nvarchar(9)') AS KPP
                      FROM      ( SELECT    @xml
                                ) feeds ( feedXml )
                                CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
                    ) tab
                    JOIN dbo.OrganizationRequest2010 a ON a.id = tab.Id


            RETURN
        END

    IF @type = 3 
        BEGIN
            BEGIN TRY
                BEGIN TRAN
                DECLARE @t TABLE ( id BIGINT )
                INSERT  @t ( id )
                        SELECT  id
                        FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id
                                  FROM      ( SELECT    @xml
                                            ) feeds ( feedXml )
                                            CROSS APPLY feedXml.nodes('/rows/row')
                                            AS item ( ref )
                                ) tab

                DELETE  a
                FROM    @t tab
                        JOIN dbo.OrganizationRequest2010 a ON a.id = tab.Id 
                IF @@TRANCOUNT > 0 
                    COMMIT
            END TRY
            BEGIN CATCH
                IF @@trancount > 0 
                    ROLLBACK
                DECLARE @msg NVARCHAR(4000)
                SET @msg = ERROR_MESSAGE()
                RAISERROR ( @msg, 16, 1 )
                RETURN -1
            END CATCH
        END
GO
PRINT N'Creating [dbo].[usp_repl_OrganizationRequest2010GetBatch]...' ;


GO

CREATE PROC [dbo].[usp_repl_OrganizationRequest2010GetBatch]
    @type INT = 3,
    @rowcount INT = 2,
    @xmlrange XML = NULL OUT,
    @xmlresult XML = NULL OUT
AS 
    SET nocount ON
    DECLARE @min TIMESTAMP,
        @max TIMESTAMP
    DECLARE @range TABLE
        (
          id BIGINT,
          umin VARBINARY(8),
          umax VARBINARY(8)
        )
    INSERT  @range
            SELECT TOP ( @rowcount )
                    id,
                    MIN(Updated) umin,
                    MAX(Updated) umax
            FROM    dbo.OrganizationRequest2010_repl
            WHERE   type = @type
                    AND done = 0
            GROUP BY id
    IF @@rowcount = 0 
        GOTO endp
    IF @type <> 3 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    b.Id AS '@Id',
                                                    b.CreateDate AS '@CreateDate',
                                                    b.UpdateDate AS '@UpdateDate',
                                                    b.FullName AS '@FullName',
                                                    b.ShortName AS '@ShortName',
                                                    b.RegionId AS '@RegionId',
                                                    b.TypeId AS '@TypeId',
                                                    b.KindId AS '@KindId',
                                                    b.INN AS '@INN',
                                                    b.OGRN AS '@OGRN',
                                                    b.OwnerDepartment AS '@OwnerDepartment',
                                                    b.IsPrivate AS '@IsPrivate',
                                                    b.IsFilial AS '@IsFilial',
                                                    b.DirectorPosition AS '@DirectorPosition',
                                                    b.DirectorFullName AS '@DirectorFullName',
                                                    b.IsAccredited AS '@IsAccredited',
                                                    b.AccreditationSertificate AS '@AccreditationSertificate',
                                                    b.LawAddress AS '@LawAddress',
                                                    b.FactAddress AS '@FactAddress',
                                                    b.PhoneCityCode AS '@PhoneCityCode',
                                                    b.Phone AS '@Phone',
                                                    b.Fax AS '@Fax',
                                                    b.EMail AS '@EMail',
                                                    b.Site AS '@Site',
                                                    b.OrganizationId AS '@OrganizationId',
                                                    b.StatusID AS '@StatusID',
--b.RegistrationDocument as '@RegistrationDocument',
                                                    b.RegistrationDocumentContentType AS '@RegistrationDocumentContentType',
                                                    b.IsForActivation AS '@IsForActivation',
                                                    b.RCModelID AS '@RCModelID',
                                                    b.RCDescription AS '@RCDescription',
                                                    b.ReceptionOnResultsCNE AS '@ReceptionOnResultsCNE',
                                                    b.KPP AS '@KPP'
                                          FROM      @range a
                                                    JOIN [dbo].[OrganizationRequest2010] b ON a.id = b.id
                                        FOR
                                          XML PATH('row'),
                                              ELEMENTS XSINIL,
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )

        END
    ELSE 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    ISNULL(a.Id, 0) AS '@Id'
                                          FROM      @range a
                                        FOR
                                          XML PATH('row'),
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )
        END

    SELECT  @xmlrange = ( SELECT    ( SELECT    Id AS '@Id',
                                                umin AS '@umin',
                                                umax AS '@umax'
                                      FROM      @range
                                    FOR
                                      XML PATH('range'),
                                          TYPE
                                    )
                        FOR
                          XML PATH('ranges'),
                              TYPE
                        )
    endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_OrganizationRequest2010ApplyBatch @xmlresult,3
--exec usp_repl_OrganizationRequest2010ApplyBatch @xmlresult,2
--exec dbo.usp_repl_OrganizationRequest2010ReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_OrganizationRequest2010GetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/
GO
PRINT N'Creating [dbo].[usp_repl_OrganizationRequest2010ReplDone]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_OrganizationRequest2010ReplDone] @xmlrange XML
AS 
    SET nocount ON
--select a.*
    UPDATE  a
    SET     Done = 1
    FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                        item.ref.value('@umin', 'varbinary(8)') AS umin,
                        item.ref.value('@umax', 'varbinary(8)') AS umax
              FROM      ( SELECT    @xmlrange
                        ) feeds ( feedXml )
                        CROSS APPLY feedXml.nodes('/ranges/range') AS item ( ref )
            ) tab
            JOIN dbo.OrganizationRequest2010_repl a ON a.id = tab.Id
                                                       AND a.Updated BETWEEN tab.umin AND tab.umax
GO
PRINT N'Creating [dbo].[usp_repl_OrganizationRequestAccountApplyBatch]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_OrganizationRequestAccountApplyBatch] @xml XML,
    @type INT
AS 
    SET nocount ON
    IF @type = 1 
        BEGIN
            SET IDENTITY_INSERT dbo.OrganizationRequestAccount ON 
            INSERT  dbo.OrganizationRequestAccount
                    (
                      OrgRequestID,
                      AccountID,
                      GroupID,
                      Id
                    )
                    SELECT  item.ref.value('@OrgRequestID', 'int') AS OrgRequestID,
                            item.ref.value('@AccountID', 'bigint') AS AccountID,
                            item.ref.value('@GroupID', 'int') AS GroupID,
                            item.ref.value('@Id', 'bigint') AS Id
                    FROM    ( SELECT    @xml
                            ) feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
            SET IDENTITY_INSERT dbo.OrganizationRequestAccount OFF
            RETURN
        END
    IF @type = 2 
        BEGIN
            UPDATE  a
            SET     OrgRequestID = tab.OrgRequestID,
                    AccountID = tab.AccountID,
                    GroupID = tab.GroupID
            FROM    ( SELECT    item.ref.value('@OrgRequestID', 'int') AS OrgRequestID,
                                item.ref.value('@AccountID', 'bigint') AS AccountID,
                                item.ref.value('@GroupID', 'int') AS GroupID,
                                item.ref.value('@Id', 'bigint') AS Id
                      FROM      ( SELECT    @xml
                                ) feeds ( feedXml )
                                CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
                    ) tab
                    JOIN dbo.OrganizationRequestAccount a ON a.id = tab.Id


            RETURN
        END

    IF @type = 3 
        BEGIN
            BEGIN TRY
                BEGIN TRAN
                DECLARE @t TABLE ( id BIGINT )
                INSERT  @t ( id )
                        SELECT  id
                        FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id
                                  FROM      ( SELECT    @xml
                                            ) feeds ( feedXml )
                                            CROSS APPLY feedXml.nodes('/rows/row')
                                            AS item ( ref )
                                ) tab

                DELETE  a
                FROM    @t tab
                        JOIN dbo.OrganizationRequestAccount a ON a.id = tab.Id 
                IF @@TRANCOUNT > 0 
                    COMMIT
            END TRY
            BEGIN CATCH
                IF @@trancount > 0 
                    ROLLBACK
                DECLARE @msg NVARCHAR(4000)
                SET @msg = ERROR_MESSAGE()
                RAISERROR ( @msg, 16, 1 )
                RETURN -1
            END CATCH
            RETURN
        END
GO
PRINT N'Creating [dbo].[usp_repl_OrganizationRequestAccountGetBatch]...' ;


GO

CREATE PROC [dbo].[usp_repl_OrganizationRequestAccountGetBatch]
    @type INT = 3,
    @rowcount INT = 2,
    @xmlrange XML = NULL OUT,
    @xmlresult XML = NULL OUT
AS 
    SET nocount ON
    DECLARE @min TIMESTAMP,
        @max TIMESTAMP
    DECLARE @range TABLE
        (
          id BIGINT,
          umin VARBINARY(8),
          umax VARBINARY(8)
        )
    INSERT  @range
            SELECT TOP ( @rowcount )
                    id,
                    MIN(Updated) umin,
                    MAX(Updated) umax
            FROM    dbo.OrganizationRequestAccount_repl
            WHERE   type = @type
                    AND done = 0
            GROUP BY id

    IF @@rowcount = 0 
        GOTO endp
    IF @type <> 3 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    b.OrgRequestID AS '@OrgRequestID',
                                                    b.AccountID AS '@AccountID',
                                                    b.GroupID AS '@GroupID',
                                                    b.Id AS '@Id'
                                          FROM      @range a
                                                    JOIN [dbo].[OrganizationRequestAccount] b ON a.id = b.id
                                        FOR
                                          XML PATH('row'),
                                              ELEMENTS XSINIL,
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )

        END
    ELSE 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    ISNULL(a.Id, 0) AS '@Id'
                                          FROM      @range a
                                        FOR
                                          XML PATH('row'),
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )
        END

    SELECT  @xmlrange = ( SELECT    ( SELECT    Id AS '@Id',
                                                umin AS '@umin',
                                                umax AS '@umax'
                                      FROM      @range
                                    FOR
                                      XML PATH('range'),
                                          TYPE
                                    )
                        FOR
                          XML PATH('ranges'),
                              TYPE
                        )
    endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_OrganizationRequestAccountApplyBatch @xmlresult,3
--exec usp_repl_OrganizationRequestAccountApplyBatch @xmlresult,2
--exec dbo.usp_repl_OrganizationRequestAccountReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_OrganizationRequestAccountGetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/
GO
PRINT N'Creating [dbo].[usp_repl_OrganizationRequestAccountReplDone]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_OrganizationRequestAccountReplDone] @xmlrange XML
AS 
    SET nocount ON
--select a.*
    UPDATE  a
    SET     Done = 1
    FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                        item.ref.value('@umin', 'varbinary(8)') AS umin,
                        item.ref.value('@umax', 'varbinary(8)') AS umax
              FROM      ( SELECT    @xmlrange
                        ) feeds ( feedXml )
                        CROSS APPLY feedXml.nodes('/ranges/range') AS item ( ref )
            ) tab
            JOIN dbo.OrganizationRequestAccount_repl a ON a.id = tab.Id
                                                          AND a.Updated BETWEEN tab.umin AND tab.umax
GO
PRINT N'Creating [dbo].[usp_repl_Tables]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_Tables]
AS 
    SET nocount ON
    SELECT  tablename,
            getbatch,
            repldone,
            so
    FROM    repl_tables
    ORDER BY so
GO
PRINT N'Creating [dbo].[usp_repl_UserAccountPasswordApplyBatch]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_UserAccountPasswordApplyBatch] @xml XML, @type INT
AS 
    SET nocount ON
    IF @type = 1 
        BEGIN
            SET IDENTITY_INSERT dbo.UserAccountPassword ON 
            INSERT  dbo.UserAccountPassword ( AccountId, Password, Id )
                    SELECT  item.ref.value('@AccountId', 'bigint') AS AccountId,
                            item.ref.value('@Password', 'nvarchar(255)') AS Password,
                            item.ref.value('@Id', 'bigint') AS Id
                    FROM    ( SELECT    @xml
                            ) feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
            SET IDENTITY_INSERT dbo.UserAccountPassword OFF
            RETURN
        END
    IF @type = 2 
        BEGIN
            UPDATE  a
            SET     AccountId = tab.AccountId,
                    Password = tab.Password
            FROM    ( SELECT    item.ref.value('@AccountId', 'bigint') AS AccountId,
                                item.ref.value('@Password', 'nvarchar(255)') AS Password,
                                item.ref.value('@Id', 'bigint') AS Id
                      FROM      ( SELECT    @xml
                                ) feeds ( feedXml )
                                CROSS APPLY feedXml.nodes('/rows/row') AS item ( ref )
                    ) tab
                    JOIN dbo.UserAccountPassword a ON a.id = tab.Id

            RETURN
        END

    IF @type = 3 
        BEGIN
            BEGIN TRY
                BEGIN TRAN
                DECLARE @t TABLE ( id BIGINT )
                INSERT  @t ( id )
                        SELECT  id
                        FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id
                                  FROM      ( SELECT    @xml
                                            ) feeds ( feedXml )
                                            CROSS APPLY feedXml.nodes('/rows/row')
                                            AS item ( ref )
                                ) tab

                DELETE  a
                FROM    @t tab
                        JOIN dbo.UserAccountPassword a ON a.id = tab.Id 
                IF @@TRANCOUNT > 0 
                    COMMIT
            END TRY
            BEGIN CATCH
                IF @@trancount > 0 
                    ROLLBACK
                DECLARE @msg NVARCHAR(4000)
                SET @msg = ERROR_MESSAGE()
                RAISERROR ( @msg, 16, 1 )
                RETURN -1
            END CATCH
            RETURN
        END
GO
PRINT N'Creating [dbo].[usp_repl_UserAccountPasswordGetBatch]...' ;


GO

CREATE PROC [dbo].[usp_repl_UserAccountPasswordGetBatch]
    @type INT = 3,
    @rowcount INT = 2,
    @xmlrange XML = NULL OUT,
    @xmlresult XML = NULL OUT
AS 
    SET nocount ON
    DECLARE @min TIMESTAMP,
        @max TIMESTAMP
    DECLARE @range TABLE
        (
          id BIGINT,
          umin VARBINARY(8),
          umax VARBINARY(8)
        )
    INSERT  @range
            SELECT TOP ( @rowcount )
                    id,
                    MIN(Updated) umin,
                    MAX(Updated) umax
            FROM    dbo.UserAccountPassword_repl
            WHERE   type = @type
                    AND done = 0
            GROUP BY id

    IF @@rowcount = 0 
        GOTO endp
    IF @type <> 3 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    b.AccountId AS '@AccountId',
                                                    b.Password AS '@Password',
                                                    b.Id AS '@Id'
                                          FROM      @range a
                                                    JOIN [dbo].[UserAccountPassword] b ON a.id = b.id
                                        FOR
                                          XML PATH('row'),
                                              ELEMENTS XSINIL,
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )

        END
    ELSE 
        BEGIN
            SET @xmlresult = ( SELECT   ( SELECT    ISNULL(a.Id, 0) AS '@Id'
                                          FROM      @range a
                                        FOR
                                          XML PATH('row'),
                                              TYPE
                                        )
                             FOR
                               XML PATH('rows'),
                                   TYPE
                             )
        END

    SELECT  @xmlrange = ( SELECT    ( SELECT    Id AS '@Id',
                                                umin AS '@umin',
                                                umax AS '@umax'
                                      FROM      @range
                                    FOR
                                      XML PATH('range'),
                                          TYPE
                                    )
                        FOR
                          XML PATH('ranges'),
                              TYPE
                        )
    endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_UserAccountPasswordApplyBatch @xmlresult,3
--exec usp_repl_UserAccountPasswordApplyBatch @xmlresult,2
--exec dbo.usp_repl_UserAccountPasswordReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_UserAccountPasswordGetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/
GO
PRINT N'Creating [dbo].[usp_repl_UserAccountPasswordReplDone]...' ;


GO
/*

*/
CREATE PROC [dbo].[usp_repl_UserAccountPasswordReplDone] @xmlrange XML
AS 
    SET nocount ON
--select a.*
    UPDATE  a
    SET     Done = 1
    FROM    ( SELECT    item.ref.value('@Id', 'bigint') AS Id,
                        item.ref.value('@umin', 'varbinary(8)') AS umin,
                        item.ref.value('@umax', 'varbinary(8)') AS umax
              FROM      ( SELECT    @xmlrange
                        ) feeds ( feedXml )
                        CROSS APPLY feedXml.nodes('/ranges/range') AS item ( ref )
            ) tab
            JOIN dbo.UserAccountPassword_repl a ON a.id = tab.Id
                                                   AND a.Updated BETWEEN tab.umin AND tab.umax
GO
PRINT N'Creating [dbo].[usp_repl_ApplyBatch]...' ;


GO

CREATE PROC [dbo].[usp_repl_ApplyBatch]
    @name NVARCHAR(250),
    @xml XML,
    @type INT
AS 
    SET nocount ON
    IF @name = 'Account' 
        BEGIN
            EXEC [dbo].usp_repl_AccountApplyBatch @xml, @type
            RETURN 
        END
    IF @name = 'Organization2010' 
        BEGIN
            EXEC [dbo].usp_repl_Organization2010ApplyBatch @xml, @type
            RETURN 
        END
    IF @name = 'OrganizationRequest2010' 
        BEGIN
            EXEC [dbo].usp_repl_OrganizationRequest2010ApplyBatch @xml, @type
            RETURN 
        END
    IF @name = 'OrganizationRequestAccount' 
        BEGIN
            EXEC [dbo].usp_repl_OrganizationRequestAccountApplyBatch @xml,
                @type
            RETURN 
        END
    IF @name = 'UserAccountPassword' 
        BEGIN
            EXEC [dbo].usp_repl_UserAccountPasswordApplyBatch @xml, @type
            RETURN 
        END
    IF @name = 'Group' 
        BEGIN
            EXEC [dbo].usp_repl_GroupApplyBatch @xml, @type
            RETURN 
        END
    IF @name = 'GroupAccount' 
        BEGIN
            EXEC [dbo].usp_repl_GroupAccountApplyBatch @xml, @type
            RETURN 
        END
    IF @name = 'GroupRole' 
        BEGIN
            EXEC [dbo].usp_repl_GroupRoleApplyBatch @xml, @type
            RETURN 
        END
GO

GO
DELETE  FROM Settings
WHERE   name = 'IsOpenSystem'
INSERT  INTO [dbo].Settings ( name, value )
VALUES  ( 'IsOpenSystem', 'yes' )
TRUNCATE TABLE repl_Tables
INSERT  INTO [dbo].repl_Tables
        (
          tablename,
          getbatch,
          repldone,
          so
        )
        SELECT  'OrganizationRequestAccount',
                'dbo.usp_repl_OrganizationRequestAccountGetBatch',
                'dbo.usp_repl_OrganizationRequestAccountReplDone',
                10
        UNION ALL
        SELECT  'OrganizationRequest2010',
                'dbo.usp_repl_OrganizationRequest2010GetBatch',
                'dbo.usp_repl_OrganizationRequest2010ReplDone',
                20
        UNION ALL
        SELECT  'Organization2010',
                'dbo.usp_repl_Organization2010GetBatch',
                'dbo.usp_repl_Organization2010ReplDone',
                30
        UNION ALL
        SELECT  'Account',
                'dbo.usp_repl_AccountGetBatch',
                'dbo.usp_repl_AccountReplDone',
                50
        UNION ALL
        SELECT  'UserAccountPassword',
                'dbo.usp_repl_UserAccountPasswordGetBatch',
                'dbo.usp_repl_UserAccountPasswordReplDone',
                31
				UNION ALL
SELECT  'Group',
        'dbo.usp_repl_GroupGetBatch',
        'dbo.usp_repl_GroupReplDone',
        40
UNION ALL
SELECT  'GroupAccount',
        'dbo.usp_repl_GroupAccountGetBatch',
        'dbo.usp_repl_GroupAccountReplDone',
        35
UNION ALL
SELECT  'GroupRole',
        'dbo.usp_repl_GroupRoleGetBatch',
        'dbo.usp_repl_GroupRoleReplDone',
        39 

GO
