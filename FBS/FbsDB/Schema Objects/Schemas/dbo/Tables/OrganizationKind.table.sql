-- справочник дополтительных типов (видов) ОУ
CREATE TABLE [dbo].[OrganizationKind] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
	-- название
    [Name]      NVARCHAR (30) NOT NULL,
	-- порядок при сортировке в UI 
    [SortOrder] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

