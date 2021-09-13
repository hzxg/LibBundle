using LibBundle;
using LibBundle.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FKTT.AddBundle;

namespace FKTT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            Control.CheckForIllegalCrossThreadCalls = false;
        }
       
        public string cddir = System.IO.Directory.GetCurrentDirectory();
        public string mainicdir = System.IO.Directory.GetCurrentDirectory();
        public string icdir = System.IO.Directory.GetCurrentDirectory();
        public IndexContainer mainic,ic;
         DataTable dt = new DataTable();
        




        private void Form1_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("c", typeof(System.Boolean));
            dt.Columns.Add("patch", typeof(System.String));

           

            treeView1.CheckBoxes = true;
            treeView1.FullRowSelect = true;
            treeView1.Indent = 20;
            treeView1.ItemHeight = 20;
            treeView1.LabelEdit = false;
            treeView1.Scrollable = true;
            treeView1.ShowPlusMinus = true;
            treeView1.ShowRootLines = true;

            timer1.Enabled = true;
            //GetindexPatch();
            string tempdir = cddir + @"\temp\";// + DateTime.Now.ToString("yyyyMMddHHmm") + "\\";
            if (Directory.Exists(tempdir))
            {
                DirectoryInfo di = new DirectoryInfo(tempdir);
                di.Delete(true);
              
            }
            System.IO.Directory.CreateDirectory(tempdir);
            System.IO.Directory.SetCurrentDirectory(tempdir);
        }
         private void Output(string msg)
        {
            textBoxOutput.AppendText(msg);
            textBoxOutput.SelectionStart = textBoxOutput.Text.Length;
            textBoxOutput.ScrollToCaret();
            textBoxOutput.Refresh();
         }
         private void OutputLine(string msg)
         {
            Output(msg + "\r\n");
         }
        private static object locker = new object();//创建锁


  

    private void button1_Click(object sender, EventArgs e)
        {
            ic = null;
            treeView1.Nodes.Clear();
            var ofd = new OpenFileDialog
            {
                
                DefaultExt = "zip",
                
                Filter = "zip|*.zip"

            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Thread t = new Thread(new ParameterizedThreadStart(loadpatch));
                 t.Start(ofd.FileName);
                Thread.Sleep(2000);
               
                lock (locker)//加锁               
                {
                    treeView1.Nodes.Add(rtn);
                }
            } 

            else
            {
                Close();
                return;
            }
          
        }
      
        private  void loadpatch(object zipPath)
        {
            lock (locker)//加锁               
            {
                var zz = ZipFile.OpenRead(zipPath.ToString());

                foreach (var item in zz.Entries)
                {
                    if (item.Name.Contains("zip"))
                    {
                       
                   
                    if (item.Name.Substring(item.Name.LastIndexOf("."), item.Name.Length- item.Name.LastIndexOf(".")) == ".zip")
                    {
                        OutputLine(item.Name);

                        string tempdir = cddir + @"\temp\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\\";
                        if (!Directory.Exists(tempdir))
                        {
                            System.IO.Directory.CreateDirectory(tempdir);
                        }

                        item.ExtractToFile(tempdir + item.Name);
                        var zz2 = ZipFile.OpenRead(tempdir + item.Name);
                        foreach (var item2 in zz2.Entries)
                        {
                            OutputLine(item2.Name);
                            if (item2.Name == "_.index.bin")
                            {
                                zz2.ExtractToDirectory(tempdir);
                                //item2.ExtractToFile(tempdir + item2.Name);
                                tempdir = tempdir + "\\Bundles2\\";
                                System.IO.Directory.SetCurrentDirectory(tempdir);
                                ic = new IndexContainer(tempdir + "_.index.bin");
                                progressBar1.Maximum = ic.Bundles.Length;
                                foreach (var item3 in ic.Bundles)
                                {
                                    progressBar1.Value = item3.bundleIndex;
                                    if (File.Exists(item3.Name))
                                    {
                                        if (item3 is BundleRecord record)
                                        {
                                            //loadedBundles = item3;

                                            foreach (var f in record.Files)
                                            {

                                                DataRow dr1 = dt.NewRow();
                                                dr1[1] = f.path;
                                                dt.Rows.Add(dr1);


                                            }

                                        }
                                    }

                                }
                                //var fr = ic.FindFiles[IndexContainer.FNV1a64Hash("Metadata/UI/UISettings.xml")];

                                //string result = Encoding.Unicode.GetString(fr.Read());

                                // MessageBox.Show(result);

                            }
                        }
                    }
                    }
                }

                addNodes();

                OutputLine("done");
            }
             
        }
      
        private  void loadpatchbak(object zipPath)
        {

            var zz = ZipFile.OpenRead(zipPath.ToString());

            foreach (var item in zz.Entries)
            {
                if (item.Name.Substring(item.Name.IndexOf(".") + 1) == "zip")
                {
                    OutputLine(item.Name);

                    string tempdir = cddir + @"\temp\" + DateTime.Now.ToString("yyyyMMddHHmm") + "\\";
                    if (!Directory.Exists(tempdir))
                    {
                        System.IO.Directory.CreateDirectory(tempdir);
                    }

                    item.ExtractToFile(tempdir + item.Name);
                    var zz2 = ZipFile.OpenRead(tempdir + item.Name);
                    foreach (var item2 in zz2.Entries)
                    {
                        OutputLine(item2.Name);
                        if (item2.Name == "_.index.bin")
                        {
                            zz2.ExtractToDirectory(tempdir);
                            //item2.ExtractToFile(tempdir + item2.Name);
                            tempdir = tempdir + "\\Bundles2\\";
                            System.IO.Directory.SetCurrentDirectory(tempdir);
                            ic = new IndexContainer(tempdir + "_.index.bin");
                            progressBar1.Maximum = ic.Bundles.Length;
                            foreach (var item3 in ic.Bundles)
                            {
                                progressBar1.Value = item3.bundleIndex;
                                if (File.Exists(item3.Name))
                                {
                                    if (item3 is BundleRecord record)
                                    {
                                        //loadedBundles = item3;

                                        foreach (var f in record.Files)
                                        {

                                            DataRow dr1 = dt.NewRow();
                                            dr1[1] = f.path;
                                            dt.Rows.Add(dr1);


                                        }

                                    }
                                }

                            }
                            //var fr = ic.FindFiles[IndexContainer.FNV1a64Hash("Metadata/UI/UISettings.xml")];

                            //string result = Encoding.Unicode.GetString(fr.Read());

                            // MessageBox.Show(result);

                        }
                    }
                }

            }

            addNodes();

            OutputLine("done");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            patchic(@"C:\Users\hzxg\Desktop\POE\ROOT\");
        }






        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

           if (check == false)
            { 
                
                setchild(e.Node);
                setparent(e.Node);
                check = false;
                
            }
            //checkTreeViewNode(treeView1.Nodes);
        }
        bool check = false;
       
        private void setchild(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = node.Checked;
            }
            check = true;
            
        }
        //设置父节点状态
        private void setparent(TreeNode node)
        {
            if (node.Parent != null)
            {
                //如果当前节点状态为勾选，则需要所有兄弟节点都勾选才能勾选父节点
                if (node.Checked)
                    foreach (TreeNode brother in node.Parent.Nodes)
                    {
                        if (brother.Checked == false)
                            return;
                    }
                node.Parent.Checked = node.Checked;
            }
        }
        //树节点的子树节点逻辑操作
       
        private bool isFilesNode(TreeNode tn)
        {
            
            if (tn.Nodes.Count == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //System.IO.Directory.SetCurrentDirectory(mainicdir);
            // mainic =new IndexContainer(mainicdir + "_.index.bin");
            var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash("Metadata/UI/UISettings.xml")];
            string result = Encoding.Unicode.GetString(fr.Read());

            MessageBox.Show(result);

            //System.IO.Directory.SetCurrentDirectory(icdir);
            var fr2 = ic.FindFiles[IndexContainer.FNV1a64Hash("Metadata/UI/UISettings.xml")];


            string result2 = Encoding.Unicode.GetString(fr2.Read());

            MessageBox.Show(result2);



        }
        public delegate void SendToMain();
        private void button4_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(btb);
            t.Start();
        }
        private  void btb()
        {
            checkTreeViewNode(treeView1.Nodes);
            DataRow[] dr = dt.Select("c='true'");
            label1.Text = dr.Length.ToString();
            string str = "Imported {0}/" + dr.Length.ToString() + " Files";
            progressBar1.Maximum = dr.Length;

            int l = loadedBundles[0].UncompressedSize;
            BundleRecord bundleToSave = loadedBundles[0];


            int count = 0;
            foreach (DataRow f in dr)
            {

                var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash(f[1].ToString())];
                var fr2 = ic.FindFiles[IndexContainer.FNV1a64Hash(f[1].ToString())];


                fr.Write(fr2.Read());
                fr.Move(bundleToSave);
                ++count;
                progressBar1.Value = count ;
                // OutputLine(string.Format(str, count));
            }
            if (count > 0)
                changed.Add(bundleToSave);
            progressBar1.Value = 0;
           OutputLine("btb done");
        }


        public void checkTreeViewNode(TreeNodeCollection node)
        {
            foreach (TreeNode n in node)
            {
                if ( isFilesNode(n))
                   // if (n.Checked && isFilesNode(n))
                    {
                   // MessageBox.Show(n.FullPath);
                    DataRow[] dr = dt.Select("patch='" + n.FullPath.Replace("ROOT\\", "").Replace("\\", "/") + "'");
                    if (dr.Length == 1)
                    {
                        dr[0][0] = n.Checked;
                        DataRow[] dr2 = dt.Select("patch='" + n.FullPath.Replace("ROOT\\", "").Replace("\\", "/") + "64'");
                        if (dr2.Length == 1)
                        {
                            dr2[0][0] = n.Checked; ;
                        }

                    }
                    

                }
                checkTreeViewNode(n.Nodes);
            }
        }
        private  TreeNode FindNode(TreeNode tnp, string str)
        {
            if (tnp == null)
            {
                return null;
            }
            if (tnp.Text == str)
            {
                return tnp;
            }
            TreeNode tnT = null;
            foreach (TreeNode tn in tnp.Nodes)
            {
                tnT = FindNode(tn, str);
                if (tnT != null)
                {
                    break;
                }

            }
            return tnT;
        }
        TreeNode rtn;
       

        public void addNodes()
        {

            DataTable dtCopy = dt.Copy();

            DataView dv = dt.DefaultView;
            dv.Sort = "patch";
            dtCopy = dv.ToTable();
            dt = dtCopy;
            //TreeNode rtn = new TreeNode("ROOT");
            //treeView1.Nodes.Add(rtn);
            rtn = new TreeNode("ROOT");
            string str64 = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[1].ToString().Replace("64", "") != str64)
                {

                    str64 = dr[1].ToString();
                    string[] values = dr[1].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    TreeNode tnt = rtn;
                    for (int i = 0; i < values.Length; i++)

                    {
                        TreeNode tntt = FindNode(tnt, values[i]);
                        if (tntt == null)
                        {
                            tnt.Nodes.Add(values[i]);
                            tnt = FindNode(tnt, values[i]);
                        }
                        else

                        {
                            tnt = tntt;
                        }

                    }
                }
            }
        }

        //---------------------------------------------------------------



        //-----------------------------------------------------------------------------------------------------------
        public static bool readFile(string filePathName, out byte[] bytes)

        {
            FileStream stream = new FileStream(filePathName, FileMode.Open);
            bool ret = false;
            bytes = null;
            if (null != stream)
            {
                int len = (int)stream.Length;
                bytes = new byte[len];
                int readLend = stream.Read(bytes, 0, len);
                stream.Flush();
                stream.Close();
                ret = readLend == len;
            }
            return ret;
        }

      
        private void GetindexPatch(object indexPath)
        {


            Environment.CurrentDirectory = Path.GetDirectoryName(indexPath.ToString());
            mainic = new IndexContainer(indexPath.ToString());
           
            progressBar1.Maximum = mainic.Bundles.Length;
            foreach (var item in  mainic.Bundles)
            {
              
                if (File.Exists(item.Name))
                    loadedBundles.Add(item);
                progressBar1.Value = item.bundleIndex ;
                 

            }
            OutputLine("loadMain  done");
            progressBar1.Value = 0;
        }
        private void ChangeFonts()
        {
            var fr =  mainic.FindFiles[IndexContainer.FNV1a64Hash("Metadata/UI/UISettings.xml")];
            string result = Encoding.Unicode.GetString(fr.Read());

            Regex r = new Regex("typeface=\"\\w*\\s?\\S?\\w*\\s?\\S?\\w*\"");
            MatchCollection mc = r.Matches(result);

            for (int i = 0; i < mc.Count; i++)
            {
                result = result.Replace(mc[i].Value, "typeface=\"微软雅黑\"");

            }
            OutputLine("总共修改" + mc.Count.ToString() + "处字体");

            

            string backupdir = cddir + @"\Backup\"+ DateTime.Now.ToString("yyyyMMddHHmm");            
            if (!Directory.Exists(backupdir))
            {
                System.IO.Directory.CreateDirectory(backupdir);
            }
            string BeforeDir = backupdir + "\\Before\\ROOT\\Metadata\\UI\\";
            if (!Directory.Exists(BeforeDir))
            {
                System.IO.Directory.CreateDirectory(BeforeDir);
            }
            File.WriteAllBytes(BeforeDir+ "UISettings.xml", fr.Read());
            OutputLine("Before导出完成");

            byte[] b1 = Encoding.Unicode.GetBytes(result);
            string AfterDir = backupdir + "\\After\\ROOT\\Metadata\\UI\\";
            if (!Directory.Exists(AfterDir))
            {
                System.IO.Directory.CreateDirectory(AfterDir);
            }
            File.WriteAllBytes(AfterDir+ "UISettings.xml", b1);

            OutputLine("After导出完成");
             
            BundleRecord bundleToSave =  mainic.GetSmallestBundle(loadedBundles);
            fr.Write(b1); 
            fr.Move(bundleToSave);
            changed.Add(bundleToSave);

            OutputLine("修改完成");
        }

        public readonly HashSet<BundleRecord> changed = new HashSet<BundleRecord>();
        public List<BundleRecord> loadedBundles = new List<BundleRecord>();
        private void patchic(object SPatch)
        {
            int l = loadedBundles[0].UncompressedSize;
            BundleRecord bundleToSave = loadedBundles[0];
         
                var fileNames = Directory.GetFiles(SPatch.ToString(), "*", SearchOption.AllDirectories);
                string str = "Imported {0}/" + fileNames.Length.ToString() + " Files";
                int count = 0;
                foreach (var f in fileNames)
                {
                    var path = f.Remove(0, SPatch.ToString().Length ).Replace("\\", "/");
                MessageBox.Show(path);
                    var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash(path)];
                    fr.Write(File.ReadAllBytes(f));
                    fr.Move(bundleToSave);
                    ++count;
                    OutputLine(string.Format(str, count));
                }
                if (count > 0)
                changed.Add(bundleToSave);



        }
        private void patchROOTDir()
        {
            //选择 补丁ROOT文件夹   
            BundleRecord bundleToSave =  mainic.GetSmallestBundle(loadedBundles);

            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var fileNames = Directory.GetFiles(dialog.SelectedPath, "*", SearchOption.AllDirectories);
                string str = "Imported {0}/" + fileNames.Length.ToString() + " Files";
                int count = 0;
                foreach (var f in fileNames)
                {
                    var path = f.Remove(0, dialog.SelectedPath.Length + 1).Replace("\\", "/");
                    var fr =  mainic.FindFiles[IndexContainer.FNV1a64Hash(path)];
                    fr.Write(File.ReadAllBytes(f));
                    fr.Move(bundleToSave);
                    ++count;
                    OutputLine(string.Format(str, count));
                }
                if (count > 0)
                    changed.Add(bundleToSave);
            }



        }
        private void button_loadMain_Click(object sender, EventArgs e)
        {
            mainic = null;
            string indexPath;
            progressBar1.Value = 0;
            var ofd = new OpenFileDialog
            {
                DefaultExt = "bin",
                FileName = "_.index.bin",
                Filter = "GGG Bundle index|_.index.bin"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            { 
                indexPath = ofd.FileName;
               
            }
            else
            {
                Close();
                return;
            }
            if (Path.GetFileName(indexPath) != "_.index.bin")
            {
                MessageBox.Show("You must select _.index.bin!", "Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                Close();
                return;
            }
            else
            {
                Thread t = new Thread(new ParameterizedThreadStart(GetindexPatch));
                t.Start(indexPath);

            }

           // Thread th = new Thread(GetindexPatch);
           // th.Start();
        }
        private void button_Save_Click(object sender, EventArgs e)
        {

            if (changed.Count > 0)
            {
                
                    var i = 1;
                    var text = "Saving {0} / " + (changed.Count + 1).ToString() + " bundles . . .";
                    foreach (var br in changed)
                    {
                       OutputLine(string.Format(text, i));
                    S:
                        if (!File.Exists(br.Name))
                        {
                            if (MessageBox.Show("File Not Found:" + Environment.NewLine + Path.GetFullPath(br.Name) + "Please put the bundle to the path and click OK", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.OK)
                                goto S;
                            else
                            {
                                MessageBox.Show(" Bundles Changed" + "Please restore the backup", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                Close();
                                return;
                            }
                        }
                        br.Save(br.Name);
                        i++;
                    }
                OutputLine(string.Format(text, i));
                 mainic.Save("_.index.bin");
               
            }
            OutputLine("完成保存");
            changed.Clear();
        }

        private void button_ChangeFonts_Click(object sender, EventArgs e)
        {
            ChangeFonts();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ti >0)
            {
                ti = ti - 1;
               
            }
            else
            {
                treeView1.Enabled = true;
            }
           


        }
        int ti;
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            ti  = 1;
            treeView1.Enabled = false;
        }

       

        private void treeView1_Click(object sender, EventArgs e)
        {
            ti = 1;
            treeView1.Enabled = false;
        }


       
    }
}
