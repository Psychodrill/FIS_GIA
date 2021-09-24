-- Регионы РФ
CREATE TABLE [dbo].[Region] (
	[Id]                   INT            NOT NULL,
	-- код региона в РФ
	[Code]                 NVARCHAR (255) NOT NULL,
	-- название региона
	[Name]                 NVARCHAR (255) NOT NULL,
	-- походу нигде больше не используется. Раньше можно было использовать при регистрации организации в регионе, но теперь регистрация перенесена в ЕСРП
	[InOrganization]       BIT            NOT NULL,
	-- если 1, то данный регион участвует в сертификации 
	[InCertificate]        BIT            NOT NULL,
	-- порядок сортирвовки при формирвании отчетов по регионам 
	[SortIndex]            TINYINT        NOT NULL,
	-- если 1, то данный регион может быть использован при регистрации организации 
	[InOrganizationEtalon] BIT            NOT NULL,
	-- ссылка на федеральный округ
	[FederalDistrictId]    INT            DEFAULT ((0)) NOT NULL,
	FOREIGN KEY ([FederalDistrictId]) REFERENCES [dbo].[FederalDistricts] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

