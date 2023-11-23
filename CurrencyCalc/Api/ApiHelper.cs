using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace CurrencyCalc.Api
{
    // ApiHelper is a utility class for managing the single instance of HttpClient used across the application.
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        // Initializes the HttpClient with base address and default headers for JSON content type.
        public static void InitializeClient()
        {
            ApiClient = new HttpClient();

            ApiClient.Timeout = TimeSpan.FromSeconds(30);

            ApiClient.BaseAddress = new Uri("https://api.nbp.pl/api/");

            ApiClient.DefaultRequestHeaders.Accept.Clear();

            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // DisposeClient ensures the HttpClient is properly disposed of, freeing up network resources.
        public static void DisposeClient()
        {
            if (ApiClient != null)
            {
                ApiClient.Dispose();
                ApiClient = null;
            }
        }
    }
}
