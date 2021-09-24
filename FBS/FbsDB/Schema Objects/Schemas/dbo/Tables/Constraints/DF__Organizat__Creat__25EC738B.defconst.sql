ALTER TABLE [dbo].[Organization2010]
    ADD CONSTRAINT [DF__Organizat__Creat__25EC738B] DEFAULT (getdate()) FOR [CreateDate];

