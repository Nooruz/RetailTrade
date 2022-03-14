using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RetailTrade.EntityFramework;
using RetailTrade.SQLServerConnectionDialog;
using RetailTradeServer.HostBuilders;
using RetailTradeServer.ViewModels;
using RetailTradeServer.ViewModels.Dialogs;
using SalePageServer.Properties;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace RetailTradeServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private Members

        private readonly IHost _host;
        private Window startingWindow;
        private static SqlException _sqlException;

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
                .AddViewModels()
                .AddMenuViewModels();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            //Settings.Default.AdminCreated = false;
            //Settings.Default.DefaultConnection = "Server=.ds;Database=RetailTradeDb;Trusted_Connection=True;";
            //Settings.Default.Save();

            startingWindow = new()
            {
                WindowStyle = WindowStyle.None,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new StartingViewModel()
            };
            startingWindow.Show();

            var contextFactory = _host.Services.GetRequiredService<RetailTradeDbContextFactory>();

            try
            {
                //await Task.Delay(5000);
                using var context = contextFactory.CreateDbContext();

                if (!CheckConnectionString(context.Database.GetConnectionString()))
                {                    
                    startingWindow.Hide();
                    if (ConnectionDialog.Show() == MessageBoxResult.OK)
                    {
                        Settings.Default.DefaultConnection = ConnectionDialog.ConnectionString;
                        Settings.Default.Save();
                        _ = System.Diagnostics.Process.Start(ResourceAssembly.Location);
                        string appSettings = File.ReadAllText("appsettings.json");
                        if (!string.IsNullOrEmpty(appSettings))
                        {
                            dynamic jsonObj = JsonConvert.DeserializeObject(appSettings);
                            jsonObj["ConnectionStrings"]["DefaultConnection"] = ConnectionDialog.ConnectionString;
                            string outAppSettings = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                            File.WriteAllText("appsettings.json", outAppSettings);
                        }
                        Current.Shutdown();
                    }
                    else
                    {
                        Current.Shutdown();
                    }
                }
                await context.Database.MigrateAsync();
                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
                window.Loaded += Window_Loaded;
                window.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            startingWindow.Close();
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

        private static bool CheckConnectionString(string connectionString)
        {
            using SqlConnection connection = new(connectionString);
            try
            {                
                connection.Open();
                return true;
            }
            catch (SqlException sqlException)
            {
                _sqlException = sqlException;
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
