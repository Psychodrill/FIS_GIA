namespace Ege.Hsc.Scheduler
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
            this.SchedulerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SchedulerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // SchedulerProcessInstaller
            // 
            this.SchedulerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.SchedulerProcessInstaller.Password = null;
            this.SchedulerProcessInstaller.Username = null;
            // 
            // SchedulerInstaller
            // 
            this.SchedulerInstaller.ServiceName = "Hsc.Scheduler";
            this.SchedulerInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SchedulerProcessInstaller,
            this.SchedulerInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller SchedulerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SchedulerInstaller;
    }
}