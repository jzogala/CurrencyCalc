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
        #region Fields
        private CurrencyCalcViewModel viewModel;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new CurrencyCalcViewModel();
            this.DataContext = viewModel;
        }
        #endregion

        #region Methods
        private async void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            List<RateModel> seletcedDateRates = new List<RateModel>();
            // Checking if sender is DatePicker type
            if (sender is DatePicker datePicker)
            {
                //Caching picked new date
                DateTime? selectedDate = datePicker.SelectedDate;

                if (selectedDate.HasValue)
                {
                    seletcedDateRates = await CurrencyRatesProcessor.LoadRates(selectedDate);
                    viewModel.Rates = new ObservableCollection<RateModel> (seletcedDateRates); 
                }
                
            }

            //txtBaseCurrencyAmount.Text = $" {seletcedDateRates} ";

        }
        #endregion
    }
}
