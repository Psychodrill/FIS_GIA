CREATE TABLE [dbo].[GroupRole] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]            INT            NOT NULL,
    [GroupId]           INT            NOT NULL,
    [IsActive]          BIT            NOT NULL,
    [IsActiveCondition] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

