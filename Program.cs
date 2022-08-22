using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleTables;

namespace OneListClient
{
    class Program
    {
        //          *void* usually goes here
        //           v
        static async Task Main(string[] args)
        {
            var token = args[0];

            var client = new HttpClient();

            var responseBodyAsStream = await client.GetStreamAsync("https://one-list-api.herokuapp.com/items?access_token={token}"); /*-- when "dotnet run" qualify the token with "cohort24" --*/
            // describes the <i> shape</i> of the data
            // (array in JSON => List, Object in JSON => Item)
            //                                    v             v   v
            var items = await JsonSerializer.DeserializeAsync<List<item>>(responseBodyAsStream);
            //  table headers **********************************************************
            var table = new ConsoleTable("Description", "Created At", "Completed");
            //  table headers **********************************************************
            // C# normalcy 
            foreach (var item in items)
            {
                // except...
                // table content **********************************************************
                table.AddRow(item.Text, item.CreatedAt, item.CompletedStatus);
                //  table content **********************************************************

                // instead of the "Console.Writeline" below...
                // Console.WriteLine($"the task {item.Text} was created on {item.CreatedAt} and has a completion of {item.CompletedStatus}");
                //          /\ coded custom property
                table.Write();
            }
        }
    }
}
