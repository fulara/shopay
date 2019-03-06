using System;

using android.Models;

namespace android.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            DataStore.ToString();
            Title = item?.Text;
            Item = item;
        }
    }
}
