ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD CONSTRAINT [FK_OrganizationUpdateHistory_TypeI_28C8E036] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[OrganizationType2010] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

