using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Fbs.Utility
{
    /// <summary>
    /// Параметров соединения с smtp сервером
    /// </summary>
    //public class SmtpParams
    //{

    //    #region Public properties

    //    public string Server = string.Empty;
    //    public string Login = string.Empty;
    //    public string Password = string.Empty;
    //    public bool EnableSSL
    //    {
    //        get { return Properties.Settings.Default.MailClient_EnableSSL; }
    //    }

    //    #endregion

    //    #region Public methods

    //    public SmtpParams() { }

    //    public SmtpParams(string server, string login, string password)
    //    {
    //        if (server != null)
    //            this.Server = server;
    //        if (login != null)
    //            this.Login = login;
    //        if (password != null)
    //            this.Password = password;
    //    }

    //    #endregion

    //}

    /// <summary>
    /// Email сообщение
    /// </summary>
    public class EmailMessage
    {

        #region Private properties

        private const string AddressDelimeters = ",; ";
        private static Regex addressReg = new Regex("(.+)\\s[\\(|\\<|\\[](.+)[\\)|\\>|\\]]", RegexOptions.Compiled);

        private MailAddress mToAddress;
        private MailAddress mFromAddress;
        private MailAddressCollection mBccAddresses = new MailAddressCollection();

        #endregion

        #region Public properties

        public string To;
        public string From;
        public string Subject;
        public string Body;
        public string ContentType;
        public System.Text.Encoding Encoding;
        public List<HttpPostedFile> AttachedFilesFromHttp;
        public List<string> AttachedFilesFromDisk;
        public Pair[] Params;

        #endregion

        #region Private methods

        private string ReplaceMetaVars(string source, Pair[] @params)
        {
            string res = source;
            foreach (Pair pair in @params)
                if (pair.Second != null)
                    res = res.Replace((string)pair.First, (string)pair.Second);

            return res;
        }

        private MailAddress MakeMailAddress(string source)
        {
            if (addressReg.IsMatch(source))
                return new MailAddress(addressReg.Match(source).Result("$2"), addressReg.Match(source).Result("$1"));

            return new MailAddress(source.Trim());
        }

        private void PrepareAddresses()
        {
            mFromAddress = MakeMailAddress(From);

            string toNorm = To.Trim(AddressDelimeters.ToCharArray());
            string[] addrList = toNorm.Split(AddressDelimeters.ToCharArray());
            mToAddress = MakeMailAddress(addrList[0]);

            if (addrList.Length > 1)
            {
                string address;
                Int32 addressIndex;
                for (addressIndex = 1; addressIndex <= addrList.Length - 1; addressIndex++)
                {
                    address = addrList[addressIndex].Trim();
                    if (address.Length > 0)
                        mBccAddresses.Add(MakeMailAddress(address));
                }
            }
        }

        private void AddBccAddresses(MailMessage message)
        {
            foreach (MailAddress addr in mBccAddresses)
            {
                message.Bcc.Add(addr);
            }
        }
        // Замена символов, которые отсутствуют в KOI8-U 
        public static string ReplaceAbsentSymbols(string str)
        {
            if ((str == null))
                return string.Empty;

            str = str.Replace("«", "«");
            str = str.Replace("»", "»");
            str = str.Replace("№", "№");
            str = str.Replace("—", "—");
            return str;
        }

        #endregion

        #region Public methods

        public EmailMessage() { }

        public MailMessage ToMailMessage()
        {
            PrepareAddresses();
            MailMessage message = new MailMessage(mFromAddress, mToAddress);
            AddBccAddresses(message);

            // Замена метапеременных
            if (Subject != null)
                message.Subject = ReplaceMetaVars(Subject, Params);

            if (!string.IsNullOrEmpty(Body))
                message.Body = ReplaceMetaVars(Body, Params);

            // Тип письма
            if (ContentType.ToLower() == "html")
                message.IsBodyHtml = true;

            // Кодировка письма
            if ((Encoding != null))
            {
                // Заменим отсутсвующие символы
                if (message.IsBodyHtml && IsKOI8BasedEncoding(Encoding))
                    message.Body = ReplaceAbsentSymbols(message.Body);

                message.SubjectEncoding = Encoding;
                message.BodyEncoding = Encoding;
            }
            // Если есть массив прикрепленных файлов, отправим их
            if (AttachedFilesFromHttp != null)
                foreach (HttpPostedFile file in AttachedFilesFromHttp)
                    message.Attachments.Add(new Attachment(file.InputStream, Path.GetFileName(file.FileName)));

            // Если есть массив прикрепленных файлов, отправим их
            if (AttachedFilesFromDisk != null)
                foreach (string fileName in AttachedFilesFromDisk)
                    if (File.Exists(fileName))
                        message.Attachments.Add(new Attachment(fileName));

            return message;
        }

        #endregion

        #region " Private  methods "

        private bool IsKOI8BasedEncoding(Encoding enc)
        {
            return object.ReferenceEquals(enc, System.Text.Encoding.GetEncoding("koi8-r")) || object.ReferenceEquals(enc, System.Text.Encoding.GetEncoding("koi8-u"));
        }

        #endregion

    }

    /// <summary>
    /// Рассыльщик email сообщений
    /// </summary>
    public class EmailSender
    {

        #region Public properties

        public readonly EmailMessage Message;
       // public SmtpParams SmtpParams;

        #endregion

        #region Public methods

        public EmailSender(EmailMessage message)
        {
            if (message.To == null) throw new ArgumentNullException("to");
            if (message.From == null) throw new ArgumentNullException("from");
            if (message.ContentType == null) message.ContentType = "html";
            if (message.Encoding == null) message.Encoding = null;
            if (message.AttachedFilesFromHttp == null) message.AttachedFilesFromHttp = new List<HttpPostedFile>();
            if (message.AttachedFilesFromDisk == null) message.AttachedFilesFromDisk = new List<string>();
            if (message.Params == null) message.Params = (Pair[])Array.CreateInstance(typeof(Pair), 0);

            this.Message = message;
        }

        public EmailSender(string to, string from, string subject, string body)
        {
            if (to == null) throw new ArgumentNullException("to");
            if (from == null) throw new ArgumentNullException("from");

            EmailMessage email = new EmailMessage();
            email.To = to;
            email.From = from;
            email.Subject = subject;
            email.Body = body;
            email.ContentType = "html";
            email.Encoding = null;
            email.AttachedFilesFromHttp = new List<HttpPostedFile>();
            email.AttachedFilesFromDisk = new List<string>();
            email.Params = (Pair[])Array.CreateInstance(typeof(Pair), 0);

            this.Message = email;
        }

        public EmailSender(string to, string from, string subject, string body, string contentType,
                Encoding encoding, List<HttpPostedFile> attachedFilesFromHttp, Pair[] @params)
        {
            if (to == null) throw new ArgumentNullException("to");
            if (from == null) throw new ArgumentNullException("from");
            if (contentType == null) contentType = "html";
            if (encoding == null) encoding = null;
            if (attachedFilesFromHttp == null) attachedFilesFromHttp = new List<HttpPostedFile>();
            if (@params == null) @params = (Pair[])Array.CreateInstance(typeof(Pair), 0);

            EmailMessage email = new EmailMessage();
            email.To = to;
            email.From = from;
            email.Subject = subject;
            email.Body = body;
            email.ContentType = contentType;
            email.Encoding = encoding;
            email.AttachedFilesFromHttp = attachedFilesFromHttp;
            email.AttachedFilesFromDisk = new List<string>();
            email.Params = @params;

            this.Message = email;
        }

        public EmailSender(string to, string from, string subject, string body, string contentType,
                Encoding encoding, List<HttpPostedFile> attachedFilesFromHttp, List<string> attachedFilesFromDisk,
                Pair[] @params)
        {
            if (to == null) throw new ArgumentNullException("to");
            if (from == null) throw new ArgumentNullException("from");
            if (contentType == null) contentType = "html";
            if (encoding == null) encoding = null;
            if (attachedFilesFromHttp == null) attachedFilesFromHttp = new List<HttpPostedFile>();
            if (attachedFilesFromDisk == null) attachedFilesFromDisk = new List<string>();
            if (@params == null) @params = (Pair[])Array.CreateInstance(typeof(Pair), 0);

            EmailMessage email = new EmailMessage();
            email.To = to;
            email.From = from;
            email.Subject = subject;
            email.Body = body;
            email.ContentType = contentType;
            email.Encoding = encoding;
            email.AttachedFilesFromHttp = attachedFilesFromHttp;
            email.AttachedFilesFromDisk = attachedFilesFromDisk;
            email.Params = @params;

            this.Message = email;
        }

        public void Send()
        {
            //if (SmtpParams == null)
            //    SmtpParams = new SmtpParams();

            SmtpClient mail = CreateSmtpClient();
            
            //if (SmtpParams.Server.Length == 0)
            //    mail = new SmtpClient();
            //else
            //{
            //    mail = new SmtpClient(SmtpParams.Server);
            //    if (SmtpParams.Login.Length > 0 && SmtpParams.Password.Length > 0)
            //        mail.Credentials = new NetworkCredential(SmtpParams.Login, SmtpParams.Password);
            //}

            MailMessage msg = Message.ToMailMessage();
           // mail.EnableSsl = SmtpParams.EnableSSL;
            try
            {
                mail.Send(msg);
            }
            finally
            {
                foreach (Attachment attach in msg.Attachments)
                    attach.ContentStream.Close();
            }
        }

        public static void SendMessage(MailMessage message)
        {
            if(message.To==null )
                throw new ArgumentNullException("Не задан получатель");

            SmtpClient MailSender = CreateSmtpClient();
            MailSender.Send(message);
        }

        private static SmtpClient CreateSmtpClient()
        {
            SmtpClient Result = new SmtpClient();
            Result.EnableSsl = Fbs.Utility.Properties.Settings.Default.MailClient_EnableSSL;
            return Result;
        }

        #endregion

    }
}
