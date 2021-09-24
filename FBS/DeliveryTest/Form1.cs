using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Mail;

namespace DeliveryTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //DeliveriesCore.DeliveriesManager Manager = new DeliveriesCore.DeliveriesManager(
                //     ConfigurationManager.AppSettings["From"],
                //     ConfigurationManager.AppSettings["SMTPHost"],
                //     int.Parse(ConfigurationManager.AppSettings["SMTPPort"]),
                //     ConfigurationManager.AppSettings["Login"],
                //     ConfigurationManager.AppSettings["Password"],
                //     ConfigurationManager.ConnectionStrings["FBS_DB_CS"].ConnectionString
                //     );
                //Manager.SendDeliveries();

                string From = TBFrom.Text;
                string Host = TBHost.Text;
                int Port = int.Parse(TBPort.Text );
                string Login = TBLogin.Text;
                string Pass = TBPassword.Text;
                SmtpClient Client = new SmtpClient(Host, Port);
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                if (!String.IsNullOrEmpty(Login))
                {
                    Client.EnableSsl = ChUseSSL.Checked ;
                    Client.Credentials = new System.Net.NetworkCredential(Login, Pass);
                }
                Client.Send(new MailMessage(From, TBTo.Text, "TEST", "TEST"));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.Message);
            }
            MessageBox.Show("Test END");
        }
    }
}
