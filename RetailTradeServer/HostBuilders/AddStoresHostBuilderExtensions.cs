using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;
using SalePageServer.State.Dialogs;

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
                services.AddSingleton<IDialogService, DialogService>();
                services.AddSingleton<IMenuNavigator, MenuNavigator>();                
                services.AddSingleton<IMessageStore, MessageStore>();
                services.AddSingleton<IZebraBarcodeScanner, ZebraBarcodeScanner>();
                services.AddSingleton<IComBarcodeService, ComBarcodeService>();
            });
        }
    }
}
