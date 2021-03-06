﻿using System;
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
            var item = args.SelectedItem as Item;
            if (item == null)
                return;
            
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(viewModel.itemStore)));
        }

        async void Refresh_Clicked(object sender, EventArgs e)
        {
            await viewModel.FetchItemsAsync();
        }

        async void RemoveSelected_Clicked(object sender, EventArgs e)
        {
            await viewModel.RemoveSelectedAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                await viewModel.FetchItemsAsync();
        }
    }
}