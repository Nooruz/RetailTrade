using Microsoft.Extensions.Hosting;

namespace RetailTrade.POS.HostBuilders
{
    public static class AddMenuViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddMenuViewModels(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {

            });
        }
    }
}
