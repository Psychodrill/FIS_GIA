using System.Net;
using System.Net.Mail;
using FbsReportSender.Properties;

namespace FbsReportSender.Email
{
    public class EMailSender
    {
        private readonly SmtpClient mailClient;
        
        public EMailSender()
        {
            mailClient = new SmtpClient(Settings.Default.SMTPServer) {Port = Settings.Default.SMTPServer_Port};

            if (Settings.Default.SMTPServer_Login.Length > 0 && Settings.Default.SMTPServer_Password.Length > 0)
                mailClient.Credentials = new NetworkCredential(
                    Settings.Default.SMTPServer_Login, Settings.Default.SMTPServer_Password);

            mailClient.EnableSsl = Settings.Default.SMTPServer_EnableSSL;
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public void SendMail(MailMessage message)
        {
            try
            {
                message.To.Add(Settings.Default.EmailAddress_To);
                if (!string.IsNullOrEmpty(Settings.Default.EmailAddress_From))
                    message.From=new MailAddress(Settings.Default.EmailAddress_From);
                mailClient.Send(message);
            }
            finally
            {
                foreach (Attachment attach in message.Attachments)
                    attach.ContentStream.Close();
            }
        }

        public static void Send(MailMessage message)
        {
            EMailSender mailSender=new EMailSender();
            mailSender.SendMail(message);
        }
    }
}
