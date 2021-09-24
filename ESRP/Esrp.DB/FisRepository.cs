using System;
using System.Data.Linq;
using System.IO;
using System.Text;
using System.Data.Linq.Mapping;

namespace Esrp.DB
{
    public class FisRepository : IDisposable
    {
        private StringBuilder Log { get; set; }
        private static string connectionString_;
        public static void Init(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString");
            connectionString_ = connectionString;
        }

        public FisDBDataContext DataContext { get; private set; }
        public static FisRepository Create()
        {
            if (String.IsNullOrEmpty(connectionString_))
                throw new InvalidOperationException("Класс не был инициализирован. Вызовите метод Init");
            return new FisRepository();
        }

        internal FisRepository()
        {
            DataContext = new FisDBDataContext(connectionString_);
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
    }
}
