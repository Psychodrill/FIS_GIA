namespace Fbs.Utility
{
    using System.IO;

    /// <summary>
    /// хелперы для работы с файлами
    /// </summary>
    public static class IOUtility
    {
        #region Public Methods and Operators

        /// <summary>
        /// Проверить существует ли папка, и если нет, то создать
        /// </summary>
        /// <param name="folderPath">
        /// путь до папки, относительный или абсолютный
        /// </param>
        /// <param name="rootPath">
        /// корневая папка в случае относительного пути
        /// </param>
        /// <returns>
        /// абсолютный путь проверяемой папки 
        /// </returns>
        public static string CheckAndCreatePhisicalFolder(string folderPath, string rootPath)
        {
            string path = folderPath;
            if (!Path.IsPathRooted(folderPath))
            {
                path = rootPath.TrimEnd(new[] { '\\', '/' }) + "\\" + folderPath.TrimStart(new[] { '\\', '/' });
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        #endregion
    }
}