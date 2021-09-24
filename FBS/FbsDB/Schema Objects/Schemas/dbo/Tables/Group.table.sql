-- группы пользователей 
CREATE TABLE [dbo].[Group] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
	-- код
    [Code]        NVARCHAR (255) NOT NULL,
	-- название
    [Name]        NVARCHAR (255) NOT NULL,
	-- какаято жуткая вещь. не особо понял, связь между группами в есрп и фбс. что как зачем....
    [GroupIdEsrp] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

