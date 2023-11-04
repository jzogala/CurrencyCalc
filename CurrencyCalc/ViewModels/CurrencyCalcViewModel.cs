using CurrencyCalc.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CurrencyCalc.ViewModels
{
    public class CurrencyCalcViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<RateModel> rates = new ObservableCollection<RateModel>();
        public ObservableCollection<RateModel> Rates
        {
            get { return rates; }
            set
            {
                if (value != rates)
                {
                    rates = value;
                    OnPropertyChanged(nameof(Rates));
                }
            }
        }

        private RateModel _selectedCurrency = new RateModel();

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

        private string datePickerComments = "Proszę wybrać datę";

        public string DatePickerComments
        {
            get { return datePickerComments; }
            set 
            {
                if (datePickerComments != value)
                {
                    datePickerComments = value;
                    OnPropertyChanged(nameof(DatePickerComments));
                }
            }
        }


        public CurrencyCalcViewModel() 
        { 
            //Rates = new ObservableCollection<RateModel>();
        }
    }
}
