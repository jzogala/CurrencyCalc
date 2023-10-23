using CurrencyCalc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Api
{
    public class NBPApiProcessor
    {

        public static async Task<NBPRatesModel> LoadRates(DateTime? selectedDate = null)
        {
            if (!selectedDate.HasValue)
            {
                selectedDate = DateTime.Today.Date;
            }

            string url = "";
            //if (selectedDate!= DateTime.Today)
            //{
            //    url = $"https://xkcd.com/{selectedDate}/info.0.json";
            //}
            //else
            //{
            //    url = "https://xkcd.com/info.0.json";
            //}

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    NBPRatesModel comic = await response.Content.ReadAsAsync<NBPRatesModel>();
                    //modeluje wszystkie dostępne właściwości na dostępne właściowości w ComicModel olewając pozostałe

                    return comic;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
