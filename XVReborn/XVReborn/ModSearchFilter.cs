using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XVReborn
{
    public class ModSearchFilter
    {
        private ListView originalListView;
        private List<ListViewItem> allItems;
        private string currentFilter = "";
        private string currentCategory = "All";

        public ModSearchFilter(ListView listView)
        {
            originalListView = listView;
            allItems = new List<ListViewItem>();
            StoreAllItems();
        }

        public void StoreAllItems()
        {
            allItems.Clear();
            foreach (ListViewItem item in originalListView.Items)
            {
                allItems.Add(item.Clone() as ListViewItem);
            }
        }

        public void ApplyFilter(string searchText, string category = "All")
        {
            currentFilter = searchText?.ToLower() ?? "";
            currentCategory = category ?? "All";

            originalListView.Items.Clear();

            var filteredItems = allItems.Where(item => 
                MatchesFilter(item) && MatchesCategory(item)).ToList();

            foreach (var item in filteredItems)
            {
                originalListView.Items.Add(item);
            }

            UpdateFilterStatus();
        }

        private bool MatchesFilter(ListViewItem item)
        {
            if (string.IsNullOrEmpty(currentFilter))
                return true;

            // Search in mod name (column 0), type (column 1), and author (column 2 if exists)
            for (int i = 0; i < Math.Min(item.SubItems.Count, 3); i++)
            {
                if (item.SubItems[i].Text.ToLower().Contains(currentFilter))
                    return true;
            }

            return false;
        }

        private bool MatchesCategory(ListViewItem item)
        {
            if (currentCategory == "All")
                return true;

            if (item.SubItems.Count > 1)
            {
                var modType = item.SubItems[1].Text;
                return modType.ToLower().Equals(currentCategory.ToLower());
            }

            return false;
        }

        public void ClearFilter()
        {
            currentFilter = "";
            currentCategory = "All";
            RestoreAllItems();
        }

        public void RestoreAllItems()
        {
            originalListView.Items.Clear();
            foreach (var item in allItems)
            {
                originalListView.Items.Add(item.Clone() as ListViewItem);
            }
            UpdateFilterStatus();
        }

        public List<string> GetAvailableCategories()
        {
            var categories = new HashSet<string> { "All" };
            
            foreach (var item in allItems)
            {
                if (item.SubItems.Count > 1)
                {
                    categories.Add(item.SubItems[1].Text);
                }
            }

            return categories.OrderBy(c => c).ToList();
        }

        public int GetFilteredCount()
        {
            return originalListView.Items.Count;
        }

        public int GetTotalCount()
        {
            return allItems.Count;
        }

        private void UpdateFilterStatus()
        {
            // This could be used to update a status label showing filter results
            // For now, we'll just update the form title or a status bar if available
        }

        public void RefreshFilter()
        {
            StoreAllItems();
            ApplyFilter(currentFilter, currentCategory);
        }

        public void AddItem(ListViewItem item)
        {
            allItems.Add(item.Clone() as ListViewItem);
            if (MatchesFilter(item) && MatchesCategory(item))
            {
                originalListView.Items.Add(item.Clone() as ListViewItem);
            }
        }

        public void RemoveItem(ListViewItem item)
        {
            var itemToRemove = allItems.FirstOrDefault(i => 
                i.SubItems[0].Text == item.SubItems[0].Text && 
                i.SubItems[1].Text == item.SubItems[1].Text);

            if (itemToRemove != null)
            {
                allItems.Remove(itemToRemove);
            }

            // Remove from visible list if it's there
            for (int i = originalListView.Items.Count - 1; i >= 0; i--)
            {
                var visibleItem = originalListView.Items[i];
                if (visibleItem.SubItems[0].Text == item.SubItems[0].Text && 
                    visibleItem.SubItems[1].Text == item.SubItems[1].Text)
                {
                    originalListView.Items.RemoveAt(i);
                }
            }
        }

        public void UpdateItem(ListViewItem oldItem, ListViewItem newItem)
        {
            RemoveItem(oldItem);
            AddItem(newItem);
        }
    }
} 