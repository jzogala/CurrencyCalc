using CurrencyCalc.Api;
using CurrencyCalc.Models;
using CurrencyCalc.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyCalc
{
    public partial class MainWindow : Window
    {
        //private readonly IHttpClientService _httpClientService;
        public MainWindow(CurrencyCalcViewModel currencyCalcViewModel)
        {
            DataContext = currencyCalcViewModel;
            InitializeComponent();
            this.Closing += OnWindowClosing;
        }

        // additonal operations while closing MainWindow
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is CurrencyCalcViewModel viewModel)
            {
                viewModel.OnMainWindowClosing();
            }
        }
    }
}
