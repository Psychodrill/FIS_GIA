ALTER TABLE [dbo].[Organization2010]
    ADD CONSTRAINT [Organization2010_fk] FOREIGN KEY ([RCModel]) REFERENCES [dbo].[RecruitmentCampaigns] ([Id]) ON DELETE NO ACTION ON UPDATE CASCADE;

