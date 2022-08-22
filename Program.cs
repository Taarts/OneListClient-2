using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleTables;

namespace OneListClient
{
    class Program
    {
        static async Task AddOneItem(string token, Item newItem)
        {
            var client = new HttpClient();

            // Generate a URL specifically referencing the endpoint for adding a todo item
            var url = $"https://one-list-api.herokuapp.com/items?access_token={token}";

            // Take the `newItem` and serialize it into JSON (Object to a string)
            var jsonBody = JsonSerializer.Serialize(newItem);

            // We turn this into a StringContent object and indicate we are using JSON
            // by ensuring there is a media type header of `application/json`
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send the POST request to the URL and supply the JSON body
            var response = await client.PostAsync(url, jsonBodyAsContent);

            // Get the response as a stream.
            var responseJson = await response.Content.ReadAsStreamAsync();

            // Supply that *stream of data* to a Deserialize that will interpret it as a *SINGLE* `Item`
            var item = await JsonSerializer.DeserializeAsync<Item>(responseJson);

            // Make a table to output our new item.
            var table = new ConsoleTable("ID", "Description", "Created At", "Updated At", "Completed");

            // Add one row to our table
            table.AddRow(item.Id, item.Text, item.CreatedAt, item.UpdatedAt, item.CompletedStatus);

            // Write the table
            table.Write(Format.Minimal);
        }
        // created after "ShowAllItems" method
        static async Task GetOneItem(string token, int id)
        {
            try
            {
                var client = new HttpClient();

                var responseBodyAsStream = await client.GetStringAsync($"https://one-list-api.herokuapp.com/api/v1/items/{id}?access_token={token}");

                var item = await JsonSerializer.DeserializeAsync<Item>(responseBodyAsStream);
                var table = new ConsoleTable("Description", "Created At", "Completed");
                table.AddRow(item.Text, item.CreatedAt, item.CompletedStatus);
                table.Write();
            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"I couldn't find that item.");

            }
        }
        static async Task ShowAllItems(string token)
        {
            var client = new HttpClient();
            // var url = $"https://one-list-api.herokuapp.com/items?access_token={token}";

            var responseBodyAsStream = await client.GetStreamAsync("https://one-list-api.herokuapp.com/items?access_token={token}"); /*-- when "dotnet run" qualify the token with "cohort24" --*/

            // var responseBodyAsStream = await client.GetStreamAsync(url);

            // describes the <i> shape</i> of the data
            // (array in JSON => List, Object in JSON => Item)
            //                                    v             v   v
            var items = await JsonSerializer.DeserializeAsync<List<Item>>(responseBodyAsStream);
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
        //          *void* usually goes here
        //           v
        static async Task Main(string[] args)
        {
            var token = "";
            if (args.Length == 0)
            {
                Console.WriteLine("What list would you like? ");
                token = Console.ReadLine();
            }
            else
            {
                token = args[0];
            }

            var keepGoing = true;
            while (keepGoing)
            {
                Console.Clear();
                Console.Write("Get (A)ll todo, or Get (O)ne Item or (C)reate a new ToDo (Q)uit: ");
                var choice = Console.ReadLine().ToUpper();
                switch (choice)
                {
                    case "Q":
                        keepGoing = false;
                        break;
                    case "O":
                        Console.Write("Enter the ID of the item to show: ");
                        var id = int.Parse(Console.ReadLine());
                        await GetOneItem(token, id);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;

                    case "C":
                        Console.Write("Enter the description of your new todo: ");
                        var text = Console.ReadLine();
                        var newItem = new Item
                        {
                            Text = text
                        };
                        await AddOneItem(token, newItem);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    case "A":
                        // have to await method when using async
                        await ShowAllItems(token);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}