ALTER TABLE [dbo].[DeprecatedCommonNationalExamCertificateSubject]
    ADD CONSTRAINT [DeprecatedCertificateSubjectPK] PRIMARY KEY CLUSTERED ([Year] ASC, [CertificateId] ASC, [SubjectId] ASC) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

