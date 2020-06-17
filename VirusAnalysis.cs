using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class VirusAnalysis
    {
        private static int autoadd = 0;

        #region 对于autorun的分析

        public void autorunanalysis(Form form, ListBox pathlist, ListBox viruskind, ListBox viruscount, ListBox viruspath)
        {
            string FilePath = Convert.ToString(pathlist.Items[0]) + "autorun.inf";
            try
            {
                string line;
                int vcount, vkind;
                //读取当前病毒种类、病毒数量
                vcount = Convert.ToInt32(viruscount.Items[0]);
                vkind = Convert.ToInt32(viruskind.Items[0]);
                //病毒路径list
                List<string> virus = new List<string>();
                // 创建一个 StreamReader 的实例来读取文件 ,using 语句也能关闭 StreamReader
                using (System.IO.StreamReader sr = new System.IO.StreamReader(FilePath))
                {
                    // 逐行分析autorun
                    int linecount = 0;

                    /* if (sr.Peek() != -1)
                     { viruspath.Items.Add("not null"); }
                     else
                     { viruspath.Items.Add("null"); }*/

                    while ((line = sr.ReadLine()) != null)
                    {
                        //该行全小写，去除多余空格
                        line = line.ToLower();
                        line = line.Trim();

                        //文件开头为[autorun]判断
                        if (linecount == 0)
                        {
                            if (String.Compare(line, "[autorun]") != 0)
                                return;
                        }
                        //病毒路径判断
                        if (line.Contains("="))
                        {
                            int index = line.IndexOf("=");
                            line = line.Remove(0, index + 1);
                            line = line.Trim();
                            //病毒路径添加
                            if (line.Contains("."))
                            {
                                //添加病毒
                                string path = Convert.ToString(pathlist.Items[0]) + line;
                                //查重判断
                                Boolean add = true;
                                for (int i = 0; i < viruspath.Items.Count; i++)
                                {
                                    string tmp = Convert.ToString(viruspath.Items[i]);
                                    if (String.Compare(path, tmp) == 0)
                                    {//重
                                        add = false;
                                    }
                                }
                                if (add)
                                { viruspath.Items.Add(path); vcount++; autoadd++; }

                            }
                        }
                        linecount++;
                    }//endwhile
                    if (autoadd > 1)
                    { vkind++; }
                    //传回listbox
                    viruscount.Items.Clear();
                    viruscount.Items.Add(vcount);
                    viruskind.Items.Clear();
                    viruskind.Items.Add(vkind);
                }
            }
            catch (Exception e)
            {
                viruspath.Items.Add("exception");
            }


        }
        #endregion
        #region 删除病毒
        public void deletevirus(Form form, ListBox virkind, ListBox vircount, ListBox virpath)
        {
            try
            {
                for (int i = 0; i < virpath.Items.Count; i++)
                {
                    File.Delete(Convert.ToString(virpath.Items[i]));
                }
                virkind.Items.Clear();
                vircount.Items.Clear();
                virpath.Items.Clear();
                virkind.Items.Add("0");
                vircount.Items.Add("0");
                MessageBox.Show("病毒已经成功删除");

            }
            catch (Exception e)
            {
                MessageBox.Show("exception");
            }


        }
        #endregion 
    }
}
