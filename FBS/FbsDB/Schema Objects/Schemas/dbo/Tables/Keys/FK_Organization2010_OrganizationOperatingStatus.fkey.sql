ALTER TABLE [dbo].[Organization2010]
    ADD CONSTRAINT [FK_Organization2010_OrganizationOperatingStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[OrganizationOperatingStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

