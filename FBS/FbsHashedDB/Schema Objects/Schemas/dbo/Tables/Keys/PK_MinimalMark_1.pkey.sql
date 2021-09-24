﻿ALTER TABLE [dbo].[MinimalMark]
    ADD CONSTRAINT [PK_MinimalMark_1] PRIMARY KEY CLUSTERED ([SubjectId] ASC, [Year] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

