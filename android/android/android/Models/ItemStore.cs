using android.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace android.Models
{
    class ItemStore
    {
        private Dictionary<string, Item> items = new Dictionary<string, Item>();

        public ObservableCollection<Item> ObservableItems { get; set; } = new ObservableCollection<Item>();

        public void HandleSnapshot(Items items)
        {
            foreach (var item in items.items)
            {
                if (!this.items.ContainsKey(item.Id)) {
                    this.items.Add(item.Id, item);
                    ObservableItems.Add(item);
                }
            }

            SortObservable();
        }

        public void SortObservable()
        {            
            var sorted = ObservableItems.OrderBy(x => x).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                var curr = ObservableItems.IndexOf(sorted[i]);

                if (curr != i)
                {
                    ObservableItems.Move(curr, i);
                }
            }
        }
    }
}
