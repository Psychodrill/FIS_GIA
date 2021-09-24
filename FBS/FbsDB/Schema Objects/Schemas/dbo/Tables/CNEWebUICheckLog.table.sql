-- логирование интерактивной проверки свидетельств ЕГЕ
CREATE TABLE [dbo].[CNEWebUICheckLog] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,	 -- id записи
    [AccountId]         BIGINT         NOT NULL,					 -- id аккунта проверяющего
    [EventDate]         DATETIME       DEFAULT (getdate()) NOT NULL, -- дата проверки
    [TypeCode]          NVARCHAR (20)  NOT NULL,					 -- тип проверки
    [TypographicNumber] NVARCHAR (20)  NULL,						 -- типографический номер проверяемого свидетельства
    [PassportSeria]     NVARCHAR (20)  NULL,						 -- серия паспорта владельца свидетельства
    [PassportNumber]    NVARCHAR (20)  NULL,						 -- номер паспотра владельца свидетельства
    [CNENumber]         NVARCHAR (20)  NULL,					     -- номер свидетельства
    [LastName]          NVARCHAR (255) NULL,						 -- фамилия владельца свидетельства
    [FirstName]         NVARCHAR (255) NULL,						 -- имя владельца свидетельства
    [PatronymicName]    NVARCHAR (255) NULL,						 -- отчетсво владельца свидетельства 
    [Marks]             NVARCHAR (500) DEFAULT (NULL) NULL,			 -- указанные в проверке оценки
    [FoundedCNEId]      NVARCHAR (255) NULL,						 -- id найденного свидетельства
    FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO
ALTER TABLE [dbo].[CNEWebUICheckLog]
    ADD CONSTRAINT [PKCNEWebUICheckLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);