CREATE TABLE [dbo].[OperatorLog] (
    [CheckedUserID] INT            NOT NULL,
    [OperatorID]    INT            NOT NULL,
    [Comments]      VARCHAR (1024) NULL,
    [DTCreate]      DATETIME       NOT NULL,
    [DTLastChange]  DATETIME       NOT NULL
);

