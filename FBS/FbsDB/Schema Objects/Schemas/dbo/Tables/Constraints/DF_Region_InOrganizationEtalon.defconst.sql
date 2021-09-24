ALTER TABLE [dbo].[Region]
    ADD CONSTRAINT [DF_Region_InOrganizationEtalon] DEFAULT ((1)) FOR [InOrganizationEtalon];

