using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;
using Fbs.Core;
using Fbs.Utility;

namespace Fbs.Web
{
    /// <summary>
    /// Проверка существования запрашиваемого ресурса
    /// </summary>
    public static class ResourceExistenceChecker
    {

        #region Private properties

        private static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private static HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        #endregion

        #region Public methods

        public static void Check()
        {
            // Проверю существование запрашиваемого ресурса...
            if (IsPathExists(Request.PhysicalPath))
            {
                // Если путь до файла начинается со значения переменной SharedDocumetsFolder конфига, 
                // то считаю что этот файл лежит на диске и отдам его через диалог просмотра/закачки 
                // файла.
                if (Request.Path.StartsWith(Config.SharedDocumetsFolder))
                    ResponseWriter.WriteFile(Path.GetFileName(Request.PhysicalPath),
                            Request.ContentType, Request.PhysicalPath);
            }
            // ...или совпадение с хэндлером
            else if (!IsHandlerExists(Request.Path))
            {
                // Ресурс не найден. Попробую найти документ по алиасу в БД.
                Fbs.Core.Document doc = Fbs.Core.Document.GetDocument(Request.FilePath);
                // Проверю существование документа. Проверка на "активность" выполняется при 
                // получении документа по алиасу на уровне БД.
                if (doc != null)
                {
                    // Отдам документ в response
                    doc.WriteToResponse();
                    // Завершу запрос
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    // Если определена 404 ошибка, выполню редирект на ее стртаницу, иначе оставлю 
                    // обработку ошибки IIS-у.
                    if (!string.IsNullOrEmpty(Config.Erorr404RedirectUrl))
                        Response.Redirect(Config.Erorr404RedirectUrl, true);
                }
            }
        }

        #endregion

        #region Private methods

        // Проверка существования заданного объекта файловой системы
        private static bool IsPathExists(string physicalPath)
        {
            string fileExtension = Path.GetExtension(physicalPath);
            if (string.IsNullOrEmpty(fileExtension))
                return Directory.Exists(physicalPath);
            else
                return File.Exists(physicalPath);
        }

        // Проверка совпадения запрашиваемого адреса с адресом, выданным хэндлеру
        private static bool IsHandlerExists(string virtualPath)
        {
            string path = string.Empty;
            foreach (HttpHandlerAction handler in Config.HandlerErrorsSection.Handlers)
            {
                path = handler.Path;
                // Сложные варианты путей до хэндлеров не рассмотриваю - просто отбрасываю маску (*)
                if (path.IndexOf("*") > -1)
                    path = path.Replace("*", string.Empty);

                // Проверяю совпадение пути до хэндлера с окончанием запрашиваего пути
                if (path.Length > 0 && virtualPath.EndsWith(path))
                    return true;
            }

            return false;
        }

        #endregion

    }
}
