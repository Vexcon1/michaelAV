using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.Permissions;
using System.ComponentModel;
using System.Net;
using System.Text.Json.Nodes;
using System.Text.Json;
using DocumentFormat.OpenXml.Drawing;

namespace MichaelAV
{ 
    public partial class ClientAV : Form
    {

        [Flags]


        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public enum KnownFolder
        {
            Contacts,
            Downloads,
            Favorites,
            Links,
            SavedGames,
            SavedSearches
        }

        public static class KnownFolders
        {
            private static readonly Dictionary<KnownFolder, Guid> _guids = new()
            {
                [KnownFolder.Contacts] = new("56784854-C6CB-462B-8169-88E350ACB882"),
                [KnownFolder.Downloads] = new("374DE290-123F-4565-9164-39C4925E467B"),
                [KnownFolder.Favorites] = new("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
                [KnownFolder.Links] = new("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
                [KnownFolder.SavedGames] = new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
                [KnownFolder.SavedSearches] = new("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
            };

            public static string GetPath(KnownFolder knownFolder)
            {
                return SHGetKnownFolderPath(_guids[knownFolder], 0);
            }

            [DllImport("shell32",
                CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
            private static extern string SHGetKnownFolderPath(
                [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
                nint hToken = 0);
        }

        String mainDir = "./";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        int infections = 0;
        int programs = 0;
        int scanAmount = 0;
        public ClientAV()
        {
            InitializeComponent();
            addLog("System", "Systems loading assets");
            checkSystem();
            addLog("System", "Systems ready");
        }

        private void checkSystem()
        {
            
        }

        #region MD5 File Scanner
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker bw = sender as BackgroundWorker;
            int value = (int)e.Argument;
            // Start the time-consuming operation.
            scanMD5(bw, value);
            addLog("System", "System started scanner.");

            // If the operation was canceled by the user,
            // set the DoWorkEventArgs.Cancel property to true.
            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        // This event handler demonstrates how to interpret
        // the outcome of the asynchronous operation implemented
        // in the DoWork event handler.
        private void backgroundWorker1_RunWorkerCompleted(
            object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("Operation was canceled");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else
            {
                // The operation completed normally.
                string msg = String.Format("Result = {0}", e.Result);
                if (infections >= 1)
                {
                    ClientResults f2 = new ClientResults();
                    f2.ShowDialog(); // Shows Form2
                    addLog("Scan", "Some files need to be handled.");
                } else
                {
                    addLog("Scan", "All files clean.");
                }
                //MessageBox.Show(msg);
            }
        }

        // MD5 Virus Scanner
        public string GetMD5FromFile(string filenPath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filenPath))
                {
                    string sx = "";
                    sx = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
                    Debug.Print(sx);
                    stream.Close();
                    return sx;
                }
            }
        }

        private void doMD5Check(String path)
        {

            if (File.Exists(path))
            {
                // This path is a file
                bool isInfected = false;
                String tbMD5 = GetMD5FromFile(path);
                string[] fileEntries = Directory.GetFiles(".\\MD5");
                Parallel.ForEach(fileEntries, (fileName) =>
                {
                    // var md5signatures = File.ReadAllLines(fileName);
                    const Int32 BufferSize = 128;
                    using (var fileStream = File.OpenRead(fileName))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            if (line.Contains(tbMD5))
                            {
                                isInfected = true;
                                break;
                            }
                        }
                    }

                });
                if (isInfected == true)
                {
                    infections++;
                    addLog("Scan", "Infected: " + path);
                    dbSet("virus" + infections, path);
                    setLabel(virusLabel, "Virus: " + infections);
                    //quarFile(path);
                    stopProcess(path);
                } else
                {
                    scanAmount++;
                    addLog("Scan", "Safe: " + path);
                    setLabel(scanLabel, "Scanned: " + scanAmount);
                }
            } else
            {
                addLog("Scan", "Not a file" + path);
            }
            GC.Collect();
        }

