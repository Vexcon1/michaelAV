using Microsoft.Win32;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace MichaelAV
{
    public partial class ClientResults : Form
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
        public class defaultSettings
        {
            public string? status { get; set; }
            public string? verison { get; set; }
            public string? mdVerison { get; set; }
        }

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
        Boolean allCheck = false;

        defaultSettings defSettings = new defaultSettings();
        public ClientResults()
        {
            InitializeComponent();
            checkSystem();
            showPending();
        }

        public void checkSystem()
        {
            if (!File.Exists(".\\MD5base.md5"))
            {
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile("https://michaelavserver.xdpro.repl.co/API/MD5base.md5", @".\MD5baseX.md5");
                    }
                } catch { }
            }
            if (dbGet("startupSet") != "true") {
                SetStartup();
                dbSet("startupSet", "true");
            }

            defSettings.verison = "1.0.0.0";
            defSettings.mdVerison = "1.0.0.0";

            updateEssentials();
        }

        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            rk.SetValue("MichaelAV", System.Windows.Forms.Application.ExecutablePath);

        }

        private async void updateEssentials()
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

                        if (deptObj.mdVerison != defSettings.verison)
                        {
                            webClient.DownloadFile("https://michaelavserver.xdpro.repl.co/API/MD5base.md5", @".\MD5base.md5");
                        }


                        Debug.Print(deptObj.verison);
                        defSettings.status = deptObj.status;
                        defSettings.verison = deptObj.verison;
                        defSettings.mdVerison = deptObj.mdVerison;

                        dbSet("mainVerison", deptObj.verison);
                        dbSet("mdVerison", deptObj.mdVerison);
                    }
                    //webClient.DownloadFile("http://mysite.com/myfile.txt", @"c:\myfile.txt");
                } catch (Exception) { }
            }
        }

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

        #endregion

        private void showPending()
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
                    if (data.Contains("virus"))
                    {
                        checkList.Items.Add("Virus - " + parts[i + 1], CheckState.Unchecked);
                    }
                }

            }

        }

        private void deleteAllSelected()
        {
                foreach (int indexChecked in checkList.CheckedIndices)
                {
                Debug.WriteLine(checkList.GetItemCheckState(indexChecked).ToString());
                if (checkList.GetItemCheckState(indexChecked).ToString() == "Checked")
                {
                    String path = checkList.Items[indexChecked].ToString().Substring(8);
                    checkList.Items.Remove(checkList.Items[indexChecked].ToString());
                    MessageBox.Show(path);
                    deleteFile(path);

                    List<string> productlines = File.ReadAllLines("michael.txt").ToList();
                    string FilePath = $"{mainDir}/michael.txt";
                    string Contents = File.ReadAllText(FilePath);
                    String text = Contents;

                    //Remove headers
                    productlines.RemoveAt(0);

                    foreach (string line in productlines)
                    {
                        string[] parts = line.Split(';');

                        for (int i = 0; i < parts.Length; i++)
                        {
                            String data = parts[i];
                            if (data.Contains("virus"))
                            {
                                if (parts[i + 1] == path)
                                {
                                    text = Contents.Replace(line, "");
                                }
                            }
                        }

                    }
                    File.WriteAllText(FilePath, text);
                }
                }
        }

        private void removeNoSelected()
        {
            foreach (int indexChecked in checkList.CheckedIndices)
            {
                    String path = checkList.Items[indexChecked].ToString().Substring(8);
                    List<string> productlines = File.ReadAllLines("michael.txt").ToList();
                    string FilePath = $"{mainDir}/michael.txt";
                    string Contents = File.ReadAllText(FilePath);
                    String text = Contents;

                    //Remove headers
                    productlines.RemoveAt(0);
                    checkList.Items.Remove(checkList.Items[indexChecked].ToString());

                foreach (string line in productlines)
                    {
                        string[] parts = line.Split(';');

                        for (int i = 0; i < parts.Length; i++)
                        {
                            String data = parts[i];
                            if (data.Contains("virus"))
                            {
                                if (parts[i + 1] == path)
                                {
                                    text = Contents.Replace(line, "");
                                }
                            }
                        }

                    }
                    File.WriteAllText(FilePath, text);
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
                try
                {
                    productlines.RemoveAt(0);
                } catch (Exception e)
                { }

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
            this.Close();
        }

        private void ClientAV_Load(object sender, EventArgs e)
        {

        }

        private void scanFileBTN_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            removeNoSelected();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            allCheck = !allCheck;
            for (int i = 0; i < checkList.Items.Count; i++)
            {
               checkList.SetItemChecked(i, allCheck);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            deleteAllSelected();
        }
    }
}