using System;
using System.IO;
using System.Windows.Forms;

namespace XVReborn
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && Directory.Exists(textBox1.Text))
            {
                Properties.Settings.Default.datafolder = textBox1.Text + @"/data";
                Properties.Settings.Default.Xenofolder = textBox1.Text;
                Properties.Settings.Default.Save();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog data = new FolderBrowserDialog();
            data.Description = "Locate data folder";

            if (data.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = data.SelectedPath;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string XVSteamPath = @"C:\Program Files (x86)\Steam\steamapps\common\DB Xenoverse";

            if (Directory.Exists(XVSteamPath))
            {
                textBox1.Text = XVSteamPath;
            }
        }
    }
}
