-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (26, '026__2011_05_21__ReanameFederalBudgetAndPaying')
-- =========================================================================


--1
IF NOT EXISTS(select * from sys.columns where Name = N'CNFBFullTime' and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	ALTER TABLE dbo.Organization2010 ADD CNFBFullTime int NOT NULL default 0
END
GO
IF EXISTS(select * from sys.columns where Name = N'CNFBFullTime'
		and Object_ID = Object_ID(N'Organization2010'))
	and 
	EXISTS(select * from sys.columns where Name = N'CNFederalBudget'
		and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	UPDATE dbo.Organization2010
	SET CNFBFullTime = CNFederalBudget;
	
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[org2010_col_fb]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP CONSTRAINT [org2010_col_fb]
	END
	
	ALTER TABLE dbo.Organization2010 DROP COLUMN CNFederalBudget
END
GO

--2
IF NOT EXISTS(select * from sys.columns where Name = N'CNFBEvening' and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	ALTER TABLE dbo.Organization2010 ADD CNFBEvening int NOT NULL default 0
END
GO
IF EXISTS(select * from sys.columns where Name = N'CNFBEvening'
		and Object_ID = Object_ID(N'Organization2010'))
	and 
	EXISTS(select * from sys.columns where Name = N'CNTargeted'
		and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	UPDATE dbo.Organization2010
	SET CNFBEvening = CNTargeted;
	
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[org2010_col_targ]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP CONSTRAINT [org2010_col_targ]
	END
	
	ALTER TABLE dbo.Organization2010 DROP COLUMN CNTargeted
END
GO

--3
IF NOT EXISTS(select * from sys.columns where Name = N'CNFBPostal' and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	ALTER TABLE dbo.Organization2010 ADD CNFBPostal int NOT NULL default 0
END
GO
IF EXISTS(select * from sys.columns where Name = N'CNFBPostal'
		and Object_ID = Object_ID(N'Organization2010'))
	and 
	EXISTS(select * from sys.columns where Name = N'CNLocalBudget'
		and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	UPDATE dbo.Organization2010
	SET CNFBPostal = CNLocalBudget;
	
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[org2010_col_lb]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP CONSTRAINT [org2010_col_lb]
	END

	ALTER TABLE dbo.Organization2010 DROP COLUMN CNLocalBudget
END
GO


--4
IF NOT EXISTS(select * from sys.columns where Name = N'CNPayFullTime' and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	ALTER TABLE dbo.Organization2010 ADD CNPayFullTime int NOT NULL default 0
END
GO
IF EXISTS(select * from sys.columns where Name = N'CNPayFullTime'
		and Object_ID = Object_ID(N'Organization2010'))
	and 
	EXISTS(select * from sys.columns where Name = N'CNPaying'
		and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	UPDATE dbo.Organization2010
	SET CNPayFullTime = CNPaying;
	
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[org2010_col_pay]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP CONSTRAINT [org2010_col_pay]
	END

	ALTER TABLE dbo.Organization2010 DROP COLUMN CNPaying
END
GO


--5
IF NOT EXISTS(select * from sys.columns where Name = N'CNPayEvening' and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	ALTER TABLE dbo.Organization2010 ADD CNPayEvening int NOT NULL default 0
END
GO
IF EXISTS(select * from sys.columns where Name = N'CNPayEvening'
		and Object_ID = Object_ID(N'Organization2010'))
	and 
	EXISTS(select * from sys.columns where Name = N'CNFullTime'
		and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	UPDATE dbo.Organization2010
	SET CNPayEvening = CNFullTime;
	
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[org2010_col_ft]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP CONSTRAINT [org2010_col_ft]
	END

	ALTER TABLE dbo.Organization2010 DROP COLUMN CNFullTime
END
GO


--6
IF NOT EXISTS(select * from sys.columns where Name = N'CNPayPostal' and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	ALTER TABLE dbo.Organization2010 ADD CNPayPostal int NOT NULL default 0
END
GO
IF EXISTS(select * from sys.columns where Name = N'CNPayPostal'
		and Object_ID = Object_ID(N'Organization2010'))
	and 
	EXISTS(select * from sys.columns where Name = N'CNEvening'
		and Object_ID = Object_ID(N'Organization2010'))
BEGIN
	UPDATE dbo.Organization2010
	SET CNPayPostal = CNEvening;
	
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[org2010_col_even]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP CONSTRAINT [org2010_col_even]
	END

	ALTER TABLE dbo.Organization2010 DROP COLUMN CNEvening
END
GO


--7. 
IF EXISTS(select * from sys.columns where Name = N'CNPostal'
		and Object_ID = Object_ID(N'Organization2010'))
BEGIN

	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[org2010_col_post]') AND type = 'D')
	BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP CONSTRAINT [org2010_col_post]
	END

	ALTER TABLE dbo.Organization2010 DROP COLUMN CNPostal

END