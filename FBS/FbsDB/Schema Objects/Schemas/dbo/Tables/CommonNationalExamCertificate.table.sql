CREATE TABLE [dbo].[CommonNationalExamCertificate] (
    [Id]                       BIGINT           NOT NULL, -- id сертификата
    [CreateDate]               DATETIME         NOT NULL, -- дата внесения в систему
    [UpdateDate]               DATETIME         NOT NULL, -- дата последнего изменения
    [UpdateId]                 UNIQUEIDENTIFIER NOT NULL, -- что это ?
    [EditorAccountId]          BIGINT           NOT NULL, -- id добавившего\изменившего сертификат. Не совсем понятно куда ссылается этот ключ. Берется из конфогов в лоадере.
    [EditorIp]                 NVARCHAR (255)   NOT NULL, -- ip изменившего сертификат, так же
    [Number]                   NVARCHAR (255)   NOT NULL, -- номер
    [EducationInstitutionCode] NVARCHAR (255)   NOT NULL, -- код учебной организации
    [Year]                     INT              NOT NULL, -- год выдачи
    [LastName]                 NVARCHAR (255)   NOT NULL, -- фамилия сертифицируемого
    [FirstName]                NVARCHAR (255)   NOT NULL, -- имя сертифицируемого
    [PatronymicName]           NVARCHAR (255)   NOT NULL, -- отчество сертифицируемого
    [Sex]                      BIT              NOT NULL, -- пол сертифицируемого 
    [Class]                    NVARCHAR (255)   NOT NULL, -- школьное обозначение класса сертифицируемого ("11A")
    [InternalPassportSeria]    NVARCHAR (255)   NOT NULL, -- внутренняя серия паспорта сертифицируемого (внутреняя ? :-))
    [PassportSeria]            NVARCHAR (255)   NOT NULL, -- серия паспорта сертифицируемого
    [PassportNumber]           NVARCHAR (255)   NOT NULL, -- номер паспорта сертифицируемого
    [EntrantNumber]            NVARCHAR (255)   NULL,	  -- номер сертифицируемого (что это?)
    [RegionId]                 INT              NOT NULL, -- id региона
    [TypographicNumber]        NVARCHAR (255)   NULL,	  -- типографический номер
	[FIO]  AS (replace(replace((ltrim(rtrim([LastName]))+ltrim(rtrim([FirstName])))+ltrim(rtrim([PatronymicName])),'ё','е'),' ','')) PERSISTED
);

GO

CREATE NONCLUSTERED INDEX [idx_CommonNationalExamCertificate_fio] ON [dbo].[CommonNationalExamCertificate] 
(
	[FIO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


