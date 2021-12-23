using CoreScanner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTradeClient.HostBuilders;
using RetailTradeClient.ViewModels;
using System;
using System.Windows;

namespace RetailTradeClient
{
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

            CCoreScannerClass _scanner = new();

            short[] scannerTypes = new short[1];
            scannerTypes[0] = 1;
            short numberOfScannerTypes = 1;
            int status;

            _scanner.Open(0, scannerTypes, numberOfScannerTypes, out status);

            _scanner.BarcodeEvent += new _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEvent);

            // Let's subscribe for events
            int opcode = 1001; // Method for Subscribe events
            string outXML; // XML Output
            string inXML = "<inArgs>" +
            "<cmdArgs>" +
            "<arg-int>6</arg-int>" + // Number of events you want to subscribe
            "<arg-int>1,2,4,8,16,32</arg-int>" + // Comma separated event IDs
            "</cmdArgs>" +
            "</inArgs>";
            _scanner.ExecCommand(opcode, ref inXML, out outXML, out status);

            try
            {                
                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
                window.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
