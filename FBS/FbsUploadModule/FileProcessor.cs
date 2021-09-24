using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Fbs.UploadModule
{
    /// <summary>
    /// Класс реализующий запись данных на диск
    /// </summary>
    public class FileProcessor : IDisposable
    {

        #region "Class Vars"
        /// <summary>
        /// Идентификатор формы с которой сделан запрос
        /// </summary>
        private string mFormPostID = "";
        /// <summary>
        /// Используется для поиска файла в запросе
        /// </summary>
        private string mFieldSeperator = "";
        /// <summary>
        /// Позиция в буфере
        /// </summary>
        private long mCcurrentBufferIndex = 0;
        /// <summary>
        /// Найдено начало файла в запросе
        /// </summary>
        private bool mStartFound = false;
        /// <summary>
        /// Найден конец файла в запросе
        /// </summary>
        private bool mEndFound = false;

        /// <summary>
        /// Имя текущего файла
        /// </summary>
        private string mCurrentFileName;

        /// <summary>
        /// Поток для записи данных
        /// </summary>
        private FileStream mFileStream = null;

        /// <summary>
        /// Поля для поиска данных в буфере
        /// </summary>
        private long mStartIndexBufferID = -1;
        private int mStartLocationInBufferID = -1;

        private long mEndIndexBufferID = -1;
        private int mEndLocationInBufferID = -1;


        /// <summary>
        /// Список уже загруженных буферов (максимум 2)
        /// </summary>
        private Dictionary<long, byte[]> mBufferHistory= new Dictionary<long, byte[]>();

        /// <summary>
        /// Список имен загруженных файлов
        /// </summary>
        private List<string> mFinishedFiles = new List<string>();

        #endregion

        #region "Constructors"
        
        public FileProcessor(string FileName)
        {
            mCurrentFileName = FileName;
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// Список имен загруженных файлов
        /// </summary>
        public List<string> FinishedFiles
        {
            get { return mFinishedFiles; }
        }

        #endregion

        #region "Methods

        /// <summary>
        /// Метод для записи данных в файл
        /// </summary>
        /// <param name="bufferData">данные для обработки</param>
        /// <param name="addToBufferHistory">если true ведет иссторию загруженных файлов</param>
        public void ProcessBuffer(ref byte[] bufferData)
        {
            int byteLocation = -1;

            // Найти начало файла если ранее не найдено
            if (!mStartFound)
            {
                byteLocation = GetStartBytePosition(ref bufferData);
                if (byteLocation != -1)
                {
                    mStartIndexBufferID = mCcurrentBufferIndex + 1;
                    mStartLocationInBufferID = byteLocation;
                    mStartFound = true;
                }
            }

            // Если начало найдено начну запись
            if (mStartFound)
            {
                int startLocation = 0;
                if (byteLocation != -1)
                {
                    startLocation = byteLocation;
                }

                int writeBytes = ( bufferData.Length - startLocation );
                int tempEndByte = GetEndBytePosition(ref bufferData);
                if (tempEndByte != -1)
                {
                    writeBytes = (tempEndByte - startLocation);
                    mEndFound = true;
                    mEndIndexBufferID = (mCcurrentBufferIndex + 1);
                    mEndLocationInBufferID = tempEndByte;
                }

                // Если есть что записать
                if (writeBytes > 0)
                {
                    // Если поток не создам - создам
                    if (mFileStream == null)
                    {
                        mFileStream = new FileStream(mCurrentFileName, FileMode.OpenOrCreate);
                    }

                    // Запишу данные в файл
                    mFileStream.Write(bufferData, startLocation, writeBytes);
                    mFileStream.Flush();
                }
            }

            // Если найден конец файла закрою поток
            if (mEndFound)
            {
                CloseStreams();
                mStartFound = false;
                mEndFound = false;

                // Убежусь что все прочитано 
                ProcessBuffer(ref bufferData);
            }

        }

        /// <summary>
        /// Закрыть поток записи файла
        /// </summary>
        public void CloseStreams()
        {
            if (mFileStream != null)
            {
                mFileStream.Dispose();
                mFileStream.Close();
                mFileStream = null;

                // добавить имя вайла в спиок загруженных
                mFinishedFiles.Add(mCurrentFileName);
            }
        }

        /// <summary>
        /// Метод для получения разделителей между файлом и заголовками в запросе
        /// </summary>
        /// <param name="bufferData"></param>
        public void GetFieldSeperators(ref byte[] bufferData)
        {
            try
            {
                mFormPostID = Encoding.UTF8.GetString(bufferData).Substring(29, 13);
                mFieldSeperator = "-----------------------------" + mFormPostID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GetFieldSeperators(): " + ex.Message);
            }
        }

        /// <summary>
        /// Метод для поиска данных в буфере (если нужно в предыдушем буфере)
        /// </summary>
        /// <param name="bufferData">данные для обработки</param>
        /// <returns>позиция в буфере</returns>
        private int GetStartBytePosition(ref byte[] bufferData)
        {
            int byteOffset = 0;
            if (mStartIndexBufferID == (mCcurrentBufferIndex + 1))
            {
                byteOffset = mStartLocationInBufferID;
            }

            // Check to see if the end index was found before this start index.  That way we keep moving ahead
            if (mEndIndexBufferID == (mCcurrentBufferIndex +1))
            {
                byteOffset = mEndLocationInBufferID;
            }

            int tempContentTypeStart = -1;
            // First see if we can find it in the current buffer batch.
            // Because there may be muliple posts in a form, we have to make sure we do not
            // re-return any possible byte offsets that we have returned before. This could lead
            // to an infinite loop.

            byte[] searchString = Encoding.UTF8.GetBytes("Content-Type: ");
            tempContentTypeStart = FindBytePattern(ref bufferData, ref searchString, byteOffset);

            if (tempContentTypeStart != -1)
            {
                // Found content type start location
                // Next search for \r\n\r\n at this substring
                //int tempSearchStringLocation = bufferDataUTF8.IndexOf("\r\n\r\n", tempContentTypeStart);
                searchString = Encoding.UTF8.GetBytes("\r\n\r\n");
                int tempSearchStringLocation = FindBytePattern(ref bufferData, ref searchString, tempContentTypeStart);

                if (tempSearchStringLocation != -1)
                {
                    // Found this.  Can get start of data here
                    // Add 4 to it. That is the number of positions before it gets to the start of the data
                    int byteStart = tempSearchStringLocation + 4;
                    return byteStart;
                }
            }
            else if((byteOffset - searchString.Length) > 0 ){

                return -1;
            }

            else
            {
                // Did not find it. Add this and previous buffer together to see if it exists.
                // Check to see if the buffer index is at the start. 
                if (mCcurrentBufferIndex > 0)
                {
                    // Get the previous buffer
                    byte[] previousBuffer = mBufferHistory[mCcurrentBufferIndex - 1];
                    byte[] mergedBytes = MergeArrays(ref previousBuffer, ref bufferData);
                    // Get the byte array for the text
                    byte[] searchString2 = Encoding.UTF8.GetBytes("Content-Type: ");
                    // Search the bytes for the searchString
                    tempContentTypeStart = FindBytePattern(ref mergedBytes, ref searchString2, previousBuffer.Length - searchString2.Length);

                    //tempContentTypeStart = combinedUTF8Data.IndexOf("Content-Type: ");
                    if (tempContentTypeStart != -1)
                    {
                        // Found content type start location
                        // Next search for \r\n\r\n at this substring

                        searchString2 = Encoding.UTF8.GetBytes("Content-Type: ");
                        // because we are searching part of the previosu buffer, we only need to go back the length of the search 
                        // array.  Any further, and our normal if statement would have picked it up when it first was processed.
                        int tempSearchStringLocation = FindBytePattern(ref mergedBytes, ref searchString2, (previousBuffer.Length - searchString2.Length));

                        if (tempSearchStringLocation != -1)
                        {
                            // Found this.  Can get start of data here
                            // It is possible for some of this to be located in the previous buffer.
                            // Find out where the excape chars are located.
                            if (tempSearchStringLocation > previousBuffer.Length)
                            {
                                // Located in the second object.  
                                // If we used the previous buffer, we shoudl not have to worry about going out of
                                // range unless the buffer was set to some really low number.  So not going to check for
                                // out of range issues.
                                int currentBufferByteLocation = (tempSearchStringLocation - previousBuffer.Length);
                                return currentBufferByteLocation;
                            }
                            else
                            {
                                // Located in the first object.  The only reason this could happen is if
                                // the escape chars ended right at the end of the buffer.  This would mean
                                // that that the next buffer would start data at offset 0
                                return 0;
                            }
                        }
                    }
                }
            }
            // indicate not found.
            return -1;
        }

        /// <summary>
        /// Method used for searching buffer data for end byte location, and if needed previous buffer data.
        /// </summary>
        /// <param name="bufferData">current buffer data needing to be processed.</param>
        /// <returns>Returns byte location of data to start</returns>
        private int GetEndBytePosition(ref byte[] bufferData)
        {

            int byteOffset = 0;
            // Check to see if the current bufferIndex is the same as any previous index found.
            // If it is, offset the searching by the previous location.  This will allow us to find the next leading
            // Stop location so we do not return a byte offset that may have happened before the start index.
            if (mStartIndexBufferID == (mCcurrentBufferIndex + 1))
            {
                byteOffset = mStartLocationInBufferID;
            }

            int tempFieldSeperator = -1;

            // First see if we can find it in the current buffer batch.
            byte[] searchString = Encoding.UTF8.GetBytes(mFieldSeperator);
            tempFieldSeperator = FindBytePattern(ref bufferData, ref searchString, byteOffset);

            if (tempFieldSeperator != -1)
            {
                // Found field ending. Depending on where the field seperator is located on this, we may have to move back into
                // the prevoius buffer to return its offset.
                if (tempFieldSeperator - 2 < 0)
                {
                    //TODO: Have to get the previous buffer data.
                    
                }
                else
                {
                    return (tempFieldSeperator - 2);
                }
            }
            else if ((byteOffset - searchString.Length) > 0)
            {

                return -1;
            }
            else
            {
                // Did not find it. Add this and previous buffer together to see if it exists.
                // Check to see if the buffer index is at the start. 
                if (mCcurrentBufferIndex > 0)
                {

                    // Get the previous buffer
                    byte[] previousBuffer = mBufferHistory[mCcurrentBufferIndex - 1];
                    byte[] mergedBytes = MergeArrays(ref previousBuffer, ref bufferData);
                    // Get the byte array for the text
                    byte[] searchString2 = Encoding.UTF8.GetBytes(mFieldSeperator);
                    // Search the bytes for the searchString
                    tempFieldSeperator = FindBytePattern(ref mergedBytes, ref searchString2, previousBuffer.Length - searchString2.Length + byteOffset);

                    if (tempFieldSeperator != -1)
                    {
                        // Found content type start location
                        // Next search for \r\n\r\n at this substring

                        searchString2 = Encoding.UTF8.GetBytes("\r\n\r\n");
                        int tempSearchStringLocation = FindBytePattern(ref mergedBytes, ref searchString2, tempFieldSeperator);
                        
                        if (tempSearchStringLocation != -1)
                        {
                            // Found this.  Can get start of data here
                            // It is possible for some of this to be located in the previous buffer.
                            // Find out where the excape chars are located.
                            if (tempSearchStringLocation > previousBuffer.Length)
                            {
                                // Located in the second object.  
                                // If we used the previous buffer, we shoudl not have to worry about going out of
                                // range unless the buffer was set to some really low number.  So not going to check for
                                // out of range issues.
                                int currentBufferByteLocation = (tempSearchStringLocation - previousBuffer.Length);
                                return currentBufferByteLocation;
                            }
                            else
                            {
                                // Located in the first object.  The only reason this could happen is if
                                // the escape chars ended right at the end of the buffer.  This would mean
                                // that that the next buffer would start data at offset 0
                                return -1;
                            }
                        }
                    }
                }
            }
            // indicate not found.
            return -1;
        }

        /// <summary>
        /// Method created to search for byte array patterns inside a byte array.
        /// </summary>
        /// <param name="containerBytes">byte array to search</param>
        /// <param name="searchBytes">byte array with pattern to search with</param>
        /// <param name="startAtIndex">byte offset to start searching at a specified location</param>
        /// <returns>-1 if not found or index of starting location of pattern</returns>
        private static int FindBytePattern(ref byte[] containerBytes, ref byte[] searchBytes, int startAtIndex)
        {
            int returnValue = -1;
            for (int byteIndex = startAtIndex; byteIndex < containerBytes.Length; byteIndex++)
            {

                // Make sure the searchBytes lenght does not exceed the containerbytes
                if (byteIndex + searchBytes.Length > containerBytes.Length)
                {
                    // return -1.
                    return -1;
                }

                // First the first reference of the bytes to search
                if (containerBytes[byteIndex] == searchBytes[0])
                {
                    bool found = true;
                    int tempStartIndex = byteIndex;
                    for (int searchBytesIndex = 1; searchBytesIndex < searchBytes.Length; searchBytesIndex++)
                    {
                        // Set next index
                        tempStartIndex++;
                        if (!(searchBytes[searchBytesIndex] == containerBytes[tempStartIndex]))
                        {
                            found = false;
                            // break out of the loop and continue searching.
                            break;
                        }
                    }
                    if (found)
                    {
                        // Indicates that the byte array has been found. Return this index.
                        return byteIndex;
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Used to merge two byte arrays into one.  
        /// </summary>
        /// <param name="arrayOne">First byte array to go to the start of the new array</param>
        /// <param name="arrayTwo">Second byte array to go to right after the first array that was passed</param>
        /// <returns>new byte array of all the new bytes</returns>
        private static byte[] MergeArrays(ref byte[] arrayOne, ref byte[] arrayTwo)
        {
            System.Type elementType = arrayOne.GetType().GetElementType();
            byte[] newArray = new byte[arrayOne.Length + arrayTwo.Length];
            arrayOne.CopyTo(newArray, 0);
            arrayTwo.CopyTo(newArray, arrayOne.Length);

            return newArray;
        }

        /// <summary>
        /// This is used as part of the clean up procedures. the Timer object will execute this function.
        /// </summary>
        /// <param name="filePath"></param>
        private static void DeleteFile(object filePath)
        {
            // File may have already been removed from the main appliation.
            try
            {
                if (System.IO.File.Exists((string)filePath))
                {
                    System.IO.File.Delete((string)filePath);
                }
            }
            catch { }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Clean up method.
        /// </summary>
        public void Dispose()
        {
            // Clear the buffer history
            mBufferHistory.Clear();
            GC.Collect();
        }

        #endregion
    }
}
