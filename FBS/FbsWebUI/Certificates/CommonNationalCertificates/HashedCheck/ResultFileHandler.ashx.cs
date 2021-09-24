namespace Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck
{
    using System;
    using System.IO;
    using System.Web;

    /// <summary>
    /// хэндлер для загрузки файла с результатами пакетов по сумме
    /// </summary>
    public class ResultFileHandler : IHttpHandler
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether IsReusable.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The process request.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="Exception">
        /// в случае когда нет имени файла
        /// </exception>
        public void ProcessRequest(HttpContext context)
        {
            string file = context.Request.QueryString["file"];
            if (string.IsNullOrEmpty(file))
            {
                throw new Exception("no file to download");
            }
            
            string fileName = Path.Combine(Path.GetTempPath(), file);
            if (!File.Exists(fileName))
            {
                throw new Exception("no file exists");
            }

            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileName));
            context.Response.WriteFile(fileName, true);
            context.Response.End();
        }

        #endregion
    }
}