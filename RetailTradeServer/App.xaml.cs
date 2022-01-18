﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;
using RetailTradeServer.HostBuilders;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.ViewModels;
using RetailTradeServer.ViewModels.Dialogs;
using SalePageServer.Properties;
using SalePageServer.State.Dialogs;
using System;
using System.Data.SqlClient;
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
        private Window startingWindow;
        private IZebraBarcodeScanner _zebraBarcodeScanner;
        private IComBarcodeService _comBarcodeService;
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

            _zebraBarcodeScanner = _host.Services.GetRequiredService<IZebraBarcodeScanner>();
            _comBarcodeService = _host.Services.GetRequiredService<IComBarcodeService>();
            _dialogService = _host.Services.GetRequiredService<IDialogService>();

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
                using var context = contextFactory.CreateDbContext();

                if (CheckConnectionString(context.Database.GetConnectionString()))
                {
                    await context.Database.MigrateAsync();

                    Window window = _host.Services.GetRequiredService<MainWindow>();
                    window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
                    window.Loaded += Window_Loaded;
                    await Task.Delay(5000);
                    window.Show();
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
                _dialogService.ShowMessage(exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
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
                _zebraBarcodeScanner.Close();
                _comBarcodeService.Close();
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

        private static async Task<bool> CreateDataBase(string connectionString)
        {
            using SqlConnection connection = new(connectionString.Replace("Database=RetailTradeDb;", string.Empty));
            string str = "CREATE DATABASE RetailTradeDb";
            SqlCommand command = new(str, connection);
            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (SqlException sqlException)
            {
                _sqlException = sqlException;
                return false;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
