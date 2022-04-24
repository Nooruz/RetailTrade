using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;
using RetailTradeServer.Properties;

namespace RetailTradeServer.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            return host.ConfigureServices((context, services) =>
            {
                //static void ConfigureDbContext(DbContextOptionsBuilder o) => o.UseSqlServer(Settings.Default.DefaultConnection);
                void ConfigureDbContext(DbContextOptionsBuilder o) => o.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"));
                _ = services.AddDbContext<RetailTradeDbContext>(ConfigureDbContext);
                _ = services.AddSingleton(new RetailTradeDbContextFactory(ConfigureDbContext));
            });
        }
    }
}
