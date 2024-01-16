using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Api
{
    // HttpClientFactory is a class for creating and configuring the single instance of HttpClient used across the application.
    public class HttpClientFactory
    {
        // Initializes the HttpClient with base address and default headers for JSON content type.
        public HttpClient InitializeClient()
        {
            HttpClient httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(30),
                BaseAddress = new Uri("https://api.nbp.pl/api/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}
