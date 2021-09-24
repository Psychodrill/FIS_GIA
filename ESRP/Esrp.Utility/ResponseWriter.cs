using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Esrp.Utility
{
    /// <summary>
    /// Класс позволяет записывать файлы из разных источников в объект HttpResponse
    /// </summary>
    public class ResponseWriter
    {
        private const string CharReplacement = "_";
        private const int BufferLength = 1024;
        private static Regex UnallowableCharsRegex = new Regex("[\\/*&?]", RegexOptions.Compiled);

        private static HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        private ResponseWriter() { }

        // Подготовка имени файла к записи в хидер ответа
        private static string PrepareFileName(string rawFileName)
        {
            string fileName = rawFileName;
            switch (HttpContext.Current.Request.Browser.Browser)
            {
                case "IE":
                    {
                        // IE некорректно отображает имя файла в диалоге загрузки, если в имени 
                        // встречаются эти символы. Заменю их на ReplacementChar.
                        fileName = UnallowableCharsRegex.Replace(fileName, CharReplacement);
                        // IE требует, чтобы кодировка хидера совпадала с локалью пользователя.
                        // Иначе имя загружаемого файла будет отдваться кракозябрами.
                        // Допускаю, что имена документов задавались в кодировке Windows-1251
                        Response.HeaderEncoding = Encoding.GetEncoding(1251);
                        break;
                    }
                case "Opera":
                    {
                        // Opera некорректно отображает имя файла в диалоге загрузки, если в имени 
                        // встречаются эти символы. Заменю их на ReplacementChar.
                        fileName = UnallowableCharsRegex.Replace(fileName, CharReplacement);
                        break;
                    }
                case "Mozilla":
                case "Firefox":
                    {
                        // Эти браузеры понимают уникод в имени файла. Оставлю имя как есть.
                        break;
                    }
                case "AppleMAC-Safari":
                    {
                        // TODO: Разобраться почему русский не умеет
                        fileName = RussianToLatin.EncodePhrase(fileName, true);
                        break;
                    }
                default:
                    {
                        // Для всех остальных выполню транслитерацию имени файла
                        fileName = RussianToLatin.EncodePhrase(fileName, true);
                        break;
                    }
            }

            return fileName;
        }

        /// <summary>
        /// Заполнение хидеров
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="contentType">Тип содержимого файла</param>
        /// <param name="contentLength">Размер файла в байтах</param>
        public static void PrepareHeaders(string fileName, string contentType, long contentLength)
        {
            PrepareHeaders(fileName, contentType, contentLength, null);
        }

        /// <summary>
        /// Заполнение хидеров
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="contentType">Тип содержимого файла</param>
        /// <param name="contentEncoding">Кодировка содержимого файла</param>
        public static void PrepareHeaders(string fileName, string contentType, Encoding contentEncoding)
        {
            PrepareHeaders(fileName, contentType, null, contentEncoding);
        }

        /// <summary>
        /// Заполнение хидеров
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="contentType">Тип содержимого файла</param>
        /// <param name="contentLength">Размер файла в байтах</param>
        /// <param name="contentEncoding">Кодировка содержимого файла</param>
        public static void PrepareHeaders(string fileName, string contentType, long? contentLength,
              Encoding contentEncoding)
        {
            // Очищу хидеры и контент ответа
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();

            // Установлю хидеры
            Response.AppendHeader("Content-Disposition",
                String.Format("attachment; filename=\"{0}\"", PrepareFileName(fileName)));
            //Response.AppendHeader("Content-Type", contentType + "; charset=utf-8");
            Response.AppendHeader("Content-Type", contentType + ";");

            // В случае неизвестной длинны файла, атрибут Content-Length не устанавливаю
            if (contentLength != null)
                Response.AppendHeader("Content-Length", contentLength.ToString());

            // Если не задана кодировка, то берется кодировка по умолчанию, т.е. уникод
            if (contentEncoding != null)
                Response.ContentEncoding = contentEncoding;
        }

        /// <summary>
        /// Запись файла в объект Response
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="contentType">Тип содержимого файла</param>
        /// <param name="content">Объект типа System.Data.Linq.Binary</param>
        public static void WriteFile(string fileName, string contentType, System.Data.Linq.Binary content)
        {
            // Установлю заголовки ответа
            PrepareHeaders(fileName, contentType, content.Length);

            Response.BinaryWrite(content.ToArray());
            Response.End();
        }

        /// <summary>
        /// Запись файла в объект Response
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="contentType">Тип содержимого файла</param>
        /// <param name="filePath">Путь до файла на диске</param>
        public static void WriteFile(string fileName, string contentType, string filePath)
        {
            // получу информацию о файле
            FileInfo info = new FileInfo(filePath);

            // Установлю заголовки ответа
            PrepareHeaders(fileName, contentType, info.Length);

            // определю буфер
            byte[] buffer = new byte[BufferLength];
            // запишу документ в выходной поток
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite))
            {
                while (true)
                {
                    int readBytes = fs.Read(buffer, 0, buffer.Length);
                    if (readBytes <= 0)
                        break;

                    // IsClientConnected проверять обязательно
                    if (Response.IsClientConnected)
                        Response.OutputStream.Write(buffer, 0, readBytes);

                    // Сразу же записываю прочитанный кусок. IsClientConnected проверять обязательно
                    if (Response.IsClientConnected)
                        Response.Flush();
                }
            }
        }

        public static void WriteStream(string fileName, string contentType, Stream streamToWrite)
        {
            // Установлю заголовки ответа
            PrepareHeaders(fileName, contentType, streamToWrite.Length);

            // определю буфер
            byte[] buffer = new byte[BufferLength];
            // запишу документ в выходной поток
           
                while (true)
                {
                    int readBytes = streamToWrite.Read(buffer, 0, buffer.Length);
                    if (readBytes <= 0)
                        break;

                    // IsClientConnected проверять обязательно
                    if (Response.IsClientConnected)
                        Response.OutputStream.Write(buffer, 0, readBytes);

                    // Сразу же записываю прочитанный кусок. IsClientConnected проверять обязательно
                    if (Response.IsClientConnected)
                        Response.Flush();
                }
        }
    }
}
