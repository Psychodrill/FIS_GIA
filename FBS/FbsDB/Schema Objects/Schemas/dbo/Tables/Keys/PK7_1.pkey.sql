ALTER TABLE [dbo].[AccountLog]
    ADD CONSTRAINT [PK7_1] PRIMARY KEY CLUSTERED ([AccountId] ASC, [VersionId] ASC) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

