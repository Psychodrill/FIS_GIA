ALTER TABLE [dbo].[OperatorLog]
    ADD CONSTRAINT [DF_OperatorLog_DTCreate] DEFAULT (getdate()) FOR [DTCreate];

