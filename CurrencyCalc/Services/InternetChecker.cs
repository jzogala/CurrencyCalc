using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyCalc.Services
{
    public static class InternetChecker
    {
        // Sprawdza dostępność sieci na podstawie statusu interfejsów sieciowych
        public static bool IsNetworkAvailable()
        {
            try
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
            catch
            {
                // W przypadku błędu, zakładamy brak dostępu do internetu
                return false;
            }
        }

        // Sprawdza rzeczywiste połączenie z Internetem
        public static bool CanConnectToInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                // W przypadku błędu, zakładamy brak dostępu do internetu
                return false;
            }
        }
    }
}
