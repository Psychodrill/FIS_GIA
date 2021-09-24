ALTER TABLE [dbo].[OrganizationRequest2010]
    ADD CONSTRAINT [DF__Organizat__Updat__2D8D9553] DEFAULT (getdate()) FOR [UpdateDate];

