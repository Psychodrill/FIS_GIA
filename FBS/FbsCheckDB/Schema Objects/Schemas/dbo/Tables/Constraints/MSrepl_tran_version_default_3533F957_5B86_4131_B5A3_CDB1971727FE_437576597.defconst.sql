ALTER TABLE [dbo].[CommonNationalExamCertificateCheck]
    ADD CONSTRAINT [MSrepl_tran_version_default_3533F957_5B86_4131_B5A3_CDB1971727FE_437576597] DEFAULT (newid()) FOR [msrepl_tran_version];

