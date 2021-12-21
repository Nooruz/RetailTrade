using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.ProductSale;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;

namespace RetailTradeClient.HostBuilders
{
    public static class AddStoresHostBuilderExtensions
    {
        public static IHostBuilder AddStores(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<INavigator, Navigator>();
                services.AddSingleton<IAuthenticator, Authenticator>();
                services.AddSingleton<IUserStore, UserStore>();
                services.AddSingleton<IUIManager, UIManager>();
                services.AddSingleton<IMessageStore, MessageStore>();
                services.AddSingleton<IReceiptService, ReceiptService>();
                services.AddSingleton<IShiftStore, ShiftStore>();
                services.AddSingleton<IProductSaleStore, ProductSaleStore>();
            });
        }
    }
}
