using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;
using RetailTradeServer.HostBuilders;
using RetailTradeServer.ViewModels;
using SalePageServer.Views.Dialogs;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
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

        //потом удалить
        private RetailTradeDbContext _dbContext;

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
            startingWindow = new()
            {
                WindowStyle = WindowStyle.None,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new StartingView()
            };
            startingWindow.Show();
            //SplashScreen splashScreen = new("SplashScreen.png");
            //splashScreen.Show(true);
            var contextFactory = _host.Services.GetRequiredService<RetailTradeDbContextFactory>();
            _dbContext = contextFactory.CreateDbContext();            
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
                        //_ = _dialogService.ShowMessage(_sqlException.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                        Current.Shutdown();
                        return;
                    }
                }
                catch (Exception exception)
                {
                    //_ = _dialogService.ShowMessage(exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    Current.Shutdown();
                    return;
                }

                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
                window.Loaded += Window_Loaded;
                //await Task.Delay(5000);
                PotomUdalit();
                window.Show();
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
            startingWindow.Close();
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

        private async void PotomUdalit()
        {
            foreach (var item in await _dbContext.Products
                    .Where(p => p.ProductCategoryId == 0)
                    .Include(p => p.ProductSubcategory)
                    .ToListAsync())
            {
                item.ProductCategoryId = item.ProductSubcategory.ProductCategoryId;
                _dbContext.Products.Update(item);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
