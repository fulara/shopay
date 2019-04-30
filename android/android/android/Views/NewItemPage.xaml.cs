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
        public NewItemPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            BindingContext = this;
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
                AddSearchHint(definition.Name);
            }
        }


        private void AddSearchHint(string hint)
        {
            var row = SearchResultContainerEntry.Children.Count / 3;
            SearchResultContainerEntry.Children.Add(new Label
            {
                Text = hint,
            }, 0, row);

            SearchResultContainerEntry.Children.Add(new Label
            {
                Text = "+",
            }, 1, row);

            SearchResultContainerEntry.Children.Add(new Label
            {
                Text = "++",
            }, 2, row);
        }
    }
}