        public async void scanMD5(BackgroundWorker bw, int scanId)
        {
            try
            {
                setLabel(scanLabel, "Scanned: " + scanAmount);
                setLabel(virusLabel, "Virus: " + infections);
                setLabel(procLabel, "Program: " + programs);
            } catch { }
            if (scanId == 1)
            {
                addLog("Scanner Mode", "Now using Quick Scan Mode.");
                String Path = "";
                //Parallel.ForEach(Directory.GetDirectories("C:\\"), (filePath) =>

                var sPath = KnownFolders.GetPath(KnownFolder.Downloads);
                foreach (string filePath in Directory.GetDirectories($"{sPath}"))
                {
                    
                    try
                    {
                        Path = filePath;
                        addLog("Scan", "Using Directory: " + Path);
                        scanMD5File(Path);
                    }

                    catch (Exception es)
                    {
                        addLog("PathNoAccess", "Failed");
                        //MessageBox.Show("ERROR: " + es);
                    }

                }

                sPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                foreach (string filePath in Directory.GetDirectories($"{sPath}"))
                {

                    try
                    {
                        Path = filePath;
                        addLog("Scan", "Using Directory: " + Path);
                        scanMD5File(Path);
                    }

                    catch (Exception es)
                    {
                        addLog("PathNoAccess", "Failed");
                        //MessageBox.Show("ERROR: " + es);
                    }

                }

                sPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                foreach (string filePath in Directory.GetDirectories($"{sPath}"))
                {

                    try
                    {
                        Path = filePath;
                        addLog("Scan", "Using Directory: " + Path);
                        scanMD5File(Path);
                    }

                    catch (Exception es)
                    {
                        addLog("PathNoAccess", "Failed");
                        //MessageBox.Show("ERROR: " + es);
                    }

                }

                sPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                foreach (string filePath in Directory.GetDirectories($"{sPath}"))
                {

                    try
                    {
                        Path = filePath;
                        addLog("Scan", "Using Directory: " + Path);
                        scanMD5File(Path);
                    }

                    catch (Exception es)
                    {
                        addLog("PathNoAccess", "Failed");
                        //MessageBox.Show("ERROR: " + es);
                    }

                }

                sPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                foreach (string filePath in Directory.GetDirectories($"{sPath}"))
                {

                    try
                    {
                        Path = filePath;
                        addLog("Scan", "Using Directory: " + Path);
                        scanMD5File(Path);
                    }

                    catch (Exception es)
                    {
                        addLog("PathNoAccess", "Failed");
                        //MessageBox.Show("ERROR: " + es);
                    }

                }

                sPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                foreach (string filePath in Directory.GetDirectories($"{sPath}"))
                {

                    try
                    {
                        Path = filePath;
                        addLog("Scan", "Using Directory: " + Path);
                        scanMD5File(Path);
                    }

                    catch (Exception es)
                    {
                        addLog("PathNoAccess", "Failed");
                        //MessageBox.Show("ERROR: " + es);
                    }

                }
            }
            if (scanId == 2)
            {
                addLog("Scanner Mode", "Now using Full Scan Mode.");
                String Path = "";
                //Parallel.ForEach(Directory.GetDirectories("C:\\"), (filePath) =>
                foreach (string filePath in Directory.GetDirectories("C:\\"))
                {

                    try
                    {
                        Path = filePath;
                        addLog("Scan", "Using Directory: " + Path);
                        scanMD5File(Path);
                    }

                    catch (Exception es)
                    {
                        addLog("PathNoAccess", "Failed");
                        //MessageBox.Show("ERROR: " + es);
                    }

                }
            }
            if (scanId == 3)
            {
                addLog("Scanner Mode", "Now using File Scan Mode.");

                foreach (String file in openFileDialog1.FileNames)
                {
                    try
                    {
                        addLog("Scan", "Scanning Specific File: " + file);
                        scanMD5File(file);
                    }
                    catch (Exception es)
                    {
                        addLog("PathNoAccess", "Failed");
                        //MessageBox.Show("ERROR: " + es);
                    }
                }
            }
    }

