CREATE NONCLUSTERED INDEX [IX_CNEWebUICheckLog_EventDate_AccountId]
ON [dbo].[CNEWebUICheckLog] ([EventDate])
INCLUDE ([AccountId])


