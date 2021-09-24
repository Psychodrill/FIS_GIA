using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using FBS.Replicator.Helpers;

namespace FBS.Replicator.DB
{
    public static class Connections
    {
        private static string FBSConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["FBS"] != null)
                    return ConfigurationManager.ConnectionStrings["FBS"].ConnectionString;
                return null;
            }
        }

        private static string ERBDConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["ERBD"] != null)
                    return ConfigurationManager.ConnectionStrings["ERBD"].ConnectionString;
                return null;
            }
        }

        private static string GVUZConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["GVUZ"] != null)
                    return ConfigurationManager.ConnectionStrings["GVUZ"].ConnectionString;
                return null;
            }
        }

        public static string CompositionsStaticPath2015
        {
            get
            {
                string result = ConfigurationManager.AppSettings["CompositionsStaticPath2015"];
                if (!String.IsNullOrEmpty(result))
                {
                    result = result.TrimEnd('\\', '/');
                }
                return result;
            }
        }

        public static string CompositionsDirectoryUser2015
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsDirectoryUser2015"];
            }
        }

        public static string CompositionsDirectoryPassword2015
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsDirectoryPassword2015"];
            }
        }

        public static string CompositionsStaticPath2016Plus
        {
            get
            {
                string result = ConfigurationManager.AppSettings["CompositionsStaticPath2016Plus"];
                if (!String.IsNullOrEmpty(result))
                {
                    result = result.TrimEnd('\\', '/');
                }
                return result;
            }
        }

        public static string CompositionsDirectoryUser2016Plus
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsDirectoryUser2016Plus"];
            }
        }

        public static string CompositionsDirectoryPassword2016Plus
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsDirectoryPassword2016Plus"];
            }
        }

        public static string CompositionsPagesCountPath2016Plus
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsPagesCountPath2016Plus"];
            }
        }
        public static string CompositionsPagesCountPath2015Plus
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsPagesCountPath2015Plus"];
            }
        }

        public static SqlConnection CreateFBSConnection()
        {
            if (String.IsNullOrEmpty(FBSConnectionString))
                return null;
            SqlConnection result = new SqlConnection(FBSConnectionString);
            return result;
        }

        public static SqlConnection CreateERBDConnection()
        {
            if (String.IsNullOrEmpty(ERBDConnectionString))
                return null;
            SqlConnection result = new SqlConnection(ERBDConnectionString);
            return result;
        }

        public static SqlConnection CreateGVUZConnection()
        {
            if (String.IsNullOrEmpty(GVUZConnectionString))
                return null;
            SqlConnection result = new SqlConnection(GVUZConnectionString);
            return result;
        }

        public static bool TryConnectToERBD(out string errorMessage)
        {
            return TryConnect(ERBDConnectionString, out errorMessage);
        }

        public static bool TryConnectToFBS(out string errorMessage)
        {
            return TryConnect(FBSConnectionString, out errorMessage);
        }

        public static bool TryConnectToGVUZ(out string errorMessage)
        {
            return TryConnect(GVUZConnectionString, out errorMessage);
        }

        private static bool TryConnect(string connectionString, out string errorMessage)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                errorMessage = "Не определена строка соединения";
                return false;
            }
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }

        public static bool TryOpen2015Directory(out string errorMessage)
        {
            return TryOpenDirectory(CompositionsStaticPath2015, CompositionsDirectoryUser2015, CompositionsDirectoryPassword2015, out errorMessage);
        }

        public static bool TryOpen2016PlusDirectory(out string errorMessage)
        {
            return TryOpenDirectory(CompositionsStaticPath2016Plus, CompositionsDirectoryUser2016Plus, CompositionsDirectoryPassword2016Plus, out errorMessage);
        }

        public static bool CheckPagesCountExists(out string errorMessage)
        {
            if (String.IsNullOrEmpty(CompositionsPagesCountPath2016Plus))
            {
                errorMessage = "Не определен путь к каталогу";
                return false;
            }
            if(!Directory.Exists(CompositionsPagesCountPath2016Plus))
            {
                errorMessage = "Каталог не существует";
                return false;
            }
            errorMessage = null;
            return true;    
        }

        private static bool TryOpenDirectory(string path, string user, string password, out string errorMessage)
        {
            if (String.IsNullOrEmpty(path))
            {
                errorMessage = "Не определен путь к каталогу";
                return false;
            }
            try
            {
                NetworkIO networkIO = new NetworkIO(user, password);
                IEnumerable<string> directories = networkIO.EnumerateDirectories(path);
                if (!directories.Any())
                    throw new Exception("Вложенные каталоги не найдены");
                 
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " - " + ex.InnerException.Message;
                }
                return false;
            }
        }
    }
}
