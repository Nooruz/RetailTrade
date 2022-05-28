using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RetailTrade.EntityFramework;
using RetailTrade.SQLServerConnectionDialog;
using RetailTradeServer.HostBuilders;
using RetailTradeServer.ViewModels;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Properties;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Linq;

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

            UpdateDataBase(contextFactory);

            base.OnStartup(e);
        }

        private static async void UpdateDataBase(RetailTradeDbContextFactory contextFactory)
        {
            try
            {
                var context = contextFactory.CreateDbContext();
                TypeWareHouse typeWareHouse = await context.TypeWareHouses.FirstOrDefaultAsync(t => t.Name == "Оптовый склад");
                if (typeWareHouse != null)
                {
                    typeWareHouse.Name = "Склад";
                    context.TypeWareHouses.Update(typeWareHouse);
                    _ = await context.SaveChangesAsync();
                }

                IEnumerable<User> users = await context.Users.Where(u => u.RoleId == 2 && u.WareHouseId == null).ToListAsync();

                if (users != null && users.Any())
                {
                    foreach (User user in users)
                    {
                        if (user.WareHouseId == null)
                        {
                            user.WareHouseId = 2;
                            context.Users.Update(user);
                            _ = await context.SaveChangesAsync();
                        }
                    }
                }

                IEnumerable<Product> products = await context.Products
                    .Include(p => p.WareHouses)
                    .Where(p => p.DeleteMark == false)
                    .ToListAsync();
                WareHouse wareHouse = await context.WareHouses.FirstOrDefaultAsync(w => w.Id == 2);

                if (wareHouse != null && products != null && products.Any())
                {
                    foreach (Product product in products)
                    {
                        if (!product.WareHouses.Any())
                        {
                            if (product.WareHouses.FirstOrDefault() == null)
                            {
                                product.WareHouses.Add(wareHouse);
                                context.Products.Update(product);
                                _ = await context.SaveChangesAsync();
                                ProductWareHouse productWareHouse = await context.ProductsWareHouses.FirstOrDefaultAsync(p => p.ProductId == product.Id && p.WareHouseId == wareHouse.Id);
                                if (productWareHouse != null)
                                {
                                    productWareHouse.Quantity = product.Quantity;
                                    productWareHouse.ArrivalPrice = product.ArrivalPrice;
                                    productWareHouse.SalePrice = product.SalePrice;
                                    context.ProductsWareHouses.Update(productWareHouse);
                                    product.Quantity = 0;
                                    product.ArrivalPrice = 0;
                                    product.SalePrice = 0;
                                    context.Products.Update(product);
                                    _ = await context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                //ignore
            }
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
