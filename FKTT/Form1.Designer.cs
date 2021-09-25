
namespace FKTT
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxOutput = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_ChangeFonts = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button4 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_loadMain = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button_InstallROOT = new System.Windows.Forms.Button();
            this.button_loadROOT = new System.Windows.Forms.Button();
            this.button_Installpatch = new System.Windows.Forms.Button();
            this.button_loadPacth = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_etc = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutput.Location = new System.Drawing.Point(0, 0);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(269, 440);
            this.textBoxOutput.TabIndex = 0;
            this.textBoxOutput.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(294, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(375, 76);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(456, 77);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(14, 106);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 23);
            this.button_Save.TabIndex = 4;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_ChangeFonts
            // 
            this.button_ChangeFonts.Location = new System.Drawing.Point(294, 13);
            this.button_ChangeFonts.Name = "button_ChangeFonts";
            this.button_ChangeFonts.Size = new System.Drawing.Size(96, 35);
            this.button_ChangeFonts.TabIndex = 5;
            this.button_ChangeFonts.Text = "ChangeFonts";
            this.button_ChangeFonts.UseVisualStyleBackColor = true;
            this.button_ChangeFonts.Click += new System.EventHandler(this.button_ChangeFonts_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(539, 440);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(537, 77);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_loadMain
            // 
            this.button_loadMain.Location = new System.Drawing.Point(14, 13);
            this.button_loadMain.Name = "button_loadMain";
            this.button_loadMain.Size = new System.Drawing.Size(75, 23);
            this.button_loadMain.TabIndex = 12;
            this.button_loadMain.Text = "loadMain";
            this.button_loadMain.UseVisualStyleBackColor = true;
            this.button_loadMain.Click += new System.EventHandler(this.button_loadMain_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(14, 135);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(558, 23);
            this.progressBar1.TabIndex = 13;
            // 
            // button_InstallROOT
            // 
            this.button_InstallROOT.Location = new System.Drawing.Point(113, 77);
            this.button_InstallROOT.Name = "button_InstallROOT";
            this.button_InstallROOT.Size = new System.Drawing.Size(104, 23);
            this.button_InstallROOT.TabIndex = 18;
            this.button_InstallROOT.Text = "InstallROOT";
            this.button_InstallROOT.UseVisualStyleBackColor = true;
            this.button_InstallROOT.Click += new System.EventHandler(this.button_InstallROOT_Click);
            // 
            // button_loadROOT
            // 
            this.button_loadROOT.Location = new System.Drawing.Point(113, 45);
            this.button_loadROOT.Name = "button_loadROOT";
            this.button_loadROOT.Size = new System.Drawing.Size(84, 23);
            this.button_loadROOT.TabIndex = 17;
            this.button_loadROOT.Text = "loadROOT";
            this.button_loadROOT.UseVisualStyleBackColor = true;
            this.button_loadROOT.Click += new System.EventHandler(this.button_loadROOT_Click);
            // 
            // button_Installpatch
            // 
            this.button_Installpatch.Location = new System.Drawing.Point(14, 77);
            this.button_Installpatch.Name = "button_Installpatch";
            this.button_Installpatch.Size = new System.Drawing.Size(93, 23);
            this.button_Installpatch.TabIndex = 16;
            this.button_Installpatch.Text = "Installpatch";
            this.button_Installpatch.UseVisualStyleBackColor = true;
            this.button_Installpatch.Click += new System.EventHandler(this.button_Installpatch_Click);
            // 
            // button_loadPacth
            // 
            this.button_loadPacth.Location = new System.Drawing.Point(14, 45);
            this.button_loadPacth.Name = "button_loadPacth";
            this.button_loadPacth.Size = new System.Drawing.Size(75, 23);
            this.button_loadPacth.TabIndex = 15;
            this.button_loadPacth.Text = "loadPacth";
            this.button_loadPacth.UseVisualStyleBackColor = true;
            this.button_loadPacth.Click += new System.EventHandler(this.button_loadPacth_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_etc);
            this.splitContainer1.Panel1.Controls.Add(this.button_loadMain);
            this.splitContainer1.Panel1.Controls.Add(this.button_Save);
            this.splitContainer1.Panel1.Controls.Add(this.button_InstallROOT);
            this.splitContainer1.Panel1.Controls.Add(this.button4);
            this.splitContainer1.Panel1.Controls.Add(this.button_loadROOT);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.button_Installpatch);
            this.splitContainer1.Panel1.Controls.Add(this.button_ChangeFonts);
            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.button_loadPacth);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer1.Panel1MinSize = 120;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(812, 644);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 19;
            // 
            // button_etc
            // 
            this.button_etc.Location = new System.Drawing.Point(396, 13);
            this.button_etc.Name = "button_etc";
            this.button_etc.Size = new System.Drawing.Size(97, 35);
            this.button_etc.TabIndex = 19;
            this.button_etc.Text = "临时汉化";
            this.button_etc.UseVisualStyleBackColor = true;
            this.button_etc.Click += new System.EventHandler(this.button_etc_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textBoxOutput);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.treeView1);
            this.splitContainer2.Size = new System.Drawing.Size(812, 440);
            this.splitContainer2.SplitterDistance = 269;
            this.splitContainer2.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 644);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBoxOutput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_ChangeFonts;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_loadMain;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button_loadPacth;
        private System.Windows.Forms.Button button_Installpatch;
        private System.Windows.Forms.Button button_loadROOT;
        private System.Windows.Forms.Button button_InstallROOT;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button_etc;
    }
}

