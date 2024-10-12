using System;
using System.IO;
using System.Windows.Forms;
using XVCharaCreator.Properties;

namespace XVReborn
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Settings.Default.language = "ca";
                this.Close();
            }
            else if (checkBox2.Checked)
            {
                Settings.Default.language = "de";
                this.Close();
            }
            else if (checkBox3.Checked)
            {
                Settings.Default.language = "en";
                this.Close();
            }
            else if (checkBox4.Checked)
            {
                Settings.Default.language = "es";
                this.Close();
            }
            else if (checkBox5.Checked)
            {
                Settings.Default.language = "fr";
                this.Close();
            }
            else if (checkBox6.Checked)
            {
                Settings.Default.language = "it";
                this.Close();
            }
            else if (checkBox7.Checked)
            {
                Settings.Default.language = "pl";
                this.Close();
            }
            else if (checkBox8.Checked)
            {
                Settings.Default.language = "pt";
                this.Close();
            }
            else if (checkBox9.Checked)
            {
                Settings.Default.language = "ru";
                this.Close();
            }

            Settings.Default.Save();
        }
    }
}
