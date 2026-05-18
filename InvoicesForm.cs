using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Hospital.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace FINAL1
{
    public partial class InvoicesForm : Form
    {
        private HospitalDBEntities db = Class1.GetContext();

        public InvoicesForm()
        {
            InitializeComponent();
        }

        private void InvoicesForm_Load(object sender, EventArgs e)
        {
            try
            {
                cmbPatientsInvoice.DataSource = db.Patients.Select(p => new { p.PatientID, p.FullName }).ToList();
                cmbPatientsInvoice.DisplayMember = "FullName";
                cmbPatientsInvoice.ValueMember = "PatientID";
                cmbPatientsInvoice.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("خطأ في جلب المرضى: " + ex.Message); }

            RefreshInvoicesGrid();
        }

        private void RefreshInvoicesGrid()
        {
            try
            {
                var invoices = (from inv in db.Invoices
                                join p in db.Patients on inv.PatientID equals p.PatientID
                                select new
                                {
                                    رقم_الفاتورة = inv.InvoiceID,
                                    اسم_المريض = p.FullName,
                                    المبلغ_الإجمالي = inv.TotalAmount,
                                    الخصم = inv.Discount,
                                    الصافي_النهائي = inv.FinalAmount,
                                    حالة_الدفع = inv.PaymentStatus,
                                    التاريخ = inv.InvoiceDate
                                }).OrderByDescending(i => i.التاريخ).ToList();
                dgvInvoices.DataSource = invoices;
            }
            catch (Exception ex) { MessageBox.Show("خطأ في تحديث الجدول: " + ex.Message); }
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPatientsInvoice.SelectedValue == null || string.IsNullOrWhiteSpace(txtServicePrice.Text))
                {
                    MessageBox.Show("الرجاء اختيار المريض وتحديد سعر الخدمة الطبية!");
                    return;
                }

                // الحسابات المالية التلقائية
                decimal total = Convert.ToDecimal(txtServicePrice.Text.Trim());
                decimal discount = string.IsNullOrWhiteSpace(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text.Trim());
                decimal final = total - discount;
                txtFinalAmount.Text = final.ToString();

                // 1. حفظ الفاتورة الرئيسية (Invoices)
                Invoices newInvoice = new Invoices()
                {
                    PatientID = (int)cmbPatientsInvoice.SelectedValue,
                    TotalAmount = total,
                    Tax = 0,
                    Discount = discount,
                    FinalAmount = final,
                    PaymentStatus = "Paid",
                    InvoiceDate = DateTime.Now
                };
                db.Invoices.Add(newInvoice);
                db.SaveChanges(); // حفظ الفاتورة للحصول على الـ InvoiceID التلقائي

                // 2. حفظ تفاصيل البند في الجدول الفرعي التابع (InvoiceDetails)
                InvoiceDetails details = new InvoiceDetails()
                {
                    InvoiceID = newInvoice.InvoiceID,
                    ItemName = string.IsNullOrWhiteSpace(txtServiceName.Text) ? "كشفية طبية عامة" : txtServiceName.Text.Trim(),
                    Quantity = 1,
                    UnitPrice = total,
                    SubTotal = total
                };
                db.InvoiceDetails.Add(details);
                db.SaveChanges();

                MessageBox.Show("تم إصدار وتثبيت الفاتورة المالية وإدراج بنودها بنجاح!");
                RefreshInvoicesGrid();
            }
            catch (Exception ex) { MessageBox.Show("خطأ في الحسابات أو الحفظ: " + ex.Message); }
        }
    }
}
