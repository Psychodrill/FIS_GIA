using System.Windows.Forms;

namespace GVUZ.ImportServiceTest
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbBaseAddrInfo = new System.Windows.Forms.GroupBox();
            this.tbResultPath = new System.Windows.Forms.TextBox();
            this.lbResPath = new System.Windows.Forms.Label();
            this.lbMethod = new System.Windows.Forms.Label();
            this.cmbxMethodsList = new System.Windows.Forms.ComboBox();
            this.lbBaseAddress = new System.Windows.Forms.Label();
            this.tbBaseAddress = new System.Windows.Forms.TextBox();
            this.gbXmlQryInfo = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbIsSubDivisionOO = new System.Windows.Forms.Label();
            this.tbOOId = new System.Windows.Forms.TextBox();
            this.tb_Count = new System.Windows.Forms.NumericUpDown();
            this.btnMakeMulty = new System.Windows.Forms.Button();
            this.lbCount = new System.Windows.Forms.Label();
            this.btnSetQryParams = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.lbLogin = new System.Windows.Forms.Label();
            this.tbXmlQry = new System.Windows.Forms.RichTextBox();
            this.btnCallService = new System.Windows.Forms.Button();
            this.gbServiceXmlResponse = new System.Windows.Forms.GroupBox();
            this.tbServiceResponse = new System.Windows.Forms.RichTextBox();
            this.menuStripOps = new System.Windows.Forms.MenuStrip();
            this.tmenuOperations = new System.Windows.Forms.ToolStripMenuItem();
            this.извлечениеПакетовИзБДToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузкаПакетовВБДToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.импортПакетовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bLoadXml = new System.Windows.Forms.Button();
            this.openPackageDialog = new System.Windows.Forms.OpenFileDialog();
            this.gbBaseAddrInfo.SuspendLayout();
            this.gbXmlQryInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Count)).BeginInit();
            this.gbServiceXmlResponse.SuspendLayout();
            this.menuStripOps.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbBaseAddrInfo
            // 
            this.gbBaseAddrInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBaseAddrInfo.Controls.Add(this.tbResultPath);
            this.gbBaseAddrInfo.Controls.Add(this.lbResPath);
            this.gbBaseAddrInfo.Controls.Add(this.lbMethod);
            this.gbBaseAddrInfo.Controls.Add(this.cmbxMethodsList);
            this.gbBaseAddrInfo.Controls.Add(this.lbBaseAddress);
            this.gbBaseAddrInfo.Controls.Add(this.tbBaseAddress);
            this.gbBaseAddrInfo.Location = new System.Drawing.Point(12, 27);
            this.gbBaseAddrInfo.Name = "gbBaseAddrInfo";
            this.gbBaseAddrInfo.Size = new System.Drawing.Size(929, 117);
            this.gbBaseAddrInfo.TabIndex = 0;
            this.gbBaseAddrInfo.TabStop = false;
            this.gbBaseAddrInfo.Text = "Адрес сервиса";
            // 
            // tbResultPath
            // 
            this.tbResultPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResultPath.Location = new System.Drawing.Point(201, 77);
            this.tbResultPath.Name = "tbResultPath";
            this.tbResultPath.ReadOnly = true;
            this.tbResultPath.Size = new System.Drawing.Size(722, 22);
            this.tbResultPath.TabIndex = 4;
            // 
            // lbResPath
            // 
            this.lbResPath.Location = new System.Drawing.Point(6, 78);
            this.lbResPath.Name = "lbResPath";
            this.lbResPath.Size = new System.Drawing.Size(189, 19);
            this.lbResPath.TabIndex = 2;
            this.lbResPath.Text = "Результирующий путь:";
            this.lbResPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbMethod
            // 
            this.lbMethod.Location = new System.Drawing.Point(6, 49);
            this.lbMethod.Name = "lbMethod";
            this.lbMethod.Size = new System.Drawing.Size(189, 19);
            this.lbMethod.TabIndex = 3;
            this.lbMethod.Text = "Метод:";
            this.lbMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbxMethodsList
            // 
            this.cmbxMethodsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbxMethodsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxMethodsList.FormattingEnabled = true;
            this.cmbxMethodsList.Items.AddRange(new object[] {
            "/checkapplication (результат проверки заявлений в пакете)",
            "/checkapplication/single (проверка одного заявления)",
            "/delete (удаление)",
            "/delete/result (получение результата удаления)",
            "/dictionary (список справочников)",
            "/dictionarydetails (детали по справочнику)",
            "/import (импорт)",
            "/import/application/single (импорт одного заявления)",
            "/import/result (получение результата импорта)",
            "/validate (валидация)",
            "/institutioninfo (получение сведений по ОУ)",
            "/institutioninfo/partof (получение части сведений по ОУ)",
            "/test/checkapplication (тест проверки заявления)",
            "/test/delete (тест удаления)",
            "/test/dictionary (тест справочника)",
            "/test/dictionarydetails (тест деталей справочника)",
            "/test/import (тест импорта)"});
            this.cmbxMethodsList.Location = new System.Drawing.Point(201, 49);
            this.cmbxMethodsList.Name = "cmbxMethodsList";
            this.cmbxMethodsList.Size = new System.Drawing.Size(722, 22);
            this.cmbxMethodsList.TabIndex = 2;
            this.cmbxMethodsList.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lbBaseAddress
            // 
            this.lbBaseAddress.Location = new System.Drawing.Point(6, 22);
            this.lbBaseAddress.Name = "lbBaseAddress";
            this.lbBaseAddress.Size = new System.Drawing.Size(189, 19);
            this.lbBaseAddress.TabIndex = 1;
            this.lbBaseAddress.Text = "Базовый адрес:";
            this.lbBaseAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbBaseAddress
            // 
            this.tbBaseAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBaseAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GVUZ.ImportServiceTest.Properties.Settings.Default, "BasePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbBaseAddress.Location = new System.Drawing.Point(201, 21);
            this.tbBaseAddress.Name = "tbBaseAddress";
            this.tbBaseAddress.Size = new System.Drawing.Size(722, 22);
            this.tbBaseAddress.TabIndex = 0;
            this.tbBaseAddress.Text = global::GVUZ.ImportServiceTest.Properties.Settings.Default.BasePath;
            this.tbBaseAddress.TextChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // gbXmlQryInfo
            // 
            this.gbXmlQryInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbXmlQryInfo.Controls.Add(this.checkBox1);
            this.gbXmlQryInfo.Controls.Add(this.cbIsSubDivisionOO);
            this.gbXmlQryInfo.Controls.Add(this.tbOOId);
            this.gbXmlQryInfo.Controls.Add(this.tb_Count);
            this.gbXmlQryInfo.Controls.Add(this.btnMakeMulty);
            this.gbXmlQryInfo.Controls.Add(this.lbCount);
            this.gbXmlQryInfo.Controls.Add(this.btnSetQryParams);
            this.gbXmlQryInfo.Controls.Add(this.tbPassword);
            this.gbXmlQryInfo.Controls.Add(this.lbPassword);
            this.gbXmlQryInfo.Controls.Add(this.tbLogin);
            this.gbXmlQryInfo.Controls.Add(this.lbLogin);
            this.gbXmlQryInfo.Controls.Add(this.tbXmlQry);
            this.gbXmlQryInfo.Location = new System.Drawing.Point(12, 150);
            this.gbXmlQryInfo.Name = "gbXmlQryInfo";
            this.gbXmlQryInfo.Size = new System.Drawing.Size(929, 276);
            this.gbXmlQryInfo.TabIndex = 1;
            this.gbXmlQryInfo.TabStop = false;
            this.gbXmlQryInfo.Text = "Запрос (XML)";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(311, 26);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cbIsSubDivisionOO
            // 
            this.cbIsSubDivisionOO.Location = new System.Drawing.Point(324, 22);
            this.cbIsSubDivisionOO.Name = "cbIsSubDivisionOO";
            this.cbIsSubDivisionOO.Size = new System.Drawing.Size(62, 19);
            this.cbIsSubDivisionOO.TabIndex = 16;
            this.cbIsSubDivisionOO.Text = "Филиал:";
            this.cbIsSubDivisionOO.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbOOId
            // 
            this.tbOOId.Location = new System.Drawing.Point(392, 18);
            this.tbOOId.Name = "tbOOId";
            this.tbOOId.Size = new System.Drawing.Size(100, 22);
            this.tbOOId.TabIndex = 15;
            this.tbOOId.Text = "587";
            // 
            // tb_Count
            // 
            this.tb_Count.Location = new System.Drawing.Point(682, 20);
            this.tb_Count.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.tb_Count.Name = "tb_Count";
            this.tb_Count.Size = new System.Drawing.Size(91, 22);
            this.tb_Count.TabIndex = 11;
            this.tb_Count.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnMakeMulty
            // 
            this.btnMakeMulty.Location = new System.Drawing.Point(781, 20);
            this.btnMakeMulty.Name = "btnMakeMulty";
            this.btnMakeMulty.Size = new System.Drawing.Size(109, 23);
            this.btnMakeMulty.TabIndex = 10;
            this.btnMakeMulty.Text = "Размножить";
            this.btnMakeMulty.UseVisualStyleBackColor = true;
            this.btnMakeMulty.Click += new System.EventHandler(this.button3_Click);
            // 
            // lbCount
            // 
            this.lbCount.Location = new System.Drawing.Point(615, 22);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(54, 19);
            this.lbCount.TabIndex = 9;
            this.lbCount.Text = "Кол-во:";
            this.lbCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSetQryParams
            // 
            this.btnSetQryParams.Location = new System.Drawing.Point(500, 20);
            this.btnSetQryParams.Name = "btnSetQryParams";
            this.btnSetQryParams.Size = new System.Drawing.Size(109, 23);
            this.btnSetQryParams.TabIndex = 7;
            this.btnSetQryParams.Text = "Установить";
            this.btnSetQryParams.UseVisualStyleBackColor = true;
            this.btnSetQryParams.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GVUZ.ImportServiceTest.Properties.Settings.Default, "UserPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbPassword.Location = new System.Drawing.Point(213, 22);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(92, 22);
            this.tbPassword.TabIndex = 6;
            this.tbPassword.Text = global::GVUZ.ImportServiceTest.Properties.Settings.Default.UserPassword;
            // 
            // lbPassword
            // 
            this.lbPassword.Location = new System.Drawing.Point(154, 22);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(59, 19);
            this.lbPassword.TabIndex = 5;
            this.lbPassword.Text = "Пароль:";
            this.lbPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbLogin
            // 
            this.tbLogin.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GVUZ.ImportServiceTest.Properties.Settings.Default, "UserLogin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbLogin.Location = new System.Drawing.Point(57, 21);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(98, 22);
            this.tbLogin.TabIndex = 4;
            this.tbLogin.Text = global::GVUZ.ImportServiceTest.Properties.Settings.Default.UserLogin;
            // 
            // lbLogin
            // 
            this.lbLogin.Location = new System.Drawing.Point(-99, 22);
            this.lbLogin.Name = "lbLogin";
            this.lbLogin.Size = new System.Drawing.Size(152, 19);
            this.lbLogin.TabIndex = 3;
            this.lbLogin.Text = "Логин:";
            this.lbLogin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbXmlQry
            // 
            this.tbXmlQry.AcceptsTab = true;
            this.tbXmlQry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbXmlQry.Location = new System.Drawing.Point(6, 50);
            this.tbXmlQry.Name = "tbXmlQry";
            this.tbXmlQry.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.tbXmlQry.Size = new System.Drawing.Size(917, 220);
            this.tbXmlQry.TabIndex = 0;
            this.tbXmlQry.Text = "";
            this.tbXmlQry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox3_KeyDown);
            // 
            // btnCallService
            // 
            this.btnCallService.Enabled = false;
            this.btnCallService.Location = new System.Drawing.Point(195, 432);
            this.btnCallService.Name = "btnCallService";
            this.btnCallService.Size = new System.Drawing.Size(124, 23);
            this.btnCallService.TabIndex = 2;
            this.btnCallService.Text = "Вызвать сервис";
            this.btnCallService.UseVisualStyleBackColor = true;
            this.btnCallService.Click += new System.EventHandler(this.button1_Click);
            // 
            // gbServiceXmlResponse
            // 
            this.gbServiceXmlResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbServiceXmlResponse.Controls.Add(this.tbServiceResponse);
            this.gbServiceXmlResponse.Location = new System.Drawing.Point(12, 461);
            this.gbServiceXmlResponse.Name = "gbServiceXmlResponse";
            this.gbServiceXmlResponse.Size = new System.Drawing.Size(929, 176);
            this.gbServiceXmlResponse.TabIndex = 3;
            this.gbServiceXmlResponse.TabStop = false;
            this.gbServiceXmlResponse.Text = "Ответ сервиса (XML)";
            // 
            // tbServiceResponse
            // 
            this.tbServiceResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServiceResponse.Location = new System.Drawing.Point(6, 21);
            this.tbServiceResponse.Name = "tbServiceResponse";
            this.tbServiceResponse.ReadOnly = true;
            this.tbServiceResponse.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.tbServiceResponse.Size = new System.Drawing.Size(917, 149);
            this.tbServiceResponse.TabIndex = 1;
            this.tbServiceResponse.Text = "";
            this.tbServiceResponse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox3_KeyDown);
            // 
            // menuStripOps
            // 
            this.menuStripOps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmenuOperations});
            this.menuStripOps.Location = new System.Drawing.Point(0, 0);
            this.menuStripOps.Name = "menuStripOps";
            this.menuStripOps.Size = new System.Drawing.Size(953, 24);
            this.menuStripOps.TabIndex = 4;
            this.menuStripOps.Text = "menuStrip1";
            // 
            // tmenuOperations
            // 
            this.tmenuOperations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.извлечениеПакетовИзБДToolStripMenuItem,
            this.загрузкаПакетовВБДToolStripMenuItem,
            this.импортПакетовToolStripMenuItem});
            this.tmenuOperations.Name = "tmenuOperations";
            this.tmenuOperations.Size = new System.Drawing.Size(75, 20);
            this.tmenuOperations.Text = "Операции";
            // 
            // извлечениеПакетовИзБДToolStripMenuItem
            // 
            this.извлечениеПакетовИзБДToolStripMenuItem.Name = "извлечениеПакетовИзБДToolStripMenuItem";
            this.извлечениеПакетовИзБДToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.извлечениеПакетовИзБДToolStripMenuItem.Text = "Извлечение пакетов из БД";
            this.извлечениеПакетовИзБДToolStripMenuItem.Click += new System.EventHandler(this.извлечениеПакетовИзБДToolStripMenuItem_Click);
            // 
            // загрузкаПакетовВБДToolStripMenuItem
            // 
            this.загрузкаПакетовВБДToolStripMenuItem.Name = "загрузкаПакетовВБДToolStripMenuItem";
            this.загрузкаПакетовВБДToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.загрузкаПакетовВБДToolStripMenuItem.Text = "Отправка пакетов";
            this.загрузкаПакетовВБДToolStripMenuItem.Click += new System.EventHandler(this.загрузкаПакетовВБДToolStripMenuItem_Click);
            // 
            // импортПакетовToolStripMenuItem
            // 
            this.импортПакетовToolStripMenuItem.Name = "импортПакетовToolStripMenuItem";
            this.импортПакетовToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.импортПакетовToolStripMenuItem.Text = "Импорт пакетов";
            // 
            // bLoadXml
            // 
            this.bLoadXml.Location = new System.Drawing.Point(21, 431);
            this.bLoadXml.Name = "bLoadXml";
            this.bLoadXml.Size = new System.Drawing.Size(168, 23);
            this.bLoadXml.TabIndex = 5;
            this.bLoadXml.Text = "Загрузить xml (пакет)";
            this.bLoadXml.UseVisualStyleBackColor = true;
            this.bLoadXml.Click += new System.EventHandler(this.bLoadXml_Click);
            // 
            // openPackageDialog
            // 
            this.openPackageDialog.DefaultExt = "*.xml";
            this.openPackageDialog.Filter = "Пакеты загрузки (*.xml)|*.xml";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 649);
            this.Controls.Add(this.bLoadXml);
            this.Controls.Add(this.gbServiceXmlResponse);
            this.Controls.Add(this.btnCallService);
            this.Controls.Add(this.gbXmlQryInfo);
            this.Controls.Add(this.gbBaseAddrInfo);
            this.Controls.Add(this.menuStripOps);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStripOps;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbBaseAddrInfo.ResumeLayout(false);
            this.gbBaseAddrInfo.PerformLayout();
            this.gbXmlQryInfo.ResumeLayout(false);
            this.gbXmlQryInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Count)).EndInit();
            this.gbServiceXmlResponse.ResumeLayout(false);
            this.menuStripOps.ResumeLayout(false);
            this.menuStripOps.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox gbBaseAddrInfo;
		private System.Windows.Forms.TextBox tbResultPath;
		private System.Windows.Forms.Label lbResPath;
		private System.Windows.Forms.Label lbMethod;
		private System.Windows.Forms.ComboBox cmbxMethodsList;
		private System.Windows.Forms.Label lbBaseAddress;
		private System.Windows.Forms.TextBox tbBaseAddress;
		private System.Windows.Forms.GroupBox gbXmlQryInfo;
		private System.Windows.Forms.RichTextBox tbXmlQry;
		private System.Windows.Forms.Button btnCallService;
		private System.Windows.Forms.GroupBox gbServiceXmlResponse;
		private System.Windows.Forms.RichTextBox tbServiceResponse;
		private System.Windows.Forms.Button btnSetQryParams;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lbPassword;
		private System.Windows.Forms.TextBox tbLogin;
		private System.Windows.Forms.Label lbLogin;
        private Button btnMakeMulty;
        private Label lbCount;
        private NumericUpDown tb_Count;
        private CheckBox checkBox1;
        private Label cbIsSubDivisionOO;
        private TextBox tbOOId;
        private MenuStrip menuStripOps;
        private ToolStripMenuItem tmenuOperations;
        private ToolStripMenuItem извлечениеПакетовИзБДToolStripMenuItem;
        private ToolStripMenuItem загрузкаПакетовВБДToolStripMenuItem;
        private Button bLoadXml;
        private OpenFileDialog openPackageDialog;
        private ToolStripMenuItem импортПакетовToolStripMenuItem;
	}
}

