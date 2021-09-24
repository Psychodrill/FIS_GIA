ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD CONSTRAINT [FK_OrganizationUpdateHistory_OrganizationOperatingStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[OrganizationOperatingStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

