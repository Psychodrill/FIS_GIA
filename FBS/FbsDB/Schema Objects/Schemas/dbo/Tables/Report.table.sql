CREATE TABLE [dbo].[Report] (
    [id]       INT           IDENTITY (1, 1) NOT NULL,
    [name]     VARCHAR (100) NOT NULL,
    [created]  DATETIME      NOT NULL,
    [xml]      XML           NOT NULL,
    [dateFrom] DATETIME      NULL,
    [dateTo]   DATETIME      NULL
);

