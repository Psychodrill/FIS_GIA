ALTER TABLE [dbo].[Organization2010]
    ADD CONSTRAINT [FK__Organizat__KindI__29BD046F] FOREIGN KEY ([KindId]) REFERENCES [dbo].[OrganizationKind] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

