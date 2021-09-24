using System;
using System.IO;
using System.Text;
using System.Web;

namespace GVUZ.Web.Helpers
{
    public class ResponseFileWriter : IDisposable
    {
        private readonly HttpResponseBase _response;
        private bool _disposed;

        public ResponseFileWriter(HttpServerUtilityBase server, HttpRequestBase request, HttpResponseBase response, string fileName, string contentType)
        {
            #region Assert arguments
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("argument is null or empty", "fileName");
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("argument is null or empty", "contentType");
            } 
            #endregion

            _response = response;
            
            fileName = fileName.Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\n", string.Empty).Replace(" ", "_").Replace("\"", string.Empty);
            _response.Clear();
            if (request.Browser.Browser == "IE" || request.Browser.Browser == "InternetExplorer")
            {
                string attachment = string.Format("attachment; filename=\"{0}\"", server.UrlPathEncode(fileName));
                _response.AddHeader("Content-Disposition", attachment);
            }
            else
            {
                _response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            }

            _response.ContentType = contentType;
            _response.Charset = "utf-8";
            _response.HeaderEncoding = Encoding.UTF8;
            _response.ContentEncoding = Encoding.UTF8;
        }

        public Stream Output
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("ResponseFileWriter");
                }

                return _response.OutputStream;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    try
                    {
                        _response.OutputStream.Flush();
                        _response.End();
                    }
                    finally
                    {
                        _disposed = true;
                    }
                }
            }
        }

        ~ResponseFileWriter()
        {
            Dispose(false);
        }
    }
}