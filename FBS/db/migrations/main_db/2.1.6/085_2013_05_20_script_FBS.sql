insert into Migrations(MigrationVersion, MigrationName) values (85, '085_2013_05_20_script_FBS.sql')

ALTER database [fbs]
ADD FILEGROUP [RBD]
GO

ALTER database [fbs]
ADD FILE
(
    NAME= 'Rbd',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\ERBD_FBC_RBD.ndf'
)
TO FILEGROUP [RBD]
GO

ALTER database [fbs]
ADD FILEGROUP [CRT]
GO

ALTER database [fbs]
ADD FILE
(
    NAME= 'Crt',
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\ERBD_FBC_CRT.ndf'
)
TO FILEGROUP [CRT]
GO