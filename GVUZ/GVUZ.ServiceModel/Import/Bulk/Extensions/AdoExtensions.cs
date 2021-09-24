using System.Data.SqlClient;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;

namespace GVUZ.ServiceModel.Import.Bulk.Extensions
{
    public static class AdoExtensions
    {
        public static readonly object _xmlToObjectLocker = new object();
        public static TResult ExecuteXmlToObject<TResult>(this SqlCommand command) where TResult : class
        {
            /* Получение результата в виде XML из БД */
            //Тут надо лок сделать, иначе куча дедлоков
            lock (_xmlToObjectLocker)
            {
                using (var readerXml = command.ExecuteXmlReader())
                {
                    readerXml.Read();
                    return new Serializer().Deserialize<TResult>(readerXml.ReadOuterXml());
                }
            }
        }
    }
}