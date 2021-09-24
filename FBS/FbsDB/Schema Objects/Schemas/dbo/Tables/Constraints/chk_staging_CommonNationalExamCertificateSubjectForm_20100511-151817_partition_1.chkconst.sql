ALTER TABLE [dbo].[CommonNationalExamCertificateSubjectForm]
    ADD CONSTRAINT [chk_staging_CommonNationalExamCertificateSubjectForm_20100511-151817_partition_1] CHECK ([Partition]<=N'2008037');

