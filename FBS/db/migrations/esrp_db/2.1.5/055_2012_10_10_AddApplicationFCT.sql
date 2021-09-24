-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (55, '055_2012_10_10_AddApplicationFCT')
go


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- ���������� ����� �������� � �� ���������� 
CREATE TABLE DictOperationSystem 	([ID] [int] IDENTITY(1,1) NOT NULL, [Name] [nvarchar](128) NOT NULL,  CONSTRAINT [PK_DictOperationSystem] PRIMARY KEY CLUSTERED ([ID] ASC) )
CREATE TABLE DictAntivirus ([ID] [int] IDENTITY(1,1) NOT NULL,	[Name] [nvarchar](128) NOT NULL, CONSTRAINT [PK_DictAntivirus] PRIMARY KEY CLUSTERED ([ID] ASC) ) 
CREATE TABLE DictUnauthAccessProtect 	([ID] [int] IDENTITY(1,1) NOT NULL, [Name] [nvarchar](128) NOT NULL,  CONSTRAINT [PK_DictUnauthAccessProtect] PRIMARY KEY CLUSTERED ([ID] ASC) )
CREATE TABLE DictElectronicLock ([ID] [int] IDENTITY(1,1) NOT NULL,	[Name] [nvarchar](128) NOT NULL, CONSTRAINT [PK_DictElectronicLock] PRIMARY KEY CLUSTERED ([ID] ASC) ) 
CREATE TABLE DictTNScreen 	([ID] [int] IDENTITY(1,1) NOT NULL, [Name] [nvarchar](128) NOT NULL,  CONSTRAINT [PK_DictTNScreen] PRIMARY KEY CLUSTERED ([ID] ASC) )
CREATE TABLE DictVipNetCrypto ([ID] [int] IDENTITY(1,1) NOT NULL,	[Name] [nvarchar](128) NOT NULL, CONSTRAINT [PK_DictVipNetCrypto] PRIMARY KEY CLUSTERED ([ID] ASC) ) 


insert into DictOperationSystem (Name) VALUES ('Linux � ������� VipNet Terminal')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows XP 32 ���������')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows XP 64 ���������')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows XP 32 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows XP 64 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows XP (SP2) 32 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows XP (SP2) 64 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Vista 32 ���������')
insert into DictOperationSystem (Name) VALUES ('Microsoft Vista 64 ���������')
insert into DictOperationSystem (Name) VALUES ('Microsoft Vista 32 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Vista 64 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Vista (SP1) 32 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Vista (SP1) 64 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Vista (SP2) 32 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Vista (SP2) 64 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 7 32 ���������')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 7 64 ���������')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 7 32 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 7 64 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 7 (SP1) 32 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 7 (SP1) 64 ��������� (����������������� �����)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 2003 Server SE 32 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 2003 Server SE 64 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 2003 Server SE R2 32 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 2003 Server SE R2 64 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 2003 Server EE 32 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 2003 Server EE 64 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 2003 Server EE R2 32 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows 2003 Server EE R2 64 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows Server 2008 Standard Edition 32 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows Server 2008 Standard Edition 64 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows Server 2008 Enterprise Edition 32 ��������� (SP2)')
insert into DictOperationSystem (Name) VALUES ('Microsoft Windows Server 2008 Enterprise Edition 64 ��������� (SP2)')

insert into DictAntivirus (Name) VALUES ('��������� ����������� 5.0')
insert into DictAntivirus (Name) VALUES ('��������� ����������� 6.0 ��� Windows Servers')
insert into DictAntivirus (Name) VALUES ('��������� ����������� 6.0 ��� Windows Workstations')
insert into DictAntivirus (Name) VALUES ('Kaspersky Endpoint Security 8 ��� Windows')
insert into DictAntivirus (Name) VALUES ('Kaspersky Security 8.0 ��� Microsoft Exchange Servers')
insert into DictAntivirus (Name) VALUES ('��������� ����������� 8.0 ��� Windows Servers Enterprise Edition')
insert into DictAntivirus (Name) VALUES ('Dr.Web Enterprise Security Suite ������ 6.0')
insert into DictAntivirus (Name) VALUES ('Dr. Web ������ 5.0  ��� Windows')
insert into DictAntivirus (Name) VALUES ('Eset NOD32 Platinum Pack 4.0')
insert into DictAntivirus (Name) VALUES ('Security studio endpoint protection Antivirus')

insert into DictUnauthAccessProtect (Name) VALUES ('Secret Net 6')
insert into DictUnauthAccessProtect (Name) VALUES ('������-Win32')
insert into DictUnauthAccessProtect (Name) VALUES ('������-Win64')
insert into DictUnauthAccessProtect (Name) VALUES ('������-NT/2000 v.3.0')
insert into DictUnauthAccessProtect (Name) VALUES ('Security studio endpoint protection')

