using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using android.Models;
using android.Views;

namespace android.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        ObservableCollection<Item> items;
        public ObservableCollection<Item> Items { get {
                return items;
            } set {
                items = value;
            }
        }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                item.SwitchChanged += Item_SwitchChanged;
                Console.WriteLine("adding???");
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

    async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Console.WriteLine("adding??");
                    item.SwitchChanged += Item_SwitchChanged;
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Item_SwitchChanged(Item obj)
        {
            var sorted = items.OrderBy(x => x).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                var curr = items.IndexOf(sorted[i]);
                items.Move(curr, i);
            }
        }
    }
}