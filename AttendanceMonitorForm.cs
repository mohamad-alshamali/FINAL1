
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hospital.Data;
using System.Data.Entity;
// تأكد من مطابقة اسم مشروع المكتبة لديك

namespace FINAL1
{
    public partial class AttendanceMonitorForm : Form
    {
        private HospitalDBEntities db = new HospitalDBEntities();
        public AttendanceMonitorForm()
        {
            InitializeComponent();
        }

        private void AttendanceMonitorForm_Load(object sender, EventArgs e)
        {
            RefreshAttendanceGrid();

        }
        private void RefreshAttendanceGrid()
        {
            try
            {
                // استعلام LINQ ذكي يربط جداول الحضور والموظفين لترتيب العرض
                var attendanceRecords = (from a in db.Attendance
                                         join e in db.Employees on a.FingerprintID equals e.FingerprintID
                                         select new
                                         {
                                             رقم_الحركة = a.AttendanceID,
                                             رقم_البصمة = a.FingerprintID,
                                             الاسم = e.FirstName + " " + e.LastName, // جلب الاسم من جدول الموظفين مباشرة e
                                             وقت_التبصيم = a.LogDateTime,
                                             نوع_الحركة = a.Direction == "IN" ? "حضور" : "انصراف"
                                         })
                          .OrderByDescending(x => x.وقت_التبصيم) // ترتيب الحركات من الأحدث للأقدم
                          .ToList();


                // ربط النتيجة المستخرجة بأداة العرض على الشاشة
                dgvAttendance.DataSource = attendanceRecords;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء جلب سجلات الحضور: " + ex.Message);
            }
        }
    }
}

