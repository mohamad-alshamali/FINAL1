namespace FINAL1
{
    partial class MedicalRecordsForm
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
            this.cmbPatients = new System.Windows.Forms.ComboBox();
            this.cmbDoctors = new System.Windows.Forms.ComboBox();
            this.txtDiagnosis = new System.Windows.Forms.TextBox();
            this.txtPrescription = new System.Windows.Forms.TextBox();
            this.btnSaveRecord = new System.Windows.Forms.Button();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPatients
            // 
            this.cmbPatients.FormattingEnabled = true;
            this.cmbPatients.Location = new System.Drawing.Point(600, 29);
            this.cmbPatients.Name = "cmbPatients";
            this.cmbPatients.Size = new System.Drawing.Size(100, 21);
            this.cmbPatients.TabIndex = 0;
            this.cmbPatients.Text = "المريض";
            // 
            // cmbDoctors
            // 
            this.cmbDoctors.FormattingEnabled = true;
            this.cmbDoctors.Location = new System.Drawing.Point(600, 111);
            this.cmbDoctors.Name = "cmbDoctors";
            this.cmbDoctors.Size = new System.Drawing.Size(100, 21);
            this.cmbDoctors.TabIndex = 1;
            this.cmbDoctors.Text = "االطبيب";
            // 
            // txtDiagnosis
            // 
            this.txtDiagnosis.Location = new System.Drawing.Point(600, 205);
            this.txtDiagnosis.Multiline = true;
            this.txtDiagnosis.Name = "txtDiagnosis";
            this.txtDiagnosis.Size = new System.Drawing.Size(100, 20);
            this.txtDiagnosis.TabIndex = 2;
            this.txtDiagnosis.Text = "التشخيص";
            // 
            // txtPrescription
            // 
            this.txtPrescription.Location = new System.Drawing.Point(600, 284);
            this.txtPrescription.Multiline = true;
            this.txtPrescription.Name = "txtPrescription";
            this.txtPrescription.Size = new System.Drawing.Size(100, 20);
            this.txtPrescription.TabIndex = 3;
            this.txtPrescription.Text = "الوصفة الطبية";
            // 
            // btnSaveRecord
            // 
            this.btnSaveRecord.Location = new System.Drawing.Point(600, 401);
            this.btnSaveRecord.Name = "btnSaveRecord";
            this.btnSaveRecord.Size = new System.Drawing.Size(100, 23);
            this.btnSaveRecord.TabIndex = 4;
            this.btnSaveRecord.Text = "حفظ السجل الطبي";
            this.btnSaveRecord.UseVisualStyleBackColor = true;
            this.btnSaveRecord.Click += new System.EventHandler(this.btnSaveRecord_Click);
            // 
            // dgvRecords
            // 
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Location = new System.Drawing.Point(24, 29);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.Size = new System.Drawing.Size(421, 395);
            this.dgvRecords.TabIndex = 5;
            // 
            // MedicalRecordsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 450);
            this.Controls.Add(this.dgvRecords);
            this.Controls.Add(this.btnSaveRecord);
            this.Controls.Add(this.txtPrescription);
            this.Controls.Add(this.txtDiagnosis);
            this.Controls.Add(this.cmbDoctors);
            this.Controls.Add(this.cmbPatients);
            this.Name = "MedicalRecordsForm";
            this.Text = "MedicalRecordsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPatients;
        private System.Windows.Forms.ComboBox cmbDoctors;
        private System.Windows.Forms.TextBox txtDiagnosis;
        private System.Windows.Forms.TextBox txtPrescription;
        private System.Windows.Forms.Button btnSaveRecord;
        private System.Windows.Forms.DataGridView dgvRecords;
    }
}