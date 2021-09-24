ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD CONSTRAINT [FK_OrganizationUpdateHistory_Organization2010] FOREIGN KEY ([NewOrgId]) REFERENCES [dbo].[Organization2010] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

