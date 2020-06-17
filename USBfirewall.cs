using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class USBfirewall : Form
    {
        public USBfirewall()
        {
            InitializeComponent();
        }
        CUSBMonitor usbMonitor = new CUSBMonitor();
        
        
        #region U盘插入检测
        protected override void WndProc(ref Message m)
        {
            usbMonitor.FillData(this, m, listBox1);

            base.WndProc(ref m);
        }
        #endregion

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        #region 开机自启动
        private void button1_Click(object sender, EventArgs e)
        {
            // 获得应用程序路径
            string strAssName = Application.StartupPath + @"\" + Application.ProductName + @".exe";
            // 获得应用程序名称
            string strShortFileName = Application.ProductName;

            // 打开注册表基项"HKEY_LOCAL_MACHINE"
            RegistryKey rgkRun = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rgkRun == null)
            {   // 若不存在，创建注册表基项"HKEY_LOCAL_MACHINE"
                rgkRun = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                MessageBox.Show("添加开机启动成功");
            }

            // 设置指定的注册表项的指定名称/值对。如果指定的项不存在，则创建该项。
            rgkRun.SetValue(strShortFileName, strAssName);
            MessageBox.Show("添加开机启动成功");
        }
        #endregion
        private void label4_Click(object sender, EventArgs e)
        {

        }


        #region 隐藏任务栏图标、显示托盘图标
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }
        #endregion

        #region 还原窗体
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        #endregion

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        private void 隐藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {

        }

        private void USBfirewall_Load(object sender, EventArgs e)
        {

        }

        #region 窗口最小化
        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.notifyIcon1.Visible = true;
        }
        #endregion

        #region 用记事本打开autorun文件
        private void button3_Click(object sender, EventArgs e)
        {
            string OpenFolderPath = usbMonitor.GetDisk() + "autorun.inf";
            try
            {
                System.Diagnostics.Process.Start("notepad.exe", OpenFolderPath);
            }
            catch 
            {
                MessageBox.Show("无法找到autorun文件！");
            }
        }
        #endregion
        #region 删除autorun文件
        private void button4_Click(object sender, EventArgs e)
        {
            string OpenFolderPath = usbMonitor.GetDisk() + "autorun.inf";
            if (File.Exists(OpenFolderPath))
            {
                File.Delete(OpenFolderPath);
                MessageBox.Show("成功删除autorun文件！");
            }
            else
                MessageBox.Show("无法找到autorun文件！");

        }
        #endregion
        #region 安全打开u盘
        private void button2_Click(object sender, EventArgs e)
        {
            string FilePath = usbMonitor.GetDisk();
            System.Diagnostics.Process.Start("explorer.exe", FilePath);
        }
        #endregion

        #region 病毒添加
        VirusAnalysis virus = new VirusAnalysis();
        private void button5_Click(object sender, EventArgs e)
        {
            virus.autorunanalysis(this, listBox1, listBox2, listBox3, listBox4);
        }

        #endregion
        #region 删除病毒
        private void button7_Click(object sender, EventArgs e)
        {
            virus.deletevirus(this, listBox2, listBox3, listBox4);
        }
        #endregion

        #region U盘格式化
        private void button8_Click(object sender, EventArgs e)
        {
            string FilePath = usbMonitor.GetDisk();
            FilePath=FilePath.Remove(2);
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;

            Process process = Process.Start(processStartInfo);

            if (process != null)
            {
                process.StandardInput.WriteLine($"FORMAT {FilePath} /y /FS:FAT32 /V:BMECG /Q");
                process.StandardInput.Close();

                string outputString = process.StandardOutput.ReadToEnd();
                if (outputString.Contains("已完成"))
                {
                    MessageBox.Show("完成U盘格式化操作");
                }
            }
        }
        #endregion
    }


}
