-- аккаунты пользователей
CREATE TABLE [dbo].[Account] (
    [Id]                              BIGINT           IDENTITY (1, 1) NOT NULL, -- id 
    [CreateDate]                      DATETIME         NOT NULL, -- дата создания
    [UpdateDate]                      DATETIME         NOT NULL, -- дата последнего изменения
    [UpdateId]                        UNIQUEIDENTIFIER NOT NULL, -- id создания\изменения
    [EditorAccountId]                 BIGINT           NULL,	 -- id аккаунта создавшего\изменившего
    [EditorIp]                        NVARCHAR (255)   NULL,	 -- ip создавшего\изменившего
    [Login]                           NVARCHAR (255)   NOT NULL, -- логин 
    [PasswordHash]                    NVARCHAR (255)   NULL,	 -- хэш пароля -- зачем он здесь?
    [LastName]                        NVARCHAR (255)   NULL,	 -- фамилия
    [FirstName]                       NVARCHAR (255)   NULL,	 -- имя
    [PatronymicName]                  NVARCHAR (255)   NULL,	 -- отчество
    [OrganizationId]                  BIGINT           NULL,	 -- id организации
    [IsOrganizationOwner]             BIT              NULL,	 -- является ли владельцем организации
    [ConfirmYear]                     INT              NULL,	 -- год подтверждения аккунта
    [Phone]                           NVARCHAR (255)   NULL,	 -- телефон
    [Email]                           NVARCHAR (255)   NULL,	 -- почта
    [RegistrationDocument]            IMAGE            NULL,	 -- регистрационный документ (скан)
    [RegistrationDocumentContentType] NVARCHAR (255)   NULL,	 -- тип регистрационного документа
    [AdminComment]                    NTEXT            NULL,	 -- комментарий админа
    [IsActive]                        BIT              NOT NULL, -- активен ли аккаунт
    [Status]                          NVARCHAR (255)   NULL,	 -- статус (по мере подтверждения)
    [IpAddresses]                     NVARCHAR (4000)  NULL,	 -- ip адреса пользователя
    [HasFixedIp]                      BIT              NULL,	 -- фиксирован ли ip входа пользователя в систему
    [HasCrocEgeIntegration]           BIT              NULL,	 -- активирована ли интеграция с АИС Экзамен
	[IsBanned]						  BIT              not null default(0) 
);


