ALTER TABLE [dbo].[ImportingCommonNationalExamCertificateSubject]
    ADD CONSTRAINT [ImportingCertificateSubjectPK] PRIMARY KEY CLUSTERED ([Year] ASC, [CertificateId] ASC, [SubjectId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

