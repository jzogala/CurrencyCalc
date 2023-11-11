using CurrencyCalc.Api;
using CurrencyCalc.Commands;
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
using System.Windows.Input;
using System.Windows.Navigation;

namespace CurrencyCalc.ViewModels
{
    public class CurrencyCalcViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<RateModel> _rates = new ObservableCollection<RateModel>();
        private string _baseCurrencyAmountText;
        private decimal _baseCurrencyAmount;
        private RateModel _selectedBaseCurrency = new RateModel();
        private RateModel _selectedTargetCurrency = new RateModel();
        private string _datePickerComments = "Proszę wybrać datę";
        private DateTime? _selectedDate;
        private string _lastLoadRatesDate = DateTime.Now.ToString("yyyy-MM-dd");

        public CurrencyCalcViewModel()
        {
            CalculateRatesCommand = new RelayCommand(ExecuteCalculateRatesCommand, CanExecuteCalculateRatesCommand);
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

        public string BaseCurrencyAmountText
        {
            get { return _baseCurrencyAmountText; }
            set
            {
                if (value != _baseCurrencyAmountText)
                {
                    _baseCurrencyAmountText = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal BaseCurrencyAmount
        {
            get { return _baseCurrencyAmount; }
            set
            {
                if (value != _baseCurrencyAmount)
                {
                    _baseCurrencyAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public RateModel SelectedBaseCurrency
        {
            get { return _selectedBaseCurrency; }
            set 
            { 
                if (value != _selectedBaseCurrency)
                {
                    _selectedBaseCurrency = value;
                    OnPropertyChanged();
                }
            }
        }

        public RateModel SelectedTargetCurrency
        {
            get { return _selectedTargetCurrency; }
            set
            {
                if (value != _selectedTargetCurrency)
                {
                    _selectedTargetCurrency = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand CalculateRatesCommand { get; private set; }
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
                    if (selectedDateRates.Count == 0)
                    {
                        DatePickerComments = $"There was a problem with downloading data on {SelectedDate.Value.ToString("dd.MM.yyyy")}. Try to chceck your internet connection.";
                    }
                    else if (SelectedDate.Value.ToString("dd.MM.yyyy") != LastLoadRatesDate)
                    {
                        DatePickerComments = $"No available data on {SelectedDate.Value.ToString("dd.MM.yyyy")}. Currency rates valid for the selected date where published on {LastLoadRatesDate}.";
                    }
                    else
                    {
                        DatePickerComments = $"Data published on {LastLoadRatesDate} has been successfully loaded.";
                    }
                    selectedDateRates.Clear();
                }
            });  
        }

        private bool CanExecuteCalculateRatesCommand(object parameter)
        {
            // Sprawdzenie, czy ComboBoxy mają wybrane elementy
            bool areComboBoxesSelected = SelectedBaseCurrency != null && SelectedTargetCurrency != null;

            // Sprawdzenie, czy TextBox zawiera prawidłową wartość liczbową
            bool isTextBoxValid = !string.IsNullOrWhiteSpace(BaseCurrencyAmountText) && decimal.TryParse(BaseCurrencyAmountText, out decimal amount) && amount > 0;

            return areComboBoxesSelected && isTextBoxValid; // Warunki, kiedy polecenie jest aktywne
        }

        private void ExecuteCalculateRatesCommand(object parameter)
        {
            var doelwaWartosc = _selectedBaseCurrency *
        }

        #endregion

    }
}
