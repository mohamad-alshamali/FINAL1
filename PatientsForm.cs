using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Hospital.Data; // استدعاء مشروع المكتبة
using System.Configuration;
using System.Data.SqlClient;
namespace FINAL1
{
    public partial class PatientsForm : Form
    {
        private HospitalDBEntities db = Class1.GetContext();

        public PatientsForm()
        {
            InitializeComponent();
            
            RefreshPatientsGrid();
        }

        private void PatientsForm_Load(object sender, EventArgs e)
        {
            RefreshPatientsGrid(); // عرض قائمة المرضى الحاليين فور فتح الشاشة
        }

        // 1. دالة جلب وعرض المرضى في الـ DataGridView
        private void RefreshPatientsGrid()
        {
            try
            {
                var patientsList = db.Patients
                                     .Select(p => new
                                     {
                                         رقم_المريض = p.PatientID,
                                         الاسم_الكامل = p.FullName,
                                         الجنس = p.Gender,
                                         تاريخ_الميلاد = p.DateOfBirth,
                                         رقم_الهاتف = p.Phone,
                                         فصيلة_الدم = p.BloodType
                                     }).ToList();

                dgvPatients.DataSource = patientsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء جلب قائمة المرضى: " + ex.Message);
            }
        }

        // 2. حدث النقر المزدوج على زر الحفظ لتخزين مريض جديد في SQL Server
        private void btnSavePatient_Click(object sender, EventArgs e)
        {
            try
            {
                // التحقق من إدخال البيانات الأساسية
                if (string.IsNullOrWhiteSpace(txtFullName.Text) || cmbGender.SelectedItem == null)
                {
                    MessageBox.Show("الرجاء إدخال اسم المريض واختيار الجنس أولاً!");
                    return;
                }

                // إنشاء كائن مريض جديد وتعبئته من أدوات الواجهة
                Patients newPatient = new Patients()
                {
                    FullName = txtFullName.Text.Trim(),
                    Gender = cmbGender.SelectedItem.ToString(),
                    DateOfBirth = dtpBirthDate.Value.Date,
                    Phone = txtPhone.Text.Trim(),
                    BloodType = txtBloodType.Text.Trim()
                };

                db.Patients.Add(newPatient); // إضافة المريض للموديل
                db.SaveChanges();            // حفظ التغييرات فعلياً في قاعدة البيانات

                MessageBox.Show("تم تسجيل المريض الجديد في النظام بنجاح!");

                // تنظيف أدوات الكتابة لتجهيزها لمريض آخر
                txtFullName.Clear();
                txtPhone.Clear();
                txtBloodType.Clear();
                cmbGender.SelectedIndex = -1;

                RefreshPatientsGrid(); // تحديث الجدول أمام المستخدم فوراً
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء حفظ بيانات المريض: " + ex.Message);
            }
        }

        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // التأكد من أن المستخدم ضغط على سطر حقيقي يحتوي على بيانات وليس العناوين
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPatients.Rows[e.RowIndex];

                // جلب البيانات من خلايا الجدول إلى أدوات الواجهة (تأكد من مطابقة أسماء الأدوات لديك)
                txtFullName.Text = row.Cells["الاسم_الكامل"].Value.ToString();
                cmbGender.SelectedItem = row.Cells["الجنس"].Value.ToString();
                dtpBirthDate.Value = Convert.ToDateTime(row.Cells["تاريخ_الميلاد"].Value);
                txtPhone.Text = row.Cells["رقم_الهاتف"].Value?.ToString();
                txtBloodType.Text = row.Cells["فصيلة_الدم"].Value?.ToString();
            }
        }

        private void btnUpdatePatient_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. التأكد من أن المستخدم حدد مريضاً من الجدول
                if (dgvPatients.CurrentRow == null)
                {
                    MessageBox.Show("الرجاء تحديد المريض المراد تعديله من الجدول أولاً!");
                    return;
                }

                // 2. جلب الرقم التعريفي (ID) للمريض المحدد حالياً من أول عمود في الجدول
                int patientId = (int)dgvPatients.CurrentRow.Cells["رقم_المريض"].Value;

                // 3. البحث عن المريض داخل قاعدة البيانات باستخدام حقل الـ ID
                var patientToUpdate = db.Patients.Find(patientId);

                if (patientToUpdate != null)
                {
                    // 4. تحديث القيم القديمة بالقيم الجديدة المكتوبة في مربعات النص
                    patientToUpdate.FullName = txtFullName.Text.Trim();
                    patientToUpdate.Gender = cmbGender.SelectedItem.ToString();
                    patientToUpdate.DateOfBirth = dtpBirthDate.Value.Date;
                    patientToUpdate.Phone = txtPhone.Text.Trim();
                    patientToUpdate.BloodType = txtBloodType.Text.Trim();

                    db.SaveChanges(); // حفظ التعديلات نهائياً في SQL Server

                    MessageBox.Show("تم تعديل بيانات المريض بنجاح!");
                    RefreshPatientsGrid(); // إعادة تحديث الجدول فوراً لترى التعديل الجديد
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء عملية التعديل: " + ex.Message);
            }
        }

        private void btnDeletePatient_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. التأكد من تحديد مريض
                if (dgvPatients.CurrentRow == null)
                {
                    MessageBox.Show("الرجاء تحديد المريض المراد حذفه من الجدول أولاً!");
                    return;
                }

                int patientId = (int)dgvPatients.CurrentRow.Cells["رقم_المريض"].Value;
                string patientName = dgvPatients.CurrentRow.Cells["الاسم_الكامل"].Value.ToString();

                // 2. إظهار رسالة تأكيدية (نعم / لا) للتأكد من رغبة المستخدم في الحذف
                DialogResult confirmResult = MessageBox.Show($"هل أنت متأكد تماماً من حذف المريض ({patientName})؟\nتنبيه: سيتم حذف جميع مواعيده وسجلاته المرتبطة به نهائياً!",
                                                             "تأكيد الحذف",
                                                             MessageBoxButtons.YesNo,
                                                             MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    // 3. البحث عن المريض وحذفه من الموديل
                    var patientToDelete = db.Patients.Find(patientId);

                    if (patientToDelete != null)
                    {
                        db.Patients.Remove(patientToDelete); // أمر الحذف من الذاكرة
                        db.SaveChanges(); // تنفيذ الحذف الفعلي والنهائي في قاعدة البيانات

                        MessageBox.Show("تم حذف المريض من النظام بنجاح!");

                        // تنظيف أدوات الكتابة بعد الحذف
                        txtFullName.Clear();
                        txtPhone.Clear();
                        txtBloodType.Clear();

                        RefreshPatientsGrid(); // تحديث الجدول فوراً لإخفاء السطر المحذوف
                    }
                }
            }
            catch (Exception ex)
            {
                // رسالة حماية في حال كان المريض مرتبطاً بفواتير أو عمليات مالية تمنع حذفه مباشرة (تكامل البيانات)
                MessageBox.Show("لا يمكن حذف هذا المريض لوجود سجلات طبية أو مواعيد نشطة مرتبطة به في النظام: " + ex.Message);
            }
        }

    }
}
