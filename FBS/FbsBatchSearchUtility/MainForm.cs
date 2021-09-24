namespace FbsBatchSearchUtility
{
    using System;
    using System.Windows.Forms;

    using FbsBatchSearchUtility.BLL;

    /// <summary>
    /// Главная форма приложения
    /// </summary>
    public partial class MainForm : Form
    {
        private ReportService reportService = new ReportService();
        private int currentPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowAuthDialog()
        {
            Enabled = false;

            this.reportService.ConnectionStringManagerInstance.LoadConnectionSettings();

            var loginForm = new LoginForm();

            loginForm.ReportServiceInstance = this.reportService;
            loginForm.FormClosed += this.LoginFormClosedHandler;

            loginForm.Login = this.reportService.ConnectionStringManagerInstance.Connection.UserName;
            loginForm.Password = this.reportService.ConnectionStringManagerInstance.Connection.Password;
            loginForm.UseSystemAuth = this.reportService.ConnectionStringManagerInstance.Connection.UseSystemCreditionals;
            loginForm.ShowDialog();
        }

        private void LoginFormClosedHandler(object sender, EventArgs e)
        {
            if (!this.reportService.Connected)
            {
                Close();
            }

            var loginForm = sender as LoginForm;
            if (loginForm != null && loginForm.SaveCreditionals)
            {
                this.reportService.ConnectionStringManagerInstance.SaveConnectionSettings();
            }

            this.Enabled = true;
            this.reportService.Disconnect();
        }

        private void OpenButtonClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            fileNameInput.Text = openFileDialog.FileName;

            var opened = this.reportService.ReportFilterInstance.LoadFilterFromCsvFile(this.openFileDialog.FileName);
            if (opened && this.reportService.ReportFilterInstance.FilterData != null)
            {
                searchButton.Enabled = true;
            }
        }

        private void SearchButtonClick(object sender, EventArgs e)
        {
            if (this.reportService.ReportFilterInstance.FilterData == null)
            {
                return;
            }


            listingControlsPanel.Enabled = true;
            this.currentPage = 0;
            this.UpdateInterfaceData();
        }

        private void ExportButtonClick(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.reportService.SaveResultsToCsvFile(this.saveFileDialog.FileName);
        }

        /// <summary>
        /// Обновляет данные оинтерфейса: текущая выборка на основе страницы отображения, количество результатов и пр.
        /// </summary>
        protected void UpdateInterfaceData()
        {
            int rowsPerPage = Convert.ToInt32(resultsPerPage.Text);
            int totalRows = this.reportService.Count();
            int totalPages = totalRows / rowsPerPage + 1;

            if (this.currentPage >= totalPages)
            {
                return;
            }

            this.resultsDataGrid.DataSource = 
                this.reportService.Select(this.currentPage * rowsPerPage, (this.currentPage + 1) * rowsPerPage);

            resultsNumLabel.Text = totalRows.ToString();
            this.currentPagelabel.Text = (this.currentPage + 1).ToString() + "/" + totalPages;
        }

        private void NextPageButtonClick(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(resultsPerPage.Text);
            int totalPages = this.reportService.Count() / count + 1;

            if (this.currentPage < totalPages - 1)
            {
                this.currentPage++;
                this.UpdateInterfaceData();
            }
        }

        private void PreviousPageButtonClick(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(resultsPerPage.Text);
            int totalPages = this.reportService.Count() / count + 1;

            if (this.currentPage > 0)
            {
                this.currentPage--;

                if (this.currentPage >= totalPages)
                {
                    this.currentPage = totalPages - 1;
                }

                this.UpdateInterfaceData();
            }
        }

        private void FirstPageButtonClick(object sender, EventArgs e)
        {
            if (this.currentPage == 0)
            {
                return;
            }

            this.currentPage = 0;
            this.UpdateInterfaceData();
        }

        private void LastPageButtonClick(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(resultsPerPage.Text);
            int totalPages = this.reportService.Count() / count + 1;

            if (this.currentPage == totalPages - 1)
            {
                return;
            }

            this.currentPage = totalPages - 1;
            this.UpdateInterfaceData();
        }

        private void ResultsPerPageTextChanged(object sender, EventArgs e)
        {
            this.currentPage = 0;
            this.UpdateInterfaceData();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            this.ShowAuthDialog();
        }

        private void FullOrgNameCheckedChanged(object sender, EventArgs e)
        {
            this.reportService.ReportFilterInstance.UseFullOrgName = ((CheckBox)sender).Checked;
        }

        private void ShortOrgNameCheckedChanged(object sender, EventArgs e)
        {
            this.reportService.ReportFilterInstance.UseShortOrgName = ((CheckBox)sender).Checked;
        }

        private void InnCheckedChanged(object sender, EventArgs e)
        {
            this.reportService.ReportFilterInstance.UseInn = ((CheckBox)sender).Checked;
        }

        private void OgrnCheckedChanged(object sender, EventArgs e)
        {
            this.reportService.ReportFilterInstance.UseOgrn = ((CheckBox)sender).Checked;
        }

        private void FounderCheckedChanged(object sender, EventArgs e)
        {
            this.reportService.ReportFilterInstance.UseFounder = ((CheckBox)sender).Checked;
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsToolStripMenuItemClick(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ConnectionStringManagerInstance = this.reportService.ConnectionStringManagerInstance;
            settingsForm.ShowDialog();
        }
    }
}