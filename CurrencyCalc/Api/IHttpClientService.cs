using System.Net.Http;

namespace CurrencyCalc.Api
{
    public interface IHttpClientService
    {
        HttpClient Client { get; }
        void Dispose();
    }
}