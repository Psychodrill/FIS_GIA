using System;
using System.Collections.Generic;
using System.Text;

namespace Esrp.UploadModule
{
    /// <summary>
    /// Класс для хранения данных о процессе загрузки файла
    /// </summary>
    [Serializable()]
    public class UploadStatus
    {

        #region "Private fields"

        private long mCurrentBytesTransfered = 0;
        private long mTotalBytes = 0;
        private string mFileId;
        private string mFileName;
        private string mFileType;

        #endregion

        #region "Public fields"

        /// <summary>
        /// Коды ошибок
        /// </summary>
        public enum ErrorEnum : int
        {
            /// <summary>
            /// Нет ошибок
            /// </summary>
            None = 0,

            /// <summary>
            /// Файл больше допустимого размера
            /// </summary>
            FileTooLarge = 1,

            /// <summary>
            /// Файл больше рекомендуемого размера
            /// </summary>
            FileIsGreaterThatRecommended = 2,

            /// <summary>
            /// Файл пуст
            /// </summary>
            FileIsEmpty = 3,

            /// <summary>
            /// Неизвестная ошибка
            /// </summary>
            UnknownError = 4
        }

        /// <summary>
        /// Закачка отменена
        /// </summary>
        public bool IsCanceled = false;

        /// <summary>
        /// Код ошибки
        /// </summary>
        public ErrorEnum ErrorLevel = ErrorEnum.None;

        /// <summary>
        /// Сообщение (текст) ошибки
        /// </summary>
        public string ErrorMsg;

        #endregion

        #region "Properties"

        /// <summary>
        /// Идентификатор файла (для звязи пользователя с закачкой)
        /// </summary>
        public string FileId
        {
            get { return mFileId; }
        }

        /// <summary>
        /// Тип (расширение) файла
        /// </summary>
        public string FileType
        {
            get { return mFileType; }
        }

        /// <summary>
        /// Имя файла в который осущесвляется запись
        /// </summary>
        public string FileName
        {
            get { return mFileName; }
        }

        /// <summary>
        /// Байт переданно
        /// </summary>
        public long CurrentBytesTransfered
        {
            get { return mCurrentBytesTransfered; }
            set { mCurrentBytesTransfered = value; }
        }

        /// <summary>
        /// Размер передаваемого файла
        /// </summary>
        public long TotalBytes
        {
            get { return mTotalBytes; }
            set { mTotalBytes = value; }
        }


        /// <summary>
        /// Закачка файла завершена
        /// </summary>
        public bool IsDone
        {
            get
            {
                return ((mTotalBytes > 0) && (mTotalBytes == mCurrentBytesTransfered)) ;
            }
        }

        #endregion

        public UploadStatus(string FileId, string FileName, string FileType)
        {
            mFileId = FileId;
            mFileName = FileName;
            mFileType = FileType;
        }
    }
}
