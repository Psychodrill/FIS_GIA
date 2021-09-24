insert into Migrations(MigrationVersion, MigrationName) values (92, '092_2013_06_19_fbs.sql')
go

ALTER TABLE rbd.Participants
 DROP CONSTRAINT PK_Participants
GO
ALTER TABLE rbd.Participants ADD CONSTRAINT
 PK_Participants PRIMARY KEY CLUSTERED 
 (
 ParticipantID,
 UseYear,
 REGION
 ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON RBD

GO

alter table prn.Certificates alter column [UpdateDate] [datetime] null
go

alter table prn.CertificatesMarks alter column [CertificateFK] [uniqueidentifier] not null
go
alter table prn.CertificatesMarks alter column PrintedMarkID ADD SPARSE 
go
ALTER TABLE prn.CertificatesMarks
 DROP CONSTRAINT PK_prn_CertificatesMarks
GO
ALTER TABLE prn.CertificatesMarks ADD CONSTRAINT
 PK_prn_CertificatesMarks PRIMARY KEY CLUSTERED 
 (
 [CertificateMarkID] ASC,
 [UseYear] ASC,
 [REGION] ASC,
 [CertificateFK] ASC
 ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON RBD
 go
alter table prn.CancelledCertificates alter column CertificateFK [uniqueidentifier] not null
go

if not exists(select * from sys.columns where name='ParticipantFK' and object_name(object_id)='CommonNationalExamCertificateCheck')
alter table CommonNationalExamCertificateCheck add ParticipantFK uniqueidentifier
go
