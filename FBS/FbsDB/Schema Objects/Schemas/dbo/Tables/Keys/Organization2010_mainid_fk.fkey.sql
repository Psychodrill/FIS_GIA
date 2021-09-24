ALTER TABLE [dbo].[Organization2010]
    ADD CONSTRAINT [Organization2010_mainid_fk] FOREIGN KEY ([MainId]) REFERENCES [dbo].[Organization2010] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

