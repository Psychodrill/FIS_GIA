namespace GVUZ.Util.UI.Parsing
{
    partial class ParseSettingsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbConn = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbInstFilter = new System.Windows.Forms.CheckBox();
            this.cbTruncate = new System.Windows.Forms.CheckBox();
            this.tbInstId = new System.Windows.Forms.TextBox();
            this.dtpMinDate = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 241);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(506, 44);
            this.panel1.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(419, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Закрыть";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(338, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tbConn);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox1.Size = new System.Drawing.Size(495, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Соединение с базой";
            // 
            // tbConn
            // 
            this.tbConn.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GVUZ.Util.Properties.Settings.Default, "parseConnectionString", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbConn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbConn.Location = new System.Drawing.Point(8, 21);
            this.tbConn.Multiline = true;
            this.tbConn.Name = "tbConn";
            this.tbConn.Size = new System.Drawing.Size(479, 41);
            this.tbConn.TabIndex = 0;
            this.tbConn.Text = global::GVUZ.Util.Properties.Settings.Default.parseConnectionString;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.cbInstFilter);
            this.groupBox2.Controls.Add(this.cbTruncate);
            this.groupBox2.Controls.Add(this.tbInstId);
            this.groupBox2.Controls.Add(this.dtpMinDate);
            this.groupBox2.Location = new System.Drawing.Point(5, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox2.Size = new System.Drawing.Size(495, 147);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Фильтрация пакетов";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = global::GVUZ.Util.Properties.Settings.Default.parseMinDateChecked;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::GVUZ.Util.Properties.Settings.Default, "parseMinDateChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Location = new System.Drawing.Point(15, 24);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "минимальная дата:";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cbInstFilter
            // 
            this.cbInstFilter.AutoSize = true;
            this.cbInstFilter.Checked = global::GVUZ.Util.Properties.Settings.Default.parseFilterInstitution;
            this.cbInstFilter.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::GVUZ.Util.Properties.Settings.Default, "parseFilterInstitution", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbInstFilter.Location = new System.Drawing.Point(15, 47);
            this.cbInstFilter.Name = "cbInstFilter";
            this.cbInstFilter.Size = new System.Drawing.Size(210, 17);
            this.cbInstFilter.TabIndex = 3;
            this.cbInstFilter.Text = "фильтр по идентификаторам ВУЗов";
            this.cbInstFilter.UseVisualStyleBackColor = true;
            // 
            // cbTruncate
            // 
            this.cbTruncate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTruncate.AutoSize = true;
            this.cbTruncate.Checked = global::GVUZ.Util.Properties.Settings.Default.parseTruncateTable;
            this.cbTruncate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::GVUZ.Util.Properties.Settings.Default, "parseTruncateTable", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbTruncate.Location = new System.Drawing.Point(264, 24);
            this.cbTruncate.Name = "cbTruncate";
            this.cbTruncate.Size = new System.Drawing.Size(223, 17);
            this.cbTruncate.TabIndex = 2;
            this.cbTruncate.Text = "полностью очищать таблицу-приемник";
            this.cbTruncate.UseVisualStyleBackColor = true;
            // 
            // tbInstId
            // 
            this.tbInstId.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInstId.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GVUZ.Util.Properties.Settings.Default, "parseInstitutionId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbInstId.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::GVUZ.Util.Properties.Settings.Default, "parseFilterInstitution", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbInstId.Enabled = global::GVUZ.Util.Properties.Settings.Default.parseFilterInstitution;
            this.tbInstId.Location = new System.Drawing.Point(8, 70);
            this.tbInstId.Multiline = true;
            this.tbInstId.Name = "tbInstId";
            this.tbInstId.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbInstId.Size = new System.Drawing.Size(479, 66);
            this.tbInstId.TabIndex = 4;
            this.tbInstId.Text = global::GVUZ.Util.Properties.Settings.Default.parseInstitutionId;
            // 
            // dtpMinDate
            // 
            this.dtpMinDate.CustomFormat = "dd.MM.yyyy";
            this.dtpMinDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::GVUZ.Util.Properties.Settings.Default, "parseMinDate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dtpMinDate.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::GVUZ.Util.Properties.Settings.Default, "parseMinDateChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dtpMinDate.Enabled = global::GVUZ.Util.Properties.Settings.Default.parseMinDateChecked;
            this.dtpMinDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMinDate.Location = new System.Drawing.Point(146, 21);
            this.dtpMinDate.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpMinDate.Name = "dtpMinDate";
            this.dtpMinDate.Size = new System.Drawing.Size(105, 20);
            this.dtpMinDate.TabIndex = 1;
            this.dtpMinDate.Value = global::GVUZ.Util.Properties.Settings.Default.parseMinDate;
            // 
            // ParseSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(506, 285);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "ParseSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки разбора приказов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ParseSettingsForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbConn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbTruncate;
        private System.Windows.Forms.TextBox tbInstId;
        private System.Windows.Forms.DateTimePicker dtpMinDate;
        private System.Windows.Forms.CheckBox cbInstFilter;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}