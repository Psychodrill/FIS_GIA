ALTER TABLE [dbo].[Organization2010]
    ADD CONSTRAINT [FK_Organization2010_Organization2010] FOREIGN KEY ([NewOrgId]) REFERENCES [dbo].[Organization2010] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

