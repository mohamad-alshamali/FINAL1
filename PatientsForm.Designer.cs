namespace FINAL1
{
    partial class PatientsForm
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
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtBloodType = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.dtpBirthDate = new System.Windows.Forms.DateTimePicker();
            this.btnSavePatient = new System.Windows.Forms.Button();
            this.dgvPatients = new System.Windows.Forms.DataGridView();
            this.btnUpdatePatient = new System.Windows.Forms.Button();
            this.btnDeletePatient = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatients)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(591, 65);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(121, 20);
            this.txtFullName.TabIndex = 0;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(591, 141);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(121, 20);
            this.txtPhone.TabIndex = 1;
            // 
            // txtBloodType
            // 
            this.txtBloodType.Location = new System.Drawing.Point(591, 195);
            this.txtBloodType.Name = "txtBloodType";
            this.txtBloodType.Size = new System.Drawing.Size(121, 20);
            this.txtBloodType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(676, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "الاسم";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(650, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "رقم الموبايل";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(663, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "زمرة الدم";
            // 
            // cmbGender
            // 
            this.cmbGender.AutoCompleteCustomSource.AddRange(new string[] {
            "ذكر",
            "أنثى"});
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            "ذكر",
            "انثى"});
            this.cmbGender.Location = new System.Drawing.Point(591, 254);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(121, 21);
            this.cmbGender.TabIndex = 6;
            // 
            // dtpBirthDate
            // 
            this.dtpBirthDate.Location = new System.Drawing.Point(591, 307);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.Size = new System.Drawing.Size(121, 20);
            this.dtpBirthDate.TabIndex = 7;
            // 
            // btnSavePatient
            // 
            this.btnSavePatient.Location = new System.Drawing.Point(626, 360);
            this.btnSavePatient.Name = "btnSavePatient";
            this.btnSavePatient.Size = new System.Drawing.Size(86, 23);
            this.btnSavePatient.TabIndex = 8;
            this.btnSavePatient.Text = "اضافة";
            this.btnSavePatient.UseVisualStyleBackColor = true;
            this.btnSavePatient.Click += new System.EventHandler(this.btnSavePatient_Click);
            // 
            // dgvPatients
            // 
            this.dgvPatients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPatients.Location = new System.Drawing.Point(12, 25);
            this.dgvPatients.Name = "dgvPatients";
            this.dgvPatients.Size = new System.Drawing.Size(472, 302);
            this.dgvPatients.TabIndex = 9;
            this.dgvPatients.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPatients_CellClick);
            // 
            // btnUpdatePatient
            // 
            this.btnUpdatePatient.Location = new System.Drawing.Point(516, 360);
            this.btnUpdatePatient.Name = "btnUpdatePatient";
            this.btnUpdatePatient.Size = new System.Drawing.Size(75, 23);
            this.btnUpdatePatient.TabIndex = 10;
            this.btnUpdatePatient.Text = "تعديل";
            this.btnUpdatePatient.UseVisualStyleBackColor = true;
            this.btnUpdatePatient.Click += new System.EventHandler(this.btnUpdatePatient_Click);
            // 
            // btnDeletePatient
            // 
            this.btnDeletePatient.Location = new System.Drawing.Point(409, 360);
            this.btnDeletePatient.Name = "btnDeletePatient";
            this.btnDeletePatient.Size = new System.Drawing.Size(75, 23);
            this.btnDeletePatient.TabIndex = 11;
            this.btnDeletePatient.Text = "حذف";
            this.btnDeletePatient.UseVisualStyleBackColor = true;
            this.btnDeletePatient.Click += new System.EventHandler(this.btnDeletePatient_Click);
            // 
            // PatientsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDeletePatient);
            this.Controls.Add(this.btnUpdatePatient);
            this.Controls.Add(this.dgvPatients);
            this.Controls.Add(this.btnSavePatient);
            this.Controls.Add(this.dtpBirthDate);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBloodType);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtFullName);
            this.Name = "PatientsForm";
            this.Text = "PatientsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtBloodType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;
        private System.Windows.Forms.Button btnSavePatient;
        private System.Windows.Forms.DataGridView dgvPatients;
        private System.Windows.Forms.Button btnUpdatePatient;
        private System.Windows.Forms.Button btnDeletePatient;
    }
}