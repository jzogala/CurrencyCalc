﻿using CurrencyCalc.Api;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        // Called when the MainWindow is closing. Ensures that the HttpClient resources are properly disposed.
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ApiHelper.DisposeClient();
        }
    }
}
