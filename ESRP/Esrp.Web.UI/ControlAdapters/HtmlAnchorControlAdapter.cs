using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Esrp.Web.Extentions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;
using Esrp.Web.UrlShortener;
namespace Esrp.Web.ControlAdapters
{
    public class HtmlAnchorControlAdapter : System.Web.UI.Adapters.ControlAdapter
    {
        protected override void Render(HtmlTextWriter writer)
        {
            string path = HttpContext.Current.Request.Path.ToLower();

            int lastExtension = path.LastIndexOf(".aspx");
            
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnhacedSecurity"]) && lastExtension!=-1)
            {
                if (this.Control is HtmlForm)
                    base.Render(new HtmlFormTextWriter(writer));
                else
                    base.Render(new LiteralControlTextWriter(writer, this.Control));
            }
            else 
                base.Render(writer);
        }
    }
    public class HtmlFormTextWriter : HtmlTextWriter
    {
        StringWriter _sw = new StringWriter();
        HtmlTextWriter _innerWriter;
        TextWriter _mainWriter;
        /// <summary>
        /// Initializes a new instance of the RewriteFormHtmlTextWriter class
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public HtmlFormTextWriter(HtmlTextWriter writer)
            : base(writer)
        {
            this.InnerWriter = writer.InnerWriter;
            this._mainWriter = writer;
          
        }

         /// <summary>
        /// Initializes a new instance of the RewriteFormHtmlTextWriter class
        /// </summary>
        /// <param name="writer">An IO text writer</param>
        public HtmlFormTextWriter(System.IO.TextWriter writer)
            : base(writer)
        {
            this.InnerWriter = writer;
            this._mainWriter = writer;
        }

