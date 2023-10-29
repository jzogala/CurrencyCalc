using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Models
{
    public class RateModel
    {
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Mid { get; set; }
    }
}
