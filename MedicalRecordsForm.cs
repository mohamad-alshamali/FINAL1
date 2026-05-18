using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Hospital.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace FINAL1
{
    public partial class MedicalRecordsForm : Form
    {
        private HospitalDBEntities db = Class1.GetContext();

        public MedicalRecordsForm()
        {
            InitializeComponent();
            LoadDataLists();
            RefreshRecordsGrid();

        }

        private void MedicalRecordsForm_Load(object sender, EventArgs e)
        {
         
        }

        private void LoadDataLists()
        {
            try
            {
                // 1. جلب البيانات من قاعدة البيانات في الذاكرة أولاً
                var patientsData = db.Patients.Select(p => new { p.PatientID, p.FullName }).ToList();
                var doctorsData = db.Employees.Select(emp => new { emp.EmployeeID, DoctorName = emp.FirstName + " " + emp.LastName }).ToList();

                // -------------------------------------------------------------
                // شحن قائمة المرضى (الترتيب الحرج والصحيح)
                // -------------------------------------------------------------
                cmbPatients.DisplayMember = "FullName";   // 1. تحديد حقل العرض المرئي أولاً
                cmbPatients.ValueMember = "PatientID";     // 2. تحديد حقل الرقم التعريفي ثانياً
                cmbPatients.DataSource = patientsData;     // 3. حقن البيانات في النهاية لتظهر الأسماء فوراً
                cmbPatients.SelectedIndex = -1;

                // -------------------------------------------------------------
                // شحن قائمة الأطباء (الترتيب الحرج والصحيح)
                // -------------------------------------------------------------
                cmbDoctors.DisplayMember = "DoctorName";   // 1. تحديد حقل اسم الدكتور المدمج أولاً
                cmbDoctors.ValueMember = "EmployeeID";     // 2. تحديد حقل المعرف الرقمي ثانياً
                cmbDoctors.DataSource = doctorsData;       // 3. حقن البيانات في النهاية لتظهر الأسماء فوراً
                cmbDoctors.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء شحن القوائم: " + ex.Message);
            }
        }



        private void RefreshRecordsGrid()
        {
            try
            {
                var records = (from rec in db.MedicalRecords
                               join p in db.Patients on rec.PatientID equals p.PatientID
                               join emp in db.Employees on rec.DoctorID equals emp.EmployeeID
                               select new
                               {
                                   رقم_السجل = rec.RecordID,
                                   المريض = p.FullName,
                                   الطبيب = emp.FirstName + " " + emp.LastName,
                                   التاريخ = rec.VisitDate,
                                   التشخيص = rec.Diagnosis,
                                   العلاج = rec.Prescription
                               }).OrderByDescending(r => r.التاريخ).ToList();
                dgvRecords.DataSource = records;
            }
            catch (Exception ex) { MessageBox.Show("خطأ في تحديث السجلات: " + ex.Message); }
        }

        private void btnSaveRecord_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDiagnosis.Text) || cmbPatients.SelectedValue == null)
                {
                    MessageBox.Show("الرجاء اختيار المريض وكتابة التشخيص الطبي!");
                    return;
                }

                MedicalRecords newRecord = new MedicalRecords()
                {
                    PatientID = (int)cmbPatients.SelectedValue,
                    DoctorID = (int)cmbDoctors.SelectedValue,
                    VisitDate = DateTime.Now,
                    Diagnosis = txtDiagnosis.Text.Trim(),
                    Prescription = txtPrescription.Text.Trim()
                };

                db.MedicalRecords.Add(newRecord);
                db.SaveChanges();

                MessageBox.Show("تم حفظ السجل الطبي للمريض بنجاح!");
                txtDiagnosis.Clear();
                txtPrescription.Clear();
                RefreshRecordsGrid();
            }
            catch (Exception ex) { MessageBox.Show("خطأ أثناء الحفظ: " + ex.Message); }
        }
    }
}
