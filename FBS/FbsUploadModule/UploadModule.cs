using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Fbs.Utility;

namespace Fbs.UploadModule
{
    /// <summary>
    /// Модуль обеспечивающий чтение multi-part post запросов.
    /// Также обеспечивает формирование данных о ходе загрузки.
    /// </summary>
    public class UploadModule : System.Web.IHttpModule
    {

        #region "Class Vars"

        /// <summary>
        /// Размер загружаемого файл для которого выдается предупреждение
        /// </summary>
        private long mBadFileSize = Fbs.UploadModule.Properties.Settings.Default.BadFileSize;
        /// <summary>
        /// Максимальный размер загружаемого файла
        /// </summary>
        private long mMaxFileSize = Fbs.UploadModule.Properties.Settings.Default.MaxFileSize;
        /// <summary>
        /// Список страниц для которых производится обработка запросов
        /// </summary>
        private string mPages = Fbs.UploadModule.Properties.Settings.Default.Pages;

        private const string msgBadFileSize = "Размер файла больше {0}kb.";
        private const string msgMaxFileSize = "Максимальный размер файла {0}kb.";
        private const string msgEmptyFile = "Вы пытаетесь загрузить пустой файл.";

        #endregion

        #region "Constructors"

        public UploadModule() { }

        #endregion

        #region "Methods"

        /// <summary>
        /// Метод проверят осущесвляется ли запрос к страницам определенным для загрузки
        /// </summary>
        private bool IsUploadPages()
        {
            HttpApplication app = HttpContext.Current.ApplicationInstance;
            string[] uploadPages = (string[])mPages.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < uploadPages.Length; i++)
            {
                if (uploadPages[i].ToLower() == app.Request.Path.Substring(1).ToLower())
                    return true;
            }
            return false;
        }

        #endregion

        #region IHttpModule Members

        void System.Web.IHttpModule.Dispose() { }

        void System.Web.IHttpModule.Init(System.Web.HttpApplication context)
        {
            // Добавляю обработчик для события "начало запроса"
            context.BeginRequest += new EventHandler(BeginRequest);
        }

        #endregion

        #region "Event Handlers / Events"

        /// <summary>
        /// Обработчик начала запроса
        /// </summary>
        /// <param name="sender">HttpApplication</param>
        /// <param name="e">EventArgs</param>
        void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication = (HttpApplication)sender;
            HttpContext context = httpApplication.Context;

            // Проверю выполняется ли запрос к страницам для загрузки
            if (!IsUploadPages())
                return;

            // Проверю есть ли в запросе файлы
            if (context.Request.ContentType.IndexOf("multipart/form-data") == -1)
                return;

            // Получу идентификатор запроса
            string currentFileID = context.Request.QueryString["PostID"];
            if (String.IsNullOrEmpty(currentFileID))
                return;

            // Получу имя закачиваемого файла
            string fileName = context.Request.QueryString["FileName"];
            if (String.IsNullOrEmpty(fileName))
                return;

            // Создам объект для хранения состояния загрузки
            UploadStatus uploadStatus = new UploadStatus(currentFileID, Path.GetTempFileName(),
                    Path.GetExtension(fileName));

