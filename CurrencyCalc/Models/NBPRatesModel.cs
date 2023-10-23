using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Models
{
    public class NBPRatesModel
    {
        public string CurrencySymbol { get; set; } = "";
        public decimal CurrencyRatio { get; set; }
    }
}
