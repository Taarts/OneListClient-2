﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneListClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            var responseBodyAsString = await client.GetStringAsync("https://one-list-api.herokuapp.com/itmes?access_token={token});

            Console.WriteLine(responseBodyAsString);
        }
    }
}
