namespace Esrp.Utility
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Базовый класс для документов Word, создаваемых из шаблона.
    /// </summary>
    public abstract class CustomTeplateBaseWordDocument : IDisposable
    {
        #region Constants and Fields

        private static readonly object mLock = new object();

        private readonly SortedList mFieldValues = new SortedList();

        private readonly string mTemplateFileName;

        private bool mDisposed;

        private string mFileName;

        private bool mHasErrors;

        private bool mNeedDeleteFileOnDispose;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTeplateBaseWordDocument"/> class.
        /// </summary>
        /// <param name="templateFileName">
        /// The template file name.
        /// </param>
        protected CustomTeplateBaseWordDocument(string templateFileName)
        {
            this.mTemplateFileName = HttpContext.Current != null
                                         ? // ReSharper disable AssignNullToNotNullAttribute
                                     Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, templateFileName)
                                         : // ReSharper restore AssignNullToNotNullAttribute
                                     templateFileName;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CustomTeplateBaseWordDocument"/> class. 
        /// </summary>
        ~CustomTeplateBaseWordDocument()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The error.
        /// </summary>
        public event ErrorEventHandler Error;

        #endregion

        #region Public Properties

        /// <summary>
        /// Имя файла, созданного по шаблону.
        /// Из файла данные можно только читать, т.к. файл может еще удерживаться Word'ом.
        /// </summary>
        public string FileName
        {
            get
            {
                lock (mLock)
                {
                    this.CreateDocument();
                    return this.mFileName;
                }
            }
        }

        /// <summary>
        /// Были ли ошибки во время подстановки в файл параметров.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                lock (mLock)
                {
                    this.CreateDocument();
                    return this.mHasErrors;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Инициализировать поля документа.
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="value">
        /// </param>
        protected void InitializeFields(string key, string value)
        {
            this.mFieldValues.Add(key, value);
        }

        /// <summary>
        /// The on error.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnError(ErrorEventArgs e)
        {
            if (this.Error != null)
            {
                this.Error(this, e);
            }
        }

        private void CleanUnmanagmentResources()
        {
            if (this.mNeedDeleteFileOnDispose)
            {
                try
                {
                    File.Delete(this.mFileName);
                }
                catch
                {
                    // Если не удалось удалить (файл занят другим процессом), оставляем.
                }
            }
        }

        private void CreateDocument()
        {
            if (this.mFileName != null)
            {
                return;
            }

            this.mFileName = this.mTemplateFileName;
            try
            {
                // Создаем документ на основании файла-шаблона.
                var document = new RtfDocument(this.mTemplateFileName);

                try
                {
                    foreach (DictionaryEntry entry in this.mFieldValues)
                    {
                        document.UpdateField(
                            entry.Key.ToString(), this.GetDocumentFormFieldValue(Convert.ToString(entry.Value)));
                    }
                }
                catch (Exception ex)
                {
                    this.OnError(new ErrorEventArgs(ex));
                    this.mHasErrors = true;
                }

                // Сохраняем в формате RTF во временный файл.
                this.mFileName = Path.GetTempFileName();
                document.Save(this.mFileName);

                this.mNeedDeleteFileOnDispose = true;
            }
            catch (Exception ex)
            {
                this.OnError(new ErrorEventArgs(ex));
                this.mHasErrors = true;
            }
        }

        private void Dispose(bool disposing)
        {
            if (this.mDisposed)
            {
                return;
            }

            this.CleanUnmanagmentResources();
            this.mDisposed = true;
        }

        private string GetDocumentFormFieldValue(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value.Length > 512)
            {
                return string.Format("{0}...", value.Substring(0, 511));
            }

            return value;
        }

        #endregion

        /// <summary>
        /// Работа с rtf документом
        /// </summary>
        private class RtfDocument
        {
            #region Constants and Fields

            private string rtfDocument;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="RtfDocument"/> class.
            /// </summary>
            /// <param name="path">
            /// The path.
            /// </param>
            public RtfDocument(string path)
            {
                this.rtfDocument = File.ReadAllText(path);
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// The save.
            /// </summary>
            /// <param name="path">
            /// The path.
            /// </param>
            public void Save(string path)
            {
                File.WriteAllText(path, this.rtfDocument, Encoding.GetEncoding(1251));
            }

            /// <summary>
            /// The update field.
            /// </summary>
            /// <param name="key">
            /// The key.
            /// </param>
            /// <param name="value">
            /// The value.
            /// </param>
            public void UpdateField(string key, string value)
            {
                this.rtfDocument = Regex.Replace(this.rtfDocument, string.Format(@"\({0}\)", key), value);
            }

            #endregion
        }
    }
}