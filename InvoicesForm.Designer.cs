namespace FINAL1
{
    partial class InvoicesForm
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
            this.cmbPatientsInvoice = new System.Windows.Forms.ComboBox();
            this.txtServiceName = new System.Windows.Forms.TextBox();
            this.txtServicePrice = new System.Windows.Forms.TextBox();
            this.txtDiscount = new System.Windows.Forms.TextBox();
            this.txtFinalAmount = new System.Windows.Forms.TextBox();
            this.dgvInvoices = new System.Windows.Forms.DataGridView();
            this.btnCreateInvoice = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoices)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPatientsInvoice
            // 
            this.cmbPatientsInvoice.FormattingEnabled = true;
            this.cmbPatientsInvoice.Location = new System.Drawing.Point(574, 23);
            this.cmbPatientsInvoice.Name = "cmbPatientsInvoice";
            this.cmbPatientsInvoice.Size = new System.Drawing.Size(193, 21);
            this.cmbPatientsInvoice.TabIndex = 0;
            this.cmbPatientsInvoice.Text = "المريض الصادرة له الفاتورة";
            // 
            // txtServiceName
            // 
            this.txtServiceName.Location = new System.Drawing.Point(574, 85);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(193, 20);
            this.txtServiceName.TabIndex = 1;
            this.txtServiceName.Text = "اسم الخدمة المقدمة ";
            // 
            // txtServicePrice
            // 
            this.txtServicePrice.Location = new System.Drawing.Point(574, 143);
            this.txtServicePrice.Name = "txtServicePrice";
            this.txtServicePrice.Size = new System.Drawing.Size(193, 20);
            this.txtServicePrice.TabIndex = 2;
            this.txtServicePrice.Text = "اسم الخدمة المقدمة يدوياً ";
            // 
            // txtDiscount
            // 
            this.txtDiscount.Location = new System.Drawing.Point(574, 202);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.Size = new System.Drawing.Size(193, 20);
            this.txtDiscount.TabIndex = 3;
            this.txtDiscount.Text = "قيمة الخصم";
            // 
            // txtFinalAmount
            // 
            this.txtFinalAmount.Location = new System.Drawing.Point(574, 256);
            this.txtFinalAmount.Name = "txtFinalAmount";
            this.txtFinalAmount.Size = new System.Drawing.Size(193, 20);
            this.txtFinalAmount.TabIndex = 4;
            this.txtFinalAmount.Text = "الصافي الإجمالي النهائي المتبقي للدفع";
            // 
            // dgvInvoices
            // 
            this.dgvInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoices.Location = new System.Drawing.Point(12, 23);
            this.dgvInvoices.Name = "dgvInvoices";
            this.dgvInvoices.Size = new System.Drawing.Size(489, 401);
            this.dgvInvoices.TabIndex = 5;
            // 
            // btnCreateInvoice
            // 
            this.btnCreateInvoice.Location = new System.Drawing.Point(627, 401);
            this.btnCreateInvoice.Name = "btnCreateInvoice";
            this.btnCreateInvoice.Size = new System.Drawing.Size(100, 23);
            this.btnCreateInvoice.TabIndex = 6;
            this.btnCreateInvoice.Text = "إصدار وتثبيت الفاتورة";
            this.btnCreateInvoice.UseVisualStyleBackColor = true;
            this.btnCreateInvoice.Click += new System.EventHandler(this.btnCreateInvoice_Click);
            // 
            // InvoicesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCreateInvoice);
            this.Controls.Add(this.dgvInvoices);
            this.Controls.Add(this.txtFinalAmount);
            this.Controls.Add(this.txtDiscount);
            this.Controls.Add(this.txtServicePrice);
            this.Controls.Add(this.txtServiceName);
            this.Controls.Add(this.cmbPatientsInvoice);
            this.Name = "InvoicesForm";
            this.Text = "InvoicesForm";
            this.Load += new System.EventHandler(this.InvoicesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPatientsInvoice;
        private System.Windows.Forms.TextBox txtServiceName;
        private System.Windows.Forms.TextBox txtServicePrice;
        private System.Windows.Forms.TextBox txtDiscount;
        private System.Windows.Forms.TextBox txtFinalAmount;
        private System.Windows.Forms.DataGridView dgvInvoices;
        private System.Windows.Forms.Button btnCreateInvoice;
    }
}