    public void scanMD5File(String path)
        {
            //string[] files = Directory.GetFiles(PathA, "*.*", SearchOption.AllDirectories);
            //foreach(string path in files)



            if (File.Exists(path))
            {
                    doMD5Check(path);
            }
            else if (Directory.Exists(path))
            {
                    // This path is a directory
                    addLog("Scan", "Next Directory " + path);
                    ProcessDirectory(path);
            }
            else
            {
                    addLog("Scan", "is not a valid file or directory. " + path);

            }
        }
        public void ProcessDirectory(string targetDirectory)
        {

            addLog("Scan", "Next Directory " + targetDirectory);


            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories);
            //foreach(string fileName in fileEntries)
            //Parallel.ForEach(fileEntries, new ParallelOptions { MaxDegreeOfParallelism = 5 }, fileName => 
            Debug.Print("TEST", fileEntries);
            Parallel.ForEach(fileEntries, new ParallelOptions { MaxDegreeOfParallelism = 35 }, fileName =>
            {
                Debug.Print(fileName);
                ProcessFile(fileName);
            });

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            //Parallel.ForEach(subdirectoryEntries, (subdirectory) =>
            foreach(string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory);
            }
        }

        public void ProcessFile(string path)
        {
            doMD5Check(path);
        }
        #endregion

        #region File deletion and Process stopper
        private static void SuspendProcess(int pid)
        {
            var process = Process.GetProcessById(pid); // throws exception if process does not exist

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(int pid)
        {
            var process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }

        public void deleteFile(String path)
        {
            Debug.WriteLine(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
            string filename = path;
            // delete file
            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                else
                {
                    Debug.WriteLine("File does not exist.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            // kill process
            try
            {
                Process[] processCollection = Process.GetProcesses();
                foreach (Process process in processCollection)
                {

                    String fileName = process.MainModule.FileName;
                    if (fileName == filename)
                    {
                        process.Kill();
                    }
                }

            }
            catch { }
        }

        private void quarFile(String path)
        {
            string filename = path;
            string fname = System.IO.Path.GetFileName(path);
            // delete file
            try
            {
                if (File.Exists(filename))
                {
                    File.Move(filename,".\\quarateen\\");
                    addLog("Quaranteen", "Quaranteen File: " + path);
                }
                else
                {
                    Debug.WriteLine("File does not exist.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            // kill process
            try
            {
                Process[] processCollection = Process.GetProcesses();
                foreach (Process process in processCollection)
                {

                    String fileName = process.MainModule.FileName;
                    if (fileName == filename)
                    {
                        process.Kill();
                    }
                }

            }
            catch { }

            try
            {
                if (File.Exists(filename))
                {
                    File.Move(filename, ".\\quarateen\\");
                    addLog("Quaranteen", "Quaranteen File: " + path);
                }
                else
                {
                    Debug.WriteLine("File does not exist.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void stopProcess(string filename)
        {
            try
            {
                Process[] processCollection = Process.GetProcesses();
                foreach (Process process in processCollection)
                {

                    String fileName = process.MainModule.FileName;
                    if (fileName == filename)
                    {
                        process.Kill();
                        addLog("Process", "Successfully Terminated: " + fileName);
                    }
                }

            }
            catch { }
        }

        #endregion

        #region Label and logging
        delegate void SetTextCallback(String operato, String str);

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
        public async void addLog(String operato, String str)
        {
            try {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.logBox.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(addLog);
                    this.Invoke(d, new object[] { operato, str });
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    logBox.AppendText("\n");
                    logBox.AppendText($"[{dt.ToString()} - {operato}] {str}");
                    
                }
            } catch(Exception)
            {

            }
            }
        #endregion

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
                        Debug.Print($"Stuff: {data}");
                        if (data == target)
                        {
                            str = parts[i + 1];
                        }
                    }

                }

                
            } else
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

        #region Panel & UI functions
        // panel movement
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void logo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void logo_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void closeBTN_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void minusBTN_Click(object sender, EventArgs e)
        {
                Hide();
                notifyIcon.Visible = true;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void ScanBTN_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync(1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync(2);
            }
        }


        

        private void ClientAV_Load(object sender, EventArgs e)
        {
            
        }

        private void scanFileBTN_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync(3);
            }
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void Results_Click(object sender, EventArgs e)
        {
            ClientResults f2 = new ClientResults();
            f2.ShowDialog(); // Shows Form2
        }

        #endregion
    }
}