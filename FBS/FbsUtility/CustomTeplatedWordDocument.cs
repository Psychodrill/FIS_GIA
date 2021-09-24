using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace Fbs.Utility
{
    /// <summary>
    /// Базовый класс для документов Word, создаваемых из шаблона.
    /// </summary>
    public abstract class CustomTeplateBaseWordDocument : IDisposable
    {
        private SortedList mFieldValues = new SortedList();
        private string mFileName;
        private string mTemplateFileName;
        private bool mHasErrors = false;
        private bool mDisposed = false;
        private bool mNeedDeleteFileOnDispose = false;
        static private object mLock = new object();

        // Получить значение поля формы документа.
        private string GetDocumentFormFieldValue(string value)
        {
            if (value == null)
                return string.Empty;
            if (value.Length > 254)
                return string.Format("{0}...", value.Substring(0, 253));
            return value;
        }

        private void CreateDocument()
        {
            if (mFileName != null)
                return;

            mFileName = mTemplateFileName;
            try
            {
                // Создаем документ на основании файла-шаблона.
                var document = new RtfDocument(mTemplateFileName);
             
                try
                {
                    foreach (DictionaryEntry entry in mFieldValues)
                    {
                        document.UpdateField(entry.Key.ToString(), GetDocumentFormFieldValue(
                                                            Convert.ToString(entry.Value)));
                    }
                }
                catch (Exception ex)
                {
                    OnError(new ErrorEventArgs(ex));
                    mHasErrors = true;
                }

                // Сохраняем в формате RTF во временный файл.
                mFileName = Path.GetTempFileName();
                document.Save(mFileName);

                mNeedDeleteFileOnDispose = true;
            }
            catch (Exception ex)
            {
                OnError(new ErrorEventArgs(ex));
                mHasErrors = true;
            }
        }

        /// <summary>
        /// Работа с rtf документом
        /// </summary>
        private class RtfDocument
        {
            private string rtfDocument;

            public RtfDocument(string path)
            {
                rtfDocument = File.ReadAllText(path);
            }

            public void UpdateField(string key, string value)
            {
                rtfDocument = Regex.Replace(rtfDocument, string.Format(@"\({0}\)", key), value);
            }

            public void Save(string path)
            {
                File.WriteAllText(path,rtfDocument,Encoding.GetEncoding(1251));
            }
        }

        private void Dispose(bool disposing)
        {
            if (this.mDisposed)
                return;

            CleanUnmanagmentResources();
            mDisposed = true;
        }

        private void CleanUnmanagmentResources()
        {
            if (mNeedDeleteFileOnDispose)
                try
                {
                    File.Delete(mFileName);
                }
                catch
                {
                    // Если не удалось удалить (файл занят другим процессом), оставляем.
                }
        }

        /// <summary>
        /// Инициализировать поля документа.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void InitializeFields(string key, string value)
        {
            mFieldValues.Add(key, value);
        }

        protected CustomTeplateBaseWordDocument(string templateFileName)
        {
            mTemplateFileName = templateFileName;
        }

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
                    CreateDocument();
                    return mFileName;
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
                    CreateDocument();
                    return mHasErrors;
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        ~CustomTeplateBaseWordDocument()
        {
            Dispose(false);
        }

        public event ErrorEventHandler Error;
        protected virtual void OnError(ErrorEventArgs e)
        {
            if (Error != null)
                Error(this, e);
        }
    }

}
