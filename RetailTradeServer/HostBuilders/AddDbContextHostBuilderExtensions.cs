using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;
using SalePageServer.Properties;

namespace RetailTradeServer.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            return host.ConfigureServices((context, services) =>
            {
                static void ConfigureDbContext(DbContextOptionsBuilder o) => o.UseSqlServer(Settings.Default.DefaultConnection);
                _ = services.AddDbContext<RetailTradeDbContext>(ConfigureDbContext);
                _ = services.AddSingleton(new RetailTradeDbContextFactory(ConfigureDbContext));
            });
        }
    }
}
