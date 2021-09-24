ALTER TABLE [dbo].[OrganizationRequest2010]
    ADD CONSTRAINT [DF__Organizat__Creat__2C99711A] DEFAULT (getdate()) FOR [CreateDate];

