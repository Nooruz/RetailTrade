using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Barcode.Services;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Printing;
using RetailTradeServer.State.Reports;
using RetailTradeServer.State.Users;
using SalePageServer.Report;
using System;

namespace RetailTradeServer.HostBuilders
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
                _ = services.AddSingleton<IMenuNavigator, MenuNavigator>();                
                _ = services.AddSingleton<IMessageStore, MessageStore>();
                _ = services.AddSingleton<IBarcodeService, BarcodeService>();
                _ = services.AddSingleton<ILabelPrintingService, LabelPrintingService>();
                _ = services.AddSingleton(CreateXReport);
                _ = services.AddSingleton<IReportService, ReportService>();
            });
        }

        private static LabelReport CreateXReport(IServiceProvider serviceProvider)
        {
            return new LabelReport();
        }
    }
}
