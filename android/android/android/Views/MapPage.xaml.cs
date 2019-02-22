using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace android.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            MapImage.Source = MapDisplay();
            Container.Children.Add(create());
        }

        ImageSource MapDisplay()
        {
            //make this async someday?
            return ImageSource.FromUri(new Uri("https://www.thegamecrafter.com/overlays/squareboard.png"));
        }

        private Label create()
        {
            var x = new Label
            {
                Text = "hello World",
            };

            AbsoluteLayout.SetLayoutBounds(x, new Rectangle(.5, .2, .5, .1));
            AbsoluteLayout.SetLayoutFlags(x, AbsoluteLayoutFlags.All);

            return x;
        }
    }
}