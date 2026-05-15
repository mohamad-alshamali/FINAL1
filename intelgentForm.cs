using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;



// البديل الرسمي المدمج في الإصدارات الحديثة




namespace FINAL1
{
    public partial class intelgentForm : Form
    {
        public intelgentForm()
        {
            InitializeComponent();
        }

        private void btnDiagnose_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        // مصفوفة عامة لتخزين وإدارة كافة عمليات بايثون المشغلة
        private List<Process> pythonServerProcesses = new List<Process>();
        
            



        private void intelgentForm_Load(object sender, EventArgs e)
        {
            // قائمة بأسماء كافة ملفات الفحوصات الطبية المخصصة للمشفى
            string[] medicalScripts = { "Creatinine.py", "sugar.py", "LiverFunctions.py", "LipidPanel.py" };

            string pythonPath = @"C:\Users\as\AppData\Local\Programs\Python\Python312\python.exe";

            foreach (string scriptName in medicalScripts)
            {
                string scriptPath = Path.Combine(Application.StartupPath, scriptName);

                if (File.Exists(scriptPath))
                {
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = pythonPath;
                        startInfo.Arguments = $"\"{scriptPath}\"";
                        startInfo.UseShellExecute = false;
                        startInfo.CreateNoWindow = true; // إخفاء الشاشة السوداء تماماً عن الطبيب
                        startInfo.WorkingDirectory = Application.StartupPath;

                        Process process = new Process();
                        process.StartInfo = startInfo;
                        process.Start();

                        // إضافة العملية إلى القائمة لمراقبتها وإغلاقها لاحقاً
                        pythonServerProcesses.Add(process);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"فشل تشغيل المحرك الطبي الخاص بـ {scriptName}: {ex.Message}", "خطأ في النظام");
                    }
                }
            }
        }

        private void intelligentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // إنهاء جماعي لكافة خوادم بايثون المخفية لضمان سلامة موارد سيرفر المشفى
            foreach (Process process in pythonServerProcesses)
            {
                if (process != null && !process.HasExited)
                {
                    process.Kill();
                    process.Dispose();
                }
            }
        }


        private async void btnDiagnose_Click_1(object sender, EventArgs e)
        {
            try
            {
                // 1. التحقق من ملء الخانات قبل الإرسال لمنع أخطاء الخادم
                if (string.IsNullOrWhiteSpace(txtSugar.Text) || string.IsNullOrWhiteSpace(txtHemoglobin.Text))
                {
                    MessageBox.Show("يرجى إدخال قيم التحاليل المطلوبة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                HttpClient client = new HttpClient();

                // 2. صياغة الـ JSON يدوياً بدقة متوافقة مع مفاتيح ملف sugar.py الحالي (glucose و hemoglobin)
                string json =
                    "{ " +
                    "\"glucose\": \"" + txtSugar.Text.Trim() + "\"," +
                    "\"hemoglobin\": \"" + txtHemoglobin.Text.Trim() + "\"" +
                    "}";

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                // 3. إرسال الطلب الرقمي إلى المنفذ الصحيح 5001 والمسار /analyze
                HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5001/analyze", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    // 4. استخراج التشخيص الطبي النظيف يدوياً لمنع تداخل أسطر الـ JSON
                    string searchKey = "\"diagnosis\":";
                    int startIndex = result.IndexOf(searchKey);

                    if (startIndex != -1)
                    {
                        startIndex += searchKey.Length;
                        string rawDiagnosis = result.Substring(startIndex)
                                                        .Replace("{", "")
                                                        .Replace("}", "")
                                                        .Replace("\"", "")
                                                        .Replace("[", "")
                                                        .Replace("]", "")
                                                        .Trim();

                        // 5. فك تشفير رموز اليونيكود لعرض الكلمات العربية الصريحة والنظيفة
                        string cleanDiagnosis = System.Text.RegularExpressions.Regex.Unescape(rawDiagnosis);

                        // عرض النتيجة النهائية المنقحة في مربع النص
                        richTextBox1.Text = "التشخيص الأولي للمحرك: " + cleanDiagnosis;
                    }
                    else
                    {
                        richTextBox1.Text = result; // في حال حدوث خطأ استثنائي، يتم طباعة المخرجات الخام للمراجعة
                    }
                }
                else
                {
                    MessageBox.Show("فشل خادم بايثون في معالجة الطلب، تأكد من سلامة الكود والخادم.", "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"تأكد من تشغيل خادم بايثون (sugar.py) في الخلفية أولاً.\nالتفاصيل: {ex.Message}", "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnCheckKidney_Click(object sender, EventArgs e)
        {
       
            try
            {
                if (string.IsNullOrWhiteSpace(txtSugar.Text) ||
                    string.IsNullOrWhiteSpace(txtHemoglobin.Text) ||
                    string.IsNullOrWhiteSpace(txtCreatinine.Text))
                {
                    MessageBox.Show("يرجى إدخال قيم جميع التحاليل المطلوبة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sugar = txtSugar.Text.Trim();
                string hemo = txtHemoglobin.Text.Trim();
                string creatinine = txtCreatinine.Text.Trim();

                // 1. صياغة نص الـ JSON يدوياً دون الحاجة لأي مكتبة خارجية
                string jsonPayload = $"{{\"Blood_Sugar\":{sugar},\"Hemoglobin\":{hemo},\"Creatinine\":{creatinine}}}";
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    // 2. إرسال الطلب الرقمي إلى خادم بايثون (تأكد من تشغيل Creatinine.py أولاً)

                    // السطر المحدث والمصحح بالكامل لحل مشكلة الـ URI
                    HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/analyze", content);


                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        // 3. تفكيك النص المستلم يدوياً لاستخراج نتيجة التشخيص الأولي
                        // النص المستلم يكون هكذا: {"prediction": "سليم"}
                        string searchKey = "\"prediction\":";
                        int startIndex = jsonResponse.IndexOf(searchKey);

                        if (startIndex != -1)
                        {
                            startIndex += searchKey.Length;
                            string prediction = jsonResponse.Substring(startIndex)
                                                            .Replace("{", "")
                                                            .Replace("}", "")
                                                            .Replace("\"", "")
                                                            .Replace("[", "")
                                                            .Replace("]", "")
                                                            .Trim();

                            // عرض التشخيص النظيف مباشرة في واجهة المشفى
                            // فك تشفير رموز اليونيكود وإظهار النص العربي الصريح في الواجهة
                            richTextBox1.Text = "التشخيص الأولي للمحرك: " + System.Text.RegularExpressions.Regex.Unescape(prediction);

                        }
                    }
                    else
                    {
                        MessageBox.Show("فشل الاتصال بمحرك بايثون (Creatinine.py).", "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("يرجى إدخال أرقام صحيحة فقط.", "خطأ في المدخلات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"تأكد من تشغيل خادم بايثون أولاً.\nالتفاصيل: {ex.Message}", "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        
            private async void btnLiver_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtALT.Text) || string.IsNullOrWhiteSpace(txtAST.Text))
                {
                    MessageBox.Show("يرجى إدخال قيم إنزيمات الكبد ALT و AST أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                HttpClient client = new HttpClient();
                string json = $"{{\"alt\":\"{txtALT.Text.Trim()}\",\"ast\":\"{txtAST.Text.Trim()}\"}}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // الإرسال إلى المنفذ 5002 الخاص بوظائف الكبد
                
                HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5002/analyze", content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    string searchKey = "\"diagnosis\":";
                    int startIndex = result.IndexOf(searchKey);

                    if (startIndex != -1)
                    {
                        startIndex += searchKey.Length;
                        string rawDiagnosis = result.Substring(startIndex).Replace("{", "").Replace("}", "").Replace("\"", "").Trim();
                        richTextBox1.Text = "تحليل الكبد الاسترشادي: " + System.Text.RegularExpressions.Regex.Unescape(rawDiagnosis);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void btnLipid_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtChol.Text) || string.IsNullOrWhiteSpace(txtTrig.Text))
                {
                    MessageBox.Show("يرجى إدخال قيم الكوليسترول والدهون الثلاثية أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                HttpClient client = new HttpClient();
                string json = $"{{\"cholesterol\":\"{txtChol.Text.Trim()}\",\"triglycerides\":\"{txtTrig.Text.Trim()}\"}}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // الإرسال إلى المنفذ 5003 الخاص بلوحة الدهون
                HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5003/analyze", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    string searchKey = "\"diagnosis\":";
                    int startIndex = result.IndexOf(searchKey);

                    if (startIndex != -1)
                    {
                        startIndex += searchKey.Length;
                        string rawDiagnosis = result.Substring(startIndex).Replace("{", "").Replace("}", "").Replace("\"", "").Trim();
                        richTextBox1.Text = "تحليل الدهون الاسترشادي: " + System.Text.RegularExpressions.Regex.Unescape(rawDiagnosis);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

    }
}


   