            try
            {
                // Создам объект для записи данных
                FileProcessor fp = new FileProcessor(uploadStatus.FileName);
                try
                {
                    // Создам объект для чтения запроса
                    HttpWorkerRequest workerRequest = (HttpWorkerRequest)context.GetType().GetProperty("WorkerRequest",
                            BindingFlags.Instance | BindingFlags.NonPublic).GetValue(context, null);

                    if (workerRequest.HasEntityBody())
                    {
                        try
                        {
                            if (currentFileID != null)
                            {
                                // Добавлю статус в Application
                                while (context.Application.Get(currentFileID) != null)
                                    context.Application.Remove(currentFileID);
                                context.Application.Add(currentFileID, uploadStatus);
                            }

                            // Получу размер запроса
                            long contentLength = long.Parse((workerRequest.GetKnownRequestHeader(
                                    HttpWorkerRequest.HeaderContentLength)));

                            // Проверю размер получаемого файл (запроса)
                            if (contentLength < 1000)
                            {
                                uploadStatus.ErrorLevel = UploadStatus.ErrorEnum.FileIsEmpty;
                                uploadStatus.ErrorMsg = msgEmptyFile;
                                return;
                            }
                            else if (contentLength > mMaxFileSize)
                            {
                                // Если файл больше допустимого размера, передам пользователю ошибку
                                uploadStatus.ErrorLevel = UploadStatus.ErrorEnum.FileTooLarge;
                                uploadStatus.ErrorMsg = String.Format(msgMaxFileSize,
                                        mMaxFileSize / 1024);
                                return;
                            }
                            else if (contentLength > mBadFileSize && context.Request.QueryString["ignore"] == null)
                            {
                                // Если файл больше рекомендуемого занчения и пользователь не 
                                // проигнорировал (не получил) предупреждение, передам предупреждение
                                uploadStatus.ErrorMsg = String.Format(msgBadFileSize, mBadFileSize / 1024);
                                uploadStatus.ErrorLevel = UploadStatus.ErrorEnum.FileIsGreaterThatRecommended;
                                return;
                            }

                            uploadStatus.TotalBytes = contentLength;

                            long receivedcount = 0;
                            // Размер буфер для чтения  
                            long defaultBuffer = 8192;

                            // Получу предзагруженные данные запроса.
                            // Может возникнуть проблема при работе на "ASP.NET Developer Server": метод 
                            // возвращет null. На полноценном IIS данной проблемы не возникает
                            byte[] preloadedBufferData = workerRequest.GetPreloadedEntityBody();

                            // Проверю что что-то загружено
                            if (preloadedBufferData == null)
                                throw new Exception("GetPreloadedEntityBody() was null. Try again");

                            // Обработаю заголовки запроса
                            fp.GetFieldSeperators(ref preloadedBufferData);

                            // Обработаю полученные данные
                            fp.ProcessBuffer(ref preloadedBufferData);

                            // Обновлю состояние загрузки
                            uploadStatus.CurrentBytesTransfered += preloadedBufferData.Length;

                            // Проверю прочитанные ли предзагруженные данные
                            if (!workerRequest.IsEntireEntityBodyIsPreloaded())
                            {
                                // Начну чтение запроса
                                do
                                {
                                    // Расчитаю необходимый размер буфера
                                    long tempBufferSize = (uploadStatus.TotalBytes - uploadStatus.CurrentBytesTransfered);
                                    if (tempBufferSize < defaultBuffer)
                                        defaultBuffer = tempBufferSize;
                                    byte[] bufferData = new byte[defaultBuffer];

                                    // Прочитаю порцию данных из запроса
                                    receivedcount = workerRequest.ReadEntityBody(bufferData, bufferData.Length);

                                    // Запишу полученные данные
                                    fp.ProcessBuffer(ref bufferData);

                                    // Обновлю состояние загрузки
                                    uploadStatus.CurrentBytesTransfered += bufferData.Length;

                                    // Если пользователь отменил загрузку завершу чтение запроса
                                    if (uploadStatus.IsCanceled)
                                    {
                                        context.Application.Remove(uploadStatus.FileId);
                                        break;
                                    }
                                }
                                while (receivedcount != 0);
                            }
                        }
                        finally
                        {
                            fp.CloseStreams();
                        }
                    }
                }
                finally
                {
                    fp.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Не оставляю на обработку в Application_Error. 
                // Вручную опубликую в логе ошибку
                LogManager.Error(ex);

                // Установлю статус закачки - "неизвестная ошибка". В этом случае javascript, 
                // поймав этот стасут, перекинет пользователя на страницу "простой" загрузки документа 
                uploadStatus.ErrorLevel = UploadStatus.ErrorEnum.UnknownError;
            }
        }

        #endregion
    }
}
