namespace CurrencyCalc.Models
{
    public interface IRateModel
    {
        string Code { get; set; }
        string Currency { get; set; }
        decimal Mid { get; set; }
    }
}