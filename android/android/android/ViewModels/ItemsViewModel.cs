using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using android.Models;
using android.Views;

using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json;
using android.Services;

namespace android.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        ObservableCollection<Item> items;
        public ObservableCollection<Item> Items
        {
            get
            {
                return itemStore.ObservableItems;
            }
        }

        private ItemStore itemStore = new ItemStore();

        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                item.SwitchChanged += Item_SwitchChanged;
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
                var remote_items = await Fetcher.ItemsAsync();
                foreach (var item in items)
                {
                    //item.SwitchChanged += Item_SwitchChanged;
                    //Items.Add(item);
                }

                //foreach (var remote_item in remote_items.items)
                //{
                    //remote_item.SwitchChanged += Item_SwitchChanged;
                    //Items.Add(remote_item);
                //}
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
            itemStore.SortObservable();
        }

        public async void FetchItemsAsync()
        {
            itemStore.HandleSnapshot(await Fetcher.ItemsAsync());
        }
    }
}