# CurrencyCalc

## Overview

CurrencyCalc is a WPF application designed for easy and real-time currency conversion. 
Leveraging the latest exchange rates from the National Bank of Poland (NBP) API, it offers a user-friendly interface for converting amounts between various currencies.

![CurrencyCalc Usage](assets/DLAN.gif)

## Features

- **Real-Time Currency Rates**: Fetches the latest currency rates from the NBP API.
- **Currency Conversion**: Allows users to convert amounts between selected currencies.
- **Historical Rates Lookup**: Users can select a date to view and use historical exchange rates.

## NBP API Overview

The NBP API provides real-time exchange rate data published by the National Bank of Poland. It offers:

- Current and historical foreign exchange rates.
- Data available in JSON and XML formats.
- Free access without authentication requirements.

For more information, visit [NBP API Documentation](http://api.nbp.pl/).

## Getting Started

To get started with CurrencyCalc, clone the repository and open the solution file in Visual Studio. Ensure you have .NET Framework installed on your system.

```bash
git clone https://your-repository-url-here
```

## Usage

1. **Select Currencies**: Choose your base and target currencies from the dropdown menus.
2. **Enter Amount**: Input the amount you wish to convert.
3. **View Conversion**: The converted amount is displayed in real-time as you type.

## Documentation

- `CurrencyCalcViewModel`: Manages the application's main functionalities including fetching and displaying currency rates, handling user input, and performing currency conversions.
- `CurrencyRatesProcessor`: Responsible for communicating with the NBP API to fetch real-time and historical currency rates.
- `RelayCommand`: Implements ICommand for handling button click events in the UI.

## Contributions

Contributions to CurrencyCalc are welcome. Please read our contribution guidelines before submitting a pull request.

## License

This project is licensed under the [MIT License](LICENSE).