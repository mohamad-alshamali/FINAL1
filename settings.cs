using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FINAL1
{
    public partial class settings : Form
    {
       

        public settings()
        {
            InitializeComponent();
        }



        

        private void settings_Load(object sender, EventArgs e)
        {
            txtServerIP.Text = Hospital.Data.Properties.Settings.Default.ServerIP;
        }
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                string inputIP = txtServerIP.Text.Trim();
                if (string.IsNullOrEmpty(inputIP))
                {
                    MessageBox.Show("الرجاء إدخال عنوان IP صحيح أو نقطة (.) للسيرفر المحلي!");
                    return;
                }

                // 1. حفظ البيانات الجديدة في ملف الخصائص على القرص
                Hospital.Data.Properties.Settings.Default.ServerIP = inputIP;
                Hospital.Data.Properties.Settings.Default.SkipIPSettings = chkHideSettings.Checked;
                Hospital.Data.Properties.Settings.Default.Save(); // تثبيت قسري

                // 2. رسالة إرشادية وتنبيهية ذكية للطبيب بضرورة إعادة تشغيل البرنامج لتطبيق المسار
                MessageBox.Show("تم حفظ إعدادات السيرفر بنجاح!\n" +
                                "سيتم إغلاق التطبيق تلقائياً الآن. الرجاء إعادة تشغيله ليعمل بالاتصال الجديد.",
                                "إعادة تشغيل النظام", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. الأمر الهندسي لإغلاق التطبيق بالكامل وبكافة نوافذه وخروجه من الذاكرة (الرام)
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء حفظ الإعدادات: " + ex.Message);
            }
        }

        private void chkHideSettings_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
