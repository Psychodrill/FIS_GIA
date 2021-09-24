ALTER TABLE [dbo].[OperatorLog]
    ADD CONSTRAINT [DF_OperatorLog_DTLastChange] DEFAULT (getdate()) FOR [DTLastChange];

