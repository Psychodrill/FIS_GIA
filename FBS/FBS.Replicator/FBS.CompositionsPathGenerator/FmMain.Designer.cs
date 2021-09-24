using System.Windows.Forms;

namespace FBS.CompositionsPathGenerator
{
    partial class FmMain : Form
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
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.tbProjectBatchId = new System.Windows.Forms.TextBox();
            this.tbProjectName = new System.Windows.Forms.TextBox();
            this.cbPagesCount = new System.Windows.Forms.ComboBox();
            this.dtpExamDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btGenerate2016 = new System.Windows.Forms.Button();
            this.tbParticipantId = new System.Windows.Forms.TextBox();
            this.tbDocument = new System.Windows.Forms.TextBox();
            this.tbSurname = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbSecondName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btGenerate2015 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbBarcode
            // 
            this.tbBarcode.Location = new System.Drawing.Point(106, 12);
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Size = new System.Drawing.Size(214, 20);
            this.tbBarcode.TabIndex = 0;
            // 
            // tbProjectBatchId
            // 
            this.tbProjectBatchId.Location = new System.Drawing.Point(106, 38);
            this.tbProjectBatchId.Name = "tbProjectBatchId";
            this.tbProjectBatchId.Size = new System.Drawing.Size(70, 20);
            this.tbProjectBatchId.TabIndex = 1;
            // 
            // tbProjectName
            // 
            this.tbProjectName.Location = new System.Drawing.Point(106, 64);
            this.tbProjectName.Name = "tbProjectName";
            this.tbProjectName.Size = new System.Drawing.Size(214, 20);
            this.tbProjectName.TabIndex = 2;
            // 
            // cbPagesCount
            // 
            this.cbPagesCount.FormattingEnabled = true;
            this.cbPagesCount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.cbPagesCount.Location = new System.Drawing.Point(365, 152);
            this.cbPagesCount.Name = "cbPagesCount";
            this.cbPagesCount.Size = new System.Drawing.Size(70, 21);
            this.cbPagesCount.TabIndex = 3;
            // 
            // dtpExamDate
            // 
            this.dtpExamDate.Location = new System.Drawing.Point(106, 89);
            this.dtpExamDate.Name = "dtpExamDate";
            this.dtpExamDate.Size = new System.Drawing.Size(214, 20);
            this.dtpExamDate.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Barcode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "ProjectBatchId";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "ProjectName";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Дата сочинения";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(276, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Число страниц";
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Location = new System.Drawing.Point(12, 237);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.Size = new System.Drawing.Size(710, 187);
            this.tbResult.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Путь к файлам";
            // 
            // btGenerate2016
            // 
            this.btGenerate2016.Location = new System.Drawing.Point(12, 179);
            this.btGenerate2016.Name = "btGenerate2016";
            this.btGenerate2016.Size = new System.Drawing.Size(137, 28);
            this.btGenerate2016.TabIndex = 12;
            this.btGenerate2016.Text = "Сформировать за 2016";
            this.btGenerate2016.UseVisualStyleBackColor = true;
            this.btGenerate2016.Click += new System.EventHandler(this.btGenerate2016_Click);
            // 
            // tbParticipantId
            // 
            this.tbParticipantId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbParticipantId.Location = new System.Drawing.Point(508, 15);
            this.tbParticipantId.Name = "tbParticipantId";
            this.tbParticipantId.Size = new System.Drawing.Size(214, 20);
            this.tbParticipantId.TabIndex = 13;
            // 
            // tbDocument
            // 
            this.tbDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDocument.Location = new System.Drawing.Point(508, 41);
            this.tbDocument.Name = "tbDocument";
            this.tbDocument.Size = new System.Drawing.Size(214, 20);
            this.tbDocument.TabIndex = 14;
            // 
            // tbSurname
            // 
            this.tbSurname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSurname.Location = new System.Drawing.Point(508, 67);
            this.tbSurname.Name = "tbSurname";
            this.tbSurname.Size = new System.Drawing.Size(214, 20);
            this.tbSurname.TabIndex = 15;
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(508, 93);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(214, 20);
            this.tbName.TabIndex = 16;
            // 
            // tbSecondName
            // 
            this.tbSecondName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSecondName.Location = new System.Drawing.Point(508, 119);
            this.tbSecondName.Name = "tbSecondName";
            this.tbSecondName.Size = new System.Drawing.Size(214, 20);
            this.tbSecondName.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(436, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "ParticipantId";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(448, 122);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Отчество";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(473, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Имя";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(446, 71);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Фамилия";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(444, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Документ";
            // 
            // btGenerate2015
            // 
            this.btGenerate2015.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btGenerate2015.Location = new System.Drawing.Point(585, 179);
            this.btGenerate2015.Name = "btGenerate2015";
            this.btGenerate2015.Size = new System.Drawing.Size(137, 28);
            this.btGenerate2015.TabIndex = 23;
            this.btGenerate2015.Text = "Сформировать за 2015";
            this.btGenerate2015.UseVisualStyleBackColor = true;
            this.btGenerate2015.Click += new System.EventHandler(this.btGenerate2015_Click);
            // 
            // FmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 436);
            this.Controls.Add(this.btGenerate2015);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbSecondName);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbSurname);
            this.Controls.Add(this.tbDocument);
            this.Controls.Add(this.tbParticipantId);
            this.Controls.Add(this.btGenerate2016);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpExamDate);
            this.Controls.Add(this.cbPagesCount);
            this.Controls.Add(this.tbProjectName);
            this.Controls.Add(this.tbProjectBatchId);
            this.Controls.Add(this.tbBarcode);
            this.Name = "FmMain";
            this.Text = "Генератор путей к бланкам сочинений";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.TextBox tbProjectBatchId;
        private System.Windows.Forms.TextBox tbProjectName;
        private System.Windows.Forms.ComboBox cbPagesCount;
        private System.Windows.Forms.DateTimePicker dtpExamDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btGenerate2016;
        private System.Windows.Forms.TextBox tbParticipantId;
        private System.Windows.Forms.TextBox tbDocument;
        private System.Windows.Forms.TextBox tbSurname;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbSecondName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btGenerate2015;
    }
}

