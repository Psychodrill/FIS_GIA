ALTER TABLE [dbo].[Report]
    ADD CONSTRAINT [DF_Report_created] DEFAULT (getdate()) FOR [created];

