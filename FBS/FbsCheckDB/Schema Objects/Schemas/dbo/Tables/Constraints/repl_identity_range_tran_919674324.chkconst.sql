ALTER TABLE [dbo].[CommonNationalExamCertificateRequestBatch]
    ADD CONSTRAINT [repl_identity_range_tran_919674324] CHECK NOT FOR REPLICATION ([Id]>(220000) AND [Id]<(230000));

