using CurrencyCalc.Api;
using CurrencyCalc.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CurrencyCalc.ViewModels
{
    public class CurrencyCalcViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<RateModel> _rates = new ObservableCollection<RateModel>();
        private RateModel _selectedCurrency = new RateModel();
        private string _datePickerComments = "Proszę wybrać datę";
        private DateTime? _selectedDate;
        private string _lastLoadRatesDate = DateTime.Now.ToString("yyyy-MM-dd");

        public CurrencyCalcViewModel()
        {
            //Rates = new ObservableCollection<RateModel>();
        }

        #region Properties
        public ObservableCollection<RateModel> Rates
        {
            get { return _rates; }
            set
            {
                if (value != _rates)
                {
                    _rates = value;
                    OnPropertyChanged(nameof(Rates));
                }
            }
        }

        public RateModel SelectedCurrency
        {
            get { return _selectedCurrency; }
            set 
            { 
                if (value != _selectedCurrency)
                {
                    _selectedCurrency = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DatePickerComments
        {
            get { return _datePickerComments; }
            set 
            {
                if (_datePickerComments != value)
                {
                    _datePickerComments = value;
                    OnPropertyChanged(nameof(DatePickerComments));
                }
            }
        }

        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set 
            { 
                if (value != _selectedDate)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    OnSelectDateChanged();
                }
            }
        }

        public string LastLoadRatesDate
        {
            get { return _lastLoadRatesDate; }
            set
            {
                if (value != _lastLoadRatesDate)
                {
                    _lastLoadRatesDate = value;
                }
            }
        }

        #endregion

        #region Methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSelectDateChanged()
        {
            Task.Run(async () =>
            {
                if (_selectedDate.HasValue)
                {
                    var selectedDateRates = await CurrencyRatesProcessor.LoadRates(_selectedDate.Value);
                    Rates = new ObservableCollection<RateModel>(selectedDateRates);
                    LastLoadRatesDate = CurrencyRatesProcessor.LastCorrectResponseDate;
                    DatePickerComments = $"Data from {LastLoadRatesDate} has been successfully loaded. If date is changed, that means that the data for the selected one was not published.";
                }
            });  
        }
        #endregion

    }
}
