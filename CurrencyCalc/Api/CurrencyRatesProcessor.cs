using CurrencyCalc.Api;
using CurrencyCalc.Models;
using CurrencyCalc.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Api
{
    public class CurrencyRatesProcessor : ICurrencyRatesProcessor
    {
        #region Fields
        private IHttpClientService _httpClientService;
        private IInternetChecker _internetChecker;
        private string _lastCorrectResponseDate = DateTime.Now.ToString("yyyy-MM-dd");
        private List<IRateModel> _rates = new List<IRateModel>();
        #endregion

        #region Constructor
        public CurrencyRatesProcessor(IHttpClientService httpClientService, IInternetChecker internetChecker)
        {
            _httpClientService = httpClientService;
            _internetChecker = internetChecker;
        }
        #endregion

        #region Properties
        public string LastCorrectResponseDate
        {
            get { return _lastCorrectResponseDate; }
            set { _lastCorrectResponseDate = value; }
        }
        #endregion

        #region Methods

        // Attempts to load currency rates for a selected date. If rates for the selected date are not available,
        // it tries for previous dates up to 10 days back.
        public async Task<List<IRateModel>> LoadRates(DateTime? selectedDate = null)
        {
            if (_internetChecker.IsNetworkAvailable())
                if (await _internetChecker.CanConnectToInternet())
                {
                    {
                        HttpResponseMessage response;
                        string url;

                        // If date is selected, URL for the API request based on the provided date is built.
                        // If date is not selected actual date is used to generate URL.
                        url = BuildUrlForDate(selectedDate ?? DateTime.Now);

                        response = await SendRequest(url);

                        if (response.IsSuccessStatusCode)
                        {
                            return await ProcessResponse(response, selectedDate);
                        }
                        else
                        {
                            // If the initial attempt fails, try fetching rates for the previous days.
                            for (int i = 1; (!response.IsSuccessStatusCode && i < 10); i++)
                            {
                                DateTime? availableDate = selectedDate - TimeSpan.FromDays(i);
                                url = BuildUrlForDate(availableDate);
                                response = await SendRequest(url);

                                if (response.IsSuccessStatusCode)
                                {
                                    return await ProcessResponse(response, availableDate);
                                }
                            }
                            throw new HttpRequestException($"Failed to fetch rates: {response.ReasonPhrase}");
                        }
                    } 
                }
            return _rates;
        }


        // Builds the URL for the API request based on the provided date e.g.http://api.nbp.pl/api/exchangerates/tables/A/2023-10-20/?format=json
        private string BuildUrlForDate(DateTime? date)
        {
            return $"exchangerates/tables/A/{date?.ToString("yyyy-MM-dd")}/?format=json";
        }


        // Sends an HTTP GET request to the provided URL and returns the response.
        private async Task<HttpResponseMessage> SendRequest(string url)
        {
            try
            {
                return await _httpClientService.Client.GetAsync(url);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Problem with internet connection occured.", ex);
            }

        }


        // Processes the HTTP response, deserializes the JSON content, and updates the rates.
        private async Task<List<IRateModel>> ProcessResponse(HttpResponseMessage response, DateTime? date)
        {

            try
            {
                string jsonString = "";
                jsonString = await response.Content.ReadAsStringAsync();
                var resultList = JsonConvert.DeserializeObject<List<CurrencyRatesModel>>(jsonString);

                if (resultList.Any())
                {
                    _rates = resultList[0].Rates.Select(rate => (IRateModel)rate).ToList();
                    if (date.HasValue)
                    {
                        LastCorrectResponseDate = date.Value.ToString("dd.MM.yyyy");
                    }
                }
                return _rates;
            }
            catch (Exception)
            {

                return new List<IRateModel>();
            }


        }
        #endregion
    }
}