using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Barcode.Services;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;

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
            });
        }
    }
}
