using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTradeClient.HostBuilders;
using RetailTradeClient.ViewModels;
using System;
using System.Windows;
using System.Xml.Serialization;

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
            .AddReports()
            .AddViewModels();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

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

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            try
            {

            }
            catch (Exception)
            {
                //ignore
            }
            base.OnExit(e);
        }

    }

    [XmlRoot("arg-xml"), XmlType("arg-xml")]
    public class ScanData
    {
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public Guid Guid { get; set; }
        public int DataType { get; set; }
        public byte[] DataLabel { get; set; }
    }

}
