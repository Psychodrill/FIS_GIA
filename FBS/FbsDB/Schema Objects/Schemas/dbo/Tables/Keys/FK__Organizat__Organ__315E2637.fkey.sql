ALTER TABLE [dbo].[OrganizationRequest2010]
    ADD CONSTRAINT [FK__Organizat__Organ__315E2637] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization2010] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

