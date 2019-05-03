using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using android.Models;
using android.Services;

namespace android.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        private ItemStore store;
        public NewItemPage(ItemStore store)
        {
            this.store = store;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            BindingContext = this;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(UiNameEdit.Text))
            {
                await DisplayAlert("Cant proceed", "Name is required", "Ok");
                return;
            }

            var item = new Item
            {
                Id = System.Guid.NewGuid().ToString(),
                Text = UiNameEdit.Text,
                Description = ""
            };

            MessagingCenter.Send(this, "AddItem", item);
            await Navigation.PopModalAsync();
        }

        private void UiNameEdit_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchResultContainerEntry.Children.Clear();

            var text = UiNameEdit.Text;

            foreach (var definition in WordLookup.Get(text).Take(10))
            {
                AddSearchHint(definition.Name, definition);
            }
        }


        private void AddSearchHint(string hint, WordLookup.Definition definition)
        {
            var row = SearchResultContainerEntry.Children.Count / 3;
            var label = new Label
            {
                Text = hint,
            };

            SearchResultContainerEntry.Children.Add(label, 0, row);

            var addb = new Button()
            {
                Text = "+",
            };

            addb.HeightRequest = 20;
            addb.Clicked += (sender, args) => { AddClicked(label, hint, definition); };
            SearchResultContainerEntry.Children.Add(addb, 1, row);

            var addEditB = new Button()
            {
                Text = "++",
            };

            addEditB.HeightRequest = 20;

            addEditB.Clicked += (sender, args) => { AddEditClicked(label, hint, definition); };

            SearchResultContainerEntry.Children.Add(addEditB, 2, row);
        }

        private void AddClicked(Label label, string hint, WordLookup.Definition definition)
        {
            label.BackgroundColor = Color.GreenYellow;

            var item = new Item();
            item.Category = definition.Category;
            item.Text = hint;
            store.AddItem(item);
        }

        private void AddEditClicked(Label label, string hint, WordLookup.Definition definition)
        {
            label.BackgroundColor = Color.GreenYellow;

            var item = new Item();
            item.Category = definition.Category;
            item.Text = hint;
            store.AddItem(item);

            Navigation.PushModalAsync(new EditPage(item));
        }
    }
}