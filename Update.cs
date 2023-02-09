
using Microsoft.Win32;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace MichaelAV
{
    public partial class Update : Form
    {

        public class defaultSettings
        {
            public string? status { get; set; }
            public string? verison { get; set; }
            public string? mdVerison { get; set; }
            public string? url { get; set; }
            public int mdInt { get; set; }
        }

        Uri siteMD1 = new Uri("https://drive.google.com/u/6/uc?id=1QcIv7k2Zt14c8ocv60RqK7VEq05lwXuX&export=download&confirm=t&uuid=32f92e61-b492-49d2-a2af-1bccf231e39e&at=ACjLJWn6jkhEGE10sFCx0XqhlR1X:1674513417064");
        Uri siteMD2 = new Uri("https://drive.google.com/u/6/uc?id=10Qrh-L5UWOB_-wh1nTXpE13Qk-ZWYOqL&export=download&confirm=t&uuid=d3be6c24-1c56-4d41-9827-217828bcee48&at=ACjLJWkzqIbikZrri645cgd1GqaX:1674513490740");
        Uri siteMD3 = new Uri("https://drive.google.com/u/6/uc?id=1IyCZiaJiUgoYFd235eV0QFIhF_RotjW5&export=download&confirm=t&uuid=361b082c-a913-4b79-a363-5002db57d7ce&at=ACjLJWmCSwW_zEaXF35ZnLywEE8S:1674513503570");

        String mainDir = "./";

        defaultSettings defSettings = new defaultSettings();
        private bool downloadComplete = false;
        public Update()
        {
            InitializeComponent();
        }

        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        public async Task checkSystem()
        {
            setLabel(statusLbl,"Checking File System...");
            addLog("Looking for MD5 Folder ");
            string folderName = @".\MD5";
            // If directory does not exist, create it
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
                addLog("Found.");
            }
            addLog("Looking for Quaranteen Folder ");
            if (!Directory.Exists(".\\Quaranteen"))
            {
                Directory.CreateDirectory(".\\Quaranteen");
                addLog("Found.");
            }
            if (!File.Exists(".\\michael.txt"))
            {
                File.CreateText(".\\michael.txt");

                var currentUser = "a";

                using (System.IO.StreamReader objReader = new System.IO.StreamReader(".\\michael.txt"))
                {
                    currentUser = objReader.ReadLine();
                }

                if (currentUser == null)
                {
                    File.WriteAllText(".\\michael.txt", "system; ready \nstartupSet; true \nmainVerison; 1.0.0.0 \nmdVerison; 1.0.0.0 \nmdInt; 1");
                }


            }
            if (!File.Exists(".\\MD5\\MD5base1.md5"))
            {
                try
                {
                    progressBar_MarqueeAnimationSpeed(0);
                    using (WebClient webClient = new WebClient())
                    {
                        setLabel(statusLbl, "Downloading Assets...");
                        addLog("Downloading MD5");
                        addLog("This might take a while, please leave PC on.");
                        webClient.DownloadProgressChanged += (s, e) =>
                        {
                            ProgressBar_StatusChange(e.ProgressPercentage);
                        };
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        webClient.DownloadFileAsync(siteMD1, @".\MD5\MD5base1.md5");
                    }
                    while (!downloadComplete)
                    {
                        Application.DoEvents();
                    }

                    downloadComplete = false;
                    //Thread.Sleep(3000);
                }
                catch(Exception e) {
                    MessageBox.Show("" + e);
                    Application.DoEvents();
                }
                try
                {
                    progressBar_MarqueeAnimationSpeed(0);
                    using (WebClient webClient = new WebClient())
                    {
                        setLabel(statusLbl, "Downloading Assets...");
                        addLog("Downloading MD5-2");
                        webClient.DownloadProgressChanged += (s, e) =>
                        {
                            ProgressBar_StatusChange(e.ProgressPercentage);
                        };
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        webClient.DownloadFileAsync(siteMD2, @".\MD5\MD5base2.md5");
                    }
                    while (!downloadComplete)
                    {
                        Application.DoEvents();
                    }

                    downloadComplete = false;
                    //Thread.Sleep(3000);
                }
                catch (Exception e)
                {
                    MessageBox.Show("" + e);
                    Application.DoEvents();
                }
                try
                {
                    progressBar_MarqueeAnimationSpeed(0);
                    using (WebClient webClient = new WebClient())
                    {
                        setLabel(statusLbl, "Downloading Assets...");
                        addLog("Downloading MD5-3");
                        webClient.DownloadProgressChanged += (s, e) =>
                        {
                            ProgressBar_StatusChange(e.ProgressPercentage);
                        };
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        webClient.DownloadFileAsync(siteMD3, @".\MD5\MD5base3.md5");
                    }
                    while (!downloadComplete)
                    {
                        Application.DoEvents();
                    }

                    downloadComplete = false;
                    //Thread.Sleep(3000);
                }
                catch (Exception e)
                {
                    MessageBox.Show("" + e);
                    Application.DoEvents();
                }
                addLog("Finished.");
            } else
            {
                addLog("Found.");
            }
            

            progressBar_MarqueeAnimationSpeed(1);

            addLog("Checking Verison...");
            if (dbGet("startupSet") != "true")
            {
                SetStartup();
                dbSet("startupSet", "true");
            }

            Debug.WriteLine("problem here.");

            defSettings.verison = dbGet("mainVerison");
            defSettings.mdVerison = dbGet("mdVerison");
            try
            {
                defSettings.mdInt = int.Parse(dbGet("mdInt"));
            } catch { }
            defSettings.url = "";

            addLog("Verison Set.");

            updateEssentials();
        }

        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            rk.SetValue("MichaelAV", System.Windows.Forms.Application.ExecutablePath);

        }

        private async Task updateEssentials()
        {
            Uri site = new Uri("https://michaelavserver.xdpro.repl.co/API/wake");
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    String json = webClient.DownloadString("https://michaelavserver.xdpro.repl.co/API/verison");
                    defaultSettings deptObj = JsonSerializer.Deserialize<defaultSettings>(json);

                    if (json != null && deptObj != null && deptObj.status == "true")
                    {
                        Debug.Print(deptObj.verison, defSettings.verison);
                        if (deptObj.verison != defSettings.verison)
                        {
                            string result = Path.GetTempPath();
                            Uri siteD = new Uri($"https://michaelavserver.xdpro.repl.co/API/update");
                            addLog("Updating to core verison");
                            progressBar_MarqueeAnimationSpeed(0);
                            using (WebClient webClientX = new WebClient())
                            {
                                setLabel(statusLbl, "Updating Assets...");
                                webClient.DownloadProgressChanged += (s, e) =>
                                {
                                    ProgressBar_StatusChange(e.ProgressPercentage);
                                };
                                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                webClient.DownloadFileAsync(siteD, $"{result}\\MichaelAV.zip");
                            }
                            while (!downloadComplete)
                            {
                                Application.DoEvents();
                            }

                            downloadComplete = false;

                            Process.Start(@".\update.bat");
                            Application.Exit();
                        }
                        else if (deptObj.mdVerison != defSettings.mdVerison)
                        {
                            defSettings.mdInt = deptObj.mdInt;
                            Uri siteD = new Uri($"{deptObj.url}");
                            addLog("Updating to latest MD5.");
                            progressBar_MarqueeAnimationSpeed(0);
                            using (WebClient webClientX = new WebClient())
                            {
                                setLabel(statusLbl, "Updating Assets...");
                                webClient.DownloadProgressChanged += (s, e) =>
                                {
                                    ProgressBar_StatusChange(e.ProgressPercentage);
                                };
                                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                webClient.DownloadFileAsync(siteD, $".\\MD5\\MD5base{defSettings.mdInt}.md5");
                            }
                            while (!downloadComplete)
                            {
                                Application.DoEvents();
                            }

                            downloadComplete = false;
                        }
                        else
                        {
                            addLog("MD5 database is up to date.");
                        }


                        defSettings.status = deptObj.status;
                        defSettings.verison = deptObj.verison;
                        defSettings.mdVerison = deptObj.mdVerison;
                        defSettings.mdInt = deptObj.mdInt;

                        dbSet("mainVerison", deptObj.verison);
                        dbSet("mdVerison", deptObj.mdVerison);
                        dbSet("mdInt", deptObj.mdInt.ToString());

                        setLabel(statusLbl, "Finished...");
                        addLog("Finishing up...");
                        Thread.Sleep(2000);
                    }
                    //webClient.DownloadFile("http://mysite.com/myfile.txt", @"c:\myfile.txt");
                }
                catch (Exception e)
                {
                    MessageBox.Show("" + e);
                    Application.DoEvents();
                }
            }
        }

        delegate void SetTextCallbackX(Label label, String str);

        public async void setLabel(Label label, String str)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (label.InvokeRequired)
                {
                    SetTextCallbackX d = new SetTextCallbackX(setLabel);
                    this.Invoke(d, new object[] { label, str });
                }
                else
                {
                    label.Text = str;

                }
            }
            catch (Exception)
            {

            }
        }
        delegate void SetTextCallback(String str);
        public async void addLog(String str)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.logBox.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(addLog);
                    this.Invoke(d, new object[] { str });
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    logBox.AppendText("\n");
                    logBox.AppendText($"[{dt.ToString()}] {str}");

                }
            }
            catch (Exception)
            {

            }
        }

        #region database crap
        // database

        private void dbSet(String target, String Value)
        {
            if (File.Exists($"{mainDir}/michael.txt"))
            {
                string FilePath = $"{mainDir}/michael.txt";
                string Contents = File.ReadAllText(FilePath);
                String text = "";
                if (Contents.Contains(target))
                {
                    foreach (string line in File.ReadLines(FilePath))
                    {
                        if (line.Contains(target))
                        {
                            text = Contents.Replace(line, $"{target};{Value}");
                            break;
                        }
                    }
                    File.WriteAllText(FilePath, text);
                }
                else
                {
                    File.WriteAllText(FilePath, Contents + "\n" + $"{target};{Value}");
                }
            }
            else
            {
                using (FileStream fs = File.Create($"{mainDir}/michael.txt"))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes($"{Value}");
                    fs.Write(title, 0, title.Length);
                }
            }
        }

        private string dbGet(String target)
        {
            String str = "";
            if (File.Exists($"{mainDir}/michael.txt"))
            {

                List<string> productlines = File.ReadAllLines("michael.txt").ToList();

                //Remove headers
                productlines.RemoveAt(0);

                foreach (string line in productlines)
                {
                    string[] parts = line.Split(';');

                    for (int i = 0; i < parts.Length; i++)
                    {
                        String data = parts[i];
                        if (data == target)
                        {
                            str = parts[i + 1];
                        }
                    }

                }


            }
            else
            {
                using (FileStream fs = File.Create($"{mainDir}/michael.txt"))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes($"system;ready");
                    fs.Write(title, 0, title.Length);
                }
            }
            return str;
        }

        #endregion


        delegate void ProgressBar_StatusChangedCallback(int am);

        public async void ProgressBar_StatusChange(int am)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (progressBar.InvokeRequired)
                {
                    ProgressBar_StatusChangedCallback d = new ProgressBar_StatusChangedCallback(ProgressBar_StatusChange);
                    this.Invoke(d, new object[] { am });
                }
                else
                {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Minimum = 0;
                    progressBar.Maximum = 100;
                    progressBar.Value = am;

                }
            }
            catch (Exception)
            {

            }
        }

        delegate void progressBar_MarqueeAnimationSpeedCallback(int am);

        public async void progressBar_MarqueeAnimationSpeed(int am)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (progressBar.InvokeRequired)
                {
                    progressBar_MarqueeAnimationSpeedCallback d = new progressBar_MarqueeAnimationSpeedCallback(progressBar_MarqueeAnimationSpeed);
                    this.Invoke(d, new object[] { am });
                }
                else
                {
                    if (am == 1)
                    {
                        progressBar.Style = ProgressBarStyle.Marquee;
                        progressBar.MarqueeAnimationSpeed = am;
                    } else
                    {
                        progressBar.Style = ProgressBarStyle.Blocks;
                        progressBar.Value = 5;
                    }

                }
            }
            catch (Exception)
            {

            }
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
          downloadComplete = true;
        }
        private void Update_Load(object sender, EventArgs e)
        {
            updateWorker.RunWorkerAsync();
        }

        private void updateWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            checkSystem();
        }

        private void updateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
            var form2 = new ClientAV();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }
    }


}