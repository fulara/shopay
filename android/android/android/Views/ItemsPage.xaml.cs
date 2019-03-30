using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using android.Models;
using android.Views;
using android.ViewModels;
using GraphQL.Common.Request;
using GraphQL.Client;
using Newtonsoft.Json;
using android.Services;

namespace android.Views
{
    struct Items
    {
        public List<Item> items;
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ziemniak");
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.WriteAllText(path, "puste");
            }

            var text = System.IO.File.ReadAllText(path);
            System.IO.File.WriteAllText(path, text + "yesy");
            
            Console.WriteLine("zaladowalem:" + text);
          

            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            viewModel.Items.Remove(item);

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async void TestItem_Clicked(object sender, EventArgs e)
        {
            viewModel.FetchItemsAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void ItemSwitched(object sender, ToggledEventArgs e)
        {
        }
    }
}