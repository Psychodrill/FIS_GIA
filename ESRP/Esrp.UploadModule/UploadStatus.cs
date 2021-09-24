using System;
using System.Collections.Generic;
using System.Text;

namespace Esrp.UploadModule
{
    /// <summary>
    /// ����� ��� �������� ������ � �������� �������� �����
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
        /// ���� ������
        /// </summary>
        public enum ErrorEnum : int
        {
            /// <summary>
            /// ��� ������
            /// </summary>
            None = 0,

            /// <summary>
            /// ���� ������ ����������� �������
            /// </summary>
            FileTooLarge = 1,

            /// <summary>
            /// ���� ������ �������������� �������
            /// </summary>
            FileIsGreaterThatRecommended = 2,

            /// <summary>
            /// ���� ����
            /// </summary>
            FileIsEmpty = 3,

            /// <summary>
            /// ����������� ������
            /// </summary>
            UnknownError = 4
        }

        /// <summary>
        /// ������� ��������
        /// </summary>
        public bool IsCanceled = false;

        /// <summary>
        /// ��� ������
        /// </summary>
        public ErrorEnum ErrorLevel = ErrorEnum.None;

        /// <summary>
        /// ��������� (�����) ������
        /// </summary>
        public string ErrorMsg;

        #endregion

        #region "Properties"

        /// <summary>
        /// ������������� ����� (��� ����� ������������ � ��������)
        /// </summary>
        public string FileId
        {
            get { return mFileId; }
        }

        /// <summary>
        /// ��� (����������) �����
        /// </summary>
        public string FileType
        {
            get { return mFileType; }
        }

        /// <summary>
        /// ��� ����� � ������� ������������� ������
        /// </summary>
        public string FileName
        {
            get { return mFileName; }
        }

        /// <summary>
        /// ���� ���������
        /// </summary>
        public long CurrentBytesTransfered
        {
            get { return mCurrentBytesTransfered; }
            set { mCurrentBytesTransfered = value; }
        }

        /// <summary>
        /// ������ ������������� �����
        /// </summary>
        public long TotalBytes
        {
            get { return mTotalBytes; }
            set { mTotalBytes = value; }
        }


        /// <summary>
        /// ������� ����� ���������
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
