using android.Services;
using android.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;

namespace android.Models
{
    public class ItemStore
    {
        private Dictionary<string, Item> items = new Dictionary<string, Item>();

        public ObservableCollection<Item> ObservableItems { get; set; } = new ObservableCollection<Item>();

        //for now 0, but in the future we need to fetch this from db.
        public int lastKnownServerUpdate = 0;

        public void HandleSnapshot(List<Item> items)
        {
            foreach (var item in items)
            {
                if (!this.items.ContainsKey(item.Id)) {
                    AddItemImpl(item);
                } else
                {
                    var localItem = this.items[item.Id];
                    if(localItem.LastUpdate < item.LastUpdate)
                    {
                        OverrideItem(localItem, item);
                    }
                }
            }

            SortObservable();
        }

        public void RemoveSelected()
        {
            var toRemove = items.Select(e => e.Value).Where(i => i.Bought).ToList();

            foreach (var item in toRemove)
            {
                items.Remove(item.Id);
            }

            SortObservable();
        }

        public void AddItem(Item item)
        {
            AddItemImpl(item);
            SortObservable();
        }

        public void SortObservable()
        {            
            ObservableItems.Clear();
            foreach (var e in items)
            {
                ObservableItems.Add(e.Value);
            }

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

        public List<Item> ItemSnapshot()
        {
            return items.Select(e => e.Value).ToList();
        }

        private void AddItemImpl(Item item)
        {
            if (items.ContainsKey(item.Id))
            {
                throw new Exception($"Attempting to add id that is already present: {item.Id}");
            }

            items.Add(item.Id, item);
            item.SwitchChanged += Item_SwitchChanged;
            ObservableItems.Add(item);
        }

        private void OverrideItem(Item toOverride, Item with)
        {
            toOverride.SwitchChanged -= Item_SwitchChanged;
            items[toOverride.Id] = with;
            with.SwitchChanged += Item_SwitchChanged;
        }

        private async void Item_SwitchChanged(Item obj)
        {
            SortObservable();
            var items = await Fetcher.Update(ItemSnapshot());

            HandleSnapshot(items.update.present);
        }
    }
}
