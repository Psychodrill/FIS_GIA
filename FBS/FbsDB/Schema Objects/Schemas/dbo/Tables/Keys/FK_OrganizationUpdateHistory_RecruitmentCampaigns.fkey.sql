ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD CONSTRAINT [FK_OrganizationUpdateHistory_RecruitmentCampaigns] FOREIGN KEY ([RCModel]) REFERENCES [dbo].[RecruitmentCampaigns] ([Id]) ON DELETE NO ACTION ON UPDATE CASCADE;

