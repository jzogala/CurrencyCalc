﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace CurrencyCalc.Api
{

    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }
        public static void InitializeClient()
        {
            ApiClient = new HttpClient();

            ApiClient.BaseAddress = new Uri("https://api.nbp.pl/");

            ApiClient.DefaultRequestHeaders.Accept.Clear();

            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}


//test