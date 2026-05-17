using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Hospital.Data; // استدعاء مشروع المكتبة
using System.Configuration;
using System.Data.SqlClient;
namespace FINAL1
{
    public partial class ManageUsersForm : Form
    {
        private HospitalDBEntities db = new HospitalDBEntities();

        public ManageUsersForm()
        {
            InitializeComponent();
        }

        private void ManageUsersForm_Load(object sender, EventArgs e)
        {
            LoadRolesCombo(); // شحن قائمة الصلاحيات (Admin, Doctor, Staff)
            RefreshUsersGrid(); // جلب وعرض الحسابات الحالية
            SetTextBoxPlaceholder(txtUserID, "🔍 أدخل رقم المستخدم هنا...");
            SetTextBoxPlaceholder(txtFingerprintID, "☝️ بصمة (اختياري)...");
            SetTextBoxPlaceholder(txtEmployeePhone, "📞 هاتف الموظف...");
            SetTextBoxPlaceholder(txtFirstName, "👤 الاسم الأول...");
            SetTextBoxPlaceholder(txtLastName, "👥 الكنية...");
            SetTextBoxPlaceholder(txtUser, "🧑‍💻 اسم المستخدم...");
            SetTextBoxPlaceholder(txtPass, "🔑 كلمة المرور...");
            SetTextBoxPlaceholder(txtUserID, "👩‍⚕️ لاسترجاع بيانات الطبيب...");
            SetTextBoxPlaceholder(textBoxclinic, "🏥 رقم العيادة...");
            SetTextBoxPlaceholder(textBoxsp, "🩺 التخصص الطبي...");
            SetTextBoxPlaceholder(textBoxfee, "💵 رسوم المعاينة...");
           

        }
        // 📌 كود المساعد لإرسال أمر التلميح لنظام تشغيل ويندوز (ضع هذه الدالة في أسفل الكلاس)
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);

private void SetTextBoxPlaceholder(TextBox textBox, string placeholderText)
        {
            // 0x1501 هو المعرف الخاص بأمر EM_SETCUEBANNER في نظام ويندوز
            SendMessage(textBox.Handle, 0x1501, 0, placeholderText);
        }

        // 1. شحن القائمة المنسدلة بالأدوار المتاحة من جدول Roles
        private void LoadRolesCombo()
        {
            try
            {
                var roles = db.Roles.Select(r => new { r.RoleID, r.RoleName }).ToList();
                cmbRoles.DataSource = roles;
                cmbRoles.DisplayMember = "RoleName";
                cmbRoles.ValueMember = "RoleID";
                cmbRoles.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("خطأ في جلب الأدوار: " + ex.Message); }
        }

        // 2. جلب وعرض الحسابات في الجدول منسقة بالعربية
        private void RefreshUsersGrid()
        {
            try
            {
                var usersList = (from u in db.Users
                                 join r in db.Roles on u.RoleID equals r.RoleID
                                 select new
                                 {
                                     رقم_الحساب = u.UserID,
                                     اسم_المستخدم = u.Username,
                                     الصلاحية_الحالية = r.RoleName,
                                     حالة_الحساب = u.IsActive == true ? "نشط" : "محظور"
                                 }).ToList();

                dgvUsers.DataSource = usersList;
            }
            catch (Exception ex) { MessageBox.Show("خطأ في تحديث الجدول: " + ex.Message); }
        }

