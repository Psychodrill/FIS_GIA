ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [fbs_db], FILENAME = '$(DefaultDataPath)main_db.mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

