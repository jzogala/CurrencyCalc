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

        public static async Task<List<RateModel>> LoadRates(DateTime? selectedDate = null)
        {
            string url = "";
            string todayDate = "";
            string jsonResponse = "";
            List<RateModel> rates = new List<RateModel>();

            if (!selectedDate.HasValue)
            {
                url = $"exchangerates/tables/A/?format=json";
            }

            //todayDate = DateTime.Now.ToString("yyyy-MM-dd"); - do wykorzystania jak bedzie wybieranie daty
            //string url = $"exchangerates/tables/A/{todayDate}/?format=json";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    jsonResponse = await response.Content.ReadAsStringAsync();
                    var resultList = JsonConvert.DeserializeObject<List<CurrencyRatesModel>>(jsonResponse);

                    if (resultList.Any())
                    {
                        rates = resultList[0].Rates;
                    }
                    return rates;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
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