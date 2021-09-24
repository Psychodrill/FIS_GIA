namespace FBS.ComposisionFix
{
    partial class MainFrm
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
            this.btnGetCompPaths = new System.Windows.Forms.Button();
            this.btnfixCompPaths = new System.Windows.Forms.Button();
            this.tbOOid = new System.Windows.Forms.TextBox();
            this.dgvMissingCompPaths = new System.Windows.Forms.DataGridView();
            this.lbOOid = new System.Windows.Forms.Label();
            this.rtbCompFilesData = new System.Windows.Forms.RichTextBox();
            this.rtbEnryptedPaths = new System.Windows.Forms.RichTextBox();
            this.dgvErbdCompInfo = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.tbProjName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbProjBatchId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbExamDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPageCount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMissingCompPaths)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErbdCompInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGetCompPaths
            // 
            this.btnGetCompPaths.Location = new System.Drawing.Point(303, 617);
            this.btnGetCompPaths.Name = "btnGetCompPaths";
            this.btnGetCompPaths.Size = new System.Drawing.Size(289, 56);
            this.btnGetCompPaths.TabIndex = 0;
            this.btnGetCompPaths.Text = "Получить пути сочинений";
            this.btnGetCompPaths.UseVisualStyleBackColor = true;
            this.btnGetCompPaths.Click += new System.EventHandler(this.btnGetCompPaths_Click);
            // 
            // btnfixCompPaths
            // 
            this.btnfixCompPaths.Location = new System.Drawing.Point(609, 617);
            this.btnfixCompPaths.Name = "btnfixCompPaths";
            this.btnfixCompPaths.Size = new System.Drawing.Size(268, 56);
            this.btnfixCompPaths.TabIndex = 1;
            this.btnfixCompPaths.Text = "Исправить нулевые пути";
            this.btnfixCompPaths.UseVisualStyleBackColor = true;
            this.btnfixCompPaths.Click += new System.EventHandler(this.btnfixCompPaths_Click);
            // 
            // tbOOid
            // 
            this.tbOOid.Location = new System.Drawing.Point(237, 12);
            this.tbOOid.Name = "tbOOid";
            this.tbOOid.Size = new System.Drawing.Size(122, 20);
            this.tbOOid.TabIndex = 2;
            this.tbOOid.TextChanged += new System.EventHandler(this.tbOOid_TextChanged);
            // 
            // dgvMissingCompPaths
            // 
            this.dgvMissingCompPaths.AllowUserToAddRows = false;
            this.dgvMissingCompPaths.AllowUserToDeleteRows = false;
            this.dgvMissingCompPaths.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMissingCompPaths.Location = new System.Drawing.Point(12, 49);
            this.dgvMissingCompPaths.Name = "dgvMissingCompPaths";
            this.dgvMissingCompPaths.ReadOnly = true;
            this.dgvMissingCompPaths.Size = new System.Drawing.Size(1212, 105);
            this.dgvMissingCompPaths.TabIndex = 3;
            // 
            // lbOOid
            // 
            this.lbOOid.AutoSize = true;
            this.lbOOid.Location = new System.Drawing.Point(13, 18);
            this.lbOOid.Name = "lbOOid";
            this.lbOOid.Size = new System.Drawing.Size(180, 13);
            this.lbOOid.TabIndex = 4;
            this.lbOOid.Text = "ID Образовательной организации";
            // 
            // rtbCompFilesData
            // 
            this.rtbCompFilesData.Location = new System.Drawing.Point(12, 261);
            this.rtbCompFilesData.Name = "rtbCompFilesData";
            this.rtbCompFilesData.Size = new System.Drawing.Size(509, 111);
            this.rtbCompFilesData.TabIndex = 5;
            this.rtbCompFilesData.Text = "";
            // 
            // rtbEnryptedPaths
            // 
            this.rtbEnryptedPaths.Location = new System.Drawing.Point(536, 261);
            this.rtbEnryptedPaths.Name = "rtbEnryptedPaths";
            this.rtbEnryptedPaths.Size = new System.Drawing.Size(688, 111);
            this.rtbEnryptedPaths.TabIndex = 6;
            this.rtbEnryptedPaths.Text = "";
            // 
            // dgvErbdCompInfo
            // 
            this.dgvErbdCompInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErbdCompInfo.Location = new System.Drawing.Point(12, 160);
            this.dgvErbdCompInfo.Name = "dgvErbdCompInfo";
            this.dgvErbdCompInfo.Size = new System.Drawing.Size(1212, 95);
            this.dgvErbdCompInfo.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(983, 617);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbProjName
            // 
            this.tbProjName.Location = new System.Drawing.Point(117, 459);
            this.tbProjName.Name = "tbProjName";
            this.tbProjName.Size = new System.Drawing.Size(242, 20);
            this.tbProjName.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 462);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "ProjName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 498);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "projBatchId";
            // 
            // tbProjBatchId
            // 
            this.tbProjBatchId.Location = new System.Drawing.Point(117, 491);
            this.tbProjBatchId.Name = "tbProjBatchId";
            this.tbProjBatchId.Size = new System.Drawing.Size(242, 20);
            this.tbProjBatchId.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 530);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "barcode";
            // 
            // tbBarcode
            // 
            this.tbBarcode.Location = new System.Drawing.Point(117, 527);
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Size = new System.Drawing.Size(242, 20);
            this.tbBarcode.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(419, 462);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "exam date";
            // 
            // tbExamDate
            // 
            this.tbExamDate.Location = new System.Drawing.Point(481, 459);
            this.tbExamDate.Name = "tbExamDate";
            this.tbExamDate.Size = new System.Drawing.Size(232, 20);
            this.tbExamDate.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(414, 498);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "page count";
            // 
            // tbPageCount
            // 
            this.tbPageCount.Location = new System.Drawing.Point(481, 495);
            this.tbPageCount.Name = "tbPageCount";
            this.tbPageCount.Size = new System.Drawing.Size(232, 20);
            this.tbPageCount.TabIndex = 18;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 687);
            this.Controls.Add(this.tbPageCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbExamDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbBarcode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbProjBatchId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbProjName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgvErbdCompInfo);
            this.Controls.Add(this.rtbEnryptedPaths);
            this.Controls.Add(this.rtbCompFilesData);
            this.Controls.Add(this.lbOOid);
            this.Controls.Add(this.dgvMissingCompPaths);
            this.Controls.Add(this.tbOOid);
            this.Controls.Add(this.btnfixCompPaths);
            this.Controls.Add(this.btnGetCompPaths);
            this.Name = "MainFrm";
            this.Text = "Исправление путей сочинений";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMissingCompPaths)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErbdCompInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetCompPaths;
        private System.Windows.Forms.Button btnfixCompPaths;
        private System.Windows.Forms.TextBox tbOOid;
        private System.Windows.Forms.DataGridView dgvMissingCompPaths;
        private System.Windows.Forms.Label lbOOid;
        private System.Windows.Forms.RichTextBox rtbCompFilesData;
        private System.Windows.Forms.RichTextBox rtbEnryptedPaths;
        private System.Windows.Forms.DataGridView dgvErbdCompInfo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbProjName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbProjBatchId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbExamDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPageCount;
    }
}

