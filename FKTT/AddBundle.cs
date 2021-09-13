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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FKTT
{
    public partial class AddBundle : Form
    {
        public AddBundle()
        {
            InitializeComponent();
        }
        public IndexContainer ic;
        public string cddir = System.IO.Directory.GetCurrentDirectory();
        DataTable dt = new DataTable();
        private void AddBundle_Load(object sender, EventArgs e)
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
        }
        private void button1_Click(object sender, EventArgs e)
        {

            string zipPath;


            var ofd = new OpenFileDialog
            {
                DefaultExt = "zip",

            };
            if (ofd.ShowDialog() == DialogResult.OK)
                zipPath = ofd.FileName;
            else
            {
                Close();
                return;
            }
            var zz = ZipFile.OpenRead(zipPath);
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
                            
                            foreach (var item3 in ic.Bundles)
                            {
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
        public void addNodes()
        {

            DataTable dtCopy = dt.Copy();

            DataView dv = dt.DefaultView;
            dv.Sort = "patch";
            dtCopy = dv.ToTable();
            dt = dtCopy;
            TreeNode rtn = new TreeNode("ROOT");
            treeView1.Nodes.Add(rtn);

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
        public void checkTreeViewNode(TreeNodeCollection node)
        {
            foreach (TreeNode n in node)
            {
                if (isFilesNode(n))
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
        public delegate void GetFromAdd(object str);
        public event GetFromAdd SendToMain;
        private void button2_Click(object sender, EventArgs e)
        {
            string backupdir = cddir + @"\Backup\" + DateTime.Now.ToString("yyyyMMddHHmm");
            if (!Directory.Exists(backupdir))
            {
                System.IO.Directory.CreateDirectory(backupdir);
            }
            checkTreeViewNode(treeView1.Nodes);
            DataRow[] dr = dt.Select("c='true'");
            label1.Text = dr.Length.ToString();



            string str = "Imported {0}/" + dr.Length.ToString() + " Files";
           
            foreach (DataRow f in dr)
            {

                var fr = ic.FindFiles[IndexContainer.FNV1a64Hash(f[1].ToString())];
                string patch = backupdir + "\\" + f[1].ToString().Replace("/","\\");
                string patchd = patch.Substring(0, patch.LastIndexOf("\\"));
                if (!Directory.Exists(patchd))
                {
                    System.IO.Directory.CreateDirectory(patchd);
                }
                File.WriteAllBytes(patch, fr.Read());
            }
            SendToMain(backupdir);
        }
    }
}
