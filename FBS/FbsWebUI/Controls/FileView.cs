using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Fbs.Web.Controls
{
    /// <summary>
    /// Отображение содержимого заданного файла
    /// </summary>
    /// <remarks>
    /// На данный момент корректно отображаются лишь файлы в кодировке utf-8
    /// </remarks>
    public class FileView : System.Web.UI.Control
    {
        private string mFilePath = string.Empty;

        /// <summary>
        /// Путь до файла относительно корня диска или сервера
        /// </summary>
        public string FilePath
        {
            get { return mFilePath; }
            set { mFilePath = value; }
        }

        /// <summary>
        /// Заменять переводы строк на тэг &lt;br/&gt;
        /// </summary>
        /// <remarks>
        /// По умолчанию True
        /// </remarks>
        public bool ConvertLineBrakes = true;

        // TODO: определение кодировки файла
        //public string FileEncoding;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Проверю наличие файла по указанному пути
            string filePath = string.Empty;
            if (File.Exists(FilePath))
                filePath = FilePath;
            else if (File.Exists(HttpContext.Current.Server.MapPath(FilePath)))
                filePath = HttpContext.Current.Server.MapPath(FilePath);
            else
                throw new FileNotFoundException(FilePath);

            // Получу содержимое файла
            string fileText;
            using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default))
                fileText = sr.ReadToEnd();

            // Заменю переводы строк на <br/>
            if (ConvertLineBrakes)
                fileText = fileText.Replace("\r\n", "<br/>");

            // Отображу контент файла на странице
            this.Controls.Add(new LiteralControl(fileText));
        }
    }
}
