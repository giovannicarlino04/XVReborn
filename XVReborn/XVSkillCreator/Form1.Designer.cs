using System;
using System.Drawing;
using System.Windows.Forms;

namespace XVSkillCreator
{
    partial class Form1 : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            cbSkillType = new ComboBox();
            label3 = new Label();
            label2 = new Label();
            label5 = new Label();
            label4 = new Label();
            label1 = new Label();
            tbSkDesc = new TextBox();
            tbSkName = new TextBox();
            txtModAuthor = new TextBox();
            txtModName = new TextBox();
            tabPage2 = new TabPage();
            button2 = new Button();
            button1 = new Button();
            label7 = new Label();
            label6 = new Label();
            txtAdditionalFiles = new TextBox();
            txtSkillFiles = new TextBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            buildXVMODFileToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            copyValuesFromGameToolStripMenuItem = new ToolStripMenuItem();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 24);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(672, 500);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(cbSkillType);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(tbSkDesc);
            tabPage1.Controls.Add(tbSkName);
            tabPage1.Controls.Add(txtModAuthor);
            tabPage1.Controls.Add(txtModName);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(664, 472);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Mod Info";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbSkillType
            // 
            cbSkillType.FormattingEnabled = true;
            cbSkillType.Items.AddRange(new object[] { "Super", "Ultimate", "Evasive" });
            cbSkillType.Location = new Point(97, 99);
            cbSkillType.Name = "cbSkillType";
            cbSkillType.Size = new Size(128, 23);
            cbSkillType.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(47, 102);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 1;
            label3.Text = "Type:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(39, 73);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 1;
            label2.Text = "Author:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(23, 157);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 1;
            label5.Text = "Skill Desc:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 131);
            label4.Name = "label4";
            label4.Size = new Size(66, 15);
            label4.TabIndex = 1;
            label4.Text = "Skill Name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 44);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 1;
            label1.Text = "Name:";
            // 
            // tbSkDesc
            // 
            tbSkDesc.Location = new Point(97, 157);
            tbSkDesc.Multiline = true;
            tbSkDesc.Name = "tbSkDesc";
            tbSkDesc.Size = new Size(267, 136);
            tbSkDesc.TabIndex = 0;
            // 
            // tbSkName
            // 
            tbSkName.Location = new Point(97, 128);
            tbSkName.Name = "tbSkName";
            tbSkName.Size = new Size(267, 23);
            tbSkName.TabIndex = 0;
            // 
            // txtModAuthor
            // 
            txtModAuthor.Location = new Point(97, 70);
            txtModAuthor.Name = "txtModAuthor";
            txtModAuthor.Size = new Size(267, 23);
            txtModAuthor.TabIndex = 0;
            // 
            // txtModName
            // 
            txtModName.Location = new Point(97, 41);
            txtModName.Name = "txtModName";
            txtModName.Size = new Size(267, 23);
            txtModName.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(label7);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(txtAdditionalFiles);
            tabPage2.Controls.Add(txtSkillFiles);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(664, 472);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Files";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(420, 52);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(420, 27);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(29, 56);
            label7.Name = "label7";
            label7.Size = new Size(58, 15);
            label7.TabIndex = 1;
            label7.Text = "Add. Files";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(54, 27);
            label6.Name = "label6";
            label6.Size = new Size(33, 15);
            label6.TabIndex = 1;
            label6.Text = "Files:";
            // 
            // txtAdditionalFiles
            // 
            txtAdditionalFiles.Location = new Point(93, 53);
            txtAdditionalFiles.Name = "txtAdditionalFiles";
            txtAdditionalFiles.Size = new Size(321, 23);
            txtAdditionalFiles.TabIndex = 0;
            // 
            // txtSkillFiles
            // 
            txtSkillFiles.Location = new Point(93, 24);
            txtSkillFiles.Name = "txtSkillFiles";
            txtSkillFiles.Size = new Size(321, 23);
            txtSkillFiles.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(672, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripSeparator1, buildXVMODFileToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // buildXVMODFileToolStripMenuItem
            // 
            buildXVMODFileToolStripMenuItem.Name = "buildXVMODFileToolStripMenuItem";
            buildXVMODFileToolStripMenuItem.Size = new Size(180, 22);
            buildXVMODFileToolStripMenuItem.Text = "Build XVMOD File";
            buildXVMODFileToolStripMenuItem.Click += buildXVMODFileToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { copyValuesFromGameToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(47, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // copyValuesFromGameToolStripMenuItem
            // 
            copyValuesFromGameToolStripMenuItem.Name = "copyValuesFromGameToolStripMenuItem";
            copyValuesFromGameToolStripMenuItem.Size = new Size(200, 22);
            copyValuesFromGameToolStripMenuItem.Text = "Copy values from game";
            copyValuesFromGameToolStripMenuItem.Click += copyValuesFromGameToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(672, 524);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "Form1";
            Text = "XVSkillCreator";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Label label2;
        private Label label1;
        private TextBox txtModAuthor;
        private TextBox txtModName;
        private TabPage tabPage2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem buildXVMODFileToolStripMenuItem;
        private ComboBox cbSkillType;
        private Label label3;
        private Label label5;
        private Label label4;
        private TextBox tbSkDesc;
        private TextBox tbSkName;
        private Label label7;
        private Label label6;
        private TextBox txtAdditionalFiles;
        private TextBox txtSkillFiles;
        private Button button2;
        private Button button1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem copyValuesFromGameToolStripMenuItem;
    }
}
