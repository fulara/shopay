using android.Models;
using android.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static android.Services.ServerStore;

namespace android.Services
{
    class Fetcher
    {
        public static async Task<UpdateQueryResult> Update(List<Item> snapshot)
        {
            return await ServerStore.UpdateQuery(0, snapshot);
        }

        public static async Task<Items> ItemsAsync()
        {
            return await ServerStore.ItemsQuery();
        }
    }
}
