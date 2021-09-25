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
        public string tempdir = System.IO.Directory.GetCurrentDirectory() + @"\temp\";
        public string Bdir = System.IO.Directory.GetCurrentDirectory() + @"\Backup\";
        public string mainicdir, icdir, Backupdir, ROOTdir;
        bool needBackup;
        public IndexContainer mainic, ic;
        DataTable dt = new DataTable();

        public readonly HashSet<BundleRecord> changed = new HashSet<BundleRecord>();
        public List<BundleRecord> loadedBundles = new List<BundleRecord>();
        private static object locker = new object();



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
            button_ChangeFonts.Enabled = false;
            button_Installpatch.Enabled = false;
            button_InstallROOT.Enabled = false;
            button_etc.Enabled = false;
            if (Directory.Exists(tempdir))
            {
                DirectoryInfo di = new DirectoryInfo(tempdir);
                di.Delete(true);

            }
            System.IO.Directory.CreateDirectory(tempdir);
             
        }
        private void button1_Click(object sender, EventArgs e)
        {
        

        }
        private void button2_Click(object sender, EventArgs e)
        {
            CDB();

            needBackup = false;
            DialogResult r1 = MessageBox.Show("是否要备份", "标题", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "Yes")
            {
                needBackup = true;
                Backupdir = Bdir + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + "\\";
                if (!Directory.Exists(Backupdir))
                {
                    System.IO.Directory.CreateDirectory(Backupdir);
                }

                MessageBox.Show("备份文件夹 " + Backupdir);
            }
            if (r1.ToString().Equals("No"))
            {
            }
            if (r1.ToString().Equals("Cancel"))
            {
                goto cfend;
            }
            Thread t = new Thread(txtetc);
            t.Start();
        cfend:;

        }
        private void button3_Click(object sender, EventArgs e)
        {
            //System.IO.Directory.SetCurrentDirectory(mainicdir);

            var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash("Metadata/StatDescriptions/banner_aura_skill_stat_descriptions.txt")];
            string result = Encoding.Unicode.GetString(fr.Read());

            MessageBox.Show(result);

            //System.IO.Directory.SetCurrentDirectory(icdir);
            //var fr2 = ic.FindFiles[IndexContainer.FNV1a64Hash("Metadata/UI/UISettings.xml")];


            //string result2 = Encoding.Unicode.GetString(fr2.Read());

            //MessageBox.Show(result2);

            //string[] f = new string[2];
            //f[1] = "Metadata/UI/UISettings.xml";
            //string fn = f[1].ToString().Replace(f[1].ToString().Substring(0, f[1].ToString().LastIndexOf("/") + 1), "");


            //string fd = Backupdir + "ROOT\\" + (f[1].ToString().Replace("/", "\\")).Replace(fn, "");
            //MessageBox.Show(fn);
            //MessageBox.Show(fd);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(btb);
            t.Start();
        }


        public bool FindCN = false;                // 识别到lang \"Simplified Chinese\""
        public int _description_Counts = 0;        // 列数
        public DataTable tb_stat_descriptions;
        string[] txt_stat_descriptions;
        public int xzi = 0;
        public string xzs = "";
        private void CDB()
        {
            tb_stat_descriptions = new DataTable();
            tb_stat_descriptions.Columns.Add("ID", Type.GetType("System.String"));
            tb_stat_descriptions.Columns.Add("description", Type.GetType("System.String"));
            tb_stat_descriptions.Columns.Add("Mod", Type.GetType("System.String"));
            tb_stat_descriptions.Columns.Add("ModLineCounts", Type.GetType("System.String"));

            tb_stat_descriptions.Columns.Add("enModCounts", Type.GetType("System.String"));
            tb_stat_descriptions.Columns.Add("cnModLine", Type.GetType("System.String"));
            tb_stat_descriptions.Columns.Add("cnModCounts", Type.GetType("System.String"));
            tb_stat_descriptions.Columns.Add("en2cn", Type.GetType("System.String"));
            tb_stat_descriptions.Columns.Add("cnLine", Type.GetType("System.String"));

            txt_stat_descriptions = new string[] {
                "active_skill_gem_stat_descriptions.txt",
                "advanced_mod_stat_descriptions.txt",
                "atlas_stat_descriptions.txt",
                "aura_skill_stat_descriptions.txt",
                "banner_aura_skill_stat_descriptions.txt",
                "beam_skill_stat_descriptions.txt",
                "brand_skill_stat_descriptions.txt",
                //"buff_skill_stat_descriptions.txt",
                "chest_stat_descriptions.txt",
                "curse_skill_stat_descriptions.txt",
                "debuff_skill_stat_descriptions.txt",
                "expedition_relic_stat_descriptions.txt",
                "gem_stat_descriptions.txt",
                "heist_equipment_stat_descriptions.txt",
                "leaguestone_stat_descriptions.txt",
                "map_stat_descriptions.txt",
                "minion_attack_skill_stat_descriptions.txt",
                "minion_skill_stat_descriptions.txt",
                "minion_spell_damage_skill_stat_descriptions.txt",
                "minion_spell_skill_stat_descriptions.txt",
                "monster_stat_descriptions.txt",
                "offering_skill_stat_descriptions.txt",
                "passive_skill_aura_stat_descriptions.txt",
                "passive_skill_stat_descriptions.txt",
                "secondary_debuff_skill_stat_descriptions.txt",
                "single_minion_spell_skill_stat_descriptions.txt",
                //"skill_stat_descriptions.txt",
                "stat_descriptions.txt",
                "variable_duration_skill_stat_descriptions.txt"
            };


        }
        private void txtetc()
        {
            string path = "Metadata/StatDescriptions/";
            string fdb= tempdir + "Before\\ROOT\\" + (path.ToString().Replace("/", "\\"));
            
            if (!Directory.Exists(fdb))
            {
                System.IO.Directory.CreateDirectory(fdb);
            }
            string fda = tempdir + "After\\ROOT\\" + (path.ToString().Replace("/", "\\"));
            if (!Directory.Exists(fda))
            {
                System.IO.Directory.CreateDirectory(fda);
            }
            progressBar1.Maximum = txt_stat_descriptions.Length;
            int i = 0;
            OutputLine("load......");
            foreach (var item in txt_stat_descriptions)
            {
                progressBar1.Value = i;
                var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash(path+ item)];
                
                    string fn = item;  
                    File.WriteAllBytes(fdb + fn, fr.Read());
                i++;
            }
            var fileNames = Directory.GetFiles(fdb, "*", SearchOption.AllDirectories);
            foreach (var f in fileNames)
            {
                LoadTxt(f);
                ExportTxt(f.Replace("Before", "After"));
                tb_stat_descriptions.Clear();
            }
            try
            {
                patchic(tempdir + "After\\ROOT\\");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
        }
        private void LoadTxt(string FileName)
        {
            string str1 = File.ReadAllText(FileName);

            str1 = str1.Replace("\n ", "\n");   // 个别  “description" 前有空格
            string[] line = str1.Split(new string[] { "\ndescription" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < line.Length; i++)
            {

                tb_stat_descriptions.Rows.Add(++_description_Counts, line[i].Trim());
            }
            for (int i = 1; i < tb_stat_descriptions.Rows.Count; i++)
            {
                FindCN = false;
                string[] line2 = (((string)tb_stat_descriptions.Rows[i][1]).Trim()).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                tb_stat_descriptions.Rows[i][2] = line2[0].Trim();
                tb_stat_descriptions.Rows[i][3] = line2.Length;
                tb_stat_descriptions.Rows[i][4] = line2[1].Trim();
                for (int iii = 0; iii < line2.Length; iii++)
                {

                    if (line2[iii].Trim() == "lang \"Simplified Chinese\"" && FindCN == false)
                    {
                        FindCN = true;
                        tb_stat_descriptions.Rows[i][5] = line2[iii].Trim();
                        tb_stat_descriptions.Rows[i][6] = line2[iii + 1].Trim();
                        tb_stat_descriptions.Rows[i][8] = iii;
                    }

                }
                try
                {


                    if (((string)tb_stat_descriptions.Rows[i][4]).Trim() == ((string)tb_stat_descriptions.Rows[i][6]).Trim())
                    {
                        tb_stat_descriptions.Rows[i][7] = 0;
                    }
                    else
                    {
                        tb_stat_descriptions.Rows[i][7] = -1;
                    }
                }
                catch (Exception)
                {
                    tb_stat_descriptions.Rows[i][7] = -1;
                }




            }

           


        }
        private void ExportTxt(string FileName)
        {
            bool FindCN = false;
            StreamWriter sw = new StreamWriter(FileName, false, Encoding.Unicode); //保存到指定路径
            if ((string)tb_stat_descriptions.Rows[0][1].ToString().Substring(0,11)== "description")
            {
                int xx;
                string[] line2 = (((string)tb_stat_descriptions.Rows[0][1]).Trim()).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                sw.WriteLine(line2[0]);
                sw.WriteLine(line2[1]);
                for (int iii = 0; iii < line2.Length; iii++)
                {
                    if (line2[iii].Trim() == "lang \"Simplified Chinese\"" && FindCN == false)
                    {
                        xzi = 0;
                        xzs = line2[iii + 1 + xzi].Trim();
                        if (xzs.Length == 0)
                        {
                            xzi++;
                            xzs = line2[iii + 1 + xzi];
                        }


                        FindCN = true;
                        sw.WriteLine(line2[iii + 1 + xzi]);
                        for (int iiii = 0; iiii < int.Parse(xzs); iiii++)
                        {

                            sw.WriteLine(line2[iii + iiii + 2 + xzi]);
                        }

                    }
                    if (line2[iii].Contains("include"))
                    {
                        sw.WriteLine(line2[iii]);
                    }
                    if (line2[iii].Contains("no_description"))
                    {
                        sw.WriteLine(line2[iii]);
                    }

                }
            }
            else
            {
                sw.WriteLine((string)tb_stat_descriptions.Rows[0][1]);
            }
            
            for (int i = 1; i < tb_stat_descriptions.Rows.Count; i++)

            {
               // sw.WriteLine("\ndescription");
                FindCN = false;
                int xx;
             
                string[] line2 = (((string)tb_stat_descriptions.Rows[i][1]).Trim()).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
               // sw.WriteLine("	" + line2[0]);

                if (int.TryParse(line2[0].Trim().Substring(0, 1), out xx))
                {
                    sw.WriteLine("\ndescription");
                    sw.WriteLine(line2[0]);
                  
                }
                else
                {
                    sw.WriteLine("\ndescription"+ " "+ line2[0]);
                    sw.WriteLine(line2[1]);
                   
                }
                for (int iii = 0; iii < line2.Length; iii++)
                {
                    if (line2[iii].Trim() == "lang \"Simplified Chinese\"" && FindCN == false)
                    {
                        xzi = 0;
                        xzs = line2[iii + 1 + xzi].Trim();
                        if (xzs.Length == 0)
                        {
                            xzi++;
                            xzs = line2[iii + 1 + xzi];
                        } 

                        FindCN = true;
                        sw.WriteLine(line2[iii + 1 + xzi]);
                        for (int iiii = 0; iiii < int.Parse(xzs); iiii++)
                        {

                            sw.WriteLine(line2[iii + iiii + 2 + xzi]);
                        }

                    }

                }

                if (FindCN == false)
                {
                    for (int ix = 1; ix < line2.Length; ix++)
                    {
                        sw.WriteLine(line2[ix]);
                    }
                }

            }


            sw.Flush();
            sw.Close();
           
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
               
                OutputLine("loading mainic");
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


        }
        private void GetindexPatch(object indexPath)
        {
            mainicdir = Path.GetDirectoryName(indexPath.ToString());
            System.IO.Directory.SetCurrentDirectory(mainicdir);
            mainic = new IndexContainer(indexPath.ToString());

            progressBar1.Maximum = mainic.Bundles.Length;
            foreach (var item in mainic.Bundles)
            {

                if (File.Exists(item.Name))
                    loadedBundles.Add(item);
                progressBar1.Value = item.bundleIndex;


            }
            OutputLine("loadMain  done");
            button_ChangeFonts.Enabled = true;
            button_etc.Enabled = true;
            progressBar1.Value = 0;
        }
        private void button_Save_Click(object sender, EventArgs e)
        {

            if (changed.Count > 0)
            {
                Thread th = new Thread(MainicSave);
                th.Start();


            }
            else
            {
                MessageBox.Show("没有需要保存的");
            }

        }
        private void MainicSave()
        {
            var i = 1;
            // var text = "Saving {0} / " + (changed.Count + 1).ToString() + " bundles . . .";
            progressBar1.Maximum = changed.Count;
            foreach (var br in changed)
            {
                //OutputLine(string.Format(text, i));
                progressBar1.Value = i;
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
            //OutputLine(string.Format(text, i));
            mainic.Save("_.index.bin");
            OutputLine("完成保存");
            changed.Clear();
            progressBar1.Value = 0;
        }
        private void button_ChangeFonts_Click(object sender, EventArgs e)
        {

            needBackup = false;
            DialogResult r1 = MessageBox.Show("是否要备份", "标题", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "Yes")
            {
                needBackup = true;
                Backupdir = Bdir + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + "\\";
                if (!Directory.Exists(Backupdir))
                {
                    System.IO.Directory.CreateDirectory(Backupdir);
                }

                MessageBox.Show("备份文件夹 " + Backupdir);
            }
            if (r1.ToString().Equals("No"))
            {
            }
            if (r1.ToString().Equals("Cancel"))
            {
                goto cfend;
            }
            Thread th = new Thread(ChangeFonts);
            th.Start();

        cfend:;
        }
        private void ChangeFonts()
        {
            var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash("Metadata/UI/UISettings.xml")];
            string result = Encoding.Unicode.GetString(fr.Read());

            Regex r = new Regex("typeface=\"\\w*\\s?\\S?\\w*\\s?\\S?\\w*\"");
            MatchCollection mc = r.Matches(result);

            for (int i = 0; i < mc.Count; i++)
            {
                result = result.Replace(mc[i].Value, "typeface=\"微软雅黑\"");

            }
            OutputLine("总共修改" + mc.Count.ToString() + "处字体");
            if (needBackup)
            {

                string fn = "UISettings.xml";
                string fd = Backupdir + "\\ROOT\\Metadata\\UI\\";
                if (!Directory.Exists(fd))
                {
                    System.IO.Directory.CreateDirectory(fd);
                }
                File.WriteAllBytes(fd + fn, fr.Read());
                OutputLine("已备份到 " + Backupdir);
            }

            byte[] b1 = Encoding.Unicode.GetBytes(result);


            BundleRecord bundleToSave = mainic.GetSmallestBundle(loadedBundles);
            fr.Write(b1);
            fr.Move(bundleToSave);
            changed.Add(bundleToSave);

            OutputLine("修改完成");
        }
        private void button_loadPacth_Click(object sender, EventArgs e)
        {
            ic = null;
            treeView1.Nodes.Clear();
            dt.Clear();

            var ofd = new OpenFileDialog
            {

                DefaultExt = "zip",

                Filter = "zip|*.zip;_.index.bin"

            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
              
                string Filedir = ofd.FileName;
                OutputLine("loading " + ofd.FileName);
                while (isFilezip(Filedir))
                {

                    var zz = ZipFile.OpenRead(Filedir);
                    int i = 0;
                    foreach (var item in zz.Entries)
                    {

                        if (isFilezip(item.Name))
                        {
                            item.ExtractToFile(tempdir + item.Name);
                            Filedir = tempdir + item.Name;
                            i++;
                        }
                        if (isFilebin(item.Name))
                        {
                            string tdir = tempdir + DateTime.Now.ToString("HHmmss") + "\\";
                            zz.ExtractToDirectory(tdir);
                            Filedir = tdir + item.FullName.Replace("/", "\\");
                            i++;
                            goto findbin;
                        }

                    }
                    if (i == 0)
                    {
                        goto findbin;
                    }

                }
            findbin:
                if (isFilebin(Filedir))
                {

                    //loadpatch(Filedir);

                    Thread t = new Thread(new ParameterizedThreadStart(loadpatch));
                    t.Start(Filedir);
                    Thread.Sleep(2000);

                    lock (locker)//加锁               
                    {
                        treeView1.Nodes.Add(rtn);
                        progressBar1.Value = 0;
                        button_Installpatch.Enabled = true;
                        button_InstallROOT.Enabled = false;
                    }

                }
                else
                {
                    MessageBox.Show("选择 正确的补丁文件 ");
                }




            }

            else
            {

                MessageBox.Show("选择 正确的补丁文件 ");
                //Close();
                return;
            }
        }
        private bool isFilezip(string str)
        {
            if (str.Length > 0 && str.LastIndexOf(".") > 0)
            {

                if (str.Substring(str.LastIndexOf("."), str.Length - str.LastIndexOf(".")) == ".zip")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool isFilebin(string str)
        {
            if (str.Length > 0 && str.LastIndexOf(".") > 0)
            {
                if (str.Substring(str.LastIndexOf("\\") + 1, str.Length - str.LastIndexOf("\\") - 1) == "_.index.bin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void loadpatch(object Filedir)
        {
            lock (locker)//加锁               
            {

                icdir = Filedir.ToString().Replace("_.index.bin", "");

                System.IO.Directory.SetCurrentDirectory(icdir);
                ic = new IndexContainer(Filedir.ToString());
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

                addNodes();

                OutputLine("loadpatch done");
            }

        }
        private void button_Installpatch_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(btb);
            t.Start();
        }
        private void btb()
        {
            checkTreeViewNode(treeView1.Nodes);
            DataRow[] dr = dt.Select("c='true'");
            if (dr.Length > 0)
            {
                needBackup = false;
                DialogResult r1 = MessageBox.Show("是否要备份", "标题", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r1.ToString() == "Yes")
                {
                    needBackup = true;
                    string Backupdir = Bdir + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + "\\";
                    if (!Directory.Exists(Backupdir))
                    {
                        System.IO.Directory.CreateDirectory(Backupdir);
                    }

                    MessageBox.Show("备份文件夹 " + Backupdir);
                }
                if (r1.ToString().Equals("No"))
                { }
                if (r1.ToString().Equals("Cancel"))
                {
                    goto btbend;
                }

                //string str = "Imported {0}/" + dr.Length.ToString() + " Files";
                progressBar1.Maximum = dr.Length;
                //BundleRecord bundleToSave = mainic.GetSmallestBundle(loadedBundles);
                int l = loadedBundles[0].UncompressedSize;
                BundleRecord bundleToSave = loadedBundles[0];
                foreach (var b in loadedBundles)
                {
                    if (b.UncompressedSize < l)
                    {
                        l = b.UncompressedSize;
                        bundleToSave = b;
                    }
                }
                //BundleRecord bundleToSave = mainic.GetSmallestBundle(loadedBundles);
                int count = 0;
                foreach (DataRow f in dr)
                {

                    System.IO.Directory.SetCurrentDirectory(icdir);
                    var fr2 = ic.FindFiles[IndexContainer.FNV1a64Hash(f[1].ToString())];

                    byte[] fr2data = fr2.Read();

                    System.IO.Directory.SetCurrentDirectory(mainicdir);
                    var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash(f[1].ToString())];
                    if (needBackup)
                    {

                        string fn = f[1].ToString().Replace(f[1].ToString().Substring(0, f[1].ToString().LastIndexOf("/") + 1), "");


                        string fd = Backupdir + "ROOT\\" + (f[1].ToString().Replace("/", "\\")).Replace(fn, "");
                        if (!Directory.Exists(fd))
                        {
                            System.IO.Directory.CreateDirectory(fd);
                        }
                        File.WriteAllBytes(fd + fn, fr.Read());
                    }
                    fr.Write(fr2data);
                    fr.Move(bundleToSave);
                    ++count;
                    progressBar1.Value = count;
                    // OutputLine(string.Format(str, count));
                }
                if (count > 0)
                    changed.Add(bundleToSave);
                progressBar1.Value = 0;
                OutputLine("btb done");

            btbend:;
            }
            else
            {
                MessageBox.Show("选取补丁内容");
            }
        }
        private void button_loadROOT_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择  ROOT 文件路径";
            dialog.SelectedPath = Bdir;
            //dialog.RootFolder = Environment.SpecialFolder.Programs;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                if (foldPath.Substring(foldPath.Length - 4, 4) == "ROOT")
                {

                   
                    ROOTdir = foldPath;
                    loadROOT(ROOTdir);
                    button_Installpatch.Enabled = false;
                    button_InstallROOT.Enabled = true;
                }
                else
                {
                    MessageBox.Show("请选择正确的  ROOT 文件路径");
                }
            }
        }
        private void loadROOT(string Rdir)
        {
            dt.Clear();
            treeView1.Nodes.Clear();
            
            var fileNames = Directory.GetFiles(Rdir.ToString(), "*", SearchOption.AllDirectories);
             
            progressBar1.Maximum = fileNames.Length;
            OutputLine("loadROOT.......");
            int count = 0;
            foreach (var f in fileNames)
            {

                progressBar1.Value = count;
               var path = f.Remove(0, Rdir.ToString().Length+1).Replace("\\", "/");
               
                DataRow dr1 = dt.NewRow();
                dr1[1] = path;
                dt.Rows.Add(dr1);
               
                
            }
            addNodes();
            treeView1.Nodes.Add(rtn);
            progressBar1.Value = 0;
            OutputLine("loadROOT done.......");
            

        }
        private void button_InstallROOT_Click(object sender, EventArgs e)
        {
            needBackup = false;
            DialogResult r1 = MessageBox.Show("是否要备份", "标题", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "Yes")
            {
                needBackup = true;
                Backupdir = Bdir + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + "\\";
                if (!Directory.Exists(Backupdir))
                {
                    System.IO.Directory.CreateDirectory(Backupdir);
                }

                MessageBox.Show("备份文件夹 " + Backupdir);
            }
            if (r1.ToString().Equals("No"))
            {
            }
            if (r1.ToString().Equals("Cancel"))
            {
                goto cfend;
            }
            Thread t = new Thread(new ParameterizedThreadStart(Dtb));
             t.Start(ROOTdir);
          
        cfend:;
        }
        private void Dtb(object SPatch)
        {
            int l = loadedBundles[0].UncompressedSize;
            BundleRecord bundleToSave = loadedBundles[0];
            foreach (var b in loadedBundles)
            {
                if (b.UncompressedSize < l)
                {
                    l = b.UncompressedSize;
                    bundleToSave = b;
                }
            }
            
            checkTreeViewNode(treeView1.Nodes);
            
            DataRow[] dr = dt.Select("c='true'");
            progressBar1.Maximum = dr.Length;
            if (dr.Length > 0)
            {
                int count = 0;
                foreach (DataRow f in dr)
                {
                    // string fpatch=   ROOTdir.Substring(0, ROOTdir.Length-4) + f[1].ToString().Replace("/", "\\");
                
                    string fpatch = ROOTdir +"\\"+ f[1].ToString().Replace("/", "\\"); 
                 
                    System.IO.Directory.SetCurrentDirectory(mainicdir);
                    var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash(f[1].ToString())];
                    if (needBackup)
                    {

                        string fn = f[1].ToString().Replace(f[1].ToString().Substring(0, f[1].ToString().LastIndexOf("/") + 1), "");


                        string fd = Backupdir + "ROOT\\" + (f[1].ToString().Replace("/", "\\")).Replace(fn, "");
                        if (!Directory.Exists(fd))
                        {
                            System.IO.Directory.CreateDirectory(fd);
                        }
                        File.WriteAllBytes(fd + fn, fr.Read());
                    }
                    fr.Write(File.ReadAllBytes(fpatch));
                    fr.Move(bundleToSave);
                    ++count;
                    progressBar1.Value = count;
                    // OutputLine(string.Format(str, count));
                }
                if (count > 0)
                    changed.Add(bundleToSave);
                progressBar1.Value = 0;
                OutputLine("Dtb done");
            }
            else
            {
                MessageBox.Show("选取补丁内容");
            }


        }
        private void patchic(object SPatch)
        {
            int l = loadedBundles[0].UncompressedSize;
            BundleRecord bundleToSave = loadedBundles[0];
            foreach (var b in loadedBundles)
            {
                if (b.UncompressedSize < l)
                {
                    l = b.UncompressedSize;
                    bundleToSave = b;
                }
            }
            var fileNames = Directory.GetFiles(SPatch.ToString(), "*", SearchOption.AllDirectories);
            //string str = "Imported {0}/" + fileNames.Length.ToString() + " Files";
            progressBar1.Maximum = fileNames.Length;
            OutputLine("wroking.......");
            int count = 0;
            foreach (var f in fileNames)
            {
               
                progressBar1.Value = count;
                var path = f.Remove(0, SPatch.ToString().Length).Replace("\\", "/");
           
                System.IO.Directory.SetCurrentDirectory(mainicdir);
                if (path.Substring(0,1) == "/")
                {
                    path = path.Substring(1, path.Length - 1);
                }
               
                var fr = mainic.FindFiles[IndexContainer.FNV1a64Hash(path)];
                if (needBackup)
                {

                    string fn = path.ToString().Replace(path.ToString().Substring(0, path.ToString().LastIndexOf("/") + 1), "");


                    string fd = Backupdir + "ROOT\\" + (path.ToString().Replace("/", "\\")).Replace(fn, "");
                    if (!Directory.Exists(fd))
                    {
                        System.IO.Directory.CreateDirectory(fd);
                    }
                    File.WriteAllBytes(fd + fn, fr.Read());
                }
                fr.Write(File.ReadAllBytes(f));
                fr.Move(bundleToSave);
                ++count;
                //OutputLine(string.Format(str, count));
            }
            if (count > 0)
                changed.Add(bundleToSave);
            OutputLine("done.......");
            progressBar1.Value = 0;


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ti > 0)
            {
                ti = ti - 1;

            }
            else
            {
                treeView1.Enabled = true;
            }



        }
        int ti;
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
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            //textBox1.Text = treeView1.SelectedNode.Text;
        }
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            ti = 1;
            treeView1.Enabled = false;
        }
        private void treeView1_Click(object sender, EventArgs e)
        {
            ti = 1;
            treeView1.Enabled = false;
        }
        public void checkTreeViewNode(TreeNodeCollection node)
        {
            foreach (TreeNode n in node)
            {
                if (isFilesNode(n))
                // if (n.Checked && isFilesNode(n))
                {
                   //MessageBox.Show(n.FullPath);
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
        private TreeNode FindNode(TreeNode tnp, string str)
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
        private void button_etc_Click(object sender, EventArgs e)
        {
            CDB();

            needBackup = false;
            DialogResult r1 = MessageBox.Show("是否要备份", "标题", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "Yes")
            {
                needBackup = true;
                Backupdir = Bdir + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + "\\";
                if (!Directory.Exists(Backupdir))
                {
                    System.IO.Directory.CreateDirectory(Backupdir);
                }

                MessageBox.Show("备份文件夹 " + Backupdir);
            }
            if (r1.ToString().Equals("No"))
            {
            }
            if (r1.ToString().Equals("Cancel"))
            {
                goto cfend;
            }
            Thread t = new Thread(txtetc);
            t.Start();
        cfend:;
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
        private void Output(string msg)
        {
            textBoxOutput.AppendText(msg);
            textBoxOutput.SelectionStart = textBoxOutput.Text.Length;
            textBoxOutput.ScrollToCaret();
            textBoxOutput.Refresh();
        }
        private void OutputLine(string msg)
        {
            Output(msg + "\r\n");
        }



    }
}
