using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Models
{
    public class CurrencyRatesModel : ICurrencyRatesModel
    {
        public string Table { get; set; }
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public List<RateModel> Rates { get; set; }
    }
}
