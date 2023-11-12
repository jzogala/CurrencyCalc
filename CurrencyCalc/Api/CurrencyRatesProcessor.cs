using CurrencyCalc.Models;
using CurrencyCalc.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Api
{
    public class CurrencyRatesProcessor
    {
        #region Fields
        private static CurrencyRatesProcessor _instance;
        private string _url = "";
        private string _jsonString = "";
        private string _lastCorrectResponseDate = DateTime.Now.ToString("yyyy-MM-dd");
        private List<RateModel> _rates = new List<RateModel>();
        private HttpResponseMessage _response = new HttpResponseMessage();
        #endregion

        #region Constructor
        private CurrencyRatesProcessor() 
        { 

        }
        #endregion

        #region Properties
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

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string LastCorrectResponseDate
        {
            get { return _lastCorrectResponseDate; }
            set { _lastCorrectResponseDate = value; }
        }
        #endregion

        #region Methods
        public async Task<List<RateModel>> LoadRates(DateTime? selectedDate = null)
        {
            if (InternetChecker.IsNetworkAvailable() == true && InternetChecker.CanConnectToInternet() == true)
            {
                if (selectedDate.HasValue)
                {
                    Url = $"exchangerates/tables/A/{selectedDate.Value.ToString("yyyy-MM-dd")}/?format=json";
                }

                using (_response = await ApiHelper.ApiClient.GetAsync(Url))
                {
                    if (_response.IsSuccessStatusCode)
                    {
                        _jsonString = await _response.Content.ReadAsStringAsync();
                        var resultList = JsonConvert.DeserializeObject<List<CurrencyRatesModel>>(_jsonString);

                        if (resultList.Any())
                        {
                            _rates = resultList[0].Rates;
                        }
                        LastCorrectResponseDate = selectedDate.Value.ToString("dd.MM.yyyy");
                        return _rates;
                    }
                    else
                    {
                        for (int i = 1; (!_response.IsSuccessStatusCode && i < 10); i++)
                        {
                            DateTime? availableDate = selectedDate - TimeSpan.FromDays(i);

                            _url = $"exchangerates/tables/A/{availableDate.Value.ToString("yyyy-MM-dd")}/?format=json";

                            using (_response = await ApiHelper.ApiClient.GetAsync(_url))
                            {
                                if (_response.IsSuccessStatusCode)
                                {
                                    _jsonString = await _response.Content.ReadAsStringAsync();
                                    var resultList = JsonConvert.DeserializeObject<List<CurrencyRatesModel>>(_jsonString);

                                    if (resultList.Any())
                                    {
                                        _rates = resultList[0].Rates;
                                    }
                                    LastCorrectResponseDate = availableDate.Value.ToString("dd.MM.yyyy");
                                    return _rates;
                                }
                            }
                        }
                        throw new Exception(_response.ReasonPhrase);
                    }
                }
            }
            return _rates;
        }
        #endregion
    }
}
//http://api.nbp.pl/api/exchangerates/tables/{table}/
// Aktualnie obowiązująca tabela kursów typu {table} - nie zwraca braku danych
//http://api.nbp.pl/api/exchangerates/tables/A/{date}/?format=json
// Zaytanie wykonane 2023.10.23 testa
// http://api.nbp.pl/api/exchangerates/rates/A/USD/?format=json
// {"table":"A","currency":"dolar amerykański","code":"USD","rates":[{"no":"205/A/NBP/2023","effectiveDate":"2023-10-23","mid":4.2022}]}
// https://api.nbp.pl/api/exchangerates/rates/A/USD/today/?format=json
// {"table":"A","currency":"dolar amerykański","code":"USD","rates":[{"no":"205/A/NBP/2023","effectiveDate":"2023-10-23","mid":4.2022}]}
// https://api.nbp.pl/api/exchangerates/rates/A/USD/2023-10-20/?format=json
// { "table":"A","currency":"dolar amerykański","code":"USD","rates":[{ "no":"204/A/NBP/2023","effectiveDate":"2023-10-20","mid":4.2194}]}
// dla zapytań w weekend i prawdopodobnie też w święta nie dostaniemy wyniku