        // 3. حدث النقر على سطر في الجدول لنقل البيانات إلى أدوات الإدخال للتعديل
        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                txtUser.Text = row.Cells["اسم_المستخدم"].Value.ToString();
                txtPass.Text = ""; // لا نعرض كلمة المرور القديمة لأسباب أمنية
                cmbRoles.Text = row.Cells["الصلاحية_الحالية"].Value.ToString();
                chkActive.Checked = row.Cells["حالة_الحساب"].Value.ToString() == "نشط";
            }
        }

        // 4. [إنشاء حساب] - زر إنشاء حساب جديد وإسناد صلاحية أولية له
        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. التحقق من تعبئة البيانات الأساسية للحساب والموظف معاً
                if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text) ||
                    string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    cmbRoles.SelectedValue == null)
                {
                    MessageBox.Show("الرجاء تعبئة بيانات الحساب والاسم الأول والكنية واختيار الصلاحية!");
                    return;
                }

                // 2. فحص أمني: التأكد من عدم تكرار اسم المستخدم في قاعدة البيانات
                if (db.Users.Any(u => u.Username == txtUser.Text.Trim()))
                {
                    MessageBox.Show("اسم المستخدم هذا مسجل مسبقاً، الرجاء اختيار اسم آخر!");
                    return;
                }

                // 3. فحص أمني: التأكد من عدم تكرار رقم البصمة في السيرفر لمنع أخطاء الـ Unique
                int? inputFingerprint = null;
                if (!string.IsNullOrWhiteSpace(txtFingerprintID.Text))
                {
                    int fId = Convert.ToInt32(txtFingerprintID.Text.Trim());
                    if (db.Employees.Any(emp => emp.FingerprintID == fId))
                    {
                        MessageBox.Show("رقم البصمة هذا مخصص لموظف آخر مسبقاً، الرجاء إدخال رقم بصمة فريد!");
                        return;
                    }
                    inputFingerprint = fId;
                }

                // -------------------------------------------------------------
                // المرحلة الأولى: إنشاء كائن الحساب وضخه في جدول Users
                // -------------------------------------------------------------
                Users newUser = new Users()
                {
                    Username = txtUser.Text.Trim(),
                    PasswordHash = txtPass.Text.Trim(),
                    RoleID = (int)cmbRoles.SelectedValue,
                    IsActive = chkActive.Checked, // استبدل chkActive بـ checkBox1 إذا لم تغير اسمه
                    ProfileImagePath = selectedImagePath
                };
                db.Users.Add(newUser);

                // حفظ الحساب أولاً ليقوم الـ SQL Server بتوليد الـ UserID التلقائي له
                db.SaveChanges();

                // -------------------------------------------------------------
                // المرحلة الثانية: إنشاء كائن الموظف وربطه بالحساب المنشأ سلفاً
                // -------------------------------------------------------------
                Employees newEmployee = new Employees()
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Phone = txtEmployeePhone.Text.Trim(),
                    FingerprintID = inputFingerprint, // ربط رقم بصمة جهاز الحضور
                    UserID = newUser.UserID           // سحب الرقم المولد تلقائياً من السيرفر للربط الفيزيائي بين الجدولين
                };
                db.Employees.Add(newEmployee);



                // حفظ بيانات الموظف المترابطة في السيرفر
                db.SaveChanges();
                // حفظ بيانات الموظف الأساسية أولاً في السيرفر لتوليد الـ EmployeeID تلقائياً
              

                // التحقق مما إذا كان الموظف الجديد طبيباً لإضافة تفاصيل عيادته
                if (cmbRoles.Text == "Doctor")
                {
                    // التحقق من إدخال رقم عيادة صحيح لمنع انهيار البرنامج عند التحويل الرقمي
                    int.TryParse(textBoxclinic.Text.Trim(), out int clinicNum);

                    DoctorDetails doctor = new DoctorDetails()
                    {
                        // 🌟 التصحيح البرمجي: ربط الطبيب بالـ EmployeeID المولد تلقائياً من السيرفر الآن
                        DoctorID = newEmployee.EmployeeID,

                        Specialization = textBoxsp.Text.Trim(),
                        ClinicNumber = textBoxclinic.Text.Trim(), // تمرير الرقم الصحيح بعد التحقق
                        ConsultationFee = decimal.TryParse(textBoxfee.Text.Trim(), out decimal fee) ? fee : 0
                    };

                    // 🌟 السطر المفقود 1: إضافة كائن الطبيب إلى جدول تفاصيل الأطباء في الـ Context
                    db.DoctorDetails.Add(doctor);

                    // 🌟 السطر M المفقود 2: حفظ تفاصيل الطبيب نهائياً داخل قاعدة البيانات
                    db.SaveChanges();
                    MessageBox.Show("💾 تم حفظ الموظف وتوليد تفاصيل العيادة الطبية بنجاح في قاعدة البيانات.", "نجاح التسجيل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     RefreshUsersGrid(); // تحديث الجدول لعرض الحساب الجديد مع تفاصيل العيادة مباشرةً بعد الإنشاء
                }




                MessageBox.Show("تم إنشاء حساب المستخدم وتعبئة بياناته الشخصية في جدول Employees بنجاح!");

                // 4. تنظيف كافة خانات الكتابة وتحديث جدول العرض أمام المستخدم
                selectedImagePath = "";
                ClearAllEmployeeInputs();
                RefreshUsersGrid();
               

            }
            catch (Exception ex)
            {
                MessageBox.Show("فشلت عملية الإدخال المترابط في قاعدة البيانات: " + ex.Message);
            }
        }


        // دالة مساعدة لتنظيف حقول النص بعد الحفظ الناجح
        private void ClearAllEmployeeInputs()
        {
            txtUser.Clear();
            txtPass.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmployeePhone.Clear();
            txtFingerprintID.Clear();
            cmbRoles.SelectedIndex = -1;
            chkActive.Checked = true;
        }



        // 5. [تغيير ومنح صلاحية] - زر تعديل الصلاحية أو حالة الحساب الحالي
        private void btnUpdateUser_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.CurrentRow == null) return;
                int userId = (int)dgvUsers.CurrentRow.Cells["رقم_الحساب"].Value;

                var userToUpdate = db.Users.Find(userId);
                if (userToUpdate != null)
                {
                    userToUpdate.Username = txtUser.Text.Trim();
                    userToUpdate.RoleID = (int)cmbRoles.SelectedValue; // منح الصلاحية الجديدة (تغيير الدور)
                    userToUpdate.IsActive = chkActive.Checked; // تغيير حالة النشاط
                    userToUpdate.IsActive = chkActive.Checked;

                    // إذا كتب الأدمن كلمة مرور جديدة نقوم بتحديثها، وإلا نترك القديمة كما هي
                    if (!string.IsNullOrWhiteSpace(txtPass.Text))
                    {
                        userToUpdate.PasswordHash = txtPass.Text.Trim();
                    }

                    db.SaveChanges();
                    MessageBox.Show("تم تحديث بيانات الحساب وتعديل الصلاحيات بنجاح!");
                    ClearInputs();
                    RefreshUsersGrid();
                }
                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    userToUpdate.ProfileImagePath = selectedImagePath;
                }

            }
            catch (Exception ex) { MessageBox.Show("خطأ أثناء التعديل: " + ex.Message); }
        }
        private void btnupdateemployee(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.CurrentRow == null) return;
                int userId = (int)dgvUsers.CurrentRow.Cells["رقم_الحساب"].Value;
                var employeeToUpdate = db.Employees.FirstOrDefault(emp => emp.UserID == userId);
                if (employeeToUpdate != null)
                {
                    employeeToUpdate.FirstName = txtFirstName.Text.Trim();
                    employeeToUpdate.LastName = txtLastName.Text.Trim();
                    employeeToUpdate.Phone = txtEmployeePhone.Text.Trim();
                    // تحديث رقم البصمة إذا تم إدخاله
                    if (!string.IsNullOrWhiteSpace(txtFingerprintID.Text))
                    {
                        int fId = Convert.ToInt32(txtFingerprintID.Text.Trim());
                        if (db.Employees.Any(emp => emp.FingerprintID == fId && emp.UserID != userId))
                        {
                            MessageBox.Show("رقم البصمة هذا مخصص لموظف آخر مسبقاً، الرجاء إدخال رقم بصمة فريد!");
                            return;
                        }
                        employeeToUpdate.FingerprintID = fId;
                    }
                    db.SaveChanges();
                    MessageBox.Show("تم تحديث بيانات الموظف بنجاح!");
                    ClearInputs();
                    RefreshUsersGrid();
                }
            }
            catch (Exception ex) { MessageBox.Show("خطأ أثناء تعديل بيانات الموظف: " + ex.Message); }




        }

        private void btnupdateclinic_Click_1(object sender, EventArgs e)
        {
            try
            {
                // 1. فحص هل تم اختيار سطر من الجدول
                if (dgvUsers.CurrentRow == null)
                {
                    MessageBox.Show("تنبيه: لم تقم باختيار أي سطر من الجدول! ⚠️");
                    return;
                }

                int userId = (int)dgvUsers.CurrentRow.Cells["رقم_الحساب"].Value;

                // 2. فحص وجود الموظف
                var employee = db.Employees.FirstOrDefault(emp => emp.UserID == userId);
                if (employee == null)
                {
                    MessageBox.Show($"خطأ: لم يتم العثور على موظف مرتبط برقم الحساب: {userId} ❌");
                    return;
                }
                
                // 3. فحص وجود تفاصيل الطبيب
                var doctorDetails = db.DoctorDetails.FirstOrDefault(doc => doc.DoctorID == employee.EmployeeID);
                if (doctorDetails == null)
                {
                    doctorDetails = new DoctorDetails();
                    doctorDetails.DoctorID = employee.EmployeeID;

                    // إضافته لقاعدة البيانات لأنه سجل جديد
                    db.DoctorDetails.Add(doctorDetails);

                }

                // 4. إذا وصل الكود هنا.. سيتم التعديل حتماً
                doctorDetails.Specialization = textBoxsp.Text.Trim();
                doctorDetails.ClinicNumber = textBoxclinic.Text.Trim();
                doctorDetails.ConsultationFee = decimal.TryParse(textBoxfee.Text.Trim(), out decimal fee) ? fee : 0;

                db.SaveChanges();
                MessageBox.Show("تم تحديث بيانات العيادة والتخصص بنجاح! ✅");

                ClearInputs();
                RefreshUsersGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ غير متوقع أثناء تعديل بيانات العيادة: " + ex.Message);
            }
        }


        // 6. [حذف حساب] - زر حذف الحساب نهائياً من قاعدة البيانات
        private void btnDeleteUser_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.CurrentRow == null) return;
                int userId = (int)dgvUsers.CurrentRow.Cells["رقم_الحساب"].Value;
                string username = dgvUsers.CurrentRow.Cells["اسم_المستخدم"].Value.ToString();

                if (userId == 10) // افتراضياً الحساب الرئيسي للأدمن هو UserID = 10 واسم المستخدم "admin"
                {
                    MessageBox.Show("حماية للنظام: لا يمكن حذف الحساب الرئيسي للأدمن!");
                    return;
                }

                DialogResult result = MessageBox.Show($"هل أنت متأكد من حذف الحساب ({username}) نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                   // var userToDelete = db.DoctorDetails.FirstOrDefault(doc => doc.DoctorID == db.Employees.FirstOrDefault(emp => emp.UserID == userId).EmployeeID);

                    var userToDelete = db.Users.Find(userId);
                    if (userToDelete != null)
                    {
                        // حذف بيانات الطبيب المرتبطة أولاً إذا كان موجوداً لمنع أخطاء المفتاح الأجنبي (Foreign Key)
                        db.DoctorDetails.RemoveRange(db.DoctorDetails.Where(doc => doc.DoctorID == db.Employees.FirstOrDefault(emp => emp.UserID == userId).EmployeeID));
                        db.Employees.RemoveRange(db.Employees.Where(emp => emp.UserID == userId));
                        db.Users.Remove(userToDelete);
                        db.SaveChanges();

                        MessageBox.Show("تم حذف الحساب نهائياً من النظام.");
                        ClearInputs();
                        RefreshUsersGrid();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("لا يمكن حذف الحساب لارتباطه ببيانات موظفين نشطة، يمكنك حظره بجعل الحالة غير نشط بدلاً من الحذف: " + ex.Message); }
        }

        private void ClearInputs()
        {
            txtUser.Clear();
            txtPass.Clear();
            cmbRoles.SelectedIndex = -1;
            chkActive.Checked = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        string selectedImagePath = "";




        private void btnChooseImage_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // تصفية الامتدادات لقبول الصور فقط
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = ofd.FileName; // حفظ مسار الصورة الكامل المختار من جهاز المستخدم
                    MessageBox.Show("تم اختيار الصورة بنجاح! سيتم حفظها عند ضغط زر الحفظ أو الإنشاء.");
                }
            }

        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttoninfo_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. التحقق من إدخال رقم المستخدم في مربع النص
                if (string.IsNullOrWhiteSpace(txtUserID.Text))
                {
                    MessageBox.Show("يرجى إدخال رقم المستخدم (User ID) أولاً لاسترجاع بيانات الطبيب.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int inputUserID = int.Parse(txtUserID.Text);

                using (var db = new HospitalDBEntities())   //
                {
                    // 2. عمل استعلام يدمج جدول الموظفين مع تفاصيل الطبيب بناءً على الـ UserID
                    // قمنا باستخدام Include لجلب البيانات المرتبطة (Eager Loading) لضمان السرعة وكفاءة الكود
                    var doctor = db.Employees
                                   .Include("DoctorDetails")
                                   .FirstOrDefault(emp => emp.UserID == inputUserID);

                    if (doctor != null)
                    {
                        // 1. تجهيز السطر الأول (الاسم الكامل)
                        string doctorName = $"{doctor.FirstName} {doctor.LastName}";
                        string specialization = "";
                        string clinicNumber = "";
                        string consultationFee = "";

                        // 2. استخراج بقية البيانات من جدول تفاصيل الأطباء
                        if (doctor.DoctorDetails != null)
                        {
                            specialization = doctor.DoctorDetails.Specialization;
                            clinicNumber = doctor.DoctorDetails.ClinicNumber.ToString();
                            consultationFee = $"{doctor.DoctorDetails.ConsultationFee} د.أ";
                        }
                        else
                        {
                            specialization = "كادر إداري / موظف عام";
                            clinicNumber = "-";
                            consultationFee = "-";
                        }

                        // 3. دمج البيانات كلها في Label واحدة مع سطر جديد (\n) لكل معلومة
                        lblDoctorName.Text = $"👨‍⚕️ اسم الطبيب المعالج: {doctorName}\n" +
                                            $"🩺 التخصص الطبي: {specialization}\n" +
                                            $"🚪 رقم العيادة / القسم: {clinicNumber}\n" +
                                            $"💵 رسوم المعاينة: {consultationFee}";

                        txtUserID.ForeColor = System.Drawing.Color.DarkBlue;
                    }
                    else
                    {
                        MessageBox.Show($"عذراً، لم يتم العثور على أي طبيب أو موظف مرتبط برقم المستخدم ({inputUserID}) في النظام.", "المستخدم غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearDoctorLabels();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل نظام الاتصال بقاعدة البيانات أثناء جلب معلومات الطبيب: {ex.Message}", "خطأ تقني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // دالة مساعدة لتنظيف واجهة العرض عند الخطأ
        private void ClearDoctorLabels()
        {
            // تفريغ كافة الأسطر داخل نفس الأداة عند حدوث خطأ أو عدم العثور على الطبيب
            txtUserID.Text = "👨‍⚕️ اسم الطبيب المعالج: غير معروف\n" +
                                "🩺 التخصص الطبي: -\n" +
                                "🚪 رقم العيادة / القسم: -\n" +
                                "💵 رسوم المعاينة: -";

            txtUserID.ForeColor = System.Drawing.Color.Red;
        }

       
    }

}


      
