
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace WindowsFormsApp1
{
    /// <summary>
    /// USB插拔监控类
    /// </summary>
    public class CUSBMonitor
    {
        private delegate void SetTextCallback(string s);
        private IList<string> _usbdiskList = new List<string>();
        private ListBox _listbox = null;
        private Form _form = null;

        public CUSBMonitor()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Enabled = true;

            // 达到间隔时发生
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerList);
            timer.AutoReset = false; // 仅在间隔第一次结束后引发一次          
        }

        public void FillData(Form form, Message m, ListBox listbox)
        {
            _listbox = listbox;
            _form = form;

            try
            {
                if (m.Msg == CWndProMsgConst.WM_DEVICECHANGE) // 系统硬件改变发出的系统消息
                {
                    switch (m.WParam.ToInt32())
                    {
                        case CWndProMsgConst.WM_DEVICECHANGE:
                            break;
                        //设备检测结束，并且可以使用
                        case CWndProMsgConst.DBT_DEVICEARRIVAL:
                            {
                                ScanUSBDisk();
                                _listbox.Items.Clear();
                                MessageBox.Show("U盘已插入");
                                foreach (string str in _usbdiskList)
                                {
                                    _listbox.Items.Add(str);
                                }
                            }
                            break;
                        // 设备卸载或者拔出
                        case CWndProMsgConst.DBT_DEVICEREMOVECOMPLETE:
                            {
                                ScanUSBDisk();
                                _listbox.Items.Clear();
                                MessageBox.Show("U盘已拔出");
                                foreach (string str in _usbdiskList)
                                {
                                    _listbox.Items.Add(str);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("当前盘不能正确识别，请重新尝试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public string GetDisk()
        {
            return _usbdiskList[0];
        }
        /// <summary>
        /// 设置USB列表
        /// </summary>
        void TimerList(object sender, System.Timers.ElapsedEventArgs e)
        {
            ScanUSBDisk();
            foreach (string str in _usbdiskList)
            {
                SetText(str);
            }
        }

        /// <summary>
        /// 扫描U口设备
        /// </summary>
        private void ScanUSBDisk()
        {
            _usbdiskList.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if ((drive.DriveType == DriveType.Removable) && !drive.Name.Substring(0, 1).Equals("A"))
                {
                    try
                    {
                        _usbdiskList.Add(drive.Name);
                    }
                    catch
                    {
                        MessageBox.Show("当前盘不能正确识别，请重新尝试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        /// <summary>
        /// 设置List列表
        /// </summary>
        /// <param name="text">名称</param>
        public void SetText(string text)
        {
            if (_listbox == null)
                return;

            if (this._listbox.InvokeRequired) // 调用方位于创建控件所在的线程以外的线程中
            {
                if (_listbox.Items.Contains(text))
                    return;

                SetTextCallback d = new SetTextCallback(SetText);
                _form.Invoke(d, new object[] { text });
            }
            else
            {
                if (_listbox.Items.Contains(text))
                    return;

                this._listbox.Items.Add(text);
            }
        }
    }

    /// <summary>
    /// windows消息常量
    /// </summary>
    class CWndProMsgConst
    {
        public const int WM_DEVICECHANGE = 0x219; // 系统硬件改变发出的系统消息
        public const int DBT_DEVICEARRIVAL = 0x8000;// 设备检测结束，并且可以使用
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;// 设备卸载或者拔出
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPEHANGED = 0x0007;
        public const int DBT_QUERYCHANGSPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;
    }
}
