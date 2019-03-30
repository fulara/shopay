using android.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace android.Services
{
    class Fetcher
    {
        public static async Task<Items> ItemsAsync()
        {
            return await ServerStore.QueryAsync<Items>(ServerStore.itemsQuery);
        }
    }
}
