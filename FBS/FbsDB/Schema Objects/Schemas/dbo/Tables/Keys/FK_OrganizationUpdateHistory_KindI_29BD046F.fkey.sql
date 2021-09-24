ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD CONSTRAINT [FK_OrganizationUpdateHistory_KindI_29BD046F] FOREIGN KEY ([KindId]) REFERENCES [dbo].[OrganizationKind] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

