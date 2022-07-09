using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;
using RetailTrade.POS.HostBuilders;
using RetailTrade.POS.ViewModels;
using RetailTrade.POS.Properties;
using System;
using System.Windows;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;

namespace RetailTrade.POS
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
                .AddViewModels()
                .AddMenuViewModels();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                await _host.StartAsync();

                var contextFactory = _host.Services.GetRequiredService<RetailTradeDbContextFactory>();

                var context = contextFactory.CreateDbContext();

                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
                window.Show();

                PointSale pointSale = await context.PointSales.FirstOrDefaultAsync(p => p.Id == Settings.Default.WareHouseId);

                if (pointSale != null)
                {
                    Settings.Default.WareHouseId = pointSale.WareHouseId;
                    Settings.Default.Save();
                }
            }
            catch (Exception)
            {

                throw;
            }
            base.OnStartup(e);
        }

    }
}
