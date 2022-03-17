using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Barcode.Services;
using RetailTradeClient.Report;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.ProductSale;
using RetailTradeClient.State.Reports;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using System;

namespace RetailTradeClient.HostBuilders
{
    public static class AddStoresHostBuilderExtensions
    {
        public static IHostBuilder AddStores(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                _ = services.AddSingleton<INavigator, Navigator>();
                _ = services.AddSingleton<IAuthenticator, Authenticator>();
                _ = services.AddSingleton<IUserStore, UserStore>();
                _ = services.AddSingleton<IMessageStore, MessageStore>();
                _ = services.AddSingleton<IShiftStore, ShiftStore>();
                _ = services.AddSingleton<IBarcodeService, BarcodeService>();
                _ = services.AddSingleton(CreateXReport);
                _ = services.AddSingleton(CreateProductSaleReport);
                _ = services.AddSingleton<IReportService, ReportService>();
                _ = services.AddSingleton<IProductSaleStore, ProductSaleStore>();
            });
        }

        private static XReport CreateXReport(IServiceProvider serviceProvider)
        {
            return new XReport(serviceProvider.GetRequiredService<IUserStore>(),
                serviceProvider.GetRequiredService<IShiftStore>());
        }

        private static ProductSaleReport CreateProductSaleReport(IServiceProvider serviceProvider)
        {
            return new ProductSaleReport(serviceProvider.GetRequiredService<IUserStore>());
        }
    }
}
