ALTER TABLE [dbo].[CommonNationalExamCertificateRequestBatch]
    ADD CONSTRAINT [MSrepl_tran_version_default_E4E249EA_F5AD_40F8_B07A_CA9ABC5DDA32_1933249942] DEFAULT (newid()) FOR [msrepl_tran_version];

