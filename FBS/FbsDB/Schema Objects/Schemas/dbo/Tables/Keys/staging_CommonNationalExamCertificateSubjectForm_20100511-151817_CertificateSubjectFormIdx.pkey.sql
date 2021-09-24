ALTER TABLE [dbo].[CommonNationalExamCertificateSubjectForm]
    ADD CONSTRAINT [staging_CommonNationalExamCertificateSubjectForm_20100511-151817_CertificateSubjectFormIdx] PRIMARY KEY CLUSTERED ([Partition] ASC, [FormId] ASC, [SubjectId] ASC) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

