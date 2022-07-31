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

                if (!await context.WareHouses.AnyAsync())
                {
                    await context.WareHouses.AddRangeAsync(new WareHouse
                    {
                        Name = "Основной склад",
                        Address = "",
                        TypeWareHouseId = 1
                    });
                    await context.SaveChangesAsync();
                }

                WareHouse wareHouse = await context.WareHouses.FirstOrDefaultAsync(w => w.Id == 1);

                IEnumerable<User> users = await context.Users.Where(u => u.RoleId == 2).ToListAsync();

                if (users != null && users.Any())
                {
                    foreach (User user in users)
                    {
                        
                    }
                }

                IEnumerable<Product> products = await context.Products
                    .Include(p => p.WareHouses)
                    .Where(p => p.DeleteMark == false)
                    .ToListAsync();

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
                            }
                        }
                    }
                }

                products = await context.Products.Where(p => p.DeleteMark == false && !string.IsNullOrEmpty(p.Barcode)).ToListAsync();

                if (products != null && products.Any())
                {
                    foreach (var item in products)
                    {
                        await context.ProductBarcode.AddAsync(new()
                        {
                            Barcode = item.Barcode,
                            ProductId = item.Id
                        });

                        item.Barcode = string.Empty;
                    }

                    context.Products.UpdateRange(products);

                    _ = await context.SaveChangesAsync();
                }

                List<Arrival> arrivals = await context.Arrivals.Where(a => a.WareHouseId == null).ToListAsync();

                if (arrivals != null && arrivals.Any())
                {
                    arrivals.ForEach(a =>
                    {
                        a.WareHouseId = 1;
                    });
                    context.UpdateRange(arrivals);

                    _ = await context.SaveChangesAsync();
                }

                MessageBox.Show("Обновление данных успешно выполнено!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
