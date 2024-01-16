using System.Collections.Generic;

namespace CurrencyCalc.Models
{
    public interface ICurrencyRatesModel
    {
        string EffectiveDate { get; set; }
        string No { get; set; }
        List<RateModel> Rates { get; set; }
        string Table { get; set; }
    }
}