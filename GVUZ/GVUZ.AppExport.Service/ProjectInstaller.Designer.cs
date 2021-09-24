namespace GVUZ.AppExport.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.eventLogInstaller = new System.Diagnostics.EventLogInstaller();
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.Description = "Предназначена для осуществления операций экспорта заявлений из ФИС приема в фонов" +
    "ом режиме";
            this.serviceInstaller.DisplayName = "ФИС приема - служба экспорта";
            this.serviceInstaller.ServiceName = "gvuzappex";
            this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // eventLogInstaller
            // 
            this.eventLogInstaller.CategoryCount = 0;
            this.eventLogInstaller.CategoryResourceFile = null;
            this.eventLogInstaller.Log = "ФИС приема";
            this.eventLogInstaller.MessageResourceFile = null;
            this.eventLogInstaller.ParameterResourceFile = null;
            this.eventLogInstaller.Source = "ФИС приема - экспорт";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.serviceInstaller,
            this.eventLogInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
        private System.Diagnostics.EventLogInstaller eventLogInstaller;
    }
}