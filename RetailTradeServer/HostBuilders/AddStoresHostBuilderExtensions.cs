using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Dialogs;
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
                services.AddSingleton<INavigator, Navigator>();
                services.AddSingleton<IAuthenticator, Authenticator>();
                services.AddSingleton<IUserStore, UserStore>();
                services.AddSingleton<IUIManager, UIManager>();
                services.AddSingleton<IMenuNavigator, MenuNavigator>();
                services.AddSingleton<IMessageStore, MessageStore>();
            });
        }
    }
}
