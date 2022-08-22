using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace OneListClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            var responseBodyAsStream = await client.GetStreamAsync("https://one-list-api.herokuapp.com/items?access_token=cohort24");
            // describes the <i> shape</i> of the data
            // (array in JSON => List, Object in JSON => Item)
            //                                    v             v   v
            var items = await JsonSerializer.DeserializeAsync<List<item>>(responseBodyAsStream);

            foreach (var item in items)
            {
                Console.WriteLine($"the task {item.Text} was created on {item.CreatedAt} and has a completion of {item.CompletedStatus}");
                //          /\ coded custom property
            }
        }
    }
}
