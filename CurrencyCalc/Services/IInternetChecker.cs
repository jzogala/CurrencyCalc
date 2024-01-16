using System.Threading.Tasks;

namespace CurrencyCalc.Services
{
    public interface IInternetChecker
    {
        Task<bool> CanConnectToInternet();
        bool IsNetworkAvailable();
    }
}