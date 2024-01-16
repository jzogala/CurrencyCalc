using CurrencyCalc.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyCalc.Api
{
    public interface ICurrencyRatesProcessor
    {
        string LastCorrectResponseDate { get; set; }

        Task<List<IRateModel>> LoadRates(DateTime? selectedDate = null);
    }
}