insert into DictElectronicLock (Name) VALUES ('Secret Net Card')
insert into DictElectronicLock (Name) VALUES ('��� ��������')
insert into DictElectronicLock (Name) VALUES ('������-����')

insert into DictTNScreen (Name) VALUES ('���������� ����� � ������� VipNet Client')
insert into DictTNScreen (Name) VALUES ('���������� ����� � ������� VipNet Coordinator')
insert into DictTNScreen (Name) VALUES ('Security studio endpoint protection Personal Firewall')
insert into DictTNScreen (Name) VALUES ('TrustAccess')
insert into DictTNScreen (Name) VALUES ('TrustAccess-S')

insert into DictVipNetCrypto (Name) VALUES ('VipNet Client')
insert into DictVipNetCrypto (Name) VALUES ('VipNet Coordinator')

-- ���������� ������� ������
CREATE TABLE ApplicationFCT(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[FillingStage] [int] NOT NULL,
	[ScanOrderContentType] [nvarchar](256) NULL,
	[ScanOrder] [image] NULL,
	[PersonFullName] [nvarchar](512) NOT NULL,
	[PersonPosition] [nvarchar](64) NOT NULL,
	[PersonWorkPhone] [nvarchar](15) NOT NULL,
	[PersonMobPhone] [nvarchar](15) NOT NULL,
	[PersonEmail] [nvarchar](64) NOT NULL,
	[IsThereAttestatK1More] [bit] NOT NULL,
	[NumARMs] [int] NOT NULL,
	[NumPDNs] [int] NOT NULL,
	[DictOperationSystemID] [int] NULL,
	[IsIPStatic] [bit] NULL,
	[IPAddress] [nvarchar](150) NULL,
	[IPMask4ARMs] [nvarchar](15) NULL,
	[FISLogin] [nvarchar](255) NULL,
	[DictAntivirusID] [int] NULL,
	[DictUnauthAccessProtectID] [int] NULL,
	[DictElectronicLockID] [int] NULL,
	[DictTNScreenID] [int] NULL,
	[IP4TNS] [nvarchar](15) NULL,
	[DictVipNetCryptoID] [int] NULL,
 CONSTRAINT [PK_ApplicationFCT] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ApplicationFCT]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFCT_DictAntivirus] FOREIGN KEY([DictAntivirusID])
REFERENCES [dbo].[DictAntivirus] ([ID])
GO

ALTER TABLE [dbo].[ApplicationFCT] CHECK CONSTRAINT [FK_ApplicationFCT_DictAntivirus]
GO

ALTER TABLE [dbo].[ApplicationFCT]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFCT_DictElectronicLock] FOREIGN KEY([DictElectronicLockID])
REFERENCES [dbo].[DictElectronicLock] ([ID])
GO

ALTER TABLE [dbo].[ApplicationFCT] CHECK CONSTRAINT [FK_ApplicationFCT_DictElectronicLock]
GO

ALTER TABLE [dbo].[ApplicationFCT]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFCT_DictOperationSystem] FOREIGN KEY([DictOperationSystemID])
REFERENCES [dbo].[DictOperationSystem] ([ID])
GO

ALTER TABLE [dbo].[ApplicationFCT] CHECK CONSTRAINT [FK_ApplicationFCT_DictOperationSystem]
GO

ALTER TABLE [dbo].[ApplicationFCT]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFCT_DictTNScreen] FOREIGN KEY([DictTNScreenID])
REFERENCES [dbo].[DictTNScreen] ([ID])
GO

ALTER TABLE [dbo].[ApplicationFCT] CHECK CONSTRAINT [FK_ApplicationFCT_DictTNScreen]
GO

ALTER TABLE [dbo].[ApplicationFCT]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFCT_DictUnauthAccessProtect] FOREIGN KEY([DictUnauthAccessProtectID])
REFERENCES [dbo].[DictUnauthAccessProtect] ([ID])
GO

ALTER TABLE [dbo].[ApplicationFCT] CHECK CONSTRAINT [FK_ApplicationFCT_DictUnauthAccessProtect]
GO

ALTER TABLE [dbo].[ApplicationFCT]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFCT_DictVipNetCrypto] FOREIGN KEY([DictVipNetCryptoID])
REFERENCES [dbo].[DictVipNetCrypto] ([ID])
GO

ALTER TABLE [dbo].[ApplicationFCT] CHECK CONSTRAINT [FK_ApplicationFCT_DictVipNetCrypto]
GO


