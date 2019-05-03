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

        public ItemStore itemStore = new ItemStore();

        public ItemsViewModel()
        {
            Title = "Browse";

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                itemStore.AddItem(item);
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

        public async Task RemoveSelectedAsync()
        {
            itemStore.RemoveSelected();
            await Fetcher.Update(itemStore.ItemSnapshot());
        }
    }
}