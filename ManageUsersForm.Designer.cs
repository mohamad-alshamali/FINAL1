namespace FINAL1
{
    partial class ManageUsersForm
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
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnUpdateUser = new System.Windows.Forms.Button();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.cmbRoles = new System.Windows.Forms.ComboBox();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtEmployeePhone = new System.Windows.Forms.TextBox();
            this.txtFingerprintID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttoninfo = new System.Windows.Forms.Button();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.lblDoctorName = new System.Windows.Forms.Label();
            this.textBoxsp = new System.Windows.Forms.TextBox();
            this.textBoxclinic = new System.Windows.Forms.TextBox();
            this.textBoxfee = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnupdateclinic = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(651, 29);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(121, 20);
            this.txtUser.TabIndex = 0;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(651, 55);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(121, 20);
            this.txtPass.TabIndex = 1;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(666, 231);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(93, 17);
            this.chkActive.TabIndex = 2;
            this.chkActive.Text = "تفعيل الحساب";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // btnCreateUser
            // 
            this.btnCreateUser.Location = new System.Drawing.Point(651, 254);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Size = new System.Drawing.Size(100, 23);
            this.btnCreateUser.TabIndex = 3;
            this.btnCreateUser.Text = "انشاء حساب جديد";
            this.btnCreateUser.UseVisualStyleBackColor = true;
            this.btnCreateUser.Click += new System.EventHandler(this.btnCreateUser_Click);
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Location = new System.Drawing.Point(651, 287);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(100, 23);
            this.btnDeleteUser.TabIndex = 4;
            this.btnDeleteUser.Text = "حذف حساب";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click_1);
            // 
            // btnUpdateUser
            // 
            this.btnUpdateUser.Location = new System.Drawing.Point(470, 254);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new System.Drawing.Size(122, 23);
            this.btnUpdateUser.TabIndex = 5;
            this.btnUpdateUser.Text = "تحديث حساب المستخدم ";
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Click += new System.EventHandler(this.btnUpdateUser_Click_1);
            // 
            // dgvUsers
            // 
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(12, 12);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.Size = new System.Drawing.Size(437, 298);
            this.dgvUsers.TabIndex = 6;
            // 
            // cmbRoles
            // 
            this.cmbRoles.FormattingEnabled = true;
            this.cmbRoles.Location = new System.Drawing.Point(651, 81);
            this.cmbRoles.Name = "cmbRoles";
            this.cmbRoles.Size = new System.Drawing.Size(121, 21);
            this.cmbRoles.TabIndex = 7;
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.Location = new System.Drawing.Point(471, 152);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(100, 23);
            this.btnChooseImage.TabIndex = 12;
            this.btnChooseImage.Text = "صورة الحساب";
            this.btnChooseImage.UseVisualStyleBackColor = true;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click_1);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(471, 29);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(100, 20);
            this.txtFirstName.TabIndex = 13;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(471, 55);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(100, 20);
            this.txtLastName.TabIndex = 14;
            // 
            // txtEmployeePhone
            // 
            this.txtEmployeePhone.Location = new System.Drawing.Point(471, 82);
            this.txtEmployeePhone.Name = "txtEmployeePhone";
            this.txtEmployeePhone.Size = new System.Drawing.Size(100, 20);
            this.txtEmployeePhone.TabIndex = 15;
            // 
            // txtFingerprintID
            // 
            this.txtFingerprintID.Location = new System.Drawing.Point(471, 118);
            this.txtFingerprintID.Name = "txtFingerprintID";
            this.txtFingerprintID.Size = new System.Drawing.Size(100, 20);
            this.txtFingerprintID.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(522, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(538, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(691, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 21;
            // 
            // buttoninfo
            // 
            this.buttoninfo.Location = new System.Drawing.Point(19, 415);
            this.buttoninfo.Name = "buttoninfo";
            this.buttoninfo.Size = new System.Drawing.Size(75, 23);
            this.buttoninfo.TabIndex = 22;
            this.buttoninfo.Text = "معلومات";
            this.buttoninfo.UseVisualStyleBackColor = true;
            this.buttoninfo.Click += new System.EventHandler(this.buttoninfo_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(115, 418);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(115, 20);
            this.txtUserID.TabIndex = 23;
            // 
            // lblDoctorName
            // 
            this.lblDoctorName.Location = new System.Drawing.Point(12, 313);
            this.lblDoctorName.Name = "lblDoctorName";
            this.lblDoctorName.Size = new System.Drawing.Size(437, 99);
            this.lblDoctorName.TabIndex = 24;
            // 
            // textBoxsp
            // 
            this.textBoxsp.Location = new System.Drawing.Point(659, 144);
            this.textBoxsp.Name = "textBoxsp";
            this.textBoxsp.Size = new System.Drawing.Size(100, 20);
            this.textBoxsp.TabIndex = 25;
            // 
            // textBoxclinic
            // 
            this.textBoxclinic.Location = new System.Drawing.Point(659, 170);
            this.textBoxclinic.Name = "textBoxclinic";
            this.textBoxclinic.Size = new System.Drawing.Size(100, 20);
            this.textBoxclinic.TabIndex = 26;
            // 
            // textBoxfee
            // 
            this.textBoxfee.Location = new System.Drawing.Point(659, 196);
            this.textBoxfee.Name = "textBoxfee";
            this.textBoxfee.Size = new System.Drawing.Size(100, 20);
            this.textBoxfee.TabIndex = 27;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(470, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "تحديث  الموظف";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnupdateemployee);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(635, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 23);
            this.label2.TabIndex = 31;
            this.label2.Text = " انشاء وتحديث بيانات الطبيب";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(468, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "تحديث بيانات الموظف";
            // 
            // btnupdateclinic
            // 
            this.btnupdateclinic.Location = new System.Drawing.Point(471, 312);
            this.btnupdateclinic.Name = "btnupdateclinic";
            this.btnupdateclinic.Size = new System.Drawing.Size(121, 23);
            this.btnupdateclinic.TabIndex = 33;
            this.btnupdateclinic.Text = "تحديث بيانات الطبيب";
            this.btnupdateclinic.UseVisualStyleBackColor = true;
            this.btnupdateclinic.Click += new System.EventHandler(this.btnupdateclinic_Click_1);
            // 
            // ManageUsersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnupdateclinic);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxfee);
            this.Controls.Add(this.textBoxclinic);
            this.Controls.Add(this.textBoxsp);
            this.Controls.Add(this.lblDoctorName);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.buttoninfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFingerprintID);
            this.Controls.Add(this.txtEmployeePhone);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.btnChooseImage);
            this.Controls.Add(this.cmbRoles);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.btnUpdateUser);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.btnCreateUser);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtUser);
            this.Name = "ManageUsersForm";
            this.Text = "ManageUsersForm";
            this.Load += new System.EventHandler(this.ManageUsersForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Button btnCreateUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnUpdateUser;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.ComboBox cmbRoles;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtEmployeePhone;
        private System.Windows.Forms.TextBox txtFingerprintID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttoninfo;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label lblDoctorName;
        private System.Windows.Forms.TextBox textBoxsp;
        private System.Windows.Forms.TextBox textBoxclinic;
        private System.Windows.Forms.TextBox textBoxfee;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnupdateclinic;
    }
}