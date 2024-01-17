using System;
using System.Net.Http;

namespace CurrencyCalc.Api
{
    public class HttpClientService : IHttpClientService, IDisposable
    {
        public HttpClient Client { get; }

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            Client = httpClientFactory.CreateClient("ApiHttpClient");
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
