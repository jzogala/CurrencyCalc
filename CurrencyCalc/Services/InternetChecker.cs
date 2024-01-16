using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using CurrencyCalc.Api;

namespace CurrencyCalc.Services
{
    public class InternetChecker : IInternetChecker
    {
        private readonly IHttpClientService _httpClient;

        public InternetChecker(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        // Checks network availability based on the status of network interfaces
        public bool IsNetworkAvailable()
        {
            try
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
            catch
            {
                return false;
            }
        }

        // Checks for an actual internet connection with gooogle.com website
        public async Task<bool> CanConnectToInternet()
        {
            try
            {
                var response = await _httpClient.Client.GetAsync("http://www.google.com");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
