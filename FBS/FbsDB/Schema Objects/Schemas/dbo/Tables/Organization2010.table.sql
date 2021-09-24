-- образовательное учреждение
CREATE TABLE [dbo].[Organization2010] (
    [Id]                       INT             IDENTITY (1, 1) NOT NULL,	-- дата создания (в бд)
    [CreateDate]               DATETIME        NOT NULL,	-- дата последнего обновления (в бд)
    [UpdateDate]               DATETIME        NOT NULL,	-- полное название
    [FullName]                 NVARCHAR (1000) NOT NULL,	-- сокращенное название
    [ShortName]                NVARCHAR (500)  NULL,	-- ссылка на регион
    [RegionId]                 INT             NOT NULL,	-- ссылка на тип 
    [TypeId]                   INT             NOT NULL,	-- ссылка на вид
    [KindId]                   INT             NOT NULL,	-- инн
    [INN]                      NVARCHAR (10)   NULL,	-- Основной государственный регистрационный номер
    [OGRN]                     NVARCHAR (13)   NULL,	-- организация-владелец 
    [OwnerDepartment]          NVARCHAR (500)  NULL,	-- частное ли это учреждение
    [IsPrivate]                BIT             NOT NULL,	-- филиал ли это
    [IsFilial]                 BIT             NOT NULL,	-- должность директора
    [DirectorPosition]         NVARCHAR (255)  NULL,	-- имя директора
    [DirectorFullName]         NVARCHAR (255)  NULL,	-- акредитовано ли
    [IsAccredited]             BIT             NULL,	-- сертификат аккредитации
    [AccreditationSertificate] NVARCHAR (255)  NULL,	-- юридический адрес
    [LawAddress]               NVARCHAR (255)  NULL,	-- фактический адрес
    [FactAddress]              NVARCHAR (255)  NOT NULL,	-- телефонный код города
    [PhoneCityCode]            NVARCHAR (10)   NULL,	-- телефон
    [Phone]                    NVARCHAR (100)  NULL,	-- факс
    [Fax]                      NVARCHAR (100)  NULL,	-- адрес эл.почты
    [EMail]                    NVARCHAR (100)  NULL,	-- сайт организации
    [Site]                     NVARCHAR (40)   NULL,	-- 1 - если была создана из справочника
    [WasImportedAtStart]       BIT             NOT NULL,    -- характеристика оу
	[RCModel]                  INT             NULL,	-- характеристика оу
    [RCDescription]            NVARCHAR (400)  NULL,	-- ссылка на головную организацию
    [MainId]                   INT             NULL,
	[DepartmentId]             INT             NULL,    -- характеристика оу
	[CNFBFullTime]             INT             DEFAULT ((0)) NOT NULL,    -- характеристика оу
	[CNFBEvening]              INT             DEFAULT ((0)) NOT NULL,    -- характеристика оу
	[CNFBPostal]               INT             DEFAULT ((0)) NOT NULL,    -- характеристика оу
	[CNPayFullTime]            INT             DEFAULT ((0)) NOT NULL,    -- характеристика оу
	[CNPayEvening]             INT             DEFAULT ((0)) NOT NULL,    -- характеристика оу
	[CNPayPostal]              INT             DEFAULT ((0)) NOT NULL,    -- характеристика оу
	[CNFederalBudget]          INT             NULL,    -- характеристика оу
	[CNTargeted]               INT             NULL,    -- характеристика оу
	[CNLocalBudget]            INT             NULL,    -- характеристика оу
	[CNPaying]                 INT             NULL,    -- характеристика оу
	[CNFullTime]               INT             NULL,    -- характеристика оу
	[CNEvening]                INT             NULL,    -- характеристика оу
	[CNPostal]                 INT             NULL,    -- ссылка статус 
	[StatusId]                 INT             DEFAULT ((1)) NOT NULL,	-- ссылка на организацию, в которую была реорганизована данная организация
    [NewOrgId]                 INT             NULL,	-- текущая версия оу
    [Version]                  INT             DEFAULT ((1)) NOT NULL,
	[DisableLog]			   BIT			   DEFAULT ((0)) NOT NULL	-- отключить журналирование, по умолчанию 0, т.е. включено	
);

