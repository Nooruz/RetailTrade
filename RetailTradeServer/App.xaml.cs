using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;
using RetailTradeServer.HostBuilders;
using RetailTradeServer.ViewModels;
using SalePageServer.State.Dialogs;
using SalePageServer.Views.Dialogs;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
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
        private IDialogService _dialogService;
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
            //_dialogService = _host.Services.GetRequiredService<IDialogService>();
            //await _dialogService.Show(new StartingView());
            SplashScreen splashScreen = new("SplashScreen.png");
            splashScreen.Show(true);
            var contextFactory = _host.Services.GetRequiredService<RetailTradeDbContextFactory>();

            //Settings.Default.AdminCreated = false;
            //Settings.Default.Save();

            //if (Settings.Default.IsFirstLaunch)
            //{
            //    _ = _dialogService.ShowDialog(new AddConnectionDialogFormModel(_dialogService, contextFactory) { Title = "Добавить подключение" },
            //        new AddConnectionDialogForm());
            //}

            //if (Settings.Default.IsDataBaseConnectionAdded)
            //{
                try
                {
                    using var context = contextFactory.CreateDbContext();
                    if (CheckConnectionString(context.Database.GetConnectionString()))
                    {
                        context.Database.Migrate();
                }
                    else
                    {
                        _ = _dialogService.ShowMessage(_sqlException.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                        Current.Shutdown();
                        return;
                    }
                }
                catch (Exception exception)
                {
                    _ = _dialogService.ShowMessage(exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    Current.Shutdown();
                    return;
                }

                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.DataContext = _host.Services.GetRequiredService<MainViewModel>();

                //await Task.Delay(100);

                window.Show();
                window.Loaded += Window_Loaded;
            //}
            //else
            //{
            //    _ = _dialogService.ShowMessage("Ошибка. Обратитесь к программистам.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            CultureInfo newCulture = new("ru-RU");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            base.OnStartup(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //_dialogService.Close();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

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
