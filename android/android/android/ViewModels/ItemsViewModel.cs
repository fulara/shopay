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
            //ServerCommunicator.Instance.Push();
            //var c = new Command(async () =>
            //{
            //    var req = new GraphQLRequest();
            //    req.Query = @"mutation{ update(data: { added :[ { name:""p"", category:""c"", bought:false }], removed:[], changed:[], }) { name}}";

            //    try
            //    {
            //        var graphQLClient = new GraphQLClient("http://192.168.0.248:4000/graphql");
            //        var post = await graphQLClient.PostAsync(req);
            //        Console.WriteLine("posting that request");
            //        Console.WriteLine("request finished: " + post.Data.ToString());
            //    } catch(Exception e)
            //    {
            //        Console.WriteLine("wtf?" + e.Message + " " + e.Data);
            //    }
            //    //var s = post.Result.ToString();
            //    //Console.WriteLine("posting that request" + s); 
            //});

            //c.Execute(null);

            var sorted = items.OrderBy(x => x).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                var curr = items.IndexOf(sorted[i]);

                if (curr != i)
                {
                    items.Move(curr, i);
                }
            }
        }
    }
}