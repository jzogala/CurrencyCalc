using CurrencyCalc.Models;
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
        private static string url = "";
        private static string jsonString = "";
        private static List<RateModel> rates = new List<RateModel>();
        private static HttpResponseMessage response = new HttpResponseMessage();
        #endregion

        #region Properties
        public static string Url
        {
            get { return url; }
            set { url = value; }
        }
        #endregion

        #region Methods
        public static async Task<List<RateModel>> LoadRates(DateTime? selectedDate = null)
        {
            if (!selectedDate.HasValue || selectedDate >= DateTime.Now.Date)
            {
                Url = "exchangerates/tables/A/?format=json";
            }
            else
            {
                Url = $"exchangerates/tables/A/{selectedDate.Value.ToString("yyyy-MM-dd")}/?format=json";
            }

            using (response = await ApiHelper.ApiClient.GetAsync(Url))
            {
                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStringAsync();
                    var resultList = JsonConvert.DeserializeObject<List<CurrencyRatesModel>>(jsonString);

                    if (resultList.Any())
                    {
                        rates = resultList[0].Rates;
                    }
                    return rates;
                }
                else
                {
                    for (int i = 1; (!response.IsSuccessStatusCode && i<10); i++)
                    {
                        DateTime? availableDate = selectedDate - TimeSpan.FromDays(i);

                        url = $"exchangerates/tables/A/{availableDate.Value.ToString("yyyy-MM-dd")}/?format=json";

                        using (response = await ApiHelper.ApiClient.GetAsync(url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                jsonString = await response.Content.ReadAsStringAsync();
                                var resultList = JsonConvert.DeserializeObject<List<CurrencyRatesModel>>(jsonString);

                                if (resultList.Any())
                                {
                                    rates = resultList[0].Rates;
                                }
                               
                                return rates;
                            }
                        }
                    }
                    throw new Exception(response.ReasonPhrase);
                }
            }
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