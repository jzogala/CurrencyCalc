using CurrencyCalc.Models;
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

        public static async Task<CurrencyRatesModel> LoadRates(DateTime? selectedDate = null)
        {
            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today.Date;
            }

            string url = $"exchangerates/tables/A/{selectedDate}/?format=json";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    CurrencyRatesModel rates = await response.Content.ReadAsAsync<CurrencyRatesModel>();
                    //modeluje wszystkie dostępne właściwości na dostępne właściowości w ComicModel olewając pozostałe

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
//http://api.nbp.pl/api/exchangerates/tables/A/{date}/?format=json
// Zaytanie wykonane 2023.10.23
// http://api.nbp.pl/api/exchangerates/rates/A/USD/?format=json
// {"table":"A","currency":"dolar amerykański","code":"USD","rates":[{"no":"205/A/NBP/2023","effectiveDate":"2023-10-23","mid":4.2022}]}
// https://api.nbp.pl/api/exchangerates/rates/A/USD/today/?format=json
// {"table":"A","currency":"dolar amerykański","code":"USD","rates":[{"no":"205/A/NBP/2023","effectiveDate":"2023-10-23","mid":4.2022}]}
// https://api.nbp.pl/api/exchangerates/rates/A/USD/2023-10-20/?format=json
// { "table":"A","currency":"dolar amerykański","code":"USD","rates":[{ "no":"204/A/NBP/2023","effectiveDate":"2023-10-20","mid":4.2194}]}
// dla zapytań w weekend i prawdopodobnie też w święta nie dostaniemy wyniku