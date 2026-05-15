using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Hospital.Data; // استدعاء مشروع المكتبة الخاص بك

namespace FINAL1
{
    public partial class AppointmentsForm : Form
    {
        private HospitalDBEntities db = new HospitalDBEntities();

        public AppointmentsForm()
        {
            InitializeComponent();
        }

        private void AppointmentsForm_Load(object sender, EventArgs e)
        {
            // ضبط صيغة تاريخ الحجز ليشمل الوقت والساعة
            dtpAppDate.Format = DateTimePickerFormat.Custom;
            dtpAppDate.CustomFormat = "yyyy-MM-dd hh:mm tt";

            LoadListsData();       // شحن قوائم الاختيار بالأطباء والمرضى من قاعدة البيانات
            RefreshAppointments(); // عرض جدول المواعيد المحجوزة حالياً
        }

        // 1. دالة شحن القوائم المنسدلة (ComboBox) ببيانات المرضى والأطباء
        private void LoadListsData()
        {
            try
            {
                // جلب المرضى
                var patientsList = db.Patients
                                     .Select(p => new { p.PatientID, p.FullName })
                                     .ToList();
                cmbPatients.DataSource = patientsList;
                cmbPatients.DisplayMember = "FullName";
                cmbPatients.ValueMember = "PatientID";

                // جلب الموظفين الذين يملكون تفاصيل أطباء
                var doctorsList = (from emp in db.Employees
                                   join doc in db.DoctorDetails on emp.EmployeeID equals doc.DoctorID
                                   select new
                                   {
                                       DoctorID = emp.EmployeeID,
                                       DoctorName = emp.FirstName + " " + emp.LastName + " (" + doc.Specialization + ")"
                                   }).ToList();
                cmbDoctors.DataSource = doctorsList;
                cmbDoctors.DisplayMember = "DoctorName";
                cmbDoctors.ValueMember = "DoctorID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل قوائم المرضى والأطباء: " + ex.Message);
            }
        }

        // 2. دالة جلب وعرض المواعيد الحالية بشكل منسق بالعربية
        private void RefreshAppointments()
        {
            try
            {
                var appointmentsList = (from app in db.Appointments
                                        join p in db.Patients on app.PatientID equals p.PatientID
                                        join emp in db.Employees on app.DoctorID equals emp.EmployeeID
                                        select new
                                        {
                                            رقم_الحجز = app.AppointmentID,
                                            اسم_المريض = p.FullName,
                                            الطبيب_المعالج = emp.FirstName + " " + emp.LastName,
                                            تاريخ_الموعد = app.AppointmentDate,
                                            حالة_الحجز = app.Status
                                        }).OrderBy(a => a.تاريخ_الموعد).ToList();

                dgvAppointments.DataSource = appointmentsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء جلب المواعيد: " + ex.Message);
            }
        }

        // 3. حدث الضغط على زر حفظ لحجز موعد جديد وإدراجه في SQL Server
        private void btnSaveAppointment_Click_1(object sender, EventArgs e)
        {
            try
            {
                // التحقق من صحة الاختيارات
                if (cmbPatients.SelectedValue == null || cmbDoctors.SelectedValue == null)
                {
                    MessageBox.Show("الرجاء اختيار المريض والطبيب أولاً!");
                    return;
                }

                // إنشاء كائن موعد جديد وتعبئته من الواجهة
                Appointments newApp = new Appointments()
                {
                    PatientID = (int)cmbPatients.SelectedValue,
                    DoctorID = (int)cmbDoctors.SelectedValue,
                    AppointmentDate = dtpAppDate.Value,
                    Status = "Scheduled" // الحالة الافتراضية: تم الحجز المسبق
                };

                db.Appointments.Add(newApp); // إضافة الحجز للموديل
                db.SaveChanges();            // حفظ التغييرات فعلياً في قاعدة البيانات

                MessageBox.Show("تم حجز موعد المريض بنجاح وتثبيته في النظام!");
                RefreshAppointments();       // إعادة تحديث الجدول ليرى المستخدم السطر الجديد فوراً
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء حفظ الموعد: " + ex.Message);
            }
        }

     
    }
}
