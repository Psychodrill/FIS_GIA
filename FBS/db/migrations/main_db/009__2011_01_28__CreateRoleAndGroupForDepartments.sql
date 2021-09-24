-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (9, '009__2011_01_28__CreateRoleAndGroupForDepartments')
-- =========================================================================




/*SELECT 'insert into [dbo].[Group] (Code, Name) values (' + 
	''''+Code+''', '''+Name+''')'
FROM
	[dbo].[Group]
GO*/

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES B WHERE B.TABLE_SCHEMA = 'dbo' AND  B.TABLE_NAME = 'Group'))
  DROP TABLE [dbo].[Group]
GO
	
CREATE TABLE [dbo].[Group] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO
	
insert into [dbo].[Group] (Code, Name) values ('User', 'Пользователи ВУЗ/ССУЗ')
insert into [dbo].[Group] (Code, Name) values ('Administrator', 'Администраторы системы')
insert into [dbo].[Group] (Code, Name) values ('Support', 'Сотрудники "горячей линии"')
insert into [dbo].[Group] (Code, Name) values ('Auditor', 'Сотрудники проверяющей организации')
insert into [dbo].[Group] (Code, Name) values ('Supervisor', 'Сотрудники контролирующей организации')
--Группа пользователей - учредителей
insert into [dbo].[Group] (Code, Name) values ('UserDepartment', 'Пользователи учредителея')
--Группа пользователей РЦОИ
insert into [dbo].[Group] (Code, Name) values ('UserRCOI', 'Пользователи РЦОИ')
GO

--Выбрать Роли для последующей вставки. Вставлять после создания таблицы
/*SELECT 'insert into [dbo].[Role] (Code, Name) values (' + 
	''''+Code+''', '''+Name+''')'
FROM
	[dbo].[Role]
GO*/

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS B WHERE B.TABLE_SCHEMA = 'dbo' AND  B.TABLE_NAME = 'AccountRole'))
  DROP VIEW [dbo].[AccountRole]
GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES B WHERE B.TABLE_SCHEMA = 'dbo' AND  B.TABLE_NAME = 'Role'))
  DROP TABLE [dbo].[Role]
GO
	
CREATE TABLE [dbo].[Role] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

