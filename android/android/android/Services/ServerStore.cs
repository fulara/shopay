using android.Models;
using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace android.Services
{
    class ServerStore
    {
        public struct Items
        {
            public List<Item> items;
        }

        public struct UpdateData
        {
            public struct Data
            {
                public int lastServerUpdate;
                public List<Item> items;
            }

            public Data data;
        }

        public static async Task<Items> ItemsQuery()
        {
            var query = @"{items{id, name,description,category,amount, unit, bought, lastUpdateTimestamp}}";

            return await QueryAsync<Items>(query);
        }

        public struct UpdateQueryResult
        {
            public struct Data
            {
                public List<Item> present;
                public int serverTime;
            }

            public Data update;
        }

        public static async Task<UpdateQueryResult> UpdateQuery(int lastServerUpdate, List<Item> snapshot)
        {
            var data = new UpdateData
            {
                data = new UpdateData.Data
                {
                    lastServerUpdate = 0,
                    items = snapshot,
                }
            };

            var q = new StringBuilder(JsonConvert.SerializeObject(data));
            //swap opening and closing {} with () just because too lazy to search how to this 'properly'.
            q[0] = '(';
            q[q.Length - 1] = ')';

            var input = Regex.Replace(q.ToString(), @"""([a-zA-z]+)"":", "$1:");
            var output = @"{present { id, name,description,category,amount, unit, bought, lastUpdateTimestamp, }serverTime}";

            var query =  $"mutation{{update{input}{output}}}";

            return await QueryAsync<UpdateQueryResult>(query);
        }

        ////    mutation {
        //        update(data :{ lastServerUpdate : 1, items:  [ { id:"4", name:"potato", description:"supa", category:"cat", amount: "1.0", unit: "m",  bought:false, updatedAt:2  } ]   }) {
        //            present {
        //              id,
        //              name,
        //              category,
        //              bought,
        //              lastUpdate,
        //            }
        //           serverTime
        //          }
        //        }

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
               
//                JsonConvert.Des
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
