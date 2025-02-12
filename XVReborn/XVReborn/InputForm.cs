using System.Windows.Forms;
using System;

public partial class InputForm : Form
{
    // Property to get the user input
    public string UserInput => txtInput.Text;

    public InputForm(string prompt, string defaultValue = "")
    {
        InitializeComponent();

        // Set the prompt text
        lblPrompt.Text = prompt;

        // Set the default value for the TextBox
        txtInput.Text = defaultValue;
    }

    // OK button click event
    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtInput.Text))
        {
            MessageBox.Show("Input cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
            DialogResult = DialogResult.OK;
        }
    }

    // Cancel button click event
    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private TextBox txtInput;
    private Button btnOk;
    private Button btnCancel;
    private Label lblPrompt;

    private void InitializeComponent()
    {
        // Initialize the controls
        this.txtInput = new TextBox();
        this.txtInput.Dock = DockStyle.Top;
        this.Controls.Add(this.txtInput);

        this.lblPrompt = new Label();
        this.lblPrompt.Dock = DockStyle.Top;
        this.Controls.Add(this.lblPrompt);

        this.btnOk = new Button();
        this.btnOk.Text = "OK";
        this.btnOk.Dock = DockStyle.Bottom;
        this.btnOk.Click += new EventHandler(this.btnOk_Click);
        this.Controls.Add(this.btnOk);

        this.btnCancel = new Button();
        this.btnCancel.Text = "Cancel";
        this.btnCancel.Dock = DockStyle.Bottom;
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
        this.Controls.Add(this.btnCancel);

        this.Text = "Input";
    }
}
