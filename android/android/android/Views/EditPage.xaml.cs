using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using android.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace android.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        public Item Item;
        public EditPage(Item beingEdit)
        {
            Item = beingEdit;
            BindingContext = this;

            InitializeComponent();

            NameEdit.Text = Item.Text;

        }
    }
}