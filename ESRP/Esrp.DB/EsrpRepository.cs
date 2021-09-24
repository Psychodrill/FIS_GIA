using System;
using System.Data.Linq;
using System.IO;
using System.Text;
using System.Data.Linq.Mapping;

namespace Esrp.DB
{
    public class EsrpRepository : IDisposable
    {
        private StringBuilder Log { get; set; }
        private static string connectionString_;
        public static void Init(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString");
            connectionString_ = connectionString;
        }

        public EsrpDBDataContext DataContext { get; private set; }
        public static EsrpRepository Create()
        {
            if (String.IsNullOrEmpty(connectionString_))
                throw new InvalidOperationException("Класс не был инициализирован. Вызовите метод Init");
            return new EsrpRepository();
        }

        internal EsrpRepository()
        {
            DataContext = new EsrpDBDataContext(connectionString_);
            DataContext.CommandTimeout = 600;
            Log = new StringBuilder();
            StringWriter stringWriter = new StringWriter(Log);
            DataContext.Log = stringWriter;
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        public static bool IsInit()
        {
            return !String.IsNullOrEmpty(connectionString_);
        }

        public string GetTableName<TEntity>(Table<TEntity> table) where TEntity : class
        {
            Type type = typeof(TEntity);
            return GetTableName(type);
        }

        public string GetTableName(Type type)
        {
            object[] attributes = type.GetCustomAttributes(
                                   typeof(TableAttribute),
                                   true);
            if (attributes.Length == 0)
                return type.Name;
            else
                return (attributes[0] as TableAttribute).Name;
        }
    }
}
