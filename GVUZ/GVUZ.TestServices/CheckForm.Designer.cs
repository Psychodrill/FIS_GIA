namespace GVUZ.TestServices
{
	partial class CheckForm
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.egeAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnTestEgeRaw = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.egeResult = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.egePinCode = new System.Windows.Forms.TextBox();
			this.btnTestEge = new System.Windows.Forms.Button();
			this.useDevTests = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.egeAddress);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(701, 46);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Выбор сервиса";
			// 
			// egeAddress
			// 
			this.egeAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.egeAddress.Location = new System.Drawing.Point(54, 16);
			this.egeAddress.Name = "egeAddress";
			this.egeAddress.Size = new System.Drawing.Size(641, 20);
			this.egeAddress.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Адрес:";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(721, 448);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnTestEgeRaw);
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Controls.Add(this.btnTestEge);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(713, 422);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Проверка ЕГЭ";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// btnTestEgeRaw
			// 
			this.btnTestEgeRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestEgeRaw.AutoSize = true;
			this.btnTestEgeRaw.Location = new System.Drawing.Point(423, 393);
			this.btnTestEgeRaw.Name = "btnTestEgeRaw";
			this.btnTestEgeRaw.Size = new System.Drawing.Size(278, 23);
			this.btnTestEgeRaw.TabIndex = 3;
			this.btnTestEgeRaw.Text = "Получить результат напрямую (SOAP, pin = 962316)";
			this.btnTestEgeRaw.UseVisualStyleBackColor = true;
			this.btnTestEgeRaw.Click += new System.EventHandler(this.btnTestEgeRaw_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.egeResult);
			this.groupBox3.Location = new System.Drawing.Point(7, 136);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(700, 251);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Результат";
			// 
			// egeResult
			// 
			this.egeResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.egeResult.Location = new System.Drawing.Point(7, 20);
			this.egeResult.Multiline = true;
			this.egeResult.Name = "egeResult";
			this.egeResult.Size = new System.Drawing.Size(687, 225);
			this.egeResult.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.useDevTests);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.egePinCode);
			this.groupBox2.Location = new System.Drawing.Point(7, 59);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(700, 71);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Параметры";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Проверочный код:";
			// 
			// egePinCode
			// 
			this.egePinCode.Location = new System.Drawing.Point(111, 16);
			this.egePinCode.Name = "egePinCode";
			this.egePinCode.Size = new System.Drawing.Size(122, 20);
			this.egePinCode.TabIndex = 0;
			// 
			// btnTestEge
			// 
			this.btnTestEge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestEge.AutoSize = true;
			this.btnTestEge.Location = new System.Drawing.Point(137, 393);
			this.btnTestEge.Name = "btnTestEge";
			this.btnTestEge.Size = new System.Drawing.Size(280, 23);
			this.btnTestEge.TabIndex = 1;
			this.btnTestEge.Text = "Получить результат через прокси (Serialized to XML)";
			this.btnTestEge.UseVisualStyleBackColor = true;
			this.btnTestEge.Click += new System.EventHandler(this.btnTestEge_Click);
			// 
			// useDevTests
			// 
			this.useDevTests.AutoSize = true;
			this.useDevTests.Location = new System.Drawing.Point(416, 16);
			this.useDevTests.Name = "useDevTests";
			this.useDevTests.Size = new System.Drawing.Size(152, 17);
			this.useDevTests.TabIndex = 2;
			this.useDevTests.Text = "Альтернативные данные";
			this.useDevTests.UseVisualStyleBackColor = true;
			// 
			// CheckForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(745, 472);
			this.Controls.Add(this.tabControl1);
			this.Name = "CheckForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Тестирование сервисов";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox egeAddress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button btnTestEge;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox egePinCode;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox egeResult;
		private System.Windows.Forms.Button btnTestEgeRaw;
		private System.Windows.Forms.CheckBox useDevTests;
	}
}

