--use [gvuz_develop]
--GO

SET XACT_ABORT ON

begin tran

	CREATE TABLE [dbo].[LevelBudget](
		[IdLevelBudget] [int] NOT NULL,
		[BudgetName] [varchar](500) NOT NULL,
	CONSTRAINT [PK_LevelBudget] PRIMARY KEY CLUSTERED 
	(
		[IdLevelBudget] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) 
	ON [PRIMARY];

    insert into [dbo].[LevelBudget]([IdLevelBudget], [BudgetName]) values(1, 'Федеральный');
	insert into [dbo].[LevelBudget]([IdLevelBudget], [BudgetName]) values(2, 'Региональный');
	insert into [dbo].[LevelBudget]([IdLevelBudget], [BudgetName]) values(3, 'Муниципальный');

	CREATE TABLE [dbo].[DistributedAdmissionVolume](
		[DistributedAdmissionVolumeID] [int] IDENTITY(1,1) NOT NULL,
		[AdmissionVolumeID] [int] NOT NULL,
		[IdLevelBudget] [int] NOT NULL,
		[NumberBudgetO] [int] NOT NULL,
		[NumberBudgetOZ] [int] NOT NULL,
		[NumberBudgetZ] [int] NOT NULL,
		[NumberQuotaO] [int] NOT NULL,
		[NumberQuotaOZ] [int] NOT NULL,
		[NumberQuotaZ] [int] NOT NULL,
		[NumberTargetO] [int] NOT NULL,
		[NumberTargetOZ] [int] NOT NULL,
		[NumberTargetZ] [int] NOT NULL,
	 CONSTRAINT [PK_DistributedAdmissionVolume] PRIMARY KEY CLUSTERED 
	(
		[DistributedAdmissionVolumeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];

ALTER TABLE [dbo].[DistributedAdmissionVolume]  WITH CHECK ADD  CONSTRAINT [FK_DistributedAdmissionVolume_AdmissionVolume] FOREIGN KEY([AdmissionVolumeID])
REFERENCES [dbo].[AdmissionVolume] ([AdmissionVolumeID]) ON DELETE CASCADE;

ALTER TABLE [dbo].[DistributedAdmissionVolume] CHECK CONSTRAINT [FK_DistributedAdmissionVolume_AdmissionVolume];

ALTER TABLE [dbo].[DistributedAdmissionVolume]  WITH CHECK ADD  CONSTRAINT [FK_DistributedAdmissionVolume_LevelBudget] FOREIGN KEY([IdLevelBudget])
REFERENCES [dbo].[LevelBudget] ([IdLevelBudget]);

ALTER TABLE [dbo].[DistributedAdmissionVolume] CHECK CONSTRAINT [FK_DistributedAdmissionVolume_LevelBudget];

commit tran

SET XACT_ABORT OFF