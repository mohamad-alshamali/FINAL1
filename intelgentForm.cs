using Hospital.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



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
            // 🌟 تم إزالة bone_analyzer.py من هنا لأنه ليس خادم مستمر، بل أداة تُستدعى عند ضغط الزر فقط
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
                        startInfo.CreateNoWindow = true;
                        startInfo.WorkingDirectory = Application.StartupPath;

                        Process process = new Process();
                        process.StartInfo = startInfo;
                        process.Start();

                        pythonServerProcesses.Add(process);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"فشل تشغيل المحرك الطبي الخاص بـ {scriptName}: {ex.Message}", "خطأ في النظام");
                    }
                }
            }
            try
            {
                using (var db = new HospitalDBEntities())
                {
                    // 1. جلب قائمة المرضى وتعبئة الـ ComboBox الخاص بهم (اسمه cmbPatients)
                    var patientsList = db.Patients
                                         .Select(p => new { p.PatientID, p.FullName })
                                         .ToList();

                    cmbPatients.DataSource = patientsList;
                    cmbPatients.DisplayMember = "FullName";   // الاسم الذي يراه الطبيب على الشاشة
                    cmbPatients.ValueMember = "PatientID";    // الرقم الحقيقي المخفي الذي نستخدمه في الكود

                    // 2. جلب قائمة الأطباء وتعبئة الـ ComboBox الخاص بهم (اسمه cmbDoctors)
                    var doctorsList = db.Employees
                                        .Select(e => new { e.EmployeeID, FullName = e.FirstName + " " + e.LastName })
                                        .ToList();

                    cmbDoctors.DataSource = doctorsList;
                    cmbDoctors.DisplayMember = "FullName";    // اسم الطبيب الظاهر
                    cmbDoctors.ValueMember = "EmployeeID";    // رقم الموظف المخفي لربط العلاقات
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل تحميل قوائم الأطباء والمرضى: {ex.Message}", "خطأ في النظام");
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



        private void btnCheckFracture_Click(object sender, EventArgs e)
        {
            // 1. التحقق من اختيار طبيب ومريض من القوائم المنسدلة
            if (cmbPatients.SelectedValue == null || cmbDoctors.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار المريض والطبيب من القوائم المنسدلة أولاً قبل الفحص.", "نقص بيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                string pythonScriptPath = Path.Combine(Application.StartupPath, "bone_analyzer.py");

                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = @"C:\Users\as\AppData\Local\Programs\Python\Python312\python.exe";
                start.Arguments = $"-u \"{pythonScriptPath}\" \"{selectedImagePath}\"";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.StandardOutputEncoding = Encoding.UTF8;

                try
                {
                    string rawResult = "";
                    string errorResult = "";

                    using (Process process = Process.Start(start))
                    {
                        rawResult = process.StandardOutput.ReadToEnd().Trim();
                        errorResult = process.StandardError.ReadToEnd().Trim();
                        process.WaitForExit();
                    }

                    if (!string.IsNullOrEmpty(errorResult))
                    {
                        MessageBox.Show($"خطأ في محرك بايثون:\n{errorResult}", "فشل التحليل", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // تفسير النتيجة القادمة من بايثون
                    string finalMessage = "";
                    switch (rawResult)
                    {
                        case "Fracture_Detected":
                            finalMessage = "🚨 التشخيص: تم رصد انقطاع حاد وتفتت في الحواف. مؤشر قوي على وجود كسر حديث.";
                            break;
                        case "Bone_Healing":
                            finalMessage = "🟡 مرحلة الشفاء: تظهر حواف كثيفة وبناء نسيجي. العظم في مرحلة الالتئام النشط.";
                            break;
                        case "Normal_Bone":
                            finalMessage = "✅ النتيجة: استمرارية العظم ممتازة والكثافة عالية. العظم سليم أو تم الشفاء بنجاح.";
                            break;
                        case "Low_Density":
                            finalMessage = "📉 تنبيه: لا توجد كسور واضحة، ولكن كثافة العظام منخفضة نسبياً.";
                            break;
                        default:
                            finalMessage = $"⚠️ استجابة غير معرفة: {rawResult}";
                            break;
                    }

                    // 🌟 الحفظ التلقائي المباشر باستخدام الـ ID المأخوذ من الـ ComboBox 🌟
                    using (var db = new HospitalDBEntities())
                    {
                        MedicalRecords newRecord = new MedicalRecords();

                        // سحب الأرقام مباشرة من الـ SelectedValue للقوائم المنسدلة
                        newRecord.PatientID = Convert.ToInt32(cmbPatients.SelectedValue);
                        newRecord.DoctorID = Convert.ToInt32(cmbDoctors.SelectedValue);

                        newRecord.VisitDate = DateTime.Now;
                        newRecord.Diagnosis = "فحص أشعة العظام الذكي: " + finalMessage;
                        newRecord.Prescription = "متابعة طبية سريرية مطابقة للتقرير الشامل.";

                        db.MedicalRecords.Add(newRecord);
                        db.SaveChanges();
                    }

                    MessageBox.Show(finalMessage + "\n\n💾 تم حفظ وتوثيق التقرير بنجاح لملف المريض المحدد.", "نجاح العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"فشل بدء عملية الفحص التقني: {ex.Message}", "خطأ نظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCheckBrainTumor_Click(object sender, EventArgs e)
        {
            // 1. التحقق من اختيار طبيب ومريض من القوائم المنسدلة
            if (cmbPatients.SelectedValue == null || cmbDoctors.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار المريض والطبيب من القوائم المنسدلة أولاً قبل فحص الرنين.", "نقص بيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MRI Images|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                string pythonScriptPath = Path.Combine(Application.StartupPath, "tumor_segmentation.py");

                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = @"C:\Users\as\AppData\Local\Programs\Python\Python312\python.exe";
                start.Arguments = $"-u \"{pythonScriptPath}\" \"{selectedImagePath}\"";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.StandardOutputEncoding = Encoding.UTF8;

                try
                {
                    string rawResult = "";
                    string errorResult = "";

                    using (Process process = Process.Start(start))
                    {
                        rawResult = process.StandardOutput.ReadToEnd().Trim();
                        errorResult = process.StandardError.ReadToEnd().Trim();
                        process.WaitForExit();
                    }

                    if (!string.IsNullOrEmpty(errorResult))
                    {
                        MessageBox.Show($"خطأ محرك الأشعة:\n{errorResult}", "فشل الفحص", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string finalMessage = "";
                    if (rawResult.StartsWith("Tumor_Detected"))
                    {
                        string[] pixelSize = rawResult.Split(':');
                        finalMessage = $"🚨 خطير: تم رصد كتلة نسيجية غير طبيعية (ورم محتمل) داخل تجويف الدماغ.\n📦 الحجم السطحي للكتلة: {pixelSize[1]} بكسل.";
                    }
                    else if (rawResult == "Clear_Brain")
                    {
                        finalMessage = "✅ النتيجة: صورة الرنين المغناطيسي سليمة تماماً، وتظهر توزيعاً طبيعياً ولا توجد مؤشرات لأورام.";
                    }
                    else
                    {
                        finalMessage = $"⚠️ تنبيه: رد غير متوقع من المحرك: {rawResult}";
                    }

                    // 🌟 الحفظ التلقائي في السجلات الطبية باستخدام الـ ID المأخوذ من الـ ComboBox 🌟
                    using (var db = new HospitalDBEntities())
                    {
                        MedicalRecords newRecord = new MedicalRecords();

                        // سحب المعرفات مباشرة دون وسطاء نصيين
                        newRecord.PatientID = Convert.ToInt32(cmbPatients.SelectedValue);
                        newRecord.DoctorID = Convert.ToInt32(cmbDoctors.SelectedValue);

                        newRecord.VisitDate = DateTime.Now;
                        newRecord.Diagnosis = "تحليل الرنين المغناطيسي: " + finalMessage;
                        newRecord.Prescription = rawResult.StartsWith("Tumor_Detected") ? "تحويل فوري لقسم جراحة الأعصاب لعقد لجنة طبية." : "لا يوجد.";

                        db.MedicalRecords.Add(newRecord);
                        db.SaveChanges();
                    }

                    MessageBox.Show(finalMessage + "\n\n💾 تم أرشفة تقرير الرنين المغناطيسي بنجاح.", "نجاح الأرشفة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"فشل بدء المعالجة النسيجية: {ex.Message}", "خطأ نظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}



   


