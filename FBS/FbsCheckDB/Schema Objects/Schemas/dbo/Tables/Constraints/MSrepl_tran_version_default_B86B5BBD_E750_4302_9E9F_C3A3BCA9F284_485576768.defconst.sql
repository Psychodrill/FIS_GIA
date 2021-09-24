ALTER TABLE [dbo].[CommonNationalExamCertificateRequest]
    ADD CONSTRAINT [MSrepl_tran_version_default_B86B5BBD_E750_4302_9E9F_C3A3BCA9F284_485576768] DEFAULT (newid()) FOR [msrepl_tran_version];

