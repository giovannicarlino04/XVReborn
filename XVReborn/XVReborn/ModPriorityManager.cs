using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XVReborn.Properties;

namespace XVReborn
{
    public class ModPriorityManager
    {
        private readonly string dataFolder;
        private readonly string priorityFile;

        public ModPriorityManager()
        {
            dataFolder = Settings.Default.datafolder;
            priorityFile = Path.Combine(dataFolder, "mod_priority.txt");
        }

        public List<string> GetModPriorityList()
        {
            if (!File.Exists(priorityFile))
                return new List<string>();

            try
            {
                return File.ReadAllLines(priorityFile)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading mod priority: {ex.Message}");
                return new List<string>();
            }
        }

        public void SaveModPriorityList(List<string> modList)
        {
            try
            {
                File.WriteAllLines(priorityFile, modList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving mod priority: {ex.Message}", "Save Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void MoveModUp(ListView lvMods, int currentIndex)
        {
            if (currentIndex <= 0 || currentIndex >= lvMods.Items.Count)
                return;

            var item = lvMods.Items[currentIndex];
            lvMods.Items.RemoveAt(currentIndex);
            lvMods.Items.Insert(currentIndex - 1, item);
            lvMods.Items[currentIndex - 1].Selected = true;

            UpdateModPriorityFromListView(lvMods);
        }

        public void MoveModDown(ListView lvMods, int currentIndex)
        {
            if (currentIndex < 0 || currentIndex >= lvMods.Items.Count - 1)
                return;

            var item = lvMods.Items[currentIndex];
            lvMods.Items.RemoveAt(currentIndex);
            lvMods.Items.Insert(currentIndex + 1, item);
            lvMods.Items[currentIndex + 1].Selected = true;

            UpdateModPriorityFromListView(lvMods);
        }

        public void MoveModToTop(ListView lvMods, int currentIndex)
        {
            if (currentIndex <= 0 || currentIndex >= lvMods.Items.Count)
                return;

            var item = lvMods.Items[currentIndex];
            lvMods.Items.RemoveAt(currentIndex);
            lvMods.Items.Insert(0, item);
            lvMods.Items[0].Selected = true;

            UpdateModPriorityFromListView(lvMods);
        }

        public void MoveModToBottom(ListView lvMods, int currentIndex)
        {
            if (currentIndex < 0 || currentIndex >= lvMods.Items.Count - 1)
                return;

            var item = lvMods.Items[currentIndex];
            lvMods.Items.RemoveAt(currentIndex);
            lvMods.Items.Add(item);
            lvMods.Items[lvMods.Items.Count - 1].Selected = true;

            UpdateModPriorityFromListView(lvMods);
        }

        private void UpdateModPriorityFromListView(ListView lvMods)
        {
            var modList = new List<string>();
            
            foreach (ListViewItem item in lvMods.Items)
            {
                var modName = item.SubItems[0].Text; // Assuming mod name is in first column
                modList.Add(modName);
            }

            SaveModPriorityList(modList);
        }

        public void SortModsByPriority(ListView lvMods)
        {
            var priorityList = GetModPriorityList();
            if (priorityList.Count == 0)
                return;

            var items = new List<ListViewItem>();
            foreach (ListViewItem item in lvMods.Items)
            {
                items.Add(item);
            }

            // Sort items based on priority list
            var sortedItems = items.OrderBy(item => 
            {
                var modName = item.SubItems[0].Text;
                var index = priorityList.IndexOf(modName);
                return index >= 0 ? index : int.MaxValue;
            }).ToList();

            lvMods.Items.Clear();
            foreach (var item in sortedItems)
            {
                lvMods.Items.Add(item);
            }
        }

        public void EnableDragAndDrop(ListView lvMods)
        {
            lvMods.AllowDrop = true;
            lvMods.ItemDrag += (sender, e) => 
            {
                if (e.Item != null)
                {
                    lvMods.DoDragDrop(e.Item, DragDropEffects.Move);
                }
            };

            lvMods.DragEnter += (sender, e) => 
            {
                if (e.Data.GetDataPresent(typeof(ListViewItem)))
                {
                    e.Effect = DragDropEffects.Move;
                }
            };

            lvMods.DragDrop += (sender, e) => 
            {
                if (e.Data.GetDataPresent(typeof(ListViewItem)))
                {
                    var draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                    var targetPoint = lvMods.PointToClient(new System.Drawing.Point(e.X, e.Y));
                    var targetItem = lvMods.GetItemAt(targetPoint.X, targetPoint.Y);

                    if (targetItem != null && draggedItem != targetItem)
                    {
                        var draggedIndex = draggedItem.Index;
                        var targetIndex = targetItem.Index;

                        lvMods.Items.RemoveAt(draggedIndex);
                        lvMods.Items.Insert(targetIndex, draggedItem);
                        draggedItem.Selected = true;

                        UpdateModPriorityFromListView(lvMods);
                    }
                }
            };
        }
    }
} 