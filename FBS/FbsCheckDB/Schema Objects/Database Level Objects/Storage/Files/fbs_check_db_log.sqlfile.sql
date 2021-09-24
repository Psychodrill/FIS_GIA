ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [fbs_check_db_log], FILENAME = '$(Path1)$(DatabaseName).ldf', MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

