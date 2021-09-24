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
    /// ������ �������������� ������ multi-part post ��������.
    /// ����� ������������ ������������ ������ � ���� ��������.
    /// </summary>
    public class UploadModule : System.Web.IHttpModule
    {

        #region "Class Vars"

        /// <summary>
        /// ������ ������������ ���� ��� �������� �������� ��������������
        /// </summary>
        private long mBadFileSize = Fbs.UploadModule.Properties.Settings.Default.BadFileSize;
        /// <summary>
        /// ������������ ������ ������������ �����
        /// </summary>
        private long mMaxFileSize = Fbs.UploadModule.Properties.Settings.Default.MaxFileSize;
        /// <summary>
        /// ������ ������� ��� ������� ������������ ��������� ��������
        /// </summary>
        private string mPages = Fbs.UploadModule.Properties.Settings.Default.Pages;

        private const string msgBadFileSize = "������ ����� ������ {0}kb.";
        private const string msgMaxFileSize = "������������ ������ ����� {0}kb.";
        private const string msgEmptyFile = "�� ��������� ��������� ������ ����.";

        #endregion

        #region "Constructors"

        public UploadModule() { }

        #endregion

        #region "Methods"

        /// <summary>
        /// ����� �������� ������������� �� ������ � ��������� ������������ ��� ��������
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
            // �������� ���������� ��� ������� "������ �������"
            context.BeginRequest += new EventHandler(BeginRequest);
        }

        #endregion

        #region "Event Handlers / Events"

        /// <summary>
        /// ���������� ������ �������
        /// </summary>
        /// <param name="sender">HttpApplication</param>
        /// <param name="e">EventArgs</param>
        void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication = (HttpApplication)sender;
            HttpContext context = httpApplication.Context;

            // ������� ����������� �� ������ � ��������� ��� ��������
            if (!IsUploadPages())
                return;

            // ������� ���� �� � ������� �����
            if (context.Request.ContentType.IndexOf("multipart/form-data") == -1)
                return;

            // ������ ������������� �������
            string currentFileID = context.Request.QueryString["PostID"];
            if (String.IsNullOrEmpty(currentFileID))
                return;

            // ������ ��� ������������� �����
            string fileName = context.Request.QueryString["FileName"];
            if (String.IsNullOrEmpty(fileName))
                return;

            // ������ ������ ��� �������� ��������� ��������
            UploadStatus uploadStatus = new UploadStatus(currentFileID, Path.GetTempFileName(),
                    Path.GetExtension(fileName));

            try
            {
                // ������ ������ ��� ������ ������
                FileProcessor fp = new FileProcessor(uploadStatus.FileName);
                try
                {
                    // ������ ������ ��� ������ �������
                    HttpWorkerRequest workerRequest = (HttpWorkerRequest)context.GetType().GetProperty("WorkerRequest",
                            BindingFlags.Instance | BindingFlags.NonPublic).GetValue(context, null);

                    if (workerRequest.HasEntityBody())
                    {
                        try
                        {
                            if (currentFileID != null)
                            {
                                // ������� ������ � Application
                                while (context.Application.Get(currentFileID) != null)
                                    context.Application.Remove(currentFileID);
                                context.Application.Add(currentFileID, uploadStatus);
                            }

                            // ������ ������ �������
                            long contentLength = long.Parse((workerRequest.GetKnownRequestHeader(
                                    HttpWorkerRequest.HeaderContentLength)));

                            // ������� ������ ����������� ���� (�������)
                            if (contentLength < 1000)
                            {
                                uploadStatus.ErrorLevel = UploadStatus.ErrorEnum.FileIsEmpty;
                                uploadStatus.ErrorMsg = msgEmptyFile;
                                return;
                            }
                            else if (contentLength > mMaxFileSize)
                            {
                                // ���� ���� ������ ����������� �������, ������� ������������ ������
                                uploadStatus.ErrorLevel = UploadStatus.ErrorEnum.FileTooLarge;
                                uploadStatus.ErrorMsg = String.Format(msgMaxFileSize,
                                        mMaxFileSize / 1024);
                                return;
                            }
                            else if (contentLength > mBadFileSize && context.Request.QueryString["ignore"] == null)
                            {
                                // ���� ���� ������ �������������� �������� � ������������ �� 
                                // �������������� (�� �������) ��������������, ������� ��������������
                                uploadStatus.ErrorMsg = String.Format(msgBadFileSize, mBadFileSize / 1024);
                                uploadStatus.ErrorLevel = UploadStatus.ErrorEnum.FileIsGreaterThatRecommended;
                                return;
                            }

                            uploadStatus.TotalBytes = contentLength;

                            long receivedcount = 0;
                            // ������ ����� ��� ������  
                            long defaultBuffer = 8192;

                            // ������ ��������������� ������ �������.
                            // ����� ���������� �������� ��� ������ �� "ASP.NET Developer Server": ����� 
                            // ��������� null. �� ����������� IIS ������ �������� �� ���������
                            byte[] preloadedBufferData = workerRequest.GetPreloadedEntityBody();

                            // ������� ��� ���-�� ���������
                            if (preloadedBufferData == null)
                                throw new Exception("GetPreloadedEntityBody() was null. Try again");

                            // ��������� ��������� �������
                            fp.GetFieldSeperators(ref preloadedBufferData);

                            // ��������� ���������� ������
                            fp.ProcessBuffer(ref preloadedBufferData);

                            // ������� ��������� ��������
                            uploadStatus.CurrentBytesTransfered += preloadedBufferData.Length;

                            // ������� ����������� �� ��������������� ������
                            if (!workerRequest.IsEntireEntityBodyIsPreloaded())
                            {
                                // ����� ������ �������
                                do
                                {
                                    // �������� ����������� ������ ������
                                    long tempBufferSize = (uploadStatus.TotalBytes - uploadStatus.CurrentBytesTransfered);
                                    if (tempBufferSize < defaultBuffer)
                                        defaultBuffer = tempBufferSize;
                                    byte[] bufferData = new byte[defaultBuffer];

                                    // �������� ������ ������ �� �������
                                    receivedcount = workerRequest.ReadEntityBody(bufferData, bufferData.Length);

                                    // ������ ���������� ������
                                    fp.ProcessBuffer(ref bufferData);

                                    // ������� ��������� ��������
                                    uploadStatus.CurrentBytesTransfered += bufferData.Length;

                                    // ���� ������������ ������� �������� ������� ������ �������
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
                // �� �������� �� ��������� � Application_Error. 
                // ������� ��������� � ���� ������
                LogManager.Error(ex);

                // ��������� ������ ������� - "����������� ������". � ���� ������ javascript, 
                // ������ ���� ������, ��������� ������������ �� �������� "�������" �������� ��������� 
                uploadStatus.ErrorLevel = UploadStatus.ErrorEnum.UnknownError;
            }
        }

        #endregion
    }
}
