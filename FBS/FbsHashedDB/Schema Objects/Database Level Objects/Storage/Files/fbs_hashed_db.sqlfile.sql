ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [fbs_hashed_db], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

