using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XVReborn.Properties;

namespace XVReborn
{
    public class InstallationHistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; } // "Install", "Uninstall", "Enable", "Disable"
        public string ModName { get; set; }
        public string ModType { get; set; }
        public string ModAuthor { get; set; }
        public string ModVersion { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string UserNotes { get; set; }
    }

    public class ModInstallationHistory
    {
        private readonly string historyFile;
        private readonly string dataFolder;
        private readonly List<InstallationHistoryEntry> historyCache;

        public ModInstallationHistory()
        {
            dataFolder = Settings.Default.datafolder;
            historyFile = Path.Combine(dataFolder, "installation_history.txt");
            historyCache = new List<InstallationHistoryEntry>();
            LoadHistory();
        }

        public void LogInstallation(string modName, string modType, string modAuthor = "", string modVersion = "", bool success = true, string errorMessage = "", string userNotes = "")
        {
            var entry = new InstallationHistoryEntry
            {
                Timestamp = DateTime.Now,
                Action = "Install",
                ModName = modName,
                ModType = modType,
                ModAuthor = modAuthor,
                ModVersion = modVersion,
                Success = success,
                ErrorMessage = errorMessage,
                UserNotes = userNotes
            };

            AddHistoryEntry(entry);
        }

        public void LogUninstallation(string modName, string modType, string modAuthor = "", string modVersion = "", bool success = true, string errorMessage = "", string userNotes = "")
        {
            var entry = new InstallationHistoryEntry
            {
                Timestamp = DateTime.Now,
                Action = "Uninstall",
                ModName = modName,
                ModType = modType,
                ModAuthor = modAuthor,
                ModVersion = modVersion,
                Success = success,
                ErrorMessage = errorMessage,
                UserNotes = userNotes
            };

            AddHistoryEntry(entry);
        }

        public void LogEnable(string modName, string modType, bool success = true, string errorMessage = "")
        {
            var entry = new InstallationHistoryEntry
            {
                Timestamp = DateTime.Now,
                Action = "Enable",
                ModName = modName,
                ModType = modType,
                Success = success,
                ErrorMessage = errorMessage
            };

            AddHistoryEntry(entry);
        }

        public void LogDisable(string modName, string modType, bool success = true, string errorMessage = "")
        {
            var entry = new InstallationHistoryEntry
            {
                Timestamp = DateTime.Now,
                Action = "Disable",
                ModName = modName,
                ModType = modType,
                Success = success,
                ErrorMessage = errorMessage
            };

            AddHistoryEntry(entry);
        }

        private void AddHistoryEntry(InstallationHistoryEntry entry)
        {
            historyCache.Add(entry);
            SaveHistory();
        }

        private void LoadHistory()
        {
            if (!File.Exists(historyFile))
                return;

            try
            {
                var lines = File.ReadAllLines(historyFile);
                historyCache.Clear();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var entry = ParseHistoryEntry(line);
                    if (entry != null)
                    {
                        historyCache.Add(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading history: {ex.Message}");
            }
        }

        private void SaveHistory()
        {
            try
            {
                var lines = new List<string>();
                
                foreach (var entry in historyCache)
                {
                    lines.Add(SerializeHistoryEntry(entry));
                }

                File.WriteAllLines(historyFile, lines);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving history: {ex.Message}");
            }
        }

        private string SerializeHistoryEntry(InstallationHistoryEntry entry)
        {
            var parts = new[]
            {
                entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                entry.Action,
                entry.ModName ?? "",
                entry.ModType ?? "",
                entry.ModAuthor ?? "",
                entry.ModVersion ?? "",
                entry.Success.ToString(),
                entry.ErrorMessage ?? "",
                entry.UserNotes ?? ""
            };

            return string.Join("|", parts.Select(p => p.Replace("|", "\\|")));
        }

        private InstallationHistoryEntry ParseHistoryEntry(string line)
        {
            try
            {
                var parts = line.Split('|');
                if (parts.Length < 7)
                    return null;

                var entry = new InstallationHistoryEntry
                {
                    Timestamp = DateTime.Parse(parts[0]),
                    Action = parts[1],
                    ModName = parts[2],
                    ModType = parts[3],
                    ModAuthor = parts[4],
                    ModVersion = parts[5],
                    Success = bool.Parse(parts[6]),
                    ErrorMessage = parts.Length > 7 ? parts[7] : "",
                    UserNotes = parts.Length > 8 ? parts[8] : ""
                };

                return entry;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing history entry: {ex.Message}");
                return null;
            }
        }

        public List<InstallationHistoryEntry> GetHistory(DateTime? fromDate = null, DateTime? toDate = null, string action = null, string modName = null)
        {
            var query = historyCache.AsEnumerable();

            if (fromDate.HasValue)
                query = query.Where(e => e.Timestamp >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(e => e.Timestamp <= toDate.Value);

            if (!string.IsNullOrEmpty(action))
                query = query.Where(e => e.Action.ToLower().Equals(action.ToLower()));

            if (!string.IsNullOrEmpty(modName))
                query = query.Where(e => e.ModName.ToLower().Contains(modName.ToLower()));

            return query.OrderByDescending(e => e.Timestamp).ToList();
        }

        public void ShowHistoryDialog()
        {
            var historyForm = new InstallationHistoryForm(this);
            historyForm.ShowDialog();
        }

        public void ExportHistory(string filePath, DateTime? fromDate = null, DateTime? toDate = null, string action = null)
        {
            try
            {
                var history = GetHistory(fromDate, toDate, action);
                var csv = new StringBuilder();

                // Header
                csv.AppendLine("Timestamp,Action,ModName,ModType,ModAuthor,ModVersion,Success,ErrorMessage,UserNotes");

                // Data
                foreach (var entry in history)
                {
                    csv.AppendLine($"\"{entry.Timestamp:yyyy-MM-dd HH:mm:ss}\"," +
                                  $"\"{entry.Action}\"," +
                                  $"\"{entry.ModName}\"," +
                                  $"\"{entry.ModType}\"," +
                                  $"\"{entry.ModAuthor}\"," +
                                  $"\"{entry.ModVersion}\"," +
                                  $"\"{entry.Success}\"," +
                                  $"\"{entry.ErrorMessage}\"," +
                                  $"\"{entry.UserNotes}\"");
                }

                File.WriteAllText(filePath, csv.ToString());
                MessageBox.Show($"History exported to: {filePath}", "Export Complete", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export history: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ClearHistory()
        {
            var result = MessageBox.Show("This will permanently delete all installation history. Continue?", 
                "Confirm Clear History", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                historyCache.Clear();
                if (File.Exists(historyFile))
                {
                    File.Delete(historyFile);
                }

                MessageBox.Show("Installation history has been cleared.", "History Cleared", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to clear history: {ex.Message}", "Clear Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CleanupOldHistory(int daysToKeep = 30)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                var oldEntries = historyCache.Where(e => e.Timestamp < cutoffDate).ToList();

                foreach (var entry in oldEntries)
                {
                    historyCache.Remove(entry);
                }

                if (oldEntries.Count > 0)
                {
                    SaveHistory();
                    MessageBox.Show($"Removed {oldEntries.Count} old history entries.", "Cleanup Complete", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to cleanup old history: {ex.Message}", "Cleanup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public InstallationHistoryEntry GetLastInstallation(string modName)
        {
            return historyCache
                .Where(e => e.ModName == modName && e.Action == "Install")
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefault();
        }

        public List<InstallationHistoryEntry> GetModHistory(string modName)
        {
            return historyCache
                .Where(e => e.ModName == modName)
                .OrderByDescending(e => e.Timestamp)
                .ToList();
        }

        public int GetTotalInstallations()
        {
            return historyCache.Count(e => e.Action == "Install" && e.Success);
        }

        public int GetTotalUninstallations()
        {
            return historyCache.Count(e => e.Action == "Uninstall" && e.Success);
        }

        public string GetMostInstalledMod()
        {
            return historyCache
                .Where(e => e.Action == "Install" && e.Success)
                .GroupBy(e => e.ModName)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }
    }

    public class InstallationHistoryForm : Form
    {
        private readonly ModInstallationHistory history;
        private DataGridView dgvHistory;
        private ComboBox cmbFilter;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnRefresh;
        private Button btnExport;
        private Button btnClear;

        public InstallationHistoryForm(ModInstallationHistory history)
        {
            this.history = history;
            InitializeComponents();
            LoadHistory();
        }

        private void InitializeComponents()
        {
            this.Text = "Installation History";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Filter controls
            var filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50
            };

            var lblFilter = new Label
            {
                Text = "Filter:",
                Location = new System.Drawing.Point(10, 15),
                Size = new System.Drawing.Size(40, 20)
            };
            filterPanel.Controls.Add(lblFilter);

            cmbFilter = new ComboBox
            {
                Location = new System.Drawing.Point(60, 12),
                Size = new System.Drawing.Size(100, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilter.Items.AddRange(new object[] { "All", "Install", "Uninstall", "Enable", "Disable" });
            cmbFilter.SelectedIndex = 0;
            filterPanel.Controls.Add(cmbFilter);

            var lblFrom = new Label
            {
                Text = "From:",
                Location = new System.Drawing.Point(180, 15),
                Size = new System.Drawing.Size(40, 20)
            };
            filterPanel.Controls.Add(lblFrom);

            dtpFrom = new DateTimePicker
            {
                Location = new System.Drawing.Point(220, 12),
                Size = new System.Drawing.Size(120, 20),
                Format = DateTimePickerFormat.Short
            };
            filterPanel.Controls.Add(dtpFrom);

            var lblTo = new Label
            {
                Text = "To:",
                Location = new System.Drawing.Point(360, 15),
                Size = new System.Drawing.Size(30, 20)
            };
            filterPanel.Controls.Add(lblTo);

            dtpTo = new DateTimePicker
            {
                Location = new System.Drawing.Point(390, 12),
                Size = new System.Drawing.Size(120, 20),
                Format = DateTimePickerFormat.Short
            };
            filterPanel.Controls.Add(dtpTo);

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(530, 10),
                Size = new System.Drawing.Size(70, 25)
            };
            btnRefresh.Click += (sender, e) => LoadHistory();
            filterPanel.Controls.Add(btnRefresh);

            btnExport = new Button
            {
                Text = "Export",
                Location = new System.Drawing.Point(610, 10),
                Size = new System.Drawing.Size(70, 25)
            };
            btnExport.Click += (sender, e) => ExportHistory();
            filterPanel.Controls.Add(btnExport);

            btnClear = new Button
            {
                Text = "Clear",
                Location = new System.Drawing.Point(690, 10),
                Size = new System.Drawing.Size(70, 25)
            };
            btnClear.Click += (sender, e) => ClearHistory();
            filterPanel.Controls.Add(btnClear);

            this.Controls.Add(filterPanel);

            // History grid
            dgvHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgvHistory);
        }

        private void LoadHistory()
        {
            var action = cmbFilter.SelectedItem?.ToString();
            if (action == "All") action = null;

            var history = this.history.GetHistory(dtpFrom.Value, dtpTo.Value, action);
            
            dgvHistory.DataSource = history;
        }

        private void ExportHistory()
        {
            var saveDialog = new SaveFileDialog
            {
                Title = "Export Installation History",
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                DefaultExt = "csv"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var action = cmbFilter.SelectedItem?.ToString();
                if (action == "All") action = null;

                history.ExportHistory(saveDialog.FileName, dtpFrom.Value, dtpTo.Value, action);
            }
        }

        private void ClearHistory()
        {
            history.ClearHistory();
            LoadHistory();
        }
    }
} 