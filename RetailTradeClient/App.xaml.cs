using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;
using RetailTradeClient.HostBuilders;
using RetailTradeClient.ViewModels;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace RetailTradeClient
{

    public static class OkaPF
    {
        [DllImport("drvasOkaMF_KZ.dll", EntryPoint = "Sale")]
        public static extern void Sale(
            [MarshalAs(UnmanagedType.BStr)]
            out string err);
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private Members

        private readonly IHost _host;

        #endregion

        #region Constructor

        public App()
        {
            //string err;

            //OkaPF.Sale(out err);
            _host = CreateHostBuilder().Build();
        }

        #endregion

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
            .AddConfiguration()
            .AddDbContext()
            .AddServices()
            .AddStores()
            .AddViewModels();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var contextFactory = _host.Services.GetRequiredService<RetailTradeDbContextFactory>();
            var clientContextFactory = _host.Services.GetRequiredService<ClientRetailTradeDbContextFactory>();

            try
            {
                using (var context = clientContextFactory.CreateDbContext())
                {
                    context.Database.Migrate();
                }

                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
                window.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
