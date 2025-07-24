using System;
using System.Windows.Forms;

namespace XVReborn
{
    public partial class InputForm : Form
    {
        public string UserInput { get; private set; }

        public InputForm(string prompt, string defaultValue = "")
        {
            InitializeComponent();
            label1.Text = prompt;
            textBox1.Text = defaultValue;
            UserInput = defaultValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserInput = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
