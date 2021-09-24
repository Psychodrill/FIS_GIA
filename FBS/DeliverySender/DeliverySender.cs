using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Configuration;

namespace DeliverySender
{
    public partial class DeliverySender : ServiceBase
    {
        public DeliverySender()
        {
            InitializeComponent();
        }

        System.Timers.Timer MainTimer;
        protected override void OnStart(string[] args)
        {
            DeliveriesCore.Logger.Write("----------Service started");

            int Seconds ;
            if (!int.TryParse(ConfigurationManager.AppSettings["ScanIntervalSeconds"], out Seconds))
            {
                DeliveriesCore.Logger.Write("ScanIntervalSeconds value is invalid and will be set to 60");
                Seconds = 60;
            }

            MainTimer = new System.Timers.Timer(Seconds*1000);
            MainTimer.AutoReset = true;
            MainTimer.Elapsed += new System.Timers.ElapsedEventHandler(MainTimer_Elapsed);
            MainTimer.Start();
        }

        void MainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DeliveriesCore.Logger.Write("Timer event handler started");
            try
            {
                DeliveriesCore.DeliveriesManager Manager = Manager = new DeliveriesCore.DeliveriesManager(
                    ConfigurationManager.AppSettings["From"],
                    ConfigurationManager.AppSettings["SMTPHost"],
                    ConfigurationManager.AppSettings["SMTPPort"],
                    ConfigurationManager.AppSettings["Login"],
                    ConfigurationManager.AppSettings["Password"],
                    ConfigurationManager.AppSettings["UseSSL"] == "true",
                    ConfigurationManager.AppSettings["ReplyTo"],
                    ConfigurationManager.ConnectionStrings["FBS_DB_CS"].ConnectionString,
                    ConfigurationManager.AppSettings["DebugMode"] == "true"
                    );

                Manager.SendOneDelivery();
            }
            catch (Exception ex)
            {
                DeliveriesCore.Logger.Write(ex.Message);
            }

            DeliveriesCore.Logger.Write("Timer event handler finished");
        }

        protected override void OnStop()
        {
            DeliveriesCore.Logger.Write("----------Service stopped");

            MainTimer.Stop();
            MainTimer.Dispose();
        }
    }
}
