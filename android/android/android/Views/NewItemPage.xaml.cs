using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using android.Models;

namespace android.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            Item = new Item
            {
                Id = System.Guid.NewGuid().ToString(),
                Text = "",
                Description = ""
            };

            BindingContext = this;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(UiTextEdit.Text))
            {
                await DisplayAlert("Cant proceed", "Name is required", "Ok");
                return;
            }

            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }
    }
}