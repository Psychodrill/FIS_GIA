ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [fbs_check_db], FILENAME = '$(Path2)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

