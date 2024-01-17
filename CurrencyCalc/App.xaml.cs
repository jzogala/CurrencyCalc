using CurrencyCalc.Api;
using CurrencyCalc.Commands;
using CurrencyCalc.Models;
using CurrencyCalc.Services;
using CurrencyCalc.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace CurrencyCalc
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<IHttpClientService, HttpClientService>();
                    services.AddHttpClient("ApiHttpClient", client =>
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);
                        client.BaseAddress = new Uri("https://api.nbp.pl/api/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    });

                    services.AddTransient<CurrencyCalcViewModel>();
                    services.AddTransient<ICurrencyRatesProcessor, CurrencyRatesProcessor>();
                    services.AddTransient<IRateModel, RateModel>();
                    services.AddTransient<IInternetChecker, InternetChecker>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
