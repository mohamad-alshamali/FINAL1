using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Hospital.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace FINAL1
{
    public partial class Form1 : Form
    {
        private HospitalDBEntities db = Class1.GetContext();
        private string currentUserRole = "";
        private static ColorDialog colorChooser = new ColorDialog();

        public Color backcolor = Color.Wheat;
        public Color forecolor = Color.FromArgb(100, 0, 0, 255);

        public Form1()
        {
            InitializeComponent();

            // أسماء القوائم
            manageUsersToolStripMenuItem.Text = "الادارة";
            invoicesToolStripMenuItem.Text = "الفواتير";
            attendanceMonitorToolStripMenuItem.Text = "لوحة التحكم";
            patientsToolStripMenuItem.Text = "إدارة المرضى";
            appointmentsToolStripMenuItem.Text = "المواعيد";
            medicalRecordsToolStripMenuItem.Text = "السجلات الطبية";

            // ربط الأحداث
            manageUsersToolStripMenuItem.Click += manageUsersToolStripMenuItem_Click;
            invoicesToolStripMenuItem.Click += invoicesToolStripMenuItem_Click;
            attendanceMonitorToolStripMenuItem.Click += attendanceMonitorToolStripMenuItem_Click;
            patientsToolStripMenuItem.Click += patientsToolStripMenuItem_Click;
            appointmentsToolStripMenuItem.Click += appointmentsToolStripMenuItem_Click;
            medicalRecordsToolStripMenuItem.Click += medicalRecordsToolStripMenuItem_Click;

            menuStrip1.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 👇⬇🔻فعله في حال فشل الاتصال للاتصال بالـ ip المحلي
           // Hospital.Data.Properties.Settings.Default.Reset();
            treeView1.Visible = false;

            menuStrip1.Visible = true;

            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Enabled = false;
            }
            settingsToolStripMenuItem.Enabled = true;
            panelLogin.Visible = true;
            panelLogin.BringToFront();

            ResetAndBuildTreeView();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenMap();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenMap();
        }

        private void OpenMap()
        {
            string url = "https://www.google.com/maps";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("تعذر فتح الرابط");
            }
        }
        bool isLoggedIn = false;
        // زر تسجيل الدخول والخروج
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
           


                // تسجيل خروج
                if (btnLogin.Text == "تسجيل الخروج")
            {
                foreach (ToolStripMenuItem item in menuStrip1.Items)
                {
                    item.Enabled = false;
                }

                label2.Text = "تسجيل الدخول";
                label2.ForeColor = Color.Black;

                txtUsername.Clear();
                txtPassword.Clear();

                txtUsername.Enabled = true;
                txtPassword.Enabled = true;

                btnLogin.Text = "تسجيل الدخول";
                btnLogin.BackColor = SystemColors.Control;
                btnLogin.ForeColor = Color.Black;

                picUserProfile.Visible = false;

                currentUserRole = "";

                MessageBox.Show("تم تسجيل الخروج بنجاح");
                isLoggedIn = false;

                return;
            }

            // تسجيل دخول
            string inputUser = txtUsername.Text.Trim();
            string inputPass = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(inputUser) || string.IsNullOrEmpty(inputPass))
            {
                MessageBox.Show("الرجاء إدخال اسم المستخدم وكلمة المرور");
                return;
            }

            txtUsername.Enabled = false;
            txtPassword.Enabled = false;

            ProcessLogin(inputUser, inputPass);
        }

        // معالجة تسجيل الدخول
        public void ProcessLogin(string username, string password)
        {
            progressBar1.Visible = true;
            try
            {
                var loggedInUser = db.Users
                    .FirstOrDefault(u =>
                        u.Username == username &&
                        u.PasswordHash == password &&
                        u.IsActive == true);

                if (loggedInUser != null)
                {
                    // معالجة الدور بدون خطأ null
                    currentUserRole = loggedInUser.Roles != null
                        ? loggedInUser.Roles.RoleName
                        : "";

                    MessageBox.Show("تم تسجيل الدخول بنجاح");
                    isLoggedIn = false;
                    progressBar1.Visible = false;
                    menuStrip1.Visible = true;

                    foreach (ToolStripMenuItem item in menuStrip1.Items)
                    {
                        item.Enabled = true;
                    }

                    var employee = db.Employees
                        .FirstOrDefault(e => e.UserID == loggedInUser.UserID);

                    string employeeName =
                        employee != null
                        ? employee.FirstName + " " + employee.LastName
                        : loggedInUser.Username;

                    label2.Text = "أهلاً بك : " + employeeName;
                    label2.ForeColor = Color.DarkGreen;

                    btnLogin.Text = "تسجيل الخروج";
                    btnLogin.BackColor = Color.Brown;
                    btnLogin.ForeColor = Color.White;

                    // الصورة الشخصية
                    try
                    {
                        if (!string.IsNullOrEmpty(loggedInUser.ProfileImagePath) &&
                            System.IO.File.Exists(loggedInUser.ProfileImagePath))
                        {
                            picUserProfile.Image =
                                Image.FromFile(loggedInUser.ProfileImagePath);
                        }
                        else
                        {
                            picUserProfile.Image = Properties.Resources.avatar;
                        }
                    }
                    catch
                    {
                        picUserProfile.Image = Properties.Resources.avatar;
                    }

                    picUserProfile.Visible = true;

                    ApplyPermissions(currentUserRole);
                }
                else
                {
                    MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة");

                    txtUsername.Enabled = true;
                    txtPassword.Enabled = true;
                }
            }
            
            catch (Exception ex)
            {
                // 1. صمام الأمان: إلغاء خاصية التخطي تلقائياً لكي تعود الشاشة للعمل عند إعادة التشغيل
                Hospital.Data.Properties.Settings.Default.SkipIPSettings = false;
                Hospital.Data.Properties.Settings.Default.Save(); // تثبيت الإلغاء على القرص

                // 2. تنبيه المستخدم بفشل الاتصال بالخلفية بسبب الـ IP الخاطئ
                MessageBox.Show("فشل الاتصال بقاعدة البيانات! قد يكون عنوان السيرفر (IP) الحالي خاطئاً.\n" +
                                "ستفتح شاشة الإعدادات تلقائياً الآن لتصحيح المسار.\n\n" +
                                "تفاصيل الخطأ الفني: " + ex.Message,
                                "تنبيه: خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 3. الإضافة الحاسمة: إنشاء نسخة من شاشة الإعدادات وفتحها قسرياً في المقدمة وتفعيلها كـ نافذة نشطة
                // تم استخدام اسم الكلاس المباشر والمحمّل بمساره (FINAL1.SettingsForm) لمنع أخطاء الـ Namespace
                FINAL1.settings setForm = new FINAL1.settings();
                setForm.ShowDialog(); // ShowDialog تجعلها نشطة وفي المقدمة ولا يمكن للمستخدم تخطيها إلا بعد التعديل

                // 4. إعادة تفعيل حقول الكتابة في شاشة الدخول ليتمكن المستخدم من المحاولة مجدداً بعد التصحيح وإعادة التشغيل
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
            }

        }

        // بناء الشجرة
        private void ResetAndBuildTreeView()
        {
            treeView1.Nodes.Clear();

            TreeNode root = new TreeNode("نظام المستشفى");

            TreeNode docs = new TreeNode("الأطباء");
            TreeNode staff = new TreeNode("الموظفين");
            TreeNode patients = new TreeNode("المرضى");

            root.Nodes.Add(docs);
            root.Nodes.Add(staff);
            root.Nodes.Add(patients);

            docs.Nodes.Add("السجلات الطبية");

            staff.Nodes.Add("لوحة التحكم");

            patients.Nodes.Add("إدارة المرضى");
            patients.Nodes.Add("المواعيد");

            treeView1.Nodes.Add(root);

            treeView1.ExpandAll();
        }

        // الصلاحيات
        private void ApplyPermissions(string role)
        {
            treeView1.Visible = true;

            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Enabled = true;
            }

            // الأدمن
            if (role == "Admin")
                return;

            // الطبيب
            if (role == "Doctor")
            {
                manageUsersToolStripMenuItem.Enabled = false;
                invoicesToolStripMenuItem.Enabled = false;
                attendanceMonitorToolStripMenuItem.Enabled = false;
            }

            // الموظف
            if (role == "Staff")
            {
                medicalRecordsToolStripMenuItem.Enabled = false;
                manageUsersToolStripMenuItem.Enabled = false;
                attendanceMonitorToolStripMenuItem.Enabled = false;
            }
        }

        // أحداث الشجرة
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Text)
            {
                case "المواعيد":
                    new AppointmentsForm().Show();
                    break;

                case "لوحة التحكم":
                    new AttendanceMonitorForm().Show();
                    break;
            }
        }

        // تغيير لون النص
        private void button1_Click(object sender, EventArgs e)
        {
            colorChooser.FullOpen = true;

            if (colorChooser.ShowDialog() == DialogResult.OK)
            {
                this.ForeColor = colorChooser.Color;
            }
        }

        // تغيير الخلفية
        private void button2_Click(object sender, EventArgs e)
        {
            colorChooser.FullOpen = true;

            if (colorChooser.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = colorChooser.Color;
            }
        }

        // رسم اللوحة
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            FontStyle style1 = FontStyle.Bold;

            Font arial = new Font("Arial", 16, style1);

            SolidBrush textBrush = new SolidBrush(Color.Black);

            SolidBrush fillbrush =
                new SolidBrush(Color.FromArgb(20, 0, 0, 255));

            Graphics graphicsObject = e.Graphics;

            graphicsObject.FillRectangle(
                fillbrush,
                0,
                0,
                panelLogin.Width,
                panelLogin.Height);
        }

        // القوائم
        private void appointmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AppointmentsForm().Show();
        }

        private void attendanceMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AttendanceMonitorForm().Show();
        }

        private void patientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PatientsForm().Show();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ManageUsersForm().Show();
        }

        private void medicalRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MedicalRecordsForm().Show();
        }

        private void invoicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new InvoicesForm().Show();
        }

        private void intelgentFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new intelgentForm().Show();
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new settings().ShowDialog();
        }

       
    }
}