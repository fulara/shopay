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
                itemStore.HandleSnapshot((await Fetcher.ItemsAsync()).items);
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



        public async Task FetchItemsAsync()
        {
            var items = await Fetcher.ItemsAsync();
            itemStore.HandleSnapshot(items.items);
        }
    }
}