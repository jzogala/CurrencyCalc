using System;
using System.Net.Http;

namespace CurrencyCalc.Api
{
    public class HttpClientService : IHttpClientService, IDisposable
    {
        public HttpClient Client { get; }

        public HttpClientService(HttpClientFactory httpClientFactory)
        {
            Client = httpClientFactory.InitializeClient();
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
