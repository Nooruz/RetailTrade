using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Barcode.Services;
using RetailTrade.POS.State.Shifts;
using RetailTrade.POS.States.Authenticators;
using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.States.Users;

namespace RetailTrade.POS.HostBuilders
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
                _ = services.AddSingleton<IBarcodeService, BarcodeService>();
                _ = services.AddSingleton<IShiftStore, ShiftStore>();
            });
        }
    }
}
