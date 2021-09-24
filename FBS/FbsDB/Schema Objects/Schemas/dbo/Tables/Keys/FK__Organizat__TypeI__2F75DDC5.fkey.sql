ALTER TABLE [dbo].[OrganizationRequest2010]
    ADD CONSTRAINT [FK__Organizat__TypeI__2F75DDC5] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[OrganizationType2010] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

