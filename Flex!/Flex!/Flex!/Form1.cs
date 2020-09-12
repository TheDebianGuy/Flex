using System;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Flex_
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);
        public Form1()
        {
            try
            {
                InitializeComponent();
                pictureBox1.BringToFront();
                label4.BringToFront();
                pictureBox9.SendToBack();
                label3.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                label8.Text = Environment.ProcessorCount.ToString() + " Cores";
                string[] drives = Environment.GetLogicalDrives();
                label11.Text = "Drives: "; foreach (string drive in drives) { label11.Text = label11.Text + drive + " "; };
                label3.BringToFront();
                label7.BringToFront();
                label7.Text = Environment.SystemDirectory;
                label2.Text = RuntimeInformation.OSDescription; //os name
                label1.Text = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion", "RegisteredOrganization", ""); //brand
                string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString(); //width
                string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString(); //height
                label9.Text = screenWidth + "x" + screenHeight; //screen bounds
                long memKb;
                GetPhysicallyInstalledSystemMemory(out memKb);
                label4.Text = (memKb / 1024 / 1024).ToString() + " GB"; //ram
                DriveInfo info = new DriveInfo(Path.GetPathRoot(Environment.CurrentDirectory));
                DriveSpace.Text = DriveSpace.Text + Path.GetPathRoot(Environment.CurrentDirectory) + ")";
                label5.Text = (info.TotalSize / 1024 / 1024 / 1024).ToString() + " GB";
                label6.Text = System.Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine("wmic path win32_VideoController get name");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                string gname = cmd.StandardOutput.ReadToEnd().Split(new[] { "\n" }, StringSplitOptions.None)[5];
                cmd.Start();
                cmd.StandardInput.WriteLine("wmic path win32_VideoController get adapterram");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                string gram = cmd.StandardOutput.ReadToEnd().Split(new[] { "\n" }, StringSplitOptions.None)[5]; 
                GpuName.Text = gname; //gpu name
                Console.WriteLine(gram);
                GpuRam.Text = gram[0] + " GB"; //gpu ram
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var embed = "<html><head>" +
            "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
            "</head><body style=\"background: #f0f0f0\">" +
            "<iframe width=\"1070\" height=\"665\" src=\"{0}\"" +
            "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
            "</body></html>";
            var url = "https://www.youtube.com/embed/3CJE6XheubM";
            this.webBrowser1.DocumentText = string.Format(embed, url);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate("https://www.pcgamebenchmark.com/ratemypc");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("WMIC CSPRODUCT GET NAME");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            string pname = cmd.StandardOutput.ReadToEnd().Split(new[] { "\n" }, StringSplitOptions.None)[5];
            cmd.Start();
            this.webBrowser1.Navigate("https://www.google.com/search?q=" + pname);
        }
    }
}
