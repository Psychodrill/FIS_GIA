﻿ALTER TABLE [dbo].[Report]
    ADD CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED ([id] ASC, [name] ASC, [created] ASC) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
