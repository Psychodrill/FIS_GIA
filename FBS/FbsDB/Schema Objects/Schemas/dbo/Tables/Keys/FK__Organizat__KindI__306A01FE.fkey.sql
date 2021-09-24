ALTER TABLE [dbo].[OrganizationRequest2010]
    ADD CONSTRAINT [FK__Organizat__KindI__306A01FE] FOREIGN KEY ([KindId]) REFERENCES [dbo].[OrganizationKind] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

