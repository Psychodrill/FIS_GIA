ALTER TABLE [dbo].[CommonNationalExamCertificateCheckBatch]
    ADD CONSTRAINT [MSrepl_tran_version_default_50690ED7_7B85_4873_9057_F2E668300FFB_809769942] DEFAULT (newid()) FOR [msrepl_tran_version];

