ALTER TABLE [dbo].[Organization2010]
    ADD CONSTRAINT [DF__Organizat__Updat__26E097C4] DEFAULT (getdate()) FOR [UpdateDate];

