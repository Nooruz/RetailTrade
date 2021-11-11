using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace RetailTradeServer.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder host)
        {
            return host.ConfigureAppConfiguration(c =>
            {
                c.AddUserSecrets<App>();
                c.AddEnvironmentVariables();
            });
        }
    }
}