insert into [dbo].[Role] (Code, Name) values ('EditSelfAccount', 'Изменение собственной учетной записи')
insert into [dbo].[Role] (Code, Name) values ('ViewUserAccount', 'Просмотр учетных записей пользователей')
insert into [dbo].[Role] (Code, Name) values ('потом удалить', 'потом удалить')
insert into [dbo].[Role] (Code, Name) values ('EditUserAccount', 'Изменение учетных записей пользователей')
insert into [dbo].[Role] (Code, Name) values ('ViewAdministrationSection', 'Просмотр раздела Ядминистрирование')
insert into [dbo].[Role] (Code, Name) values ('ViewSupportAccount', 'Просмотр учетных записей тех. поддержки')
insert into [dbo].[Role] (Code, Name) values ('EditSupportAccount', 'Изменение учетных записей тех. поддержки')
insert into [dbo].[Role] (Code, Name) values ('ViewAdministratorAccount', 'Просмотр учетной записи администратора')
insert into [dbo].[Role] (Code, Name) values ('EditAdministratorAccount', 'Изменение учетной записи администратора')
insert into [dbo].[Role] (Code, Name) values ('EditDocument', 'Редактирование документов')
insert into [dbo].[Role] (Code, Name) values ('EditNews', 'Редактирование новостей')
insert into [dbo].[Role] (Code, Name) values ('EditFAQ', 'Редактирование ЧАВО')
insert into [dbo].[Role] (Code, Name) values ('ViewDocument', 'Просмотр документов')
insert into [dbo].[Role] (Code, Name) values ('ViewNews', 'Просмотр новостей')
insert into [dbo].[Role] (Code, Name) values ('ViewFAQ', 'Просмотр ЧАВО')
insert into [dbo].[Role] (Code, Name) values ('ViewAuditorAccount', 'Просмотр учетной записи сотрудника проверяющей организации')
insert into [dbo].[Role] (Code, Name) values ('EditAuditorAccount', 'Редактирование учетной записи сотрудника проверяющей организации')
insert into [dbo].[Role] (Code, Name) values ('ViewAdministrationReport', 'Просмотр отчетов администрирования')
insert into [dbo].[Role] (Code, Name) values ('ViewCertificateSection', 'Просмотр раздела Свидетельства')
insert into [dbo].[Role] (Code, Name) values ('CheckCommonNationalCertificate', 'Проверка сертификатов ЕГЭ')
insert into [dbo].[Role] (Code, Name) values ('CheckCommonNationalCertificateExam', 'Проверка сдачи экзаменов ЕГЭ')
insert into [dbo].[Role] (Code, Name) values ('ViewCommonNationalCertificateReport', 'Просмотр отчетов по загрузке сертификатов')
insert into [dbo].[Role] (Code, Name) values ('LoadEntrant', 'Загрузка данных по поступающим')
insert into [dbo].[Role] (Code, Name) values ('ViewCommonNationalCertificateDocument', 'Просмотр документов сертификатов ЕГЭ')
insert into [dbo].[Role] (Code, Name) values ('CheckCommonNationalCertificateExtended', 'Проверка сертификатов ЕГЭ расширенная')
insert into [dbo].[Role] (Code, Name) values ('CheckEntrant', 'Проверка абитуриентов')
insert into [dbo].[Role] (Code, Name) values ('CheckSchoolLeavingCertificate', 'Проверка школьных аттестатов')
insert into [dbo].[Role] (Code, Name) values ('CheckSchoolCompetitionCertificate', 'Проверка сертификатов олимпиад')
insert into [dbo].[Role] (Code, Name) values ('HasIntegration', 'Интеграция через службу')
insert into [dbo].[Role] (Code, Name) values ('LoadCommonNationalCertificate', 'Загрузка сертификатов ЕГЭ')
insert into [dbo].[Role] (Code, Name) values ('ActivateDeactivateUsers', 'Активация и деактивация пользователей')
insert into [dbo].[Role] (Code, Name) values ('CheckCommonNationalCertificateWildcard', 'Проверка сертификатов ЕГЭ по неполным данным')
insert into [dbo].[Role] (Code, Name) values ('EditMinimalMarksDictionary', 'Редактирование справочника минимальных баллов')
insert into [dbo].[Role] (Code, Name) values ('EditExpireDatesDictionary', 'Редактирование справочника сроков действия')
insert into [dbo].[Role] (Code, Name) values ('ViewDeliveries', 'Просмотр рассылок')
insert into [dbo].[Role] (Code, Name) values ('EditDeliveries', 'Редактирование рассылок')
insert into [dbo].[Role] (Code, Name) values ('CreateOrganization', 'Добавление организаций в справочник')
--Добавляем Роли для Учредителей
insert into [dbo].[Role] (Code, Name) values ('ViewStatisticSubordinate', 'Получение статистики по подведомственным учреждениям')
--Роль для пользователей РЦОИ 
insert into [dbo].[Role] (Code, Name) values ('ViewStatisticRCOI', 'Получение статистики по ОУ, находящихся на территории субъекта РФ')
GO

DELETE [dbo].[Role]
WHERE Id = 3
GO

--Выбрать Назначение ролей группам пользователей для последующей вставки. Вставлять после создания таблицы
/*SELECT
	distinct T1.[Insert]+
	CASE T2.[Values]
		WHEN '' THEN ', null)'
		ELSE ', '''+T2.[Values]+''')'
	END
FROM
	(
	SELECT
		RoleId,
		GroupId,
		'insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (' + 
			cast(RoleId as varchar(50))+', '+cast(GroupId as varchar(50))+', '+
			cast(IsActive as varchar(50)) as [Insert]
	FROM
		[dbo].[GroupRole] GR
	) T1
	INNER JOIN
	(
	SELECT
		RoleId,
		GroupId,
		ISNULL(IsActiveCondition, '') as [Values]
	from 
		[dbo].[GroupRole] GR
	) T2
	ON T1.RoleId = T2.RoleId and T1.GroupId = T2.GroupId
GO*/


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES B WHERE B.TABLE_SCHEMA = 'dbo' AND  B.TABLE_NAME = 'GroupRole'))
  DROP TABLE [dbo].[GroupRole]
