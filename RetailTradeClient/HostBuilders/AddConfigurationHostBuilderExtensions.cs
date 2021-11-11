using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace RetailTradeClient.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder host)
        {
            return host.ConfigureAppConfiguration(c =>
            {
                c.AddUserSecrets<App>();
                //c.AddJsonFile("appsettings.json");
                c.AddEnvironmentVariables();
            });
        }
    }
}
