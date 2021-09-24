ALTER TABLE [dbo].[OrganizationRequest2010]
    ADD CONSTRAINT [FK__Organizat__Regio__2E81B98C] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Region] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

