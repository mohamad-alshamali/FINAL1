using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Hospital.Data; // استدعاء مشروع المكتبة

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

        // 6. [حذف حساب] - زر حذف الحساب نهائياً من قاعدة البيانات
        private void btnDeleteUser_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.CurrentRow == null) return;
                int userId = (int)dgvUsers.CurrentRow.Cells["رقم_الحساب"].Value;
                string username = dgvUsers.CurrentRow.Cells["اسم_المستخدم"].Value.ToString();

                if (username.ToLower() == "admin")
                {
                    MessageBox.Show("حماية للنظام: لا يمكن حذف الحساب الرئيسي للأدمن!");
                    return;
                }

                DialogResult result = MessageBox.Show($"هل أنت متأكد من حذف الحساب ({username}) نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    var userToDelete = db.Users.Find(userId);
                    if (userToDelete != null)
                    {
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
    }
}
      
