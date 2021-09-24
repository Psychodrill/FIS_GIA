ALTER TABLE [dbo].[CommonNationalExamCertificateForm]
    ADD CONSTRAINT [chk_staging_CommonNationalExamCertificateForm_20100511-151637_partition_1] CHECK ([Partition]<=N'2008044');

