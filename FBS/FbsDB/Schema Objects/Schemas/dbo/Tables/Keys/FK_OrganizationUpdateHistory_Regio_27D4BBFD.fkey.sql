ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD CONSTRAINT [FK_OrganizationUpdateHistory_Regio_27D4BBFD] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Region] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

