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
    public class CurrencyCalcViewModel : INotifyPropertyChanged
    {
        private IHttpClientService _httpClientService;
        private ICurrencyRatesProcessor _currencyRatesProcessor;

        private ObservableCollection<IRateModel> _rates = new ObservableCollection<IRateModel>();
        private IRateModel _selectedBaseCurrency;
        private IRateModel _selectedTargetCurrency;

        public event PropertyChangedEventHandler? PropertyChanged;
        private string _baseCurrencyAmountText;
        private decimal _baseCurrencyAmount;
        private string _targetCurrencyAmountText;
        private decimal _targetCurrencyAmount;
        private string _datePickerComments = "";
        private DateTime? _selectedDate;
        private string _lastLoadRatesDate = DateTime.Now.ToString("yyyy-MM-dd");

        // Initializes the ViewModel and sets up the CalculateRatesCommand
        public CurrencyCalcViewModel(IHttpClientService httpClientService, ICurrencyRatesProcessor currencyRatesProcessor)
        {
            _httpClientService = httpClientService;
            _currencyRatesProcessor = currencyRatesProcessor;
            CalculateRatesCommand = new RelayCommand(ExecuteCalculateRatesCommand, CanExecuteCalculateRatesCommand);
        }

        #region Properties
        public ObservableCollection<IRateModel> Rates
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
                    OnPropertyChanged(nameof(BaseCurrencyAmountText));
                    if (decimal.TryParse(_baseCurrencyAmountText, out decimal result))
                    {
                        _baseCurrencyAmount = result;
                    }
                }
            }
        }

        public decimal BaseCurrencyAmount
        {
            get { return _baseCurrencyAmount; }
        }

        public string TargetCurrencyAmountText
        {
            get { return _targetCurrencyAmountText; }
            set
            {
                if (value != _targetCurrencyAmountText)
                {
                    _targetCurrencyAmountText = value;
                    OnPropertyChanged(nameof(TargetCurrencyAmountText));
                }
            }
        }

        public decimal TargetCurrencyAmount
        {
            get { return _targetCurrencyAmount; }
            set
            {
                if (value != _targetCurrencyAmount)
                {
                    _targetCurrencyAmount = value;
                    TargetCurrencyAmountText = _targetCurrencyAmount.ToString("F2");
                }
            }
        }

        public IRateModel SelectedBaseCurrency
        {
            get { return _selectedBaseCurrency; }
            set
            {
                if (value != _selectedBaseCurrency)
                {
                    _selectedBaseCurrency = value;
                    OnPropertyChanged(nameof(SelectedBaseCurrency));
                    if (_selectedBaseCurrency != null && _selectedBaseCurrency.Mid != null)
                    {
                        string DisplayedItem = _selectedBaseCurrency.Mid.ToString();
                    }
                }
            }
        }

        public IRateModel SelectedTargetCurrency
        {
            get { return _selectedTargetCurrency; }
            set
            {
                if (value != _selectedTargetCurrency)
                {
                    _selectedTargetCurrency = value;
                    OnPropertyChanged(nameof(SelectedTargetCurrency));
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

        // Handles the logic when the selected date changes, including fetching rates and updating UI messages
        private void OnSelectDateChanged()
        {
            Task.Run(async () =>
            {
                if (_selectedDate.HasValue)
                {
                    ResetView();

                    List<IRateModel> selectedDateRates = await _currencyRatesProcessor.LoadRates(_selectedDate.Value);
                    Rates = new ObservableCollection<IRateModel>(selectedDateRates);
                    LastLoadRatesDate = _currencyRatesProcessor.LastCorrectResponseDate;
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

        // Ressetting the view properites 
        private void ResetView()
        {
            _baseCurrencyAmountText = "";
            _targetCurrencyAmountText = "";
            _selectedBaseCurrency = null;
            _selectedTargetCurrency = null;
            _datePickerComments = "";

            OnPropertyChanged(nameof(BaseCurrencyAmountText));
            OnPropertyChanged(nameof(TargetCurrencyAmountText));
            OnPropertyChanged(nameof(SelectedBaseCurrency));
            OnPropertyChanged(nameof(SelectedTargetCurrency));
            OnPropertyChanged(nameof(DatePickerComments));
        }

        private bool CanExecuteCalculateRatesCommand(object parameter)
        {
            // Check if ComboBoxes have selected items
            bool areComboBoxesSelected = SelectedBaseCurrency != null && SelectedTargetCurrency != null && SelectedBaseCurrency.Code != null && SelectedTargetCurrency.Code != null;

            // Check if TextBox contains a valid numerical value
            bool isTextBoxValid = !string.IsNullOrWhiteSpace(BaseCurrencyAmountText) && decimal.TryParse(BaseCurrencyAmountText, out decimal amount) && amount > 0;

            return areComboBoxesSelected && isTextBoxValid; // Conditions when the command is active
        }

        private void ExecuteCalculateRatesCommand(object parameter)
        {
            TargetCurrencyAmount = (SelectedBaseCurrency.Mid / SelectedTargetCurrency.Mid) * BaseCurrencyAmount;
        }

        public void OnMainWindowClosing()
        {
            _httpClientService.Dispose();
            _httpClientService = null;
        }

        #endregion
    }
}
