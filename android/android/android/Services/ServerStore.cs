using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace android.Services
{
    class ServerStore
    {
        public static string itemsQuery = @"{items{id, name,description,category,amount, unit, bought, lastUpdate}}";

        struct Result<T>
        {

        }

        public static async Task<T> QueryAsync<T>(string query)
        {
            var req = new GraphQLRequest();
            req.Query = query;
            try
            {
                var graphQLClient = new GraphQLClient("http://192.168.0.248:4000/graphql");
                var post = await graphQLClient.PostAsync(req);

                Console.WriteLine("dostalem: " + post.Data.ToString());
                var deserialized = JsonConvert.DeserializeObject<T>(post.Data.ToString());
                return deserialized;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GraphQl request failed: " + query + " msg: " + ex.Message);

                return default(T);
            }
        }
    }
}
