using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;

namespace RetailTrade.POS.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            return host.ConfigureServices((context, services) =>
            {
                void ConfigureDbContext(DbContextOptionsBuilder o) => o.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"));
                _ = services.AddDbContext<RetailTradeDbContext>(ConfigureDbContext);
                _ = services.AddSingleton(new RetailTradeDbContextFactory(ConfigureDbContext));
            });
        }
    }
}
