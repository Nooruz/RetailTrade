﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;
using RetailTradeServer.HostBuilders;
using RetailTradeServer.Properties;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Data.SqlClient;
using System.Globalization;
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
        private IUIManager _manager;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            _manager = _host.Services.GetRequiredService<IUIManager>();
            var contextFactory = _host.Services.GetRequiredService<RetailTradeDbContextFactory>();

            //Settings.Default.IsFirstLaunch = true;
            //Settings.Default.Save();

            if (Settings.Default.IsFirstLaunch)
            {
                _ = _manager.ShowDialog(new AddConnectionDialogFormModel(_manager, contextFactory) { Title = "Добавить подключение" },
                    new AddConnectionDialogForm());
            }

            if (Settings.Default.IsDataBaseConnectionAdded)
            {

                using (var context = contextFactory.CreateDbContext())
                {
                    if (CheckConnectionString(context.Database.GetConnectionString()))
                    {
                        context.Database.Migrate();
                    }
                    else
                    {
                        _ = _manager.ShowMessage("Не удалось подключиться к базе данных.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        Current.Shutdown();
                        return;
                    }
                }

                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
                window.Show();
            }
            else
            {
                _ = _manager.ShowMessage("Ошибка. Обратитесь к программистам.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CultureInfo newCulture = new("ru-RU");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }

        private bool CheckConnectionString(string connectionString)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
