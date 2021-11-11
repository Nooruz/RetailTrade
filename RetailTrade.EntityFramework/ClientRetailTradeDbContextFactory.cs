using Microsoft.EntityFrameworkCore;
using System;

namespace RetailTrade.EntityFramework
{
    public class ClientRetailTradeDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public ClientRetailTradeDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public ClientRetailTradeDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<ClientRetailTradeDbContext> options = new DbContextOptionsBuilder<ClientRetailTradeDbContext>();

            _configureDbContext(options);

            return new ClientRetailTradeDbContext(options.Options);
        }
    }
}
