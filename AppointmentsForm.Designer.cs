namespace FINAL1
{
    partial class AppointmentsForm
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
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.cmbPatients = new System.Windows.Forms.ComboBox();
            this.cmbDoctors = new System.Windows.Forms.ComboBox();
            this.dtpAppDate = new System.Windows.Forms.DateTimePicker();
            this.btnSaveAppointment = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointments.Location = new System.Drawing.Point(69, 12);
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.Size = new System.Drawing.Size(424, 391);
            this.dgvAppointments.TabIndex = 0;
            // 
            // cmbPatients
            // 
            this.cmbPatients.FormattingEnabled = true;
            this.cmbPatients.Location = new System.Drawing.Point(599, 141);
            this.cmbPatients.Name = "cmbPatients";
            this.cmbPatients.Size = new System.Drawing.Size(121, 21);
            this.cmbPatients.TabIndex = 1;
            // 
            // cmbDoctors
            // 
            this.cmbDoctors.FormattingEnabled = true;
            this.cmbDoctors.Location = new System.Drawing.Point(599, 246);
            this.cmbDoctors.Name = "cmbDoctors";
            this.cmbDoctors.Size = new System.Drawing.Size(121, 21);
            this.cmbDoctors.TabIndex = 2;
            // 
            // dtpAppDate
            // 
            this.dtpAppDate.Location = new System.Drawing.Point(599, 12);
            this.dtpAppDate.Name = "dtpAppDate";
            this.dtpAppDate.Size = new System.Drawing.Size(121, 20);
            this.dtpAppDate.TabIndex = 3;
            // 
            // btnSaveAppointment
            // 
            this.btnSaveAppointment.Location = new System.Drawing.Point(599, 380);
            this.btnSaveAppointment.Name = "btnSaveAppointment";
            this.btnSaveAppointment.Size = new System.Drawing.Size(121, 23);
            this.btnSaveAppointment.TabIndex = 4;
            this.btnSaveAppointment.Text = "حجز موعد";
            this.btnSaveAppointment.UseVisualStyleBackColor = true;
            this.btnSaveAppointment.Click += new System.EventHandler(this.btnSaveAppointment_Click_1);
            // 
            // AppointmentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSaveAppointment);
            this.Controls.Add(this.dtpAppDate);
            this.Controls.Add(this.cmbDoctors);
            this.Controls.Add(this.cmbPatients);
            this.Controls.Add(this.dgvAppointments);
            this.Name = "AppointmentsForm";
            this.Text = "AppointmentsForm";
            this.Load += new System.EventHandler(this.AppointmentsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.ComboBox cmbPatients;
        private System.Windows.Forms.ComboBox cmbDoctors;
        private System.Windows.Forms.DateTimePicker dtpAppDate;
        private System.Windows.Forms.Button btnSaveAppointment;
    }
}