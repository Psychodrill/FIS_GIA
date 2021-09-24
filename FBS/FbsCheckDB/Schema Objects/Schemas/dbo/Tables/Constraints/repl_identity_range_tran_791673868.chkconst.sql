ALTER TABLE [dbo].[CommonNationalExamCertificateCheckBatch]
    ADD CONSTRAINT [repl_identity_range_tran_791673868] CHECK NOT FOR REPLICATION ([Id]>(220000) AND [Id]<(230000));