GO
	
CREATE TABLE [dbo].[GroupRole] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsActiveCondition] [nvarchar](max) NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO

--Вставка Ролей для групп пользователей
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (1, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (1, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (10, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (10, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (11, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (13, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (13, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (14, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (16, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (16, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (17, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (17, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (18, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (18, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (18, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (18, 5, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (19, 1, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (19, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (19, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (19, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (2, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (2, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (2, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (20, 1, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (20, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (20, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (20, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (21, 1, 1, 'isnull(account.HasCrocEgeIntegration, 0) = 1')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (21, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (21, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (21, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (22, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (22, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (22, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (23, 1, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (23, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (23, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (23, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (24, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (24, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (24, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (25, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (25, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (25, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (26, 1, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (26, 2, 0, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (26, 3, 0, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (26, 4, 0, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (27, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (27, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (27, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (28, 1, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (28, 2, 0, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (28, 3, 0, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (28, 4, 0, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (29, 1, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (29, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (30, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (31, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (31, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (32, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (32, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (32, 5, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (33, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (34, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (35, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (36, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (37, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (37, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (4, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (4, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (5, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (5, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (5, 4, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (6, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (6, 3, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (7, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (8, 2, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (9, 2, 1, null)
--вставка записей для Учредителей
	--Просмотр раздела Свидетельства
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (19, 6, 1, null)
	--просмотр статистики по подведомственным учреждениям 
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (38, 6, 1, null)
	--Добавление организаций в справочник
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (37, 6, 1, null)
--вставка записей для Пользователей РЦОИ
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (19, 7, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (20, 7, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (21, 7, 1, 'isnull(account.HasCrocEgeIntegration, 0) = 1')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (23, 7, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (26, 7, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (28, 7, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (29, 7, 1, null)
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (39, 7, 1, null)
GO

-- =============================================
-- Роли пользователей
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- =============================================
CREATE view [dbo].[AccountRole] with schemabinding
as
select 
	group_account.AccountId AccountId
	, [role].Code RoleCode
	, group_account.GroupId GroupId
	, group_role.RoleId RoleId
	, group_role.IsActiveCondition IsActiveCondition
from
	dbo.GroupAccount group_account
		inner join dbo.GroupRole group_role 
			on group_account.GroupId = group_role.GroupId
		inner join dbo.[Role] [role] 
			on [role].Id = group_role.RoleId
where
	group_role.IsActive = 1

GO


-- =============================================
-- Modified 29.01.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
ALTER PROCEDURE [dbo].[UpdateUserAccount]
@login NVARCHAR (255)=null OUTPUT, 
@passwordHash NVARCHAR (255)=null, 
@lastName NVARCHAR (255)=null, 
@firstName NVARCHAR (255)=null, 
@patronymicName NVARCHAR (255)=null, 
@organizationRegionId INT=null, 
@organizationFullName NVARCHAR (2000)=null,
@organizationShortName NVARCHAR (2000)=null, 
@organizationINN NVARCHAR (10)=null, 
@organizationOGRN NVARCHAR (13)=null, 
@organizationFounderName NVARCHAR (2000)=null, 
@organizationFactAddress NVARCHAR (500)=null,
@organizationLawAddress NVARCHAR (500)=null, 
@organizationDirName NVARCHAR (255)=null,
@organizationDirPosition NVARCHAR (500)=null, 
@organizationPhoneCode NVARCHAR (255)=null,
@organizationFax NVARCHAR (255)=null,
@organizationIsAccred bit=0,
@organizationIsPrivate bit=0,
@organizationIsFilial bit=0,
@organizationAccredSert nvarchar(255)=null,
@organizationEMail nvarchar(255)=null,
@organizationSite nvarchar(255)=null, 
@organizationPhone NVARCHAR (255)=null, 
@phone NVARCHAR (255)=null, 
@email NVARCHAR (255)=null, 
@ipAddresses NVARCHAR (4000)=null, 
@status NVARCHAR (255)=null OUTPUT, 
@registrationDocument IMAGE=null, 
@registrationDocumentContentType NVARCHAR (225)=null, 
@editorLogin NVARCHAR (255)=null, 
@editorIp NVARCHAR (255)=null, 
@password NVARCHAR (255)=null, 
@hasFixedIp BIT=null, 
@hasCrocEgeIntegration BIT=null, 
@organizationTypeId INT=null,
@organizationKindId INT=null, 
@ExistingOrgId INT=null
AS
begin
	
	declare 
		@newAccount bit
		, @accountId bigint
		, @currentYear int
		, @isOrganizationOwner bit
		, @organizationId bigint
		, @editorAccountId bigint
		, @departmentOwnershipCode nvarchar(255)
		, @eventCode nvarchar(100)
		, @userGroupId int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @useOnlyDocumentParam bit

	set @updateId = newid()
	
	if @organizationTypeId = 6
	begin
		select	@userGroupId = [group].[Id]
		from dbo.[Group] [group] with (nolock, fastfirstrow)
		where [group].[code] = N'UserDepartment'
	end
	else begin
		if @organizationTypeId = 3 
		begin
			select	@userGroupId = [group].[Id]
			from dbo.[Group] [group] with (nolock, fastfirstrow)
			where [group].[code] = N'UserRCOI' 
		end
		else begin
			select	@userGroupId = [group].[Id]
			from dbo.[Group] [group] with (nolock, fastfirstrow)
			where [group].[code] = N'User'
		end
	end

	declare @oldIpAddress table (ip nvarchar(255))
	declare @newIpAddress table (ip nvarchar(255))

	set @currentYear = year(getdate())
	set @departmentOwnershipCode = null

	select @editorAccountId = account.[Id]
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @editorLogin

	
	if isnull(@login, '') = ''
	begin 
		set @useOnlyDocumentParam = 1
		set @eventCode = N'USR_REG'
	end
	else
	begin
		set @useOnlyDocumentParam = 0
		set @eventCode = N'USR_EDIT'
	end

	if isnull(@login, '') = ''
		select top 1 @login = account.login
		from dbo.Account account with (nolock)
		where account.email = @email
			and dbo.GetUserStatus(@currentYear, 
				account.Status, account.ConfirmYear, account.RegistrationDocument) = 'registration'
		order by account.UpdateDate desc

	if isnull(@login, '') = '' -- внесение нового пользователя
	begin
		set @newAccount = 1

		exec dbo.GetNewUserLogin @login = @login output

		set @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		set @hasFixedIp = isnull(@hasFixedIp, 1)
		set @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, 0)
	end
	else
	begin -- update существующего пользователя
		
		select 
			@accountId = account.[Id]
			, @status = dbo.GetUserStatus(@currentYear, isnull(@status, account.Status), @currentYear
				, @registrationDocument)
			, @registrationDocument = isnull(@registrationDocument, case
				-- Если документ нельзя просмотривать, то считаем, что его нет.
				when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
					or @useOnlyDocumentParam = 1 
					or isnull(datalength(account.RegistrationDocument),0)=0 
					then null
				else account.RegistrationDocument
			end)
			, @registrationDocumentContentType = case
				when not @registrationDocument is null then @registrationDocumentContentType
				-- Если документ нельзя просмотривать, то считаем, что его нет.
				when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
					or @useOnlyDocumentParam = 1 				
					then null
				else account.RegistrationDocumentContentType
			end
			, @isOrganizationOwner = account.IsOrganizationOwner
			, @organizationId = account.OrganizationID
			, @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
			, @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, account.HasCrocEgeIntegration)
		from dbo.Account account with (nolock, fastfirstrow)
		where account.[Login] = @login


		if @accountId is null
			return 0

		
		insert @oldIpAddress(ip)
		select account_ip.ip
		from dbo.AccountIp account_ip with (nolock, fastfirstrow)
		where account_ip.AccountId = @accountId
	end

	if @hasFixedIp = 1
		insert @newIpAddress(ip)
		select ip_addresses.[value]
		from dbo.GetDelimitedValues(@ipAddresses) ip_addresses

	begin tran insert_update_account_tran

		
		if @newAccount = 1 -- внесение нового пользователя
		begin
--			if isnull(@organizationTypeId, 0) = 0
--				set @educationInstitutionTypeId = dbo.GetOrganizationEducationInstitutionTypeIdByName(@organizationFullName) 

--			insert dbo.Organization
--				(
--				CreateDate
--				, UpdateDate
--				, UpdateId
--				, EditorAccountId
--				, EditorIp
--				, RegionId
--				, DepartmentOwnershipCode
--				, [Name]
--				, FounderName
--				, Address
--				, ChiefName
--				, Fax
--				, Phone
--				, ShortName
--				, EducationInstitutionTypeId
--				, EtalonOrgId
--				)
--			select
--				getdate()
--				, getdate()
--				, @updateId
--				, @editorAccountId
--				, @editorIp
--				, @organizationRegionId
--				, @departmentOwnershipCode
--				, @organizationFullName
--				, @organizationFounderName
--				, @organizationLawAddress
--				, @organizationDirName
--				, @organizationFax
--				, @organizationPhone
--				, dbo.GetShortOrganizationName(@organizationFullName)
--				, @organizationTypeId
--				, @ExistingOrgId


			insert dbo.OrganizationRequest2010
				(
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
				OrganizationId
				)
			select
				@organizationFullName,
				@organizationShortName,
				@organizationRegionId,
				@organizationTypeId,
				@organizationKindId,
				@organizationINN,
				@organizationOGRN,		
				@organizationFounderName,
				@organizationIsPrivate,
				@organizationIsFilial,
				@organizationDirPosition,
				@organizationDirName,
				@organizationIsAccred,
				@organizationAccredSert,
				@organizationLawAddress,
				@organizationFactAddress,
				@organizationPhoneCode,
				@organizationPhone,
				@organizationFax,
				@organizationEMail,
				@organizationSite,	
				@ExistingOrgId
				 
			if (@@error <> 0)
				goto undo

			select @organizationId = scope_identity()

			if (@@error <> 0)
				goto undo

			insert dbo.Account
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, [Login]
				, PasswordHash
				, LastName
				, FirstName
				, PatronymicName
				, OrganizationId
				, IsOrganizationOwner
				, ConfirmYear
				, Phone
				, Email
				, RegistrationDocument
				, RegistrationDocumentContentType
				, AdminComment
				, IsActive
				, Status
				, IpAddresses
				, HasFixedIp
				, HasCrocEgeIntegration
				)
			select
				GetDate()
				, GetDate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @login
				, @passwordHash
				, @lastName
				, @firstName
				, @patronymicName
				, @organizationId
				, 1
				, @currentYear
				, @phone
				, @email
				, @registrationDocument
				, @registrationDocumentContentType
				, null
				, 1
				, @status
				, @ipAddresses
				, @hasFixedIp
				, @hasCrocEgeIntegration

			if (@@error <> 0)
				goto undo

			select @accountId = scope_identity()

			if (@@error <> 0)
				goto undo

			insert dbo.AccountIp(AccountId, Ip)
			select	@accountId, new_ip_address.ip
			from @newIpAddress new_ip_address

			if (@@error <> 0)
				goto undo

			insert dbo.GroupAccount(GroupId, AccountID)
			select	@UserGroupId, @accountId

			if (@@error <> 0)
				goto undo
		end	
		else 
		begin -- update существующего пользователя
			if @isOrganizationOwner = 1
--				update organization
--				set 
--					UpdateDate = GetDate()
--					, UpdateId = @updateId
--					, EditorAccountId = @editorAccountId
--					, EditorIp = @editorIp
--					, RegionId = @organizationRegionId
--					, DepartmentOwnershipCode = @departmentOwnershipCode
--					, [Name] = @organizationFullName
--					, FounderName = @organizationFounderName
--					, Address = @organizationLawAddress
--					, ChiefName = @organizationDirName
--					, Fax = @organizationFax
--					, Phone = @organizationPhone
--					, ShortName = dbo.GetShortOrganizationName(@organizationFullName)
--					, EducationInstitutionTypeId = @organizationTypeId
--					, EtalonOrgId=@ExistingOrgId
--				from 
--					dbo.Organization organization with (rowlock)
--				where
--					organization.[Id] = @organizationId

				update OReq
				set 
					UpdateDate = GetDate(),
					FullName=@organizationFullName,
					ShortName=@organizationShortName,
					RegionId=@organizationRegionId,
					TypeId=@organizationTypeId,
					KindId=@organizationKindId,
					INN=@organizationINN,
					OGRN=@organizationOGRN,		
					OwnerDepartment=@organizationFounderName,
					IsPrivate=@organizationIsPrivate,
					IsFilial=@organizationIsFilial,
					DirectorPosition=@organizationDirPosition,
					DirectorFullName=@organizationDirName,
					IsAccredited=@organizationIsAccred,
					AccreditationSertificate=@organizationAccredSert,
					LawAddress=@organizationLawAddress,
					FactAddress=@organizationFactAddress,
					PhoneCityCode=@organizationPhoneCode,
					Phone=@organizationPhone,
					Fax=@organizationFax,
					EMail=@organizationEMail,
					Site=@organizationSite,	
					OrganizationId=@ExistingOrgId
				from 
					dbo.OrganizationRequest2010 OReq with (rowlock)
				where
					OReq.[Id] = @organizationId

			if (@@error <> 0)
				goto undo

			update account
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, PasswordHash=isnull(@passwordHash,PasswordHash)
				, EditorIp = @editorIp
				, LastName = @lastName
				, FirstName = @firstName
				, PatronymicName = @patronymicName
				, Phone = @phone
				, Email = @email
				, ConfirmYear = @currentYear
				, Status = @status
				, IpAddresses = @ipAddresses
				, RegistrationDocument = @registrationDocument
				, RegistrationDocumentContentType = @registrationDocumentContentType
				, HasFixedIp = @hasFixedIp
				, HasCrocEgeIntegration = @hasCrocEgeIntegration
			from dbo.Account account with (rowlock)
			where account.[Id] = @accountId

			if (@@error <> 0)
				goto undo

			if exists(	select 1 
						from @oldIpAddress old_ip_address
						full outer join @newIpAddress new_ip_address
						on old_ip_address.ip = new_ip_address.ip
						where old_ip_address.ip is null
							or new_ip_address.ip is null) 
			begin
				delete account_ip
				from dbo.AccountIp account_ip
				where account_ip.AccountId = @accountId

				if (@@error <> 0)
					goto undo

				insert dbo.AccountIp(AccountId, Ip)
				select @accountId, new_ip_address.ip
				from @newIpAddress new_ip_address

				if (@@error <> 0)
					goto undo
			end
		end	

-- временно
	if isnull(@password, '') <> '' 
	begin
		if exists(select 1 
				from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId)
		begin
			update user_account_password
			set [Password] = @password
			from dbo.UserAccountPassword user_account_password
			where user_account_password.AccountId = @accountId

			if (@@error <> 0)
				goto undo
		end
		else
		begin
			insert dbo.UserAccountPassword(AccountId, [Password])
			select @accountId, @password

			if (@@error <> 0)
				goto undo
		end
	end

	if @@trancount > 0
		commit tran insert_update_account_tran

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RefreshRoleActivity @accountId = @accountId

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	return 0

	undo:
	rollback tran insert_update_account_tran
	return 1
end
GO
