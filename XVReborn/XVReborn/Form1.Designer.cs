using System;
using System.Windows.Forms;

namespace XVReborn
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.installModsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCHARASELEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearInstallationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.installModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.compileScriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearInstallationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.ch = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.editPSCFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvMods = new System.Windows.Forms.ListView();
            this.ch1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.MainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(859, 24);
            this.MainMenuStrip.TabIndex = 2;
            this.MainMenuStrip.Text = "menuStrip122";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.installModsToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(225, 22);
            this.toolStripMenuItem1.Text = "Install Mods";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.installmod);
            // 
            // installModsToolStripMenuItem
            // 
            this.installModsToolStripMenuItem.Name = "installModsToolStripMenuItem";
            this.installModsToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.installModsToolStripMenuItem.Text = "Install X2M (EXPERIMENTAL)";
            this.installModsToolStripMenuItem.Click += new System.EventHandler(this.installx2m);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(222, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCHARASELEToolStripMenuItem,
            this.toolStripSeparator3,
            this.clearInstallationToolStripMenuItem1});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // editCHARASELEToolStripMenuItem
            // 
            this.editCHARASELEToolStripMenuItem.Name = "editCHARASELEToolStripMenuItem";
            this.editCHARASELEToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.editCHARASELEToolStripMenuItem.Text = "Charalist Editor";
            this.editCHARASELEToolStripMenuItem.Click += new System.EventHandler(this.editCHARASELEToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(159, 6);
            // 
            // clearInstallationToolStripMenuItem1
            // 
            this.clearInstallationToolStripMenuItem1.Name = "clearInstallationToolStripMenuItem1";
            this.clearInstallationToolStripMenuItem1.Size = new System.Drawing.Size(162, 22);
            this.clearInstallationToolStripMenuItem1.Text = "Clear Installation";
            this.clearInstallationToolStripMenuItem1.Click += new System.EventHandler(this.clearInstallationToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem22
            // 
            this.fileToolStripMenuItem22.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installModToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem22.Name = "fileToolStripMenuItem22";
            this.fileToolStripMenuItem22.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem22.Text = "File";
            // 
            // installModToolStripMenuItem
            // 
            this.installModToolStripMenuItem.Name = "installModToolStripMenuItem";
            this.installModToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.installModToolStripMenuItem.Text = "Install Mod";
            this.installModToolStripMenuItem.Click += new System.EventHandler(this.installmod);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(130, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.closeToolStripMenuItem.Text = "Exit";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem22
            // 
            this.toolsToolStripMenuItem22.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileScriptsToolStripMenuItem,
            this.toolStripSeparator1,
            this.clearInstallationToolStripMenuItem});
            this.toolsToolStripMenuItem22.Name = "toolsToolStripMenuItem22";
            this.toolsToolStripMenuItem22.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem22.Text = "Tools";
            // 
            // compileScriptsToolStripMenuItem
            // 
            this.compileScriptsToolStripMenuItem.Name = "compileScriptsToolStripMenuItem";
            this.compileScriptsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.compileScriptsToolStripMenuItem.Text = "Compile Scripts";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // clearInstallationToolStripMenuItem
            // 
            this.clearInstallationToolStripMenuItem.Name = "clearInstallationToolStripMenuItem";
            this.clearInstallationToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.clearInstallationToolStripMenuItem.Text = "Clear Installation";
            this.clearInstallationToolStripMenuItem.Click += new System.EventHandler(this.clearInstallationToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 455);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Installed Mods: 0";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6});
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(37, 19);
            this.toolStripMenuItem5.Text = "File";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(98, 22);
            this.toolStripMenuItem6.Text = "Save";
            // 
            // editToolStripMenuItem5
            // 
            this.editToolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editPSCFileToolStripMenuItem});
            this.editToolStripMenuItem5.Name = "editToolStripMenuItem5";
            this.editToolStripMenuItem5.Size = new System.Drawing.Size(39, 19);
            this.editToolStripMenuItem5.Text = "Edit";
            // 
            // editPSCFileToolStripMenuItem
            // 
            this.editPSCFileToolStripMenuItem.Name = "editPSCFileToolStripMenuItem";
            this.editPSCFileToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.editPSCFileToolStripMenuItem.Text = "Edit .PSC File";
            // 
            // lvMods
            // 
            this.lvMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch1,
            this.ch2,
            this.ch3});
            this.lvMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMods.HideSelection = false;
            this.lvMods.Location = new System.Drawing.Point(0, 24);
            this.lvMods.MultiSelect = false;
            this.lvMods.Name = "lvMods";
            this.lvMods.Size = new System.Drawing.Size(859, 431);
            this.lvMods.TabIndex = 5;
            this.lvMods.UseCompatibleStateImageBehavior = false;
            this.lvMods.View = System.Windows.Forms.View.Details;
            // 
            // ch1
            // 
            this.ch1.Text = "Name";
            this.ch1.Width = 116;
            // 
            // ch2
            // 
            this.ch2.Text = "Author";
            this.ch2.Width = 122;
            // 
            // ch3
            // 
            this.ch3.Text = "Type";
            this.ch3.Width = 591;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 470);
            this.Controls.Add(this.lvMods);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MainMenuStrip);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "XVReborn";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem22;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installModToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem22;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem clearInstallationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem editPSCFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileScriptsToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader header;
        private System.Windows.Forms.ColumnHeader ch;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem installModsToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem clearInstallationToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ListView lvMods;
        private ColumnHeader ch1;
        private ColumnHeader ch2;
        private ColumnHeader ch3;
        private ToolStripMenuItem editCHARASELEToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
    }
}