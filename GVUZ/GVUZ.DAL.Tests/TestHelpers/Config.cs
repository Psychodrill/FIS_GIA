using System;
using System.Configuration;

namespace GVUZ.DAL.Tests.TestHelpers
{
    internal static class Config
    {
        public static string MasterDBConnectionString
        {
            get
            {
                string result = ConfigurationManager.ConnectionStrings["Master"].ConnectionString;
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано соединение с master-БД");
                return result;
            }
        }

        public static string GVUZConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["GVUZTest"] == null)
                    throw new Exception("Не указано соединение с тестовой БД ФИС");

                string result = ConfigurationManager.ConnectionStrings["GVUZTest"].ConnectionString;
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано соединение с тестовой БД ФИС");
                return result;
            }
        }

        public static string GVUZBackupPath
        {
            get
            {
                string result = ConfigurationManager.AppSettings["GVUZBackupPath"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указан путь к бэкапу БД ФИС");
                return result;
            }
        }

        public static string GVUZBackupDataFileLogicalName
        {
            get
            {
                string result = ConfigurationManager.AppSettings["GVUZBackupDataFileLogicalName"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано логическое имя файла данных в бэкапе ФИС");
                return result;
            }
        }

        public static string GVUZBackupLogFileLogicalName
        {
            get
            {
                string result = ConfigurationManager.AppSettings["GVUZBackupLogFileLogicalName"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано логическое имя файла журнала в бэкапе ФИС");
                return result;
            }
        }

        public static string ESRPConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["ESRPTest"] == null)
                    throw new Exception("Не указано соединение с тестовой БД ЕСРП");

                string result = ConfigurationManager.ConnectionStrings["ESRPTest"].ConnectionString;
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано соединение с тестовой БД ЕСРП");
                return result;
            }
        }

        public static string ESRPBackupPath
        {
            get
            {
                string result = ConfigurationManager.AppSettings["ESRPBackupPath"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указан путь к бэкапу БД ЕСРП");
                return result;
            }
        }

        public static string ESRPBackupDataFileLogicalName
        {
            get
            {
                string result = ConfigurationManager.AppSettings["ESRPBackupDataFileLogicalName"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано логическое имя файла данных в бэкапе ЕСРП");
                return result;
            }
        }

        public static string ESRPBackupLogFileLogicalName
        {
            get
            {
                string result = ConfigurationManager.AppSettings["ESRPBackupLogFileLogicalName"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано логическое имя файла журнала в бэкапе ЕСРП");
                return result;
            }
        }

        public static string FBSConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["FBSTest"] == null)
                    throw new Exception("Не указано соединение с тестовой БД ФБС");

                string result = ConfigurationManager.ConnectionStrings["FBSTest"].ConnectionString;
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано соединение с тестовой БД ФБС");
                return result;
            }
        }

        public static string FBSBackupPath
        {
            get
            {
                string result = ConfigurationManager.AppSettings["FBSBackupPath"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указан путь к бэкапу БД ФБС");
                return result;
            }
        }

        public static string FBSBackupDataFileLogicalName
        {
            get
            {
                string result = ConfigurationManager.AppSettings["FBSBackupDataFileLogicalName"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано логическое имя файла данных в бэкапе ФБС");
                return result;
            }
        }

        public static string FBSBackupLogFileLogicalName
        {
            get
            {
                string result = ConfigurationManager.AppSettings["FBSBackupLogFileLogicalName"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указано логическое имя файла журнала в бэкапе ФБС");
                return result;
            }
        }

        public static string GVUZMigrationsFolderPath
        {
            get
            {
                string result = ConfigurationManager.AppSettings["GVUZMigrationsFolderPath"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указан путь к миграционным скриптам ФИС");
                return result;
            }
        }

        public static string ESRPMigrationsFolderPath
        {
            get
            {
                string result = ConfigurationManager.AppSettings["ESRPMigrationsFolderPath"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указан путь к миграционным скриптам ЕСРП");
                return result;
            }
        }

        public static string FBSMigrationsFolderPath
        {
            get
            {
                string result = ConfigurationManager.AppSettings["FBSMigrationsFolderPath"];
                if (String.IsNullOrEmpty(result))
                    throw new Exception("Не указан путь к миграционным скриптам ФБС");
                return result;
            }
        }
    }
}
