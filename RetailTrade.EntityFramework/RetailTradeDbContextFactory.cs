using Microsoft.EntityFrameworkCore;
using System;

namespace RetailTrade.EntityFramework
{
    public class RetailTradeDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;
        
        public RetailTradeDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public RetailTradeDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<RetailTradeDbContext> options = new DbContextOptionsBuilder<RetailTradeDbContext>();

            _configureDbContext(options);

            return new RetailTradeDbContext(options.Options);
        }
    }
}
