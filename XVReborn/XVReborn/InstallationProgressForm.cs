using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XVReborn
{
    public partial class InstallationProgressForm : Form
    {
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label lblProgress;
        private Button btnCancel;
        private bool isCancelled = false;

        public InstallationProgressForm(string title = "Installation Progress")
        {
            InitializeComponents(title);
        }

        private void InitializeComponents(string title)
        {
            this.Text = title;
            this.Size = new Size(400, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;

            // Status label
            lblStatus = new Label
            {
                Text = "Preparing installation...",
                Location = new Point(20, 20),
                Size = new Size(350, 20),
                Font = new Font("Arial", 9)
            };
            this.Controls.Add(lblStatus);

            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(20, 50),
                Size = new Size(350, 23),
                Style = ProgressBarStyle.Continuous,
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };
            this.Controls.Add(progressBar);

            // Progress percentage label
            lblProgress = new Label
            {
                Text = "0%",
                Location = new Point(20, 80),
                Size = new Size(350, 20),
                Font = new Font("Arial", 9),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblProgress);

            // Cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(150, 110),
                Size = new Size(80, 25),
                Font = new Font("Arial", 9)
            };
            btnCancel.Click += (sender, e) => 
            {
                isCancelled = true;
                btnCancel.Enabled = false;
                lblStatus.Text = "Cancelling...";
            };
            this.Controls.Add(btnCancel);
        }

        public void UpdateProgress(int percentage, string status = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgress(percentage, status)));
                return;
            }

            progressBar.Value = Math.Min(percentage, 100);
            lblProgress.Text = $"{percentage}%";
            
            if (!string.IsNullOrEmpty(status))
            {
                lblStatus.Text = status;
            }
        }

        public void UpdateStatus(string status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateStatus(status)));
                return;
            }

            lblStatus.Text = status;
        }

        public bool IsCancelled => isCancelled;

        public void SetCancellable(bool cancellable)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SetCancellable(cancellable)));
                return;
            }

            btnCancel.Enabled = cancellable;
        }

        public void Complete(string finalStatus = "Installation completed!")
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Complete(finalStatus)));
                return;
            }

            progressBar.Value = 100;
            lblProgress.Text = "100%";
            lblStatus.Text = finalStatus;
            btnCancel.Text = "Close";
            btnCancel.Enabled = true;
            btnCancel.Click += (sender, e) => this.Close();
        }

        public void ShowError(string errorMessage)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowError(errorMessage)));
                return;
            }

            lblStatus.Text = $"Error: {errorMessage}";
            btnCancel.Text = "Close";
            btnCancel.Enabled = true;
            btnCancel.Click += (sender, e) => this.Close();
        }
    }

    public class ProgressReporter
    {
        private readonly InstallationProgressForm progressForm;
        private readonly IProgress<int> progress;

        public ProgressReporter(InstallationProgressForm form)
        {
            progressForm = form;
            progress = new Progress<int>(percentage => progressForm.UpdateProgress(percentage));
        }

        public void ReportProgress(int percentage, string status = null)
        {
            progress.Report(percentage);
            if (!string.IsNullOrEmpty(status))
            {
                progressForm.UpdateStatus(status);
            }
        }

        public void ReportStatus(string status)
        {
            progressForm.UpdateStatus(status);
        }

        public bool IsCancelled => progressForm.IsCancelled;
    }
} 