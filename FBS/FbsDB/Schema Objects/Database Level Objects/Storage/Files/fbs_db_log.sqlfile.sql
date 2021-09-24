ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [fbs_db_log], FILENAME = '$(DefaultLogPath)main_db.ldf', MAXSIZE = 2097152 MB, FILEGROWTH = 1024 KB);

