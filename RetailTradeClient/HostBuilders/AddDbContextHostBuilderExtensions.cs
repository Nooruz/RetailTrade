using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.EntityFramework;

namespace RetailTradeClient.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            return host.ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                void ConfigureDbContext(DbContextOptionsBuilder o) => o.UseSqlServer(connectionString);

                _ = services.AddDbContext<RetailTradeDbContext>(ConfigureDbContext);
                _ = services.AddSingleton(new RetailTradeDbContextFactory(ConfigureDbContext));

                string clientConnectionString = context.Configuration.GetConnectionString("ClientConnection");
                void ConfigureClientDbContext(DbContextOptionsBuilder o) => o.UseSqlite(clientConnectionString);

                _ = services.AddDbContext<ClientRetailTradeDbContext>(ConfigureClientDbContext);
                _ = services.AddSingleton(new ClientRetailTradeDbContextFactory(ConfigureClientDbContext));
            });
        }
    }
}