        public override void Write(string s)
        {
            if (String.IsNullOrEmpty(s))
                return;
           
            if (Regex.IsMatch(s, "\\<a") && !Regex.IsMatch(s,"\\<a.+?\\>"))
            {
                if (this.InnerWriter != this._innerWriter)
                {
                    _sw = new StringWriter();
                    this._innerWriter = new HtmlTextWriter(this._sw);
                    this.InnerWriter = this._innerWriter;
                }
                base.Write(s);
                return;
            }
            if (this._innerWriter == this.InnerWriter && Regex.IsMatch(s, "\\>"))
            {
                base.Write(s);
                s = _sw.ToString();
                this.InnerWriter = _mainWriter;
                
            }
            if (Regex.IsMatch(s, "\\<a.+?\\>", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                foreach (Match match in Regex.Matches(s, "\\<a.+?href=[\"'](.+?)[\"'].*?\\>", System.Text.RegularExpressions.RegexOptions.Singleline))
            {
                string newUrl = ProcessLink(match.Groups[1].Value);
                s = s.Replace(match.Groups[1].Value, newUrl);
            }
            base.Write(s);
        }
        /// <summary>
        /// Do the actual attribute writing of the action attribute
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <param name="fEncode">Dont know</param>
        public override void WriteAttribute(string name, string value, bool fEncode)
        {

            //Uri url = new Uri(oldValue, UriKind.RelativeOrAbsolute);
            if (name.Equals("action"))
            {
                value = ProcessLink(value);
                //var context = HttpContext.Current;
                //if (!value.Contains("?actionHash") && !value.StartsWith("javascript:"))
                //{
                //    RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider();
                //    string hash = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(value.Substring(value.IndexOf('?')+1)), false)));
                //    value = Regex.Replace(value, "\\?(.+)", String.Format("?actionHash={0}", hash));
                //}

            }

            base.WriteAttribute(name, value, fEncode);
        }
        private string ProcessLink(string s)
        {
            string oldValue = s;
            Uri url = new Uri(oldValue, UriKind.RelativeOrAbsolute);
            
            string value = oldValue;

            if (oldValue.Contains("?") &&  !Regex.IsMatch(oldValue, "\\?[\\d\\w]+$") && !Regex.IsMatch(oldValue, "^(#)|(javascript\\:)|(mailto\\:)") && (!url.IsAbsoluteUri || (url.Host == HttpContext.Current.Request.Url.Host && url.Port == HttpContext.Current.Request.Url.Port)))
            {
                
                string hash = ShortUrlUtils.GenerateShortUrl(oldValue.Substring(oldValue.IndexOf('?') + 1));
                value = Regex.Replace(oldValue, "\\?(.+)", String.Format("?{0}", hash));
            }
            return value;
        }
    }
    public class LiteralControlTextWriter : HtmlTextWriter
    {
        Control _control;
        bool _isRendered;
        StringWriter _sw = new StringWriter();
        HtmlTextWriter _innerWriter;
        TextWriter _mainWriter;
        /// <summary>
        /// Initializes a new instance of the RewriteFormHtmlTextWriter class
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public LiteralControlTextWriter(HtmlTextWriter writer,Control control)
            : base(writer)
        {
            this.InnerWriter = writer.InnerWriter;
            this._control = control;
            this._mainWriter = writer;
        }

         /// <summary>
        /// Initializes a new instance of the RewriteFormHtmlTextWriter class
        /// </summary>
        /// <param name="writer">An IO text writer</param>
        public LiteralControlTextWriter(System.IO.TextWriter writer, Control control)
            : base(writer)
        {
            this.InnerWriter = writer;
            this._control = control;
            this._mainWriter = writer;
        }
        protected override void FilterAttributes()
        {
            base.FilterAttributes();
        }
        public override void Write(string s)
        {
            if (this._isRendered || this._control is HtmlForm)
                return;
            if (this._control is DataBoundLiteralControl)
            {
                s = (this._control as DataBoundLiteralControl).Text;
                this._isRendered = true;
                
            }
            if (this._control is DesignerDataBoundLiteralControl)
            {
                s = (this._control as DesignerDataBoundLiteralControl).Text;
            }
            if (String.IsNullOrEmpty(s))
                return;
            if (Regex.IsMatch(s, "\\<a") && !Regex.IsMatch(s, "\\<a.+?\\>"))
            {
                if (this.InnerWriter != this._innerWriter)
                {
                    _sw = new StringWriter();
                    this._innerWriter = new HtmlTextWriter(this._sw);
                    this.InnerWriter = this._innerWriter;
                }
                base.Write(s);
                return;
            }
            if (this._innerWriter == this.InnerWriter && Regex.IsMatch(s, "\\>"))
            {
                base.Write(s);
                s = _sw.ToString();
                this.InnerWriter = _mainWriter;
            }
            if (Regex.IsMatch(s, "\\<a.+?\\>", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                foreach (Match match in Regex.Matches(s, "\\<a.+?href=[\"'](.+?)[\"'].*?\\>", System.Text.RegularExpressions.RegexOptions.Singleline))
                {
                    string oldUrl = Regex.Match(s, "\\<a.+?href=[\"'](.+?)[\"']").Groups[1].Value;
                    string newUrl = this.ProcessLink(oldUrl);
                    s = s.Replace(oldUrl, newUrl);
                }
            }
            base.Write(s);
        }

        private string ProcessLink(string s)
        {
            string oldValue = s;
            Uri url = new Uri(oldValue,UriKind.RelativeOrAbsolute);
            
            string value = oldValue;
            if (oldValue.Contains("?") && !Regex.IsMatch(oldValue, "\\?[\\d\\w]+$") && !Regex.IsMatch(oldValue, "^(#)|(javascript\\:)|(mailto\\:)") && (!url.IsAbsoluteUri || (url.Host == HttpContext.Current.Request.Url.Host && url.Port == HttpContext.Current.Request.Url.Port)))
            {

                string hash = ShortUrlUtils.GenerateShortUrl(oldValue.Substring(oldValue.IndexOf('?') + 1));
                value = Regex.Replace(oldValue, "\\?(.+)", String.Format("?{0}", hash));
            }
            return value;
        }

        protected override bool OnAttributeRender(string name, string value, HtmlTextWriterAttribute key)
        {
            return base.OnAttributeRender(name, value, key);
        }

        public override void WriteAttribute(string name, string value)
        {
           
            this.WriteAttribute(name, value,true);
        }
        /// <summary>
        /// Do the actual attribute writing of the action attribute
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <param name="fEncode">Dont know</param>
        public override void WriteAttribute(string name, string value, bool fEncode)
        {
            
            //Uri url = new Uri(oldValue, UriKind.RelativeOrAbsolute);
            if (name.Equals("href") )
            {
               value =ProcessLink(value);
                //var context = HttpContext.Current;
                //if (!value.Contains("?actionHash") && !value.StartsWith("javascript:"))
                //{
                //    RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider();
                //    string hash = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(value.Substring(value.IndexOf('?')+1)), false)));
                //    value = Regex.Replace(value, "\\?(.+)", String.Format("?actionHash={0}", hash));
                //}

            }

            base.WriteAttribute(name, value, fEncode);
        }
    }
}
