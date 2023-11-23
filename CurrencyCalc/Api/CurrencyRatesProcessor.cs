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
    public class CurrencyRatesProcessor
    {
        #region Fields
        private static CurrencyRatesProcessor _instance;
        private string _lastCorrectResponseDate = DateTime.Now.ToString("yyyy-MM-dd");
        private List<RateModel> _rates = new List<RateModel>();
        #endregion

        #region Constructor
        private CurrencyRatesProcessor() 
        { 

        }
        #endregion

        #region Properties

        // Singleton instance of CurrencyRatesProcessor. Ensures that only one instance of the processor is created and used throughout the application.
        public static CurrencyRatesProcessor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CurrencyRatesProcessor();
                }
                return _instance;
            }
        }

        public string LastCorrectResponseDate
        {
            get { return _lastCorrectResponseDate; }
            set { _lastCorrectResponseDate = value; }
        }
        #endregion

        #region Methods

        // Attempts to load currency rates for a selected date. If rates for the selected date are not available,
        // it tries for previous dates up to 10 days back.
        public async Task<List<RateModel>> LoadRates(DateTime? selectedDate = null)
        {
            if (InternetChecker.IsNetworkAvailable() && InternetChecker.CanConnectToInternet())
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
                return await ApiHelper.ApiClient.GetAsync(url);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Problem with internet connection occured.", ex);
            }

        }


        // Processes the HTTP response, deserializes the JSON content, and updates the rates.
        private async Task<List<RateModel>> ProcessResponse(HttpResponseMessage response, DateTime? date)
        {
            string jsonString = "";
            jsonString = await response.Content.ReadAsStringAsync();
            var resultList = JsonConvert.DeserializeObject<List<CurrencyRatesModel>>(jsonString);

            if (resultList.Any())
            {
                _rates = resultList[0].Rates;
                if (date.HasValue)
                {
                    LastCorrectResponseDate = date.Value.ToString("dd.MM.yyyy");
                }
            }
            return _rates;
        }
        #endregion
    }
}