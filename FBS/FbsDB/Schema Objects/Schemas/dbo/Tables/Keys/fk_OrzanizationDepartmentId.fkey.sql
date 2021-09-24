ALTER TABLE [dbo].[Organization2010]
    ADD CONSTRAINT [fk_OrzanizationDepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Organization2010] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

